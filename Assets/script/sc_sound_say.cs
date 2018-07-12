using UnityEngine;
using System.Collections;

public class sc_sound_say : MonoBehaviour {
	public AudioClip sayNiceShoot;
	public AudioClip sayVeryGood;
	public AudioClip sayCombo;

	void say_praise() {
		if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
			float randSay = Random.Range (0f, 2f);
			if (randSay > 1f) {
				GetComponent<AudioSource>().PlayOneShot (sayNiceShoot, 10f);
			} else {
				GetComponent<AudioSource>().PlayOneShot (sayVeryGood, 10f);
			}
		}
	}

	void say_combo() {
		if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
			GetComponent<AudioSource>().PlayOneShot (sayCombo, 15f);
		}
	}
}
