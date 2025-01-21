using System;
using Camera;
using Cysharp.Threading.Tasks;
using Inputs;
using Signals;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsMoving => _moveVector.magnitude > 0;
        public bool IsGrounded => !layerDetectorDown.IsLayerDetected(); 
        public bool IsCrouching => _isCrouching;

        [Header("Colliders")]
        [SerializeField] private Collider baseCollider;
        [SerializeField] private Collider crouchCollider;
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float injuredMoveSpeed;
        [SerializeField] private float crouchingSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private Transform initialPosition;

        [Header("VFX")]
        [SerializeField] private ParticleSystem hitEffect;
        
        [Header("SFX")]
        [SerializeField] private AudioClip collectSound;
        
        [Header("Class References")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private HealthController healthController;
        [SerializeField] private LayerDetector layerDetectorDown;
        [SerializeField] private LayerDetector layerDetectorUp;
        
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        private Vector2 _moveVector;
        private float _baseSpeed;
        private bool _isAttachedToEnemy;
        private bool _canMove;
        private bool _isCrouching;
        private bool _canStandUp;
        private bool _isJumping;
        private bool _isLevelComplete;

        #region Unity Methods (Awake, OnEnable, OnDisable, Start, Update)
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            _baseSpeed = moveSpeed;
            baseCollider.enabled = true;
            crouchCollider.enabled = false;
        }
        
        private void OnEnable()
        {
            _canMove = false;
            InputHandler.Instance.PlayerInputs.Player.Crouch.performed += OnCrouchPerformed;
            InputHandler.Instance.PlayerInputs.Player.Jump.performed += OnJumpPerformed;
            InputHandler.Instance.PlayerInputs.Player.Hide.performed += OnHidePerformed;
            InputHandler.Instance.PlayerInputs.Player.Heal.performed += OnHealPerformed;
            CoreGameSignals.Instance.OnCompleteLevel += ()=> _canMove = false;
            CoreGameSignals.Instance.OnGameStart += () => _canMove = true;
            CoreGameSignals.Instance.OnPlayerDie += OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteObjective += ()=> _isLevelComplete = true;
        }

        private void OnDisable()
        {
            InputHandler.Instance.PlayerInputs.Player.Crouch.performed -= OnCrouchPerformed;
            InputHandler.Instance.PlayerInputs.Player.Jump.performed -= OnJumpPerformed;
            InputHandler.Instance.PlayerInputs.Player.Hide.performed -= OnHidePerformed;
            InputHandler.Instance.PlayerInputs.Player.Heal.performed -= OnHealPerformed;
            CoreGameSignals.Instance.OnCompleteLevel -= ()=> _canMove = false;
            CoreGameSignals.Instance.OnGameStart -= () => _canMove = true;
            CoreGameSignals.Instance.OnPlayerDie -= OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteObjective -= ()=> _isLevelComplete = true;
        }
        private void Update()
        {
            if (healthController.IsInjured)
            {
                moveSpeed = injuredMoveSpeed;
            }
            else if (_isCrouching)
            {
                moveSpeed = crouchingSpeed;
            }
            else
            {
                moveSpeed = _baseSpeed;
            }
            layerDetectorDown.IsLayerDetected();
            _canStandUp = !layerDetectorUp.IsLayerDetected();
            RotateToMoveDirection();
            Move();
        }
        #endregion
        #region Unity Methods (Physics)
        private void OnCollisionEnter(Collision other)
        {
            if (!_canMove)
            {
                return;
            }
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    _isAttachedToEnemy = true;
                    TakeDamageAsync(10,1).Forget();
                    break;
                case "Obstacle":
                    _isAttachedToEnemy = true;
                    healthController.Damage(1);
                    if (!hitEffect.isPlaying)
                    {
                        hitEffect.Play();
                    }
                    cameraShake.ShakeCamera();
                    break;
                case "DangerArea":
                    _isAttachedToEnemy = true;
                    TakeDamageAsync(1,1).Forget();
                    break;
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {

            switch (other.tag)
            {
                case "Collectable":
                    _audioSource.PlayOneShot(collectSound);
                    break;
                case "Medkit":
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
                    break;
            }
        }
        private void OnCollisionExit(Collision other)
        {
            _isAttachedToEnemy = other.gameObject.tag switch
            {
                "Enemy" or "Obstacle" or "DangerArea" => false,
                _ => _isAttachedToEnemy
            };
        }
        #endregion
        #region Input Actions

        //When player press the "E" key. If the player health is less than max health, the player can use medkit
        private void OnHealPerformed(InputAction.CallbackContext obj)
        {
            if (healthController.CurrentHealth < healthController.MaxHealth)
            {
                healthController.UseMedkit();
            }
        }

        //When player press the "R" key. If the player is not dead, the player can hide by falling down
        private void OnHidePerformed(InputAction.CallbackContext obj)
        {
            if (healthController.IsDead)
            {
                return;
            }

            if (_canMove)
            {
                CoreGameSignals.Instance.OnPlayerHide?.Invoke();
                _canMove = false;
            }
            else
            {
                StandUp().Forget();
            }
        }

        //When player press the "Space" key. If the player health is less than 25 or crouching, the player can't jump
        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            //if the player health is less than 25 or crouching, the player can't jump
            if (healthController.IsInjured || _isCrouching)
            {
                return;
            }
            if (layerDetectorDown.IsLayerDetected())
            {
                if (!_canMove)
                {
                    return;
                }
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void OnCrouchPerformed(InputAction.CallbackContext obj)
        {
            if (!_canStandUp)
            {
                return;
            }
            _isCrouching = !_isCrouching;
            baseCollider.enabled = !_isCrouching;
            crouchCollider.enabled = _isCrouching;
        }

        #endregion
        #region Player Methods
        private void OnPlayerDie()
        {
            _isAttachedToEnemy = false;
            CoreGameSignals.Instance.OnPlayerHide?.Invoke();
            _canMove = false;
        }
        private void Move()
        {
            _moveVector = InputHandler.Instance.GetMoveInput();
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
        //This method is used to stand up after the player hides
        private async UniTaskVoid StandUp()
        {
            CoreGameSignals.Instance.OnPlayerWakeUp?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _canMove = true;
        }
        //This method is used to take damage every second if the player is attached to the enemy
        private async UniTaskVoid TakeDamageAsync(int damage, float duration)
        {
            while (true)
            {
                if (_isLevelComplete)
                {
                    _isAttachedToEnemy = false;
                    return;
                }
                if (_isAttachedToEnemy && !healthController.IsDead)
                {
                    healthController.Damage(damage);
                    if (!hitEffect.isPlaying)
                    {
                        hitEffect.Play();
                    }
                    cameraShake.ShakeCamera();
                }
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
            }
        }

        #endregion
    }
}