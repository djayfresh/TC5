using UnityEngine;
using System.Collections;

public class PathedCamera : Player {

	public CameraNode[] destinations;
	int currentDestination;
	float DISTANCE_BUFFER = 0.5f;
	private Cover activeCover;
	private float coverLerpSpeed;
	private bool canCover;
	private float lerpSpeed;
	// Use this for initialization
	void Start () {
		currentDestination = 0;
		transform.rotation = destinations [currentDestination].camera.transform.rotation;
		transform.position = destinations [currentDestination].transform.position;
		lerpSpeed = destinations[currentDestination].lerpSpeed;
		activeCover = null;
		canCover = false;
		inCover = false;
		coverLerpSpeed = 1;

		Cover.OnCover += PlayerCover;
		Cover.OnExitCover += PlayerLeftCover;

	}

	void PlayerCover(Cover cover)
	{
		if(weapon[currentWeapon] != null)
		{
			weapon[currentWeapon].reload();
		}
		if(activeCover != cover && currentDestination < destinations.Length)
		{
			for(int i = 0; i < destinations[currentDestination].cover.Length; i++)
			{
				Cover hasCover = destinations[currentDestination].cover[i];
				if(hasCover == cover)
				{
					activeCover = cover;
					inCover = true;
				}
			}
		}
	}

	void PlayerLeftCover(Cover cover)
	{
		if(activeCover == cover)
		{
			activeCover = null;
			inCover = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(activeCover != null && canCover)
		{
			coverLerpSpeed = activeCover.lerpSpeed;
			
			Vector3 pos = activeCover.transform.position;
			float dist = Vector3.Distance (pos, this.transform.position);
			float rot = Quaternion.Angle(transform.rotation, activeCover.camera.transform.rotation);
			if(rot != 0) //Possibliy broken
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, 
				                                     activeCover.camera.transform.rotation, Time.deltaTime * 
				                                     lerpSpeed * coverLerpSpeed);
			}
			if(dist > DISTANCE_BUFFER)
			{
				inCover = true;
				transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * lerpSpeed * coverLerpSpeed);
				//transform.position = Vector3.Lerp(initalPos, pos, lerpTime);
			}
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, activeCover.camera.fieldOfView, Time.deltaTime * lerpSpeed * coverLerpSpeed);
		}
		else if(currentDestination < destinations.Length)
		{
			lerpSpeed = destinations[currentDestination].lerpSpeed;
			Vector3 pos = destinations [currentDestination].transform.position;
			float dist = Vector3.Distance (pos, this.transform.position);
			float rot = Quaternion.Angle(transform.rotation, destinations[currentDestination].camera.transform.rotation);
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, destinations[currentDestination].camera.fieldOfView, Time.deltaTime * lerpSpeed * coverLerpSpeed);

			if(rot != 0) //Possibliy broken
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, 
				                                     destinations[currentDestination].camera.transform.rotation, 
				                                     Time.deltaTime * lerpSpeed * coverLerpSpeed);
			}
			if(dist > DISTANCE_BUFFER)
			{
				inCover = true;
				transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * lerpSpeed * coverLerpSpeed);
			}
			else
			{
				canCover = true;
				
				inCover = false;
				if(destinations[currentDestination].move())
				{
					canCover = false;
					coverLerpSpeed = 1;
					destinations[currentDestination].Despawn();
					currentDestination++;
				}
			}
		}
	}

	void OnDestroy()
	{
		Cover.OnCover -= PlayerCover;
		Cover.OnExitCover -= PlayerLeftCover;
	}
}
