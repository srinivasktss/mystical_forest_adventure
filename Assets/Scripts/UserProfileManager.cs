using UnityEngine;

namespace MysticalForestAdventure
{
    public class UserProfileManager : MonoBehaviour
    {
		private const string k_PlayerNameKey = "UserProfile_Name";
		private const string k_PlayerAmountKey = "UserProfile_Amount";

		[SerializeField] private UserProfile _userProfile;

		private void Awake()
		{
			LoadUserProfile();
		}

		private void LoadUserProfile()
		{
			string name;
			float amount;
			bool newUserCreated = false;
			if (PlayerPrefs.HasKey(k_PlayerNameKey) && PlayerPrefs.HasKey(k_PlayerAmountKey))
			{
				name = PlayerPrefs.GetString(k_PlayerNameKey);
				amount = PlayerPrefs.GetFloat(k_PlayerAmountKey);
			}
			else
			{
				Debug.Log("No saved user profile data found. So, creating new profile data.");
				name = $"USER_{Random.Range(1000, 10000)}";
				amount = 100;

				newUserCreated = true;
			}

			_userProfile ??= new UserProfile();
			_userProfile.SetName(name);
			_userProfile.SetAmount(amount);

			if(newUserCreated)
			{
				SaveUserProfile();
			}
		}

		private void SaveUserProfile()
		{
			PlayerPrefs.SetString(k_PlayerNameKey, _userProfile.Name);
			PlayerPrefs.SetFloat(k_PlayerAmountKey, _userProfile.Amount);
			PlayerPrefs.Save();
		}

		public float GetAmount() => _userProfile.Amount;

		public bool UpdateAmount(float amount)
		{
			if (amount < 0 && _userProfile.Amount + amount < 0)
			{
				Debug.LogWarning("Insufficient funds for deduction.");
				return false;
			}

			_userProfile.SetAmount(_userProfile.Amount + amount);
			SaveUserProfile();

			return true;
		}
	}
}
