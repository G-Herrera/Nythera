using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SistemaVida))]
public class SpawnerReactivo : MonoBehaviour, IDamageable
{
    [Header("Configuración de Spawn")]
    public GameObject enemigoPrefab;
    public Transform[] waypointsCompartidos;
    public float rangoActivacion = 10f;
    public int maxEnemigos = 6;
    public float delaySpawn = 1f;

    private List<GameObject> enemigosVivos = new List<GameObject>();
    private Transform jugador;
    private float tiempoSiguienteSpawn;
    private SistemaVida vidaSpawner;
    private bool spawnerMuerto = false;
    private bool mirandoDerecha = true;

    void Start()
    {
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;

        vidaSpawner = GetComponent<SistemaVida>();
    }

    void Update()
    {
        if (spawnerMuerto || !gameObject.activeInHierarchy || jugador == null) return;

        // Validar si el jugador sigue vivo
        bool jugadorVivo = false;
        SistemaVida vidaPlayer = jugador.GetComponent<SistemaVida>();
        if (vidaPlayer != null && vidaPlayer.ObtenerVidaActual() > 0)
        {
            jugadorVivo = true;
        }

        // Limpiar la lista de enemigos destruidos
        enemigosVivos.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (!jugadorVivo) return;

        // 1. Calcular dirección y hacer que el spawner mire hacia el jugador
        float direccionX = jugador.position.x - transform.position.x;
        ActualizarGiroSpawner(direccionX);

        // 2. Calcular distancia al jugador
        float dist = Vector2.Distance(transform.position, jugador.position);

        // Si está en rango y no se pasa del límite, genera enemigos
        if (dist <= rangoActivacion && enemigosVivos.Count < maxEnemigos)
        {
            if (Time.time >= tiempoSiguienteSpawn)
            {
                Spawnear();
                tiempoSiguienteSpawn = Time.time + delaySpawn;
            }
        }
    }

    private void ActualizarGiroSpawner(float dirX)
    {
        if (dirX > 0.1f)
        {
            mirandoDerecha = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (dirX < -0.1f)
        {
            mirandoDerecha = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Spawnear()
    {
        // Genera al enemigo directamente en la posición del spawner
        GameObject nuevo = Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
        EnemigoVeloz v = nuevo.GetComponent<EnemigoVeloz>();

        if (v != null) v.puntosPatrullaje = waypointsCompartidos;

        enemigosVivos.Add(nuevo);
    }

    public void TakeDamage(AttackData data)
    {
        if (vidaSpawner != null && !spawnerMuerto)
        {
            vidaSpawner.RecibirDano(data);

            if (vidaSpawner.ObtenerVidaActual() <= 0)
            {
                spawnerMuerto = true;
            }
        }
    }
}