using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuPromoCode : GUI_Dialog {
	public UIInput inputPromo;
	public GemuDialogBoxController dialogBox;
	// Use this for initialization
	public override void OnStart () {

		GemuAPI.OnPromoResponse += OnPromoResponse;
	}
	
	void OnDestroy()
	{
		GemuAPI.OnPromoResponse -= OnPromoResponse;
	}

	void OnPromoResponse(Restifizer.RestifizerResponse response)
	{
		Debug.LogError ("done");
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "\"0\"")
		{
			dialogBox.Show("Info","Promo Code has been processed",false,"",this.gameObject);
			//GameDataManager.instance.LoadData();
			OnClickClose();
		}
		else
		{
			dialogBox.Show("Error",data["errdetail"].ToString(),false,"",this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickGetPromo()
	{
		Debug.LogError ("test");
		SoundManager.instance.PlayButton();
		
		Hashtable data = new Hashtable();
		data.Add("username", PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
		data.Add("token", PlayerPrefs.GetString(PlayerPrefHandler.keyToken));
		data.Add("kdkupon", inputPromo.value);
		
		try
		{
			GemuAPI.Promo(data);
		}
		catch(GemuAPI_Exception exc)
		{
			Debug.LogError(exc.Message);
		}
	}

	public void OnClickClose()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas();
	}
}
