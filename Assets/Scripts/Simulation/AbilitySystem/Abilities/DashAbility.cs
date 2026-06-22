using System.Threading;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public sealed class DashAbility: Ability, IAsyncAbility, ICooldownAbility
    {
        public class Parameters
        {
            public Vector3 MoveDirection { get; set; }
        }
        
        [SerializeField]
        private CharacterController _characterController;
        
        [SerializeField, Min(0)]
        private float _moveSpeed = 15;
        
        [SerializeField, Min(0)]
        private float _duration = 1;
        
        [SerializeField]
        private Cooldown _cooldown;
        
        public Cooldown Cooldown => _cooldown;

        protected override bool CanUse()
        {
            return _cooldown.IsFinished && base.CanUse();
        }

        Awaitable IAsyncAbility.UseAsync(object parameters, CancellationToken token)
        {
            _cooldown.Start();
            return DashAsync((Parameters)parameters, token);
        }
        
        private async Awaitable DashAsync(Parameters parameters, CancellationToken token)
        {
            var moveDirection = parameters.MoveDirection;
            var moveDuration = _duration;
            
            if (moveDirection == Vector3.zero)
                return;
            
            var velocity = new Vector3(moveDirection.x, 0, moveDirection.z).normalized * _moveSpeed;
            velocity.y = -2;
            
            while(true)
            {
                _characterController.Move(velocity * Time.deltaTime);
                moveDuration -= Time.deltaTime;
                if (moveDuration <= 0)
                    break;

                await Awaitable.NextFrameAsync(token);
            }
        }
    }
}