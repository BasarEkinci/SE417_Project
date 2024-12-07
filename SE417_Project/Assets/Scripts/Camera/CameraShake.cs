using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineFollow cinemachineFollow;

        private CinemachineBasicMultiChannelPerlin _perlin;
        private float _timer;

        private void Awake()
        {
            _perlin = cinemachineFollow.GetComponent<CinemachineBasicMultiChannelPerlin>();   
        }

        public void ShakeCamera(float intensity,float time)
        {
            _perlin.AmplitudeGain = intensity;
            _timer = time;
        }
        
        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _perlin.AmplitudeGain = 0;
                }
            }
        }
    }
}
