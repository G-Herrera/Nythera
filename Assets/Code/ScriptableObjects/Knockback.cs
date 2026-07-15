using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AplicarEmpuje(AttackData data)
    {
        if (rb != null)
        {
            // El mensaje de depuración te ayudará a confirmar que el golpe llega
            Debug.Log("¡Aplicando fuerza de empuje: " + data.KnockbackForce);

            rb.velocity = Vector2.zero; // Frenamos cualquier movimiento previo
            rb.AddForce(data.AttackDirection * data.KnockbackForce, ForceMode2D.Impulse);
        }
    }
}