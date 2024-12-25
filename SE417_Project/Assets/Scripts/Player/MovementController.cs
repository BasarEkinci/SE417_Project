using System;
using Camera;
using Cysharp.Threading.Tasks;
using Inputs;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class MovementController : MonoBehaviour
    {
        public bool IsMoving => _moveVector.magnitude > 0;
        public bool IsJumping => !layerDetector.IsLayerDetected();

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float injuredMoveSpeed;
        [SerializeField] private float jumpForce;
        
        [Header("References")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private HealthController healthController;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private LayerDetector layerDetector;
        
        private Rigidbody _rigidbody;
        private Vector2 _moveVector;
        private float _baseSpeed;
        private bool _isAttachedToEnemy;
        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
             healthController.Initialize();
            _baseSpeed = moveSpeed;
            GetDamageAsync().Forget();
        }

        private void Update()
        {
            RotateToMoveDirection();
            Move();
            Jump();
        }
        
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isAttachedToEnemy = true;
            }
        }
        
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isAttachedToEnemy = false;
            }
        }        
        
        private async UniTaskVoid GetDamageAsync()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                if (_isAttachedToEnemy)
                {
                    healthController.GetDamage(10);
                    cameraShake.ShakeCamera();   
                }    
            }
        }
        
        private void Jump()
        {
            //if the player is injured and the health is less than 25, the player can't jump
            if (healthController.IsInjured)
            {
                return;
            }
            if (inputHandler.GetJumpInput() && layerDetector.IsLayerDetected())
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        private void Move()
        {
            _moveVector = inputHandler.GetMoveInput();
            moveSpeed = healthController.IsInjured ? injuredMoveSpeed : _baseSpeed;
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