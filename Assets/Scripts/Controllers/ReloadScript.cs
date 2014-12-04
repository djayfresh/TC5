using UnityEngine;
using System.Collections;

public class ReloadScript : MonoBehaviour {

    private float screenHeight;
    private float screenWidth;
    private float heightPosition;
    private float widthPosition;
    private Rect reloadTextPosition;
    public Texture reloadTexture;
    private bool desplayReloadText = false;
    private Player user;

    void Start()
    {
        user = GetComponent<Player>();

		UpdateGUIPosition();
        reloadTextPosition = new Rect(widthPosition, heightPosition, reloadTexture.width, reloadTexture.height);
    }

	void UpdateGUIPosition()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		heightPosition = screenHeight - (reloadTexture.height + (reloadTexture.height/2));
		widthPosition = (screenWidth / 2) - (reloadTexture.width / 2);
		if(user == Player.player1)
		{
			widthPosition -= reloadTexture.width;
		}
		else if(user == Player.player2)
		{
			widthPosition += reloadTexture.width;
		}
	}

    void Update()
	{
		UpdateGUIPosition();
        if (user.getWeapon().clipRemaining == 0 && user.tracked)
        {
            showReloadTexture();
        }
        else
        {
            hideReloadTexture();
        }
    }

    private void hideReloadTexture()
    {
        desplayReloadText = false;
    }

    private void showReloadTexture()
    {
        desplayReloadText = true;
    }

    void OnGUI()
    {
        if (desplayReloadText)
            GUI.Label(reloadTextPosition, reloadTexture);
    }
}
