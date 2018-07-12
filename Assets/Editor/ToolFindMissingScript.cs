using UnityEngine;
using UnityEditor;
public class ToolFindMissingScript : EditorWindow {

	[MenuItem("Tools/FindMissingScript")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(ToolFindMissingScript));
	}
	
	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected prefabs"))
		{
			FindAll();
		}
	}

	private static void FindAll()
	{
		Debug.Log("[ToolFindMissingScript] FindAll");
		GameObject[] go = Selection.gameObjects;
		int go_count = 0, components_count = 0, missing_count = 0;
		foreach (GameObject g in go)
		{
			FindInSelected(g,"");
		}
		Debug.LogError ("missing=" + go_count);
	}

	private static void FindInSelected(GameObject g,string parent)
	{
		parent += g.name+"/";
		Component[] components = g.GetComponents<Component>();
		Debug.Log("[ToolFindMissingScript] FindIn " + g.name+" amount="+components.Length);
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] == null)
			{
				Debug.Log("[ToolFindMissingScript] Missing script found at '"+parent+"' on "+i.ToString());
			}
			else
			{
				Debug.Log("[ToolFindMissingScript]    script at "+i.ToString()+" is '"+components[i].ToString()+"'");
				
			}
		}
		
		for ( int i=0 ; i<g.transform.childCount ; i++ )
		{
			FindInSelected(g.transform.GetChild(i).gameObject,parent);
		}
	}
	/*
	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		int go_count = 0, components_count = 0, missing_count = 0;
		foreach (GameObject g in go)
		{
			go_count++;
			Component[] components = g.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				components_count++;
				if (components[i] == null)
				{
					missing_count++;
					string s = g.name;
					Transform t = g.transform;
					while (t.parent != null) 
					{
						s = t.parent.name +"/"+s;
						t = t.parent;
					}
					Debug.Log (s + " has an empty script attached in position: " + i, g);
				}
			}
		}
		
		Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
	}
	*/
}
