using UnityEngine;
using System.Collections;

public class WeaponPickup: MonoBehaviour {

	public ActionTrigger trigger;
	public Weapon weapon;
	public Player player;
	public GameObject[] destoryList;
	// Use this for initialization
	void Start () {
		ActionTrigger.OnTrigger += OnTrigger;
	}

	void OnTrigger(ActionTrigger other)
	{
		if(other.Equals(trigger))
		{
			if(player != null && weapon != null)
			{
				player.addWeapon(weapon);
				player.changeWeapon();
				destory();
			}
		}
	}

	public void destory()
	{
		foreach(GameObject g in destoryList)
		{
			Destroy(g);
		}
	}
}
