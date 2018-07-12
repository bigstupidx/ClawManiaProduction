using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_dataload : MonoBehaviour {
	Text textDataLoad;
	
	public string getURL = "http://128.199.223.154:12000/felix/api/getUsersAsset/TEST";

	// Use this for initialization
	void Start () {
		StartCoroutine (loadData());
	}



	IEnumerator loadData() {
		//Debug.Log ("ambil data dari : " + getURL);
		WWW getname = new WWW (getURL);
		yield return getname;
		//Debug.Log ("data : " + getname.text);

		//textDataLoad = GetComponent<Text> ();
		//textDataLoad.text = getname.text;
	}
	
}
