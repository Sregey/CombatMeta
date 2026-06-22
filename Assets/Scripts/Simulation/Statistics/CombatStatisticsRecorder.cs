using System;
using SaberCombatMeta.Meta;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.Simulation
{
    public class CombatStatisticsRecorder: MonoBehaviour
    {
        [SerializeField]
        private EnemiesWave _enemiesWave;
        
        private HealthSystem _healthSystem;
        
        private float _startTime = -1;

        private bool _isVictory;
        private float _duration;
        private float _damageDealt;
        private float _damageTaken;
        private int _enemiesKilled;

        public bool IsRecording => _startTime >= 0;

        public event Action<Combat> CombatFinished;
        
        [Inject]
        public void Construct(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            Reset();
        }

        private void Awake()
        {
            _healthSystem.DamageDealt += OnDamageDealt;
            _enemiesWave.EnemiesCountChanged += OnEnemiesCountChanged;
        }
        
        private void OnDestroy()
        {
            _healthSystem.DamageDealt -= OnDamageDealt;
            _enemiesWave.EnemiesCountChanged -= OnEnemiesCountChanged;
        }

        private void OnEnemiesCountChanged()
        {
            if (_enemiesWave.EnemiesRemainingCount == _enemiesWave.EnemiesMaxCount)
            {
                StartRecording();
            }
        }

        private void StartRecording()
        {
            if (IsRecording)
                return;
            
            _startTime = Time.unscaledTime;
        }
        
        private void StopRecording()
        {
            if (!IsRecording)
                return;
            
            _duration = Time.unscaledTime - _startTime;
            
            var combat = new Combat(_isVictory, _duration, _damageDealt, _damageTaken, _enemiesKilled);
            CombatFinished?.Invoke(combat);
        }

        private void Reset()
        {
            _startTime = -1;
            
            _isVictory = false;
            _duration = 0;
            _damageDealt = 0;
            _damageTaken = 0;
            _enemiesKilled = 0;
        }

        private void OnDamageDealt(Health health, float damage)
        {
            if (!IsRecording)
                return;
            
            if (health.Owner as PlayerEntity)
            {
                _damageTaken += damage;
                if (health.IsDead)
                {
                    _isVictory = false;
                    StopRecording();
                }
            }
            else if (health.Owner as EnemyEntity)
            {
                _damageDealt += damage;
                if (health.IsDead)
                {
                    _enemiesKilled++;
                    if (_enemiesWave.EnemiesRemainingCount == 0)
                    {
                        _isVictory = true;
                        StopRecording();
                    }
                }
            }
        }
    }
}