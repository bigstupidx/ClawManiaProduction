using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_target : MonoBehaviour {
	Text textTarget;
	sc_stage scriptStage;
	public int targetLevel=40;

	// Use this for initialization
	void Start () {
		textTarget = GetComponent<Text> ();
		textTarget.text = "000";
		targetLevel = 40;

		scriptStage = GameObject.Find("txtStage").GetComponent<sc_stage>();
		if (scriptStage.levelAktif == 1) {
			targetLevel=40;
			//targetLevel=1;
			textTarget.text = "" + targetLevel;
		} else if (scriptStage.levelAktif == 2) {
			targetLevel=150;
			//targetLevel=2;
			textTarget.text = "" + targetLevel;
		} else if (scriptStage.levelAktif == 3) {
			targetLevel=250;
			//targetLevel=3;
			textTarget.text = "" + targetLevel;
		} else if (scriptStage.levelAktif == 4) {
			targetLevel=999;
			//targetLevel=4;
			textTarget.text = "-";
		}
		//targetLevel = 0;
		//textTarget.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
