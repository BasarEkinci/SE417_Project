using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Camera
{
    public class CameraObstacleHandle : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask targetLayer;

        private Vector3 _direction;
        private Material _material;
        private GameObject _hitObject;
        private List<GameObject> _hitObjects = new List<GameObject>();
        private void Update()
        {
            MakeTransparent();
        }
        
        private void MakeTransparent()
        {
            _direction = player.position - transform.position;
            float maxDistance = _direction.magnitude;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, _direction, maxDistance, targetLayer);
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    
                    if (!_hitObjects.Contains(hitObject))
                    {
                        _hitObjects.Add(hitObject);
                    }
                    MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        Color currentColor = material.color;
                        if (currentColor.a > 0.5f) 
                        {
                            material.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f), 0.3f);
                        }
                    }
                }
            }
            for (int i = _hitObjects.Count - 1; i >= 0; i--)
            {
                if (!Array.Exists(hits, h => h.collider.gameObject == _hitObjects[i]))
                {
                    MeshRenderer renderer = _hitObjects[i].GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        material.DOColor(new Color(material.color.r, material.color.g, material.color.b, 1f), 0.3f);
                    }
                    _hitObjects.RemoveAt(i);
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(player.position, transform.position - player.position);
        }
    }
}
