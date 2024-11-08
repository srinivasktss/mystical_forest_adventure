using System;
using System.Collections.Generic;
using UnityEngine;

namespace MysticalForestAdventure
{
    public enum Symbol
    {
        LOW1, LOW2, LOW3, LOW4, LOW5,
        HIGH1, HIGH2, HIGH3, HIGH4,
        WILD,
        BONUS,
        SCATTER
    }

    [Serializable]
    public class SymbolData
    {
		[SerializeField] private Symbol _symbol;
		public Symbol Symbol => _symbol;
		[SerializeField] private Sprite _symbolSprite;
		public Sprite SymbolSprite => _symbolSprite;
	}

	[Serializable]
	public class PayLine
	{
		[SerializeField] private int[] _positionIndices;
        public int[] PositionIndices => _positionIndices;
	}
}
