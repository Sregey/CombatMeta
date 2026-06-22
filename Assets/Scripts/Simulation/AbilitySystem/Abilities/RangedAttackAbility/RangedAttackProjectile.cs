using System.Collections.Generic;
using SaberCombatMeta.Common;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class RangedAttackProjectile: MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField, Min(0)]
        private float _damage;
        
        [SerializeField]
        private List<DamageModifier> _damageModifiers;
        
        private ObjectPool<RangedAttackProjectile> _projectilePool;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<Health>(out var otherHealth))
            {
                otherHealth.TakeDamage(_damage, _damageModifiers);
            }
            
            _projectilePool.Return(this);
        }

        public void Initialize(ObjectPool<RangedAttackProjectile> projectilePool)
        {
            _projectilePool = projectilePool;
        }

        public void Reset()
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
        
        public void Reset(Vector3 position, Vector3 linearVelocity)
        {
            transform.position = position;
            _rigidbody.position = position; 
            _rigidbody.AddForce(linearVelocity, ForceMode.VelocityChange);
        }
    }
}