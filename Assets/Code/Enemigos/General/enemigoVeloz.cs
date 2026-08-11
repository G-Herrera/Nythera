using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class EnemigoVeloz : MonoBehaviour, IDamageable
{
    [Header("Configuración")]
    public float velocidad = 6f;
    public int dañoAlJugador = 10;
    public float rangoVisionPersecucion = 15f;
    private bool siendoEmpujado = false;

    [Header("Ruta (Opcional)")]
    public Transform[] puntosPatrullaje;
    private int indicePuntoActual = 0;

    private Rigidbody2D rb;
    private Transform jugador;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (siendoEmpujado) return;

        // 1. Validar si el jugador está vivo
        bool jugadorVivo = false;
        if (jugador != null)
        {
            SistemaVida vidaPlayer = jugador.GetComponent<SistemaVida>();
            if (vidaPlayer != null && vidaPlayer.ObtenerVidaActual() > 0)
            {
                jugadorVivo = true;
            }
        }

        // 2. Si el jugador murió o no existe, el enemigo se queda completamente quieto
        if (!jugadorVivo)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // 3. Ir directo al jugador
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoVisionPersecucion)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb.velocity = new Vector2(direccion.x * velocidad, rb.velocity.y);

            // Rota hacia el jugador
            ActualizarGiro(direccion.x);
        }
        else if (puntosPatrullaje != null && puntosPatrullaje.Length > 0)
        {
            Transform destino = puntosPatrullaje[indicePuntoActual];
            Vector2 direccionPatrulla = (destino.position - transform.position).normalized;
            rb.velocity = new Vector2(direccionPatrulla.x * velocidad, rb.velocity.y);

            // Rota hacia la patrulla
            ActualizarGiro(direccionPatrulla.x);

            if (Vector2.Distance(transform.position, destino.position) < 0.5f)
            {
                indicePuntoActual = (indicePuntoActual + 1) % puntosPatrullaje.Length;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void ActualizarGiro(float direccionX)
    {
        if (direccionX > 0.05f)
        {
            mirandoDerecha = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direccionX < -0.05f)
        {
            mirandoDerecha = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(AttackData data)
    {
        GetComponent<SistemaVida>().RecibirDano(data);
        GetComponent<Knockback>().AplicarEmpuje(data);
        StartCoroutine(PausaMovimiento(0.2f));
    }

    private IEnumerator PausaMovimiento(float tiempo)
    {
        siendoEmpujado = true;
        yield return new WaitForSeconds(tiempo);
        siendoEmpujado = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SistemaVida vidaJugador = collision.gameObject.GetComponent<SistemaVida>();
            if (vidaJugador != null)
            {
                AttackData ataqueAlJugador = new AttackData(dañoAlJugador, 0f, Vector2.zero);
                vidaJugador.RecibirDano(ataqueAlJugador);
            }
        }
    }
}