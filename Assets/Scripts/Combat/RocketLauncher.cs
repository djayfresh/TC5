using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {

	public Rocket rocket;
	public Bullet regularBullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{

	}

	public class Rocket : Bullet
	{
		void Start()
		{
			damage = 9001;
		}
	}
}
