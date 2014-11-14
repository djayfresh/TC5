using UnityEngine;
using System.Collections;
using System;

public class HoldTimeButton : MenuButton {

	public ActionTrigger trigger;
	public bool active;

	void Start()
	{
		ActionTrigger.OnTrigger += onTrigger;
	}

	void Update()
	{
		updateButtonState();
	}

	override protected void onEntry()
	{
		buttonCountDown -= Time.deltaTime;
		buttonHeld = true;
		if(buttonCountDown <= 0)
		{
			fireEvent();
			buttonCountDown = countDownTime;
			Destroy(this.gameObject);
		}
	}

	override protected void onExit()
	{

	}

	void onTrigger(ActionTrigger t)
	{
		if(t.Equals(trigger))
		{
			active = true;
		}
	}

	void OnGUI()
	{
		GUI.depth = 1;
		if(active)
		{
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

	void OnDestroy()
	{
		ActionTrigger.OnTrigger -= onTrigger;
	}
}
