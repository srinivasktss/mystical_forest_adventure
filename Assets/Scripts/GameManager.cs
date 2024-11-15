using System;
using UnityEngine;

namespace MysticalForestAdventure
{
	[DefaultExecutionOrder(100)]
    public class GameManager : MonoBehaviour
    {
		public static GameManager Instance { get; private set; }

		[SerializeField] private BetData _betData;
		[SerializeField] private float _currentBetAmount;
		[SerializeField] private UserProfileManager _userProfileManager;
		[SerializeField] private ReelController _reelController;
		[SerializeField] private ScoreManager _scoreManager;
		[SerializeField] private GameAudioController _gameAudioController;

		private WinResult _winResult;
		public float CurrentBetAmount => _currentBetAmount;

		public event Action<float> OnBetAmountUpdated, OnAmountUpdated, OnWinAmountUpdated;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
			}
			else
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}

			_winResult = new WinResult();
			_currentBetAmount = _betData.MinBetAmount;
		}

		private void Start()
		{
			_gameAudioController.PlayBGM();
		}

		private void OnDisable()
		{
			_reelController.OnReelSpinCompleted -= OnSpinCompleted;
		}

		public void IncrmentBet()
		{
			if(_userProfileManager.GetAmount() < _currentBetAmount + _betData.BetIncrement)
			{
				Debug.Log($"Insufficient amount");
				return;
			}

			_gameAudioController.PlayGeneralButtonClickSfx();

			_currentBetAmount += _betData.BetIncrement;
			ClampBetAmount();
		}

		public void DecrementBet()
		{
			if (_currentBetAmount - _betData.BetIncrement <= 0f)
			{
				Debug.Log($"Cannot reduce");
				return;
			}

			_gameAudioController.PlayGeneralButtonClickSfx();

			_currentBetAmount -= _betData.BetIncrement;
			ClampBetAmount();
		}

		private void ClampBetAmount()
		{
			if (_currentBetAmount < _betData.MinBetAmount)
				_currentBetAmount = _betData.MinBetAmount;
			else if(_currentBetAmount > _betData.MaxBetAmount)
				_currentBetAmount = _betData.MaxBetAmount;

			OnBetAmountUpdated?.Invoke(_currentBetAmount);
		}

		public void CheckAndSpinReel()
		{
			if(_userProfileManager.GetAmount() < _currentBetAmount)
			{
				Debug.Log("Insufficient amount");
				return;
			}

			_gameAudioController.StopBGM();

			_userProfileManager.UpdateAmount(-_currentBetAmount);
			OnWinAmountUpdated?.Invoke(0f);

			OnAmountUpdated?.Invoke(_userProfileManager.GetAmount());

			_reelController.OnReelSpinCompleted -= OnSpinCompleted;
			_reelController.OnReelSpinCompleted += OnSpinCompleted;

			_reelController.SpinReel();
		}

		private void OnSpinCompleted()
		{
			_reelController.CheckMatchingPayLines(ref _winResult);
			CheckResult();
		}

		public void CheckResult()
		{
			if(_winResult == null)
			{
				Debug.Log($"No result available");
				return;
			}

			_gameAudioController.PlayWinSfx(_winResult);

			float multiplier = _scoreManager.GetMultiplier(_winResult.Symbol, (ConsequitiveCount)_winResult.MaxLength);

			float winAmount = _currentBetAmount * multiplier;

			OnWinAmountUpdated?.Invoke(winAmount);

			_userProfileManager.UpdateAmount(winAmount);
		}
	}
}
