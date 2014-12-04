using UnityEngine;
using System.Collections;

public class Enemy : Actor 
{
	public Weapon weapon;
	public bool shouldShoot = true;
	public bool trackPlayer = true;
	public float bulletHandicap = 0.65f; // the higher the value the more time between shots
	
	protected GameObject muzzleFlare;
	protected Vector3 lastFramePosition;
	protected float timefromLastShot;

	protected bool bulletInc = false;
	protected float bulletTimer = 0;
	protected float coverTimer = 0;
	protected MoveToFrom mtf;
	protected BoxCollider headCollider;
	protected Transform HealthBar;

	protected bool canCover;
	protected float timeToCover;
	protected float inCoverTimer;
	void Start () 
	{
		maxHealth = health;
		Cover.OnCover += OnCover;
		Cover.OnExitCover += OnExit;
		mtf = GetComponent<MoveToFrom> ();

		timeToCover = Random.Range (2, 5);

		foreach(Transform t in transform)
		{
			if(t.name == "HealthBar")
				HealthBar = t;
		}
		HealthBar.localScale = Vector3.zero;
		findHeadCollider();
	}

	void findHeadCollider()
	{
		BoxCollider[] colliders = GetComponents<BoxCollider>();
		Vector3 smallCenter = Vector3.zero;
		float smallDist = 0;
		if(colliders.Length > 1)
		{
			foreach(BoxCollider c in colliders)
			{
				float thisDist = Vector3.Distance(c.center * 1000, smallCenter * 1000);
				if(thisDist > smallDist)
				{
					smallDist = thisDist;
					headCollider = c;
				}
			}
		}
	}
	float newRandomFloat ()
	{
		return (Random.value - 0.5f) * weapon.spread;
	}

	void OnCover(Player player, Cover c)
	{
		coverTimer = 0;
	}

	void OnExit(Player Player, Cover c)
	{
		if(bulletInc && coverTimer < bulletHandicap)
		{
			bulletInc = false;
			bulletTimer = bulletHandicap;
			timefromLastShot = 0;
		}
		coverTimer += Time.deltaTime;
	}
	// Update is called once per frame
	void Update () 
	{
		if(mtf != null)
		{
			canCover = mtf.canCover ();
		}
		if(canCover && timeToCover < inCoverTimer)
		{
			if(inCover)
				leaveCover();
			else
				goIntoCover();
			inCoverTimer = 0;
		}
		inCoverTimer += Time.deltaTime;
		if(health != maxHealth && HealthBar != null)
			HealthBar.localScale = new Vector3(0.0033f * health/maxHealth,.0033f,.0033f);
		
		if(weapon != null && weapon.bullet != null)
		{
			shouldShoot = mtf.shouldShoot ();
			Vector3 playerPosition = Camera.main.transform.position;

			if(timefromLastShot > weapon.fireRate && shouldShoot && mtf.hasMoved())
			{
				timefromLastShot = 0;
				Vector3 randomVector = new Vector3(newRandomFloat (), newRandomFloat(), newRandomFloat());

				float dist = Vector3.Distance(randomVector, Vector3.zero);
				if(dist < Random.value && 
				   !Physics.Linecast(weapon.transform.position + transform.forward, playerPosition - transform.forward) && !bulletInc)
				{
					SoundManager.playEnemyFire();
					weapon.showFlash();
					bulletInc = true;
				}
				else
				{
					playerPosition += randomVector;
				}				
			}

			if(bulletInc)
			{
				bulletTimer -= Time.deltaTime;
				if(bulletTimer <= 0)
				{
					bulletInc = false;
					if(Player.player1 != null)
					{
						Player.player1.SendMessage("ApplyDamage",weapon.bullet.damage,SendMessageOptions.RequireReceiver);
					}
					if(Player.player2 != null)
					{
						Player.player2.SendMessage("ApplyDamage",weapon.bullet.damage,SendMessageOptions.RequireReceiver);
					}
				}
			}
			else
			{
				bulletTimer = bulletHandicap;
			}
		}
		timefromLastShot += Time.deltaTime;
	}
	private void goIntoCover()
	{
		inCover = true;
		transform.position -= new Vector3 (0, 5, 0);
	}
	private void leaveCover()
	{
		inCover = false;
		transform.position += new Vector3 (0, 5, 0);
	}
	bool hasMoved()
	{
		bool moved = lastFramePosition != transform.position;
		lastFramePosition = transform.position;
		return moved;
	}

	public bool wasHeadShot(BoxCollider whatWeHit)
	{
		if(headCollider != null && headCollider.Equals(whatWeHit))
		{
			return true;
		}
		return false;
	}

	public override void OnDeath()
	{
		GameController.controller.score++;

		SoundManager.playEnemyDeath ();
		//Debug.Log("Score: " + playerClass.score);
		Destroy(this.gameObject);
	}

	public void OnDestroy()
	{
		Cover.OnCover -= OnCover;
		Cover.OnExitCover -= OnExit;
	}
}
