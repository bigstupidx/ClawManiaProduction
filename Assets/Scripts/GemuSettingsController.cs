using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuSettingsController : GUI_Dialog {

	public GemuCreditsController canvasCredits;

	public GameObject[] musicSprites = new GameObject[2];
	public GameObject[] soundSprites = new GameObject[2];

	public UIButton[] buttonMusic;
	public GemuBGMController gemuBGMController;
	void RefreshInfo()
	{
		bool sprId = SoundManager.instance.MusicOn ? true : false;
		musicSprites [0].gameObject.SetActive (!sprId);
		musicSprites [1].gameObject.SetActive (sprId);

		sprId = SoundManager.instance.SoundOn ? true : false;
		soundSprites [0].gameObject.SetActive (!sprId);
		soundSprites [1].gameObject.SetActive (sprId);
	}

	public void OnChooseMusic()
	{
		UIButton button = UIButton.current;
		if (button == null)
			return;
		for (int i=0; i<buttonMusic.Length; i++) {
			if ( button == buttonMusic[i] ) {
				gemuBGMController.OnBGMChange(i);
				break;
			}
		}
	}

	public override void OnTweenDone()
	{
		this.gameObject.SetActive (false);
	}

	public override void OnShow()
	{
		RefreshInfo ();
	}

	// Use this for initialization
	public override void OnStart () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonMusic()
	{
		SoundManager.instance.PlayButton();

		SoundManager.instance.MusicOn = !SoundManager.instance.MusicOn;
		RefreshInfo ();

	}

	public void OnButtonSound()
	{
		SoundManager.instance.PlayButton();

		SoundManager.instance.SoundOn = !SoundManager.instance.SoundOn;
		
		RefreshInfo ();

	}

	public void OnButtonCredits()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.InsertStack(canvasCredits.gameObject);

	}

	public void OnButtonBack()
	{
		SoundManager.instance.PlayButton();

		GUI_Dialog.ReleaseTopCanvas();

	}
}
