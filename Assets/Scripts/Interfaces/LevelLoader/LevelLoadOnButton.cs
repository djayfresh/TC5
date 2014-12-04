using UnityEngine;
using System.Collections;

public class LevelLoadOnButton : LevelLoader {

	public MenuButton buttonAction;
	// Use this for initialization
	void Start () {
		MenuButton.OnButtonPress += buttonFired;
	}


	void buttonFired(MenuButton button)
	{
		if(buttonAction.Equals(button))
		{
			loadLevel();
		}
	}

	void OnDestroy()
	{
		MenuButton.OnButtonPress -= buttonFired;
	}

	public override void loadLevel()
	{
		GameController.controller.nextLevel = levelNumber;
		Debug.Log("Loading screen load");
		Application.LoadLevel(5);
	}
}
