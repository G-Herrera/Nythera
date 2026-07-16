using UnityEngine;
using System.Collections;

public class PlataformaDesvanecible : MonoBehaviour
{
    public float tiempoParaDesaparecer = 2f;
    public float tiempoParaReaparecer = 3f;

    // Referencia al SpriteRenderer para ocultarlo sin apagar el objeto entero
    private SpriteRenderer sprite;
    private Collider2D col;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CicloDesvanecimiento());
        }
    }

    IEnumerator CicloDesvanecimiento()
    {
        yield return new WaitForSeconds(tiempoParaDesaparecer);

        // En lugar de SetActive(false), ocultamos el sprite y desactivamos el collider
        // Esto mantiene el script vivo para que pueda seguir contando el tiempo
        sprite.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(tiempoParaReaparecer);

        // Ahora el script sigue vivo y puede reactivar las cosas
        sprite.enabled = true;
        col.enabled = true;
    }
}