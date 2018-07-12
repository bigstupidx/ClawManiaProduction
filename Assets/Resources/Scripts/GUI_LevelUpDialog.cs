using UnityEngine;
using System.Collections;

//public class DialogBoxQueue
//{
//	public string sTitle;
//	public string sText;
//	public bool bShowCancel;
//	public string sExec;
//	public GameObject sender;
//}
public class GUI_LevelUpDialog : GUI_Dialog {
	
	public UILabel labelText;
	public UILabel labelTitle;

//	void ProcessQueue()
//	{
//		Debug.LogError("ProcessQueue count="+queues.Count);
//		if ( queues != null && queues.Count > 0 )
//		{
//			DialogBoxQueue queue = (DialogBoxQueue)queues[0];
//			queues.RemoveAt(0);
//			
//			labelText.text = queue.sText;
//			labelText.gameObject.SetActive (false);
//			labelText.gameObject.SetActive (true);
//			labelTitle.text = queue.sTitle;
//			//Debug.LogError("showcancel="+queue.bShowCancel);
//			
//			GUI_Dialog.InsertStack(this.gameObject);
//			//Show ();
//		}
//	}

	public void Show(string sTitle,string sText)
	{
//		DialogBoxQueue queue = new DialogBoxQueue ();
//		queue.sTitle = sTitle;
//		queue.sText = sText;
//		
//		queues.Add (queue);
//		if ( !isVisible() )
//			ProcessQueue();

		labelText.text = sText;
		labelText.gameObject.SetActive (false);
		labelText.gameObject.SetActive (true);
		labelTitle.text = sTitle;
		//Debug.LogError("showcancel="+queue.bShowCancel);
		
		GUI_Dialog.InsertStack(this.gameObject);
		//Show ();
	}
	public void OnClickOk(){
		GUI_Dialog.ReleaseTopCanvas ();
	}
}
