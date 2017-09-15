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
public class TipSpawner : MonoBehaviour
{
    private static float INTERVAL = 60f;
    private static int MAX_RUBBISH = 500;

	[Require] private Score.Writer ScoreWriter;

    private float nextCheckTime = -1;

    private void OnEnable()
    {
        nextCheckTime = Time.time + INTERVAL;
    }

    private void Update() {
        if (Time.time > nextCheckTime) {
            nextCheckTime = Time.time + INTERVAL;

            int numRubbish = GameObject.FindGameObjectsWithTag("RubbishTipWtf").Length;
            if (numRubbish < MAX_RUBBISH)
            {
				Vector3 position = PositionUtils.GetRandomPosition();
				position.y = 0f;
				var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateRubbishTipTemplate(position);
				SpatialOS.Commands.CreateEntity(ScoreWriter, entityTemplate)
					.OnFailure(errorDetails => Debug.LogWarning("Failed to drop tip with error: " + errorDetails.ErrorMessage));
            }
		}
	}
}




