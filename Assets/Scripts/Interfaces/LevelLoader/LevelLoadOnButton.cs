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
		if(levelName.Length == 0 && levelNumber >= 0)
		{
			Application.LoadLevel(levelNumber);
		}
		else if(levelName.Length > 0)
		{
			Application.LoadLevel(levelName);
		}
		else
		{
			Debug.LogError("Level Failed to load");
		}
	}
}
