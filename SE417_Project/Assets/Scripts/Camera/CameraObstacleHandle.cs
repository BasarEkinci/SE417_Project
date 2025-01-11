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
            _direction = player.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _direction, out hit, _direction.magnitude, targetLayer))
            {
                if (hit.collider != null)
                {
                    _hitObject = hit.collider.gameObject;
                    if (!_hitObjects.Contains(_hitObject))
                    {
                        _hitObjects.Add(_hitObject);
                    }
                    _material = _hitObject.GetComponent<Collider>().GetComponent<MeshRenderer>().material;
                    _material.DOColor(new Color(_material.color.r, _material.color.g, _material.color.b, 0.5f), 0.3f);
                }
            }
            else
            {
                foreach (var hitObject in _hitObjects)
                {
                    _material = hitObject.GetComponent<Collider>().GetComponent<MeshRenderer>().material;
                    _material.DOColor(new Color(_material.color.r, _material.color.g, _material.color.b, 1f), 0.3f);
                }
                _hitObjects.Clear();
            }
        }

    }
}
