using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuDialogBoxController : GUI_Dialog {
	public UILabel text;
	public UILabel title;
	//static GemuDialogBoxController instance;
	public UIButton buttonOk;
	public UIButton buttonCancel;
	// Use this for initialization
	public void OnStart () {
		//instance = this;
	}

	void OnAwake()
	{
		//instance = this;
	}

	void OnDestroy()
	{
		//instance = null;
	}
	GameObject lastsender;
	string sCurrExec = "";
	bool bLastCancel = false;
	public void Show(string title,string text,bool bShowCancel,string sExec,GameObject sender)
	{
		this.text.text = text;
		this.title.text = title;

		bLastCancel = bShowCancel;

		sCurrExec = sExec;
		lastsender = sender;
		//base.Show ();
		
		GUI_Dialog.InsertStack(this.gameObject);
	}

	public override void OnShow()
	{
		if ( bLastCancel )
		{
			buttonCancel.gameObject.SetActive(true);
		}
		else
		{
			buttonCancel.gameObject.SetActive(false);
		}
	}

	public void OnClickBack()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas ();
	}
	
	public void OnClickOk()
	{
		if ( string.IsNullOrEmpty ( sCurrExec ) == false )
		{
			if ( sCurrExec == "logoutfb" )
			{
				lastsender.SendMessage("ConfirmLogoutFB");
			}
		}

		lastsender = null;
		sCurrExec = "";
		OnClickBack ();
	}

	/*
	public static GemuDialogBoxController GetInstance()
	{
		return instance;
	}
	*/
	
	public override void OnTweenDone()
	{
	}

	// Update is called once per frame
	void Update () {
	
	}
}
