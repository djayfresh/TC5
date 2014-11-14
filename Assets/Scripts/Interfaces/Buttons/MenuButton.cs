using UnityEngine;
using System.Collections;
using System;

public class MenuButton : MonoBehaviour {

	public delegate void buttonPress(MenuButton button);
	public static event buttonPress OnButtonPress;

	public ReticleMovement retical;
	public Texture buttonTexture;
	public int countDownTime;
	public string label = "";
	public Rect buttonSize;
	protected Vector2 buttonDimensions;
	protected Rect screenButton;
	protected float buttonCountDown;
	protected bool buttonHeld = false;
	void Start () {
		scaleButton();
	}

	void scaleButton()
	{
		if(buttonSize.x == 0 || buttonSize.y == 0 || buttonSize.width == 0 || buttonSize.height == 0)
		{
			buttonDimensions = new Vector2(Screen.width/3, Screen.height/8);
			screenButton = new Rect((Screen.width/2) - buttonDimensions.x/2, buttonDimensions.y/2 + Screen.height/2, buttonDimensions.x, buttonDimensions.y);
		}
		else
		{
			screenButton = new Rect(Screen.width * buttonSize.x, Screen.height * buttonSize.y, Screen.width * buttonSize.width, Screen.height * buttonSize.height);
		}
	}
	// Update is called once per frame
	void Update () 
	{
		updateButtonState();
	}

	protected virtual void updateButtonState()
	{
		scaleButton();
		Vector2 guiSpace = new Vector2(retical.getScreenPositionCentered().x, Screen.height - retical.getScreenPositionCentered().y);
		if(screenButton.Contains(guiSpace))
		{
			onEntry();
		}
		else
		{
			onExit();
		}
	}

	protected virtual void onEntry()
	{
		buttonCountDown -= Time.deltaTime;
		buttonHeld = true;
		if(buttonCountDown <= 0)
		{
			Debug.Log("Button Countdown");
			fireEvent();
			buttonCountDown = countDownTime;
		}
	}

	protected virtual void fireEvent()
	{
		if(OnButtonPress != null)
		{
			OnButtonPress(this);
		}
	}

	protected virtual void onExit()
	{
		buttonHeld = false;
		buttonCountDown = countDownTime;
	}

	void OnGUI()
	{
		GUI.depth = 1;

		GUIStyle myStyle = new GUIStyle();
		myStyle.alignment = TextAnchor.MiddleCenter;
		myStyle.fontSize = (int)screenButton.height;
		myStyle.normal.textColor = Color.white;

		GUI.DrawTexture(screenButton, buttonTexture, ScaleMode.StretchToFill, false);

		GUIStyle menuButtonLabelStyle = GUI.skin.GetStyle("Label");
		menuButtonLabelStyle.alignment = TextAnchor.MiddleCenter;
		menuButtonLabelStyle.fontSize = (int)screenButton.height;
		menuButtonLabelStyle.normal.textColor = Color.black;
		
		GUI.Label(screenButton, label, menuButtonLabelStyle);
	
		if(buttonHeld)
		{
			GUI.Label(screenButton, String.Format("{0:0.}", buttonCountDown), myStyle);
		}
	}
}
