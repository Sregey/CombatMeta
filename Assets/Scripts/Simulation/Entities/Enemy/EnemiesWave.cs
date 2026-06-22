using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.Simulation
{
    public class EnemiesWave: MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spawnLocations;
        
        private EnemyManager _enemyManager;
        
        public int EnemiesMaxCount => _spawnLocations.Length;
        
        public int EnemiesRemainingCount {get; private set;}

        public event Action EnemiesCountChanged;
        
        [Inject]
        private void Construct(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerEntity>(out var targetPlayer))
            {
                EnemiesRemainingCount = _spawnLocations.Length;
                EnemiesCountChanged?.Invoke();
                
                StartWave(targetPlayer, destroyCancellationToken);
                
                gameObject.SetActive(false);
            }
        }

        private async void StartWave(PlayerEntity targetPlayer, CancellationToken token)
        {
            var awaitables = new Awaitable<EnemyEntity>[_spawnLocations.Length];
            for (var i = 0; i < _spawnLocations.Length; i++)
            {
                awaitables[i] = _enemyManager.SpawnAsync(_spawnLocations[i], token);
            }
            
            for (var i = 0; i < awaitables.Length; i++)
            {
                try
                {
                    var enemy = await awaitables[i];
                    enemy.Target = targetPlayer.transform;
                    enemy.Health.HealthChanged += OnEnemyHealthChanged;
                }
                catch (OperationCanceledException)
                {
                    EnemiesRemainingCount--;
                }
            }
        }

        private void OnEnemyHealthChanged(Health health)
        {
            if (health.IsDead)
            {
                EnemiesRemainingCount--;
                EnemiesCountChanged?.Invoke();
                
                health.HealthChanged -= OnEnemyHealthChanged;
                
                var enemy = (EnemyEntity)health.Owner;
                _enemyManager.Despawn(enemy);
            }
        }
    }
}