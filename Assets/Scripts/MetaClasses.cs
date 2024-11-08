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
        [SerializeField] private float _amount;

        public string Name => _name;

        public float Amount => _amount;

		public void SetName(string name) => _name = name;
        public void SetAmount(float amount) => _amount = amount;
	}

    [Serializable]
    public class BetData
    {
        [SerializeField] private float _minBetAmount;
        public float MinBetAmount => _minBetAmount;
        [SerializeField] private float _maxBetAmount;
        public float MaxBetAmount => _maxBetAmount;
        [SerializeField] private float _betIncrement;
        public float BetIncrement => _betIncrement;
    }

    [Serializable]
    public class WinResult
    {
        public Symbol Symbol;
        public int MaxLength;
        public int PayLineIndex;
    }

    public enum ConsequitiveCount
    {
        Three = 3,
        Four = 4,
        Five = 5
    }

    [Serializable]
    public class ScorePoint
    {
        public Symbol Symbol;
        public ConsequitiveCount ConsequtiveCount;
        public float Multiplier;
    }
}
