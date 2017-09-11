using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Map : MonoBehaviour
	{
		public void Start() {
			Plane plane = new Plane ();
			plane.SetNormalAndPosition (
				new Vector3(0, 1, 0),
				new Vector3(0, 0, 0)
			);


		}
	}
}

