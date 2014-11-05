using UnityEngine;
using System.Collections;

public class SceneInfo : MonoBehaviour 
{
	public float Accuracy{ get; set; }
	public float Score{ get; set; }
	public float LivesLeft{ get; set; }
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad (this);
        Accuracy = -1;
        LivesLeft = -1;
        Score = -1;
	}
	public void recordInfo(float accuracy, float score, float livesleft)
	{
		Accuracy = accuracy;
		Score = score;
		LivesLeft = livesleft;
	}


}
