using UnityEngine;
using System.Collections;

public class DestroyTrigger : DestructableActor {

	public delegate void OnTriggerEvent(DestroyTrigger e);
	public static event OnTriggerEvent OnTrigger;

	public override void OnDeath ()
	{
		if(OnTrigger != null)
		{
			OnTrigger(this);
		}
		Destroy(this.gameObject);
	}
}
