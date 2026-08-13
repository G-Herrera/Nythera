using UnityEngine;

public class PortalFinJefe : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreSiguienteNivel = "scn_FireLevel"; // Nombre de tu siguiente nivel

    [Header("Referencias del Jefe")]
    public GameObject jefeObjetivo;
    public SpriteRenderer spritePortal;

    private Collider2D colisionador;
    private bool activado = false;

    void Start()
    {
        colisionador = GetComponent<Collider2D>();
        // El portal arranca oculto y apagado
        if (colisionador) colisionador.enabled = false;
        if (spritePortal) spritePortal.enabled = false;
    }

    void Update()
    {
        if (activado) return;

        // Verificamos si el jefe fue derrotado (se destruyó o se desactivó)
        bool jefeMuerto = (jefeObjetivo == null);
        if (!jefeMuerto && !jefeObjetivo.activeInHierarchy) jefeMuerto = true;

        if (jefeMuerto)
        {
            activado = true;
            if (colisionador) colisionador.enabled = true;
            if (spritePortal) spritePortal.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al cruzar el portal del jefe ya activo, usa el GestorNiveles igual que tu otro portal
        if (collision.CompareTag("Player") && activado)
        {
            if (GestorNiveles.instancia != null)
            {
                GestorNiveles.instancia.IrANivel(nombreSiguienteNivel);
            }
            else
            {
                Debug.LogError("¡No se encontró el GestorNiveles en la escena!");
            }
        }
    }
}