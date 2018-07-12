/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Annop "Nargus" Prapasapong
 * Created: 7 June 2012
 * *************************************************************************/
 
using UnityEngine;
using System.Collections;
 
/* **************************************************************************
 * CLASS: FPS COUNTER
 * *************************************************************************/ 
public class FPSCounter : MonoBehaviour {
	public  float updateInterval = 0.5F;
 
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private float fps = 0;
	public static string AdditionalInfo = "";
	
	void Start()
	{

	    timeleft = updateInterval;  
	}
	 
	void Update()
	{
	    timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;
	 
	    // Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 )
	    {
	        // display two fractional digits (f2 format)
			fps = accum/frames;
			//	DebugConsole.Log(format,level);
		        timeleft = updateInterval;
		        accum = 0.0F;
		        frames = 0;
	    }
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(0,Screen.height*0.5f,Screen.width,20),(fps.ToString()+" "+AdditionalInfo));
	}
}