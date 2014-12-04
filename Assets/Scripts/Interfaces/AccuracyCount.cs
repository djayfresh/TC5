using UnityEngine;
using System.Collections;
using System;

public class AccuracyCount : MonoBehaviour
{
    private SceneInfo gameUser;
    private float accuracyCount;
    private int screenWidth;
    private int screenHeight;
    private float AccuracyCountWidthLocation, AccuracyCountHeightLocation;
    private string playerAccuracyCountText;

    // Use this for initialization
    void Start()
    {
		gameUser = SceneInfo.info;
		if(gameUser == null)
		{
			gameUser = new SceneInfo() {Accuracy = -9001, LivesLeft = 9001, Score = -11};
		}
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        AccuracyCountWidthLocation = (screenWidth/3);
        AccuracyCountHeightLocation = (screenHeight) - ((screenHeight / 2));
        accuracyCount = gameUser.Accuracy;
        playerAccuracyCountText = String.Format("{0:0.}", accuracyCount) + "%";

    }

    void OnGUI()
    {
        GUI.depth = 1;
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = screenHeight / 10;
        myStyle.normal.textColor = Color.yellow;
        GUI.Label(
            new Rect(AccuracyCountWidthLocation, AccuracyCountHeightLocation, screenWidth, screenHeight),
            playerAccuracyCountText,
            myStyle);
    }
}