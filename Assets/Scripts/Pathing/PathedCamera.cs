using UnityEngine;
using System.Collections;

public class PathedCamera : MonoBehaviour {

	public CameraNode[] destinations;
	int currentDestination;
	float DISTANCE_BUFFER = 0.5f;
	private Cover activeCover;
	private float coverLerpSpeed;
	private bool canCover;
	private float lerpSpeed;
	public Camera camera;
	// Use this for initialization
	void Start () {
		currentDestination = 0;
		if(destinations.Length > 0)
		{
			transform.rotation = destinations [currentDestination].camera.transform.rotation;
			transform.position = destinations [currentDestination].transform.position;
			lerpSpeed = destinations[currentDestination].lerpSpeed;
		}
		activeCover = null;
		canCover = false;
		coverLerpSpeed = 1;
		camera = GetComponent<Camera>();

		Cover.OnCover += PlayerCover;
		Cover.OnCover += keyPressCover;
		Cover.OnExitCover += PlayerLeftCover;
	}

	void keyPressCover(Player player, Cover cover)
	{
		PlayerCover(player, cover);
	}

	void PlayerCover(Player activePlayer, Cover cover)
	{
		if(activeCover != cover && currentDestination < destinations.Length)
		{
			for(int i = 0; i < destinations[currentDestination].cover.Length; i++)
			{
				Cover hasCover = destinations[currentDestination].cover[i];
				if(hasCover == cover)
				{
					activeCover = cover;
					setPlayerCover(true);
				}
			}
		}
	}

	void PlayerLeftCover(Player activePlayer, Cover cover)
	{
		setPlayerCover(false);
		if(activeCover == cover)
		{
			activeCover = null;
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
				transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * lerpSpeed * coverLerpSpeed);
				//transform.position = Vector3.Lerp(initalPos, pos, lerpTime);
			}
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, activeCover.camera.fieldOfView, Time.deltaTime * lerpSpeed * coverLerpSpeed);
		}
		else if(currentDestination < destinations.Length)
		{
			lerpSpeed = destinations[currentDestination].lerpSpeed;
			Vector3 pos = destinations [currentDestination].transform.position;
			float dist = Vector3.Distance (pos, this.transform.position);
			float rot = Quaternion.Angle(transform.rotation, destinations[currentDestination].camera.transform.rotation);
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, destinations[currentDestination].camera.fieldOfView, Time.deltaTime * lerpSpeed * coverLerpSpeed);

			if(rot != 0) //Possibliy broken
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, 
				                                     destinations[currentDestination].camera.transform.rotation, 
				                                     Time.deltaTime * destinations[currentDestination].rotationLerpSpeed * coverLerpSpeed);
			}
			if(dist > DISTANCE_BUFFER)
			{
				
				setPlayerCover(true);
				transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * lerpSpeed * coverLerpSpeed);
			}
			else
			{
				canCover = true;
				
				setPlayerCover(false);
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

	void setPlayerCover(bool inCover)
	{
		Player.player1.inCover = inCover;
		if(Player.player2 != null)
		{
			Player.player2.inCover = inCover;
		}
	}

	void OnDestroy()
	{
		Cover.OnCover -= PlayerCover;
		Cover.OnCover -= keyPressCover;
		Cover.OnExitCover -= PlayerLeftCover;
	}
}
