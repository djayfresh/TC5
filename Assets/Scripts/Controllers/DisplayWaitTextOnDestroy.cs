using UnityEngine;
using System.Collections;

public class DisplayWaitTextOnDestroy : MonoBehaviour {

	void OnDestroy()
	{
		WaitScript ws = GameObject.FindObjectOfType<WaitScript> ();
		if(ws != null)
		{
			ws.showWaitTexture ();
		}
	}
}
