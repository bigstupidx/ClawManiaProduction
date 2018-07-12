using UnityEngine;
using System.Collections;
using Pokkt;

public class ScreenManager : MonoBehaviour
{
	// constants
	private const string ScreenOpenParam = "Open";
	private const string IdleScreenStateName = "Idle";
	private const string OpenScreenStateName = "Open";
	private const string CloseSreenStateName = "Close";

	// exposed properties
	public GameObject UICanvas = null;

	// internal states
	private Animator _currentScreen;
	private int _openScreenAnimationId;
		
	void Awake()
	{
		// set debug
		PokktManager.SetDebug(true);
	}

	void Start()
	{
		_openScreenAnimationId = Animator.StringToHash(ScreenOpenParam);
		
		// default screen
		OpenScreen("DemoSelectorScreen");
	}
	
	void Update()
	{
	}

	public void OpenScreen(string screenName)
	{
		Transform screenToOpenTransform = UICanvas.transform.Find(screenName);
		if (screenToOpenTransform != null)
		{
			Animator screenToOpen = screenToOpenTransform.GetComponent<Animator>();
			OpenScreen(screenToOpen);
		}
	}

	public void OpenScreen(Animator screenToOpen)
	{
		if (screenToOpen == null || screenToOpen == _currentScreen)
			return;

		CloseCurrentScreen();

		// open requested screen
		screenToOpen.gameObject.SetActive(true);
		screenToOpen.SetBool(_openScreenAnimationId, true);

		_currentScreen = screenToOpen;
	}

	public void CloseCurrentScreen()
	{
		if (_currentScreen == null)
			return;

		_currentScreen.SetBool(_openScreenAnimationId, false);

		// disable it once the animation is over
		StartCoroutine(DisableScreen(_currentScreen));
	}

	private IEnumerator DisableScreen(Animator screen)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!screen.IsInTransition(0))
				closedStateReached = screen.GetCurrentAnimatorStateInfo(0).IsName(CloseSreenStateName);
			
			wantToClose = !screen.GetBool(_openScreenAnimationId);
			
			yield return new WaitForEndOfFrame();
		}
		
		if (wantToClose)
			screen.gameObject.SetActive(false);
	}

	public void SetAutoCaching(bool autoCaching)
	{
		Storage.setAutoCaching(autoCaching);
	}	
}
