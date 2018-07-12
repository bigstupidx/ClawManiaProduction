using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioSource music;
	public AudioSource buttonSnd;
	public List<AudioSource> sfx = new List<AudioSource>();

	private bool isMusicOn = true;
	private bool isSoundOn = true;

	private MusicListenerController musicListener;

	public bool MusicOn
	{
		set
		{
			isMusicOn = value;
			ImplementMusic();
		}
		get
		{
			return isMusicOn;
		}
	}

	public bool SoundOn
	{
		set
		{
			isSoundOn = value;
			ImplementSound();
		}
		get
		{
			return isSoundOn;
		}
	}
	


	void Awake()
	{
		
		MusicOn = (PlayerPrefs.GetInt(PlayerPrefHandler.keyMusic, 1) == 1);
		SoundOn = (PlayerPrefs.GetInt(PlayerPrefHandler.keySound, 1) == 1);
		if (instance == null)
		{
			instance = this;

			musicListener = (MusicListenerController) FindObjectOfType(typeof( MusicListenerController));
			transform.position = musicListener.transform.position;
			transform.parent = musicListener.transform;
			//if (music.clip == null || music.clip.name.CompareTo (musicListener.musicClip.name) != 0)
			//{
				//music.Stop();

			//}
		}
		else
			DestroyImmediate(this.gameObject);

	}

	void OnDestroy()
	{
		PlayerPrefs.SetInt(PlayerPrefHandler.keyMusic, (isMusicOn ? 1 : 0));
		PlayerPrefs.SetInt(PlayerPrefHandler.keySound, (isSoundOn ? 1 : 0));
		PlayerPrefs.Save();

		if (instance == this)
			instance = null;
	}

	void OnLevelWasLoaded(int level) {
		musicListener = (MusicListenerController) FindObjectOfType(typeof( MusicListenerController));
		transform.position = musicListener.transform.position;
		transform.parent = musicListener.transform;
		//if (music.clip == null || music.clip.name.CompareTo (musicListener.musicClip.name) != 0)
		//{
			//music.Stop();
			music.clip = musicListener.musicClip;
			music.Play();
		//}
		
	}

	// Use this for initialization
	void Start () {
		music.clip = musicListener.musicClip;
		music.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ImplementMusic()
	{
		music.mute = !isMusicOn;
	}

	void ImplementSound()
	{
			buttonSnd.mute = !isSoundOn;
		for (int i = 0; i < sfx.Count; ++i)
		{
			sfx[i].mute = !isSoundOn;
		}
	}

	public void PlayButton()
	{
		//if ( SoundOn )
			buttonSnd.Play();
	}
}
