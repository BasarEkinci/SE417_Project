using UnityEngine;

namespace Utilities
{
    public class LayerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float detectionRadius;

    
        //To detect all selected layers
        internal bool IsLayerDetected()
        {
            return Physics.CheckSphere(transform.position, detectionRadius, layerMask);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, detectionRadius);
        }
    }
}
