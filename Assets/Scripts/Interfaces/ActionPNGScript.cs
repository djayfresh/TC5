using UnityEngine;
using System.Collections;

public class ActionPNGScript : MonoBehaviour {
	
	private float screenHeight;
	private float screenWidth;
	private float heightPosition;
	private float widthPosition;
	private Rect actionPosition;
	public Texture actionTexture;
	private bool desplayAction = false;
	
	void Start () {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		heightPosition = screenHeight - screenHeight / 2;
		widthPosition = (screenWidth / 2) - (actionTexture.width / 2);
		actionPosition = new Rect(widthPosition, heightPosition,actionTexture.width, actionTexture.height);
	}
	
	public void hideActionTexture()
	{
		desplayAction = false;
	}
	
	public void showActionTexture()
	{
		desplayAction = true;
	}
	
	void OnGUI()
	{
		if(desplayAction)
			GUI.Label(actionPosition, actionTexture);
	}
}
