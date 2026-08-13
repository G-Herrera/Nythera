using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalFinJefe : MonoBehaviour
{
    [Header("Configuración de Carga")]
    public string nivelDestino = "scn_PlayerController";
    public string escenaCarga = "LoadingScene";

    [Header("Referencias")]
    public GameObject jefeObjetivo;
    public SpriteRenderer spritePortal;

    private Collider2D colisionador;
    private bool activado = false;

    void Start()
    {
        colisionador = GetComponent<Collider2D>();

        // El portal arranca completamente oculto e inactivo
        if (colisionador) colisionador.enabled = false;
        if (spritePortal) spritePortal.enabled = false;
    }

    void Update()
    {
        if (activado) return;

        bool debeActivarse = false;

        // Condición 1: Si el jefe desapareció de la jerarquía (fue destruido con Destroy)
        if (jefeObjetivo == null)
        {
            debeActivarse = true;
        }
        else
        {
            // Condición 2: Si el jefe sigue en escena, revisamos si su objeto fue desactivado (gameObject.activeInHierarchy == false)
            if (!jefeObjetivo.activeInHierarchy)
            {
                debeActivarse = true;
            }
            else
            {
                // Condición 3: Intentamos leer su vida mediante reflexión o componentes comunes si existe
                // (Esto evita que falle si el método se llama distinto)
                SistemaVida vidaJefe = jefeObjetivo.GetComponent<SistemaVida>();
                if (vidaJefe != null && vidaJefe.ObtenerVidaActual() <= 0)
                {
                    debeActivarse = true;
                }
            }
        }

        if (debeActivarse)
        {
            ActivarPortal();
        }
    }

    void ActivarPortal()
    {
        activado = true;
        if (colisionador) colisionador.enabled = true;
        if (spritePortal) spritePortal.enabled = true;
        Debug.Log("¡Jefe derrotado! El portal se ha abierto.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && activado)
        {
            PlayerPrefs.SetString("NivelDestino", nivelDestino);
            Debug.Log("Cargando escena de transición: " + escenaCarga);
            SceneManager.LoadScene(escenaCarga);
        }
    }
}