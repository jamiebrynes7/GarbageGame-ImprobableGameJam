using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHider : MonoBehaviour {

	void OnEnable()
	{
		MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
		render.enabled = false;
	}
	
}
