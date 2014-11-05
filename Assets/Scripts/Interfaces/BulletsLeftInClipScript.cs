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
        bulletsInClip = gameUser.getWeapon().clipRemaining;
	}
	
	// Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        scoreWidthPosition = screenWidth / 8;
        scoreHeightPosition = screenHeight - screenHeight / 5;
		bulletsInClip = gameUser.getWeapon().clipRemaining;
		bulletsLeftText = String.Format("{0:0.}", bulletsInClip);
	}

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = screenHeight / 5;
        myStyle.normal.textColor = Color.magenta;
        GUI.Label(
            new Rect(scoreWidthPosition, scoreHeightPosition, screenWidth, screenHeight),
            bulletsLeftText,
            myStyle);
    }
}
