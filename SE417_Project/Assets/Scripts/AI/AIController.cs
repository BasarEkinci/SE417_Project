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
        [SerializeField] private Transform playerTransform;
        [SerializeField] private AudioClip audioClip;
        
        private NavMeshAgent _agent;
        private AudioSource _audioSource;
        
        private bool _canMove;
        private bool _isActive;
        private bool _isPlayerAlive;
        private float _baseSpeed;

        protected virtual void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
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
                _agent.SetDestination(playerTransform.position);
            }
        }
        
        protected virtual async void ActivateAI()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _audioSource.Play();
            _isActive = true;
            _agent.speed = _baseSpeed;
        }
        
        protected virtual void DeactivateAI()
        {
            _audioSource.Stop();
            _isActive = false;
            _agent.speed = 0;
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide += DeactivateAI;
            CoreGameSignals.Instance.OnPlayerWakeUp += ActivateAI;
            CoreGameSignals.Instance.OnPlayerDie += ActivateAI;
            CoreGameSignals.Instance.OnCompleteLevel += DeactivateAI;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPlayerHide -= DeactivateAI;
            CoreGameSignals.Instance.OnPlayerWakeUp -= ActivateAI;
            CoreGameSignals.Instance.OnPlayerDie -= ActivateAI;
            CoreGameSignals.Instance.OnCompleteLevel -= DeactivateAI;
        }
    }
}
