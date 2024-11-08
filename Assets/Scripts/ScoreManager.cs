using System.Collections.Generic;
using UnityEngine;

namespace MysticalForestAdventure
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private List<ScorePoint> _scorePoints;

        private Dictionary<Symbol, Dictionary<ConsequitiveCount, float>> _scorePointsMap;

		private void Awake()
		{
			InitializeScorePointsMap();
		}

		private void InitializeScorePointsMap()
		{
			_scorePointsMap ??= new Dictionary<Symbol, Dictionary<ConsequitiveCount, float>>();
			_scorePointsMap.Clear();

			foreach (ScorePoint scorePoint in _scorePoints)
			{
				if(!_scorePointsMap.ContainsKey(scorePoint.Symbol))
				{
					_scorePointsMap.Add(scorePoint.Symbol, new Dictionary<ConsequitiveCount, float>());
				}

				if (!_scorePointsMap[scorePoint.Symbol].ContainsKey(scorePoint.ConsequtiveCount))
				{
					_scorePointsMap[scorePoint.Symbol].Add(scorePoint.ConsequtiveCount, scorePoint.Multiplier);
				}

				_scorePointsMap[scorePoint.Symbol][scorePoint.ConsequtiveCount] = scorePoint.Multiplier;
			}
		}

		public float GetMultiplier(Symbol symbol, ConsequitiveCount consequitiveCount)
		{
			float multiplier = 0;

			if(_scorePointsMap.TryGetValue(symbol, out var conseq))
			{
				Debug.Log($"Score for {symbol} found");
			}

			if(conseq != null && conseq.TryGetValue(consequitiveCount, out multiplier))
			{
				Debug.Log($"Multiplier for {consequitiveCount} found: {multiplier}");
			}

			return multiplier;
		}
	}
}
