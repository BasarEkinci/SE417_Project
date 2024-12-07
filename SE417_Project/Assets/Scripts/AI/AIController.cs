using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        private void Update()
        {
            if (playerTransform != null)
            {
                _agent.SetDestination(playerTransform.position);
            }
        }
    }
}
