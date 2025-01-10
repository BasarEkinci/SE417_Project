using System;
using Camera;
using Cysharp.Threading.Tasks;
using Inputs;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
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
        private bool _canMove;
        private bool _isDead;
        
        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _baseSpeed = moveSpeed;
            _canMove = true;
            TakeDamageAsync().Forget();
        }
        
        private void Update()
        {
            UseMedkit();
            Hide();
            Dead();
            RotateToMoveDirection();
            Move();
            Jump();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _isAttachedToEnemy = true;
            }
        }
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _isAttachedToEnemy = false;
            }
        }

        private void Dead()
        {
            if (healthController.CurrentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                PlayerSignals.Instance.OnPlayerDie?.Invoke();
                Fall();
            }
        }

        private void UseMedkit()
        {
            if (inputHandler.GetHealInput() && healthController.CurrentHealth < healthController.MaxHealth)
            {
                healthController.UseMedkit();
            }
        }
        private void Hide()
        {
            if (_isDead)
            {
                return;
            }
            if (inputHandler.GetHideInput())
            {
                if (_canMove)
                {
                    Fall();
                }
                else
                {
                    StandUp().Forget();
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
                if (!_canMove)
                {
                    return;
                }
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        private void Move()
        {
            _moveVector = inputHandler.GetMoveInput();
            moveSpeed = healthController.IsInjured ? injuredMoveSpeed : _baseSpeed;
            Vector3 movement = new Vector3(_moveVector.x, 0, _moveVector.y) * moveSpeed;
            if (!_canMove)
            {
                return;
            }
            transform.position += movement * Time.deltaTime;
        }
        
        //This method is used to rotate the player towards the move direction
        private void RotateToMoveDirection()
        {
            if (!_canMove)
            {
                return;
            }
            if (_moveVector != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(_moveVector.x, _moveVector.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        //This method is used to make the player fall when the player dies.
        private void Fall()
        {
            PlayerSignals.Instance.OnPlayerHide?.Invoke();
            _canMove = false;
        }
        
        //This method is used to stand up after the player hides
        private async UniTaskVoid StandUp()
        {
            PlayerSignals.Instance.OnPlayerWakeUp?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _canMove = true;
        }
        
        
        //This method is used to take damage every second if the player is attached to the enemy
        private async UniTaskVoid TakeDamageAsync()
        {
            while (true)
            {
                if (_isAttachedToEnemy && !_isDead)
                {
                    cameraShake.ShakeCamera();
                    healthController.Damage(20);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
            }
        }
    }
}