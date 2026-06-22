using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class MeleeAttackAbility: Ability, IAsyncAbility
    {
        [SerializeField]
        private MeleeAttackWeapon _weapon;
        
        [SerializeField]
        private Cooldown _cooldown;

        public override void Initialize(AbilityManager manager)
        {
            base.Initialize(manager);
            _weapon.gameObject.SetActive(false);
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
            await _weapon.AttackAsync(token);
        }
    }
}