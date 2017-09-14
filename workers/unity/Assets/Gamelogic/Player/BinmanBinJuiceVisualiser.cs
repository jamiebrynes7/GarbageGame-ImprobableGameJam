using System.Collections;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityClient)]
public class BinmanBinJuiceVisualiser : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "BinJuice")
		{
			other.gameObject.SetActive(false);
		}
	}
}
