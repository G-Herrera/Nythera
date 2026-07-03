using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SistemaVida), typeof(Knockback))]
public class EnemigoDistancia : MonoBehaviour, IDamageable
{
    [Header("Configuración de Disparo")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float rangoDeteccion = 8f;
    public float cadenciaDisparo = 2f;
    private bool siendoEmpujado = false;

    private float tiempoProximoDisparo;
    private Transform jugador;

    void Start()
    {
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (jugador == null || siendoEmpujado) return;

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
        Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.Euler(0, 0, angulo));

        // Flip sin usar escala para que no se deforme
        GetComponent<SpriteRenderer>().flipX = (direccion.x < 0);
    }
}