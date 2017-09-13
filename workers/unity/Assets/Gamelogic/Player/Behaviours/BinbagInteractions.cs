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

	private void Update(){
		if (Input.GetKeyDown(KeyCode.Q))
		{
			BinJuiceInfoWriter.Send (new BinJuiceInfo.Update().AddSpawn(new BinJuiceSpawnData(this.transform.position.ToSpatialCoordinates())));
		}
	}
}

