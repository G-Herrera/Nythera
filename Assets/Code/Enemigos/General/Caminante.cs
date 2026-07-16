using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class EnemigoCaminante : MonoBehaviour, IDamageable
{
    private enum Estado { Patrullando, Persiguiendo }
    private Estado estadoActual;

    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 4f;
    private bool siendoEmpujado = false; // Bloquea el movimiento al recibir golpe

    [Header("Detección del Jugador")]
    public float rangoVision = 5f;
    public float rangoAtaque = 1f;
    private Transform jugador;

    [Header("Ruta de Patrullaje")]
    public Transform[] puntosPatrullaje;
    private int indicePuntoActual = 0;

    [Header("Configuración de Ataque")]
    public float dañoAtaque = 10f;
    public float frecuenciaAtaque = 1f;
    private float tiempoUltimoAtaque;

    private Rigidbody2D rb;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        estadoActual = Estado.Patrullando;
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (jugador == null || siendoEmpujado) return; // Si está siendo empujado, no se mueve solo

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        switch (estadoActual)
        {
            case Estado.Patrullando:
                Patrullar();
                if (distanciaAlJugador <= rangoVision) estadoActual = Estado.Persiguiendo;
                break;
            case Estado.Persiguiendo:
                if (distanciaAlJugador > rangoVision) estadoActual = Estado.Patrullando;
                else Perseguir();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SistemaVida vidaJugador = collision.gameObject.GetComponent<SistemaVida>();
            if (vidaJugador != null)
            {
                // Enviamos el daño al chocar
                AttackData ataque = new AttackData((int)dañoAtaque, 0f, Vector2.zero);
                vidaJugador.RecibirDano(ataque);
            }
        }
    }

    public void TakeDamage(AttackData data)
    {
        // 1. Restar vida
        GetComponent<SistemaVida>().RecibirDano(data);

        // 2. Empujar
        GetComponent<Knockback>().AplicarEmpuje(data);

        // 3. Pausar movimiento automático para que se note el empuje
        StartCoroutine(PausaMovimiento(0.25f));
    }

    private IEnumerator PausaMovimiento(float tiempo)
    {
        siendoEmpujado = true;
        yield return new WaitForSeconds(tiempo);
        siendoEmpujado = false;
    }

    private void Patrullar()
    {
        if (puntosPatrullaje.Length == 0) return;
        Transform destinoActual = puntosPatrullaje[indicePuntoActual];
        Vector2 direccion = (destinoActual.position - transform.position).normalized;
        rb.velocity = new Vector2(direccion.x * velocidadPatrulla, rb.velocity.y);
        DeterminarDireccionMirada(direccion.x);

        if (Mathf.Abs(transform.position.x - destinoActual.position.x) < 0.5f)
        {
            indicePuntoActual = (indicePuntoActual + 1) % puntosPatrullaje.Length;
        }
    }

    private void Perseguir()
    {
        float distanciaX = Mathf.Abs(jugador.position.x - transform.position.x);
        if (distanciaX <= rangoAtaque)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (Time.time >= tiempoUltimoAtaque + frecuenciaAtaque) AtacarJugador();
        }
        else
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb.velocity = new Vector2(direccion.x * velocidadPersecucion, rb.velocity.y);
            DeterminarDireccionMirada(direccion.x);
        }
    }

    private void AtacarJugador()
    {
        tiempoUltimoAtaque = Time.time;
        SistemaVida vidaJugador = jugador.GetComponent<SistemaVida>();
        if (vidaJugador != null)
        {
            Vector2 direccionAtaque = (jugador.position - transform.position).normalized;
            AttackData ataque = new AttackData((int)dañoAtaque, 5f, direccionAtaque);
            vidaJugador.RecibirDano(ataque);
        }
    }

    private void DeterminarDireccionMirada(float direccionX)
    {
        if ((direccionX > 0 && !mirandoDerecha) || (direccionX < 0 && mirandoDerecha)) Voltear();
    }

    private void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, rangoVision);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}