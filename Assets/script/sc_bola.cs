using UnityEngine;
using System.Collections;

public class sc_bola : MonoBehaviour {
	public GameObject preBola; 
	
	void Awake() {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
	
	// Use this for initialization
	void Start () {
		Instantiate(preBola,transform.position, transform.rotation);
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void buat_bola_baru() {
		Instantiate(preBola,transform.position, transform.rotation);
	}
}
