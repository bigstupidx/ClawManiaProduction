using UnityEngine;
using System.Collections;

public class GUI_MissionCompleted : GUI_Dialog {
	public UILabel missionName;
	public UILabel coinRewardValue;
	public UILabel expRewardValue;
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}
}
