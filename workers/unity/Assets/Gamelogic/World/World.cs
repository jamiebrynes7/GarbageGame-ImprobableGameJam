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
			this.gameObject.transform.localScale = new Vector3 (2, 1, 2);
			for (int z = -10; z < 10; z = z + 3) {
				for (int x = -10; x < 10; x = x + 2) {
					GameObject instance = Instantiate (Resources.Load("EntityPrefabs/Buildings/Prefab/abandoned_house")) as GameObject;
					instance.transform.position = new Vector3 (x, 0, z);
					instance.transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
					instance.tag = "House";
					instance.transform.parent = this.gameObject.transform;
				}
			}

		}
	}
}

