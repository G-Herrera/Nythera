using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PantallaCarga : MonoBehaviour
{
    void Start()
    {
        string nivelACargar = PlayerPrefs.GetString("NivelDestino", "scn_PlayerController");

        if (string.IsNullOrEmpty(nivelACargar))
        {
            Debug.LogError("¡No se encontró un nivel destino guardado!");
            return;
        }

        StartCoroutine(CargarNivelAsync(nivelACargar));
    }

    IEnumerator CargarNivelAsync(string nombreNivel)
    {
        yield return new WaitForSeconds(1.5f); // Pausa visual de carga

        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreNivel);

        while (!operacion.isDone)
        {
            yield return null;
        }
    }
}