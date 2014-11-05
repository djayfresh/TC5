using UnityEngine;
using System.Collections;

public abstract class LevelLoader : MonoBehaviour {

	public string levelName;
	public int levelNumber = -1;

	public abstract void loadLevel();
}
