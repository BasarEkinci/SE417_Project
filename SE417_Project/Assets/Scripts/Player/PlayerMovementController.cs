using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        #region Class References
        private PlayerInputs _playerInputs;
        private Animator _animator;
        private Rigidbody _rigidbody;
        #endregion

        #region Variables
        private bool _isRunning;
        private float _baseSpeed;
        private Vector2 moveVector;
        #endregion

        #region Unity Functions

        private void Awake()
        {
            _playerInputs = new PlayerInputs();
            _playerInputs.Player.Enable();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _baseSpeed = moveSpeed;
        }

        private void Update()
        {
            SetAmimationParameters();
            if (moveSpeed == _baseSpeed && _isRunning)
            {
                moveSpeed = _baseSpeed * 2;
            }
            else if (moveSpeed != _baseSpeed && !_isRunning)
            {
                moveSpeed = _baseSpeed;
            }
            // Rotate player to face movement direction
            if (moveVector != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void OnEnable()
        {
            _playerInputs.Player.Run.started += OnRun;
            _playerInputs.Player.Run.canceled += OnRun;
        }

        private void OnDisable()
        {
            _playerInputs.Disable();
        }
        #endregion

        #region Custom Functions
        //This function controls the run key pressed or released : "Shift"
        private void OnRun(InputAction.CallbackContext context)
        {
            _isRunning = context.started;
        }

        //This function controls the movement of the player. It takes the input and sets the velocity of the rigidbody.
        private void MovePlayer()
        {
            moveVector = _playerInputs.Player.Move.ReadValue<Vector2>();
            _rigidbody.linearVelocity = new Vector3(moveVector.x, _rigidbody.linearVelocity.y, moveVector.y) * moveSpeed;
        }

        //This function sets the animation parameters for the animator.
        private void SetAmimationParameters()
        {
            _animator.SetBool("isRunning", _isRunning);
            _animator.SetBool("isWalking", moveVector.magnitude > 0);
        }
        #endregion
    }
}