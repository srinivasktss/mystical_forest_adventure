using System;
using System.Collections.Generic;
using UnityEngine;

namespace MysticalForestAdventure
{
    [CreateAssetMenu(fileName = "ReelData", menuName = "Scriptable Objects/ReelData", order = 1)]
    public class ReelDataScriptableObject : ScriptableObject
    {
        [SerializeField] private string _reelName;
        public string ReelName => _reelName;

        [SerializeField] private int _reelRows;
        public int ReelRows => _reelRows;

        [SerializeField] private int _reelCols;
        public int ReelCols => _reelCols;

		[SerializeField] private int _totalSlots;
		public int TotalSlots => _totalSlots;

		[SerializeField] private SymbolData[] _symbolData;
        public SymbolData[] SymbolData => _symbolData;

		[SerializeField] private PayLine[] _payLines;
        public PayLine[] PayLines => _payLines;

		private Dictionary<Symbol, Sprite> _symbolDataMap;

		public int TotalSymbols { get; private set; }

		private void OnEnable()
		{
			_totalSlots = _reelRows * _reelCols;
			TotalSymbols = _symbolData.Length;
			InitializeSymbolDataMap();
			ValidatePayLines();
		}

		private void InitializeSymbolDataMap()
        {
			_symbolDataMap ??= new Dictionary<Symbol, Sprite>();

			_symbolDataMap.Clear();

            foreach (SymbolData symbolData in _symbolData)
            {
				_symbolDataMap.Add(symbolData.Symbol, symbolData.SymbolSprite);
			}
		}

		private void ValidatePayLines()
		{
			int maxIndex = _reelRows * _reelCols - 1;

			foreach (PayLine payLine in _payLines)
			{
				foreach (int index in payLine.PositionIndices)
				{
					if (index < 0 || index > maxIndex)
					{
						throw new Exception($"PayLine contains invalid index {index}. Valid range is 0 to {maxIndex}.");
					}
				}
			}
		}
	}
}
