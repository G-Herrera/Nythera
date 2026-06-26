using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Necesario para eventos personalizados en el Inspector

public class SistemaVida : MonoBehaviour
{
    [Header("Estadísticas de Vida")]
    // El jugador empieza con 100 pts de vida, los enemigos pueden ajustarse en el inspector
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Estado de Combate")]
    public bool esInvulnerable = false;

    [Header("Eventos (Asignar en Inspector)")]
    // Aquí conectaremos qué pasa cuando recibe daño (ej. parpadeo rojo)
    public UnityEvent OnRecibirDano;
    // Aquí conectaremos qué pasa cuando muere (ej. jugador reaparece, enemigo desaparece)
    public UnityEvent OnMorir;

    void Start()
    {
        // Al iniciar, la vida actual siempre se llena al máximo
        vidaActual = vidaMaxima;
    }

    // Método universal para recibir daño
    public void RecibirDano(float cantidad)
    {
        // Si el personaje está en medio del dash y tiene invulnerabilidad, ignoramos el daño
        if (esInvulnerable) return;

        vidaActual -= cantidad;

        // Disparamos el evento de recibir daño para reproducir sonidos o animaciones
        OnRecibirDano?.Invoke();

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
        }
    }

    // Método universal para curarse (Ideal para las zonas de descanso)
    public void Curar(float cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima; // Evita que la vida supere el máximo
        }
    }

    // Método que el script del PlayerController puede llamar al hacer Dash
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
        // Dispara el evento de muerte
        OnMorir?.Invoke();
    }

    // Método extra opcional por si necesitas consultar cuánta vida tiene desde otro script
    public float ObtenerVidaActual()
    {
        return vidaActual;
    }
}
