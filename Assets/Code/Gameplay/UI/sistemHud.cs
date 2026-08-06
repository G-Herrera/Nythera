using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaHUD : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPausa;
    public GameObject PanelConfig; 
    public GameObject panelMapa;
    public GameObject camaraMapa;

    private bool juegoPausado = false;
    private bool mapaAbierto = false;
    private bool configAbierta = false; // <-- Estado del panel de configuración

    void Update()
    {
        // Control de Pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mapaAbierto) CerrarMapa();
            else if (configAbierta) CerrarConfiguracion(); // Si config está abierto, lo cierra con ESC
            else if (juegoPausado) ReanudarJuego();
            else PausarJuego();
        }

        // Control de Mapa (ej. tecla M)
        if (Input.GetKeyDown(KeyCode.M) && !juegoPausado && !configAbierta)
        {
            if (mapaAbierto) CerrarMapa();
            else AbrirMapa();
        }
    }

    // --- MÉTODOS DE PAUSA ---
    public void PausarJuego()
    {
        juegoPausado = true;
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        panelPausa.SetActive(false);
        if (PanelConfig != null) PanelConfig.SetActive(false);
        configAbierta = false;
        Time.timeScale = 1f;
    }

    // --- MÉTODOS DE CONFIGURACIÓN ---
    public void AbrirConfiguracion()
    {
        if (PanelConfig != null)
        {
            PanelConfig.SetActive(true);
            configAbierta = true;
        }

        // Opcional: Puedes ocultar el panel de pausa de fondo si quieres que solo se vea el de config
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    public void CerrarConfiguracion()
    {
        if (PanelConfig != null)
        {
            PanelConfig.SetActive(false);
            configAbierta = false;
        }

        // Volvemos a mostrar el panel de pausa principal
        if (panelPausa != null) panelPausa.SetActive(true);
    }

    // --- MÉTODOS PARA LOS BOTONES DEL MENÚ DE PAUSA ---
    public void IrAMainMenu()
    {
        Time.timeScale = 1f;

        if (GestorNiveles.instancia != null)
        {
            GestorNiveles.instancia.IrANivel("scn_MainMenu");
        }
        else
        {
            SceneManager.LoadScene("scn_MainMenu");
        }
    }

    // --- MÉTODOS DE MAPA ---
    public void AbrirMapa()
    {
        if (camaraMapa != null) camaraMapa.SetActive(true);
        panelMapa.SetActive(true);
        mapaAbierto = true;
        Time.timeScale = 0f;
    }

    public void CerrarMapa()
    {
        if (camaraMapa != null) camaraMapa.SetActive(false);
        panelMapa.SetActive(false);
        mapaAbierto = false;
        Time.timeScale = 1f;
    }
}