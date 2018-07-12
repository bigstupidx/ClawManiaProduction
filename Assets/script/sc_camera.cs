using UnityEngine;
using System.Collections;

public class sc_camera : MonoBehaviour {
	float speedRotate=3.0f;
	public Canvas cvPause;
	public Canvas cvQuit;
	//public Canvas cvRetry;

	void Awake() {
		if (GameObject.Find ("cont_music_bgm") != null) {
			Destroy (GameObject.Find ("cont_music_bgm").gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt(PlayerPrefHandler.keyPause,0);
		//cvPause = GameObject.Find ("Canvas_pause").GetComponent<Canvas> ();
		cvPause.gameObject.SetActive(false);
		cvQuit.gameObject.SetActive(false);
		//cvRetry.gameObject.SetActive(false);

		PlayerPrefs.Save();

		//Debug.Log("--- Gameplay : " + PlayerPrefs.GetString(PlayerPrefHandler.keyUserName) + " | statLogin : " + PlayerPrefs.GetString (PlayerPrefHandler.keyLoginActive) + " ---");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Space)) { 
			Debug.Log("klik pause");
			if(PlayerPrefs.GetInt(PlayerPrefHandler.keyPause)==0) {
				PlayerPrefs.SetInt(PlayerPrefHandler.keyPause,1);
				cvPause.gameObject.SetActive(true);
				Debug.Log("klik pause1");
			} else {
				PlayerPrefs.SetInt(PlayerPrefHandler.keyPause,0);
				cvPause.gameObject.SetActive(false);
				Debug.Log("klik pause0");
			}
		}

		float korX = Input.GetAxis("Horizontal") * Time.deltaTime * 6.0f;
		float korY = Input.GetAxis("Vertical") * Time.deltaTime * 6.0f;

		this.transform.Translate(new Vector3(korX, korY, 0));

		if (Input.GetKey (KeyCode.A)) {
			this.transform.Rotate(new Vector3(0f, speedRotate, 0.0f));
		} else if (Input.GetKey (KeyCode.D)) {
			this.transform.Rotate(new Vector3(0f, -speedRotate, 0.0f));
		} else if (Input.GetKey (KeyCode.W)) {
			this.transform.Rotate(new Vector3(speedRotate, 0f, 0.0f));
		} else if (Input.GetKey (KeyCode.S)) {
			this.transform.Rotate(new Vector3(-speedRotate, 0f, 0.0f));
		}
		PlayerPrefs.Save();
	}


	public void balik_ke_menu() {
		SoundManager.instance.PlayButton();

		PlayerPrefs.SetInt(PlayerPrefHandler.keyPause,0);
		PlayerPrefs.SetString(PlayerPrefHandler.keyHalAsal,"game");

		PlayerPrefs.Save();
		Application.LoadLevel ("scene_utama");
	}

	public void OpenQuit()
	{
		SoundManager.instance.PlayButton();
		cvQuit.gameObject.SetActive(true);
	}

	
	public void HideQuit()
	{
		SoundManager.instance.PlayButton();
		cvQuit.gameObject.SetActive(false);
	}

	public void OpenRetry()
	{
		SoundManager.instance.PlayButton();
		//cvRetry.gameObject.SetActive(true);
	}
	
	
	public void HideRetry()
	{
		SoundManager.instance.PlayButton();
		//cvRetry.gameObject.SetActive(false);
	}

	public void hilangkan_pause_game() {
		SoundManager.instance.PlayButton();

		PlayerPrefs.SetInt(PlayerPrefHandler.keyPause,0);

		PlayerPrefs.Save();
		cvPause.gameObject.SetActive(false);	
	}

}
