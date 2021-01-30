using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public string dieName;
    public string damageName;
    [SerializeField] SoundManager soundManager;

    public void GetDamage(Combat combat)
    {
        int ran = (int)Random.Range(1f, 100f);
        float dmg;
        bool isCrit = false;
        if(ran <= combat.CriticalRate)  // Critical
        {
            //Debug.Log("CRIT: " + combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration))) * ((100 + combat.CriticalDamage) / 100));
            dmg = (float)combat.Attack * (100.0f / (100.0f + (float)Defense - ((float)Defense * (float)combat.Penetration))) * ((100.0f + (float)combat.CriticalDamage) / 100.0f);
            Health -= (int)dmg;
            isCrit = true;
        }
        else // Not Critical
        {
            //Debug.Log("NOT CRIT: " + combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration))));
            dmg = combat.Attack * (100 / (100 + Defense - (Defense * combat.Penetration)));
            Health -= (int)dmg;
        }

        if(Health <= 0)
        {
            soundManager.playSoundEffect(dieName);
            Die();
        }


        GameObject popup = ObjectPooler.Instance.SpawnFromPool("popup", gameObject.transform.position, Quaternion.identity);
        TextMeshPro tmp = popup.GetComponent<TextMeshPro>();
        tmp.text = ((int)dmg).ToString();
        if (isCrit)
        {
            tmp.fontSize = 5;
            tmp.faceColor = new Color(212, 1, 0);
            soundManager.playSoundEffect(damageName);
        }
        else
        {
            tmp.fontSize = 3;
            tmp.faceColor = new Color(212, 125, 0);
        }

    }

    protected virtual void Die()
    {
        // Do Something
    }

    public void LossStat(string stat)
	{
        switch (stat)
		{
            case "Health":
                Health -= 10;
                break;
            case "Attack":
                Attack -= 10;
                break;
            case "Defense":
                Defense -= 10;
                break;
            case "MovementSpeed":
                MovementSpeed -= 10;
                break;
            case "AttackSpeed":
                AttackSpeed -= 10;
                break;
            case "CriticalRate":
                CriticalRate -= 10;
                break;
            case "CriticalDamage":
                CriticalDamage -= 10;
                break;
            case "Penetration":
                Penetration -= 10;
                break;
        }
	}

    public void GetStat(string stat)
    {
        switch (stat)
        {
            case "Health":
                Health += 10;
                break;
            case "Attack":
                Attack += 10;
                break;
            case "Defense":
                Defense += 10;
                break;
            case "MovementSpeed":
                MovementSpeed += 10;
                break;
            case "AttackSpeed":
                AttackSpeed += 10;
                break;
            case "CriticalRate":
                CriticalRate += 10;
                break;
            case "CriticalDamage":
                CriticalDamage += 10;
                break;
            case "Penetration":
                Penetration += 10;
                break;
        }
    }
}
