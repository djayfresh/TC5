using UnityEngine;
using System.Collections;

public class gameOverOptionsScript : MonoBehaviour {

    public Texture winTexture;
    public Texture loseTexture;
    private SceneInfo gameUser;

    void Start()
    {
        gameUser = FindObjectOfType<SceneInfo>();
        if (gameUser == null)
		{
			gameUser = new SceneInfo() {Accuracy = 0, LivesLeft = 9009, Score = -11};
        }
    }

    void OnGUI()
    {
        GUI.depth = 2; //smaller the closer to camera
        if (GameController.controller.victory)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), winTexture, ScaleMode.StretchToFill, true);
        }
        else
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loseTexture, ScaleMode.StretchToFill, true);
        }
    }
}
