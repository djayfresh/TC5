using UnityEngine;
using System.Collections;
using System;

public class FinalLivesCount : MonoBehaviour
{
    private SceneInfo gameUser;
    private float playerHealthRemaining = 0;
    private int screenWidth;
    private int screenHeight;
    private float healthRemainingWidthLocation, healthRemainingHeightLocation;
    private string playerHealthRemainingText;
    
	// Use this for initialization
	void Start () 
    {
        gameUser = FindObjectOfType<SceneInfo>();
        playerHealthRemaining = gameUser.LivesLeft;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
	}
	
	// Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        healthRemainingWidthLocation = (screenWidth / 3);
        healthRemainingHeightLocation = (screenHeight/2) + ((screenHeight/10.5f));
        playerHealthRemaining = gameUser.LivesLeft;
        playerHealthRemainingText = String.Format("{0:0.}", playerHealthRemaining);
	
	}

    void OnGUI()
    {
        GUI.depth = 1;
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = screenHeight / 10;
        myStyle.normal.textColor = Color.yellow;
        GUI.Label(
            new Rect(healthRemainingWidthLocation, healthRemainingHeightLocation, screenWidth, screenHeight),
            playerHealthRemainingText,
            myStyle);
    }
}
