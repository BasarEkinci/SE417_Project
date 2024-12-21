using Inputs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2.8f;
        
        private float _horizontalInput;
        private float _verticalInput;
        private Vector3 _moveVector;
        
        private Rigidbody _rigidbody;
        private Animator _animator;
        private InputHandler _inputHandler;
        private bool _isRunning;
        private bool _isWalking;
        private float _baseSpeed;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _baseSpeed = moveSpeed;
            _inputHandler = GetComponent<InputHandler>();
        }
        
        private void Update()
        {
            GetMoveInput();
            SetAnimatorValues();
            transform.LookAt(transform.position + new Vector3(_moveVector.x,0,_moveVector.z));
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _moveVector * moveSpeed;
        }

        private void GetMoveInput()
        {
            Vector2 movementInput = _inputHandler.GetMovementInput();
            _moveVector = new Vector3(movementInput.x, _rigidbody.linearVelocity.y, movementInput.y);
            moveSpeed = _inputHandler.IsRunning(_isRunning) ? _baseSpeed * 2 : _baseSpeed;
        }
        
        private void SetAnimatorValues()
        {
            if (_moveVector.magnitude > 0)
            {
                _isWalking = true;
            }
            else
            {
                _isWalking = false;
            }
            _animator.SetBool("isRunning",_isRunning);
            _animator.SetBool("isWalking",_isWalking);
        }
    }
}
