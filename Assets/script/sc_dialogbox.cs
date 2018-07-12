using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_dialogbox : MonoBehaviour {


	public Text text;
	public GameObject buttonCancel;
	// Use this for initialization
	void Start () {

	}

	public void Show(string text, bool showCancel)
	{
		this.text.text = text;
		buttonCancel.gameObject.SetActive (showCancel);
		this.gameObject.SetActive (true);
	}

	public void OnClickCancel()
	{
		this.gameObject.SetActive (false);
	}
	
	public void OnClickOk()
	{
		this.gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
