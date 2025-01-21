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
        [SerializeField] private Transform initialPosition;
        [SerializeField] private bool isReptile;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private float activateTime = 2f;
        
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
            SubscribeEvents();
            _baseSpeed = _agent.speed;
            _audioSource.clip = audioClip;
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

        private void OnGameStart()
        {
            if (isReptile)
                DeactivateAI();
            else
                ActivateAI();
        }
        
        private async void ActivateAI()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(activateTime));
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
            CoreGameSignals.Instance.OnCompleteObjective += DeactivateAI;
            CoreGameSignals.Instance.OnPlayerEnterBed += OnPlayerEnterBed;
            CoreGameSignals.Instance.OnGameRestart += OnGameRestart;
            CoreGameSignals.Instance.OnGameStart += OnGameStart;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide -= DeactivateAI;
            CoreGameSignals.Instance.OnPlayerWakeUp -= ActivateAI;
            CoreGameSignals.Instance.OnPlayerDie -= DeactivateAI;
            CoreGameSignals.Instance.OnCompleteLevel -= DeactivateAI;
            CoreGameSignals.Instance.OnPlayerEnterBed -= OnPlayerEnterBed;
            CoreGameSignals.Instance.OnGameRestart -= OnGameRestart;
            CoreGameSignals.Instance.OnGameStart -= OnGameStart;
            CoreGameSignals.Instance.OnCompleteObjective -= DeactivateAI;
        }

        private void OnGameRestart()
        {
            transform.position = initialPosition.position;
            ActivateAI();
        }

        private void OnPlayerEnterBed(bool condition)
        {
            if (condition == isReptile)
            {
                ActivateAI();
            }
            else
            {
                DeactivateAI();
            }
        }
    }
}
