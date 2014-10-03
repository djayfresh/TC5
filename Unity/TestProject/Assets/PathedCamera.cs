using UnityEngine;
using System.Collections;

public class PathedCamera : MonoBehaviour {

	public CameraNode[] destinations;
	int currentDestination;
	float DISTANCE_BUFFER = 0.5f;
	public float lerpSpeed = 2;
	Vector3 initalPos;
	// Use this for initialization
	void Start () {
		currentDestination = 0;
		initalPos = transform.position = destinations[currentDestination].transform.position;
		transform.rotation = destinations [currentDestination].camera.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentDestination < destinations.Length)
		{
			Vector3 pos = destinations [currentDestination].transform.position;
			float dist = Vector3.Distance (pos, this.transform.position);
			if(dist > DISTANCE_BUFFER)
			{
				transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * lerpSpeed);
				transform.rotation = Quaternion.Lerp(transform.rotation, destinations[currentDestination].camera.transform.rotation, Time.deltaTime);
				//transform.position = Vector3.Lerp(initalPos, pos, lerpTime);
			}
			else
			{
				if(destinations[currentDestination].move())
				{
					currentDestination++;
					initalPos = transform.position;
				}
			}
		}
	}
}
