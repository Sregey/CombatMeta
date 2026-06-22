using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class EnemyEntity: MonoBehaviour
    {
        [Header("Core")]
        [SerializeField]
        private Health _health;
        
        [SerializeField]
        private AbilityManager _abilityManager;
        
        [Header("Combat")]
        [SerializeField, MaybeNull]
        private Transform _target;
        
        [SerializeField]
        private float _meleeAttackDistance = 1.5f;
        
        private LocomotionAbility _locomotionAbility;
        private MeleeAttackAbility _meleeAttackAbility;
        
        private LocomotionAbility.Parameters _locomotionAbilityParameters;
        
        public Health Health => _health;

        [MaybeNull]
        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private void Awake()
        {
            _locomotionAbility = _abilityManager.Get<LocomotionAbility>();
            _meleeAttackAbility = _abilityManager.Get<MeleeAttackAbility>();
            
            _locomotionAbilityParameters = new LocomotionAbility.Parameters();
        }

        private void Update()
        {
            GetMoveDirection(out var moveDirection);
            MoveAndRotate(moveDirection);
            Attack(moveDirection);
        }
        
        private void GetMoveDirection(out Vector3 moveDirection)
        {
            moveDirection = _target ? (_target.position - transform.position) : Vector3.zero;
        }

        private void MoveAndRotate(Vector3 moveDirection)
        {
            _locomotionAbilityParameters.MoveDirection = moveDirection;
            _locomotionAbilityParameters.LookDirection = moveDirection;
            _locomotionAbility
                .TryUseAsync(_locomotionAbilityParameters, destroyCancellationToken)
                .LogExceptionsAndForget();
        }

        private void Attack(Vector3 targetDistance)
        {
            if (targetDistance.magnitude < _meleeAttackDistance)
            {
                _meleeAttackAbility
                    .TryUseAsync(token: destroyCancellationToken)
                    .LogExceptionsAndForget();
            }
        }
    }
}