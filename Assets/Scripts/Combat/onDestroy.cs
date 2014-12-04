using UnityEngine;
using System.Collections;

public class onDestroy : MonoBehaviour {

	public GameObject[] toDestroy;

	void OnDestroy()
	{
		foreach(GameObject g in toDestroy)
		{
			if(g != null)
			{
				Destroy(g);
			}
		}
	}
}
