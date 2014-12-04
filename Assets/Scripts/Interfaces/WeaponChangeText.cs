using UnityEngine;
using System.Collections;

public class WeaponChangeText : MonoBehaviour {
	
	public delegate void WeaponChangeActive(bool show);
	public static event WeaponChangeActive onTextShow;

	private float heightPosition;
	private float widthPosition;
	private Rect waitPosition;
	private bool displayWeaponChange = false;
	private ShootingController userFireController;

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

			waitPosition = new Rect(0, 0,Screen.width, Screen.height);
			GUI.Label(waitPosition, weaponText, myStyle);
		}
	}
}
