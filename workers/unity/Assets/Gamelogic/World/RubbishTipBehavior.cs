using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Core;
using Improbable.Entity.Component;
using UnityEngine;

using Improbable.Environment;
using Improbable.Unity;

namespace Assets.Gamelogic.World {

	[WorkerType(WorkerPlatform.UnityWorker)]
	public class RubbishTipBehavior : MonoBehaviour {

		[Require] private RubbishTipInfo.Writer RubbishTipInfoWriter;
		public int capacity;

		private void OnEnable()
		{
			RubbishTipInfoWriter.NumberBinBagsUpdated.Add(OnNumberBinBagsUpdated);

			// Register command responses.
			RubbishTipInfoWriter.CommandReceiver.OnIncrementTip.RegisterResponse(OnIncrementTip);
		}
		
		private void OnDisable()
		{
			RubbishTipInfoWriter.NumberBinBagsUpdated.Remove(OnNumberBinBagsUpdated);


			// Deregister command response
			RubbishTipInfoWriter.CommandReceiver.OnIncrementTip.DeregisterResponse();
		}

		private IncrementTipResponse OnIncrementTip(IncrementTipRequest request, ICommandCallerInfo callerInfo)
		{
			uint new_number = RubbishTipInfoWriter.Data.numberBinBags + 1;
			RubbishTipInfoWriter.Send( new RubbishTipInfo.Update().SetNumberBinBags(new_number));

			return new IncrementTipResponse();
		}

		private void OnNumberBinBagsUpdated(uint numberBinBags) {

			if (numberBinBags >= capacity) {
				SpatialOS.WorkerCommands.DeleteEntity(this.gameObject.EntityId());
			}
		}
	}
}
