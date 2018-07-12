using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GUI_Gl1_Login : GUI_Dialog {
	public UIInput labelUsername;
	public UIInput labelPassword;
	public GUI_DialogBox dialogBox;
	public GUI_GL1_UserData guiUserData;
	public GUI_Gl1_SignUp guiSignUp;
	public GUI_Share guiShare;	
	// Use this for initialization
	public override void OnStart () 
	{
		if ( string.IsNullOrEmpty(GL1Connector.GetInstance().GetToken()) == false && string.IsNullOrEmpty(GL1Connector.GetInstance().GetCurrUser()) == false )
		{
			GL1Connector.GetInstance().PlayGame(this.gameObject);
		}
	}
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void OnShow()
	{
		labelUsername.value = "";
		labelPassword.value = "";
	}

	public void OnClickOK()
	{
		if ( string.IsNullOrEmpty(labelUsername.value) )
		{
			dialogBox.Show("Info", "Please type your username",false,"",this.gameObject);
		}
		else if ( string.IsNullOrEmpty(labelPassword.value) )
		{
			dialogBox.Show("Info", "Please type your password",false,"",this.gameObject);
		}
		else
		{
			GL1Connector.GetInstance().Login(this.gameObject,labelUsername.value,labelPassword.value,"");
		}
	}
	
	public void OnClickSignUp()
	{
		GUI_Dialog.ReleaseTopCanvas();
		//guiSignUp.Show ();
		
		GUI_Dialog.InsertStack(guiSignUp.gameObject);
	}

	IEnumerator WaitingToShowUserData()
	{
		yield return new WaitForSeconds (0.5f);
		//guiUserData.Show();
		
		GUI_Dialog.InsertStack(guiUserData.gameObject);
	}

	public void OnGL1Done(JSONNode N)
	{
		if ( GL1Connector.GetInstance().GetLastURL().Contains("login") )
		{
			if (N["errcode"].ToString() == "\"0\"")
			{
				GUI_Dialog.ReleaseTopCanvas();
				guiShare.OnGemuGemuLoggedIn();
				StartCoroutine(WaitingToShowUserData());

			}
			else if (N["errcode"].ToString() == "\"1\"")
			{
				dialogBox.Show("Info", N["errdetail"],false,"",this.gameObject);
			}
			else
			{
				dialogBox.Show("Info", "Unable to connect",false,"",this.gameObject);
			}
		}
		else if ( GL1Connector.GetInstance().GetLastURL().Contains("getuser") )
		{
			if ( N["errcode"].ToString() == "\"0\"" )
			{
				var userdata = JSONNode.Parse(N["userdata"].ToString());
				if ( userdata != null )
				{

					/*
					foreach ( string key in ((JSONClass)userdata).GetKeys() )
					{
						//Debug.LogError(key);

						Transform trField = contentContainer.transform.Find("Field_"+key);
						if ( trField )
						{
							UIInput input = trField.GetComponent<UIInput>();
							if ( input )
							{
								input.value = userdata[key];
							}
						}
					}
						*/
				}
			}
		}
		else if ( GL1Connector.GetInstance().GetLastURL().Contains("playgame") )
		{
			if (N["errcode"].ToString() == "\"1\"")
			{
				// wrong token / username not found
			}
		}
			


	}
}
