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
        if(binbagInfoWriter != null && other.tag == "Stone"){
            HitStone(other.transform.parent.gameObject);
        }
    }

    private void HitStone(GameObject stoneGameObject){
        var entityId = stoneGameObject.EntityId();
        SpatialOS.Commands.DeleteEntity(binbagInfoWriter, entityId)
	        .OnFailure(errorDetails => 
                       Debug.LogError("Failed to delete stone entity with error: " + errorDetails.ErrorMessage));
    }
}
