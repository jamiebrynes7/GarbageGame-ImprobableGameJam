using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;
using UnityEngine.AI;
using Improbable.Unity.Common.Core.Math;



namespace Assets.Gamelogic.Player.Behaviours
{
	// Add this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class HealthChecker : MonoBehaviour
	{
		[Require] private BinbagInfo.Writer BinbagInfoWriter;

		private void OnEnable()
		{
			BinbagInfoWriter.HealthUpdated.Add(OnCurrentHealthUpdated);
		}

		private void OnDisable()
		{
			BinbagInfoWriter.HealthUpdated.Remove(OnCurrentHealthUpdated);
		}


		// Callback for whenever the CurrentHealth property of the Health component is updated
		private void OnCurrentHealthUpdated(uint currentHealth)
		{
			if (currentHealth <= 0)
			{
				Respawn ();
			}
		}

		private static void OnSuccessfulPlayerCreation(EntityId id, BinbagInfo.Writer writer) {
			SpatialOS.Commands.DeleteEntity(writer, id)
				.OnSuccess(entityId => Debug.LogWarning("Deleted entity: " + entityId))
				.OnFailure(errorDetails => Debug.LogWarning("Failed to delete entity with error: " + errorDetails.ErrorMessage));
		}

		private void Respawn() {
			Vector3 position = new Vector3(Random.Range(-100, 100), 2, Random.Range(-100, 100));
			NavMeshHit hit;
			NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas);
			position = hit.position;
			this.gameObject.transform.position = position;
			Debug.LogWarning (position);
			var clientInfo = GetComponent<BinbagClientNPCInfo>();
			if (clientInfo == null || !clientInfo.IsNPCBinBag ()) {
				SpatialOS.Commands.SendCommand (BinbagInfoWriter, PlayerMovement.Commands.Respawn.Descriptor, new SpawnPosition (position.ToSpatialVector3d ()), this.gameObject.EntityId ())
					.OnSuccess (entityId => BinbagInfoWriter.Send (new BinbagInfo.Update ().SetHealth (10).SetSize (0)))
					.OnFailure (errorDetails => Debug.LogWarning ("Failed to respawn with error: " + errorDetails.ErrorMessage));
			} else {
				BinbagInfoWriter.Send (new BinbagInfo.Update ().SetHealth (10).SetSize (0));
			}
		}
	}
}