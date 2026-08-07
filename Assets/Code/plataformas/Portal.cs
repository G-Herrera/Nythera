using UnityEngine;

public class PortalNivel : MonoBehaviour
{
    [SerializeField] private string nombreSiguienteNivel; // Escribe "Carlos" aquí
    private bool jugadorCerca = false;

    private void Update()
    {
        // Si el jugador está cerca Y presiona la tecla 'E'
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            // Usamos tu Gestor Global para ir a la escena
            GestorNiveles.instancia.IrANivel(nombreSiguienteNivel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
            // Opcional: Aquí podrías activar un icono de "Presiona E"
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}