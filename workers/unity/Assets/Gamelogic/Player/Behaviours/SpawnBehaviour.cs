using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Gamelogic.Player.Behaviours
{
	// Add this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityClient)]
	public class SpawnBehaviour : MonoBehaviour
	{
		[Require] private BinbagInfo.Reader BinbagInfoReader;

		private void OnEnable()
		{
			BinbagInfoReader.HealthUpdated.Add(OnCurrentHealthUpdated);
		}

		private void OnDisable()
		{
			BinbagInfoReader.HealthUpdated.Remove(OnCurrentHealthUpdated);
		}


		// Callback for whenever the CurrentHealth property of the Health component is updated
		private void OnCurrentHealthUpdated(uint currentHealth)
		{
			//if (currentHealth <= 0)
			//{
				Respawn ();
			//}
		}

		private void Respawn()
		{
			//BinbagInfoWriter.Send (new BinbagInfo.Update().SetHealth(10));
			Vector3 position = new Vector3(Random.Range(-100, 100), 2, Random.Range(-100, 100));
			NavMeshHit hit;
			NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas);
			position = hit.position;
			this.gameObject.transform.position = position;
		}
	}
}