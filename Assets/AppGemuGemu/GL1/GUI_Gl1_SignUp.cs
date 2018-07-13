using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GUI_Gl1_SignUp : GUI_Dialog {
	#region GEMU API
	// public UIInput labelPassword;
	// public UIInput labelRetypePassword;
	// public UIInput labelName;
	// public UIInput labelEmail;
	// public UIInput labelReferal;
	// public GUI_DialogBox dialogBox;
	// // Use this for initialization
	
	// public void OnClickBack()
	// {
	// 	GUI_Dialog.ReleaseTopCanvas ();
	// }

	// public override void OnStart ()  {
		
	// }
	
	// // Update is called once per frame
	// void Update () {
		
	// }
	
	// public void OnClickOK()
	// {
	// 	if ( string.IsNullOrEmpty(labelEmail.value) )
	// 	{
	// 		dialogBox.Show("Info", "Please type your email",false,"",this.gameObject);
	// 	}	
	// 	else if ( string.IsNullOrEmpty(labelName.value) )
	// 	{
	// 		dialogBox.Show("Info", "Please type your name",false,"",this.gameObject);
	// 	}
	// 	else if ( string.IsNullOrEmpty(labelPassword.value) )
	// 	{
	// 		dialogBox.Show("Info", "Please type your password",false,"",this.gameObject);
	// 	}
	// 	else if ( string.IsNullOrEmpty(labelRetypePassword.value) )
	// 	{
	// 		dialogBox.Show("Info", "Please retype your password",false,"",this.gameObject);
	// 	}
	// 	else if ( labelPassword.value != labelRetypePassword.value )
	// 	{
	// 		dialogBox.Show("Info", "Different password",false,"",this.gameObject);
	// 	}
	// 	else if ( !labelEmail.value.Contains("@")  )
	// 	{
	// 		dialogBox.Show("Info", "Email does not in the correct format",false,"",this.gameObject);
	// 	}
	// 	else if ( !labelEmail.value.Contains(".") )
	// 	{
	// 		dialogBox.Show("Info", "Email does not in the correct format",false,"",this.gameObject);
	// 	}
	// 	else
	// 	{
			
	// 		string sFBID = "";
	// 		if (SPFacebook.instance.IsInited && SPFacebook.instance.IsLoggedIn)
	// 			sFBID = SPFacebook.instance.UserId;
	// 		GL1Connector.GetInstance().Register(this.gameObject,labelName.value,
	// 		                      labelEmail.value,
	// 		                      "787665566",
	// 		                      "",
	// 		                      "",
	// 		                      "",
	// 		                      "",
	// 		                      "",
	// 		                      labelPassword.value,
	// 		                                    sFBID,
	// 		                      "",
	// 		                      "",
	// 		                      "0",
    //                                 labelReferal.value,
    //                                 SystemInfo.deviceUniqueIdentifier.ToString());
	// 	}
	// }

	// public override void OnShow()
	// {


	// 	labelPassword.value = "";
	// 	labelPassword.gameObject.SetActive (false);
	// 	labelPassword.gameObject.SetActive (true);

	// 	labelRetypePassword.value = "";
	// 	labelRetypePassword.gameObject.SetActive (false);
	// 	labelRetypePassword.gameObject.SetActive (true);

	// 	labelName.value = "";
	// 	labelName.gameObject.SetActive (false);
	// 	labelName.gameObject.SetActive (true);

	// 	labelEmail.value = "";
	// 	labelEmail.gameObject.SetActive (false);
	// 	labelEmail.gameObject.SetActive (true);
	// }

	// public void OnGL1Done(JSONNode N)
	// {
	// 	if (N["errcode"].ToString() == "\"0\"")
	// 	{
	// 		Gamestate gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate>();
	// 		if ( gs )
	// 		{
	// 			gs.achievementManager.OnAchievementEvent(AchievementType.RegisterGemuGemu,1);
	// 			GUI_Dialog.ReleaseTopCanvas();
	// 			gs.ShowDialogBox("Info","You have been registered",false,"",this.gameObject);
	// 		}
	// 	}
	// 	else
	// 	{
	// 		dialogBox.Show("Info", N["errdetail"].ToString(),false,"",this.gameObject);
	// 	}
	// }
	#endregion
}
