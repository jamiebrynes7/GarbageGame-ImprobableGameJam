using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class World : MonoBehaviour
	{
		public Transform roadBlockParent;

		[ContextMenu("Reset World")]
		void ResetWorld() {
			// Reset World
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("House")) {
				DestroyImmediate(obj);
			}
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoadBlock")) {
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

			// Add Roadblocks
			int roadblock_counter = 0;
			for (int z = -10; z < 10; z++) {
				for (int x = -10; x < 10; x++) {
					if (UnityEngine.Random.Range (0f, 1f) < 0.2f) {
						GameObject instance = Instantiate (Resources.Load ("RoadBlocker/RoadBlocker2")) as GameObject;
						instance.transform.localPosition = new Vector3 (x * 15f - 1.5f, 0, z * 20f);
						instance.transform.localScale = new Vector3 (7f, 1f, 3f);
						instance.tag = "RoadBlock";
						instance.name = "rb" + roadblock_counter;
						instance.transform.parent = roadBlockParent;

						roadblock_counter++;
					}

					if (UnityEngine.Random.Range (0f, 1f) < 0.2f) {
						GameObject instance = Instantiate (Resources.Load ("RoadBlocker/RoadBlocker2")) as GameObject;
						instance.transform.localPosition = new Vector3 (x * 15f + 6f, 0, z * 20f + 9f);
						instance.transform.localScale = new Vector3 (6f, 1f, 3f);
						var rotationVector = instance.transform.rotation.eulerAngles;
						rotationVector.y += 90;
						instance.transform.rotation = Quaternion.Euler (rotationVector);
						instance.tag = "RoadBlock";
						instance.name = "rb" + roadblock_counter;
						instance.transform.parent = roadBlockParent;

						roadblock_counter++;
					}
				}
			}

		}
	}
}

