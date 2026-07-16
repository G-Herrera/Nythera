using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public float velocidad = 2f;
    public float distancia = 5f;
    public bool movimientoVertical = false;

    private Vector3 posicionInicial;

    void Start() { posicionInicial = transform.position; }

    void Update()
    {
        float movimiento = Mathf.PingPong(Time.time * velocidad, distancia);
        if (movimientoVertical)
            transform.position = posicionInicial + new Vector3(0, movimiento, 0);
        else
            transform.position = posicionInicial + new Vector3(movimiento, 0, 0);
    }

    // --- Lógica para "pegar" al jugador ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hace al jugador hijo de la plataforma
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Libera al jugador cuando salta o se baja
            collision.transform.SetParent(null);
        }
    }
}