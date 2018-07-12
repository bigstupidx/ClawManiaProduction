using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_prebola : MonoBehaviour {
	float powerX=1350f;
	float powerY=2000f;
	
	float addPowX = 0;
	float addPowY = 0;
	float powXbola = 4.2f;
	float powYbola = 17.0f;
	bool statTekan=false;
	bool waktuAnim=false;
	
	public bool statMasuk=false;
	bool statKenaTanahFirst=false;
	bool bolaDilempar=false;
	
	float kemiringan=0;

	float delayShoot=2;
	int toloreansiMiring=10;
	int kekuatanPutaran=200;

	public Texture2D textureBola1;
	public Texture2D textureBola2;
	public Texture2D textureBola3;

	bool nextShoot=false;
	sc_waktu scriptWaktu;

	Text teksTouch;

	float posYMouseDitekan;

	public AudioClip soBolaKenaRing;
	public AudioClip soBolaKenaTanah;
	public AudioClip soBolaKenaBola;
	public AudioClip soBolaMasuk;
	public AudioClip soExplode;



	// Use this for initialization
	void Start () {
		hilangkan_api_bola ();
		random_material_bola ();
		nextShoot = true;
		scriptWaktu = GameObject.Find("txtWaktu").GetComponent<sc_waktu>();

		//teksTouch = GameObject.Find("txtTouch").GetComponent<Text>();
	}

	void random_material_bola() {
		int nilaiRandom = Random.Range (1, 4);
		if (nilaiRandom == 1) {
			this.GetComponent<Renderer>().material.mainTexture = textureBola1;
		} else if (nilaiRandom == 2) {
			this.GetComponent<Renderer>().material.mainTexture = textureBola2;
		} else {
			this.GetComponent<Renderer>().material.mainTexture = textureBola3;
		}
	}

	void Update () {
		//Debug.Log ("Touch Count : " + Input.touchCount + " => " + Input.GetTouch(0).phase);
		//if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)

		if(scriptWaktu.statOpeningDone==true) {
			if(scriptWaktu.waktu_main>0 && PlayerPrefs.GetInt(PlayerPrefHandler.keyPause)==0) {
				//teksTouch.text = "" + Input.touches.Length;
				//Debug.Log("touches : " + Input.touches.Length);

				if(statTekan==true) {
					delayShoot -= 1*(Time.deltaTime*1.5f);
					//Debug.Log("Delay Shoot : " + delayShoot);
					if(delayShoot<=0 && nextShoot==true) {
						statTekan=false;
						waktuAnim=true;
						StartCoroutine("proses_bola");
						GameObject.Find("hand_anim").SendMessage("panggil_animasi_tangan");
						nextShoot=false;
					}

					if(nextShoot==true) {
						sc_bar scriptBar;

						#if UNITY_EDITOR
						if(Input.GetAxis("Mouse Y")>0){
							scriptBar = GameObject.Find("Main Camera").GetComponent<sc_bar>();
							if(scriptBar.barDisplay<1) {

								if(Input.mousePosition.y > posYMouseDitekan) {
									float jarakMouse = Input.mousePosition.y - posYMouseDitekan;
									//Debug.Log("Jarak Mouse : " + jarakMouse);
									addPowX = jarakMouse * Time.deltaTime;
									addPowY = jarakMouse * Time.deltaTime;
									powerX += addPowX * 10;
									powerY += addPowY * 12;
									GameObject.Find("Main Camera").SendMessage("resize_powerBar",addPowX);
								} 

							}
						}
						#endif
						
						#if UNITY_ANDROID
						if(Input.touchCount>0) {
							if(Input.GetTouch(0).phase==TouchPhase.Moved) {
								scriptBar = GameObject.Find("Main Camera").GetComponent<sc_bar>();
								if(scriptBar.barDisplay<1) {


									if(Input.GetTouch(0).position.y > posYMouseDitekan) {
										float jarakMouse = Input.GetTouch(0).position.y - posYMouseDitekan;
										Debug.Log("Jarak Mouse : " + jarakMouse);
										addPowX = jarakMouse/2 * Time.deltaTime;
										addPowY = jarakMouse/2 * Time.deltaTime;
										powerX += addPowX * 10;
										powerY += addPowY * 12;
										GameObject.Find("Main Camera").SendMessage("resize_powerBar",addPowX);
									} 

								}
							}
						}
						#endif

					}
					if(Input.GetAxis("Mouse X") > 0) {
						kemiringan++;
					} else if (Input.GetAxis("Mouse X") < 0) {
						kemiringan--;
					}
					//Debug.Log("Kemiringan X : " + kemiringan);
				}
				
				if(bolaDilempar) {
					this.transform.Rotate(Vector3.forward * Time.deltaTime * -kekuatanPutaran);
				}
				
				Rect bounds = new Rect(0, 0, Screen.width, Screen.height);

				#if UNITY_EDITOR
		        if (Input.GetMouseButtonDown(0) && bounds.Contains(Input.mousePosition))
		        {
		         	statTekan=true;
					posYMouseDitekan = Input.mousePosition.y;
		        } else if (Input.GetMouseButtonUp(0)) {
					if(statTekan==true && nextShoot==true) {
						statTekan=false;
						waktuAnim=true;
						StartCoroutine("proses_bola");
						GameObject.Find("hand_anim").SendMessage("panggil_animasi_tangan");
						nextShoot=false;
					}
				}
				#endif

				#if UNITY_ANDROID
				if(Input.touchCount>0) {
					if (Input.GetTouch(0).phase==TouchPhase.Began)
					{
						statTekan=true;
						//Debug.Log("Touch Begin Position : " + Input.GetTouch(0).position.y);
						posYMouseDitekan = Input.GetTouch(0).position.y;
					} else if (Input.GetTouch(0).phase==TouchPhase.Ended) {
						if(statTekan==true && nextShoot==true) {
							statTekan=false;
							waktuAnim=true;
							StartCoroutine("proses_bola");
							GameObject.Find("hand_anim").SendMessage("panggil_animasi_tangan");
							nextShoot=false;
						}
					}
				}
				#endif

				if (waktuAnim == true && bolaDilempar==false) {
					//Debug.Log("waktuAnim : " + waktuAnim);
					this.transform.Translate(Vector3.up * 2 * Time.deltaTime);		//naikkanbola
					this.transform.Translate(Vector3.right * 5 * Time.deltaTime); 	//mundurkan bola
				}
			} else {


			}
		} 
	}
	
	/*
	void OnMouseDown() {
		statTekan=true;
	}
	void OnMouseUp() {
		if(statTekan==true) {
			statTekan=false;
			StartCoroutine("proses_bola");
		}
	}
	*/
	
	IEnumerator proses_bola() {
		if(statTekan==false) {
			if(this.gameObject.GetComponent<Rigidbody>()==null) {
				yield return new WaitForSeconds(0.5f);
				waktuAnim=false;

				Rigidbody gameObjectRigidBody = this.gameObject.AddComponent<Rigidbody>();
				gameObjectRigidBody.mass=5;
				//Debug.Log(this.gameObject);
				//gameObjectRigidBody.mass=5;
				
				if(kemiringan>toloreansiMiring || kemiringan<-toloreansiMiring) { 
					kemiringan -= toloreansiMiring; 
					kemiringan *= 50;
				}
				//Debug.Log("kemiringan : " + kemiringan);
				this.GetComponent<Rigidbody>().AddForce(new Vector3(-powerX,powerY,kemiringan));
				bolaDilempar=true;
				waktuAnim=false;

				yield return new WaitForSeconds(0.5f);
				scriptWaktu = GameObject.Find("txtWaktu").GetComponent<sc_waktu>();
				if(scriptWaktu.waktu_main>0) {	
					GameObject.Find("posisiBola").SendMessage("buat_bola_baru");
					GameObject.Find("Main Camera").SendMessage("refreshBar");
					powerX=1650f;
					addPowX=0;
					kemiringan=0;
					delayShoot=2;
				}
			}

		}
	}
	
	void update_status_masuk() {
		statMasuk=true;
		sc_ringin scriptRingin = GameObject.Find ("ring_in").GetComponent<sc_ringin> ();
		if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
			GetComponent<AudioSource>().PlayOneShot (soBolaMasuk, 10f);
		}
		//Debug.Log ("Combo Masukkk ::::::::::: " + scriptRingin.comboMasuk);
		if (scriptRingin.comboMasuk == 1) {
			this.gameObject.GetComponent<TrailRenderer> ().enabled = true;
			if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
				GetComponent<AudioSource>().PlayOneShot (soExplode, 5f);
			}
			GameObject.Find("objSound_say").SendMessage("say_combo");
		} else {
			GameObject.Find("objSound_say").SendMessage("say_praise");
		}

	}
	
	void OnCollisionEnter(Collision tumbukkan) {
		//Debug.Log("bola tabrak :::::::::::::::: " + tumbukkan.gameObject.name);

		if (tumbukkan.gameObject.name == "tanah") {
			if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
				//audio.PlayOneShot (soBolaKenaTanah, 5.0f);
			}
			if (statMasuk == false && statKenaTanahFirst == false) {
				statKenaTanahFirst = true;
				GameObject.Find ("ring_in").SendMessage ("reset_combo");
			}
			
			Destroy (this.gameObject, 5);
		} else if (tumbukkan.gameObject.name == "ring") {
			if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
				GetComponent<AudioSource>().PlayOneShot (soBolaKenaRing, 10.0f);
			}
		} else if (tumbukkan.gameObject.name == "prebola(clone)" || tumbukkan.gameObject.name=="papan") {
			if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
				GetComponent<AudioSource>().PlayOneShot(soBolaKenaBola, 15.0f);
			}
		}
	}


	void hilangkan_api_bola() {
		this.gameObject.GetComponent<TrailRenderer>().enabled = false;
	}
}
