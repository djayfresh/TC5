using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource[] sounds;
	// Use this for initialization
	void Start () 
	{
		playMusic ();
	}

	public void playPistolSound()
	{
		if(sounds.Length >= 3)
			sounds [3].Play ();
	}
	public void playRifleSound()
	{
		if(sounds.Length >= 2)
			sounds [2].Play ();
	}
	public void playShotGunSound()
	{
		if(sounds.Length >= 1)
			sounds [1].Play ();
	}
	public void playMusic()
	{
		if(sounds != null)
			sounds [0].Play ();	
	}
	public void playEnemyDeath()
	{
		if(sounds.Length >= 4)
			sounds[4].Play();
	}
	public void playEnemyFire()
	{
		if(sounds.Length >= 5)
			sounds [5].Play ();
	}
	public void muteMusic(bool mute)
	{
		sounds [0].mute = mute;
	}

}
