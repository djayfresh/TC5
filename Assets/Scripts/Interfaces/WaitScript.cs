using UnityEngine;
using System.Collections;

public class WaitScript : MonoBehaviour {
	
	public delegate void WaitTextActive(bool show);
	public static event WaitTextActive onTextShow;

    private float screenHeight;
    private float screenWidth;
	private float heightPosition;
	private float widthPosition;
    private Rect waitPosition;
    public Texture waitTexture;
    private bool desplayWait = false;
	private ShootingController userFireController;

	void Start () {
		screenWidth = Screen.width;
		screenHeight = Screen.height;

        heightPosition = screenHeight - screenHeight / 2;
        widthPosition = (screenWidth / 2) - (waitTexture.width/2);
        waitPosition = new Rect(widthPosition, heightPosition,waitTexture.width, waitTexture.height);
	}

    public void hideWaitTexture()
    {
        desplayWait = false;
		if(onTextShow != null)
		{
			onTextShow(desplayWait);
		}
    }

    public void showWaitTexture()
    {
		desplayWait = true;
		if(onTextShow != null)
		{
			onTextShow(desplayWait);
		}
    }

    void OnGUI()
    {
        if(desplayWait)
            GUI.Label(waitPosition, waitTexture);
    }
}
