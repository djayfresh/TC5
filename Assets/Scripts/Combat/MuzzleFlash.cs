using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {
	public static MuzzleFlash flash;

	public GameObject muzzleFlash;

	protected float activeLength = 1;
	protected float flashLength = 0.5f;

	void Awake()
	{
		if(flash == null)
		{
			flash = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		muzzleFlash.SetActive (false);
	}

	public static void showFlash()
	{
		if(flash.muzzleFlash != null)
		{
			flash.activeLength = 0;
		}
	}

	void Update()
	{
		activeLength += Time.deltaTime;
		muzzleFlash.SetActive (activeLength < flashLength);
		if(activeLength < flashLength)
		{
			Debug.Log("Showing Hit Flash");
		}
	}
}
