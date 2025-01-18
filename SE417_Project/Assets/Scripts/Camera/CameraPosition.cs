using System;
using Inputs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Camera
{
    public class CameraPosition : MonoBehaviour
    {
        [SerializeField] private CinemachineFollow followCam;
        [SerializeField] private LayerDetector layerDetector;
        private const float DefaultPos = 6f;
        private const float CrouchPos = 0f;
        
        private bool _isPlayerCrouching;

        private void OnEnable()
        {
            InputHandler.Instance.PlayerInputs.Player.Crouch.performed += OnCrouchPerformed;
            _isPlayerCrouching = false;
            followCam.FollowOffset.y = DefaultPos;
        }

        private void Update()
        {
            if (_isPlayerCrouching)
            {
                followCam.FollowOffset.y = Mathf.Lerp(followCam.FollowOffset.y,CrouchPos,Time.deltaTime * 2);
            }
            else
            {
                followCam.FollowOffset.y = Mathf.Lerp(followCam.FollowOffset.y,DefaultPos,Time.deltaTime * 2);
            }
        }

        private void OnDisable()
        {
            InputHandler.Instance.PlayerInputs.Player.Crouch.performed -= OnCrouchPerformed;
        }

        private void OnCrouchPerformed(InputAction.CallbackContext obj)
        {
            if (layerDetector.IsLayerDetected())
            {
                return;
            }
            _isPlayerCrouching = !_isPlayerCrouching;
        }
    }
}
