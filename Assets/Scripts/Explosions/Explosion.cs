using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public GameObject shrapnal;
	public ParticleSystem effect;
	public float force = 901;
	public int numParticles;

	// Use this for initialization
	void Start () {
		effect.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(effect.isStopped)
		{
			Destroy(this);
		}
	}
}
