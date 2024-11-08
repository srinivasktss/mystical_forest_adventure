using UnityEngine;

namespace MysticalForestAdventure
{
    public class UserInputHandler : MonoBehaviour
    {

        public void OnBetIncrementButtonClicked()
        {
            GameManager.Instance.IncrmentBet();
        }

        public void OnBetDecrementButtonClicked()
        {
            GameManager.Instance.DecrementBet();
        }

        public void OnSpinButtonClicked()
        {
            GameManager.Instance.CheckAndSpinReel();

		}
    }
}
