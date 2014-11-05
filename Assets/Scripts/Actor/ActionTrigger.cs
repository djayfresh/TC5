using UnityEngine;
using System.Collections;

public class ActionTrigger : MonoBehaviour {

	public delegate void OnTriggerEvent(ActionTrigger e);
	public static event OnTriggerEvent OnTrigger;

	public Collider onCollideWith;
	public bool notifyOnExit = false;
	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other)
	{
		if(other == onCollideWith && OnTrigger != null)
		{
			Debug.Log("Trigger Volumn Hit");
			OnTrigger(this);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(onCollideWith != null)
		{
			if(other.gameObject == onCollideWith.gameObject && OnTrigger != null && notifyOnExit)
			{
				OnTrigger(this);
			}
		}
	}
}
