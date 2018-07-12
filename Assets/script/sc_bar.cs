using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_bar : MonoBehaviour {
	
	public float barDisplay = 0;
	//Vector2 pos = new Vector2(20,Screen.height-220);
	//Vector2 size = new Vector2 (20, 200);
	public Texture progressBarEmpty;
	public Texture progressBarFull;
	
	float pindahTarget=0;
	float target3Point=350;
	//float powerBar=3f;
	public Scrollbar powerBarShoot;
	public AudioClip bgSound;

	Image imgBar;
	AudioSource audioSourceBG;

	void Start() {
		audioSourceBG = this.GetComponent<AudioSource>();

		update_map_ring ("2point");
		/*
		if (PlayerPrefs.GetInt (PlayerPrefHandler.keyMusic) == 1) {
			audioSourceBG.clip = bgSound;
			audioSourceBG.loop = true;
			//audioSourceBG.Play();
			audioSourceBG.volume = 1.0f;
			//audioSourceBG.enabled = true;
		} else {
			audioSourceBG.enabled=false;
		}
		*/

		imgBar = GameObject.Find("Handle").GetComponent<Image> ();
		imgBar.fillAmount = 0f;
	}

	public void nyalan_mati_bgsound() {
		Debug.Log ("Music : " + PlayerPrefs.GetInt (PlayerPrefHandler.keyMusic));

		SoundManager.instance.MusicOn = (PlayerPrefs.GetInt (PlayerPrefHandler.keyMusic) == 1);
	}

	void Update () {

	}

	public void resize_powerBar(float power_shoot) {
		imgBar.fillAmount += (power_shoot) / 100;
	}

	void update_map_ring(string status) {

		if (status == "2point") {
			GameObject.Find ("map_ring").transform.position = GameObject.Find ("point_map_2point").transform.position;
		} else {
			GameObject.Find ("map_ring").transform.position = GameObject.Find ("point_map_3point").transform.position;
		}
	}

	public void Pindahkan_target_ring() {
		Vector3 posMap = GameObject.Find ("map_ring").transform.position;
		Vector3 pos3point = GameObject.Find ("point_map_3point").transform.position;

		if(posMap.y < pos3point.y) {
			posMap.y += 2;
			GameObject.Find ("map_ring").transform.position = posMap;
		} else {
			GameObject.Find ("map_ring").transform.position = GameObject.Find ("point_map_3point").transform.position;
		}
	}
	public void paskan_target_ring() {
		GameObject.Find ("map_ring").transform.position = GameObject.Find ("point_map_3point").transform.position;
	}
	
	public void refreshBar() {
		barDisplay = Time.time * 0f;
		//powerBarShoot.size = 0f;
		imgBar.fillAmount = 0f;
		//Debug.Log ("===> resize Power bar : " + powerBarShoot.size);
	}

	public void lagu_bg_stop() {
		//GetComponent<AudioSource>().Stop ();
		SoundManager.instance.music.Stop();
	}
}
