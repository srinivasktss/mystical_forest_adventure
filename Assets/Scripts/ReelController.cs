using UnityEngine;
using UnityEngine.UI;

namespace MysticalForestAdventure
{
	public class ReelController : MonoBehaviour
    {
		[SerializeField] private ReelDataScriptableObject _reelData;
		[SerializeField] private SymbolData[] _currentReelSymbolData;
		[SerializeField] private Image[] _symbolImages;

		private void Awake()
		{
			InitializeCurrentReel();
		}

		private void Update()
		{
            if (Input.GetKeyDown(KeyCode.S))
            {
                FillReel();
            }
        }

		private void InitializeCurrentReel()
		{
			_currentReelSymbolData = new SymbolData[_reelData.TotalSlots];
		}

		private SymbolData GetRandomSymbolData()
		{
			int randomSymbolIndex = Random.Range(0, _reelData.SymbolData.Length);

			return _reelData.SymbolData[randomSymbolIndex];
		}

		private void GenerateCurrentReel()
		{
            for (int i = 0; i < _currentReelSymbolData.Length; i++)
            {
				_currentReelSymbolData[i] = GetRandomSymbolData();
			}
        }

		private void UpdateReelUI()
		{
            for (int i = 0; i < _currentReelSymbolData.Length; i++)
            {
				_symbolImages[i].sprite = _currentReelSymbolData[i].SymbolSprite;
            }
        }

		private void FillReel()
		{
			GenerateCurrentReel();
			UpdateReelUI();
		}
	}
}
