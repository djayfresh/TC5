using UnityEngine;
using System.Collections;
using System;

public class EndGameOnDestroy : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy()
	{
		Player player = FindObjectOfType<Player> ();
		SceneInfo si = FindObjectOfType<SceneInfo> ();
		if(player != null && si != null)
		{
			si.recordInfo (player.getAccuracy(), player.score, player.getHeadShots());
		}
		else if(si != null)
		{
			si.recordInfo(-1, 9001, -1);
		}
		Application.LoadLevel("Credits");
	}
}
