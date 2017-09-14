using System.Collections;
using System.Collections.Generic;
using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;
using UnityEngine.AI;
using Improbable.Entity.Component;
using Improbable.Collections;

[WorkerType(WorkerPlatform.UnityWorker)]
public class BinbagNPCController : MonoBehaviour {

    private static float UPDATE_INTERVAL = 0.5f;

	[Require] private Position.Writer positionWriter;
	[Require] private PlayerRotation.Writer playerRotationWriter;
	[Require]
    private PlayerMovement.Writer playerMovementWriter;

	[Require]
	private NPCInfo.Writer npcInfoWriter;

    public NavMeshAgent navMeshAgent;
    public Rigidbody rigidBody;

    private float lastUpdateTime;

    private void OnEnable()
    {
        navMeshAgent.enabled = true;
        rigidBody.isKinematic = true;
		if (npcInfoWriter.Data.destination.HasValue)
		{
			Vector3 position = npcInfoWriter.Data.destination.Value.ToUnityVector();
			navMeshAgent.destination = position;
		}
		else
		{
			UpdateTarget();
		}
		playerMovementWriter.CommandReceiver.OnRespawn.RegisterResponse (OnRespawn);
    }

    private void OnDisable()
    {
        navMeshAgent.enabled = false;
		playerMovementWriter.CommandReceiver.OnRespawn.DeregisterResponse ();
    }

    private void Update()
    {
        float dist = navMeshAgent.remainingDistance;
        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete
            && System.Math.Abs(navMeshAgent.remainingDistance) < 5f)
        {
            UpdateTarget();
        }

        UpdatePlayerControls();
	}

	private MovementResponse OnRespawn(SpawnPosition position, ICommandCallerInfo callerInfo) {
		rigidBody.MovePosition(position.position.ToUnityVector());
		return new MovementResponse (position.position);
	}


	private void UpdatePlayerControls()
	{
        var newTargetPosition = rigidBody.position;
        if ((Time.time - lastUpdateTime) > UPDATE_INTERVAL)
		{
			positionWriter.Send(new Position.Update().SetCoords(new Coordinates(newTargetPosition.x, 0, newTargetPosition.z)));
			playerMovementWriter.Send(new PlayerMovement.Update().AddMovementUpdate(new MovementUpdate(newTargetPosition.ToSpatialVector3d(), Time.time)));
			playerRotationWriter.Send(new PlayerRotation.Update().SetYaw(transform.eulerAngles.y)
									  .AddRotationUpdate(new RotationUpdate(transform.eulerAngles.y, Time.time)));
            lastUpdateTime = Time.time;
		}
	}

    private void UpdateTarget(){
        Vector3 randomPoint = PositionUtils.GetRandomPosition();
		npcInfoWriter.Send(new NPCInfo.Update().SetDestination(new Option<Coordinates>(randomPoint.ToSpatialCoordinates())));
        navMeshAgent.destination = randomPoint;
    }
}
