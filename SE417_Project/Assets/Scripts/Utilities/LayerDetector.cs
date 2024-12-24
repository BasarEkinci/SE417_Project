using UnityEngine;

public class LayerDetector : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float detectionRadius;

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
