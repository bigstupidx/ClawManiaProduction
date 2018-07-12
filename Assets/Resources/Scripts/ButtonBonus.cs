using UnityEngine;
using System.Collections;

public class ButtonBonus : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnClick()
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			Transform trHiddenID = this.transform.parent.Find("Label Hidden ID");
			if ( trHiddenID)
			{
				UILabel label = trHiddenID.gameObject.GetComponent<UILabel>();
				if ( label )
				{
					int index = int.Parse(label.text);

					bool bContinue = true;
					ContentCategory cat = (ContentCategory)gs.categories[index];
					for ( int j=0; j<cat.contents.Length; j++ )
					{
						int iPrizeAmount = 0;
						if ( PlayerPrefs.HasKey("cc."+index+"."+j) )
						{
							iPrizeAmount = PlayerPrefs.GetInt("cc."+index+"."+j);
						}

						if ( iPrizeAmount == 0 )
						{
							bContinue = false;
							break;
						}
					}

					if ( bContinue )
						gs.OnClickBonusButton(index);
					else
						gs.ShowDialogBox("Info","Not enough prize",false,"",this.gameObject);
				}
			}
		}
	}
}
