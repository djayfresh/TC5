using UnityEngine;
using System.Collections;

public class EndGameOnDestroy : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy()
	{
		PathedCamera player = FindObjectOfType<PathedCamera> ();
		SceneInfo si = FindObjectOfType<SceneInfo> ();
		if(player != null && si != null)
		{
			si.recordInfo (-1, player.score, player.health);
		}
		else if(si != null)
		{
			si.recordInfo(-1, 9001, -1);
		}
		Debug.Log ("Object Destroyed");
		Application.LoadLevel("Credits");
	}
}
