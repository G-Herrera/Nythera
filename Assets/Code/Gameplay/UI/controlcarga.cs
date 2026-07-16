using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PantallaCarga : MonoBehaviour
{
    [Header("Configuración de Carga")]
    public Slider barraProgreso;
    [Tooltip("Velocidad de llenado de la barra (0.1 a 1.0)")]
    public float velocidadCarga = 0.5f;

    void Start()
    {
        // Recuperamos el nivel que guardamos en el puente global
        if (GestorNiveles.instancia != null)
        {
            string nivel = GestorNiveles.instancia.nivelPendiente;
            StartCoroutine(Cargar(nivel));
        }
        else
        {
            Debug.LogError("¡No se encontró el GestorNiveles en la escena!");
        }
    }

    IEnumerator Cargar(string nombre)
    {
        // Iniciamos la carga en segundo plano
        AsyncOperation op = SceneManager.LoadSceneAsync(nombre);

        // Bloqueamos la activación automática para controlar el tiempo
        op.allowSceneActivation = false;

        float progresoSimulado = 0f;

        while (!op.isDone)
        {
            // 1. Obtenemos el progreso real del motor (0 a 0.9)
            float progresoReal = Mathf.Clamp01(op.progress / 0.9f);

            // 2. Animamos el slider de forma suave
            progresoSimulado = Mathf.MoveTowards(progresoSimulado, progresoReal, Time.deltaTime * velocidadCarga);
            barraProgreso.value = progresoSimulado;

            // 3. Verificamos si podemos terminar
            if (op.progress >= 0.9f && progresoSimulado >= 0.95f)
            {
                // Damos un pequeño respiro antes de entrar
                yield return new WaitForSeconds(0.5f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}