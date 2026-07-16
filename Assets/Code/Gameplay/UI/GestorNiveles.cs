using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorNiveles : MonoBehaviour
{
    public static GestorNiveles instancia;
    public string nivelPendiente;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // ¡Crucial! Esto hace que el objeto sobreviva entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IrANivel(string nombreNivel)
    {
        nivelPendiente = nombreNivel;
        SceneManager.LoadScene("LoadingScene"); // Carga tu escena de carga
    }
}
