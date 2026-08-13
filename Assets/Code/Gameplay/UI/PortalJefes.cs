using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalFinJefe : MonoBehaviour
{
    [Header("Configuración")]
    public string nivelDestino = "Nivel1"; // Nombre de tu nivel
    public string escenaCarga = "Loading"; // Nombre de tu escena de carga

    public GameObject jefeObjetivo;
    public SpriteRenderer spritePortal;
    private Collider2D colisionador;
    private bool activado = false;

    void Start()
    {
        colisionador = GetComponent<Collider2D>();
        if (colisionador) colisionador.enabled = false;
        if (spritePortal) spritePortal.enabled = false;
    }

    void Update()
    {
        if (!activado && (jefeObjetivo == null || jefeObjetivo.GetComponent<SistemaVida>().ObtenerVidaActual() <= 0))
        {
            activado = true;
            if (colisionador) colisionador.enabled = true;
            if (spritePortal) spritePortal.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && activado)
        {
            // Guardamos el nivel al que queremos ir en la memoria temporal de Unity
            PlayerPrefs.SetString("NivelDestino", nivelDestino);
            // Cargamos la pantalla de carga
            SceneManager.LoadScene(escenaCarga);
        }
    }
}