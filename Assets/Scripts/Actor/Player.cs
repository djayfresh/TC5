using UnityEngine;
using System.Collections;
using System;
using Windows.Kinect;

public class Player : Actor {

	public static Player player1;
	public static Player player2;
	public static int numPlayers = 0;

	public int score = 0;
	public Weapon[] weapons;
	public int currentWeapon;
	private int numberOfBulletsHit;
	private int numberOfHeadShots;
	public bool tracked = false;

	void Awake()
	{
		numPlayers++;
		if (player1 == null) 
		{
			player1 = this;
			//DontDestroyOnLoad(this);
		} 
		else if (player1 != this && player2 == null) 
		{
			player2 = this;
			//DontDestroyOnLoad(this);
			if(player2.name.Contains("1") && player1.name.Contains("2"))
			{
				Player tmp = Player.player1;
				Player.player1 = this;
				Player.player2 = tmp;
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start () 
	{
		inCover = false;
		numberOfBulletsHit = 0;
		Cover.OnCover += keyPressCover;
		ControllerKinect.playerHandLeftPhiz += reloadPlayer;
	}

	void keyPressCover(Player player, Cover cover)
	{
		getWeapon().reload();
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
			MuzzleFlash.showFlash();
			if(health <= 0)
			{
				OnDeath();
			}
		}
		else
		{
			Debug.Log("Cover prevented dmg");
		}
	}

	protected void reloadPlayer(Player activePlayer, Body body, JointType type)
	{
		if(activePlayer.Equals(this) && tracked)
		{
			if(weapons[currentWeapon] != null)
			{
				weapons[currentWeapon].reload();
				if(weapons[currentWeapon].outOfAmmo())
				{
					changeWeapon();
				}
			}
		}
	}

	public Weapon getWeapon()
	{
		return weapons[currentWeapon];
	}

	public void changeWeapon()
	{
		for(int w = 0; w < weapons.Length; w++)
		{
			if((weapons[w].Ammo > 0 || weapons[w].clipRemaining > 0)|| weapons[w].Ammo == Weapon.INFINITE_AMMO)
			{
				currentWeapon = w;
			}
		}
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
		EndGameOnDestroy eGOD = FindObjectOfType<EndGameOnDestroy>();
		if(eGOD != null)
		{
			Destroy(eGOD.gameObject);
		}
	}

	void EndGame()
	{
		GameController.storePlayerWeapons(this);
		if(this == Player.player1)
		{
			GameController.controller.victory = health > 0;

			SceneInfo.recordInfo (getAccuracy(), GameController.controller.score, health);
		}
	}

	void OnDestroy()
	{
		EndGame();
		Cover.OnCover -= keyPressCover;
		ControllerKinect.playerHandLeftPhiz -= reloadPlayer;
	}
}
