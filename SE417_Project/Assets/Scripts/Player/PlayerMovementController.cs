using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private float _moveSpeed = 2.8f;
        
        private float _horizontalInput;
        private float _verticalInput;
        private Vector3 _moveVector;
        
        private Rigidbody _rigidbody;
        private Animator _animator;
        private bool _isRunning;
        private bool _isWalking;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        private void Update()
        {
            GetMoveInput();
            SetAnimatorValues();
            transform.LookAt(transform.position + new Vector3(_moveVector.x,0,_moveVector.z));
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _moveVector * _moveSpeed;
        }

        private void GetMoveInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _moveVector = new Vector3(_horizontalInput, _rigidbody.linearVelocity.y, _verticalInput);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _moveSpeed = 3.5f;
                _isRunning = true;
            }
            else
            {
                _moveSpeed = 2.8f;
                _isRunning = false;
            }
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
