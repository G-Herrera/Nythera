using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class JefeColoso : MonoBehaviour, IDamageable
{
    [Header("Configuración de Vida y Combate")]
    public int dañoCuerpoAMuerpo = 20;
    public float rangoDeteccion = 12f;
    public float rangoAtaqueCercano = 4.5f; // Subido un poco para que detecte mejor de cerca
    public float frecuenciaAtaques = 1.5f;
    private float tiempoUltimoAtaque;
    private bool estaAtacando = false;

    [Header("Configuración de Proyectiles (Ataque Lejano)")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float velocidadProyectil = 10f;

    [Header("Referencias")]
    private Transform jugador;
    private Rigidbody2D rb;
    private SistemaVida sistemaVida;
    private Animator anim;
    private bool siendoEmpujado = false;
    private bool estaMuerto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sistemaVida = GetComponent<SistemaVida>();
        anim = GetComponent<Animator>();

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (estaMuerto || siendoEmpujado || jugador == null) return;

        SistemaVida vidaPlayer = jugador.GetComponent<SistemaVida>();
        if (vidaPlayer == null || vidaPlayer.ObtenerVidaActual() <= 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            GirarHaciaJugador();

            // Evaluar ataques solo si no está ejecutando uno y pasó el tiempo
            if (!estaAtacando && Time.time >= tiempoUltimoAtaque + frecuenciaAtaques)
            {
                if (distanciaAlJugador <= rangoAtaqueCercano)
                {
                    StartCoroutine(AtaqueCercanoRoutine());
                }
                else
                {
                    StartCoroutine(AtaqueLejanoRoutine());
                }
                tiempoUltimoAtaque = Time.time;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void GirarHaciaJugador()
    {
        if (jugador.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator AtaqueCercanoRoutine()
    {
        estaAtacando = true;
        anim.ResetTrigger("AtacarLejos");
        anim.SetTrigger("AtacarCerca");

        yield return new WaitForSeconds(0.8f); // Duración de la animación de golpe cuerpo a cuerpo
        estaAtacando = false;
    }

    private IEnumerator AtaqueLejanoRoutine()
    {
        estaAtacando = true;
        anim.ResetTrigger("AtacarCerca");
        anim.SetTrigger("AtacarLejos");

        yield return new WaitForSeconds(0.3f); // Momento en que sale el proyectil

        if (prefabProyectil != null && puntoDisparo != null)
        {
            GameObject proyectil = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);

            // Le pasamos la dirección exacta según hacia dónde mira el jefe
            ProyectilLanza scriptProyectil = proyectil.GetComponent<ProyectilLanza>();
            if (scriptProyectil != null)
            {
                scriptProyectil.ConfigurarDireccion(transform.localScale.x);
            }
        }

        yield return new WaitForSeconds(0.5f); // Tiempo para que la animación de la lanza termine
        estaAtacando = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SistemaVida vidaJugador = collision.gameObject.GetComponent<SistemaVida>();
            if (vidaJugador != null)
            {
                AttackData ataque = new AttackData(dañoCuerpoAMuerpo, 5f, (collision.transform.position - transform.position).normalized);
                vidaJugador.RecibirDano(ataque);
            }
        }
    }

    public void TakeDamage(AttackData data)
    {
        if (estaMuerto) return;

        sistemaVida.RecibirDano(data);
        GetComponent<Knockback>().AplicarEmpuje(data);

        if (sistemaVida.ObtenerVidaActual() <= 0)
        {
            estaMuerto = true;
            StartCoroutine(MuerteRoutine());
        }
        else
        {
            StartCoroutine(PausaEmpuje(0.2f));
        }
    }

    private IEnumerator PausaEmpuje(float tiempo)
    {
        siendoEmpujado = true;
        yield return new WaitForSeconds(tiempo);
        siendoEmpujado = false;
    }

    private IEnumerator MuerteRoutine()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}