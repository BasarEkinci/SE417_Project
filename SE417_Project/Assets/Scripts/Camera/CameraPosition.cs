using Inputs;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraPosition : MonoBehaviour
    {
        [SerializeField] private CinemachineFollow followCam;
        private float _defaultPos = 6f;
        private float _crouchPos = 0f;
        
        private bool _isPlayerCrouching;

        private void Update()
        {
            if (InputHandler.Instance.GetCrouchInput())
            {
                _isPlayerCrouching = !_isPlayerCrouching;
            }

            if (_isPlayerCrouching)
            {
                followCam.FollowOffset.y = _crouchPos;
            }
            else
            {
                followCam.FollowOffset.y = _defaultPos;
            }
        }
    }
}
