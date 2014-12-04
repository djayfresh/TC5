using UnityEngine;
using System.Collections;

public class PlayOnTrigger : MonoBehaviour {

	public ActionTrigger trigger;
	public int soundIndex;
	
	// Use this for initialization
	void Start () {
		ActionTrigger.OnTrigger += OnTrigger;
	}

	void OnTrigger(ActionTrigger t)
	{
		if(t.Equals(trigger))
		{
			Debug.Log("Audio Trigger");
			SoundManager.play(soundIndex);
		}
	}

	public void destory()
	{
		ActionTrigger.OnTrigger -= OnTrigger;
	}
}
