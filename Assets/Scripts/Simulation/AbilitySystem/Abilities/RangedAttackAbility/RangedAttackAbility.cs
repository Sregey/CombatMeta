using SaberCombatMeta.Common;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class RangedAttackAbility: Ability, ISyncAbility
    {
        [SerializeField]
        private RangedAttackProjectile _projectilePrefab;
        
        [SerializeField]
        private Transform _projectileSpawnPoint;
        
        [SerializeField]
        private float _projectileInitialSpeed;
        
        [SerializeField]
        private int _projectilePoolInitialSize;
        
        [SerializeField]
        private Cooldown _cooldown;
        
        private ObjectPool<RangedAttackProjectile> _projectilePool;

        public override void Initialize(AbilityManager manager)
        {
            base.Initialize(manager);
            _projectilePool = CreatePool();
            return;

            ObjectPool<RangedAttackProjectile> CreatePool()
            {
                return new ObjectPool<RangedAttackProjectile>(
                    (pool) =>
                    {
                        var projectile = Instantiate(_projectilePrefab);
                        projectile.Initialize(pool);
                        return projectile;
                    },
                    (projectile) => projectile.Reset(),
                    (projectile) =>
                    {
                        if (projectile)
                            Destroy(projectile.gameObject);
                    },
                    _projectilePoolInitialSize);
            }
        }

        public override void Dispose()
        {
            _projectilePool.Clear();
            base.Dispose();
        }

        protected override bool CanUse()
        {
            return _cooldown.IsFinished && base.CanUse();
        }

        void ISyncAbility.Use(object parameters)
        {
            _cooldown.Start();
            Attack();
        }

        private void Attack()
        {
            var spawnPosition = _projectileSpawnPoint.position;
            var direction = _projectileSpawnPoint.forward;

            var projectile = _projectilePool.Get();
            projectile.Reset(spawnPosition, direction * _projectileInitialSpeed);
        }
    }
}