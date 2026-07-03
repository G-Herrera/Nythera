using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private int health = 30;

    public void TakeDamage(AttackData attackData)
    {
        health -= attackData.Damage;

        Debug.Log($"Vida restante: {health}");

        if(health <= 0)
        {
            Destroy(gameObject);
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
