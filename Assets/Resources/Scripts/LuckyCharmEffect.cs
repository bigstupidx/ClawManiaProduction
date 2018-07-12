using UnityEngine;
using System.Collections;

public class LuckyCharmEffect : MonoBehaviour {
	
	public AudioClip audioCharmStart;
	// Use this for initialization
	void Start () {
	
	}
	public void SetActive(bool bVal)
	{
		if ( bVal )
		{
			PlayAudio(audioCharmStart,false);
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	
	public void PlayAudio(AudioClip clip,bool bLoop)
	{
		if ( GetComponent<AudioSource>() == null )
			return;
		
		if ( GetComponent<AudioSource>().clip == clip )
			return;
		
		if ( GameManager.bSoundOn == false )
			return;
		GetComponent<AudioSource>().loop = bLoop;
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().Play ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
