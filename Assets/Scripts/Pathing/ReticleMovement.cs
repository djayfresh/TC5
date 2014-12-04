using UnityEngine;
using System.Collections;

public class ReticleMovement : MonoBehaviour
{
	public float marginOfError = .05f;
	private Vector2 localPositon;
	private Vector2 positonCenter;
	public Texture crosshair;
	public Texture crosshair2;
	public float scaleFactor = 1.0f;
	private bool shouldShowCrosshair = false;
	private bool waitTextShown = false;
	// Use this for initialization
	void Start()
	{
		Screen.showCursor = false;
		WaitScript.onTextShow += negateShowCrosshair;
		if(!GetComponent<Player>().Equals(Player.player1))
		{
			crosshair = crosshair2;
		}
		updatePositionCenter();
	}

	void negateShowCrosshair(bool WaitTextVisibility)
	{
		waitTextShown = WaitTextVisibility;
	}

	public void hideCrosshair()
	{
		shouldShowCrosshair = false;
	}

	public void showCrosshair()
	{
		shouldShowCrosshair = true;
	}

	public void showCrosshair(bool enable)
	{
		shouldShowCrosshair = enable;
	}

	public void setPosition(Vector2 pos)
	{
		if((pos - localPositon).magnitude > marginOfError)
        	localPositon = pos;
	}

	public Vector2 getScreenPositionCentered()
	{
		return localPositon;
	}
	void updatePositionCenter()
	{
		positonCenter = new Vector2(scaleFactor * (Screen.width - crosshair.width), scaleFactor * (Screen.height - crosshair.height));
	}
	// Update is called once per frame
	void Update()
	{
		updatePositionCenter();

		Vector2 position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		if(GameController.controller.debugMouse)
		{
			setPosition(position);
		}
	}

	void OnGUI()
	{
		if(shouldShowCrosshair && !waitTextShown)
		{
			GUI.depth = 0;
			GUI.DrawTexture(new Rect(localPositon.x - positonCenter.x/2, Screen.height - (localPositon.y+(positonCenter.y/2)), positonCenter.x, positonCenter.y), crosshair, ScaleMode.StretchToFill);
		}
	}

	void OnDestroy()
	{
		WaitScript.onTextShow -= negateShowCrosshair;
	}
}