using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CheckVersion : MonoBehaviour {
	public GUI_DialogBox guiDialogBox;
	string game_ver = "1.7.1";
	string gameID = "CLAW";
	// Use this for initialization
	void Start () {
		//GameObject.Find ("VersionText").GetComponent<UILabel> ().text = game_ver;
	}

	public void RunCheckVersion(){
		string postURL = "https://www.gemugemu.com/api/checkver.php";
		string jsonString = "{\"gameid\":\"" + gameID + "\"," +
			"\"game_ver\":\""  + game_ver    + "\"" + 
				"}";
		
		var encoding = new System.Text.UTF8Encoding ();
		
		WWW www = new WWW (postURL, encoding.GetBytes (jsonString));
		
		StartCoroutine (Sending (www)); 
	}

	IEnumerator Sending(WWW www){
		yield return www;

		if (www.error == null) {
			JSONNode data = JSONNode.Parse (www.text);

			string status = data ["gamedata"] ["status"];
			string currGameVer = data["gamedata"]["version"];
			string desc = "";

			if(currGameVer != game_ver)
				status="2";

			switch(status){
			case "0":
				desc = "Server is disabled. Please try again later";
				break;
			case "1":
				Debug.Log("enabled");
				break;
			case "2":
				desc = "Please update to the latest version";
				break;
			case "3":
				desc = "Under maintenance. Please try again later";
				break;
			}

			if(status != "1")
				guiDialogBox.Show ("Info", desc, false, "checkver"+status,this.gameObject);

		} else {
			Debug.Log ("WWW error! "+www.error);
		}
	}
}
