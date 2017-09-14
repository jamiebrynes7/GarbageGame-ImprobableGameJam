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
	[SerializeField] public InputField NameInput;

	private AudioSource audioSource;
	public AudioClip clickSound;

	private string Name;


	/*
		UI control section.
	*/
	public void AttemptBinbagConnection() {
		PlayClickSound();
		SetIsBinBag(true);
		AttemptSpatialOsConnection();
	}

	public void AttemptBinmanConnection() {
		PlayClickSound();
		SetIsBinBag(false);
		AttemptSpatialOsConnection();
	}

	public void OnChangeNameSelectorField()
	{
		Name = NameInput.text;
	}

	private void DisableUI() 
	{
		BinbagButton.interactable = false;
		BinmanButton.interactable = false;
		NameInput.interactable = false;
	}


	/*
		SpatialOS connection details.
	 */
	private void AttemptSpatialOsConnection()
	{
		DisableUI();
		AttemptConnection();
	}

	private void SetIsBinBag(bool isBinBag) 
	{
		Bootstrap boot = FindObjectOfType<Bootstrap>();
		boot.SetIsBinBag(isBinBag);
	}

	private void SetPlayerName()
	{
		Bootstrap boot = FindObjectOfType<Bootstrap>();
		boot.SetPlayerName(Name);
	}

	private void AttemptConnection() 
	{
		SetPlayerName();
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
		NameInput.interactable = true;
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
