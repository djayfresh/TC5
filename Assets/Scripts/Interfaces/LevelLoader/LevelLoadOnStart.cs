using UnityEngine;
using System.Collections;

public class LevelLoadOnStart : LevelLoader {

	public float delay = 0;
	private float delayTime = 0;
	
	void Update()
	{
		delayTime += Time.deltaTime;
		if(delayTime > delay)
		{
			loadLevel();
		}
	}
	public override void loadLevel()
	{
		GameController.loadNextLevel();
	}
}
