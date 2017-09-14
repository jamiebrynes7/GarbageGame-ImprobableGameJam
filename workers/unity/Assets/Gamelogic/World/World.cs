using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class World : MonoBehaviour
	{
		[ContextMenu("Reset World")]
		void ResetWorld() {
			// Reset World
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("House")) {
				DestroyImmediate(obj);
			}
		}

		[ContextMenu ("Create World")]
		void CreateWorld ()
		{
			ResetWorld ();

			// Create World

			// Add Houses
			//this.gameObject.transform.localScale = new Vector3 (200, 1, 200);
			for (int z = -10; z < 10; z++) {
				for (int x = -10; x < 10; x++) {
					GameObject instance = Instantiate (Resources.Load("EntityPrefabs/Building")) as GameObject;
                    instance.transform.localPosition = new Vector3 (6f + x * 15f, 0, z * 20f);
					instance.transform.localScale = new Vector3 (1f, 1f, 1f);
					instance.tag = "House";
					instance.transform.parent = this.gameObject.transform;
				}
			}

		}
	}
}

