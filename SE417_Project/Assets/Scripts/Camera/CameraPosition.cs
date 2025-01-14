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
                followCam.FollowOffset.y = Mathf.Lerp(followCam.FollowOffset.y,_crouchPos,Time.deltaTime);
            }
            else
            {
                followCam.FollowOffset.y = Mathf.Lerp(followCam.FollowOffset.y,_defaultPos,Time.deltaTime);
            }
        }
    }
}
