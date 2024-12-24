using Inputs;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        public float MoveSpeed => _moveVector.magnitude;
        public bool IsMoving => _moveVector.magnitude > 0;
        public bool IsJumping => _isJumping;
        private InputHandler _inputHandler;
        private Rigidbody _rigidbody;
        private Vector2 _moveVector;
        private bool _isJumping;
        private bool _isGrounded;

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Debug.Log(_isJumping);
            // Rotate player to face movement direction
            if (_moveVector != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(_moveVector.x, _moveVector.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            if (_inputHandler.GetJumpInput() && _isGrounded)
            {
                Jump();
            }
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collision is with the ground
            if (collision.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Grounded");
                _isGrounded = true;
                _isJumping = false;
            }
        }


        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce);
            _isJumping = true;
            _isGrounded = false;
        }

        //This function controls the movement of the player. It takes the input and sets the velocity of the rigidbody.
        private void MovePlayer()
        {
            _moveVector = _inputHandler.GetMoveInput();
            _rigidbody.linearVelocity = new Vector3(_moveVector.x, _rigidbody.linearVelocity.y, _moveVector.y) * moveSpeed;
        }

    }
}