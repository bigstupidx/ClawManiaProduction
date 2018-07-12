using UnityEngine;
using System.Collections;

public class GUI_BasketSetting : GUI_Dialog {
	public Texture[] soundIcons;
	public Texture[] musicIcons;
	public UITexture textureMusic;
	public UITexture textureSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RefreshInfo()
	{
		int musicOn = PlayerPrefs.GetInt(PlayerPrefHandler.keyMusic);
		int soundOn = PlayerPrefs.GetInt(PlayerPrefHandler.keySound);
		textureMusic.mainTexture = musicIcons[musicOn];
		textureSound.mainTexture = soundIcons[soundOn];

	}

	public void ToogleSound ()
	{
		int soundOn = PlayerPrefs.GetInt(PlayerPrefHandler.keySound);
		if (soundOn == 0) {
			soundOn = 1;
			SoundManager.instance.SoundOn = true;
		} else if (soundOn == 1) {
			soundOn = 0;
			SoundManager.instance.SoundOn = false;
		}
		PlayerPrefs.SetInt(PlayerPrefHandler.keySound,soundOn);
		RefreshInfo ();
	}

	public void ToogleMusic()
	{
		int musicOn = PlayerPrefs.GetInt(PlayerPrefHandler.keyMusic);
		if (musicOn == 0) {
			musicOn = 1;
		} else if (musicOn == 1) {
			musicOn = 0;
		}
		PlayerPrefs.SetInt(PlayerPrefHandler.keyMusic,musicOn);
		if ( musicOn == 0 )
			SoundManager.instance.music.Stop();
		else if ( musicOn == 1 )
			SoundManager.instance.music.Play();
		RefreshInfo ();
	}

	public override void OnShow()
	{

	}
}
