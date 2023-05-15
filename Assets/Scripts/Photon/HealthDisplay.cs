using UnityEngine;
using UnityEngine.UI;

namespace Photon
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text text;

        public void Init(int maxHealth)
        {
            slider.maxValue = maxHealth;
            UpdateHealth(maxHealth);
        }

        public void UpdateHealth(int currentHealth)
        {
            slider.value = currentHealth;
            text.text = currentHealth.ToString();
        }
    }
}