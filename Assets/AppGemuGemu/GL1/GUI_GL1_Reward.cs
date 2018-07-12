using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GUI_GL1_Reward : GUI_Dialog 
{
	public GameObject prefabContent;
	public UIScrollView scrollView;
	public GUI_Gl1_Login guiLogin;
	public GUI_DialogBox guiDialogBox;
	public UILabel labelTicketAmount;
	// Use this for initialization
	
	public override void OnStart ()  {

	}
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnShow()
	{
		labelTicketAmount.text = GameManager.TICKET.ToString();
		//GL1Connector.GetInstance().GetReward (this.gameObject);
	}

	ButtonReward currReward;
	public void OnClickRedeem()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;

		if ( button.transform.parent.gameObject.GetComponent<ButtonReward>() )
			currReward = button.transform.parent.gameObject.GetComponent<ButtonReward>();

		if ( currReward == null )
		{

			return;
		}
		Gamestate gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate> ();
		if ( gs )
		{
			string url = "https://www.gemugemu.com/redeem.htm?token="+GL1Connector.GetInstance().GetToken()+"&email="+GL1Connector.GetInstance().GetCurrUser();
			Application.OpenURL(url);
			//gs.ShowDialogBox("Confirmation","Redeem "+currReward.tiket+" Ticket ?",true,"redeemreward",this.gameObject);
		}
	}

	public void OnConfirmRedeem()
	{
		if ( currReward == null )
			return;
		
		GL1Connector.GetInstance().Redeem (this.gameObject,currReward.kode, this.gameObject);
		currReward = null;
	}

	public void OnGL1Done(JSONNode N)
	{
		if (GL1Connector.GetInstance ().GetLastURL ().Contains ("reward")) {
			Debug.LogError ("ProcessRewardList amount=" + N.Count);
			int indexReward = 0;
			foreach (JSONNode nChild in N.Childs) {
				Transform targetChild = null;
				for (int i=0; i<scrollView.transform.childCount; i++) {
					Transform child = scrollView.transform.GetChild (i);
					//Debug.LogError("child.name="+child.name+" "+" kode="+nChild["kode"]);
					if (child && child.name.Equals (nChild ["kode"])) {
						targetChild = child;
						break;
					}
				}

				if (targetChild == null) {
					Debug.LogError ("instantiate");
					GameObject ob = (GameObject)Instantiate (prefabContent);
					ob.name = nChild ["kode"];
					targetChild = ob.transform;
					ob.transform.parent = scrollView.transform;
					ob.transform.localPosition = new Vector3 (0, 230 - (indexReward * 230), 0);
					ob.transform.localScale = Vector3.one;

					Transform trIcon = targetChild.Find ("Icon");
					if (trIcon) {
						UIButton buttonRedeem = trIcon.gameObject.GetComponent<UIButton> ();
						if (buttonRedeem) {
							buttonRedeem.onClick.Add (new EventDelegate (this, "OnClickRedeem"));
						}
						
						WWW_Texture wwwTexture = trIcon.gameObject.GetComponent<WWW_Texture> ();
						if (wwwTexture) {
							wwwTexture.StartDownloadImage (nChild ["gambar"]);
						}
					}

					indexReward ++;
				}

				if (targetChild) {
					ButtonReward reward = targetChild.GetComponent<ButtonReward> ();
					reward.nama = nChild ["nama"];
					reward.kode = nChild ["kode"];
					reward.des = nChild ["des"];
					reward.gambar = nChild ["gambar"];
					reward.tiket = int.Parse (nChild ["tiket"]);
					GameManager.SetNGUILabel (targetChild.Find ("Label Title"), nChild ["nama"]);
					GameManager.SetNGUILabel (targetChild.Find ("Label Desc"), nChild ["des"]);
					GameManager.SetNGUILabel (targetChild.Find ("Label Ticket Amount"), nChild ["tiket"]);




				}
			}
		}
		else if ( GL1Connector.GetInstance().GetLastURL().Contains("redeem") )
		{
			if (N["errcode"].ToString() == "\"0\"")
			{
				guiDialogBox.Show("Info", "Redeem success",false,"",this.gameObject);
			}
			else
			{
				guiDialogBox.Show("Info", N["errdetail"],false,"",this.gameObject);
			}
		}
	}
}
