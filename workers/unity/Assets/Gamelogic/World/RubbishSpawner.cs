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
    private static float INTERVAL = 10f;
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

            int numRubbish = GameObject.FindGameObjectsWithTag("Rubbish").Length;
            if (numRubbish < MAX_RUBBISH)
            {
                for (var i = 0; i < 50; i++)
                {
                    Vector3 position = PositionUtils.GetRandomPosition();
                    position.y = 0f;
                    var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateRubbishTemplate(position, (uint)Random.Range(0, 3));
                    SpatialOS.Commands.CreateEntity(ScoreWriter, entityTemplate)
                        .OnFailure(errorDetails => Debug.LogWarning("Failed to drop stone with error: " + errorDetails.ErrorMessage));
                }
            }
		}
	}
}




