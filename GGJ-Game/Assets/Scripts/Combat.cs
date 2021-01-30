using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int MovementSpeed { get; set; }
    public int AttackSpeed { get; set; }
    public int CriticalRate { get; set; }
    public int CriticalDamage { get; set; }
    public int Penetration { get; set; }


    public void GetDamage(Combat combat)
    {
        int ran = (int)Random.Range(1f, 100f);
        if(ran <= combat.CriticalRate)  // Critical
        {
            //Debug.Log("CRIT: " + combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration))) * ((100 + combat.CriticalDamage) / 100));
            Health -= combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration))) * ((100 + combat.CriticalDamage) / 100);
        }
        else // Not Critical
        {
            //Debug.Log("NOT CRIT: " + combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration))));
            Health -= combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration)));
        }

        if(Health <= 0)
        {
            Die();
        }
        Debug.Log("HP: " + Health.ToString());
    }

    protected virtual void Die()
    {
        // Do Something
    }
}
