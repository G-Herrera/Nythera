using UnityEngine;
using UnityEngine.UI;

public class HUDKeys : MonoBehaviour
{
    [Header("Key Icons")]
    [SerializeField] private Image[] keyIcons;

    [Header("Visual Settings")]
    [SerializeField, Range(0f, 1f)]
    private float uncollectedAlpha = 0.2f;

    [SerializeField, Range(0f, 1f)]
    private float collectedAlpha = 1f;

    private GestorLlaves gestorLlaves;

    public void Initialize(GestorLlaves gestor)
    {
        if (gestorLlaves != null)
            gestorLlaves.OnLlavesChanged -= UpdateKeys;

        gestorLlaves = gestor;

        if (gestorLlaves == null)
        {
            Debug.LogError(
                "HUDKeys recibió un GestorLlaves nulo.",
                this
            );

            return;
        }

        gestorLlaves.OnLlavesChanged += UpdateKeys;

        // Inicialización inmediata
        UpdateKeys(
            gestorLlaves.llavesRecolectadas,
            gestorLlaves.llavesNecesarias
        );
    }

    private void OnDisable()
    {
        if (gestorLlaves != null)
            gestorLlaves.OnLlavesChanged -= UpdateKeys;
    }

    private void UpdateKeys(int collectedKeys, int requiredKeys)
    {
        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (keyIcons[i] == null)
                continue;

            // Si esta llave ya fue recogida, se ve completamente.
            bool collected = i < collectedKeys;

            Color color = keyIcons[i].color;

            color.a = collected
                ? collectedAlpha
                : uncollectedAlpha;

            keyIcons[i].color = color;

            // Si hay más slots que llaves necesarias,
            // ocultamos los sobrantes.
            keyIcons[i].gameObject.SetActive(i < requiredKeys);
        }
    }
}