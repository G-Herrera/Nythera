using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemigoCaminante : MonoBehaviour
{
    private enum Estado { Patrullando, Persiguiendo }
    private Estado estadoActual;

    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 4f;

    [Header("Detección del Jugador")]
    public float rangoVision = 5f;
    public float rangoAtaque = 1f; // Distancia a la que se detiene para atacar
    private Transform jugador;

    [Header("Ruta de Patrullaje")]
    [Tooltip("Añade aquí todos los puntos (GameObjects vacíos) por donde quieres que pase el enemigo.")]
    public Transform[] puntosPatrullaje; // ¡Un arreglo para poner los puntos que quieras!
    private int indicePuntoActual = 0;

    [Header("Configuración de Ataque")]
    public float dañoAtaque = 10f;
    public float frecuenciaAtaque = 1f; // Cada cuánto ataca
    private float tiempoUltimoAtaque;

    private Rigidbody2D rb;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        estadoActual = Estado.Patrullando;

        // Buscamos automáticamente al jugador usando el Tag "Player"
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Calculamos a qué distancia está el jugador
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        switch (estadoActual)
        {
            case Estado.Patrullando:
                Patrullar();

                // Si el jugador entra en su rango de visión, cambia a persecución
                if (distanciaAlJugador <= rangoVision)
                {
                    estadoActual = Estado.Persiguiendo;
                }
                break;

            case Estado.Persiguiendo:
                // Si el jugador sale del rango, vuelve a su patrón de patrullaje
                if (distanciaAlJugador > rangoVision)
                {
                    estadoActual = Estado.Patrullando;
                }
                else
                {
                    Perseguir();
                }
                break;
        }
    }

    private void Patrullar()
    {
        // Si no le asignaste ningún punto en el inspector, que no haga nada para evitar errores
        if (puntosPatrullaje.Length == 0) return;

        Transform destinoActual = puntosPatrullaje[indicePuntoActual];

        // Calculamos la dirección hacia el destino
        Vector2 direccion = (destinoActual.position - transform.position).normalized;
        rb.velocity = new Vector2(direccion.x * velocidadPatrulla, rb.velocity.y);

        DeterminarDireccionMirada(direccion.x);

        // Si ya llegó (o está muy cerca) al punto de patrullaje, pasa al siguiente
        // Pon esta en su lugar:
        if (Mathf.Abs(transform.position.x - destinoActual.position.x) < 0.5f)
        {
            indicePuntoActual++;

            // Si ya llegó al último punto de la lista, reinicia al primero
            if (indicePuntoActual >= puntosPatrullaje.Length)
            {
                indicePuntoActual = 0;
            }
        }
    }

    private void Perseguir()
    {
        float distanciaX = Mathf.Abs(jugador.position.x - transform.position.x);

        if (distanciaX <= rangoAtaque)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            // --- AQUÍ ESTÁ LA LÓGICA DE ATAQUE ---
            if (Time.time >= tiempoUltimoAtaque + frecuenciaAtaque)
            {
                AtacarJugador();
            }
        }
        else
        {
            // Moverse hacia el jugador
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb.velocity = new Vector2(direccion.x * velocidadPersecucion, rb.velocity.y);
            DeterminarDireccionMirada(direccion.x);
        }
    }

    private void AtacarJugador()
    {
        tiempoUltimoAtaque = Time.time;

        // Buscamos el componente SistemaVida en el jugador
        SistemaVida vidaJugador = jugador.GetComponent<SistemaVida>();

        if (vidaJugador != null)
        {
            vidaJugador.RecibirDano(dañoAtaque);
            Debug.Log("¡El enemigo atacó al jugador!");
        }
    }

    private void DeterminarDireccionMirada(float direccionX)
    {
        if (direccionX > 0 && !mirandoDerecha)
        {
            Voltear();
        }
        else if (direccionX < 0 && mirandoDerecha)
        {
            Voltear();
        }
    }

    private void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // Dibujamos guías visuales en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);

        // Dibuja una línea celeste que conecta la ruta de patrullaje en el editor
        if (puntosPatrullaje != null && puntosPatrullaje.Length > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < puntosPatrullaje.Length - 1; i++)
            {
                if (puntosPatrullaje[i] != null && puntosPatrullaje[i + 1] != null)
                {
                    Gizmos.DrawLine(puntosPatrullaje[i].position, puntosPatrullaje[i + 1].position);
                }
            }
            // Cierra el ciclo dibujando una línea del último punto al primero
            if (puntosPatrullaje[0] != null && puntosPatrullaje[puntosPatrullaje.Length - 1] != null)
            {
                Gizmos.DrawLine(puntosPatrullaje[puntosPatrullaje.Length - 1].position, puntosPatrullaje[0].position);
            }
        }
    }
}