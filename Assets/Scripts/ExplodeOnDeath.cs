using UnityEngine;
using System.Collections;

public class ExplodeOnDeath : MonoBehaviour 
{
	public ParticleEmitter explodeEffect;

	private bool isQuitting = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		if (!isQuitting)
		{
			ParticleEmitter pe;
			if (explodeEffect != null) 
			{
				pe = (GameObject.Instantiate (explodeEffect.gameObject, transform.position, Quaternion.identity) as GameObject).GetComponent<ParticleEmitter> ();
				if (pe != null) 
				{
					//pe.worldVelocity = backVector * distBack;
					SoundManager.playExplosion();
					pe.Emit ();
				}
			}
		}
	}

	
	void OnApplicationQuit()
	{
		isQuitting = true;
	}
}
