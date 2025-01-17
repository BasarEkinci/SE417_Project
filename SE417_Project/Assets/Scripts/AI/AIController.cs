using System;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private AudioClip audioClip;
        private NavMeshAgent _agent;
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _canMove;
        private bool _isPlayerAlive;
        private float _baseSpeed;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            _baseSpeed = _agent.speed;
            _canMove = true;
        }

        private void OnEnable()
        {
            SubscribeEvents();
            _audioSource.clip = audioClip;
            _animator.enabled = true;
            _audioSource.Play();
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void OnPlayerDie()
        {
            _isPlayerAlive = false;
            Stop();
        }

        private void OnCompleteObjective(int level)
        {
            Stop();
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }
            if (playerTransform != null && _canMove)
            {
                _agent.SetDestination(playerTransform.position);
            }
        }
        private void Stop()
        {
            _audioSource.Stop();
            _canMove = false;
            _agent.speed = 0;
            _animator.enabled = false;
            _audioSource.Stop();
        }
        
        private async void Resume()
        {   
            _canMove = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _animator.enabled = true;
            _agent.speed = _baseSpeed;
            _audioSource.Play();
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide += Stop;
            CoreGameSignals.Instance.OnPlayerWakeUp += Resume;
            CoreGameSignals.Instance.OnPlayerDie += OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteObjective += OnCompleteObjective;
        }
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide -= Stop;
            CoreGameSignals.Instance.OnPlayerWakeUp -= Resume;
            CoreGameSignals.Instance.OnPlayerDie -= OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteObjective -= OnCompleteObjective;
        }
    }
}
