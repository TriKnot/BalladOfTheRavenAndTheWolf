using System;
using Player.PlayerAbilities;
using Player.State;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Rigidbody _rb;
        public float CurrentSpeed;
        public float MinSpeed;
        public float TargetSpeed => _playerManager.State == PlayerState.Locked ? 0 : _defaultMoveSpeed * MoveSpeedMultiplier;
        private float _defaultMoveSpeed => _playerManager.Stats.MoveSpeed;
        [HideInInspector] public float MoveSpeedMultiplier = 1f;
        
        [SerializeField] [Range(0f, 0.2f)] private float _accelerationSpeed = 0.1f;
        [SerializeField] [Range(0f,0.2f)] private float _decelerationSpeed = 0.1f;
        [HideInInspector] public float AccelerationMultiplier = 1f;
        [HideInInspector] public float DecelerationMultiplier = 1f;
        [SerializeField] private Transform _playerBody;
        private Vector3 _direction = Vector3.zero;
        private float _newSpeed;
        private Quaternion _rotation;
        [SerializeField] Animator animator;
        
        [Header("Gravity")]
        [SerializeField] private float _gravity;
        public bool isGravityOn = true;

        private Vector2 _moveInput;
        private Vector3 _targetVector;

        [Header("Camera")]
        [SerializeField] private Camera _myCam;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] [Range(1,20)] private int rotationSpeed = 10;

        private Vector2 _movementInput = Vector2.zero;

        [Header("States")]
        public bool groundCheck;    
        
        private PlayerManager _playerManager;
        private PlayerStats _playerStats;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        
        private void Update()
        {
            Move();
            GroundedUpdate();
        } 
        private void FixedUpdate()
        {
            if (isGravityOn)
                Gravity();
        }

        
        private void Gravity()
        {
            _rb.AddForce(new Vector3(0f,-_gravity,0f));
        }

        private void GroundedUpdate()
        {
            groundCheck = (Physics.Raycast(transform.position, Vector3.down*1f, 2f));
        }
        public void Move()
        {
            if (_movementInput.magnitude > 0.3f)
            {
                _direction = new Vector3(_movementInput.normalized.x, 0, _movementInput.normalized.y);
                _direction = Quaternion.Euler(0, _myCam.gameObject.transform.eulerAngles.y, 0) * _direction;
                Vector3 norm = _direction.normalized;
                _direction = (_direction.magnitude > norm.magnitude) ? norm : _direction;
                
                _rotation = Quaternion.LookRotation(_direction); //Set rotation
                if(animator)
                    animator.SetBool("IsMoving", true);
            }
            else
            {
                if(animator)
                    animator.SetBool("IsMoving", false);
            }

            
            _playerBody.transform.rotation = Quaternion.RotateTowards(_playerBody.transform.rotation, _rotation, rotationSpeed); //Rotate body
      
            if (_movementInput.magnitude > 0.1f)
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, _accelerationSpeed*AccelerationMultiplier);
            }
            else if (CurrentSpeed > 0.1f)
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, MinSpeed, _decelerationSpeed*DecelerationMultiplier);
            }
            else if (CurrentSpeed < 0.1f)
            {
                CurrentSpeed = 0f;
            }
            
            _newSpeed = CurrentSpeed * Time.fixedDeltaTime;

            _rb.velocity = new Vector3(_direction.x * _newSpeed * 10, _rb.velocity.y, _direction.z * _newSpeed * 10);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }
    }
}
