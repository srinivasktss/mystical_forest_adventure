using System;
using System.Collections.Generic;
using UnityEngine;

namespace MysticalForestAdventure
{
    public enum Symbol
    {
        NONE = -1,
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
		[Tooltip("Index should be within 0 to rows * cols - 1")]
		[SerializeField] private int[] _positionIndices;
        public int[] PositionIndices => _positionIndices;
	}

    [Serializable]
	public class UserProfile
    {
        [SerializeField] private string _name;
        [SerializeField] private double _amount;

        public string Name => _name;

        public double Amount => _amount;

		public void SetName(string name) => _name = name;
        public void SetAmount(double amount) => _amount = amount;

	}
}
