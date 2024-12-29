using System;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private int damageAmount;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform arms;
        private NavMeshAgent _agent;
        private Animator _animator;
        private HealthController _player;
        private bool _canMove;
        private bool _isAttachedToPlayer;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }
        
        private void Start()
        {
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
            if (playerTransform != null && _canMove)
            {
                _agent.SetDestination(playerTransform.position);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            _player = other.gameObject.GetComponent<HealthController>();
            if (_player != null)
            {
                _isAttachedToPlayer = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            _player = other.gameObject.GetComponent<HealthController>();
            if (_player != null)
            {
                _isAttachedToPlayer = false;
            }
        }

        public void DamagePlayer()
        {
            if (_isAttachedToPlayer)
            {
                _player.GetDamage(damageAmount);
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
            _agent.speed = 3.5f;
        }
    }
}
