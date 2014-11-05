using UnityEngine;
using System.Collections;
using System;

public class TimerScript : MonoBehaviour {

    private float timeLeft = 60;
    private int screenWidth;
    private int screenHeight;
    private float scoreWidthPosition, scoreHeightPosition;
    private string timerText;
	// Use this for initialization
	void Start () {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
        scoreWidthPosition = screenWidth - screenWidth / 4;
        scoreHeightPosition = screenHeight - screenHeight / 5;
	}
	
	// Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        scoreWidthPosition = screenWidth - screenWidth / 4;
        scoreHeightPosition = screenHeight - screenHeight / 5;

        timeLeft = timeLeft - Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
        }
        timerText = String.Format("{0:00.0}", timeLeft);
	}

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = screenHeight/5;
        myStyle.normal.textColor = Color.cyan;
        GUI.Label(
            new Rect(scoreWidthPosition, scoreHeightPosition, screenWidth, screenHeight), 
            timerText,
            myStyle);
    }
}
