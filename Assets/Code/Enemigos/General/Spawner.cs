using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SistemaVida))]
public class SpawnerReactivo : MonoBehaviour, IDamageable // <-- 1. Añadimos la interfaz aquí
{
    public GameObject enemigoPrefab;
    public Transform[] waypointsCompartidos;
    public float rangoActivacion = 10f;
    public int maxEnemigos = 6;
    public float delaySpawn = 1f;

    private List<GameObject> enemigosVivos = new List<GameObject>();
    private Transform jugador;
    private float tiempoSiguienteSpawn;
    private SistemaVida vidaSpawner;

    void Start()
    {
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;

        vidaSpawner = GetComponent<SistemaVida>();
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy || jugador == null) return;

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

    // 2. Método obligatorio de IDamageable para que la espada le haga daño
    public void TakeDamage(AttackData data)
    {
        if (vidaSpawner != null)
        {
            vidaSpawner.RecibirDano(data);

            // Si la vida llega a cero, el SistemaVida se encargará de disparar 
            // el evento OnMorir que tengas configurado en el Inspector de este spawner.
        }
    }
}