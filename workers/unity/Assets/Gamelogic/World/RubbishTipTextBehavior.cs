using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Improbable.Unity.Visualizer;
using Improbable.Core;
using Improbable.Unity;

using Improbable.Environment;

namespace Assets.Gamelogic.World {

	[WorkerType(WorkerPlatform.UnityClient)]
	public class RubbishTipTextBehavior : MonoBehaviour {

		public GameObject text;

		[Require] private RubbishTipInfo.Reader RubbishTipInfoReader;

		private void OnEnable() 
		{
			RubbishTipInfoReader.NumberBinBagsUpdated.Add(OnNumberBinBagsUpdated);
		}

		private void OnDisable()
		{
			RubbishTipInfoReader.NumberBinBagsUpdated.Remove(OnNumberBinBagsUpdated);
		}

		private void OnNumberBinBagsUpdated(uint numberBinBags)
		{
			// Update floating text
			text.GetComponent<TextMesh>().text = numberBinBags.ToString() + "/10";
		}
	}

}