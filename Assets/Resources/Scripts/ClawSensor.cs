using UnityEngine;
using System.Collections;

public class ClawSensor : MonoBehaviour {
	public Pengait pengait;
	public GameObject currentCollider;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if ( collider.gameObject.layer == GameManager.LayerContent )
			currentCollider = collider.gameObject;
		else
			currentCollider = null;
		//	Debug.LogError ("OnTriggerEnter me="+this.transform.parent.name+" collider=" + collider.name);
		pengait.SendMessage ("SetListTrigger", this);
	}

	void OnTriggerStay(Collider collider)
	{
		if ( collider.gameObject.layer == GameManager.LayerContent )
			currentCollider = collider.gameObject;
		else
			currentCollider = null;
		//Debug.LogError ("OnTriggerStay me="+this.transform.parent.name+" collider=" + collider.name);
		pengait.SendMessage ("SetListTrigger", this);
	}
	
	void OnTriggerExit(Collider collider)
	{
		if ( collider.gameObject.layer == GameManager.LayerContent )
			currentCollider = collider.gameObject;
		else
			currentCollider = null;
		currentCollider = null;
		//Debug.LogError ("OnTriggerExit me="+this.transform.parent.name+" collider=" + collider.name);
		pengait.SendMessage ("SetListTrigger", this);
	}
}
