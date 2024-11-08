using System.Collections.Generic;
using UnityEngine;

namespace MysticalForestAdventure
{
    [CreateAssetMenu(fileName = "ReelData", menuName = "Scriptable Objects/ReelData", order = 1)]
    public class ReelDataScriptableObjectScript : ScriptableObject
    {
        [SerializeField] private string _reelName;
        public string ReelName => _reelName;

        [SerializeField] private int _reelRows;
        public int ReelRows => _reelRows;

        [SerializeField] private int _reelCols;
        public int ReelCols => _reelCols;

        [SerializeField] private SymbolData[] _symbolData;
        public SymbolData[] SymbolData => _symbolData;

		[SerializeField] private PayLine[] _payLines;
        public PayLine[] PayLines => _payLines;

		private Dictionary<Symbol, Sprite> _symbolDataMap;

		private void Awake()
		{
            SetUpSymbolDataMap();
		}

        private void SetUpSymbolDataMap()
        {
			_symbolDataMap ??= new Dictionary<Symbol, Sprite>();

			_symbolDataMap.Clear();

            foreach (SymbolData symbolData in _symbolData)
            {
				_symbolDataMap.Add(symbolData.Symbol, symbolData.SymbolSprite);
			}
		}
	}
}
