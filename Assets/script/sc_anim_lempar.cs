using UnityEngine;
using System.Collections;

public class sc_anim_lempar : MonoBehaviour {
	protected Animator animator;

	// Use this for initialization
	void Start () {
		//this.animation.Play ("lempar");
		animator = GetComponent<Animator>();


	}

	public void panggil_animasi_tangan() {
		//Debug.Log("Animasi Tangan terpanggil !!!");
		animator.SetBool("stat_anim",true);

		//Debug.Log()

	}

	// Update is called once per frame
	void Update () {
		if(this.animator.GetCurrentAnimatorStateInfo (0).IsName("lempar")) {
			animator.SetBool("stat_anim",false);
		}
	}
}
