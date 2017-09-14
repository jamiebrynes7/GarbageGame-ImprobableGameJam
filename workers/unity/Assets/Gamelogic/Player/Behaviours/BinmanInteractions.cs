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

	private void Update(){
		if (Input.GetKeyDown(KeyCode.Space))
		{
            var position = this.transform.position;
            position.y = 0;
			StoneInfoWriter.Send (new StoneInfo.Update().AddSpawn(new SpawnData(position.ToSpatialCoordinates())));
		}
	}
}

