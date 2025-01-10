using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform arms;
        [SerializeField] private float moveSpeed;
        
        private NavMeshAgent _agent;
        private Animator _animator;
        private bool _canMove;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }
        
        private void Start()
        {
            _animator.enabled = false;
            _canMove = true;
        }

        private void OnEnable()
        {
            PlayerSignals.Instance.OnPlayerHide += Stop;
            PlayerSignals.Instance.OnPlayerWakeUp += Resume;
            PlayerSignals.Instance.OnPlayerDie += Stop;
        }
        private void OnDisable()
        {
            PlayerSignals.Instance.OnPlayerHide -= Stop;
            PlayerSignals.Instance.OnPlayerWakeUp -= Resume;
            PlayerSignals.Instance.OnPlayerDie -= Stop;
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
            _canMove = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _animator.enabled = true;
            _agent.speed = moveSpeed;
        }
    }
}
