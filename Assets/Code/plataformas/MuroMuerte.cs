using UnityEngine;

public class MuroMortal : MonoBehaviour
{
    [Header("Configuración")]
    public int dañoAlTocar = 999; // Un valor alto para asegurar la muerte

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que toca es el jugador
        if (collision.CompareTag("Player"))
        {
            SistemaVida vidaJugador = collision.GetComponent<SistemaVida>();

            if (vidaJugador != null)
            {
                // Creamos un paquete de daño que sea mortal
                // Usamos 0 de empuje porque el jugador morirá inmediatamente
                AttackData ataqueMortal = new AttackData(dañoAlTocar, 0f, Vector2.zero);

                // Aplicamos el daño al jugador
                vidaJugador.RecibirDano(ataqueMortal);

                Debug.Log("¡El jugador tocó el muro de la muerte!");
            }
        }
    }
}