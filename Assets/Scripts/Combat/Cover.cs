using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class Cover : MonoBehaviour {
	
	public delegate void PlayerCover(Player player, Cover cover);
	public static event PlayerCover OnCover;

	public delegate void PlayerLeaveCover(Player player, Cover cover);
	public static event PlayerLeaveCover OnExitCover;

	private Player trackedPlayer;
	private bool trackingTwoPlayer;

	public float lerpSpeed = 4;
	// Use this for initialization
	void Start () {
		ControllerKinect.playerHandLeftPhiz += playerEnteredCover;
		ControllerKinect.playerHandInPhiz += playerLeaveCover;
	}

	void playerEnteredCover(Player activePlayer, Body body, JointType hand)
	{
		if(trackedPlayer == null && !trackingTwoPlayer)
		{
			trackedPlayer = activePlayer;
		}
		if(trackedPlayer != null && !trackedPlayer.Equals(activePlayer))
		{
			trackingTwoPlayer = activePlayer.tracked;
		}

		if(trackingTwoPlayer || MissingTrackedPlayer())
		{
			if(OnCover != null)
			{
				OnCover(activePlayer, this);
			}
		}
	}

	bool MissingTrackedPlayer()
	{
		return Player.numPlayers == 1 || (Player.player1.tracked && !Player.player2.tracked) || (!Player.player1.tracked && Player.player2.tracked);
	}

	void playerLeaveCover(Player activePlayer, Body body, JointType hand, Vector2 handPos)
	{
		if(trackingTwoPlayer || MissingTrackedPlayer())
		{
			trackedPlayer = null;
			trackingTwoPlayer = false;
			if(OnExitCover != null)
			{
				OnExitCover(activePlayer, this);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cover/Reload"))
		{
			if(OnCover != null)
			{
				OnCover(null, this);
			}

		}
		else if(Input.GetButtonUp("Cover/Reload"))
		{
			if(OnExitCover != null)
			{
				OnExitCover(null, this);
			}
		}
	}

	void Destroy()
	{
		ControllerKinect.playerHandLeftPhiz -= playerEnteredCover;
		ControllerKinect.playerHandInPhiz -= playerLeaveCover;
	}
}
