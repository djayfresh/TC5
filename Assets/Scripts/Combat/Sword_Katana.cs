using UnityEngine;
using System.Collections;

public class Sword_Katana : Weapon {

	// Use this for initialization
	void Start () {
		bullet = new Bullet{damage = 5};
		fireRate = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
