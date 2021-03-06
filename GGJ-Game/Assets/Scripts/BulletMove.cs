﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour, IPooledObject
{
    //private Rigidbody2D rigid;
    [SerializeField] Rigidbody2D rigid;

    public float speed;
    private Combat combat;

    public void OnObjectSpawn()
    {
        rigid.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }

    public void SetCombatStats(Combat other)
    {
        combat = other;
    }

    public Combat GetCombatStats()
    {
        return combat;
    }
}
