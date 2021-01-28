using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
    private void Start()
    {
        Health = 100;
        Attack = 100;
        Defense = 100;
        MovementSpeed = 100;
        AttackSpeed = 100;
        CriticalRate = 100;
        CriticalDamage = 100;
        Penetration = 100;
    }
    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if(tag == "EnemyBullet")
        {
            Combat enemyCombat = collision.gameObject.GetComponentInParent(typeof(Combat)) as Combat;
            GetDamage(enemyCombat);
        }
    }
}
