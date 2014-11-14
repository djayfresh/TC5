using UnityEngine;
using System.Collections;

public class ReticleMovement : MonoBehaviour
{
	public ControllerKinect kinect;
	public bool debugMouse = false;
	public float marginOfError = .05f;
	private Vector2 localPositon;
	private Vector2 positonCenter;
	public Texture crosshair;
	public float scaleFactor = 1.0f;
	private bool shouldShowCrosshair = true;
	// Use this for initialization
	void Start()
	{
		Screen.showCursor = false;
		WaitScript.onTextShow += showCrosshair;
		updatePositionCenter();
	}

	public void hideCrosshair()
	{
		shouldShowCrosshair = false;
	}

	public void showCrosshair()
	{
		shouldShowCrosshair = true;
	}

	void showCrosshair(bool enable)
	{
		shouldShowCrosshair = !enable;
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

		if(Input.GetKeyDown(KeyCode.BackQuote))
			debugMouse = !debugMouse;

		Vector2 position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		if (kinect.getSensor() == null) {
			setPosition(position);
		}
		else{ 
			if(debugMouse)
			{
				setPosition(position);
			}
		}
	}

	void OnGUI()
	{
		if(shouldShowCrosshair)
		{
			GUI.depth = 0;
			GUI.DrawTexture(new Rect(localPositon.x - positonCenter.x/2, Screen.height - (localPositon.y+(positonCenter.y/2)), positonCenter.x, positonCenter.y), crosshair, ScaleMode.StretchToFill, true);
		}
	}

	void OnDestroy()
	{
		WaitScript.onTextShow -= showCrosshair;
	}
}