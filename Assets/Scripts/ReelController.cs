using System;
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
			FillReel();
		}

		private SymbolData GetReelSymbolData(int index)
		{
			if (index < 0 || index >= _reelData.TotalSlots)
				throw new IndexOutOfRangeException();

			return _currentReelSymbolData[index];
		}

		private void InitializeCurrentReel()
		{
			_currentReelSymbolData = new SymbolData[_reelData.TotalSlots];
		}

		private SymbolData GetRandomSymbolData()
		{
			int randomSymbolIndex = UnityEngine.Random.Range(0, _reelData.SymbolData.Length);

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
				_symbolImages[i].sprite = GetReelSymbolData(i).SymbolSprite;
            }
        }

		public void FillReel()
		{
			GenerateCurrentReel();
			UpdateReelUI();
		}

		public void CheckMatchingPayLines(ref WinResult winResult)
		{
			Symbol maxMatchSymbol = Symbol.NONE;
			int maxLength = 0;

			Symbol firstMatchSymbol, currentMatchSymbol;
			int currentLength;

			int maxPayLineIndex = -1;

			PayLine currentPayLine;

            for (int payLineIndex = 0; payLineIndex < _reelData.PayLines.Length; payLineIndex++)
            {
				currentPayLine = _reelData.PayLines[payLineIndex];

				firstMatchSymbol = GetReelSymbolData(currentPayLine.PositionIndices[0]).Symbol;
				currentLength = 1;

                for (int positionIndex = 1; positionIndex < currentPayLine.PositionIndices.Length; positionIndex++)
                {
					currentMatchSymbol = GetReelSymbolData(currentPayLine.PositionIndices[positionIndex]).Symbol;

					if(currentMatchSymbol == firstMatchSymbol || currentMatchSymbol == Symbol.WILD)
					{
						currentLength++;
					}
					else
					{
						break;
					}
				}

				if(currentLength > maxLength || (currentLength == maxLength && firstMatchSymbol > maxMatchSymbol))
				{
					maxMatchSymbol = firstMatchSymbol;
					maxLength = currentLength;
					maxPayLineIndex = payLineIndex;
				}
            }

			Debug.Log($"maxMatchSymbol: {maxMatchSymbol}, maxLength: {maxLength}, maxPayLineIndex: {maxPayLineIndex}");

			winResult.MaxLength = maxLength;
			winResult.Symbol = maxMatchSymbol;
			winResult.PayLineIndex = maxPayLineIndex;
		}
	}
}
