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
        // 1. Si choca con la espada, se destruye
        if (collision.CompareTag("Sword") || collision.name.Contains("Sword"))
        {
            Destroy(gameObject);
            return;
        }

        // 2. Si choca con el jugador
        if (collision.CompareTag("Player"))
        {
            SistemaVida vida = collision.GetComponent<SistemaVida>();
            if (vida != null)
            {
                AttackData ataque = new AttackData(dano, 0f, Vector2.zero);
                vida.RecibirDano(ataque);
            }

            // Aplicamos el efecto directamente buscando los componentes en el jugador
            GameObject jugadorObj = collision.gameObject;

            // Buscamos el script de movimiento
            Behaviour scriptMovimiento = jugadorObj.GetComponent("PlayerController") as Behaviour;
            if (scriptMovimiento == null)
                scriptMovimiento = jugadorObj.GetComponent("MovimientoJugador") as Behaviour;

            // Buscamos el SpriteRenderer para el color
            SpriteRenderer spritePlayer = jugadorObj.GetComponentInChildren<SpriteRenderer>();

            // Ejecutamos la congelación usando un componente temporal o estático que no muera con la lanza,
            // o delegamos la tarea ejecutando una corrutina respaldada en el GameObject del jugador:
            JugadorEfectosTemporales manejadorEfectos = jugadorObj.GetComponent<JugadorEfectosTemporales>();
            if (manejadorEfectos == null)
            {
                manejadorEfectos = jugadorObj.AddComponent<JugadorEfectosTemporales>();
            }
            manejadorEfectos.IniciarCongelamiento(scriptMovimiento, spritePlayer, tiempoCongelamiento);

            Destroy(gameObject); // La lanza se destruye segura sin cortar el efecto
        }
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}

// ==========================================
// CLASE AUXILIAR INTERNA (Todo en el mismo archivo)
// ==========================================
public class JugadorEfectosTemporales : MonoBehaviour
{
    public void IniciarCongelamiento(Behaviour scriptMovimiento, SpriteRenderer spritePlayer, float duracion)
    {
        StartCoroutine(RutinaCongelar(scriptMovimiento, spritePlayer, duracion));
    }

    private IEnumerator RutinaCongelar(Behaviour scriptMovimiento, SpriteRenderer spritePlayer, float duracion)
    {
        // Desactivar movimiento
        if (scriptMovimiento != null) scriptMovimiento.enabled = false;

        Rigidbody2D rbPlayer = GetComponent<Rigidbody2D>();
        if (rbPlayer != null) rbPlayer.velocity = Vector2.zero;

        // Cambiar color
        Color colorOriginal = Color.white;
        if (spritePlayer != null)
        {
            colorOriginal = spritePlayer.color;
            spritePlayer.color = new Color(0.4f, 0.8f, 1f); // Azul hielo
        }

        // Esperar el tiempo exacto (vive en el jugador, por lo que nunca se interrumpe)
        yield return new WaitForSeconds(duracion);

        // Restaurar
        if (spritePlayer != null) spritePlayer.color = colorOriginal;
        if (scriptMovimiento != null) scriptMovimiento.enabled = true;

        // Se autodestruye este componente temporal al terminar
        Destroy(this);
    }
}