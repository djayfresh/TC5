using UnityEngine;
using System.Collections;

public class CreditsInfo : MonoBehaviour 
{
	private SceneInfo si;
	// Use this for initialization
	void Start () 
	{
		si = FindObjectOfType<SceneInfo> ();
		Debug.Log ("Accuracy: " + si.Accuracy);
		Debug.Log ("Score: " + si.Score);
		Debug.Log ("Lives Left: " + si.LivesLeft);
	}
}
