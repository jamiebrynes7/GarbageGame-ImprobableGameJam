using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Core;
using UnityEngine;

using Improbable.Environment;
using Improbable.Unity;

namespace Assets.Gamelogic.World {

	[WorkerType(WorkerPlatform.UnityWorker)]
	public class RubbishTipBehavior : MonoBehaviour {

		[Require] private RubbishTipInfo.Reader RubbishTipInfoReader;

		public int capacity;

		private void OnEnable()
		{
			RubbishTipInfoReader.NumberBinBagsUpdated.Add(OnNumberBinBagsUpdated);
		}
		
		private void OnDisable()
		{
			RubbishTipInfoReader.NumberBinBagsUpdated.Remove(OnNumberBinBagsUpdated);
		}

		private void OnNumberBinBagsUpdated(uint numberBinBags) {

			if (numberBinBags >= capacity) {
				SpatialOS.WorkerCommands.DeleteEntity(this.gameObject.EntityId());
			}
		}
	}
}
