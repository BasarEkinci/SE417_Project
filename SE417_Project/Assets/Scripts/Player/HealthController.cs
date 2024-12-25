using System;
using UI;
using UnityEngine;

namespace Player
{
    public class HealthController : MonoBehaviour
    {
        public bool IsInjured => _currentHealth < injuredHealth;
        public int CurrentHealth => _currentHealth;
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private int maxHealth;
        [SerializeField] private int injuredHealth;
                
        private int _currentHealth;

        public void Initialize()
        {
            _currentHealth = maxHealth;
            healthBar.InitializeValues(maxHealth);
        }

        private void Update()
        {
            healthBar.UpdateValues(_currentHealth);
        }

        public void GetDamage(int damageAmount)
        {
            if (_currentHealth > 0)
            {
                _currentHealth -= damageAmount;
            }
        }
        public void Heal(int healAmount)
        {
            if (_currentHealth < maxHealth)
            {
                _currentHealth += healAmount;
            }
            healthBar.UpdateValues(_currentHealth);
        }
    }
}
