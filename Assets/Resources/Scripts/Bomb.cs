using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	public AudioClip audioBlast;
	public AudioClip audioStart;
	// Use this for initialization
	void Start () {

		PlayAudio (audioStart, true);
		StartCoroutine (WaitForExplode ());
	}

	void PlayAudio(AudioClip clip,bool bLoop)
	{
		if ( clip == null )
			return;

		if ( this.GetComponent<AudioSource>() == null )
			return;

		if ( GameManager.bSoundOn == false )
			return;
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().loop = bLoop;
		GetComponent<AudioSource>().Play ();
	}

	IEnumerator WaitForExplode()
	{
		yield return new WaitForSeconds(5);
		Collider[] 	colliders = Physics.OverlapSphere(this.transform.position,2);
		//Debug.LogError("explostion amount="+colliders.Length);
		foreach ( Collider coll in colliders )
		{
			if ( coll.gameObject.layer == GameManager.LayerContent )
			{
				//Debug.LogError(coll.transform.parent.gameObject.name);
				coll.GetComponent<Rigidbody>().AddExplosionForce(800.0f,this.transform.position,40.0f,3.0f);
			}
		}
		PlayAudio (audioBlast,false);
		this.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(3);
		GameObject.Destroy (this.gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
