using UnityEngine;
using System.Collections;

public class Content : MonoBehaviour 
{
	public string theName;
	public int exp;
	public Texture icon;
    public int fundReward = 10;
	Collider collider;
	// Use this for initialization
	void Start () {
		collider = this.GetComponentInChildren<Collider> ();
		StartCoroutine (CheckFalling ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator CheckFalling()
	{
		if ( collider == null )
			yield break;
		bool bChecking = true;
		while ( bChecking )
		{
			if ( collider )
			{
				
				if ( collider.transform.position.y < 5 )
				{
					//Debug.LogError("falling");
					bChecking = false;
				}
			}
			else
				bChecking = false;
			yield return new WaitForSeconds(1);
		}

		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").gameObject.GetComponent<Gamestate_Gameplay>();
		if ( gs )
		{
			//gs.OnDropSuccess(this.gameObject);
		}
		GameObject.Destroy (collider.gameObject);
	}

}
