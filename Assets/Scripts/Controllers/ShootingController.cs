using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;
//Should be able to be attached to anything.
//Prefab for bullet/projectile should have a Rigidbody

public class ShootingController : MonoBehaviour 
{
	private Weapon weapon;
	private ReticleMovement crosshairs;
	private float timeFromlastShot = 0;
	public bool canShoot = true;
    public ParticleEmitter enemyHitEffect;
    public ParticleEmitter enemyMissEffect;
	private bool triggerHeld = false;
	private Player thisPlayer;
	public WeaponChangeText showPistol;

	// Use this for initialization
	void Start () 
	{
		ControllerKinect.playerHandInPhiz += handInPhiz;
		ControllerKinect.playerHandLeftPhiz += handLeftPhiz;
		WaitScript.onTextShow += shooting;
		thisPlayer = GetComponent<Player>();
	}

	void handLeftPhiz (Player player, Body body, JointType hand)
	{
		if(crosshairs != null && player.Equals(thisPlayer))
		{
			crosshairs.showCrosshair(false);
		}
	}
	public void enableShooting()
	{
		canShoot = true;
	}
	public void disableShooting()
	{
		canShoot = false;
	}

	void shooting(bool onOff)
	{
		canShoot = !onOff;
	}

	void Update () 
	{
		if(thisPlayer != null)
		{
			weapon = thisPlayer.getWeapon();
			crosshairs = thisPlayer.GetComponent<ReticleMovement>();
		}
		if(Input.GetButtonDown("Fire1"))
		{
			triggerHeld = true;
		}
		else if(Input.GetButtonUp("Fire1"))
		{
			triggerHeld = false;
		}

		if(GameController.controller.debugMouse && thisPlayer.tracked)
		{
			crosshairs.showCrosshair(true);
			if (triggerHeld && weapon != null && timeFromlastShot > weapon.fireRate && weapon.clipRemaining > 0 && canShoot)
		    {
				fireWeapon();
		    }
			else if(triggerHeld && weapon != null && timeFromlastShot > weapon.fireRate && weapon.clipRemaining <= 0 && canShoot)
			{
				SoundManager.playDryFire();
				timeFromlastShot = 0;
			}

			if(weapon != null && weapon.outOfAmmo() && thisPlayer != null)
			{
				thisPlayer.changeWeapon();
				if(thisPlayer.currentWeapon == 0 && thisPlayer.getWeapon().Ammo == Weapon.INFINITE_AMMO && showPistol != null)
				{
					showPistol.showWaitTexture();
				}
				timeFromlastShot = 0;
			}
		}

		timeFromlastShot += Time.deltaTime;

	}

	void handInPhiz(Player shootingPlayer, Body body, JointType type, Vector2 p)
	{
		if(shootingPlayer.Equals(thisPlayer))
		{
			weapon = shootingPlayer.getWeapon();
			if(crosshairs != null)
			{
				crosshairs.showCrosshair(thisPlayer.tracked);
			}
			bool handOpen = false;
			if(type == JointType.HandRight)
			{
				handOpen = body.HandRightState.Equals(HandState.Open);
			}
			else 
			{
				handOpen = body.HandLeftState.Equals(HandState.Open);
			}
			var crosshairsPosition = new Vector3 (p.x * Screen.width, p.y * Screen.height, 0);
			if(crosshairs != null)
			{
				crosshairs.setPosition(new Vector2(crosshairsPosition.x, crosshairsPosition.y));
			}
			if (weapon != null && handOpen && timeFromlastShot > weapon.fireRate && weapon.clipRemaining > 0) 
			{
				if(canShoot)
				{
					fireWeapon();
				}
			}
			if(weapon != null && handOpen && timeFromlastShot > weapon.fireRate && weapon.clipRemaining <= 0)
			{
				if(canShoot)
				{
					SoundManager.playDryFire();
					timeFromlastShot = 0;	
				}
			}

			if(!handOpen)
			{
				if(weapon != null)
					timeFromlastShot = weapon.fireRate + 1;
				else
					timeFromlastShot = 0;
			}
		}
	}

	void fireWeapon()
	{
		Vector3 bulletDirection = Camera.main.ScreenPointToRay
			(new Vector3(crosshairs.getScreenPositionCentered().x , crosshairs.getScreenPositionCentered().y,0)).direction;
		fireExplosion(Camera.main.ScreenToWorldPoint(crosshairs.getScreenPositionCentered()), (bulletDirection));
		weapon.clipRemaining--;
		timeFromlastShot = 0;
		SoundManager.play(3);
	}

	void fireExplosion(Vector3 origin, Vector3 direction)
	{
	    RaycastHit[] rays = Physics.RaycastAll(new Ray(origin, direction));
	    Vector3 collisionOn = origin;
		Collider collider = null;
	    Actor e = null;
	    float dist = float.MaxValue;
	    for (int r = 0; r < rays.Length; r++)
	    {
	        if (rays[r].distance < dist)
	        {
				collider = rays[r].collider;
	       	 	collisionOn = rays[r].point;
	            dist = rays[r].distance;
	            e = rays[r].collider.GetComponent<Actor>();
	        }
	    }
		if(weapon.bullet != null)
		{
		    hitEnemy(e, weapon.bullet.damage);
			if(checkForHeadShot(e as Enemy, collider))
			{
				hitEnemy(e, weapon.bullet.damage * weapon.headShotMultiplier);
			}
		}
	    Vector3 backVector = Vector3.Normalize(-direction);
	    float distBack = 0;
	    Quaternion rotator = Quaternion.FromToRotation(collisionOn, collisionOn + backVector);

	    ParticleEmitter pe;
		if (enemyHitEffect != null && enemyMissEffect != null)
		{
            if (e != null)
            {
                pe = (GameObject.Instantiate(enemyHitEffect.gameObject, collisionOn + (backVector * distBack), rotator) as GameObject).GetComponent<ParticleEmitter>();
            }
            else
            {
                pe = (GameObject.Instantiate(enemyMissEffect.gameObject, collisionOn + (backVector * distBack), rotator) as GameObject).GetComponent<ParticleEmitter>();
            }
            if (pe != null)
            {
                pe.worldVelocity = backVector * distBack;
                pe.Emit();
            }
		}
	}

	void hitEnemy(Actor e, float damage)
	{		
		if(e != null)
		{
			if(thisPlayer != null)
			{
				thisPlayer.hitEnemy();
				thisPlayer.getAccuracy();
			}
			e.ApplyDamage(damage);
		}
	}

	bool checkForHeadShot(Enemy enemy, Collider whatWeHit)
	{
		if(enemy != null && whatWeHit != null)
		{
			if(enemy.wasHeadShot(whatWeHit as BoxCollider))
			{
				//Play better animation
				//Explosion
				//Sound
				if(thisPlayer != null)
				{
					thisPlayer.headShot();
				}
				Debug.Log("HEAD SHOT!");
				//enemy.OnDeath();
				return true;
			}
		}
		return false;
	}
	
	void OnDestroy()
	{
		ControllerKinect.playerHandInPhiz -= handInPhiz;
		ControllerKinect.playerHandLeftPhiz -= handLeftPhiz;
		WaitScript.onTextShow -= shooting;
	}
}
