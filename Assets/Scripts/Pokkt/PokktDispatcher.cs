using UnityEngine;
using System.Collections;
using System;

public class PokktDispatcher : MonoBehaviour
{
	// pokkt events
	public event Action<string> VideoClosedEvent;
	public event Action<string> VideoDisplayedEvent;
	public event Action<string> VideoSkippedEvent;
	public event Action<string> VideoCompletedEvent;
	public event Action<string> VideoGratifiedEvent;
	public event Action<string> DownloadCompletedEvent;
	public event Action<string> DownloadFailedEvent;
	public event Action<string> CoinResponseEvent;
	public event Action<string> CoinResponseWithTransIdEvent;
	public event Action<string> CoinResponseFailedEvent;
	public event Action<string> CampaignAvailabilityEvent;
	public event Action<string> OfferwallClosedEvent;
	
	void Start()
	{
	}
	
	void Update()
	{
	}

	public void VideoClosed(string message)
	{
		if (VideoClosedEvent != null)
			VideoClosedEvent(message);
	}
	
	public void VideoDisplayed(string message)
	{
		if (VideoDisplayedEvent != null)
			VideoDisplayedEvent(message);
	}
	
	public void VideoSkipped(string message)
	{
		if (VideoSkippedEvent != null)
			VideoSkippedEvent(message);
	}
	
	public void VideoCompleted(string message)
	{
		if (VideoCompletedEvent != null)
			VideoCompletedEvent(message);
	}
	
	public void VideoGratified(string message)
	{
		if (VideoGratifiedEvent != null)
			VideoGratifiedEvent(message);
	}
		
	public void DownloadCompleted(string message)
	{
		if (DownloadCompletedEvent != null)
			DownloadCompletedEvent(message);
	}
	
	public void DownloadFailed(string message)
	{
		if (DownloadFailedEvent != null)
			DownloadFailedEvent(message);
	}
	
	public void CoinResponse(string message)
	{
		if (CoinResponseEvent != null)
			CoinResponseEvent(message);
	}
	
	public void CoinResponseWithTransId(string message)
	{
		if (CoinResponseWithTransIdEvent != null)
			CoinResponseWithTransIdEvent(message);
	}
	
	public void CoinResponseFailed(string message)
	{
		if (CoinResponseFailedEvent != null)
			CoinResponseFailedEvent(message);
	}
	
	public void CampaignAvailability(string message)
	{
		if (CampaignAvailabilityEvent != null)
			CampaignAvailabilityEvent(message);
	}
	
	public void OfferwallClosed(string message)
	{
		if (OfferwallClosedEvent != null)
			OfferwallClosedEvent(message);
	}
}
