using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public static SoundManager soundManager;
	public AudioSource[] sounds;
	// Use this for initialization
	void Start () 
	{
		if(soundManager == null)
		{
			soundManager = this;
			playMusic ();
		}
		else if(soundManager != this)
		{
			Destroy(gameObject);
		}
	}
	public static void playPistolSound()
	{
		play (3);
	}
	public static void playRifleSound()
	{
		play (2);
	}
	public static void playShotGunSound()
	{
		play (1);
	}
	public static void playMusic()
	{
		play(0);
	}
	public static void playEnemyDeath()
	{
		play(4);
	}
	public static void playEnemyFire()
	{
		play(5);
	}
	public static void playDryFire()
	{
		play(6);
	}
	public static void startHelicopterNoise()
	{
		play(7);
	}
	public static void playExplosion()
	{
		play(8);
	}
	public static void muteMusic(bool mute)
	{
		soundManager.sounds[0].mute = mute;
	}

	public static void play(int index)
	{
		if(soundManager != null && soundManager.sounds.Length > index)
			soundManager.sounds[index].Play();
	}

	void OnDestroy()
	{
		soundManager = null;
	}
}
