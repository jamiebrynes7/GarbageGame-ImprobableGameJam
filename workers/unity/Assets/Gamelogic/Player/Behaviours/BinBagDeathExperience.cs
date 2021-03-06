﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Improbable.Unity.Visualizer;
using Improbable.Unity;
using Improbable.Core;
using Improbable.Unity.Core;
using Improbable.Unity.Common.Core.Math;

using Improbable.Player;
using Assets.Gamelogic.Utils;

[WorkerType(WorkerPlatform.UnityClient)]
public class BinBagDeathExperience : MonoBehaviour {

	private string TIP_MESSAGE = "The bin bag gods are pleased with your contribution to the cause and are blessing you with another life. Go forth and prosper young bin bag.";
	private string BINMAN_MESSAGE = "You have been captured by the notorious binmen. You are now be sent to the incinerator for processing. Lucky for you, bin bags are reincarnated...";
	private string STONE_MESSAGE = "You've been torn to shreds by stones. A horrible way to go. Hopefully next time you learn how to steer.";

	private GameObject DeathGUI;
	private Text respawnMessage;

	private int stoneCount;

	[Require] private ClientAuthorityCheck.Writer CACWriter;

	private void OnEnable()
	{
		GameObject c = GameObject.Find("Canvas");
		DeathGUI = c.transform.Find("Death Panel").gameObject;
		if (DeathGUI == null)
		{
			Debug.LogError("WTF");
		}
		respawnMessage = DeathGUI.transform.Find("DeathMessage").gameObject.GetComponent<Text>();
		stoneCount = 0;
	}

	void Update()
	{
		if (transform.position.y > 3000 && !DeathGUI.activeSelf)
		{
			respawnMessage.text = "You have been sent to bin bag purgatory due to YOLO code and network latency. Luckily, you happen to have a 'Get Out of Purgatory' card in your binbag. How utterly convenient.";
			DeathGUI.SetActive(true);
		}
		if (transform.position.y > 3000 && Input.GetKeyDown(KeyCode.R))
		{
			// Reset stone count
			stoneCount = 0;
			// Move player to new spawn position
			// Remove GUI
			Vector3 position = PositionUtils.GetRandomPosition();
			this.gameObject.transform.position = position;

			SpatialOS.Commands.SendCommand (CACWriter, PlayerMovement.Commands.Respawn.Descriptor, new SpawnPosition (position.ToSpatialVector3d ()), this.gameObject.EntityId ())
				.OnFailure (errorDetails => Debug.LogWarning ("Failed to respawn with error: " + errorDetails.ErrorMessage));
			
			DeathGUI.SetActive(false);
		} 
		else if (this.gameObject.transform.position.y < 3000 && this.gameObject.transform.position.y > 1000)
		{
			// Don't let them fall too far.
			Vector3 position = new Vector3(0f, 9000f, 0f);
			this.gameObject.transform.position = position;
			SpatialOS.Commands.SendCommand (CACWriter, PlayerMovement.Commands.Respawn.Descriptor, new SpawnPosition (position.ToSpatialVector3d ()), this.gameObject.EntityId ())
				.OnFailure (errorDetails => Debug.LogWarning ("Failed to respawn with error: " + errorDetails.ErrorMessage));
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (CACWriter != null) {
			if (collision.tag == "Binman")
			{
				respawnMessage.text = BINMAN_MESSAGE;
				DeathGUI.SetActive(true);
			}
			else if (collision.tag == "RubbishTipWtf")
			{
				respawnMessage.text = TIP_MESSAGE;
				DeathGUI.SetActive(true);
			} else if (collision.tag == "StoneWtf")
			{
				if (++stoneCount == 10)
				{
					respawnMessage.text = STONE_MESSAGE;
					DeathGUI.SetActive(true);
				}
			}
		}
	}

}
