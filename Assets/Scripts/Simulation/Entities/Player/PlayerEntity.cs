using UnityEngine;
using UnityEngine.InputSystem;

namespace SaberCombatMeta.Simulation
{
    public class PlayerEntity: MonoBehaviour
    {
        [Header("Core")]
        [SerializeField]
        private Health _health;
        
        [SerializeField]
        private AbilityManager _abilityManager;
        
        [Header("Input")]
        [SerializeField]
        private PlayerInput _playerInput;
        
        private CameraController _cameraController;
        
        private LocomotionAbility _locomotionAbility;
        private RangedAttackAbility _rangedAttackAbility;
        private AoeAttackAbility _aoeAttackAbility;
        private DashAbility _dashAbility;
        
        private LocomotionAbility.Parameters _locomotionAbilityParameters;
        private DashAbility.Parameters _dashAbilityParameters;

        private InputAction _moveAction;
        private InputAction _attackAction;
        private InputAction _dashAction;
        private InputAction _aoeAction;

        public Health Health => _health;
        
        public AoeAttackAbility AoeAttackAbility => _aoeAttackAbility;
        public DashAbility DashAbility => _dashAbility;
        
        public CameraController CameraController
        {
            get => _cameraController;
            set => _cameraController = value;
        }
        
        public void SetEnabledInputs(bool enableInput)
        {
            if (enableInput)
            {
                _playerInput.actions.FindActionMap("Player").Enable();
            }
            else
            {
                _playerInput.actions.FindActionMap("Player").Disable();
            }
        }

        private void Awake()
        {
            _moveAction = _playerInput.actions["Move"];
            _attackAction = _playerInput.actions["PrimaryAttack"];
            _dashAction = _playerInput.actions["Dash"];
            _aoeAction = _playerInput.actions["SecondaryAttack"];

            _locomotionAbility = _abilityManager.Get<LocomotionAbility>();
            _rangedAttackAbility = _abilityManager.Get<RangedAttackAbility>();
            _aoeAttackAbility = _abilityManager.Get<AoeAttackAbility>();
            _dashAbility = _abilityManager.Get<DashAbility>();
            
            _locomotionAbilityParameters = new LocomotionAbility.Parameters();
            _dashAbilityParameters = new DashAbility.Parameters();
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _attackAction.Enable();
            _dashAction.Enable();
            _aoeAction.Enable();
        }

        private void OnDisable()
        {
            _moveAction.Disable();
            _attackAction.Disable();
            _dashAction.Disable();
            _aoeAction.Disable();
        }

        private void Update()
        {
            GetMoveAndLookDirections(out var moveDirection, out var lookDirection);
            MoveAndRotate(moveDirection, lookDirection);
            Attack();
            AttackAoe();
            Dash(moveDirection);
        }
        
        private void GetMoveAndLookDirections(out Vector3 moveDirection, out Vector3 lookDirection)
        {
            var rotation = _cameraController.transform.rotation;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            
            lookDirection = rotation * Vector3.forward;
            
            var moveActionValue = _moveAction.ReadValue<Vector2>();
            moveDirection = moveActionValue != Vector2.zero
                ? rotation * new Vector3(moveActionValue.x, 0, moveActionValue.y)
                : Vector3.zero;
        }

        private void MoveAndRotate(Vector3 moveDirection, Vector3 lookDirection)
        {
            _locomotionAbilityParameters.MoveDirection = moveDirection;
            _locomotionAbilityParameters.LookDirection = lookDirection;
            _locomotionAbility
                .TryUseAsync(_locomotionAbilityParameters, destroyCancellationToken)
                .LogExceptionsAndForget();
        }

        private void Attack()
        {
            if (_attackAction.WasPressedThisFrame())
            {
                _rangedAttackAbility
                    .TryUseAsync(token: destroyCancellationToken)
                    .LogExceptionsAndForget();
            }
        }
        
        private void AttackAoe()
        {
            if (_aoeAction.WasPressedThisFrame())
            {
                _aoeAttackAbility
                    .TryUseAsync(token: destroyCancellationToken)
                    .LogExceptionsAndForget();
            }
        }

        private void Dash(Vector3 moveDirection)
        {
            if (_dashAction.WasPressedThisFrame())
            {
                _dashAbilityParameters.MoveDirection = moveDirection;
                _dashAbility
                    .TryUseAsync(_dashAbilityParameters, destroyCancellationToken)
                    .LogExceptionsAndForget();
            }
        }
    }
}