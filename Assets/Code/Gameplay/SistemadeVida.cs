using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

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
    public event Action<float, float> OnVidaChanged;

    void Awake()
    {
        vidaActual = vidaMaxima;
    }

    // Método universal para recibir daño
    public void RecibirDano(AttackData data)
    {
        if (esInvulnerable) return;

        vidaActual -= data.Damage;

        if (vidaActual < 0)
            vidaActual = 0;

        OnVidaChanged?.Invoke(vidaActual, vidaMaxima);

        OnRecibirDano?.Invoke();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // Método universal para curarse
    public void Curar(float cantidad)
    {
        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
            vidaActual = vidaMaxima;

        OnVidaChanged?.Invoke(vidaActual, vidaMaxima);
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

    public float ObtenerVidaMaxima() => vidaMaxima;
    public float ObtenerVidaActual() => vidaActual;
}