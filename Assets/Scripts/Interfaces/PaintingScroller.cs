using UnityEngine;
using System.Collections;

public class PaintingScroller : MonoBehaviour {

	public Material[] paintings;
	public float cycleTime = 0.5f;
	public int currentPainting = 0;
	private float timeFromLastPainting;
	// Use this for initialization
	void Start () {
		timeFromLastPainting = 0;
	}

	void setPainting(Material m)
	{
		renderer.materials[1].mainTexture = m.mainTexture;
		renderer.materials[1].color = m.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeFromLastPainting > cycleTime)
		{
			currentPainting++;
			currentPainting = currentPainting % paintings.Length;

			setPainting(paintings[currentPainting]);
			timeFromLastPainting = 0;
		}
		timeFromLastPainting += Time.deltaTime;
	}
}
