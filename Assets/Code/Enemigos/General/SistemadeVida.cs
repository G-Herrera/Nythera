using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SistemaVida : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Estado")]
    public bool esInvulnerable = false;

    [Header("Eventos")]
    public UnityEvent OnRecibirDano;
    public UnityEvent OnMorir;

    void Awake()
    {
        vidaActual = vidaMaxima;
    }

    // Método universal para recibir daño
    public void RecibirDano(AttackData data)
    {
        if (esInvulnerable) return;

        vidaActual -= data.Damage;

        // Disparamos el evento (aquí conectas el parpadeo rojo o efectos)
        OnRecibirDano?.Invoke();

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
        }
    }

    // Método universal para curarse
    public void Curar(float cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
    }

    // Método para la invulnerabilidad (usado en el Dash del Player)
    public void ActivarInvulnerabilidadTemporal(float tiempo)
    {
        StartCoroutine(RutinaInvulnerabilidad(tiempo));
    }

    private IEnumerator RutinaInvulnerabilidad(float tiempo)
    {
        esInvulnerable = true;
        yield return new WaitForSeconds(tiempo);
        esInvulnerable = false;
    }

    private void Morir()
    {
        // Esto dispara lo que configuraste en el Inspector (desactivar objeto, animaciones, etc.)
        OnMorir?.Invoke();
    }

    public float ObtenerVidaActual() => vidaActual;
}