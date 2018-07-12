using UnityEngine;
using System.Collections;

public class sc_ringin : MonoBehaviour {
	public int comboMasuk=0;
	private int comboMax = 0;
	public int tambahPoint=0;

	private GUIText txtCombo;
	// Use this for initialization
	void Start () {
		txtCombo = GameObject.Find("txtCombo").GetComponent<GUIText>();
		txtCombo.enabled = false;

		comboMax = PlayerPrefs.GetInt(PlayerPrefHandler.keyCombo, 1);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		
		Debug.DrawRay(transform.position, fwd * 2.0f, Color.red);
		if (Physics.Raycast(transform.position, fwd, out hit, 2)) {
			//if(hit.collider.tag=="bolabasket") {
			if(hit.collider.CompareTag("bolabasket")) {
				sc_prebola script = hit.collider.gameObject.GetComponent<sc_prebola>();
				
				//Debug.Log("stat Masuk : " + script.statMasuk);
				if(script.statMasuk==false) {
					hit.collider.gameObject.SendMessage("update_status_masuk");
					cekmasuk();					
				}
				
				
			}
			
		}
	}
	
	void cekmasuk() {
		GameObject.Find("txtScore").SendMessage("score_tambah");
		comboMasuk++;
		if (comboMasuk < 2) {
			//txtCombo.enabled=false;
		} else {
			txtCombo.enabled=true;
			txtCombo.text = comboMasuk + " Combo";
			//Vector3 scale_combo = new Vector3 (2.0f,2.0f,2.0f);
			//iTween.ScaleTo (GameObject.Find ("txtCombo"), iTween.Hash ("scale", scale_combo, "time", 0.5f, "loopType", iTween.LoopType.pingPong));

			if (comboMasuk==2) {
				iTween.ShakePosition(GameObject.Find("Arcade Machine"),iTween.Hash("x",0.7f,"y",0.5f,"time",1.5f));
				tambahPoint=1;
			} else if (comboMasuk==5) {
#if UNITY_ANDROID
				Handheld.Vibrate();
#endif
				iTween.ShakePosition(GameObject.Find("Arcade Machine"),iTween.Hash("x",0.7f,"y",0.5f,"time",1.5f));
				GameObject.Find("Boom").SendMessage("nyalakan_boom");
				tambahPoint=2;
			}

			if (comboMasuk > comboMax)
			{
				comboMax = comboMasuk;
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCombo, comboMax);
				PlayerPrefs.Save();
			}

			GameObject.Find("pre_fire").SendMessage("nyalakan_api");
			//GameObject.Find("pre_bola").SendMessage("beri_api_bola");
			iTween.Stop(txtCombo.gameObject);
			iTween.FadeTo(txtCombo.gameObject, iTween.Hash("alpha", 1.0f, "y", 0.2f, "time", 0.01f));
			StartCoroutine(TextComboTweenCoRtn());
			 
		}
	}

	IEnumerator TextComboTweenCoRtn()
	{
		yield return new WaitForSeconds(0.1f);

		iTween.ShakePosition (txtCombo.gameObject, iTween.Hash ("x", 0.2f, "y", 0.2f, "time", 1.0f));
		iTween.FadeTo(txtCombo.gameObject, iTween.Hash("alpha", 0.0f, "time", 1.0f, "delay", 1.5f));
	}
	
	void reset_combo() {
		comboMasuk=0;
		tambahPoint=0;
		txtCombo.enabled=false;
		GameObject.Find("pre_fire").SendMessage("matikan_api");
		txtCombo.text = comboMasuk + " Combo";
	}
	
}
