using UnityEngine;
using UnityEngine.InputSystem;

namespace SaberCombatMeta.Simulation
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private float _distanceFromTarget = 5f;

        [SerializeField]
        private float _heightAboveTarget = 2f;

        [Header("Rotation Settings")]
        [SerializeField]
        private float _mouseSensitivity = 10f;

        [SerializeField]
        private float _minVerticalAngle = -30f;

        [SerializeField]
        private float _maxVerticalAngle = 60f;

        [Header("Smoothing")]
        [SerializeField]
        private float _positionSmoothTime = 0.3f;

        [SerializeField]
        private bool _useSmoothing = true;

        [Header("Collision")]
        [SerializeField]
        private float _collisionRadius = 0.3f;

        [SerializeField]
        private LayerMask _collisionLayers;

        [SerializeField]
        private bool _enableCollisionAvoidance = true;

        private float _horizontalRotation;
        private float _verticalRotation;
        private Vector3 _velocitySmoothing = Vector3.zero;
        private float _currentDistance;

        private InputAction _lookAction;
        // private InputAction _scrollAction;
        
        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                if (_target != null)
                    SetupInitialPositionAndRotation();
            }
        }

        private void Awake()
        {
            _lookAction = InputSystem.actions.FindAction("Look");
            // _scrollAction = InputSystem.actions.FindAction["Scroll"];
        }

        private void OnEnable()
        {
            _lookAction.Enable();
        }

        private void OnDisable()
        {
            _lookAction.Disable();
        }

        private void Start()
        {
            _currentDistance = _distanceFromTarget;
            
            if (_target != null)
                SetupInitialPositionAndRotation();
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            HandleInput();
            UpdateCameraRotation();
            UpdateCameraPosition();
        }

        private void SetupInitialPositionAndRotation()
        {
            _horizontalRotation = _target.eulerAngles.y;
            _verticalRotation = Mathf.Clamp(0, _minVerticalAngle, _maxVerticalAngle);
            var initialRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);

            var targetPivot = _target.position + Vector3.up * _heightAboveTarget;
            var initialPosition = targetPivot + (initialRotation * Vector3.back * _currentDistance);

            transform.SetPositionAndRotation(initialPosition, initialRotation);
        }

        private void HandleInput()
        {
            var lookInput = _lookAction.ReadValue<Vector2>();

            _horizontalRotation += lookInput.x * _mouseSensitivity * Time.deltaTime;

            _verticalRotation -= lookInput.y * _mouseSensitivity * Time.deltaTime;
            _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);

            // var scroll = _scrollAction?.ReadValue<float>() ?? 0;
            // if (scroll != 0)
            // {
            //     _currentDistance = Mathf.Clamp(_currentDistance - scroll * 2f, 1f, 15f);
            // }
        }

        private void UpdateCameraRotation()
        {
            var rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
            transform.rotation = rotation;
        }

        private void UpdateCameraPosition()
        {
            var desiredPosition = _target.position 
                                  + transform.rotation * Vector3.back * _currentDistance
                                  + Vector3.up * _heightAboveTarget;

            if (_enableCollisionAvoidance)
            {
                desiredPosition = HandleCollisions(_target.position + Vector3.up * _heightAboveTarget, desiredPosition);
            }

            if (_useSmoothing)
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    desiredPosition,
                    ref _velocitySmoothing,
                    _positionSmoothTime
                );
            }
            else
            {
                transform.position = desiredPosition;
            }

            transform.LookAt(_target.position + Vector3.up * (_heightAboveTarget * 0.5f));
        }

        private Vector3 HandleCollisions(Vector3 lookFromPosition, Vector3 desiredPosition)
        {
            var directionToCamera = (desiredPosition - lookFromPosition).normalized;
            var distanceToCamera = Vector3.Distance(lookFromPosition, desiredPosition);

            if (Physics.SphereCast(
                    lookFromPosition,
                    _collisionRadius,
                    directionToCamera,
                    out RaycastHit hit,
                    distanceToCamera,
                    _collisionLayers
                ))
            {
                return lookFromPosition + directionToCamera * (hit.distance - _collisionRadius);
            }

            return desiredPosition;
        }
    }
}