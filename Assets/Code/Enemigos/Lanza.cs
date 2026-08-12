using UnityEngine;
using System.Collections;

public class ProyectilLanza : MonoBehaviour
{
    public float velocidad = 10f;
    public int dano = 15;
    public float tiempoVida = 3f;
    private float direccionX = 1f;

    [Header("Efecto de Congelamiento")]
    public float tiempoCongelamiento = 1.5f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    public void ConfigurarDireccion(float escalaXChef)
    {
        if (escalaXChef < 0)
        {
            direccionX = -1f;
            Vector3 escalaActual = transform.localScale;
            transform.localScale = new Vector3(-Mathf.Abs(escalaActual.x), escalaActual.y, escalaActual.z);
        }
        else
        {
            direccionX = 1f;
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * direccionX * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. SI COLISIONA CON LA ESPADA O EL ARMA DEL JUGADOR
        // (Asegúrate de que tu espada tenga el Tag "Sword" o el nombre con el que la detectes)
        if (collision.CompareTag("Sword") || collision.name.Contains("Sword"))
        {
            // Opcional: Aquí puedes poner efectos de partículas de destrucción si gustas
            Destroy(gameObject); // Se destruye y se anula cualquier efecto
            return;
        }

        // 2. SI COLISIONA CON EL JUGADOR (Daño y congelamiento normal)
        if (collision.CompareTag("Player"))
        {
            SistemaVida vida = collision.GetComponent<SistemaVida>();

            if (vida != null)
            {
                AttackData ataque = new AttackData(dano, 0f, Vector2.zero);
                vida.RecibirDano(ataque);
            }

            StartCoroutine(CongelarJugadorTemporalmente(collision.gameObject));
            Destroy(gameObject);
        }
        // 3. SI COLISIONA CON EL SUELO
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CongelarJugadorTemporalmente(GameObject jugador)
    {
        Behaviour scriptMovimiento = jugador.GetComponent("PlayerController") as Behaviour;
        if (scriptMovimiento == null)
            scriptMovimiento = jugador.GetComponent("MovimientoJugador") as Behaviour;

        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = false;
        }

        Rigidbody2D rbPlayer = jugador.GetComponent<Rigidbody2D>();
        if (rbPlayer != null)
        {
            rbPlayer.velocity = Vector2.zero;
        }

        SpriteRenderer spritePlayer = jugador.GetComponentInChildren<SpriteRenderer>();
        Color colorOriginal = Color.white;

        if (spritePlayer != null)
        {
            colorOriginal = spritePlayer.color;
            spritePlayer.color = new Color(0.4f, 0.8f, 1f);
        }

        yield return new WaitForSeconds(tiempoCongelamiento);

        if (spritePlayer != null)
        {
            spritePlayer.color = colorOriginal;
        }

        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = true;
        }
    }
}