using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
	private void Start()
	{
		Health = 50;
		Attack = 18;
		Defense = 0;
		MovementSpeed = 7;
		AttackSpeed = 1;
		CriticalRate = 10;
		CriticalDamage = 50;
		Penetration = 0;
	}
	protected override void Die()
	{
		Destroy(gameObject);
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

	public int GetStatByName(string name)
	{
		int stat = -1;
		switch (name)
		{
			case "Health":
				stat = Health;
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
				stat = 0;
				break;
			case "Defense":
				stat = 0;
				break;
			case "MovementSpeed":
				stat = 0;
				break;
			case "AttackSpeed":
				stat = 0;
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
				r = Methods.Max(Health - 50, GetMinStatByName(name));
				break;
			case "Attack":
				r = Methods.Max(Attack - 5, GetMinStatByName(name));
				break;
			case "Defense":
				r = Methods.Max(Defense - 20, GetMinStatByName(name));
				break;
			case "MovementSpeed":
				r = Methods.Max(MovementSpeed - 1, GetMinStatByName(name));
				break;
			case "AttackSpeed":
				r = Methods.Max(AttackSpeed - 1, GetMinStatByName(name));
				break;
			case "CriticalRate":
				r = Methods.Max(CriticalRate - 5, GetMinStatByName(name));
				break;
			case "CriticalDamage":
				r = Methods.Max(CriticalDamage - 5, GetMinStatByName(name));
				break;
			case "Penetration":
				r = Methods.Max(Penetration - 20, GetMinStatByName(name));
				break;
		}
		return r;
	}

	public void LossStat(string name)
	{
		switch (name)
		{
			case "Health":
				Health = TryLossStat(name);
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
				r = Health + 50;
				break;
			case "Attack":
				r = Attack + 5;
				break;
			case "Defense":
				r = Defense + 20;
				break;
			case "MovementSpeed":
				r = MovementSpeed + 1;
				break;
			case "AttackSpeed":
				r = AttackSpeed + 1;
				break;
			case "CriticalRate":
				r = CriticalRate + 5;
				break;
			case "CriticalDamage":
				r = CriticalDamage + 5;
				break;
			case "Penetration":
				r = Penetration + 20;
				break;
		}
		return r;
	}

	public void GetStat(string name)
	{
		switch (name)
		{
			case "Health":
				Health = TryGetStat(name);
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
