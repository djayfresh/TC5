using UnityEngine;
using System.Collections;
using System;

public class BulletsLeftInClipScript : MonoBehaviour {

	public bool showUI = true;
    private Player gameUser;
    private int bulletsInClip;
    private int screenWidth;
    private int screenHeight;
    private float scoreWidthPosition, scoreHeightPosition;
    private string bulletsLeftText;
    private GUIText bulletsText;
	private float playerAmmoPostion = 0;
	private Color textColor = Color.magenta;
	// Use this for initialization
    void Start()
    {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
        gameUser = GetComponent<Player>();
		if(gameUser == null)
		{
			gameUser = new Player() {weapons = new Weapon[1]};
			gameUser.weapons[0] = new Weapon() {clipRemaining = 9001};
		}

        bulletsInClip = gameUser.getWeapon().clipRemaining;
	}
	
	// Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        scoreWidthPosition = screenWidth / 4;
        scoreHeightPosition = screenHeight - (screenHeight / 8);
		
		if(gameUser == Player.player2)
		{
			textColor = Color.cyan;
			playerAmmoPostion = screenWidth - scoreWidthPosition;

		}
		else if(gameUser == Player.player1)
		{

		}

		bulletsInClip = gameUser.getWeapon().clipRemaining;
		int ammo = gameUser.getWeapon().Ammo;
		bulletsLeftText = String.Format("{0:0.}", bulletsInClip);
		if(ammo >= 0)
		{
			bulletsLeftText += "/" + String.Format("{0:0.}", ammo);
		}
	}

    void OnGUI()
    {
		if(showUI)
		{
	        GUIStyle myStyle = new GUIStyle();
			myStyle.fontSize = (screenHeight / 5) - bulletsLeftText.Length * 5;
			myStyle.alignment = TextAnchor.MiddleCenter;
	        myStyle.normal.textColor = textColor;
			if(!gameUser.tracked)
			{
				myStyle.normal.textColor = Color.gray;
			}
	        GUI.Label(
				new Rect(playerAmmoPostion, Screen.height - myStyle.fontSize, scoreWidthPosition, myStyle.fontSize),
	            bulletsLeftText,
	            myStyle);
		}
    }
}
