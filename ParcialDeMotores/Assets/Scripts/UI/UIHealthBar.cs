using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHealthBar : MonoBehaviour
    {
        [Header("Componentes de la barra de vida")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Gradient _colorGradient;

        /// <summary>
        /// Actualiza el valor y el color de la barra de vida.
        /// </summary>
        /// <param name="normalizedHealth">Valor entre 0 y 1 que representa la salud.</param>
        public void SetHealth(float normalizedHealth)
        {
            _healthSlider.value = normalizedHealth;
            _fillImage.color = _colorGradient.Evaluate(normalizedHealth);
        }
    }
}