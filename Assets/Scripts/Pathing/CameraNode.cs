using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class CameraNode : MonoBehaviour {

	public Camera camera;
	public Cover[] cover;
	public float waitTime;
	public bool shouldmove;
	public float cameraTimeHere;
	public GameObject[] eneimes;
	public GameObject[] despawnList;
	public float lerpSpeed;

	public ActionTrigger triggerVolumn;
	// Use this for initialization
	void Start () {
		cameraTimeHere = 0;
		camera = GetComponentInChildren<Camera> ();
		if(triggerVolumn != null)
		{
			ActionTrigger.OnTrigger += volumnCollide;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Despawn()
	{
		for (int i = 0; i < despawnList.Length; i++) 
		{
			Destroy (despawnList[i]);
		}
	}

	void volumnCollide(ActionTrigger e)
	{
		if(e == triggerVolumn)
			shouldmove = true;
	}

	public bool allEneimesDead()
	{
		bool dead = true;

		for (int i = 0; i < eneimes.Length; i++) {
			if(dead)
				dead = eneimes[i] == null;
				}
		
		return dead;
	}

	public void notifyEnemy()
	{
		for(int i = 0; i < eneimes.Length; i++)
		{
			if(eneimes[i] != null)
			{
				MoveToFrom tf = eneimes[i].GetComponent<MoveToFrom>();
				if(tf != null)
				{
					tf.shouldMove = true;
				}
			}
		}
	}

	public bool move()
	{
		notifyEnemy ();
		cameraTimeHere += Time.deltaTime;
		if (cameraTimeHere >= waitTime && shouldmove && allEneimesDead()) 
		{
			cameraTimeHere = 0;
						return true;
				} else {
						return false;
				}
	}

	void OnDestory()
	{
		ActionTrigger.OnTrigger -= volumnCollide;
	}
}
