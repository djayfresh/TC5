using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class ControllerKinect : MonoBehaviour {

	public static KinectSensor sensor;
	public static ColorFrameReader cfReader;
	public static DepthFrameReader dfReader;
	public static BodyFrameReader bfReader;
	public static InfraredFrameReader ifReader;

	public delegate void HandInPhiz (Player player, Body body, JointType hand, Vector2 handPosition);
	public static event HandInPhiz playerHandInPhiz;

	public delegate void HandOutOfPhiz(Player Player, Body body, JointType hand);
	public static event HandOutOfPhiz playerHandLeftPhiz;

    public float phizFactor = .9f;

	private float shoulderDist;
	private float aspectRatioX;

	public float marginOfError = 0.009f;
	public float handJumpError = 0.1f;

	private PlayerBody[] players;

	void Start () {
		Debug.Log("Setup Kinect");
		sensor = KinectSensor.GetDefault ();
		if (sensor != null) 
		{
			if(!sensor.IsOpen)
			{
				Debug.Log("Open Kinect");
				sensor.Open();
			}

			
			Debug.Log("Setup Readers for Kinect");
			cfReader = sensor.ColorFrameSource.OpenReader ();
			dfReader = sensor.DepthFrameSource.OpenReader ();
			bfReader = sensor.BodyFrameSource.OpenReader ();
			ifReader = sensor.InfraredFrameSource.OpenReader ();
		}
		players = new PlayerBody[2]; // HARD CODEDED!!! BAD - Player.numPlayers

		shoulderDist = 0;
		aspectRatioX = Screen.width / Screen.height;
	}

	public KinectSensor getSensor()
	{
		return sensor;
	}

	public Body[] getBodies()
	{
		Body[] bodies = new Body[0];
		if(sensor != null)
		{
			var frame = bfReader.AcquireLatestFrame();
			if(frame != null)
			{
				//Debug.Log("Body Frame is not Null");
				bodies = new Body[frame.BodyCount];
				frame.GetAndRefreshBodyData(bodies);
				frame.Dispose();
			}
		}
		return bodies;
	}

	public Rect getHandRelativeToPhiz (Body body, JointType handType, out Vector2 hand)
	{
		CameraSpacePoint neckCS = body.Joints [JointType.Neck].Position;
		CameraSpacePoint shoulderLeftCS = body.Joints [JointType.ShoulderLeft].Position;
		CameraSpacePoint shoulderRightCS = body.Joints [JointType.ShoulderRight].Position;
		CameraSpacePoint handCS = body.Joints [handType].Position;
		JointType handTip = handType == JointType.HandRight? JointType.HandTipRight : JointType.HandTipLeft;
		CameraSpacePoint hand2CS = body.Joints[handTip].Position;
		float deltaShoulderDist = new Vector2 (shoulderRightCS.X - shoulderLeftCS.X, shoulderRightCS.Y - shoulderLeftCS.Y).magnitude * phizFactor;

		if (shoulderDist == 0 || (deltaShoulderDist != shoulderDist))
		{
			shoulderDist = deltaShoulderDist;
			//Debug.Log ("Shoulder Distance: " + shoulderDist);
		}

		float width = shoulderDist * aspectRatioX;
		float height = shoulderDist;
		
        float boxRelativeToShoulder = handType == JointType.HandRight ? shoulderDist/2 : -(shoulderDist/4);

		Rect playerBox = new Rect (neckCS.X - width / 2 + boxRelativeToShoulder, (neckCS.Y - (height) / 2), width, height);
		hand = new Vector2 ((handCS.X + hand2CS.X) / 2, (handCS.Y + hand2CS.Y)/2);
		return playerBox;
	}
	/**
	 * @Return hand inside of phiz as percentage. Null if hand is out of phiz
	 * **/
	public Vector2 trackHandInPhiz(Body body, JointType handType)
	{
		Vector2 hand;
		Rect playerBox = getHandRelativeToPhiz (body, handType, out hand);

		if (playerBox.Contains (hand)) {
			//Debug.Log ("Hand is box");
			return new Vector2((hand.x - playerBox.position.x) / playerBox.width,(hand.y - playerBox.position.y) / playerBox.height);
			//Debug.Log ("X%: " + p.x * 100 + " Y%: " + p.y * 100);
		}
		return Vector2.zero;
	}

	public bool handInPhiz(Body body, JointType handType)
	{
		Vector2 hand;
		Rect playerBox = getHandRelativeToPhiz (body, handType, out hand);		
		return playerBox.Contains (hand);
	}

	void Update()
	{	
		if(GameController.controller != null && !GameController.controller.debugMouse)
		{
			bodyTracking();
		}
	}

	void bodyTracking()
	{
		Body[] bodies = getBodies();
		bool player1Tracked = false;
		bool player2Tracked = false;
		bool aPlayerTracked = false;
		for(int i = 0; i < bodies.Length; i++)
		{
			if(bodies[i].IsTracked)
			{
				aPlayerTracked = true;
				if(players[0] == null || (players[0].body.TrackingId == bodies[i].TrackingId || players[0].bodyIDToTrack == bodies[i].TrackingId))
				{
					if(Player.numPlayers > 1 && players[1] != null && players[1].body.TrackingId == bodies[i].TrackingId)
					{
						playerTrackingUpdate(1, bodies[i]);
						player2Tracked = true;
					}
					else
					{
						playerTrackingUpdate (0, bodies[i]);
						player1Tracked = true;
					}
				}
				if(players.Length == 2 && (players[1] == null || players[1].body.TrackingId == bodies[i].TrackingId))
				{
					if(players[0] != null && players[0].body.TrackingId != bodies[i].TrackingId)
					{
						playerTrackingUpdate(1, bodies[i]);
						player2Tracked = true;
					}
					else if(players[0] == null)
					{
						playerTrackingUpdate(1, bodies[i]);
						player2Tracked = true;
					}
				}
				else if(players[1] != null && players[0] == null)
				{
					//Debug.Log("player 1 left");
					playerTrackingUpdate(1, bodies[i]);

					player2Tracked = true;
				}
			}
		}
		if(aPlayerTracked)
		{
			if(!player1Tracked)
			{
				//Debug.Log("Player 1 lost tracking");
				Player.player1.tracked = false;
				players[0] = null;
			}
			if(!player2Tracked && Player.numPlayers > 1)
			{
				//Debug.Log("Player 2 lost tracking");
				Player.player2.tracked = false;
				players[1] = null;
			}
		}
		else if(bodies.Length > 0)
		{
			//Debug.Log("No bodies tracked");
			Player.player1.tracked = false;
			players[0] = null;
			if(Player.numPlayers > 1)
			{
				Player.player2.tracked = false;
				players[1] = null;
			}
		}
	}

	Vector2 handCorrection(Vector2 thisHandFramePosition, int playerIndex)
	{
		if(thisHandFramePosition != Vector2.zero)
		{
			Vector2 p2 = players[playerIndex].handLocationLastFrame;
			float handMagnitude = (thisHandFramePosition - p2).magnitude/phizFactor;
			if(handMagnitude < marginOfError && p2 != Vector2.zero)
			{
				thisHandFramePosition = p2;
				//Debug.Log("Hand corrected: " + handMagnitude);
			}
			else if(handMagnitude > handJumpError && p2 != Vector2.zero)
			{
				if(handMagnitude < phizFactor - handJumpError)
				{
					players[playerIndex].handLocationLastFrame = thisHandFramePosition;
				}
				thisHandFramePosition = (thisHandFramePosition - p2).normalized * ((handMagnitude - handJumpError)/phizFactor);
				Debug.Log("Hand Jumped: " + handMagnitude);
			}
			else
			{
				//Debug.Log("Hand okay: " + handMagnitude);
				players[playerIndex].handLocationLastFrame = thisHandFramePosition;
			}
		}
		return thisHandFramePosition;
	}
	
	void playerUpdate(int playerIndex)
	{
		Vector2 p = handCorrection(trackHandInPhiz(players[playerIndex].body, JointType.HandRight), playerIndex); //Margin of error on this vector

		if(p != Vector2.zero)
		{
			if(playerHandInPhiz != null)
			{
				players[playerIndex].handToTrack =  JointType.HandRight; //Store Tracked hand
				playerHandInPhiz(players[playerIndex].player, players[playerIndex].body, JointType.HandRight, p);
			}
		}
		else
		{
			if(playerHandLeftPhiz != null && players[playerIndex].handToTrack.Equals(JointType.HandRight)) //Only send if Right hand is one to track
			{
				playerHandLeftPhiz(players[playerIndex].player, players[playerIndex].body, JointType.HandRight);
				players[playerIndex].handLocationLastFrame = Vector2.zero;
			}
			p = handCorrection(trackHandInPhiz(players[playerIndex].body, JointType.HandLeft), playerIndex);
			if(p != Vector2.zero)
			{
				if(playerHandInPhiz != null)
				{
					players[playerIndex].handToTrack = JointType.HandLeft;
					playerHandInPhiz(players[playerIndex].player, players[playerIndex].body, JointType.HandLeft, p);
				}
			}
			else
			{
				if(playerHandLeftPhiz != null && players[playerIndex].handToTrack.Equals(JointType.HandLeft)) //Only send if Left hand is tracked
				{
					playerHandLeftPhiz(players[playerIndex].player, players[playerIndex].body, JointType.HandLeft);
					players[playerIndex].handLocationLastFrame = Vector2.zero;
				}
			}
		}
		if(players[playerIndex].bodyIDToTrack == null && players[playerIndex].handToTrack != null)
		{
			Debug.Log("Player " + (playerIndex + 1) + " tracking ID: " + players[playerIndex].body.TrackingId);
			players[playerIndex].player.tracked = true;
			players[playerIndex].bodyIDToTrack = players[playerIndex].body.TrackingId;
		}
		else{
			Debug.Log("Player " + (playerIndex + 1) + "Found but not tracking");
		}
	}

	void playerTrackingUpdate (int i, Body body)
	{
		if (players [i] == null) {
			players [i] = new PlayerBody (i == 0? Player.player1 : Player.player2);
		}
		players [i].body = body;
		playerUpdate (i);
	}

	void OnDestroy()
	{
		if (cfReader != null) 
		{
			cfReader.Dispose();
			cfReader = null;
		}
		if(dfReader != null)
		{
			dfReader.Dispose();
			dfReader = null;
		}
		if(bfReader != null)
		{
			bfReader.Dispose();
			bfReader = null;
		}
		if(ifReader != null)
		{
			ifReader.Dispose();
			ifReader = null;
		}
		if(sensor != null)
		{
			sensor.Close ();
		}
	}
}

class PlayerBody
{
	public Player player;
	public Body body;
	public JointType? handToTrack = null;
	public ulong? bodyIDToTrack = null;
	public Vector2 handLocationLastFrame;

	public PlayerBody(Player player)
	{
		this.player = player;
	}
}
