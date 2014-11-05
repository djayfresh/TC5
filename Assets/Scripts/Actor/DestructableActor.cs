using UnityEngine;
using System.Collections;

public class DestructableActor : Actor {

	public override void OnDeath()
	{
		Destroy(this.gameObject);
	}
}
