using Assets.Gamelogic.EntityTemplates;
using Improbable;
using Improbable.Entity.Component;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Worker;

namespace Assets.Gamelogic.Core
{
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class PlayerCreatingBehaviour : MonoBehaviour
	{
		[Require]
		private PlayerCreation.Writer PlayerCreationWriter;

		private void OnEnable()
		{
			PlayerCreationWriter.CommandReceiver.OnCreatePlayer.RegisterResponse(OnCreatePlayer);
		}

		private void OnDisable()
		{
			PlayerCreationWriter.CommandReceiver.OnCreatePlayer.DeregisterResponse();
		}

		private CreatePlayerResponse OnCreatePlayer(CreatePlayerRequest request, ICommandCallerInfo callerinfo)
		{
			CreatePlayerWithReservedId(callerinfo.CallerWorkerId, request.isBinBag);
			return new CreatePlayerResponse();
		}

		private void CreatePlayerWithReservedId(string clientWorkerId, bool isBinBag)
		{
			SpatialOS.Commands.ReserveEntityId(PlayerCreationWriter)
				.OnSuccess(reservedEntityId => CreatePlayer(clientWorkerId, reservedEntityId.ReservedEntityId, isBinBag))
				.OnFailure(failure => OnFailedReservation(failure, clientWorkerId, isBinBag));
		}

		private void OnFailedReservation(ICommandErrorDetails response, string clientWorkerId, bool isBinBag)
		{
			Debug.LogError("Failed to Reserve EntityId for Player: " + response.ErrorMessage + ". Retrying...");
			CreatePlayerWithReservedId(clientWorkerId, isBinBag);
		}

		private void CreatePlayer(string clientWorkerId, EntityId entityId, bool isBinBag)
		{
			Entity playerEntityTemplate;
			if(isBinBag){
				playerEntityTemplate = EntityTemplateFactory.CreateBinbagTemplate(clientWorkerId);
			}else{
				playerEntityTemplate = EntityTemplateFactory.CreateBinmanTemplate(clientWorkerId);
			}
			SpatialOS.WorkerCommands.CreateEntity(entityId, playerEntityTemplate)
				.OnFailure(failure => OnFailedPlayerCreation(failure, clientWorkerId, entityId, isBinBag));
		}

		private void OnFailedPlayerCreation(ICommandErrorDetails response, string clientWorkerId, EntityId entityId, bool isBinBag)
		{
			Debug.LogError("Failed to Create Player Entity: " + response.ErrorMessage + ". Retrying...");
			CreatePlayer(clientWorkerId, entityId, isBinBag);
		}
	}
}