using UnityEngine;
using System.Collections;

public class HideWaitTextOnDestroy : MonoBehaviour {

	void OnDestroy()
	{
		WaitScript ws = GameObject.FindObjectOfType<WaitScript> ();
		if(ws != null)
		{
			ws.hideWaitTexture ();
		}
	}
}
