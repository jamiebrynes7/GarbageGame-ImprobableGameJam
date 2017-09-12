using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Core;
using UnityEngine;

using Improbable.Environment;
using Improbable.Unity;
using Assets.Gamelogic.Core;

namespace Assets.Gamelogic.World {

	[WorkerType(WorkerPlatform.UnityWorker)]
	public class RubbishTipCollision : MonoBehaviour {

		[Require] private RubbishTipInfo.Writer RubbishTipInfoWriter;

        public void OnTriggerEnter(Collider other)
        {
            // Only interested about collisions with binbags.
			if (other != null && other.gameObject.tag == SimulationSettings.BinbagTag)
			{
				// Update the number of bin bags.
				uint newNumberBinBags = RubbishTipInfoWriter.Data.numberBinBags + 1;
				RubbishTipInfoWriter.Send(new RubbishTipInfo.Update().SetNumberBinBags(newNumberBinBags));
			}
        }
	}
}
