using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaberCombatMeta.Meta
{
    [Serializable]
    public class Profile
    {
        [SerializeField]
        private float _score;
        [SerializeField]
        private List<Combat> _combats = new();
        
        private float _lastCombatScore;
        
        public float Score => _score;
        public float LastCombatScore => _lastCombatScore;
        
        public IEnumerable<Combat> Combats => _combats;

        public void AddCombat(Combat combat)
        {
            _lastCombatScore = CalculateCombatScore(combat);
            _score += _lastCombatScore;
            _combats.Add(combat);

            if (_combats.Count > 5)
            {
                _combats.RemoveAt(0);
            }
        }

        public static Profile GetDefault()
        {
            return new Profile();
        }

        private float CalculateCombatScore(Combat combat)
        {
            var multiplier = combat.IsVictory ? 1f : 0.5f;
            return combat.EnemiesKilled * 10 * multiplier;
        }
    }
}