using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Pokkt
{
	internal static class PokktNativeExtension
	{

		public static IPerformer GetPerformer()
		{
			if (Application.platform == RuntimePlatform.Android) {
				return new AndroidPerformer ();
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return new IOSPerformer ();
			} else {
				Debug.LogWarning ("[UNITY] Performer not implemented for platform: " + Application.platform.ToString ());
				return null;
			}
		}

		public static void PerformOperation(PokktOperations operation, string param = "")
		{
			switch (operation)
			{
				// common methods
				case PokktOperations.SetParams:                 NotifyNative("setParams",               param);   break;
				case PokktOperations.SetDebug:                  NotifyNative("setDebug",                param);   break;
				case PokktOperations.ShowLog:                   NotifyNative("showLog",                 param);   break;
				case PokktOperations.ShowToast:                 NotifyNative("showToast",               param);   break;
				case PokktOperations.SetThirdPartyUserId:       NotifyNative("setThirdPartyUserId",     param);   break;

				// session methods
				case PokktOperations.StartSession:       		NotifyNative("startSession",     		param);   break;
				case PokktOperations.EndSession:       			NotifyNative("endSession",     			param);   break;
					
				// offerwall methods
				case PokktOperations.SetCloseOnSuccessFlag:     NotifyNative("setCloseOnSuccessFlag",   param);   break;
				case PokktOperations.GetCoins:              	NotifyNative("getCoins",            	param);   break;
				case PokktOperations.GetPendingCoins:           NotifyNative("getPendingCoins",     	param);   break;
				case PokktOperations.CheckOfferWallCampaign:    NotifyNative("checkOfferWallCampaign",  param);   break;

				// video methods
				case PokktOperations.GetVideo:                  NotifyNative("getVideo",            	param);   break;
				case PokktOperations.GetVideoNonIncent:    		NotifyNative("getVideoNonIncent",   	param);   break;					
				case PokktOperations.CacheVideoCampaign:        NotifyNative("cacheVideoCampaign",      param);   break;
				case PokktOperations.SetSkipEnabled:          	NotifyNative("setSkipEnabled",        	param);   break;
				case PokktOperations.SetDefaultSkipTime:    	NotifyNative("setDefaultSkipTime",      param);   break;
				case PokktOperations.SetCustomSkipMessage:    	NotifyNative("setCustomSkipMessage",    param);   break;
				case PokktOperations.SetBackButtonDisabled:    	NotifyNative("setBackButtonDisabled",   param);   break;

				// optional methods
				case PokktOperations.SetName:                   NotifyNative("setName",                 param);   break;
				case PokktOperations.SetAge:                    NotifyNative("setAge",              	param);   break;
				case PokktOperations.SetSex:                    NotifyNative("setSex",              	param);   break;
				case PokktOperations.SetMobileNo:               NotifyNative("setMobileNo",             param);   break;
				case PokktOperations.SetEmailAddress:           NotifyNative("setEmailAddress",         param);   break;
				case PokktOperations.SetLocation:               NotifyNative("setLocation",             param);   break;
				case PokktOperations.SetBirthday:               NotifyNative("setBirthday",             param);   break;
				case PokktOperations.SetMaritalStatus:          NotifyNative("setMaritalStatus",        param);   break;
				case PokktOperations.SetFacebookId:             NotifyNative("setFacebookId",           param);   break;
				case PokktOperations.SetTwitterHandle:          NotifyNative("setTwitterHandle",        param);   break;
				case PokktOperations.SetEducation:          	NotifyNative("setEducation",    		param);   break;
				case PokktOperations.SetNationality:            NotifyNative("setNationality",          param);   break;
				case PokktOperations.SetEmployment:       		NotifyNative("setEmployment",     		param);   break;
				case PokktOperations.SetMaturityRating:         NotifyNative("setMaturityRating",       param);   break;
			}
		}

		private static void NotifyNative(string operation, string param)
		{
			Debug.Log("[UNITY] Performing operation: " + operation + " with params: " + param);
			IPerformer _performer = GetPerformer();
			if (_performer != null)
				_performer.NotifyNative(operation, param);
		}

		public static bool IsVideoAvailable()
		{
			Debug.Log("[UNITY] Checking video availability...");
			IPerformer _performer = GetPerformer();
			if (_performer != null)
				return _performer.IsVideoAvailable();

			return false;
		}
		
		public static float GetVideoVC()
		{
			Debug.Log("[UNITY] Getting video vc ...");
			IPerformer _performer = GetPerformer();
			if (_performer != null)
				return _performer.GetVideoVC();

			return -1;
		}
		
		public static string GetPokktSDKVersion()
		{
			Debug.Log("[UNITY] Getting Pokkt SDK version ...");
			IPerformer _performer = GetPerformer();
			if (_performer != null)
				return _performer.GetPokktSDKVersion();

			return "";
		}
	}
}