using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_pause_music : MonoBehaviour {
	public Texture music_sound_on;
	public Texture music_sound_off;
	RawImage imgBtnOn;

	void Start() 
	{
		imgBtnOn = this.GetComponent<RawImage>();
		if (PlayerPrefs.GetInt (PlayerPrefHandler.keyMusic) == 0) {
			imgBtnOn.texture = music_sound_off;
		} else {
			imgBtnOn.texture = music_sound_on;
		}
	}

	public void pause_music_btn_on_off() {
		SoundManager.instance.PlayButton();

		if (PlayerPrefs.GetInt (PlayerPrefHandler.keyMusic) == 0) {
			imgBtnOn.texture = music_sound_on;
			PlayerPrefs.SetInt(PlayerPrefHandler.keyMusic,1);
			GameObject.Find("Main Camera").SendMessage("nyalan_mati_bgsound");
		} else {
			imgBtnOn.texture = music_sound_off;
			PlayerPrefs.SetInt(PlayerPrefHandler.keyMusic,0);
			GameObject.Find("Main Camera").SendMessage("nyalan_mati_bgsound");
		}

		PlayerPrefs.Save();
	}

}
