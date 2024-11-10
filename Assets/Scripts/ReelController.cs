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
		[SerializeField] private int[] _currentReelBottomIndices;
		[SerializeField] private Ease _spinFirstAnimatinEase = Ease.Linear;
		[SerializeField] private Ease _spinLastAnimatinEase = Ease.OutElastic;

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
			_currentReelBottomIndices = new int[_reelData.ReelCols];
			_currentReelSymbolData = new SymbolData[_reelData.TotalSlots];
		}

		private SymbolData GetRandomSymbolData()
		{
			int randomSymbolIndex = UnityEngine.Random.Range(0, _reelData.SymbolData.Length);

			return _reelData.SymbolData[randomSymbolIndex];
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
		}

		public void SpinReel()
		{
			float totalSymbols = _reelData.TotalSymbols;
			float singleSpriteHeight = 1 / totalSymbols;

			Sequence[] sequences = new Sequence[_reelRawImageList.Length];

			for (int i = 0; i < _reelRawImageList.Length; i++)
			{
				RawImage rawImage = _reelRawImageList[i];
				Rect imageRect = rawImage.uvRect;

				rawImage.uvRect = new Rect(0f, 0f, imageRect.width, imageRect.height);

				float value = 0f;
				int randomBottomIndex = UnityEngine.Random.Range(0, _reelData.TotalSymbols);
				float randomEndValue = 5f + randomBottomIndex * singleSpriteHeight;
				float randomDuration = UnityEngine.Random.Range(4f, 5f);
				_currentReelBottomIndices[i] = randomBottomIndex;

				sequences[i] = DOTween.Sequence();

				sequences[i].Append(
					DOTween.To(() => value, x => value = x, randomEndValue * 0.9f, randomDuration * 0.8f)
						   .SetEase(_spinFirstAnimatinEase)
						   .OnUpdate(() =>
						   {
							   rawImage.uvRect = new Rect(0f, value, imageRect.width, imageRect.height);
						   })
				);

				sequences[i].Append(
					DOTween.To(() => value, x => value = x, randomEndValue, randomDuration * 0.2f)
						   .SetEase(_spinLastAnimatinEase)
						   .OnUpdate(() =>
						   {
							   rawImage.uvRect = new Rect(0f, value, imageRect.width, imageRect.height);
						   })
				);
			}
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
