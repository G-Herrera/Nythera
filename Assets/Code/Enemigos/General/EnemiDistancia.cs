using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class EnemigoDistancia : MonoBehaviour, IDamageable
{
    [Header("Configuración de Disparo")]
    public GameObject prefabProyectil;
    public Transform rotadorPuntoDisparo; // Objeto vacío hijo que pivota
    public Transform puntoDisparo;        // Objeto donde sale la bala
    public float rangoDeteccion = 8f;
    public float cadenciaDisparo = 2f;

    private bool siendoEmpujado = false;
    private float tiempoProximoDisparo;
    private Transform jugador;
    private SpriteRenderer sr;
    private float offsetXInicial;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Guardamos la posición original del punto de disparo para corregirla
        offsetXInicial = puntoDisparo.localPosition.x;

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (jugador == null || siendoEmpujado) return;

        // 1. Cálculo del ángulo hacia el jugador
        Vector3 direccion = jugador.position - rotadorPuntoDisparo.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 2. Control de dirección (Flip) y corrección de rotación
        bool mirandoDerecha = jugador.position.x > transform.position.x;
        sr.flipX = !mirandoDerecha;

        // Si mira a la izquierda, sumamos 180 grados para compensar el giro del sprite
        if (!mirandoDerecha)
        {
            rotadorPuntoDisparo.rotation = Quaternion.Euler(0, 0, angulo + 180);
        }
        else
        {
            rotadorPuntoDisparo.rotation = Quaternion.Euler(0, 0, angulo);
        }

        // 3. Corrección de posición (evita que el punto salga por la espalda)
        float nuevoX = mirandoDerecha ? Mathf.Abs(offsetXInicial) : -Mathf.Abs(offsetXInicial);
        puntoDisparo.localPosition = new Vector3(nuevoX, puntoDisparo.localPosition.y, puntoDisparo.localPosition.z);

        // 4. Lógica de disparo
        float distancia = Vector2.Distance(transform.position, jugador.position);
        if (distancia <= rangoDeteccion)
        {
            if (Time.time >= tiempoProximoDisparo)
            {
                Disparar();
                tiempoProximoDisparo = Time.time + cadenciaDisparo;
            }
        }
    }

    public void TakeDamage(AttackData data)
    {
        GetComponent<SistemaVida>().RecibirDano(data);
        GetComponent<Knockback>().AplicarEmpuje(data);
        StartCoroutine(PausaMovimiento(0.25f));
    }

    private IEnumerator PausaMovimiento(float tiempo)
    {
        siendoEmpujado = true;
        yield return new WaitForSeconds(tiempo);
        siendoEmpujado = false;
    }

    void Disparar()
    {
        if (prefabProyectil != null && puntoDisparo != null)
        {
            // Instancia la bala con la rotación del rotador
            Instantiate(prefabProyectil, puntoDisparo.position, rotadorPuntoDisparo.rotation);
        }
    }
}