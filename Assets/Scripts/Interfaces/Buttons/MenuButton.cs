using UnityEngine;
using System.Collections;
using System;

public class MenuButton : MonoBehaviour {

	public delegate void buttonPress(MenuButton button);
	public static event buttonPress OnButtonPress;

	public ControllerKinect kinect;
	public ReticleMovement retical;
	public Texture buttonTexture;
	private GUIElement button;
	private Vector2 buttonSize;
	private Rect screenButton;
	private float buttonCountDown;
	public int countDownTime;

	// Use this for initialization
	void Start () {
		buttonSize = new Vector2(Screen.width/3, Screen.height/8);
		screenButton = new Rect((Screen.width/2) - buttonSize.x/2, buttonSize.y/2 + Screen.height/2, buttonSize.x, buttonSize.y);
	}
	
	// Update is called once per frame
	void Update () 
	{
		buttonSize = new Vector2(Screen.width/3, Screen.height/8);
		screenButton = new Rect((Screen.width/2) - buttonSize.x/2, buttonSize.y/2 + Screen.height/2, buttonSize.x, buttonSize.y);

		Vector2 guiSpace = new Vector2(retical.getScreenPositionCentered().x, Screen.height - retical.getScreenPositionCentered().y);
		if(screenButton.Contains(guiSpace))
		{
			buttonCountDown -= Time.deltaTime;
			if(buttonCountDown <= 0)
			{
				Debug.Log("Button Countdown");
				if(OnButtonPress != null)
				{
					OnButtonPress(this);
				}
				buttonCountDown = countDownTime;
			}
		}
		else
		{
			buttonCountDown = countDownTime;
		}
	}

	void OnGUI()
	{
		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = Screen.height / 5;
		myStyle.normal.textColor = Color.white;
		GUI.depth = 1;
		GUI.DrawTexture(screenButton, buttonTexture, ScaleMode.StretchToFill, false);
		if(buttonCountDown < countDownTime)
		{
			GUI.Label(new Rect(screenButton.x + (screenButton.width/2) - myStyle.fontSize/2, screenButton.y - myStyle.fontSize/4, screenButton.width, screenButton.height), String.Format("{0:0.}", buttonCountDown), myStyle);
		}
	}
}
