using System;
using Camera;
using Cysharp.Threading.Tasks;
using Inputs;
using Signals;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsMoving => _moveVector.magnitude > 0;
        public bool IsJumping => !layerDetector.IsLayerDetected();
        public bool IsCrouching => _isCrouching;
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float injuredMoveSpeed;
        [SerializeField] private float crouchingSpeed;
        [SerializeField] private float jumpForce;

        [Header("Effects")]
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip collectSound;
        
        [Header("Class References")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private HealthController healthController;
        [SerializeField] private LayerDetector layerDetector;
        
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        private Vector2 _moveVector;
        private float _baseSpeed;
        private bool _isAttachedToEnemy;
        private bool _canMove;
        private bool _isDead;
        private bool _isCrouching;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _baseSpeed = moveSpeed;
            _canMove = true;
        }
        
        private void Update()
        {
            moveSpeed = _isCrouching ? crouchingSpeed : _baseSpeed;
            Debug.Log(moveSpeed);
            Crouch();
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
                TakeDamageAsync(20,1).Forget();
            }
            else if (other.gameObject.CompareTag("Obstacle"))
            {
                _isAttachedToEnemy = true;
                _audioSource.PlayOneShot(hitSound);
                healthController.Damage(5);
                if (!hitEffect.isPlaying)
                {
                    hitEffect.Play();
                }
                cameraShake.ShakeCamera();            }
            else if (other.gameObject.CompareTag("DangerArea"))
            {
                _isAttachedToEnemy = true;
                TakeDamageAsync(1,1).Forget();
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable"))
            {
                _audioSource.PlayOneShot(collectSound);
            }

            if (other.CompareTag("Medkit"))
            {
                if (healthController.MedkitCount < 3 && healthController.CurrentHealth >= healthController.MaxHealth)
                {
                    _audioSource.PlayOneShot(collectSound);
                    healthController.AddMedkit();
                    other.gameObject.SetActive(false);
                }
                else if (healthController.CurrentHealth < healthController.MaxHealth)
                {
                    healthController.Heal(20);
                    other.gameObject.SetActive(false);   
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _isAttachedToEnemy = false;
            }
            else if (other.gameObject.CompareTag("Obstacle"))
            {
                _isAttachedToEnemy = false;
            }
            else if (other.gameObject.CompareTag("DangerArea"))
            {
                _isAttachedToEnemy = false;
            }
        }

        private void Crouch()
        {
            if (InputHandler.Instance.GetCrouchInput())
            {
                _isCrouching = !_isCrouching;
            }
        }
        
        private void Dead()
        {
            if (healthController.CurrentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                CoreGameSignals.Instance.OnPlayerDie?.Invoke();
                Fall();
            }
        }

        private void UseMedkit()
        {
            if (InputHandler.Instance.GetHealInput() && healthController.CurrentHealth < healthController.MaxHealth)
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
            if (InputHandler.Instance.GetHideInput())
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
            if (InputHandler.Instance.GetJumpInput() && layerDetector.IsLayerDetected())
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
            _moveVector = InputHandler.Instance.GetMoveInput();
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
            CoreGameSignals.Instance.OnPlayerHide?.Invoke();
            _canMove = false;
        }
        
        //This method is used to stand up after the player hides
        private async UniTaskVoid StandUp()
        {
            CoreGameSignals.Instance.OnPlayerWakeUp?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _canMove = true;
        }

        private void Damage(int damage)
        {
            healthController.Damage(damage);
        }
        //This method is used to take damage every second if the player is attached to the enemy
        private async UniTaskVoid TakeDamageAsync(int damage, float duration)
        {
            while (true)
            {
                if (_isAttachedToEnemy && !_isDead)
                {

                }
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
            }
        }
    }
}