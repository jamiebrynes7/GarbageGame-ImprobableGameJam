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
public class BinbagInteractions : MonoBehaviour
{
	[Require] BinJuiceInfo.Writer BinJuiceInfoWriter;

	private void OnTriggerEnter(Collider collision){
		if (collision.tag == "Binman") {
			SpatialOS.Commands.SendCommand (BinJuiceInfoWriter, BinbagInfo.Commands.BinmanTriggered.Descriptor, new TriggerData (this.transform.position.ToSpatialCoordinates ()),  this.gameObject.EntityId ());
		}
	}

	private void Update(){
		if (Input.GetKeyDown(KeyCode.Space))
		{
			var position = this.transform.position;
			position.y = 0;
			BinJuiceInfoWriter.Send (new BinJuiceInfo.Update().AddSpawn(new BinJuiceSpawnData(position.ToSpatialCoordinates())));
		}
	}
}

