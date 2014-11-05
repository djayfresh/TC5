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
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        user = FindObjectOfType<Player>();
        heightPosition = (screenHeight / 2) - (reloadTexture.height/2);
        widthPosition = (screenWidth / 2) - (reloadTexture.width / 2);
        reloadTextPosition = new Rect(widthPosition, heightPosition, reloadTexture.width, reloadTexture.height);
    }

    void Update()
    {
        if (user.getWeapon().clipRemaining == 0)
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
