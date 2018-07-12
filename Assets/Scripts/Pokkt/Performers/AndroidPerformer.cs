using System;
using UnityEngine;

namespace Pokkt
{
	public class AndroidPerformer : IPerformer
	{
#if UNITY_ANDROID
		private static AndroidJavaClass _jc = new AndroidJavaClass("com.pokkt.unity.PokktNativeExtension");

		private static void NotifyAndroid(string operation, string param)
		{
			_jc.CallStatic("NotifyAndroid", operation, param);
		}
		
		private static bool IsVideoAvailableOnAndroid()
		{
			return _jc.CallStatic<bool>("isVideoAvailableOnAndroid");
		}
		
		private static float GetVideoVCOnAndroid()
		{
			return _jc.CallStatic<float>("getVideoVcOnAndroid");
		}
		
		private static string GetPokktSDKVersionOnAndroid()
		{
			return _jc.CallStatic<string>("getPokktSDKVersionOnAndroid");
		}
#endif
		
		public void NotifyNative(string operation, string param)
		{
#if UNITY_ANDROID
			NotifyAndroid(operation, param);
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}

		public bool IsVideoAvailable()
		{
#if UNITY_ANDROID
			return IsVideoAvailableOnAndroid();
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}

		public float GetVideoVC()
		{
#if UNITY_ANDROID
			return GetVideoVCOnAndroid();
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}

		public string GetPokktSDKVersion()
		{
#if UNITY_ANDROID
			return GetPokktSDKVersionOnAndroid();
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}
	}
}