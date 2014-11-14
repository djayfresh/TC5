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

	protected GameObject player;

	protected bool bulletInc = false;
	protected float bulletTimer = 0;
	protected float coverTimer = 0;
	protected MoveToFrom mtf;
	protected Player playerClass;
	protected SoundManager sm;
	protected BoxCollider headCollider;
	protected Transform HealthBar;
	// Use this for initialization
	void Start () 
	{
		maxHealth = health;
		Cover.OnCover += OnCover;
		Cover.OnExitCover += OnExit;
		mtf = GetComponent<MoveToFrom> ();
		player = Camera.main.gameObject;
		playerClass = player.GetComponent<Player> ();
		sm = FindObjectOfType<SoundManager> ();

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
			Debug.Log("Found Head: " + (headCollider.center * 1000));
		}
	}
	float newRandomFloat ()
	{
		return (Random.value - 0.5f) * weapon.spread;
	}

	void OnCover(Cover c)
	{
		coverTimer = 0;
	}

	void OnExit(Cover c)
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
		if(health != maxHealth)
			HealthBar.localScale = new Vector3(0.0033f * health/maxHealth,.0033f,.0033f);
		
		if(weapon != null && weapon.bullet != null)
		{

			shouldShoot = mtf.shouldShoot ();
			Vector3 playerPosition = player.transform.position;

			if(timefromLastShot > weapon.fireRate && shouldShoot && mtf.hasMoved())
			{
				timefromLastShot = 0;
				Vector3 randomVector = new Vector3(newRandomFloat (), newRandomFloat(), newRandomFloat());
				//Vector3 randomVector = new Vector3(0, 0, 0);

				float dist = Vector3.Distance(randomVector, Vector3.zero);
				if(dist < Random.value && 
				   !Physics.Linecast(weapon.transform.position + transform.forward, playerPosition - transform.forward) && !bulletInc)
				{
					sm.playEnemyFire();
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
					if(playerClass != null)
					{
						playerClass.SendMessage("ApplyDamage",weapon.bullet.damage,SendMessageOptions.DontRequireReceiver);
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
		playerClass.score++;

		sm.playEnemyDeath ();
		//Debug.Log("Score: " + playerClass.score);
		Destroy(this.gameObject);
	}

	public void OnDestroy()
	{
		Cover.OnCover -= OnCover;
		Cover.OnExitCover -= OnExit;
	}
}
