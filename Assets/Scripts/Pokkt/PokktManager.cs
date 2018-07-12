using UnityEngine;
using System.Collections;
using System;

namespace Pokkt
{
	public class PokktManager
	{
		// dispatcher name
		private const string DispatcherName = "pokktDispatcher";


		// internal state
		private static GameObject _pokktDispatcher;

		public static void initPokkt(PokktConfig pokktConfig)
		{
			CheckAndInitDispatcher();

			SetParams(pokktConfig);
		}

		private static void CheckAndInitDispatcher()
		{
			// check for any existing dispatcher go
			_pokktDispatcher = GameObject.Find(DispatcherName);

			// if it does not exists, create one
			if (_pokktDispatcher == null)
			{
				// create a game object to listen to messages from java
				_pokktDispatcher = new GameObject(DispatcherName);

				// retain this object
				GameObject.DontDestroyOnLoad(_pokktDispatcher);
			}

			// attach the dispatcher sript component to it, if not available already
			if (_pokktDispatcher.GetComponent<PokktDispatcher>() == null)
				_pokktDispatcher.AddComponent<PokktDispatcher>();
		}

		public static PokktDispatcher Dispathcer
		{
			get
			{
				// keep checking for dispatcher
				CheckAndInitDispatcher();
				
				return _pokktDispatcher.GetComponent<PokktDispatcher>();
			}
		}


		// COMMON METHODS

		private static void SetParams(PokktConfig pokktConfig)
		{
			string paramString = pokktConfig.getSecurityKey() + "," + pokktConfig.getApplicationId() 
				+ "," + (int)pokktConfig.getIntegrationType() + "," + (pokktConfig.isAutoCacheVideo() ? "true" : "false");
			PokktNativeExtension.PerformOperation(PokktOperations.SetParams, paramString);
		}

		public static void SetDebug(bool value)
		{
			PokktNativeExtension.PerformOperation(PokktOperations.SetDebug, (value ? "true" : "false"));
		}

		public static void ShowLog(string message)
		{
			PokktNativeExtension.PerformOperation(PokktOperations.ShowLog, message);
		}

		public static void ShowToast(string message)
		{
			PokktNativeExtension.PerformOperation(PokktOperations.ShowToast, message);
		}


		// SESSION METHODS

		public static void StartSession(PokktConfig pokktConfig)
		{
			SetParams(pokktConfig);
			PokktNativeExtension.PerformOperation(PokktOperations.StartSession);
		}

		public static void EndSession()
		{
			PokktNativeExtension.PerformOperation(PokktOperations.EndSession);
		}


		// OFFERWALL METHODS

		public static void GetCoins(PokktConfig pokktConfig)
		{
			PokktNativeExtension.PerformOperation(PokktOperations.GetCoins, pokktConfig.getOfferWallAssetValue());
		}

		public static void GetPendingCoins()
		{
			PokktNativeExtension.PerformOperation(PokktOperations.GetPendingCoins);
		}

		public static void CheckOfferWallCampaign()
		{
			PokktNativeExtension.PerformOperation(PokktOperations.CheckOfferWallCampaign);
		}


		// VIDEO METHODS

		public static void GetVideo(PokktConfig pokktConfig)
		{
			PokktNativeExtension.PerformOperation(PokktOperations.GetVideo, pokktConfig.getScreenName());
		}

		public static void GetVideoNonIncent(PokktConfig pokktConfig)
		{
			PokktNativeExtension.PerformOperation(PokktOperations.GetVideoNonIncent, pokktConfig.getScreenName());
		}

		public static void CacheVideoCampaign()
		{
			PokktNativeExtension.PerformOperation(PokktOperations.CacheVideoCampaign);
		}

		public static bool IsVideoAvailable()
		{
			return PokktNativeExtension.IsVideoAvailable();
		}
		
		public static float GetVideoVc()
		{
			return PokktNativeExtension.GetVideoVC();
		}

		// OPTIONAL METHODS

		public static string GetPokktSDKVersion()
		{
			return PokktNativeExtension.GetPokktSDKVersion();
		}
	}
}