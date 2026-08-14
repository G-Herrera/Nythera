using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PantallaCarga : MonoBehaviour
{
    [Header("Referencias de UI")]
    public Slider barraProgreso;
    public Image imagenRellenoBarra; // <--- Arrastra aquí la Image del "Fill" de tu Slider para que cambie de color

    [Header("Configuración de Carga Lenta")]
    [Tooltip("Velocidad con la que se llena la barra (más bajo = más lento y suave)")]
    public float velocidadSuavizado = 2f;

    [Header("Configuración de Colores")]
    public Color colorInicio = Color.blue;
    public Color colorMitad = Color.yellow;
    public Color colorFinal = Color.green;

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

        StartCoroutine(CargarNivelSuaveYDeColores(nivelACargar));
    }

    IEnumerator CargarNivelSuaveYDeColores(string nombreNivel)
    {
        yield return new WaitForSeconds(0.5f); // Pausa estética inicial

        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreNivel);
        operacion.allowSceneActivation = false; // Pausamos un momento para controlar la fluidez visual

        float valorVisual = 0f;

        while (!operacion.isDone)
        {
            // El progreso real de Unity va de 0 a 0.9
            float progresoReal = Mathf.Clamp01(operacion.progress / 0.9f);

            // Hacemos que la barra avance de forma progresiva y lenta hacia el objetivo
            valorVisual = Mathf.MoveTowards(valorVisual, progresoReal, velocidadSuavizado * Time.deltaTime);

            if (barraProgreso != null)
            {
                barraProgreso.value = valorVisual;
            }

            // Cambiar colores de la barra según qué tan llena esté
            if (imagenRellenoBarra != null)
            {
                if (valorVisual < 0.5f)
                {
                    // Transición del color inicio al color mitad (0% al 50%)
                    imagenRellenoBarra.color = Color.Lerp(colorInicio, colorMitad, valorVisual * 2f);
                }
                else
                {
                    // Transición del color mitad al color final (50% al 100%)
                    imagenRellenoBarra.color = Color.Lerp(colorMitad, colorFinal, (valorVisual - 0.5f) * 2f);
                }
            }

            // Cuando la barra visual llega al 100% (1.0), dejamos que el juego abra la escena
            if (valorVisual >= 0.99f && operacion.progress >= 0.9f)
            {
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}