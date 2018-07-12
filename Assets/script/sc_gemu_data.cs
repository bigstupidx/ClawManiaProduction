using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_gemu_data : MonoBehaviour {
	public InputField inputUsername;
	public InputField inputEmail;
	public InputField inputCoin;
	public InputField inputTicket;
	// Use this for initialization
	void Start () 
	{
		GemuAPI.OnGetUserResponse += OnGetUserResponse;
	}

	void OnDestroy () 
	{
		GemuAPI.OnGetUserResponse -= OnGetUserResponse;
	}

	public void OnClose()
	{
		SoundManager.instance.PlayButton();
		this.gameObject.SetActive (false);
	}

	void OnGetUserResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			Hashtable userdata = (Hashtable)data["userdata"];

			inputUsername.text = PlayerPrefs.GetString(PlayerPrefHandler.keyUserName);
			inputEmail.text = userdata["email"].ToString();
			inputCoin.text = userdata["coin"].ToString();
			inputTicket.text = userdata["tiket"].ToString();
		}
		else
		{
			PlayerPrefs.SetString(PlayerPrefHandler.keyUserName,"");
			PlayerPrefs.SetString(PlayerPrefHandler.keyToken,"");
		}
	}

	public void btnPromo_show()
	{

	}

	public void OnClickGetPromo()
	{
		SoundManager.instance.PlayButton();
		sc_mainmenu_canvas_handler.Instance().cvGemuPromo.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
	}

	public void OnClickLogout()
	{
		SoundManager.instance.PlayButton();
		PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, "");
		PlayerPrefs.SetString (PlayerPrefHandler.keyToken, "");
		this.gameObject.SetActive (false);
	}

	void Awake()
	{
		inputUsername.text = "";
		inputEmail.text = "";
		inputCoin.text = "";
		inputTicket.text = "";

		//GameDataManager.instance.LoadData ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
