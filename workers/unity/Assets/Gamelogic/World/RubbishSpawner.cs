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
using Improbable.Misc;
using Assets.Gamelogic.Utils;


[WorkerType(WorkerPlatform.UnityWorker)]
public class RubbishSpawner : MonoBehaviour
{
	[Require] Score.Writer ScoreWriter;
	private float time = 0;


	private void Update() {
		time += Time.deltaTime;
		if (time > 6) {
			Vector3 position = PositionUtils.GetRandomPosition ();
			var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateRubbishTemplate (position, (uint) Random.Range(0, 3));
			SpatialOS.Commands.CreateEntity (ScoreWriter, entityTemplate)
				.OnFailure (errorDetails => Debug.LogWarning ("Failed to drop stone with error: " + errorDetails.ErrorMessage));
		}
	}
}




