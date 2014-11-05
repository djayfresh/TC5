using UnityEngine;
using System.Collections;
using Windows.Kinect;
public class InstructionLogic : MonoBehaviour 
{
	private KinectSensor sensor;
	private BodyFrameReader reader;
	private Body[] bodies = null;
	private float shoulderDist;
	private float aspectRatioX;

	// Use this for initialization
	void Start () 
	{
		sensor = KinectSensor.GetDefault();
		
		if (sensor != null)
		{
			reader = sensor.BodyFrameSource.OpenReader();
			
			if (!sensor.IsOpen)
			{
				sensor.Open();
			}
		}
		shoulderDist = 0;
		
		aspectRatioX = Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (reader != null)
		{
			var frame = reader.AcquireLatestFrame();
			if (frame != null)
			{
				if (bodies == null)
				{
					bodies = new Body[sensor.BodyFrameSource.BodyCount];
				}
				
				frame.GetAndRefreshBodyData(bodies);
				
				frame.Dispose();
				frame = null;
			}
		}

		if(bodies != null)
		{
			for (int i = 0; i < this.bodies.Length; i++) 
			{
				//Debug.Log(i + ": " + bodies[i].HandRightState.ToString());

				if(bodies[i].IsTracked)
				{
					CameraSpacePoint neckCS = bodies[i].Joints[JointType.Neck].Position;
					CameraSpacePoint shoulderLeftCS = bodies[i].Joints[JointType.ShoulderLeft].Position;
					CameraSpacePoint shoulderRightCS = bodies[i].Joints[JointType.ShoulderRight].Position;
					//CameraSpacePoint handCS = bodies[i].Joints[JointType.HandRight].Position;

					float deltaShoulderDist = new Vector2(shoulderRightCS.X - shoulderLeftCS.X, shoulderRightCS.Y - shoulderLeftCS.Y).magnitude;

					if(shoulderDist == 0 || deltaShoulderDist > shoulderDist)
					{
						shoulderDist = deltaShoulderDist;
						Debug.Log ("Shoulder Distance: " + shoulderDist);
					}
					float width = shoulderDist * aspectRatioX;
					float height = shoulderDist;

					Rect playerBox = new Rect(neckCS.X  - width/2 + shoulderDist/2, (neckCS.Y - (height)/2), width, height);

					//ColorSpacePoint playerBoxPlane = sensor.CoordinateMapper.MapCameraSpacePointToColorSpace(NeckCS);
					Debug.Log(playerBox.x + " , " + playerBox.y);

				}
			}
		}
	}

}