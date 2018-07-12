using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GUI_GL1_Promo : GUI_Dialog {
	public GUI_Gl1_Login guiLogin;
	public UIInput inputKode;
	public GUI_DialogBox dialogBox;

	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public void OnClickPromo()
	{
		GL1Connector.GetInstance().Promo (this.gameObject,inputKode.value);
	}
	
	public void OnGL1Done(JSONNode N)
	{
		if ( GL1Connector.GetInstance().GetLastURL().Contains("promo") )
		{
			if (N["errcode"].ToString() == "\"0\"")
			{
				dialogBox.Show("Info", "Promo success",false,"",this.gameObject);
				
			}
			else
			{
				dialogBox.Show("Info", N["errdetail"],false,"",this.gameObject);
			}
		}
	}
}
