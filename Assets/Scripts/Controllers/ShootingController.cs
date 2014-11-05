using UnityEngine;
using System.Collections;
using Windows.Kinect;
//Should be able to be attached to anything.
//Prefab for bullet/projectile should have a Rigidbody

public class ShootingController : MonoBehaviour 
{
	public ControllerKinect kinect;

	private Weapon weapon;
	private ReticleMovement crosshairs;
	private float timeFromlastShot = 0;
	public bool canShoot = true;
	private SoundManager soundManager;
	public ParticleEmitter explosion;
	//private InteractionHandPointer ihp;
	// Use this for initialization
	void Start () 
	{
		ControllerKinect.playerHandInPhiz += handInPhiz;
		ControllerKinect.playerHandLeftPhiz += reloadGun;
		WaitScript.onTextShow += shooting;
		crosshairs = FindObjectOfType<ReticleMovement>();
		soundManager = FindObjectOfType<SoundManager>();
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

	void reloadGun(Body body, JointType joint)
	{
		if(weapon != null)
			weapon.reload();
	}
	// Update is called once per frame
	void Update () 
	{
		Player player = FindObjectOfType<Player>();
		if(player != null)
		{
			weapon = player.getWeapon();
		}
		if (Input.GetButtonDown("Fire1") && weapon != null && weapon.clipRemaining > 0 && canShoot)
	    {
			fireWeapon();
	    }

		timeFromlastShot += Time.deltaTime;

	}

	void handInPhiz(Body body, JointType type, Vector2 p)
	{
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

		if(!handOpen)
		{
			if(weapon != null)
				timeFromlastShot = weapon.fireRate + 1;
			else
				timeFromlastShot = 0;
		}
	}

	void fireWeapon()
	{
		Vector3 bulletDirection = Camera.main.ScreenPointToRay
			(new Vector3(crosshairs.getScreenPositionCentered().x , crosshairs.getScreenPositionCentered().y,0)).direction;
		fireExplosion(Camera.main.ScreenToWorldPoint(crosshairs.getScreenPositionCentered()), (bulletDirection));
		weapon.clipRemaining--;
		timeFromlastShot = 0;
		soundManager.playPistolSound();
	}

	void fireExplosion(Vector3 origin, Vector3 direction)
	{
		if(explosion != null)
		{
			RaycastHit[] rays = Physics.RaycastAll(new Ray(origin, direction));
			Vector3 collisionOn = origin;
			Actor e = null;
			float dist = float.MaxValue;
			Collider collider = null;
			for(int r = 0; r < rays.Length; r++)
			{
				if(rays[r].distance < dist)
				{
					collisionOn = rays[r].point;
					dist = rays[r].distance;
					e = rays[r].collider.GetComponent<Actor>();
					collider = rays[r].collider;
				}
			}
			hitEnemy(e, weapon.bullet.damage);
			checkForHeadShot(e as Enemy, collider);
			Vector3 backVector = Vector3.Normalize(-direction);
			//float distBack = 9;
			//Quaternion rotator = Quaternion.FromToRotation(collisionOn, collisionOn + (backVector * distBack));
			//ParticleEmitter pe = (GameObject.Instantiate(explosion.gameObject, collisionOn + (backVector * distBack), rotator)as GameObject).GetComponent<ParticleEmitter>();
			Quaternion rotator = Quaternion.FromToRotation(collisionOn, collisionOn);
			ParticleEmitter pe = (GameObject.Instantiate(explosion.gameObject, collisionOn, rotator)as GameObject).GetComponent<ParticleEmitter>();
			
			if(pe != null)
			{
				pe.Emit();
			}
		}
	}

	void hitEnemy(Actor e, int damage)
	{		
		if(e != null)
		{
			e.ApplyDamage(damage);

		}
	}

	void checkForHeadShot(Enemy enemy, Collider whatWeHit)
	{
		if(enemy != null)
		{
			if(enemy.wasHeadShot(whatWeHit as BoxCollider))
			{
				//Play better animation
				//Explosion
				//Sound
				Debug.Log("HEAD SHOT!");
				enemy.OnDeath();
			}
		}
	}
	
	void OnDestroy()
	{
		ControllerKinect.playerHandInPhiz -= handInPhiz;
		ControllerKinect.playerHandLeftPhiz -= reloadGun;
		WaitScript.onTextShow -= shooting;
	}
}
