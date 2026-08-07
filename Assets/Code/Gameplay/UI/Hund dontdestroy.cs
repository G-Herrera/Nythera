using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorPersistenteHUD : MonoBehaviour
{
    private static GestorPersistenteHUD instancia;

    [Header("Nombres de escenas donde NO debe aparecer el HUD")]
    public string nombreEscenaMenu = "MainMenu";       // Cambia esto por el nombre exacto de tu escena de menú
    public string nombreEscenaCarga = "LoadingScene";   // Cambia esto por el nombre exacto de tu escena de carga

    void Awake()
    {
        // Patrón Singleton para evitar duplicados si regresas al menú y vuelves a entrar
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        // Nos suscribimos al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Nos desuscribimos para evitar fugas de memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene escena, LoadSceneMode modo)
    {
        // Comprobamos si la escena actual es el Main Menu o la de Carga
        if (escena.name == nombreEscenaMenu || escena.name == nombreEscenaCarga)
        {
            // Si estamos en una de esas escenas, destruimos este objeto persistente
            Destroy(gameObject);
        }
    }
}