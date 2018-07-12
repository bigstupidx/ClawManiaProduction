using UnityEngine;
using System.Collections;

public class GemuPaymentController : MonoBehaviour {

	public int currentType = -1; // 0 - 5

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

	public void OnCloseButton()
	{
		SoundManager.instance.PlayButton();

		GUI_Dialog.ReleaseTopCanvas();

		gameObject.SetActive(false);
	}

	public void OnGoogleButton()
	{
		SoundManager.instance.PlayButton();

	}

	public void OnMolButton()
	{
		SoundManager.instance.PlayButton();
	}

	public void OnUnipinButton()
	{
		SoundManager.instance.PlayButton();
	}
}
