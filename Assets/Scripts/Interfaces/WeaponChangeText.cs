using UnityEngine;
using System.Collections;

public class WeaponChangeText : MonoBehaviour {

	private float heightPosition;
	private float widthPosition;
	private Rect waitPosition;
	private bool displayWeaponChange = false;
	private ShootingController userFireController;
	public delegate void WeaponChangeActive(bool show);
	public static event WeaponChangeActive onTextShow;

	public float weaponChangeShowTime;
	private float weaponChangedTime;
	public string weaponText;
	public Font font;
	
	void Start () {

	}

	void Update()
	{
		if(displayWeaponChange)
		{
			weaponChangedTime += Time.deltaTime;
			displayWeaponChange = weaponChangedTime < weaponChangeShowTime;
		}
		else
		{
			weaponChangedTime = 0;
		}
	}
	
	public void hideWaitTexture()
	{
		displayWeaponChange = false;
		if(onTextShow != null)
		{
			onTextShow(displayWeaponChange);
		}
	}
	
	public void showWaitTexture()
	{
		displayWeaponChange = true;
		if(onTextShow != null)
		{
			onTextShow(displayWeaponChange);
		}
	}
	
	void OnGUI()
	{
		if(displayWeaponChange)
		{
			GUI.depth = 1;
			GUIStyle myStyle = new GUIStyle();
			myStyle.alignment = TextAnchor.MiddleCenter;
			if(font != null)
			{
				myStyle.font = font;
			}
			myStyle.fontSize = Screen.height / 10;
			myStyle.normal.textColor = Color.red;
			
			heightPosition = Screen.height - (Screen.height / 2);
			widthPosition = (Screen.width / 2) - ((weaponText.Length/2) * (myStyle.fontSize/2));
			waitPosition = new Rect(widthPosition, heightPosition,widthPosition, Screen.height/6);
			GUI.Label(waitPosition, weaponText, myStyle);
		}
	}
}
