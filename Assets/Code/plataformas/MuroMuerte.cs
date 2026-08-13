using UnityEngine;

public class MuroMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si lo que tocó el muro es el jugador
        if (collision.CompareTag("Player"))
        {
            // 1. Buscamos el script de respawn que pusiste en el jugador
            SistemaRespawn respawn = collision.GetComponent<SistemaRespawn>();

            if (respawn != null)
            {
                // Llamamos a la función para que reaparezca sin reiniciar el nivel
                respawn.MorirYReaparecer();
            }
            else
            {
                // Por si acaso al jugador le faltara el script, lo mandamos a unas coordenadas seguras o de inicio
                Debug.LogWarning("El jugador no tiene el script SistemaRespawn asignado.");
                collision.transform.position = new Vector2(0f, 0f);
            }

            // Opcional: Si quieres que además de reaparecer pierda vida, puedes llamar a su sistema de vida aquí:
            // SistemaVida vida = collision.GetComponent<SistemaVida>();
            // if (vida != null) { vida.RecibirDano(...); }
        }
    }
}