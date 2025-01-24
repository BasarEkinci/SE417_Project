using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;

namespace AI
{
    public class ZurgGun : MonoBehaviour
    {
        [SerializeField] private float bulletForce;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private List<Transform> gunBarrels;
        [SerializeField] private Transform detector;
        [SerializeField] private AudioClip gunSound;
        
        private AudioSource _audioSource;
        private bool _isPlayerDetected;
        private int _currentGunBarrelIndex;
        private bool _canShoot;

        private void Awake()
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

        private void OnEnable()
        {
            _canShoot = true;
            CoreGameSignals.Instance.OnCompleteLevel += ()=> _canShoot = false;
            CoreGameSignals.Instance.OnPlayerDie += ()=> _canShoot = false;
            CoreGameSignals.Instance.OnPlayerHide += ()=> _canShoot = false;
            CoreGameSignals.Instance.OnPlayerWakeUp += EnableGun;
            CoreGameSignals.Instance.OnPauseGame += (condition) => _canShoot = !condition;
            Shoot().Forget();
        } 
        
        private void Update()
        {
            DetectPlayer();
        }
        private void OnDisable()
        {
            CoreGameSignals.Instance.OnCompleteLevel -= ()=> _canShoot = false;
            CoreGameSignals.Instance.OnPlayerDie -= ()=> _canShoot = false;
            CoreGameSignals.Instance.OnPlayerHide -= ()=> _canShoot = false;
            CoreGameSignals.Instance.OnPlayerWakeUp -= EnableGun;
            CoreGameSignals.Instance.OnPauseGame -= (condition) => _canShoot = !condition;
        }
        private void EnableGun()
        {
            EnableGunAsync().Forget();
        }
        private async UniTaskVoid EnableGunAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _canShoot = true;
        }
        private void DetectPlayer()
        {
            _isPlayerDetected = Physics.Raycast(detector.position, detector.forward, 100, layerMask) && _canShoot;
        }
        //Silahın ateş etmesini sağlayan fonksiyon
        private async UniTaskVoid Shoot()
        {
            while (true)
            {
                if (_isPlayerDetected)
                {
                    var bullet = Instantiate(bulletPrefab, gunBarrels[_currentGunBarrelIndex].position, gunBarrels[_currentGunBarrelIndex].rotation);
                    bullet.GetComponent<Rigidbody>().AddForce(gunBarrels[_currentGunBarrelIndex].forward * bulletForce,ForceMode.Impulse);
                    _audioSource.PlayOneShot(gunSound);
                    _currentGunBarrelIndex++;
                    if (_currentGunBarrelIndex >=3)
                    {
                        _currentGunBarrelIndex = 0;
                    }
                }
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            }   
        }
    }
}
