using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SistemaVida))]
public class EnemigoVeloz : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 6f;
    public float dañoAlJugador = 10f;

    [Header("Ruta (Opcional)")]
    public Transform[] puntosPatrullaje;
    private int indicePuntoActual = 0;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Solo patrulla si tiene puntos asignados
        if (puntosPatrullaje != null && puntosPatrullaje.Length > 0)
        {
            Transform destino = puntosPatrullaje[indicePuntoActual];
            Vector2 direccion = (destino.position - transform.position).normalized;

            rb.velocity = new Vector2(direccion.x * velocidad, rb.velocity.y);

            if (Vector2.Distance(transform.position, destino.position) < 0.5f)
            {
                indicePuntoActual = (indicePuntoActual + 1) % puntosPatrullaje.Length;
            }
        }
    }

    // Lógica de daño al tocar al jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SistemaVida vidaJugador = collision.gameObject.GetComponent<SistemaVida>();
            if (vidaJugador != null)
            {
                vidaJugador.RecibirDano(dañoAlJugador);
            }
        }
    }
}