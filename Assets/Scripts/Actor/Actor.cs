using UnityEngine;
using System.Collections;

public abstract class Actor : MonoBehaviour {

	public float health;
	public bool inCover = false;
	public abstract void OnDeath();
	protected float maxHealth;
	
	public void ApplyDamage(float damage)
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
