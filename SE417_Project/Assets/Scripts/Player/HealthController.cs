using System.Collections.Generic;
using Camera;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthController : MonoBehaviour
    {
        public bool IsInjured => _currentHealth < injuredHealth;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => maxHealth;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private int maxHealth;
        [SerializeField] private int injuredHealth;
        [SerializeField] private List<Image> medkitIcons;
        
        private CameraShake _cameraShake;
        private int _currentHealth;
        private int _medkitCount = 0;
        
        private void Awake()
        {
            _cameraShake = GetComponent<CameraShake>();
        }

        private void Start()
        {
            foreach (var icon in medkitIcons)
            {
                icon.color = new Color(255, 255, 255, 0.2f);
            }
            _currentHealth = maxHealth;
            healthBar.InitializeValues(maxHealth);
        }
        private void Update()
        {
            healthBar.UpdateValues(_currentHealth);
        }

        public void Damage(int damageAmount)
        {
            if (_currentHealth > 0)
            {
                _currentHealth -= damageAmount;
                _cameraShake.ShakeCamera();
            }
        }

        public void UseMedkit()
        {
            if (_medkitCount == 0) return;
            _medkitCount--;
            medkitIcons[_medkitCount].color = new Color(255, 255, 255, 0.2f);
            Heal(20);
        }
        
        public void Heal(int healAmount)
        {
            if (_currentHealth >= maxHealth)
            {
                medkitIcons[_medkitCount].color = Color.white;
                _medkitCount++;
            }
            if (_currentHealth < maxHealth)
            {
                _currentHealth += healAmount;
            }
        }
    }
}
