using UnityEngine;
using System.Collections;

public class MascotController : MonoBehaviour {

	public GemuMascotSound mascotSound;
	public Texture[] idleAnimations;
	public Texture[] tapAnimations;
	UITexture texture;
	float index = 0;
	eMascotState state = eMascotState.IDLE;
	public enum eMascotState
	{
		IDLE,
		TAP
	}

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		texture = GetComponent<UITexture> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (state == eMascotState.IDLE) {
			index += Time.deltaTime * 10;
			if ((int)index >= idleAnimations.Length) {
				index = 0;
			}
			texture.mainTexture = idleAnimations [(int)index];
		}
		else if (state == eMascotState.TAP) {
			index += Time.deltaTime * 10;
			if ((int)index >= tapAnimations.Length -1) {
				state = eMascotState.IDLE;
			}
			texture.mainTexture = tapAnimations [(int)index];
		}
	}

	public void OnTap()
	{
		//animator.StopPlayback();
		//animator.SetInteger("State", (int)eMascotState.TAP);
		//animator.Play("Tap");
		if (state == eMascotState.IDLE) {
			state = eMascotState.TAP;
			index = 0;
		}
		mascotSound.OnTapMascot();
	}

	public void OnExitTap()
	{
		animator.SetInteger("State", (int)eMascotState.IDLE);
	}
}
