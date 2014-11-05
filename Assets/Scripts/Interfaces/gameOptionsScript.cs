using UnityEngine;
using System.Collections;

public class gameOptionsScript : MonoBehaviour {

    public Texture startTexture;

    void OnGUI()
    {
        GUI.depth = 2; //smaller the closer to camera
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), startTexture, ScaleMode.StretchToFill, true);
	}
}
