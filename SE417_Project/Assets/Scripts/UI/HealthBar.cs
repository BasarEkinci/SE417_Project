using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private float lerpSpeed;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider easeHealthBar;

        public void InitializeValues(int healthAmount)
        {
            healthBar.maxValue = healthAmount;
            easeHealthBar.maxValue = healthAmount;
        }
        
        public void UpdateValues(int health)
        {
            healthBar.value = health;
            easeHealthBar.value = Mathf.Lerp(easeHealthBar.value, health, lerpSpeed);
        }
    }
}
