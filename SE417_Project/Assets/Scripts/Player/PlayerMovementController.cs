using Camera;
using Inputs;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public bool IsMoving => _moveVector.magnitude > 0;
        public bool IsJumping => !_layerDetector.IsLayerDetected();
        public bool IsInjured => _currentHealth < 25f;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float injuredMoveSpeed;
        [SerializeField] private float jumpForce;
        
        [Header("Health Settings")]
        [SerializeField] private int maxHealth;
        private int _currentHealth;
        
        [Header("References")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private CameraShake cameraShake;

        private InputHandler _inputHandler;
        private LayerDetector _layerDetector;
        private Rigidbody _rigidbody;
        private Vector2 _moveVector;
        private float _baseSpeed;
        private bool _isInjured;
        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
            _layerDetector = GetComponentInChildren<LayerDetector>();
        }

        private void Start()
        {
            _currentHealth = maxHealth;
            _baseSpeed = moveSpeed;
            healthBar.InitializeValues(maxHealth);
        }

        private void Update()
        {
            RotateToMoveDirection();
            Move();
            Jump();
            healthBar.UpdateValues(_currentHealth);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _currentHealth -= 10;
                cameraShake.ShakeCamera();
            }
        }

        private void Jump()
        {
            //if the player is injured and the health is less than 25, the player can't jump
            if (_currentHealth <=25f)
            {
                return;
            }
            
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
            moveSpeed = IsInjured ? injuredMoveSpeed : _baseSpeed;
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