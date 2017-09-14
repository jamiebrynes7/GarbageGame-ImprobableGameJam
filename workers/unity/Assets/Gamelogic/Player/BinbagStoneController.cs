using System.Collections;
using System.Collections.Generic;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityWorker)]
public class BinbagStoneController : MonoBehaviour {

    [Require]
    private BinbagInfo.Writer binbagInfoWriter;

    private void OnTriggerEnter(Collider other)
    {
        if(binbagInfoWriter != null && other.tag == "StoneWtf"){
            other.gameObject.SetActive(false);
            HitStone(other.transform.parent.gameObject);
		}
		else if (binbagInfoWriter != null && other.tag == "Rubbish")
		{
			other.gameObject.SetActive(false);
			HitRubbish(other.transform.parent.gameObject);
		}
    }

    private void HitStone(GameObject stoneGameObject){
        var entityId = stoneGameObject.EntityId();
        var newHealth = binbagInfoWriter.Data.health - 1;
        binbagInfoWriter.Send(new BinbagInfo.Update().SetHealth(newHealth));
        SpatialOS.Commands.DeleteEntity(binbagInfoWriter, entityId)
	        .OnFailure(errorDetails => 
                       Debug.LogError("Failed to delete stone entity with error: " + errorDetails.ErrorMessage));
    }

    private void HitRubbish(GameObject rubbishGameObject){
		var entityId = rubbishGameObject.EntityId();
        var newSize = binbagInfoWriter.Data.size + 1;
        binbagInfoWriter.Send(new BinbagInfo.Update().SetSize(newSize));
		SpatialOS.Commands.DeleteEntity(binbagInfoWriter, entityId)
			.OnFailure(errorDetails =>
					   Debug.LogError("Failed to delete rubbish entity with error: " + errorDetails.ErrorMessage));
    }
}
