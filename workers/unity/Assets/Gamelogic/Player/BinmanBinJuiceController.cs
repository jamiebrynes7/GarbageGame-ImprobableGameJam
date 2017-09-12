using System.Collections;
using System.Collections.Generic;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityWorker)]
public class BinmanBinJuiceController : MonoBehaviour
{
    private static float DRUNK_TIME = 10;

    [Require]
    private BinmanInfo.Writer binmanInfoWriter;

    private float timeToSoberUp = -1;

    private void OnEnable()
    {
        if(binmanInfoWriter.Data.isDrunk){
            binmanInfoWriter.Send(new BinmanInfo.Update().SetIsDrunk(false));
        }
    }

    private void Update()
    {
        if(timeToSoberUp > 0 && Time.time > timeToSoberUp){
            timeToSoberUp = -1;
            binmanInfoWriter.Send(new BinmanInfo.Update().SetIsDrunk(false));
        }
    }

    private void OnTriggerEnter(Collider other)
	{
		if (binmanInfoWriter != null && other.tag == "BinJuice")
		{
			other.gameObject.SetActive(false);
			HitBinJuice(other.transform.parent.gameObject);
		}
	}

	private void HitBinJuice(GameObject binJuiceGameObject)
	{
		var entityId = binJuiceGameObject.EntityId();
        binmanInfoWriter.Send(new BinmanInfo.Update().SetIsDrunk(true));
        timeToSoberUp = Time.time + DRUNK_TIME;
		SpatialOS.Commands.DeleteEntity(binmanInfoWriter, entityId)
			.OnFailure(errorDetails =>
					   Debug.LogError("Failed to delete binjuice entity with error: " + errorDetails.ErrorMessage));
	}
}
