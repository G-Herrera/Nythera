using UnityEngine;
using UnityEngine.UI;

public class NieblaMapaContinuo : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    private RawImage imagenNieblaUI;
    public RectTransform iconoJugador;

    [Header("Configuración de la Textura")]
    public int anchoTextura = 512;
    public int altoTextura = 512;
    public int radioPincel = 22;

    [Header("Límites del Mundo (Nivel Carlos)")]
    public Vector2 minMundo = new Vector2(-25f, -12f);
    public Vector2 maxMundo = new Vector2(25f, 12f);

    [Header("Ajuste Fino (Offset)")]
    public Vector2 desplazamientoMundo = Vector2.zero;

    private Texture2D texturaNiebla;
    private Vector2 ultimaPosicionTextura;
    private bool inicializado = false;

    void Awake()
    {
        imagenNieblaUI = GetComponent<RawImage>();
        InicializarTextura();
    }

    void Start()
    {
        BuscarJugador();
        if (jugador != null)
        {
            // Posición inicial para evitar saltos al arrancar
            Vector2 posInicial = (Vector2)jugador.position + desplazamientoMundo;
            float nx = Mathf.InverseLerp(minMundo.x, maxMundo.x, posInicial.x);
            float ny = Mathf.InverseLerp(minMundo.y, maxMundo.y, posInicial.y);
            ultimaPosicionTextura = new Vector2(Mathf.RoundToInt(nx * anchoTextura), Mathf.RoundToInt(ny * altoTextura));
            inicializado = true;
        }
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
        if (jugador == null)
        {
            BuscarJugador();
            return;
        }

        if (!inicializado) return;

        // 1. EL RASTRO SE DIBUJA SIEMPRE (aunque el mapa esté cerrado)
        Vector2 posRealJugador = (Vector2)jugador.position + desplazamientoMundo;

        float normalX = Mathf.InverseLerp(minMundo.x, maxMundo.x, posRealJugador.x);
        float normalY = Mathf.InverseLerp(minMundo.y, maxMundo.y, posRealJugador.y);

        int posX = Mathf.RoundToInt(normalX * anchoTextura);
        int posY = Mathf.RoundToInt(normalY * altoTextura);
        Vector2 posicionActualTextura = new Vector2(posX, posY);

        // Dibujamos la línea continua desde la última posición conocida
        DibujarLinea(ultimaPosicionTextura, posicionActualTextura);
        ultimaPosicionTextura = posicionActualTextura;

        // 2. LA UI SOLO SE ACTUALIZA SI EL PANEL DEL MAPA ESTÁ ABIERTO
        if (gameObject.activeInHierarchy && iconoJugador != null && imagenNieblaUI != null)
        {
            RectTransform panelRect = imagenNieblaUI.rectTransform;
            float anchoUI = panelRect.rect.width;
            float altoUI = panelRect.rect.height;

            float uiX = (normalX - 0.5f) * anchoUI;
            float uiY = (normalY - 0.5f) * altoUI;

            iconoJugador.anchoredPosition = new Vector2(uiX, uiY);
        }
    }

    void DibujarLinea(Vector2 p1, Vector2 p2)
    {
        int x0 = (int)p1.x;
        int y0 = (int)p1.y;
        int x1 = (int)p2.x;
        int y1 = (int)p2.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        bool huboCambios = false;

        while (true)
        {
            if (PintarPunto(x0, y0)) huboCambios = true;

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }

        if (huboCambios)
        {
            texturaNiebla.Apply();
        }
    }

    bool PintarPunto(int xCentral, int yCentral)
    {
        bool cambio = false;
        int radioSq = radioPincel * radioPincel;

        for (int x = -radioPincel; x <= radioPincel; x++)
        {
            for (int y = -radioPincel; y <= radioPincel; y++)
            {
                int px = xCentral + x;
                int py = yCentral + y;

                if (px >= 0 && px < anchoTextura && py >= 0 && py < altoTextura)
                {
                    if (x * x + y * y <= radioSq)
                    {
                        if (texturaNiebla.GetPixel(px, py).a > 0)
                        {
                            texturaNiebla.SetPixel(px, py, Color.clear);
                            cambio = true;
                        }
                    }
                }
            }
        }
        return cambio;
    }
}