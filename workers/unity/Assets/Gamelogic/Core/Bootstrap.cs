﻿using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Configuration;
using Improbable.Unity.Core;
using Improbable.Unity.Core.EntityQueries;
using UnityEngine;
using UnityEngine.SceneManagement;

// Placed on a GameObject in a Unity scene to execute SpatialOS connection logic on startup.
namespace Assets.Gamelogic.Core
{
	public class Bootstrap : MonoBehaviour
	{
		public WorkerConfigurationData Configuration = new WorkerConfigurationData();


		// Boolean to handle whether to spawn player as binbag or binman. Defaults to binbag.
		private bool isBinBag = true;
		private string name = "Garbage";

        // Called when the Play button is pressed in Unity.
        public void Start()
        {
            SpatialOS.ApplyConfiguration(Configuration);
            if (SpatialOS.Configuration.WorkerPlatform == WorkerPlatform.UnityClient)
            {
                SceneManager.LoadScene(BuildSettings.SplashScreenScene, LoadSceneMode.Additive);
            }

			Time.fixedDeltaTime = 1.0f / SimulationSettings.FixedFramerate;

			// Distinguishes between when the Unity is running as a client or a server.
			switch (SpatialOS.Configuration.WorkerPlatform)
			{
			case WorkerPlatform.UnityWorker:
				Application.targetFrameRate = SimulationSettings.TargetServerFramerate;
				SpatialOS.OnDisconnected += reason => Application.Quit();
				SpatialOS.Connect(gameObject);
				break;
			case WorkerPlatform.UnityClient:
				Application.targetFrameRate = SimulationSettings.TargetClientFramerate;
				SpatialOS.OnConnected += () => CreatePlayer(this.isBinBag, this.name);
				break;
			}
		}

        // Connect to game server
        public void ConnectToSpatialOS() 
        {
            SpatialOS.Connect(gameObject);
        }

		// Set isBinBag boolean for player spawning
		public void SetIsBinBag(bool b) {
			this.isBinBag = b;
		}

		// Set player name for player spawning
		public void SetPlayerName(string name) {
			this.name = name;
		}

		// Search for the PlayerCreator entity in the world in order to send a CreatePlayer command.
		public static void CreatePlayer(bool isBinBag, string name)
		{
			var playerCreatorQuery = Query.HasComponent<PlayerCreation>().ReturnOnlyEntityIds();
			SpatialOS.WorkerCommands.SendQuery(playerCreatorQuery)
				.OnSuccess(result => OnSuccessfulPlayerCreatorQuery(result, isBinBag, name))
				.OnFailure(details => OnFailedPlayerCreatorQuery(details, isBinBag, name));
		}

		private static void OnSuccessfulPlayerCreatorQuery(EntityQueryResult queryResult, bool isBinBag, string name)
		{
			if (queryResult.EntityCount < 1)
			{
				Debug.LogError("Failed to find PlayerCreator. SpatialOS probably hadn't finished loading the initial snapshot. Try again in a few seconds.");
				return;
			}

			var playerCreatorEntityId = queryResult.Entities.First.Value.Key;
			RequestPlayerCreation(playerCreatorEntityId, isBinBag, name);
		}

		// Retry a failed search for the PlayerCreator entity after a short delay.
		private static void OnFailedPlayerCreatorQuery(ICommandErrorDetails _, bool isBinBag, string name)
		{
			Debug.LogError("PlayerCreator query failed. Spati   alOS workers probably haven't started yet. Try again in a few seconds.");
			TimerUtils.WaitAndPerform(SimulationSettings.PlayerCreatorQueryRetrySecs, () => CreatePlayer(isBinBag, name));
		}

		// Send a CreatePlayer command to the PLayerCreator entity requesting a Player entity be spawned.
		private static void RequestPlayerCreation(EntityId playerCreatorEntityId, bool isBinBag, string name)
		{
			SpatialOS.WorkerCommands.SendCommand(PlayerCreation.Commands.CreatePlayer.Descriptor, new CreatePlayerRequest(isBinBag, name), playerCreatorEntityId)
				.OnFailure(response => OnCreatePlayerFailure(response, playerCreatorEntityId, isBinBag, name));
		}

		// Retry a failed creation of the Player entity after a short delay.
		private static void OnCreatePlayerFailure(ICommandErrorDetails _, EntityId playerCreatorEntityId, bool isBinBag, string name)
		{
			Debug.LogWarning("CreatePlayer command failed - you probably tried to connect too soon. Try again in a few seconds.");
			TimerUtils.WaitAndPerform(SimulationSettings.PlayerEntityCreationRetrySecs, () => RequestPlayerCreation(playerCreatorEntityId, isBinBag, name));
		}
	}
}