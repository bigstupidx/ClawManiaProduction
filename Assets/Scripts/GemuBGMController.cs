using UnityEngine;
using System.Collections;

public class GemuBGMController : MonoBehaviour {

	private const string keyGemuBGM = "keyGemuBGM";

	public static GemuBGMController instance;

	public MusicListenerController musicListener;

	public AudioClip[] BGMs;
	public int currentBGMIdx = 0;

	void Awake()
	{
		currentBGMIdx =  PlayerPrefs.GetInt (keyGemuBGM, 0);
		if (currentBGMIdx >= BGMs.Length)
			currentBGMIdx = 0;
		musicListener.musicClip = BGMs[currentBGMIdx];
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			DestroyImmediate(this.gameObject);
			return;
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBGMChange(int idx)
	{
		PlayerPrefs.SetInt (keyGemuBGM, idx);
		PlayerPrefs.Save();

		currentBGMIdx = idx;
		SoundManager.instance.music.clip = BGMs[currentBGMIdx];
		SoundManager.instance.music.Play ();
	}
}
