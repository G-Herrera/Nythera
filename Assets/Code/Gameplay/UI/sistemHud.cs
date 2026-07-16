using UnityEngine;

public class SistemaHUD : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPausa;
    public GameObject panelMapa;
    public GameObject camaraMapa;

    private bool juegoPausado = false;
    private bool mapaAbierto = false;

    void Update()
    {
        // Control de Pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mapaAbierto) CerrarMapa();
            else if (juegoPausado) ReanudarJuego();
            else PausarJuego();
        }

        // Control de Mapa (ej. tecla M)
        if (Input.GetKeyDown(KeyCode.M) && !juegoPausado)
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
        Time.timeScale = 0f; // Detiene el tiempo
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // Reanuda el tiempo
    }

    // --- MÉTODOS DE MAPA ---
    public void AbrirMapa()
    {
        if (camaraMapa != null) camaraMapa.SetActive(true); // Verificación de seguridad
        panelMapa.SetActive(true);
        mapaAbierto = true;

        Time.timeScale = 0f; // Esto detendrá las físicas y animaciones
    }

    public void CerrarMapa()
    {
        if (camaraMapa != null) camaraMapa.SetActive(false);
        panelMapa.SetActive(false);
        mapaAbierto = false;

        Time.timeScale = 1f;
    }
}