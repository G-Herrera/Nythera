using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PantallaCarga : MonoBehaviour
{
    [Header("Opcional")]
    public Slider barraProgreso;

    void Start()
    {
        string nivelACargar = "";

        // 1. Obtenemos el nivel desde tu GestorNiveles global
        if (GestorNiveles.instancia != null && !string.IsNullOrEmpty(GestorNiveles.instancia.nivelPendiente))
        {
            nivelACargar = GestorNiveles.instancia.nivelPendiente;
        }
        else
        {
            // 2. Respaldo por PlayerPrefs por seguridad
            nivelACargar = PlayerPrefs.GetString("NivelDestino", "scn_FireLevel");
        }

        if (string.IsNullOrEmpty(nivelACargar))
        {
            Debug.LogError("¡No se encontró un nivel destino guardado!");
            return;
        }

        StartCoroutine(CargarNivelDirecto(nivelACargar));
    }

    IEnumerator CargarNivelDirecto(string nombreNivel)
    {
        // Pequeña pausa estética para que se alcance a ver la pantalla de carga
        yield return new WaitForSeconds(1.0f);

        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreNivel);

        // Permitimos que la escena cargue de golpe de forma fluida sin trabarse
        while (!operacion.isDone)
        {
            float progreso = Mathf.Clamp01(operacion.progress / 0.9f);

            if (barraProgreso != null)
            {
                barraProgreso.value = progreso;
            }

            yield return null;
        }
    }
}