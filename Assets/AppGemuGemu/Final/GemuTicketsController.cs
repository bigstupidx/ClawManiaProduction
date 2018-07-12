using UnityEngine;
using System.Collections;

public class GemuTicketsController : GUI_Dialog {

	public UILabel textTix;
	public GemuDialogBoxController dialogBox;
	//public GemuRedeemController redeemController;
	//public GemuRewardController rewardHandler;
	public GemuRewardVerifyController rewardVerifyHandler;
	public UIScrollView scrollView;
	public UISprite scrollGuide;
	public GameObject prefabContent;
	public int HorizontalAmount;
	ButtonReward currReward;
	
	public GemuSpecialReward uiSpecialReward;
	void Awake()
	{
		//Hashtable data = new Hashtable();
		//data.Add("username", PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
		//data.Add("token", PlayerPrefs.GetString(PlayerPrefHandler.keyToken));

	}

	public override void OnShow()
	{
		
		scrollView.transform.gameObject.SetActive (false);
		scrollView.transform.gameObject.SetActive (true);
		scrollView.ResetPosition ();

		textTix.text = PlayerPrefs.GetInt(PlayerPrefHandler.keyUserTiket, 0).ToString();
		for ( int i=0; i<scrollView.transform.childCount; i++ )
		{
			ButtonReward button = (ButtonReward)scrollView.transform.GetChild(i).gameObject.GetComponent<ButtonReward>();
			if ( button )
			{
				Transform trIcon = button.transform.Find("Icon");
				if ( trIcon )
				{
					
					WWW_Texture wwwTexture = trIcon.gameObject.GetComponent<WWW_Texture>();
					wwwTexture.sender = button.gameObject;
					if ( wwwTexture )
					{
						//if ( !wwwTexture.IsDone() )
						//{
							wwwTexture.StartDownloadImage(button.gambar);
						//}
					}
				}
			}
		}

	}

	void OnDestroy()
	{
		
		GemuAPI.OnGetUserResponse -= OnGetUserResponse;
		GemuAPI.OnRewardResponse -= OnRewardResponse;
	}

	// Use this for initialization
	public override void OnStart () 
	{
		GemuAPI.OnRewardResponse += OnRewardResponse;
		GemuAPI.OnGetUserResponse += OnGetUserResponse;
		GemuAPI.Reward ();
	}
	
	void OnRewardResponse(Restifizer.RestifizerResponse response)
	{
		ArrayList data = response.ResourceList;
		for ( int i=0; i<response.ResourceList.Count; i++ )
		{
			
			Hashtable dataReward = (Hashtable)response.ResourceList[i];
			Transform targetChild = null;
			if ( scrollView )
				for (int j=0; j<scrollView.transform.childCount; j++ )
				{
					Transform child = scrollView.transform.GetChild(j);
					//Debug.LogError("child.name="+child.name+" "+" kode="+nChild["kode"]);
					if ( child && child.name.Equals( dataReward["kode"] ))
					{
						targetChild = child;
						break;
					}
				}

			if ( targetChild == null )
			{
				GameObject ob = Instantiate(prefabContent);
				
				ob.name = dataReward["kode"].ToString();
				targetChild = ob.transform;
				ob.gameObject.SetActive(true);
				ob.transform.parent = scrollView.transform;
				ob.transform.localScale = Vector3.one;
				ob.gameObject.SetActive(false);

				ButtonReward reward = targetChild.GetComponent<ButtonReward>();
				reward.nama = dataReward["nama"].ToString();
				reward.kode = dataReward["kode"].ToString();
				reward.des = dataReward["des"].ToString();
				reward.gambar = dataReward["gambar"].ToString();
				reward.tiket = int.Parse(dataReward["tiket"].ToString());
				targetChild.gameObject.SetActive(false);
				targetChild.gameObject.SetActive(true);

				UISprite sprite = ob.GetComponent<UISprite>();
				if ( HorizontalAmount == 1 )
				{
					ob.transform.localPosition = new Vector3(-10,100-((scrollView.transform.childCount-1)*sprite.height),0);
				}
				else if ( HorizontalAmount == 2 )
				{
					if ( i%HorizontalAmount == 0 )
						ob.transform.localPosition = new Vector3(-310,80-(int)(i/2)*400,0);
					else if ( i%HorizontalAmount == 1 )
						ob.transform.localPosition = new Vector3(310,80-(int)(i/2)*400,0);
				}

				UIButton buttonRedeem = ob.GetComponent<UIButton>();
				if ( buttonRedeem )
				{
					buttonRedeem.onClick.Add(new EventDelegate(this,"OnClickRedeem"));
				}

				Transform trIcon = ob.transform.Find("Icon");
				if ( trIcon )
				{
					WWW_Texture wwwTexture = trIcon.gameObject.GetComponent<WWW_Texture>();
					wwwTexture.sender = ob;
					if ( wwwTexture )
					{
						wwwTexture.StartDownloadImage(dataReward["gambar"].ToString());
					}
				}
			}
			if ( targetChild )
			{

			}
		}
	}
/*
	public void OnRedeem()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;
		
		if ( button.transform.parent.gameObject.GetComponent<ButtonReward>() )
			currReward = button.transform.parent.gameObject.GetComponent<ButtonReward>();
		
		if ( currReward == null )
			return;

		Gamestate gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate> ();
		if ( gs )
		{
			gs.ShowDialogBox("Confirmation","Redeem "+currReward.tiket+" Ticket ?",false,"redeemreward",this.gameObject);
		}
	}
*/
	void OnGetUserResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		//Debug.LogError ("success");
		if ( data["errcode"].ToString() == "0")
		{
			Hashtable userdata = (Hashtable)data["userdata"];

			int iTiket = (int) float.Parse(userdata["tiket"].ToString());
			textTix.text = iTiket.ToString();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{

	}

	public void OnBackButton()
	{
		SoundManager.instance.PlayButton();

		GUI_Dialog.ReleaseTopCanvas();
		//Hide ();
	}

	/*
	public void OnRewardButton(int type) //0-4
	{
		rewardVerifyHandler.SetData(rewardHandler.rewardList[type]);
		rewardVerifyHandler.gameObject.SetActive(true);
		rewardVerifyHandler.gameObject.SetActive(true);
		GemuMainMenuController.instance.InsertStack(rewardVerifyHandler.gameObject);
		rewardVerifyHandler.Show ();
	}
*/
	public void OnClickRedeem()
	{
		if ( PlayerPrefs.GetString(PlayerPrefHandler.keyUserName) == "" )
		{
			dialogBox.Show("Info","Please login.",false,"",this.gameObject);
			return;
		}

		SoundManager.instance.PlayButton();
		UIButton button = UIButton.current;
		if ( button == null )
			return;

		if ( button.gameObject.GetComponent<ButtonReward>() )
			currReward = button.gameObject.GetComponent<ButtonReward>();

		if ( currReward == null )
			return;

		if (uiSpecialReward != null) {
			if (currReward.kode != "80007") {
				rewardVerifyHandler.textTickets.text = currReward.tiket.ToString ();
				rewardVerifyHandler.sKodeReward = currReward.kode;
				GUI_Dialog.InsertStack (rewardVerifyHandler.gameObject);
			} else {
				GUI_Dialog.InsertStack (uiSpecialReward.gameObject);
			}
		} else {
			rewardVerifyHandler.textTickets.text = currReward.tiket.ToString ();
			rewardVerifyHandler.sKodeReward = currReward.kode;
			GUI_Dialog.InsertStack (rewardVerifyHandler.gameObject);
		}
	}

}
