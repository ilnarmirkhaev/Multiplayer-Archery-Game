using UnityEngine;
using UnityEngine.UI;

namespace Photon
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text text;

        private int _max;

        public void Init(int maxHealth)
        {
            _max = maxHealth;
            slider.minValue = 0;
            slider.maxValue = maxHealth;
            UpdateHealth(maxHealth);
        }

        public void UpdateHealth(int currentHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, _max);
            slider.value = currentHealth;
            text.text = currentHealth.ToString();
        }
    }
}