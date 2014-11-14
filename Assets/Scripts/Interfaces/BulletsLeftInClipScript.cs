using UnityEngine;
using System.Collections;
using System;

public class BulletsLeftInClipScript : MonoBehaviour {

    private Player gameUser;
    private int bulletsInClip;
    private int screenWidth;
    private int screenHeight;
    private float scoreWidthPosition, scoreHeightPosition;
    private string bulletsLeftText;
    private GUIText bulletsText;

	// Use this for initialization
    void Start()
    {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
        gameUser = FindObjectOfType<Player>();
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
        GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = (screenHeight / 5) - bulletsLeftText.Length * 5;
		myStyle.alignment = TextAnchor.MiddleCenter;
        myStyle.normal.textColor = Color.magenta;
        GUI.Label(
			new Rect(0, Screen.height - myStyle.fontSize, scoreWidthPosition, myStyle.fontSize),
            bulletsLeftText,
            myStyle);
    }
}
