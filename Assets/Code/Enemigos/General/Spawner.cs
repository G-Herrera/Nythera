using UnityEngine;
using System.Collections.Generic;

// Agregamos [RequireComponent] para que el Spawner siempre tenga vida
[RequireComponent(typeof(SistemaVida))]
public class SpawnerReactivo : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public Transform[] waypointsCompartidos;
    public float rangoActivacion = 10f;
    public int maxEnemigos = 6;
    public float delaySpawn = 1f;

    private List<GameObject> enemigosVivos = new List<GameObject>();
    private Transform jugador;
    private float tiempoSiguienteSpawn;
    private SistemaVida vidaSpawner; // Referencia a su propia vida

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        vidaSpawner = GetComponent<SistemaVida>();
    }

    void Update()
    {
        // SI EL SPAWNER ESTÁ DESACTIVADO (por muerte), ya no hacemos nada
        if (!gameObject.activeInHierarchy) return;

        enemigosVivos.RemoveAll(e => e == null || !e.activeInHierarchy);

        float dist = Vector2.Distance(transform.position, jugador.position);

        if (dist <= rangoActivacion && enemigosVivos.Count < maxEnemigos)
        {
            if (Time.time >= tiempoSiguienteSpawn)
            {
                Spawnear();
                tiempoSiguienteSpawn = Time.time + delaySpawn;
            }
        }
    }

    void Spawnear()
    {
        GameObject nuevo = Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
        EnemigoVeloz v = nuevo.GetComponent<EnemigoVeloz>();
        if (v != null) v.puntosPatrullaje = waypointsCompartidos;
        enemigosVivos.Add(nuevo);
    }
}