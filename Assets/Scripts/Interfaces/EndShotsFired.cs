using UnityEngine;
using System.Collections;
using System;

public class EndShotsFired : MonoBehaviour
{
    private SceneInfo gameUser;
    private float shotsFired = 0;
    private int screenWidth;
    private int screenHeight;
    private float shotsFiredWidthLocation, shotsFiredHeightLocation;
    private string shotsFiredText;

    // Use this for initialization
    void Start()
    {
        gameUser = FindObjectOfType<SceneInfo>();
        shotsFired = gameUser.Score;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        shotsFiredWidthLocation = (screenWidth / 3);
        shotsFiredHeightLocation = (screenHeight / 2) + ((screenHeight / 5));
        shotsFired = gameUser.Score;
        shotsFiredText = String.Format("{0:0.}", shotsFired);

    }

    void OnGUI()
    {
        GUI.depth = 1;
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = screenHeight / 10;
        myStyle.normal.textColor = Color.yellow;
        GUI.Label(
            new Rect(shotsFiredWidthLocation, shotsFiredHeightLocation, screenWidth, screenHeight),
            shotsFiredText,
            myStyle);
    }
}