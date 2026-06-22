using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class AoeAttackAbility: Ability, IAsyncAbility, ICooldownAbility
    {
        [SerializeField]
        private GameObject _shockwavePrefab;
        
        [SerializeField]
        private Transform _shockwaveSpawnPoint;
        
        [SerializeField]
        private Cooldown _cooldown;
        
        private AoeAttackShockwave _shockwave;
        
        public Cooldown Cooldown => _cooldown;

        public override void Initialize(AbilityManager manager)
        {
            base.Initialize(manager);
            _shockwave = Instantiate(_shockwavePrefab).GetComponent<AoeAttackShockwave>();
        }

        public override void Dispose()
        {
            Destroy(_shockwave);
            base.Dispose();
        }
        
        protected override bool CanUse()
        {
            return _cooldown.IsFinished && base.CanUse();
        }

        Awaitable IAsyncAbility.UseAsync(object parameters, CancellationToken token)
        {
            _cooldown.Start();
            return AttackAsync(token);
        }
        
        private async Awaitable AttackAsync(CancellationToken token)
        {
            _shockwave.transform.position = _shockwaveSpawnPoint.position;
            await _shockwave.AttackAsync(token);
        }
    }
}