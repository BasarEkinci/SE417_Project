using Camera;
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
        
        private CameraShake _cameraShake;
        private int _currentHealth;

        private void Awake()
        {
            _cameraShake = GetComponent<CameraShake>();
        }

        private void Start()
        {
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
        public void Heal(int healAmount)
        {
            if (_currentHealth < maxHealth)
            {
                _currentHealth += healAmount;
            }
        }
    }
}
