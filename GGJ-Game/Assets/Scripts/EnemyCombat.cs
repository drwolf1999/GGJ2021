using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : Combat
{
    private void Start()
    {
        Health = 100;
        Attack = 18;
        Defense = 0;
        MovementSpeed = 8;
        AttackSpeed = 2;
        CriticalRate = 0;
        CriticalDamage = 0;
        Penetration = 0;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if(tag == "PlayerBullet")
        {
            Combat playerCombat = collision.gameObject.GetComponent<BulletMove>().GetCombatStats();
            GetDamage(playerCombat);
        }
    }
}
