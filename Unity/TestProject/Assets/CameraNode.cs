using UnityEngine;
using System.Collections;

public class CameraNode : MonoBehaviour {

	public Camera camera;
	public float waitTime;
	public bool shouldmove;
	public float cameraTimeHere;
	// Use this for initialization
	void Start () {
		cameraTimeHere = 0;
		camera = GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool move()
	{
		cameraTimeHere += Time.deltaTime;
		if (cameraTimeHere >= waitTime && shouldmove) 
		{
			cameraTimeHere = 0;
						return true;
				} else {
						return false;
				}
	}
}
