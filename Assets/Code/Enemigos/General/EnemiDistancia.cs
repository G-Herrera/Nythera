using UnityEngine;

// Aseguramos que el objeto tenga SistemaVida. 
// Quitamos la herencia de "SistemaVida" y usamos MonoBehaviour.
[RequireComponent(typeof(SistemaVida))]
public class EnemigoDistancia : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float rangoDeteccion = 8f;
    public float cadenciaDisparo = 2f;

    private float tiempoProximoDisparo;
    private Transform jugador;
    private SistemaVida vida; // Referencia al componente

    void Start()
    {
        // Obtenemos la referencia al componente de vida que está en este mismo objeto
        vida = GetComponent<SistemaVida>();

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

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

    void Disparar()
    {
        // 1. Calculamos la dirección hacia el jugador
        Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;

        // 2. Calculamos el ángulo en grados para rotar el objeto
        // Mathf.Atan2 nos da el ángulo en radianes, * Mathf.Rad2Deg lo convierte a grados
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 3. Instanciamos el proyectil con la rotación calculada
        GameObject bala = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.Euler(0, 0, angulo));

        // 4. (Opcional) Si quieres que el enemigo también mire al jugador
        if (direccion.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
    }
}