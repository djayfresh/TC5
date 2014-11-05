using UnityEngine;
using System.Collections;

public class Player : Actor {

	public int score;
	public Weapon[] weapon;
	public int currentWeapon;
	// Use this for initialization
	void Start () 
	{
		health = 7;
		inCover = false;
	}

	public int shotsFired()
	{
		int shotCount = 0;
		foreach(Weapon w in weapon)
		{
			shotCount += w.numBulletsFired();
		}

		return shotCount;
	}

	public void ApplyDamage(int damage)
	{

		if(!inCover)
		{
			health -= damage;
			weapon[currentWeapon].showFlash();
			//Debug.Log (this.name + "Has " + health + " health.");
			if(health <= 0)
			{
				OnDeath();
			}
		}
		else
		{
			//Debug.Log("Cover prevented dmg");
		}
	}

	public Weapon getWeapon()
	{
		return weapon[currentWeapon];
	}

	public override void OnDeath()
	{
		//SceneInfo si = FindObjectOfType<SceneInfo> ();
		//si.Accuracy = 35;
		//si.Score = score;
		//si.LivesLeft = 32;
		EndGameOnDestroy eGOD = FindObjectOfType<EndGameOnDestroy>();
		if(eGOD != null)
		{
			Destroy(eGOD.gameObject);
		}
		Application.LoadLevel("Credits");
	}
}
