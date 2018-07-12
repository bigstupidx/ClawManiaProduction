#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class PreloadSigningAlias
{

	static PreloadSigningAlias ()
	{
		PlayerSettings.Android.keystorePass = "baskethoop@gemugemu";
		PlayerSettings.Android.keyaliasName = "baskethooprelease";
		PlayerSettings.Android.keyaliasPass = "baskethoop@gemugemu";
	}

}

#endif