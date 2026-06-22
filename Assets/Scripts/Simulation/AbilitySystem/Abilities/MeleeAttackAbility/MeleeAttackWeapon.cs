using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class MeleeAttackWeapon: MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float _damage;
        
        [SerializeField]
        private List<DamageModifier> _damageModifiers;
        
        [SerializeField, Min(0)]
        private float _attackDuration;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Health>(out var otherHealth))
            {
                otherHealth.TakeDamage(_damage, _damageModifiers);
            }
        }

        public async Awaitable AttackAsync(CancellationToken token)
        {
            gameObject.SetActive(true);
            await Awaitable.WaitForSecondsAsync(_attackDuration, token);
            gameObject.SetActive(false);
        }
    }
}