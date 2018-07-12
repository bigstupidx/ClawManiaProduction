using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public AudioClip audioLaserStart;
	public AudioClip audioLaserLoop;
	// Use this for initialization
	void Start () {
	
	}

	public void SetActive(bool bVal)
	{
		if ( bVal )
		{
			PlayAudio(audioLaserStart,false);
			StartCoroutine(WaitingForAudio());
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	IEnumerator WaitingForAudio()
	{
		while ( GetComponent<AudioSource>().isPlaying )
			yield return null;
		
		PlayAudio(audioLaserLoop,true);
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
