using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    public float velocidad = 10f;
    public int dano = 10; // Cambiado a int para que coincida con AttackData
    public float tiempoVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SistemaVida vida = collision.GetComponent<SistemaVida>();

            if (vida != null)
            {
                // Creamos el paquete. Usamos 'dano' (int) y 0f para el empuje
                // La dirección la ponemos como Vector2.zero porque es un proyectil 
                // (a menos que quieras que el proyectil empuje al jugador, ahí pondrías una dirección)
                AttackData ataque = new AttackData(dano, 0f, Vector2.zero);

                vida.RecibirDano(ataque);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}