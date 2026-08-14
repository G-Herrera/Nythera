using UnityEngine;
using UnityEngine.UI;

public class HUDHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider healthSlider;

    private SistemaVida sistemaVida;

    public void Initialize(SistemaVida vida)
    {
        if (sistemaVida != null)
            sistemaVida.OnVidaChanged -= UpdateHealthBar;

        sistemaVida = vida;

        if (sistemaVida == null)
        {
            Debug.LogError("HUDHealthBar recibió un SistemaVida nulo.");
            return;
        }

        sistemaVida.OnVidaChanged += UpdateHealthBar;

        UpdateHealthBar(
            sistemaVida.ObtenerVidaActual(),
            sistemaVida.ObtenerVidaMaxima()
        );
    }

    private void OnDisable()
    {
        if (sistemaVida != null)
            sistemaVida.OnVidaChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}