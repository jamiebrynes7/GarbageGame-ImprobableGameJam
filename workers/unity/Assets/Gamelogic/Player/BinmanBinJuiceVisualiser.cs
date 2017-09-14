using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinmanBinJuiceVisualiser : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "BinJuice")
		{
			other.gameObject.SetActive(false);
		}
	}
}
