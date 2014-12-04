using UnityEngine;
using System.Collections;

public abstract class LevelLoader : MonoBehaviour {

	public int levelNumber = -1;

	public abstract void loadLevel();
}
