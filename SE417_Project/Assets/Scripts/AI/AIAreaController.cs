using Signals;
using UnityEngine;

namespace AI
{
    public class AIAreaController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CoreGameSignals.Instance.OnPlayerEnterBed?.Invoke(true);
            }            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited bed");
                CoreGameSignals.Instance.OnPlayerEnterBed?.Invoke(false);
            }
        }
    }
}