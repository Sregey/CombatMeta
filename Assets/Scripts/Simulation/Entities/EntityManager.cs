using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace SaberCombatMeta.Simulation
{
    public abstract class EntityManager<TEntity>: MonoBehaviour, IDisposable
        where TEntity: MonoBehaviour
    {
        [SerializeField]
        private TEntity _entityPrefab;

        private readonly List<TEntity> _entities = new();
        
        private DiContainer  _diContainer;

        protected List<TEntity> Entities => _entities;
        public int Count => _entities.Count;
        
        public event Action<TEntity> EntitySpawned;
        public event Action<TEntity> EntityDespawning;
        public event Action<TEntity> EntityDespawned;
        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public async Awaitable<TEntity> SpawnAsync(Transform spawnLocation, CancellationToken token)
        {
            OnEntitySpawning();
            
            spawnLocation.GetPositionAndRotation(out var spawnPosition, out var spawnRotation);
            var parameters = new InstantiateParameters() { worldSpace = true };
            var entities = await InstantiateAsync(_entityPrefab, spawnPosition, spawnRotation, parameters, token);
            var entity = entities[0];
            _diContainer.InjectGameObject(entity.gameObject);
            
            _entities.Add(entity);
            OnEntitySpawned(entity);
            EntitySpawned?.Invoke(entity);

            return entity;
        }

        public void Despawn(TEntity entity)
        {
            OnEntityDespawning(entity);
            EntityDespawning?.Invoke(entity);
            _entities.Remove(entity);
            
            Destroy(entity.gameObject);

            OnEntityDespawned(entity);
            EntityDespawned?.Invoke(entity);
        }

        public void Dispose()
        {
            foreach (var entity in _entities.ToArray())
            {
                Despawn(entity);
            }
        }
        
        protected virtual void OnEntitySpawning()
        {
        }

        protected virtual void OnEntitySpawned(TEntity enemy)
        {
        }
        
        protected virtual void OnEntityDespawning(TEntity entity)
        {
        }

        protected virtual void OnEntityDespawned(TEntity entity)
        {
        }
    }
}