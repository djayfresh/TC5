using UnityEngine;
using System.Collections;

public abstract class Actor : MonoBehaviour {

	public int health;
	public bool inCover = false;
	public abstract void OnDeath();

	public void ApplyDamage(int damage)
	{
		if(!inCover)
		{
			health -= damage;
			//Debug.Log (this.name + "Has " + health + " health.");
			if(health <= 0)
			{
				OnDeath();
			}
		}
		else
		{
			//Debug.Log("Cover prevented dmg");
		}
	}

}
