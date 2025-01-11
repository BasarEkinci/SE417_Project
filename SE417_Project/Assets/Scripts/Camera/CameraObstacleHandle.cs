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
        private Color _currentColor;

        private void Start()
        {
            _currentColor = new Color(255, 255, 0, 255);
        }

        private void Update()
        {
            _direction = player.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _direction, out hit, _direction.magnitude, targetLayer))
            {
                if (hit.collider != null)
                {
                    _hitObject = hit.collider.gameObject;
                    _material = _hitObject.GetComponent<Collider>().GetComponent<MeshRenderer>().material;
                    _currentColor = _material.color;
                    _material.DOColor(new Color(_currentColor.r, _currentColor.g, _currentColor.b, 0.5f), 0.3f);
                }
            }
            else
            {
                _material.DOColor(_currentColor, 0.3f);
                _material = null;
                _hitObject = null;
            }
        }

    }
}
