using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_pause_sound : MonoBehaviour {
	public Texture texture_sound_on;
	public Texture texture_sound_off;
	RawImage imgBtnOn;
	int sementara = 1;

	// Use this for initialization
	void Start () {
		imgBtnOn = this.GetComponent<RawImage>();
		if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 0) {
			imgBtnOn.texture = texture_sound_off;
		} else {
			imgBtnOn.texture = texture_sound_on;
		}
	}
	
	public void pause_sound_btn_on_off() {
		SoundManager.instance.PlayButton();

		if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 0) {
			SoundManager.instance.SoundOn = true;
			imgBtnOn.texture = texture_sound_on;
			PlayerPrefs.SetInt(PlayerPrefHandler.keySound,1);
		} else {
			SoundManager.instance.SoundOn = false;
			imgBtnOn.texture = texture_sound_off;
			PlayerPrefs.SetInt(PlayerPrefHandler.keySound,0);
		}
		PlayerPrefs.Save();

	}


}
