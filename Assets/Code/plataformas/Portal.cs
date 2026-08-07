using UnityEngine;
using UnityEngine.SceneManagement; // <-- Añadimos esto para forzar la carga directa

public class PortalNivel : MonoBehaviour
{
    public string nombreSiguienteNivel = "Carlos";
    private bool jugadorCerca = false;

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Intentando cargar directamente la escena: " + nombreSiguienteNivel);

            // Forzamos la carga directa por código de Unity sin pasar por el GestorNiveles
            SceneManager.LoadScene(nombreSiguienteNivel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
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