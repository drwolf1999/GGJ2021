using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
	private UI uiManager;
	private void Awake()
	{
		ResetStat();

		uiManager = GameObject.Find("UI").GetComponent<UI>();
	}

	public void ResetStat()
	{
		MaxHealth = 200;
		Health = MaxHealth;
		Attack = 20;
		Defense = 0;
		MovementSpeed = 7;
		AttackSpeed = 300;
		CriticalRate = 10;
		CriticalDamage = 50;
		Penetration = 0;
	}


	protected override void Die()
	{
		uiManager.SubMenuToggle();
		this.gameObject.SetActive(false);
/*		Destroy(gameObject);*/
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "EnemyBullet")
		{
			//Combat enemyCombat = collision.gameObject.GetComponentInParent(typeof(Combat)) as Combat;
			Combat enemyCombat = collision.gameObject.GetComponent<EnemyBulletMove>().GetCombatStats();
			GetDamage(enemyCombat);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyMelee")
        {
			Combat enemyCombat = collision.gameObject.GetComponent<Combat>();
			GetDamage(enemyCombat);
			collision.gameObject.GetComponent<EnemyMelee>().SwitchCollider();
		}
    }

    public int GetStatByName(string name)
	{
		int stat = -1;
		switch (name)
		{
			case "Health":
				stat = MaxHealth;
				break;
			case "Attack":
				stat = Attack;
				break;
			case "Defense":
				stat = Defense;
				break;
			case "MovementSpeed":
				stat = MovementSpeed;
				break;
			case "AttackSpeed":
				stat = AttackSpeed;
				break;
			case "CriticalRate":
				stat = CriticalRate;
				break;
			case "CriticalDamage":
				stat = CriticalDamage;
				break;
			case "Penetration":
				stat = Penetration;
				break;
		}
		return stat;
	}

	public int GetMinStatByName(string name)
	{
		int stat = -1;
		switch (name)
		{
			case "Health":
				stat = 50;
				break;
			case "Attack":
				stat = 5;
				break;
			case "Defense":
				stat = 0;
				break;
			case "MovementSpeed":
				stat = 1;
				break;
			case "AttackSpeed":
				stat = 10;
				break;
			case "CriticalRate":
				stat = 0;
				break;
			case "CriticalDamage":
				stat = 0;
				break;
			case "Penetration":
				stat = 0;
				break;
		}
		return stat;
	}

	public int TryLossStat(string name)
	{
		int r = 0;
		switch (name)
		{
			case "Health":
				r = Methods.Max(MaxHealth - 50, GetMinStatByName(name));
				break;
			case "Attack":
				r = Methods.Max(Attack - 5, GetMinStatByName(name));
				break;
			case "Defense":
				r = Methods.Max(Defense - 15, GetMinStatByName(name));
				break;
			case "MovementSpeed":
				r = Methods.Max(MovementSpeed - 1, GetMinStatByName(name));
				break;
			case "AttackSpeed":
				r = Methods.Max(AttackSpeed - 50, GetMinStatByName(name));
				break;
			case "CriticalRate":
				r = Methods.Max(CriticalRate - 5, GetMinStatByName(name));
				break;
			case "CriticalDamage":
				r = Methods.Max(CriticalDamage - 10, GetMinStatByName(name));
				break;
			case "Penetration":
				r = Methods.Max(Penetration - 3, GetMinStatByName(name));
				break;
		}
		return r;
	}

	public void LossStat(string name)
	{
		switch (name)
		{
			case "Health":
				MaxHealth = TryLossStat(name);
				break;
			case "Attack":
				Attack = TryLossStat(name);
				break;
			case "Defense":
				Defense = TryLossStat(name);
				break;
			case "MovementSpeed":
				MovementSpeed = TryLossStat(name);
				break;
			case "AttackSpeed":
				AttackSpeed = TryLossStat(name);
				break;
			case "CriticalRate":
				CriticalRate = TryLossStat(name);
				break;
			case "CriticalDamage":
				CriticalDamage = TryLossStat(name);
				break;
			case "Penetration":
				Penetration = TryLossStat(name);
				break;
		}
	}

	public int TryGetStat(string name)
	{
		int r = 0;
		switch (name)
		{
			case "Health":
				r = MaxHealth + 70;
				break;
			case "Attack":
				r = Attack + 7;
				break;
			case "Defense":
				r = Defense + 20;
				break;
			case "MovementSpeed":
				r = MovementSpeed + 1;
				break;
			case "AttackSpeed":
				r = AttackSpeed + 70;
				break;
			case "CriticalRate":
				r = CriticalRate + 7;
				break;
			case "CriticalDamage":
				r = CriticalDamage + 14;
				break;
			case "Penetration":
				r = Penetration + 5;
				break;
		}
		return r;
	}

	public void GetStat(string name)
	{
		switch (name)
		{
			case "Health":
				MaxHealth = TryGetStat(name);
				break;
			case "Attack":
				Attack = TryGetStat(name);
				break;
			case "Defense":
				Defense = TryGetStat(name);
				break;
			case "MovementSpeed":
				MovementSpeed = TryGetStat(name);
				break;
			case "AttackSpeed":
				AttackSpeed = TryGetStat(name);
				break;
			case "CriticalRate":
				CriticalRate = TryGetStat(name);
				break;
			case "CriticalDamage":
				CriticalDamage = TryGetStat(name);
				break;
			case "Penetration":
				Penetration = TryGetStat(name);
				break;
		}
	}
}
