using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;
using Improbable.Environment;
using Improbable.Unity.Common.Core.Math;


[WorkerType(WorkerPlatform.UnityWorker)]
public class StoneSpawner : MonoBehaviour
{
	[Require] BinmanInfo.Writer BinmanInfoWriter;
	[Require] StoneInfo.Reader StoneInfoReader;

	private void OnEnable() {
		StoneInfoReader.SpawnTriggered.Add (CreateStone);
	}

	private void OnDisable() {
		StoneInfoReader.SpawnTriggered.Remove(CreateStone);
	}

	private void CreateStone(SpawnData args) {
		Debug.LogWarning ("Spawning stone");
		var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateStoneTemplate (args.initialPosition.ToUnityVector());
		SpatialOS.Commands.CreateEntity(BinmanInfoWriter, entityTemplate)
			.OnSuccess(result => Debug.LogWarning("Created new stone"))
			.OnFailure(errorDetails => Debug.LogWarning("Failed to drop stone with error: " + errorDetails.ErrorMessage));
	}
}




