﻿using System.Collections;
using System.Collections.Generic;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityClient)]
public class BinbagStoneVisualiser : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Stone")
		{
            other.gameObject.SetActive(false);
		}
	}
}