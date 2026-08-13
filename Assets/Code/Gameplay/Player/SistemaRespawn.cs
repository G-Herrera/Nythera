using UnityEngine;
using System.Collections;

public class SistemaRespawn : MonoBehaviour
{
    private Vector2 puntoRespawn;
    private Rigidbody2D rb;
    private SpriteRenderer spritePlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spritePlayer = GetComponentInChildren<SpriteRenderer>();

        // Al iniciar el nivel, el punto de respawn inicial es donde aparece el jugador por primera vez
        puntoRespawn = transform.position;
    }

    // Método para actualizar el punto de respawn (puedes llamarlo desde una bandera, un checkpoint o un trigger)
    public void ActualizarCheckpoint(Vector2 nuevaPosicion)
    {
        puntoRespawn = nuevaPosicion;
        Debug.Log("¡Checkpoint guardado!");
    }

    // Este método se ejecuta cuando el jugador muere (puedes llamarlo desde tu script de vida)
    public void MorirYReaparecer()
    {
        StartCoroutine(RutinaRespawn());
    }

    private IEnumerator RutinaRespawn()
    {
        // 1. Desactivar movimiento y física temporalmente para que no caiga mientras se reinicia
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false; // Apaga la física
        }

        // Opcional: Ocultar o parpadear el sprite un momento
        if (spritePlayer != null)
        {
            spritePlayer.enabled = false;
        }

        // Espera un breve instante (efecto de muerte / pausa)
        yield return new WaitForSeconds(0.5f);

        // 2. Teletransportar al jugador al último punto de respawn guardado
        transform.position = puntoRespawn;

        // 3. Restaurar físicas, gráficos y vida (si tu sistema de vida lo requiere)
        SistemaVida vida = GetComponent<SistemaVida>();
        if (vida != null)
        {
            // Si tienes un método para curar al máximo, ponlo aquí, por ejemplo:
            // vida.CurarAlMaximo();
        }

        if (spritePlayer != null)
        {
            spritePlayer.enabled = true;
        }

        if (rb != null)
        {
            rb.simulated = true; // Reactiva la física
        }
    }
}