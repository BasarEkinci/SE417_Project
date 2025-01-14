using Inputs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    public class CameraPosition : MonoBehaviour
    {
        [SerializeField] private CinemachineFollow followCam;
        private const float DefaultPos = 6f;
        private const float CrouchPos = 0f;
        
        private bool _isPlayerCrouching;

        private void OnEnable()
        {
            InputHandler.Instance.PlayerInputs.Player.Crouch.performed += OnCrouchPerformed;
        }
        private void OnCrouchPerformed(InputAction.CallbackContext obj)
        {
            _isPlayerCrouching = !_isPlayerCrouching;
            if (_isPlayerCrouching)
            {
                followCam.FollowOffset.y = Mathf.Lerp(followCam.FollowOffset.y,CrouchPos,Time.deltaTime *2);
            }
            else
            {
                followCam.FollowOffset.y = Mathf.Lerp(followCam.FollowOffset.y,DefaultPos,Time.deltaTime*2);
            }
        }
    }
}
