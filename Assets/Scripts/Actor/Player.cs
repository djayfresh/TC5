using UnityEngine;
using System.Collections;
using System;

public class Player : Actor {

	public int score;
	public Weapon[] weapons;
	public int currentWeapon;
	private int numberOfBulletsHit;
	private int numberOfHeadShots;
	// Use this for initialization
	void Start () 
	{
		health = 7;
		inCover = false;
		numberOfBulletsHit = 0;
	}

	public int shotsFired()
	{
		int shotCount = 0;
		foreach(Weapon w in weapons)
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
			weapons[currentWeapon].showFlash();
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
		return weapons[currentWeapon];
	}

	public void changeWeapon()
	{
		currentWeapon = (currentWeapon + 1) % weapons.Length;
	}

	public void addWeapon(Weapon weapon)
	{
		Weapon[] newArray = new Weapon[weapons.Length + 1];
		for(int i = 0; i < weapons.Length; i++)
		{
			newArray[i] = weapons[i];
		}
		newArray[weapons.Length] = weapon;

		weapons = newArray;
	}

	public void hitEnemy()
	{
		numberOfBulletsHit++;
	}

	public void headShot()
	{
		numberOfBulletsHit--;
		numberOfHeadShots++;
	}

	public float getAccuracy()
	{
		int shots = shotsFired();
		if(shots == 0)
		{
			return 100.0f;
		}
		return (numberOfBulletsHit / (shots * 1.0f)) * 100.0f;
	}

	public int getHeadShots()
	{
		return numberOfHeadShots;
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
