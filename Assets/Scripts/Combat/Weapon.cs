using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float fireRate;
	public float spread;
	public Bullet bullet;
	public int Ammo;
	private int initalAmmo;
	public GameObject muzzleFlash;
	public static int INFINITE_AMMO = -1;
    public int maxClipSize;
    public int clipRemaining;
	public float headShotMultiplier = 2;

	protected float activeLength = 1;
	protected float flashLength = .5f;

	public void Start()
	{
		initalAmmo = Ammo;
		muzzleFlash.SetActive (false);
		reload();
	}

	public int numBulletsFired()
	{
		return initalAmmo - (Ammo + clipRemaining);
	}
	public void Update()
	{
		activeLength += Time.deltaTime;
		muzzleFlash.SetActive (activeLength < flashLength);
	}

	public void reload()
	{
		if(Ammo > 0 && (clipRemaining != maxClipSize || clipRemaining != Ammo))
		{
			int reloadAmmo = Mathf.Min(maxClipSize, Ammo + clipRemaining);
			Ammo = (Ammo - reloadAmmo) + clipRemaining;
			clipRemaining = reloadAmmo;
		}
		else if(Ammo == INFINITE_AMMO && clipRemaining != maxClipSize)
		{
			initalAmmo += maxClipSize - clipRemaining;
			clipRemaining = maxClipSize;
		}
	}

	public bool outOfAmmo()
	{
		return Ammo == 0 && clipRemaining == 0;
	}

	public void showFlash()
	{
		if(muzzleFlash != null)
			activeLength = 0;
	}
}

