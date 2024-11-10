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

		private WinResult _winResult;
		public float CurrentBetAmount => _currentBetAmount;

		public event Action<float> OnBetAmountUpdated, OnAmountUpdated;

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

			_currentBetAmount += _betData.BetIncrement;
			ClampBetAmount();
		}

		public void DecrementBet()
		{
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

			_userProfileManager.UpdateAmount(-_currentBetAmount);
			
			OnAmountUpdated?.Invoke(_userProfileManager.GetAmount());

			_reelController.OnReelSpinCompleted -= OnSpinCompleted;
			_reelController.OnReelSpinCompleted += OnSpinCompleted;

			_reelController.SpinReel();
		}

		private void OnSpinCompleted()
		{
			CheckResult();
		}

		public void CheckResult()
		{
			if(_winResult == null)
			{
				Debug.Log($"No result available");
				return;
			}

			float multiplier = _scoreManager.GetMultiplier(_winResult.Symbol, (ConsequitiveCount)_winResult.MaxLength);

			_userProfileManager.UpdateAmount(_currentBetAmount * multiplier);
		}
	}
}
