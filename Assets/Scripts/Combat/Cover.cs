using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class Cover : MonoBehaviour {
	
	public delegate void PlayerCover(Cover cover);
	public static event PlayerCover OnCover;

	public delegate void PlayerLeaveCover(Cover cover);
	public static event PlayerLeaveCover OnExitCover;

	public float lerpSpeed = 4;
	// Use this for initialization
	void Start () {
		ControllerKinect.playerHandLeftPhiz += playerEnteredCover;
		ControllerKinect.playerHandInPhiz += playerLeaveCover;
	}

	void playerEnteredCover(Body body, JointType hand)
	{
		if(OnCover != null)
		{
			OnCover(this);
		}
	}

	void playerLeaveCover(Body body, JointType hand, Vector2 handPos)
	{
		if(OnExitCover != null)
		{
			OnExitCover(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightAlt))
		{
			if(OnCover != null)
			{
				OnCover(this);
			}

		}
		else if(Input.GetKeyUp(KeyCode.RightAlt))
		{
			if(OnExitCover != null)
			{
				OnExitCover(this);
			}
		}
	}
}
