using UnityEngine;

public class Llave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Busca el gestor en la escena y le avisa
            GestorLlaves gestor = FindObjectOfType<GestorLlaves>();
            if (gestor != null)
            {
                gestor.RecogerLlave();
                Destroy(gameObject); // Destruye la llave recolectada
            }
        }
    }
}