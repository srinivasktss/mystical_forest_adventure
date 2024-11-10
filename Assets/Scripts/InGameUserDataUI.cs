using TMPro;
using UnityEngine;

namespace MysticalForestAdventure
{
	[DefaultExecutionOrder(200)]
	public class InGameUserDataUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountValueText;
        [SerializeField] private TMP_Text _winValueText;
        [SerializeField] private TMP_Text _betValueText;

		private void Start()
		{
            SetUIValues();
		}

		private void OnEnable()
		{
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnBetAmountUpdated += SetBetValueText;
				GameManager.Instance.OnAmountUpdated += SetAmountValueText;
				GameManager.Instance.OnWinAmountUpdated += SetWinValueText;
			}
		}

		private void OnDisable()
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.OnBetAmountUpdated -= SetBetValueText;
				GameManager.Instance.OnAmountUpdated -= SetAmountValueText;
                GameManager.Instance.OnWinAmountUpdated -= SetWinValueText;
			}
		}

        private void SetUIValues()
        {
            UserProfileManager upm = FindFirstObjectByType<UserProfileManager>();
            SetAmountValueText(upm ? upm.GetAmount() : 0f);

            SetBetValueText(GameManager.Instance ? GameManager.Instance.CurrentBetAmount : 0f);

            SetWinValueText(0f);
		}

		public void SetAmountValueText(float value)
        {
            _amountValueText.text = value.ToString("00.00");
        }

        public void SetWinValueText(float value)
        {
            _winValueText.text = value.ToString("00.00");
        }

        public void SetBetValueText(float value)
        {
            _betValueText.text = value.ToString("00.00");
        }
    }
}
