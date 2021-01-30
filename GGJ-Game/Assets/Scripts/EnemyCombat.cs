using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : Combat
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();

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
        animator.SetBool("meleeDead", true);
        Destroy(gameObject, 0.3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if(tag == "PlayerBullet")
        {
            animator.SetBool("isHit", true);
            Combat playerCombat = collision.gameObject.GetComponent<BulletMove>().GetCombatStats();
            GetDamage(playerCombat);
        }
        animator.SetBool("isHit", false);
    }
}
