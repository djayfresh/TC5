using UnityEngine;
using System.Collections;

public class WeaponShooter : MonoBehaviour {

	public DestroyTrigger trigger;
	public Weapon weapon;
	public Player player;
	// Use this for initialization
	void Start () {
		DestroyTrigger.OnTrigger += HandleOnTrigger;
	}

	void HandleOnTrigger (DestroyTrigger e)
	{
		if(e.Equals(trigger))
		{
			if(player != null && weapon != null)
			{
				player.addWeapon(weapon);
				player.changeWeapon();
			}
			Destroy(this.gameObject);
		}
	}
}
