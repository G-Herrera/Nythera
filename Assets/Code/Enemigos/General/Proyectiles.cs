using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    public float velocidad = 10f;
    public float dano = 10f;
    public float tiempoVida = 3f; // Para que desaparezca si no golpea nada

    void Start()
    {
        Destroy(gameObject, tiempoVida); // Se destruye solo si no pega con nada
    }

    void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca al jugador, le hace daño
        if (collision.CompareTag("Player"))
        {
            SistemaVida vida = collision.GetComponent<SistemaVida>();
            if (vida != null) vida.RecibirDano(dano);
            Destroy(gameObject);
        }
        // Si toca el suelo o pared, desaparece
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}