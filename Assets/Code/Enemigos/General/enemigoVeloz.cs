using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class EnemigoVeloz : MonoBehaviour, IDamageable
{
    [Header("Configuración")]
    public float velocidad = 6f;
    public int dañoAlJugador = 10;
    private bool siendoEmpujado = false;

    [Header("Ruta (Opcional)")]
    public Transform[] puntosPatrullaje;
    private int indicePuntoActual = 0;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (siendoEmpujado) return;

        if (puntosPatrullaje != null && puntosPatrullaje.Length > 0)
        {
            Transform destino = puntosPatrullaje[indicePuntoActual];
            Vector2 direccion = (destino.position - transform.position).normalized;
            rb.velocity = new Vector2(direccion.x * velocidad, rb.velocity.y);

            if (Vector2.Distance(transform.position, destino.position) < 0.5f)
            {
                indicePuntoActual = (indicePuntoActual + 1) % puntosPatrullaje.Length;
            }
        }
    }

    public void TakeDamage(AttackData data)
    {
        // ¿Esto se ejecuta? Pon un Debug.Log para saberlo
        Debug.Log("Enemigo recibiendo daño: " + data.Damage);

        // 1. Esto es lo que resta la vida
        GetComponent<SistemaVida>().RecibirDano(data);

        // 2. Esto es lo que mueve
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