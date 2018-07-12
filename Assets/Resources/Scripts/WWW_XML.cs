using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class WWW_XML : MonoBehaviour {
	
	private WWW _xml_loading;
	private XmlDocument _xmlDoc;
	private bool bDownloading;
	private string sLastURL;
	// Use this for initialization
	void Start () {

	}

	void OnDestroy () {
		_xml_loading = null;
	}

	public void GetXml ( string url , string sFile , string parameter )
	{
		StopCoroutine("RetrievingData");
		sLastURL = sFile;

		if ( _xml_loading != null )
		{
			_xml_loading.Dispose();
			_xml_loading = null;
		}
		string sUrlTarget = url + sFile + "?" + parameter;
		//if (Application.platform == RuntimePlatform.IPhonePlayer)
		//	sUrlTarget = System.Uri.EscapeUriString (sUrlTarget);
		//GUI_Debug.Log("[WWW_XML] GetXml : '"+sUrlTarget+"'");
		_xml_loading = new WWW(sUrlTarget);
		if ( _xmlDoc != null )
		{
			_xmlDoc.RemoveAll();
			_xmlDoc = null;
		}
		bDownloading = true;
		
		StartCoroutine(RetrievingData());
	}

	public string GetLastURL()
	{
		return sLastURL;
	}

	public XmlDocument GetXmlDoc()
	{
		return _xmlDoc;
	}

	IEnumerator RetrievingData () 
	{
		bool bError = false;
		//GUI_Debug.Log("[WWW_XML] Retrieving Data : "+sLastURL);
		while ( bDownloading == true )
		{	
			if ( _xml_loading == null )
			{
				//GUI_Debug.Log("[WWW_XML] Retrieving Data : xml_loading is null");
				bDownloading = false;
			}
			else
			{
				if ( _xml_loading.isDone )
				{
					bDownloading = false;
					if ( _xml_loading.error == null )
					{
						_xmlDoc = new XmlDocument(); 
						//Debug.LogError(sLastURL);
						_xmlDoc.LoadXml(_xml_loading.text);


					}
					else
					{
						bError = true;
						//GUI_Debug.Log("[WWW_XML] RetrievingData : "+_xml_loading.url+" Fail->"+_xml_loading.error);	
					}
					
					_xml_loading.Dispose();
					_xml_loading = null;
				}
			}
			yield return null;
		}
		//GUI_Debug.Log("[WWW_XML] Retrieving Data : Done");
		if ( bError == false )
		{
			
			//GUI_Debug.Log("[WWW_XML] RetrievingData : "+sLastURL+" Done");	
			this.gameObject.SendMessage("OnWWWXMLDone");
		}
		yield break;
	}
}
