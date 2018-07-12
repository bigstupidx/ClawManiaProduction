using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class sc_hscore : MonoBehaviour {
	public AudioClip soClickButton;
	string postURLgetLeaderboard = "felix/api/getLeaderBoard";
	//http://128.199.223.154:12000/felix/api/getLeaderBoard

	public Text[] txtNames; 
	public Text[] txtScores; 

	// Use this for initialization
	void Start () {
		//Debug.Log ("HScore : " + PlayerPrefs.GetInt("highscore"));

		for (int i=0; i<5; i++) {
			//txtNames[i].text = "";
			//txtScores[i].text = "";
		}

		//get_leaderboard_API();
	}

	public void get_leaderboard_API() {
		string userid = PlayerPrefs.GetString(PlayerPrefHandler.keyUserName);
		string url = PlayerPrefs.GetString("ipaddress") + postURLgetLeaderboard;
		WWW www = new WWW(url);
		//StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		if (www.error == null) {
			Debug.Log("Load Leaderboard Ok!: " + www.text);
			SimpleJSON.JSONNode rsl = SimpleJSON.JSON.Parse(www.text);
			Debug.Log("Length : " + rsl["aaData"].Count);

			int barisAwal=1;
			for (int i=0; i< rsl["aaData"].Count; i++) {
				Debug.Log(rsl["aaData"][i]);

				//data per field
				/*Debug.Log("id:" + rsl["aaData"][a][0]);
				Debug.Log("gameid:" + rsl["aaData"][a][1]);
				Debug.Log("userid:" + rsl["aaData"][a][2]);
				Debug.Log("rangkinno:" + rsl["aaData"][a][3]);
				Debug.Log("score:" + rsl["aaData"][a][4]);
				Debug.Log("status:" + rsl["aaData"][a][5]);*/

				string dataa = rsl["aaData"][i][1].ToString();
				string datab = "\"" + PlayerPrefs.GetInt(PlayerPrefHandler.keyGameId).ToString() + "\"";

				if(dataa==datab) {
					txtNames[i].text = rsl["aaData"][i][2];
					txtScores[i].text = rsl["aaData"][i][4];
					barisAwal++;
				}
			}


		} else {
			Debug.Log("LeaderboardGet Error: "+ www.error);
		}    
	}

	public void balik_ke_menu() {
		Debug.Log ("Balik ke Menuuuuuuuuuuuuu");
		GetComponent<AudioSource>().PlayOneShot (soClickButton, 1f);
		//PlayerPrefs.SetString(PlayerPrefHandler.keyHalAsal,"highscore");
		//Application.LoadLevel("scene_utama");
		sc_mainmenu_canvas_handler.Instance().cvHighscore.gameObject.SetActive( false );
	}
}
