using System;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    [Serializable]
    public struct Cooldown
    {
        [SerializeField, Min(0)]
        private float _cooldownDuration;

        private float _cooldownTimer;
        
        public float Duration => _cooldownDuration;
        public float RemainingTime => Mathf.Max(_cooldownTimer - Time.time, 0);
        public bool IsFinished => Time.time >= _cooldownTimer;

        public void Start()
        {
            _cooldownTimer = Time.time + _cooldownDuration;
        }
    }
}