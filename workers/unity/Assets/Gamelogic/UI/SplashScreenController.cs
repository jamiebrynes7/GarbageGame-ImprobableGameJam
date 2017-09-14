using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Improbable.Unity.Core;

using Assets.Gamelogic.Utils;
using Assets.Gamelogic.Core;

public class SplashScreenController : MonoBehaviour {

	[SerializeField] public Button BinbagButton;
	[SerializeField] public Button BinmanButton;

	private AudioSource audioSource;
	public AudioClip clickSound;

	// Button callback for "Connect as Binbag"
	public void AttemptBinbagConnection() {
		PlayClickSound();
		SetIsBinBag(true);
		AttemptSpatialOsConnection();
	}

	// Button callback for "Connect as Binman"
	public void AttemptBinmanConnection() {
		PlayClickSound();
		SetIsBinBag(false);
		AttemptSpatialOsConnection();
	}

	private void AttemptSpatialOsConnection()
	{
		DisableConnectButtons();
		AttemptConnection();
	}

	private void SetIsBinBag(bool isBinBag) 
	{
		Bootstrap boot = FindObjectOfType<Bootstrap>();
		boot.SetIsBinBag(isBinBag);
	}

	private void DisableConnectButtons() 
	{
		BinbagButton.interactable = false;
		BinmanButton.interactable = false;
	}

	private void AttemptConnection() 
	{
        FindObjectOfType<Bootstrap>().ConnectToSpatialOS();
		StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.ClientConnectionTimeoutSecs, ConnectionTimeout));
	}

	private void ConnectionTimeout() 
	{
		if (SpatialOS.IsConnected)
		{
			SpatialOS.Disconnect();
		}

		BinbagButton.interactable = true;
		BinmanButton.interactable = true;
	}

	/*
		Section for playing sound on a button press.
	 */

	 private void PlayClickSound()
	 {
		 if (audioSource == null)
		 {
			 audioSource = gameObject.AddComponent<AudioSource>();
		 }

		 audioSource.clip = clickSound;
		 audioSource.Play();
	 }
}
