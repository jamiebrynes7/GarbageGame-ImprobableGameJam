using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Improbable.Unity;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Core;

using Improbable.Environment;
using Assets.Gamelogic.Core;


[WorkerType(WorkerPlatform.UnityClient)]
public class ArrowPointerManager : MonoBehaviour {

	public GameObject arrow;
	private GameObject[] rubbishTips;
	private GameObject closestRubbishTip;

	[Require] private ClientAuthorityCheck.Writer CACWriter; 

	void Update() 
	{
		// Poll local worker area for current list of tips in the world.
		GetCurrentRubbishTips();
		LookAtClosestTip();
		if (closestRubbishTip != null)
		{
			// Enable the arrow renderer.
			MeshRenderer render = arrow.GetComponentInChildren<MeshRenderer>();
			render.enabled = true;
			arrow.transform.LookAt(closestRubbishTip.transform);
		}
	}

	private void OnEnable()
	{


		// Poll local worker area for current list of tips in the world.
		GetCurrentRubbishTips();

		// Find tip with smallest distance.
		// Look at this.
		LookAtClosestTip();
	}

	private void GetCurrentRubbishTips()
	{	
		rubbishTips = GameObject.FindGameObjectsWithTag(SimulationSettings.RubbishTipTag);
	}

	private void LookAtClosestTip()
	{
		GameObject closest = null;
		float distance = 1000000f;
		foreach (GameObject tip in rubbishTips)
		{
			// Calculate distance
			Vector3 diff = new Vector3(
				tip.transform.position.x - this.transform.position.x, 
				tip.transform.position.y - this.transform.position.y, 
				tip.transform.position.z - this.transform.position.z);

			float d = (float) System.Math.Sqrt(
				Math.Pow(diff.x, 2f) + 
				Math.Pow(diff.y, 2f) + 
				Math.Pow(diff.z, 2f)
			);
			
			// Check if closer.
			if (d < distance)
			{
				closest = tip;
				distance = d;
			}
		}
		closestRubbishTip = closest;
	}

}
