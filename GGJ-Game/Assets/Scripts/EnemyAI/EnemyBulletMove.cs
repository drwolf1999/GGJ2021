using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMove : MonoBehaviour, IPooledObject
{
    [SerializeField] Rigidbody2D rigid;

    public float speed;

    // Start is called before the first frame update

    public void OnObjectSpawn()
    {
        rigid.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EnemyMelee")
        {
            gameObject.SetActive(false);
        }
    }

    private Combat combat;

    public void SetCombatStats(Combat other)
    {
        combat = other;
    }

    public Combat GetCombatStats()
    {
        return combat;
    }
}
