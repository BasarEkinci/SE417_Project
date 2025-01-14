using UnityEngine;

namespace Utilities
{
    public class LayerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float detectionRadius;

    
        //To detect all selected layers
        internal bool IsLayersDetected()
        {
            return Physics.CheckSphere(transform.position, detectionRadius, layerMask);
        }

        internal bool IsLayerDetected(string layerName)
        {
            return Physics.CheckSphere(transform.position, detectionRadius, LayerMask.GetMask(layerName));
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, detectionRadius);
        }
    }
}
