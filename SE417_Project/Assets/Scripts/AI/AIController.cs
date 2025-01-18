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

        protected virtual void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
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
        
        protected virtual void ActivateAI()
        {
            _audioSource.Play();
            _isActive = true;
        }
        
        protected virtual void DeactivateAI()
        {
            _audioSource.Stop();
            _isActive = false;
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
