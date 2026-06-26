using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public float velocidad = 2f;
    public float distancia = 5f;
    public bool movimientoVertical = false;

    private Vector3 posicionInicial;

    void Start() { posicionInicial = transform.position; }

    void Update()
    {
        float movimiento = Mathf.PingPong(Time.time * velocidad, distancia);
        if (movimientoVertical)
            transform.position = posicionInicial + new Vector3(0, movimiento, 0);
        else
            transform.position = posicionInicial + new Vector3(movimiento, 0, 0);
    }
}
