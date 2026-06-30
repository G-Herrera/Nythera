using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        AttackData attackData = new AttackData(damage, knockbackForce, Vector2.right);


        if (damageable != null)
        {
            damageable.TakeDamage(attackData);
        }
    }
}
