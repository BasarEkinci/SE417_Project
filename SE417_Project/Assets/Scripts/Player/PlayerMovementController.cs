using Inputs;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public float MoveSpeed => _moveVector.magnitude;
        public bool IsMoving => _moveVector.magnitude > 0;
        public bool IsJumping => !_layerDetector.IsLayerDetected();

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;

        private InputHandler _inputHandler;
        private LayerDetector _layerDetector;
        private Rigidbody _rigidbody;
        private Vector2 _moveVector;

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
            _layerDetector = GetComponentInChildren<LayerDetector>();
        }
        private void Update()
        {
            RotateToMoveDirection();
            Move();
            Jump();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Damaged");
            }
        }

        private void Jump()
        {
            if (_inputHandler.GetJumpInput() && _layerDetector.IsLayerDetected())
            {

                Vector3 velocity = _rigidbody.linearVelocity;
                velocity.y = 0f;
                _rigidbody.linearVelocity = velocity;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        private void Move()
        {
            _moveVector = _inputHandler.GetMoveInput();
            Vector3 movement = new Vector3(_moveVector.x, 0, _moveVector.y) * moveSpeed;
            transform.position += movement * Time.deltaTime;
        }
        private void RotateToMoveDirection()
        {
            if (_moveVector != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(_moveVector.x, _moveVector.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}