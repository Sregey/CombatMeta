using System;
using UnityEngine;

namespace SaberCombatMeta.Meta
{
    [Serializable]
    public class Combat
    {
        [SerializeField]
        private bool _isVictory;
        [SerializeField]
        private float _duration;
        [SerializeField]
        private float _damageDealt;
        [SerializeField]
        private float _damageTaken;
        [SerializeField]
        private int _enemiesKilled;

        public bool IsVictory => _isVictory;
        public float Duration => _duration;
        public float DamageDealt => _damageDealt;
        public float DamageTaken => _damageTaken;
        public int EnemiesKilled => _enemiesKilled;

        public Combat(bool isVictory, float duration, float damageDealt, float damageTaken, int enemiesKilled)
        {
            _isVictory = isVictory;
            _duration = duration;
            _damageDealt = damageDealt;
            _damageTaken = damageTaken;
            _enemiesKilled = enemiesKilled;
        }
    }
}