using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
    private void Start()
    {
        Health = 100;
        Attack = 18;
        Defense = 0;
        MovementSpeed = 7;
        AttackSpeed = 1;
        CriticalRate = 50;
        CriticalDamage = 70;
        Penetration = 0;
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
            //Combat enemyCombat = collision.gameObject.GetComponentInParent(typeof(Combat)) as Combat;
            Combat enemyCombat = collision.gameObject.GetComponent<EnemyBulletMove>().GetCombatStats();
            GetDamage(enemyCombat);
        }
    }
}
