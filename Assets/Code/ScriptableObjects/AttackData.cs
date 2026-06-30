using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    public int Damage { get; }
    public float KnockbackForce { get; }
    public Vector2 AttackDirection { get; }
    public AttackData(int damage, float knockbackForce, Vector2 attackDirection)
    {
        Damage = damage;
        KnockbackForce = knockbackForce;
        AttackDirection = attackDirection;

    }

    
}
