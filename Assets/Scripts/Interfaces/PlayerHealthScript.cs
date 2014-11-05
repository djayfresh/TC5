using UnityEngine;
using System.Collections;
using System;

public class PlayerHealthScript : MonoBehaviour {
    private Player gameUser;
    private int playerHealthRemaining;
    private int screenWidth;
    private int screenHeight;
    private float healthRemainingWidthLocation, healthRemainingHeightLocation;
    private string playerHealthRemainingText;
    private GUIText bulletsText;

    // Use this for initialization
    void Start()
    {
        gameUser = FindObjectOfType<Player>();
        playerHealthRemaining = gameUser.health;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        healthRemainingWidthLocation = (screenWidth / 2) - (screenHeight / 10);
        healthRemainingHeightLocation = screenHeight - screenHeight / 5;
		playerHealthRemaining = gameUser.health;
		playerHealthRemainingText = String.Format("{0:0.}", playerHealthRemaining);
    }

    void OnGUI()
	{
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = screenHeight / 5;
        myStyle.normal.textColor = Color.yellow;
        GUI.Label(
            new Rect(healthRemainingWidthLocation, healthRemainingHeightLocation, screenWidth, screenHeight),
            playerHealthRemainingText,
            myStyle);
    }
}
