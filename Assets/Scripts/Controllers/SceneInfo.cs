using UnityEngine;
using System.Collections;

public class SceneInfo : MonoBehaviour 
{
	public static SceneInfo info;
	public float Accuracy{ get; set; }
	public float Score{ get; set; }
	public float LivesLeft{ get; set; }
	// Use this for initialization
	void Awake()
	{
		if(info == null)
		{
			info = this;
			DontDestroyOnLoad (this);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void Start () 
	{
        Accuracy = -1;
        LivesLeft = -1;
        Score = -1;
	}
	public static void recordInfo(float accuracy, float score, float livesleft)
	{
		info.Accuracy = accuracy;
		info.Score = score;
		info.LivesLeft = livesleft;
	}


}
