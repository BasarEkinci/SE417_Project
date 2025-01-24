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
        [SerializeField] private float baseSpeed;
        
        private NavMeshAgent _agent;
        private AudioSource _audioSource;
        private Animator _animator;
        private Transform _playerTransform;
        
        private bool _canMove;
        private bool _isActive;
        private bool _isPlayerAlive;
        private bool _isPlayerUnderBed;
        private bool _isLevelCompleted;
        private float _currentSpeed;
        
        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _agent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            _isLevelCompleted = false;
            SubscribeEvents();
            _audioSource.clip = audioClip;
            _currentSpeed = baseSpeed;
            _currentSpeed -= PlayerPrefs.GetFloat("SpeedMultiplier");
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
            activateTime = 1f;
            if (isReptile)
                DeactivateAI();
            else
                ActivateAI();
        }
        
        private async void ActivateAI()
        {
            if (_isLevelCompleted)
            {
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(activateTime));
            _audioSource.Play();
            _animator.enabled = true;
            _isActive = true;
            _agent.speed = _currentSpeed;
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
            CoreGameSignals.Instance.OnCompleteObjective += OnCompleteObjective;
            CoreGameSignals.Instance.OnPlayerEnterBed += OnPlayerEnterBed;
            CoreGameSignals.Instance.OnGameRestart += OnGameRestart;
            CoreGameSignals.Instance.OnGameStart += OnGameStart;
            CoreGameSignals.Instance.OnPauseGame += OnPauseGame;
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
            CoreGameSignals.Instance.OnCompleteObjective -= OnCompleteObjective;
            CoreGameSignals.Instance.OnPauseGame -= OnPauseGame;
        }

        private void OnCompleteObjective()
        {
            _isLevelCompleted = true;
            DeactivateAI();
        }

        private void OnGameRestart()
        {
            transform.position = initialPosition.position;
            ActivateAI();
        }
        private void OnPauseGame(bool condition)
        {
            activateTime = 0.1f;
            if (isReptile && !_isPlayerUnderBed)
            {
                return;
            }
            if (condition)
            {
                DeactivateAI();
            }
            else
            {
                ActivateAI();
            }
        }
        private void OnPlayerEnterBed(bool condition)
        {
            if (condition == isReptile)
            {
                _isPlayerUnderBed = condition;
                ActivateAI();
            }
            else
            {
                _isPlayerUnderBed = condition;
                DeactivateAI();
            }
        }
    }
}
