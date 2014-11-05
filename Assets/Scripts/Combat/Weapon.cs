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
		return initalAmmo - Ammo;
	}
	public void Update()
	{
		activeLength += Time.deltaTime;
		muzzleFlash.SetActive (activeLength < flashLength);
	}

	public void reload()
	{
		if(Ammo != INFINITE_AMMO)
		{
			Ammo -= maxClipSize - clipRemaining;
			clipRemaining = Mathf.Min(Ammo, maxClipSize);
		
		}
		else
			clipRemaining = maxClipSize;
	}

	public void showFlash()
	{
		if(muzzleFlash != null)
			activeLength = 0;
	}
}

