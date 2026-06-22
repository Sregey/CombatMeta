using UnityEngine;

namespace SaberCombatMeta.Simulation
{
    public class LocomotionAbility: Ability, ISyncAbility
    {
        public class Parameters
        {
            public Vector3 MoveDirection { get; set; }
            public Vector3 LookDirection { get; set; }
        }
        
        [SerializeField]
        private CharacterController _characterController;
        
        [SerializeField, Min(0)]
        private float _moveSpeed = 2;
        
        [SerializeField, Min(0)]
        private float _moveAcceleration = 3;
        
        [SerializeField, Min(0)]
        private float _angularSpeed = 180;

        private Vector3 _currentVelocity;
        private float _currentRotationY;

        public override void Initialize(AbilityManager manager)
        {
            base.Initialize(manager);
            _currentRotationY = transform.eulerAngles.y;
        }

        void ISyncAbility.Use(object parameters)
        {
            MoveAndRotate((Parameters)parameters);
        }

        private void MoveAndRotate(Parameters parameters)
        {
            Move(parameters.MoveDirection);
            Rotate(parameters.LookDirection);
        }

        private void Move(Vector3 moveDirection)
        {
            var currentHorizontalVelocity = new Vector3(_currentVelocity.x, 0, _currentVelocity.z);
            var targetHorizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z).normalized * _moveSpeed;
            currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetHorizontalVelocity, _moveAcceleration * Time.deltaTime);

            _currentVelocity = new Vector3(currentHorizontalVelocity.x, -2, currentHorizontalVelocity.z);
            _characterController.Move(_currentVelocity * Time.deltaTime);
            _currentVelocity = _characterController.velocity;
        }

        private void Rotate(Vector3 lookDirection)
        {
            lookDirection.y = 0;
            if (lookDirection == Vector3.zero)
                return;

            var targetRotationY = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
            _currentRotationY = Mathf.MoveTowardsAngle(_currentRotationY, targetRotationY, _angularSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, _currentRotationY, 0);
        }
    }
}