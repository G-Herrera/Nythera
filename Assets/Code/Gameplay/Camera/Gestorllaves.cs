using System;
using UnityEngine;
using TMPro;

public class GestorLlaves : MonoBehaviour
{
    [Header("Key Settings")]
    public int llavesRecolectadas = 0;
    public int llavesNecesarias = 3;

    [Header("Door")]
    public GameObject puertaOObjetoADestruir;

    [Header("Legacy HUD - Optional")]
    public TextMeshProUGUI textoHUD;

    // Evento para cualquier UI que quiera escuchar cambios
    public event Action<int, int> OnLlavesChanged;

    private void Start()
    {
        ActualizarHUD();
        NotifyKeyChanged();
    }

    public void RecogerLlave()
    {
        llavesRecolectadas++;

        ActualizarHUD();
        NotifyKeyChanged();

        if (llavesRecolectadas >= llavesNecesarias)
        {
            if (puertaOObjetoADestruir != null)
            {
                Destroy(puertaOObjetoADestruir);
            }
        }
    }

    private void ActualizarHUD()
    {
        // Lo conservamos por compatibilidad con el sistema de tu compañero.
        if (textoHUD != null)
        {
            textoHUD.text =
                "Llaves: " + llavesRecolectadas + "/" + llavesNecesarias;
        }
    }

    private void NotifyKeyChanged()
    {
        OnLlavesChanged?.Invoke(
            llavesRecolectadas,
            llavesNecesarias
        );
    }
}