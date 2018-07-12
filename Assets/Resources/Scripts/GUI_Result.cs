using UnityEngine;
using System.Collections;

public class ClawResultQueue
{
	public Texture icon;
	public string name;
}

[System.Serializable]
public class GUI_Result : GUI_Dialog	 
{
	public UITexture icon;
	public UILabel labelName;
	// Use this for initialization
	
	ArrayList queues;
	// Use this for initialization
	
	public override void OnStart () {
		queues = new ArrayList ();
	}

	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	void ProcessQueue()
	{
		Debug.LogError ("processqueue amount="+queues.Count);
		if ( queues != null && queues.Count > 0 )
		{
			ClawResultQueue queue = (ClawResultQueue)queues[0];
			queues.RemoveAt(0);
			icon.mainTexture = queue.icon;
			labelName.text = queue.name;
			//Debug.LogError("result="+queue.name);
			GUI_Dialog.InsertStack(this.gameObject);
			//Show ();
		}
	}

	public void Show(Texture texture,string sName,int fund)
	{
		ClawResultQueue queue = new ClawResultQueue ();
		queue.icon = texture;
		queue.name = "$"+fund.ToString();
		
		queues.Add (queue);
		if ( !isVisible() )
			ProcessQueue();
	}
	
	public override void OnTweenDone()
	{
		ProcessQueue ();
	}
}
