using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuRedeemController : MonoBehaviour {

	public Text textTix;

	void Awake()
	{
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnYesButton()
	{
		SoundManager.instance.PlayButton();

		//GemuMainMenuController.instance.ReleaseTopCanvas();
		gameObject.SetActive(false);
	}

	public void OnNoButton()
	{
		SoundManager.instance.PlayButton();

		//GemuMainMenuController.instance.ReleaseTopCanvas();
		gameObject.SetActive(false);
	}
}
