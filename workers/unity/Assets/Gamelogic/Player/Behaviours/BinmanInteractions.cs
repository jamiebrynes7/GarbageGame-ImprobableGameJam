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


[WorkerType(WorkerPlatform.UnityClient)]
public class BinmanInteractions : MonoBehaviour
{
	[Require] StoneInfo.Writer StoneInfoWriter;

	private void OnTriggerEnter(Collider collision){
		if (collision.tag == "Binbag") {
			SpatialOS.Commands.SendCommand (StoneInfoWriter, BinbagInfo.Commands.BinmanTriggered.Descriptor, new TriggerData (this.transform.position.ToSpatialCoordinates ()), collision.gameObject.EntityId ());
		}
	}

	private void Update(){
		if (Input.GetKeyDown(KeyCode.Q))
		{
			StoneInfoWriter.Send (new StoneInfo.Update().AddSpawn(new SpawnData(this.transform.position.ToSpatialCoordinates())));
		}
	}
}

