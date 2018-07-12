using UnityEngine;
using System.Collections;

public class ButtonReward : MonoBehaviour 
{
	public string kode = "";
	public string nama = "";
	public string des = "";
	public string gambar = "";
	public int tiket = 0;
	public GameObject progress;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDownloadingPicDone()
	{
		progress.gameObject.SetActive (false);
	}
}
