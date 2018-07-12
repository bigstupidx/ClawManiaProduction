using UnityEngine;
using System.Collections;

public class GUI_PrizeCollection : GUI_Dialog 
{
	public UIScrollView panelContainer;
	public UISprite scrollView;
	public GameObject prefabContent;
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	// Use this for initialization
	public override void OnStart () 
	{
		Gamestate_Gameplay gs = (Gamestate_Gameplay)GameObject.FindGameObjectWithTag ("Gamestate").gameObject.GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			for ( int i=0; i<gs.categories.Length; i++ )
			{
				ContentCategory cat = gs.categories[i].GetComponent<ContentCategory>();

				if ( cat.ShowInUI )
				{
					GameObject obInst = (GameObject) Instantiate (prefabContent);
					obInst.name = "cc."+i;
					obInst.transform.parent = panelContainer.transform;
					obInst.transform.localScale = new Vector3(1,1,1);
					obInst.transform.localPosition = new Vector3(i*scrollView.localSize.x,120,0);

					GameManager.SetNGUILabel(obInst.transform.Find("Label Name"),cat.theName);
					GameManager.SetNGUILabel(obInst.transform.Find("Label Hidden ID"),i.ToString());

					Transform trIconContainer = obInst.transform.Find("Sprite");
					for ( int j=0; j<cat.contents.Length; j++ )
					{
						Transform trItem = trIconContainer.transform.Find("Item"+(j+1));
						if ( trItem )
						{
							Transform trIcon = trItem.transform.Find("Icon");
							UITexture texture = trIcon.GetComponent<UITexture>();
							texture.mainTexture = cat.contents[j].icon;

							int iAmount = 0;
							if ( PlayerPrefs.HasKey("cc."+i+"."+j) )
								iAmount = PlayerPrefs.GetInt("cc."+i+"."+j);

							//PlayerPrefs.SetInt("cc."+i+"."+j,1);
							GameManager.SetNGUILabel(trItem.transform.Find("Label Amount"),iAmount.ToString());

						}
					}
					ActivateColliders(obInst,false);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnShow()
	{
		
		Gamestate_Gameplay gs = (Gamestate_Gameplay)GameObject.FindGameObjectWithTag ("Gamestate").gameObject.GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			for ( int i=0; i<gs.categories.Length; i++ )
			{
				Transform trCategory = panelContainer.transform.Find("cc."+i);
				if ( trCategory )
				{
					ContentCategory cat = (ContentCategory)gs.categories[i];

					Transform trIconContainer = trCategory.Find("Sprite");
					for ( int j=0; j<cat.contents.Length; j++ )
					{
						Transform trItem = trIconContainer.transform.Find("Item"+(j+1));
						if ( trItem )
						{
							int iAmount = 0;
							if ( PlayerPrefs.HasKey("cc."+i+"."+j) )
								iAmount = PlayerPrefs.GetInt("cc."+i+"."+j);
							GameManager.SetNGUILabel(trItem.transform.Find("Label Amount"),iAmount.ToString());
						}
					}

					GameManager.SetNGUILabel(trCategory.Find("Label Reward"),cat.bonus+" Energy "+cat.bonusEXP+" exp");
					
					ActivateColliders(trCategory.gameObject,true);
				}

			}
		}
	}
}
