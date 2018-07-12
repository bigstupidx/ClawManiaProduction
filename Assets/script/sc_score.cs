using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_score : MonoBehaviour {
	public int score=0;
	Text textScore;
	sc_ringin scriptRingIn;

	// Use this for initialization
	void Start () {
		score = PlayerPrefs.GetInt (PlayerPrefHandler.keyCurrentScore, 0);
		textScore = GetComponent<Text> ();
		textScore.text = score_empat_digit(score);
		//this.guiText.text = score_empat_digit(score);
	}

	string score_empat_digit(int angka_score) {
		int pjgScore = score.ToString ().Length;
		string tulisanScore = "000";
		if(pjgScore==1) {
			tulisanScore = "00"+angka_score;
		} else if (pjgScore==2) {
			tulisanScore = "0"+angka_score;
		} else if (pjgScore==3) {
			tulisanScore = ""+angka_score;
		}
		return tulisanScore;
	}
	
	public void score_tambah() {
		scriptRingIn = GameObject.Find("ring_in").GetComponent<sc_ringin>();

		//stat3Point
		sc_waktu scriptWaktu = GameObject.Find("txtWaktu").GetComponent<sc_waktu>();
		if(scriptWaktu.waktu_main>0) {
			if(scriptWaktu.stat3Point==false) {
				//score+=2+scriptRingIn.tambahPoint;
				score += 2;
			} else {
				//score+=3+scriptRingIn.tambahPoint;
				score += 3;
			}
		}
		//this.guiText.text = score_empat_digit(score);
		textScore.text = score_empat_digit(score);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
