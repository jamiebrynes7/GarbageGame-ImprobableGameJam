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
	private static float DROP_INTERVAL = 1f;

	[Require] StoneInfo.Writer StoneInfoWriter;

	private float lastDropTime = -1f;

	private void OnTriggerEnter(Collider collision){
		if (collision.tag == "Binbag") {
			SpatialOS.Commands.SendCommand (StoneInfoWriter, BinbagInfo.Commands.BinmanTriggered.Descriptor, new TriggerData (this.transform.position.ToSpatialCoordinates ()), collision.gameObject.EntityId ());
		}
	}

	private void Update(){
		if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDropTime + DROP_INTERVAL)
		{
            lastDropTime = Time.time;
            var position = this.transform.position;
            position.y = 0;
			StoneInfoWriter.Send (new StoneInfo.Update().AddSpawn(new SpawnData(position.ToSpatialCoordinates())));
		}
	}
}

