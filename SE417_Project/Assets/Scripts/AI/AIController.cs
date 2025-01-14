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
        
        private NavMeshAgent _agent;
        private Animator _animator;
        private bool _canMove;
        private bool _isPlayerAlive;
        private float _baseSpeed;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }
        
        private void Start()
        {
            _baseSpeed = _agent.speed;
            _animator.enabled = false;
            _canMove = true;
        }

        private void OnEnable()
        {
            CoreGameSignals.Instance.OnPlayerHide += Stop;
            CoreGameSignals.Instance.OnPlayerWakeUp += Resume;
            CoreGameSignals.Instance.OnPlayerDie += OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteObjective += OnCompleteObjective;
        }
        private void OnDisable()
        {
            CoreGameSignals.Instance.OnPlayerHide -= Stop;
            CoreGameSignals.Instance.OnPlayerWakeUp -= Resume;
            CoreGameSignals.Instance.OnPlayerDie -= OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteObjective -= OnCompleteObjective;
        }

        private void OnPlayerDie()
        {
            _isPlayerAlive = false;
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
            _canMove = false;
            _agent.speed = 0;
            _animator.enabled = false;
        }
        
        private async void Resume()
        {   
            if(!_isPlayerAlive) return;
            _canMove = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _animator.enabled = true;
            _agent.speed = _baseSpeed;
        }
    }
}
