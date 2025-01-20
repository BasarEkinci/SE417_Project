using System;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;

        private NavMeshAgent _agent;
        private AudioSource _audioSource;
        private Animator _animator;
        private Transform _playerTransform;
        
        private bool _canMove;
        private bool _isActive;
        private bool _isPlayerAlive;
        private float _baseSpeed;

        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _agent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _baseSpeed = _agent.speed;
            _audioSource.clip = audioClip;
            ActivateAI();
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void Update()
        {
            if (_isActive)
            {
                _agent.SetDestination(_playerTransform.position);
            }
        }
        
        private async void ActivateAI()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _audioSource.Play();
            _animator.enabled = true;
            _isActive = true;
            _agent.speed = _baseSpeed;
        }
        
        private void DeactivateAI()
        {
            _audioSource.Stop();
            _animator.enabled = false;
            _isActive = false;
            _agent.speed = 0;
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide += DeactivateAI;
            CoreGameSignals.Instance.OnPlayerWakeUp += ActivateAI;
            CoreGameSignals.Instance.OnPlayerDie += DeactivateAI;
            CoreGameSignals.Instance.OnCompleteLevel += DeactivateAI;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide -= DeactivateAI;
            CoreGameSignals.Instance.OnPlayerWakeUp -= ActivateAI;
            CoreGameSignals.Instance.OnPlayerDie -= DeactivateAI;
            CoreGameSignals.Instance.OnCompleteLevel -= DeactivateAI;
        }
    }
}
