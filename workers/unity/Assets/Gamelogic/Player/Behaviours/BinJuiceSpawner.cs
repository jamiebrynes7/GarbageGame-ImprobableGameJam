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
public class BinJuiceSpawner : MonoBehaviour
{
	[Require] BinbagInfo.Writer BinbagInfoWriter;
	[Require] BinJuiceInfo.Reader BinJuiceInfoReader;

	private void OnEnable() {
		BinJuiceInfoReader.SpawnTriggered.Add (CreateBinJuice);
	}

	private void OnDisable() {
		BinJuiceInfoReader.SpawnTriggered.Remove(CreateBinJuice);
	}

	private void CreateBinJuice(BinJuiceSpawnData args) {
		var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateBinJuiceTemplate (args.initialPosition.ToUnityVector());
		SpatialOS.Commands.CreateEntity(BinbagInfoWriter, entityTemplate)
			.OnFailure(errorDetails => Debug.LogWarning("Failed to drop stone with error: " + errorDetails.ErrorMessage));
	}
}




