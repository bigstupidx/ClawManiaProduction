using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class GUI_Dialog : MonoBehaviour 
{
	
	public static int layer = 290;
	public GameObject Window;
	public UIPanel uiPanel;
	public UITweener tweener;
	public UITweener.Method methodShow = UITweener.Method.Linear;
	public UITweener.Method methodHide = UITweener.Method.Linear;
	public float DurationShow = 1; // in seconds
	public float DurationHide = 1; // in seconds
	
	static Stack<GameObject> activeCanvasStack = new Stack<GameObject>();
	public static void InsertStack(GameObject ob)
	{
		if ( ob.GetComponent<GUI_Dialog>() )
		{
			Debug.Log("[GUIDialog] InsertStack "+ob.name);
			ob.gameObject.SetActive(true);
			activeCanvasStack.Push(ob);
			ob.GetComponent<GUI_Dialog>().Show();
			UIPanel panel = ob.GetComponent<UIPanel>();
			if ( panel.depth > 285 )
			{
				panel.depth = layer;
				layer ++;
				if ( layer > 490 )
					layer = 290;

				Transform trWindow = ob.GetComponent<GUI_Dialog>().Window.transform;
				if ( trWindow )
				{
					foreach ( UIScrollView scrollview in trWindow.gameObject.GetComponentsInChildren<UIScrollView>() )
					{
						UIPanel panelScroll = scrollview.GetComponent<UIPanel>();
						panelScroll.depth = layer;
						layer++;

					}
				}
			}
		}
	}

	public static int GetActiveStackAmount()
	{
		return activeCanvasStack.Count;
	}

	public static void ClearStack()
	{
		activeCanvasStack = new Stack<GameObject>();
	}
	
	public static void ReleaseTopCanvas()
	{
		if (activeCanvasStack.Count > 0) 
		{
			GameObject canvasGo = activeCanvasStack.Pop ();
			Debug.Log("[GUI_Dialog] hiding "+canvasGo.name+" stacks left="+activeCanvasStack.Count);
			canvasGo.GetComponent<GUI_Dialog>().Hide();
		}
		else
			Debug.Log("[GUI_Dialog] No more stack");
	}

	bool bShow = false;

	// Use this for initialization
	void Start () {
		OnStart ();
			tweener.enabled = false;
		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		OnUpdate ();
	}

	void OnEnable()
	{
		WhenEnabled ();
	}
	
	public virtual void OnUpdate(){}
	public virtual void WhenEnabled(){}
	public virtual void OnShow(){}
	public virtual void OnTweenDone(){}
	public virtual void OnStart(){}
	protected virtual void OnHide(){}
	public virtual void OnDoneTweening()
	{
		if ( !bShow && !bFirstTime )
		{
			if ( uiPanel )
				uiPanel.alpha = 0;
			
			ActivateColliders (this.gameObject, false);
			this.gameObject.SetActive(false);
			OnTweenDone ();
		}
	}

	public void ActivateColliders(GameObject ob,bool bVal)
	{
		BoxCollider collider = ob.GetComponent<BoxCollider> ();
		if ( collider )
		{
			collider.enabled = bVal;
			
			if ( bVal && ob.GetComponent<UIButton>())
			{
				ob.gameObject.SetActive(false);
				ob.gameObject.SetActive(true);
			}
		}

		for ( int i=0; i<ob.transform.childCount; i++ )
		{
			ActivateColliders(ob.transform.GetChild(i).gameObject,bVal);
		}
	}
	
	bool bFirstTime = true;
	public void Show()
	{
		this.gameObject.SetActive (true);
		bShow = true;
		if ( tweener )
		{
			tweener.duration = DurationShow;
			tweener.method = methodShow;
			tweener.enabled = true;
			if ( bFirstTime )
			{
				tweener.ResetToBeginning();
				bFirstTime = false;
			}
			tweener.Play(true);
		}
		if ( uiPanel )
			uiPanel.alpha = 1;
		ActivateColliders (this.gameObject, true);
		OnShow ();
	}

	void Hide()
	{
		bShow = false;
		if ( tweener )
		{
			tweener.duration = DurationHide;
			tweener.enabled = true;
			tweener.method = methodHide;
			tweener.Play(false);
		}
		OnHide ();
	}

	public bool isVisible()
	{
		if ( uiPanel )
		{
			if ( uiPanel.alpha == 0 )
				return false;
			else
				return true;
		}
		return false;
	}
}
