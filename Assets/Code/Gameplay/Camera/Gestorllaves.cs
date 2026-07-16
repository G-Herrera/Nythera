using UnityEngine;
using TMPro; // Necesitas tener TextMeshPro instalado

public class GestorLlaves : MonoBehaviour
{
    public int llavesRecolectadas = 0;
    public int llavesNecesarias = 3;
    public GameObject puertaOObjetoADestruir; // Arrastra aquí el objeto que quieres que desaparezca
    public TextMeshProUGUI textoHUD; // Arrastra aquí tu texto de la pantalla

    void Start()
    {
        ActualizarHUD();
    }

    public void RecogerLlave()
    {
        llavesRecolectadas++;
        ActualizarHUD();

        if (llavesRecolectadas >= llavesNecesarias)
        {
            if (puertaOObjetoADestruir != null)
            {
                Destroy(puertaOObjetoADestruir);
            }
        }
    }

    void ActualizarHUD()
    {
        if (textoHUD != null)
        {
            textoHUD.text = "Llaves: " + llavesRecolectadas + "/" + llavesNecesarias;
        }
    }
}
