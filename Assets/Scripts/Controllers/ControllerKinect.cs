using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class ControllerKinect : MonoBehaviour {

	public static KinectSensor sensor;
	public static ColorFrameReader cfReader;
	public static DepthFrameReader dfReader;
	public static BodyFrameReader bfReader;
	public static InfraredFrameReader ifReader;

	public delegate void HandInPhiz (Body body, JointType hand, Vector2 handPosition);
	public static event HandInPhiz playerHandInPhiz;

	public delegate void HandOutOfPhiz(Body body, JointType hand);
	public static event HandOutOfPhiz playerHandLeftPhiz;

    public float phizFactor = .9f;

	private float shoulderDist;
	private float aspectRatioX;

	private JointType? handToTrack = null;
	private ulong bodyIDToTrack;
	private bool firstBodyTracked = false;
	
	void Start () {
		sensor = KinectSensor.GetDefault ();
		if (sensor != null) 
		{
			if(!sensor.IsOpen)
			{
				sensor.Open();
			}


			cfReader = sensor.ColorFrameSource.OpenReader ();
			dfReader = sensor.DepthFrameSource.OpenReader ();
			bfReader = sensor.BodyFrameSource.OpenReader ();
			ifReader = sensor.InfraredFrameSource.OpenReader ();
		}

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
		float deltaShoulderDist = new Vector2 (shoulderRightCS.X - shoulderLeftCS.X, shoulderRightCS.Y - shoulderLeftCS.Y).magnitude * phizFactor;

		if (shoulderDist == 0 || (deltaShoulderDist != shoulderDist)) 
		{
			shoulderDist = deltaShoulderDist;
			//Debug.Log ("Shoulder Distance: " + shoulderDist);
		}

		float width = shoulderDist * aspectRatioX;
		float height = shoulderDist;
		
        float boxRelativeToShoulder = handType == JointType.HandRight ? shoulderDist / 2 : -(shoulderDist / 4);

		Rect playerBox = new Rect (neckCS.X - width / 2 + boxRelativeToShoulder, (neckCS.Y - (height) / 2), width, height);
		hand = new Vector2 (handCS.X, handCS.Y);
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
		Body[] bodies = getBodies();
		bool anyBodiesTrack = false;
		for(int i = 0; i < bodies.Length; i++)
		{
			if(!anyBodiesTrack)
				anyBodiesTrack = bodies[i].IsTracked;

			if(bodies[i].IsTracked && (!firstBodyTracked || bodies[i].TrackingId == bodyIDToTrack))
			{
				Vector2 p = trackHandInPhiz(bodies[i], JointType.HandRight);
				if(p != Vector2.zero)
				{
					if(playerHandInPhiz != null)
					{
						handToTrack = JointType.HandRight; //Store Tracked hand
						playerHandInPhiz(bodies[i], JointType.HandRight, p);
					}
				}
				else
				{
					if(playerHandLeftPhiz != null && handToTrack.Equals(JointType.HandRight)) //Only send if Right hand is one to track
					{
						playerHandLeftPhiz(bodies[i], JointType.HandRight);
					}
					p = trackHandInPhiz(bodies[i], JointType.HandLeft);
					if(p != Vector2.zero)
					{
						if(playerHandInPhiz != null)
						{
							handToTrack = JointType.HandLeft;
							playerHandInPhiz(bodies[i], JointType.HandLeft, p);
						}
					}
					else
					{
						if(playerHandLeftPhiz != null && handToTrack.Equals(JointType.HandLeft)) //Only send if Left hand is tracked
						{
							playerHandLeftPhiz(bodies[i], JointType.HandLeft);
						}
					}
				}
				if(!firstBodyTracked && handToTrack != null)
				{
					firstBodyTracked = true;
					bodyIDToTrack = bodies[i].TrackingId;
				}
			}
		}
		if(!anyBodiesTrack && bodies.Length > 0)
		{
			firstBodyTracked = false;
			handToTrack = null;
		}
	}

	void OnDestory()
	{
		if(sensor != null)
		{
			sensor.Close ();
		}
		if (cfReader != null) 
		{
			cfReader.Dispose ();
		}
		if(dfReader != null)
		{
			dfReader.Dispose();
		}
		if(bfReader != null)
		{
			bfReader.Dispose();
		}
		if(ifReader != null)
		{
			ifReader.Dispose();
		}
	}
}
