using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MysticalForestAdventure
{
	public class ReelController : MonoBehaviour
    {
		[SerializeField] private ReelDataScriptableObject _reelData;
		[SerializeField] private SymbolData[] _currentReelSymbolData;
		[SerializeField] private Image[] _symbolImages;

		[SerializeField] private int _reelSpriteSpacing = 10;
		[SerializeField] private RawImage[] _reelRawImageList;

		private List<SymbolData>[] _randomGeneratedFullReel;

		private void Awake()
		{
			InitializeCurrentReel();
			GenerateReel();
		}

		private SymbolData GetReelSymbolData(int index)
		{
			if (index < 0 || index >= _reelData.TotalSlots)
				throw new IndexOutOfRangeException();

			return _currentReelSymbolData[index];
		}

		private void InitializeCurrentReel()
		{
			_randomGeneratedFullReel = new List<SymbolData>[_reelData.ReelCols];
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

		public void GenerateReel()
		{
			List<SymbolData> symbolDatas = new List<SymbolData>();

			List<Sprite> randomSymbolsSprites = new List<Sprite>();

			int randomIndex;

			for(int i = 0; i < _reelData.ReelCols; i++)
			{
				symbolDatas.AddRange(_reelData.SymbolData);

				randomSymbolsSprites.Clear();

				_randomGeneratedFullReel[i] ??= new List<SymbolData>();
				_randomGeneratedFullReel[i].Clear();

				while (symbolDatas.Count > 0)
				{
					randomIndex = UnityEngine.Random.Range(0, symbolDatas.Count);
					randomSymbolsSprites.Add(symbolDatas[randomIndex].SymbolSprite);
					_randomGeneratedFullReel[i].Add(symbolDatas[randomIndex]);

					symbolDatas.RemoveAt(randomIndex);
				}

				_reelRawImageList[i].texture = SpriteCombiner.CombineSpritesVerticallyForTexture(randomSymbolsSprites, _reelSpriteSpacing);
			}

			/*GenerateCurrentReel();
			UpdateReelUI();*/
		}

		[SerializeField] private Ease _spinFirstAnimatinEase = Ease.Linear;
		[SerializeField] private Ease _spinLastAnimatinEase = Ease.OutElastic;
		public void SpinReel()
		{
			float totalSymbols = _reelData.TotalSymbols;
			float singleSpriteHeight = 1 / totalSymbols;

			Debug.Log($"totalSymbols: {totalSymbols}, singleSpriteHeight: {singleSpriteHeight}");

			_reelRawImageList[0].uvRect = new Rect(0f, 0f, _reelRawImageList[0].uvRect.width, _reelRawImageList[0].uvRect.height);

			float value = 0f;
			float randomEndValue = 5f + UnityEngine.Random.Range(0, _reelData.TotalSymbols) * singleSpriteHeight;

			Sequence sequence = DOTween.Sequence();

			sequence.Append(
				DOTween.To(() => value, x => value = x, randomEndValue * 0.9f, 5f * 0.8f)
					   .SetEase(_spinFirstAnimatinEase)
					   .OnUpdate(() =>
					   {
						   _reelRawImageList[0].uvRect = new Rect(0f, value, _reelRawImageList[0].uvRect.width, _reelRawImageList[0].uvRect.height);
					   })
			);

			sequence.Append(
				DOTween.To(() => value, x => value = x, randomEndValue, 5f * 0.2f)
					   .SetEase(_spinLastAnimatinEase)
					   .OnUpdate(() =>
					   {
						   _reelRawImageList[0].uvRect = new Rect(0f, value, _reelRawImageList[0].uvRect.width, _reelRawImageList[0].uvRect.height);
					   })
			);
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
