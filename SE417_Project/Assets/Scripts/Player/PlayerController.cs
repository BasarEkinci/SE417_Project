using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioClip hitSound;
        //[SerializeField] private AudioClip collectSound;
        
        [Header("Health Settings")]
        [SerializeField] private int health = 100;
        [SerializeField] private float damageInterval = 1f;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider easeHealthbar;
        [SerializeField] private float lerpSpeed = 5f;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed;
        
        [Header("Visual Effects")]
        [SerializeField] private Color damageColor;
        [SerializeField] private ParticleSystem damageParticle;
        
        private Material _material;
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        
        private Vector3 _moveVector;
        
        private float _horizontalInput;
        private float _verticalInput;
        private bool _isTouchingEnemy;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            _material = GetComponent<Renderer>().material;
        }

        private void Start()
        {
            GetDamageFromEnemy().Forget(); 
        }

        private void Update()
        {
            GetMovementInput();
            UpdateHealthBar();
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                damageParticle.gameObject.transform.position = other.GetContact(0).point;
                _isTouchingEnemy = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _isTouchingEnemy = false;
            }
        }

        private async UniTaskVoid GetDamageFromEnemy()
        {
            while (true)
            {
                if (health > 0 && _isTouchingEnemy)
                {
                    health -= 10;
                    _audioSource.PlayOneShot(hitSound);
                    _material.DOColor(damageColor, 0.1f).SetLoops(2, LoopType.Yoyo);
                    if (!damageParticle.isPlaying)
                    {
                        damageParticle.Play();
                    }
                }
                await UniTask.Delay(TimeSpan.FromSeconds(damageInterval));  
            }
        }

        private void UpdateHealthBar()
        {
            healthBar.value = health;
            easeHealthbar.value = Mathf.Lerp(easeHealthbar.value, health,lerpSpeed);
        }

        private void GetMovementInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _moveVector = new Vector3(_horizontalInput, 0, _verticalInput);
            
            //To make the player move faster when holding shift key to decrease the linear damping
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _rigidbody.linearDamping = 5;
            }
            else
            {
                _rigidbody.linearDamping = 10;
            }
        }
        
        private void Move()
        {
            _rigidbody.linearVelocity += _moveVector * moveSpeed;
        }
    }
}
