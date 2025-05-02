using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image fillImage;
        [SerializeField] private Gradient colorGradient;

        public void SetHealth(float normalizedHealth)
        {
            healthSlider.value = normalizedHealth;
            fillImage.color = colorGradient.Evaluate(normalizedHealth);
        }
    }
}