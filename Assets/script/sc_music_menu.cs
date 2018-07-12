using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_music_menu : MonoBehaviour {
	//AudioSource audioSourceBGM;
	public RawImage rImgMusic;
	public Texture musicActive;
	public Texture musicNonActive;

	private static bool created=false;

	void Awake() {
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyHalAsal) == false) {
			PlayerPrefs.SetString(PlayerPrefHandler.keyHalAsal,"awal");
			PlayerPrefs.Save();
		}
		//audioSourceBGM = this.gameObject.GetComponent<AudioSource> ();

		if (created == false) {
			//DontDestroyOnLoad (this.gameObject);
			created = true;
		} else {
			settingMusicOnOff();
			Debug.Log("Halaman Asal ::::::::::::::::::: " + PlayerPrefs.GetString(PlayerPrefHandler.keyHalAsal));
			if(PlayerPrefs.GetString(PlayerPrefHandler.keyHalAsal)=="game") {
				//DontDestroyOnLoad (this.gameObject);
			} else if (PlayerPrefs.GetString(PlayerPrefHandler.keyHalAsal)=="highscore") {
				//Destroy(this.gameObject);
			}
		}
	}

	
	// Use this for initialization
	void Start () {
		//rImgMusic = GameObject.Find ("btnMusic").GetComponent<RawImage> ();
		//settingMusicOnOff ();
	}

	private void settingMusicOnOff() {
		Debug.Log("setting Music OnOFF : " + PlayerPrefs.GetInt(PlayerPrefHandler.keyMusic));
		if (rImgMusic == null) {
			//rImgMusic = GameObject.Find ("btnMusic").GetComponent<RawImage> ();
		}

		if (PlayerPrefs.GetInt(PlayerPrefHandler.keyMusic)==1) {
			//rImgMusic.texture = musicActive;
			//audioSourceBGM.Play();
			//PlayerPrefs.SetInt (PlayerPrefHandler.keyMusic, 1);
		} else {
			//rImgMusic.texture = musicNonActive;
			//audioSourceBGM.Stop();
			//PlayerPrefs.SetInt (PlayerPrefHandler.keyMusic, 0);
		}
		PlayerPrefs.Save();

	}

	public void atur_music_bgm() {
		MusicOnOff_clicked ();
	}

	private void MusicOnOff_clicked() {

		if (PlayerPrefs.GetInt(PlayerPrefHandler.keyMusic, 1)==1) {
			rImgMusic.texture = musicNonActive;
			//audioSourceBGM.Stop();
			SoundManager.instance.MusicOn = false;
			PlayerPrefs.SetInt (PlayerPrefHandler.keyMusic, 0);
		} else {
			rImgMusic.texture = musicActive;
			//audioSourceBGM.Play();
			SoundManager.instance.MusicOn = true;
			PlayerPrefs.SetInt (PlayerPrefHandler.keyMusic, 1);
		}
		PlayerPrefs.Save();
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
