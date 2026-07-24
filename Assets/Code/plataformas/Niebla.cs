using UnityEngine;
using UnityEngine.UI;

public class NieblaMapa : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    private RawImage imagenNieblaUI;
    public RectTransform iconoJugador;

    [Header("Configuración de la Textura")]
    public int anchoTextura = 256;
    public int altoTextura = 256;
    public int radioPincel = 15;

    [Header("Límites del Mundo (Nivel Carlos)")]
    public Vector2 minMundo = new Vector2(-10f, -5f);
    public Vector2 maxMundo = new Vector2(60f, 30f);

    [Header("Límites del Panel en la UI")]
    public Vector2 minUI = new Vector2(-400f, -200f);
    public Vector2 maxUI = new Vector2(400f, 200f);

    private Texture2D texturaNiebla;

    void Awake()
    {
        imagenNieblaUI = GetComponent<RawImage>();
        InicializarTextura();
    }

    void OnEnable()
    {
        BuscarJugador();
    }

    void BuscarJugador()
    {
        if (jugador == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) jugador = p.transform;
        }
    }

    void InicializarTextura()
    {
        texturaNiebla = new Texture2D(anchoTextura, altoTextura, TextureFormat.RGBA32, false);
        texturaNiebla.filterMode = FilterMode.Bilinear;

        Color32 colorNegro = new Color32(0, 0, 0, 255);
        Color32[] pixelsIniciales = new Color32[anchoTextura * altoTextura];

        for (int i = 0; i < pixelsIniciales.Length; i++)
        {
            pixelsIniciales[i] = colorNegro;
        }

        texturaNiebla.SetPixels32(pixelsIniciales);
        texturaNiebla.Apply();

        if (imagenNieblaUI != null)
        {
            imagenNieblaUI.texture = texturaNiebla;
        }
    }

    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            ActualizarMapaUI();
        }
    }

    void ActualizarMapaUI()
    {
        if (jugador == null)
        {
            BuscarJugador();
            return;
        }

        // 1. Mapeamos la posición del jugador en el mundo a porcentajes (0 a 1)
        float normalX = Mathf.InverseLerp(minMundo.x, maxMundo.x, jugador.position.x);
        float normalY = Mathf.InverseLerp(minMundo.y, maxMundo.y, jugador.position.y);

        // 2. Borramos la niebla justo donde está el jugador
        int posX = Mathf.RoundToInt(normalX * anchoTextura);
        int posY = Mathf.RoundToInt(normalY * altoTextura);
        RevelarArea(posX, posY, radioPincel);

        // 3. Movemos el icono rojo en el mapa usando los límites de UI
        if (iconoJugador != null)
        {
            float uiX = Mathf.Lerp(minUI.x, maxUI.x, normalX);
            float uiY = Mathf.Lerp(minUI.y, maxUI.y, normalY);
            iconoJugador.anchoredPosition = new Vector2(uiX, uiY);
        }
    }

    void RevelarArea(int xCentral, int yCentral, int radio)
    {
        bool huboCambios = false;

        for (int x = -radio; x <= radio; x++)
        {
            for (int y = -radio; y <= radio; y++)
            {
                int px = xCentral + x;
                int py = yCentral + y;

                if (px >= 0 && px < anchoTextura && py >= 0 && py < altoTextura)
                {
                    if (x * x + y * y <= radio * radio)
                    {
                        texturaNiebla.SetPixel(px, py, new Color(0, 0, 0, 0));
                        huboCambios = true;
                    }
                }
            }
        }

        if (huboCambios)
        {
            texturaNiebla.Apply();
        }
    }
}