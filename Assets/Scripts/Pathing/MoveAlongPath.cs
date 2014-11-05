using UnityEngine;
using System.Collections;

public class MoveAlongPath : MonoBehaviour {

	public GameObject[] destinations;
	int currentDestination;
	float DISTANCE_BUFFER = 0.5f;
	public float lerpSpeed = 2;
	// Use this for initialization
	void Start () {
		currentDestination = 0;
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
			}
			else
			{
				currentDestination++;
			}
		}
	}
}
