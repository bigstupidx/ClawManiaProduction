using System;

namespace Pokkt
{
	public interface IPerformer
	{
		void NotifyNative(string operation, string param);
		bool IsVideoAvailable();
		float GetVideoVC();
		string GetPokktSDKVersion();
	}
}

