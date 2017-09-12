using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;


namespace Assets.Gamelogic.Player.Behaviours
{
	// Add this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class HealthChecker : MonoBehaviour
	{
		[Require] private BinbagInfo.Writer BinbagInfoWriter;
		[Require]
		private ClientAuthorityCheck.Reader authorityCheck;

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
				var id = gameObject.EntityId();
				var clientInfo = GetComponent<BinbagClientNPCInfo>();
				if (clientInfo != null && clientInfo.IsNPCBinBag ()) {
					Debug.LogWarning ("NPC");
					var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateBinbagNPCTemplate (new Vector3(Random.Range(-100, 100), 1, Random.Range(-100, 100)));
					SpatialOS.Commands.CreateEntity(BinbagInfoWriter, entityTemplate)
						.OnSuccess(result => OnSuccessfulPlayerCreation(id, BinbagInfoWriter))
						.OnFailure(errorDetails => Debug.LogWarning("Failed to delete entity with error: " + errorDetails.ErrorMessage));
				} else {
					Debug.LogWarning ("Player: " + authorityCheck.Data.clientid);
					//Bootstrap.CreatePlayer (true);
					var entityTemplate = Assets.Gamelogic.EntityTemplates.EntityTemplateFactory.CreateBinbagTemplate (authorityCheck.Data.clientid);
					SpatialOS.Commands.CreateEntity(BinbagInfoWriter, entityTemplate)
						.OnSuccess(result => OnSuccessfulPlayerCreation(id, BinbagInfoWriter))
						.OnFailure(errorDetails => Debug.LogWarning("Failed to delete entity with error: " + errorDetails.ErrorMessage));

				}
			}
		}

		private static void OnSuccessfulPlayerCreation(EntityId id, BinbagInfo.Writer writer) {
			Debug.LogWarning ("Created new entity");
			SpatialOS.Commands.DeleteEntity(writer, id)
				.OnSuccess(entityId => Debug.LogWarning("Deleted entity: " + entityId))
				.OnFailure(errorDetails => Debug.LogWarning("Failed to delete entity with error: " + errorDetails.ErrorMessage));
		}
	}
}