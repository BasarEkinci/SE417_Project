using System;
using Signals;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Transform layerDetector;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private AudioClip defaultFootStepSound;
        [SerializeField] private AudioClip carpetFootStepSound;
        [SerializeField] private AudioClip defaultJumpSound;
        [SerializeField] private AudioClip carpetJumpSound;
        
        private PlayerController _playerController;
        private HealthController _healthController;
        private AudioSource _audioSource;
        private Animator _animator;
        

        private static readonly int IsInjured = Animator.StringToHash("IsInjured");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsStandingUp = Animator.StringToHash("IsStandingUp");
        private static readonly int IsCrouching = Animator.StringToHash("IsCrouching");
        private void Awake()
        {
            _audioSource = GetComponentInParent<AudioSource>();
            _playerController = GetComponentInParent<PlayerController>();
            _animator = GetComponent<Animator>();
            _healthController = GetComponentInParent<HealthController>();
        }

        public void OnEnable()
        {
            CoreGameSignals.Instance.OnPlayerHide += Fall;
            CoreGameSignals.Instance.OnPlayerWakeUp += StandUp;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnPlayerHide -= Fall;
            CoreGameSignals.Instance.OnPlayerWakeUp -= StandUp;
        }

        private void Update()
        {
            SetAnimationParameters();
        }

        private void SetAnimationParameters()
        {
            _animator.SetBool(IsMoving, _playerController.IsMoving);
            _animator.SetBool(IsJumping, _playerController.IsGrounded);
            _animator.SetBool(IsInjured, _healthController.IsInjured);
            _animator.SetBool(IsCrouching, _playerController.IsCrouching);
        }
        
        private void Fall()
        {
            _animator.Play("Fall");
            _animator.SetBool(IsStandingUp,false);
        }

        private void StandUp()
        {
            _animator.SetBool(IsStandingUp,true);
        }

        /// <summary>
        /// This method for playing footstep sounds in animation events
        /// </summary>
        public void PlayFootstepSound()
        {
            SetSound(defaultFootStepSound,carpetFootStepSound);
        }
        public void PlayJumpSound()
        {
            Debug.Log("Jump");
            SetSound(defaultJumpSound,carpetJumpSound);
        }

        private void SetSound(AudioClip soundType1, AudioClip soundType2)
        {
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            Collider[] results = new Collider[10];
            int colliderCount = Physics.OverlapSphereNonAlloc(layerDetector.position, 0.3f, results, groundLayer);
            if (colliderCount > 0)
            {
                Debug.Log("Tag: " + results[0].tag);
                switch (results[0].tag) 
                { 
                    case "Ground": 
                        _audioSource.PlayOneShot(soundType1); 
                        break;
                    case "Carpet": 
                        _audioSource.PlayOneShot(soundType2); 
                        break;
                    default: 
                        _audioSource.PlayOneShot(soundType1); 
                        break;
                }
            }
            else
            {
                Debug.Log("No ground detected");
            }
        }
    } 
}

