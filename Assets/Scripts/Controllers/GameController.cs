using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public static GameController controller;
	
	public bool victory = true;
	public bool debugMouse = false;

	public int nextLevel;

	public Weapon[] p1Weapons;
	public Weapon[] p2Weapons;

	public int score;

	void Awake()
	{
		if (controller == null) 
		{
			DontDestroyOnLoad (gameObject);
			controller = this;
		} 
		else if (controller != this) 
		{
			Destroy (gameObject);
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			GameController.controller.debugMouse = !GameController.controller.debugMouse;
		}
		if(GameController.controller.debugMouse && Player.player1 != null)
		{
			Player.player1.tracked = true;	
		}
	}

	public static void storePlayerWeapons(Player player)
	{
		if(player == Player.player1)
		{
			controller.p1Weapons = player.weapons;
		}
		else if(player == Player.player2)
		{
			controller.p2Weapons = player.weapons;
		}
	}

	public static int getNextLevel()
	{
		return controller.nextLevel;
	}

	public static void loadNextLevel()
	{
		Debug.Log("Loading Level: " + controller.nextLevel);
		Application.LoadLevel(controller.nextLevel);
	}
}
