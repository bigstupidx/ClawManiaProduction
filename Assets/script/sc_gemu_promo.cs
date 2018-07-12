using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sc_gemu_promo : MonoBehaviour {
	public InputField inputPromo;
	public Text labelResult;
	// Use this for initialization
	void Start () {
		GemuAPI.OnPromoResponse += OnPromoResponse;
	}

	void OnDestroy () {
		GemuAPI.OnPromoResponse -= OnPromoResponse;
	}

	void OnPromoResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			//GameDataManager.instance.LoadData();
		}
		else
		{
			labelResult.text = data["errdetail"].ToString();
		}
	}

	void Awake()
	{
		labelResult.text = "";
		inputPromo.text = "";
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClose()
	{
		this.gameObject.SetActive (false);
	}

	public void OnClickGetPromo()
	{
		Hashtable data = new Hashtable();
		data.Add("username", PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
		data.Add("token", PlayerPrefs.GetString(PlayerPrefHandler.keyToken));
		data.Add("kdkupon", inputPromo.text);
		
		try
		{
			GemuAPI.Promo(data);
		}
		catch(GemuAPI_Exception exc)
		{
			Debug.LogError(exc.Message);
		}
	}
}
