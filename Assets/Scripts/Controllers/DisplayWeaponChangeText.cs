using UnityEngine;
using System.Collections;

public class DisplayWeaponChangeText : MonoBehaviour {
	public WeaponChangeText textToShow;

	void OnDestroy()
	{
		if(textToShow != null)
		{
			textToShow.showWaitTexture ();
		}
	}

}
