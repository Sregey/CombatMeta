using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class AoeAttackShockwave: MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float _range;
        
        [SerializeField, Min(0)]
        private float _damage;
        
        [SerializeField, Min(0)]
        private float _attackDuration;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Health>(out var otherHealth))
            {
                otherHealth.TakeDamage(_damage);
            }
        }

        public async Awaitable AttackAsync(CancellationToken token)
        {
            gameObject.SetActive(true);
            
            var scaleXZ = 0f;
            var scaleY = transform.localScale.y;
            var speed = 2 * _range / _attackDuration;

            while (true)
            {
                scaleXZ += speed * Time.deltaTime;
                transform.localScale = new Vector3(scaleXZ, scaleY, scaleXZ);
                if (scaleXZ >= 2 * _range)
                    break;

                await Awaitable.NextFrameAsync(token);
            }
            
            gameObject.SetActive(false);
        }
    }
}