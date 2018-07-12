using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemuAPI_EventHandler : MonoBehaviour {

	void Awake()
	{
		GemuAPI.OnRegisterResponse += OnRegisterResponse;
		GemuAPI.OnLoginResponse += OnLoginResponse;
		GemuAPI.OnGetUserResponse += OnGetUserResponse;
		GemuAPI.OnPlayResponse += OnPlayResponse;
		GemuAPI.OnPlayResultResponse += OnPlayResultResponse;
		GemuAPI.OnRewardResponse += OnRewardResponse;
		GemuAPI.OnLeaderboardResponse += OnLeaderboardResponse;
		GemuAPI.OnRedeemResponse += OnRedeemResponse;
		GemuAPI.OnPromoResponse += OnPromoResponse;
	}

	void OnDestroy()
	{
		GemuAPI.OnRegisterResponse -= OnRegisterResponse;
		GemuAPI.OnLoginResponse -= OnLoginResponse;
		GemuAPI.OnGetUserResponse -= OnGetUserResponse;
		GemuAPI.OnPlayResponse -= OnPlayResponse;
		GemuAPI.OnPlayResultResponse -= OnPlayResultResponse;
		GemuAPI.OnRewardResponse -= OnRewardResponse;
		GemuAPI.OnLeaderboardResponse -= OnLeaderboardResponse;
		GemuAPI.OnRedeemResponse -= OnRedeemResponse;
		GemuAPI.OnPromoResponse -= OnPromoResponse;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnRegisterResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnLoginResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnGetUserResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnPlayResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnPlayResultResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnRewardResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnLeaderboardResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnRedeemResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}
	void OnPromoResponse(Restifizer.RestifizerResponse response)
	{
		LogResponse(response.Resource);
	}

	void LogResponse(Hashtable hashTable)
	{
		Debug.Log( "{ \n ");

		foreach(DictionaryEntry entry in hashTable)
		{
			Debug.Log( "    " + entry.Key + " : " + entry.Value + "\n" );
		}

		Debug.Log( "}" );

	}
}
