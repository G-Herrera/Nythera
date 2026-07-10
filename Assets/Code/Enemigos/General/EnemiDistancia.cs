using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class EnemigoDistancia : MonoBehaviour, IDamageable
{
    [Header("Configuración de Disparo")]
    public GameObject prefabProyectil;
    public Transform rotadorPuntoDisparo; // El objeto vacío que rota
    public Transform puntoDisparo;        // El punto donde sale la bala
    public float rangoDeteccion = 8f;
    public float cadenciaDisparo = 2f;

    private bool siendoEmpujado = false;
    private float tiempoProximoDisparo;
    private Transform jugador;
    private SpriteRenderer sr;
    private float offsetXInicial; // Guardamos la posición original

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Guardamos la distancia inicial al centro para corregirla después
        offsetXInicial = puntoDisparo.localPosition.x;

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (jugador == null || siendoEmpujado) return;

        // 1. Giro del ROTADOR hacia el jugador
        Vector3 direccion = jugador.position - rotadorPuntoDisparo.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        rotadorPuntoDisparo.rotation = Quaternion.Euler(0, 0, angulo);

        // 2. Control de dirección y corrección de posición
        if (sr != null)
        {
            bool mirandoDerecha = jugador.position.x > transform.position.x;
            sr.flipX = !mirandoDerecha;

            // Corregimos la posición del punto de disparo para que no se vaya a la espalda
            float nuevoX = mirandoDerecha ? Mathf.Abs(offsetXInicial) : -Mathf.Abs(offsetXInicial);
            puntoDisparo.localPosition = new Vector3(nuevoX, puntoDisparo.localPosition.y, puntoDisparo.localPosition.z);
        }

        // 3. Lógica de disparo
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
            Instantiate(prefabProyectil, puntoDisparo.position, rotadorPuntoDisparo.rotation);
        }
    }
}