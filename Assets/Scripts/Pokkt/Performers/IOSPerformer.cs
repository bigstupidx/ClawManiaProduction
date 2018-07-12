using System;
using System.Runtime.InteropServices;

namespace Pokkt
{
	public class IOSPerformer : IPerformer
	{
#if UNITY_IOS
		// internal calls
		[DllImport("__Internal")]
		private static extern void NotifyIOS(string operation, string param);
		
		[DllImport("__Internal")]
		private static extern bool IsVideoAvailableOnIOS();
		
		[DllImport("__Internal")]
		private static extern float GetVideoVCOnIOS();
		
		[DllImport("__Internal")]
		private static extern string GetPokktSDKVersionOnIOS();
#endif

		public void NotifyNative(string operation, string param)
		{
#if UNITY_IOS
			NotifyIOS(operation, param);
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}
		
		public bool IsVideoAvailable()
		{
#if UNITY_IOS
			return IsVideoAvailableOnIOS();
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}
		
		public float GetVideoVC()
		{
#if UNITY_IOS
			return GetVideoVCOnIOS();
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}
		
		public string GetPokktSDKVersion()
		{
#if UNITY_IOS
			return GetPokktSDKVersionOnIOS();
#else
			throw new InvalidOperationException("Method not implemented!");
#endif
		}
	}
}