using UnityEngine;
using System.Collections;
using System;

public class EndGameOnDestroy : MonoBehaviour {

	public int nextLevel;
	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy()
	{
		GameController.controller.nextLevel = nextLevel;
		Application.LoadLevel("Credits");
	}
}
