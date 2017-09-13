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

[WorkerType(WorkerPlatform.UnityWorker)]
public class BinbagNPCController : MonoBehaviour {

    private static float UPDATE_INTERVAL = 0.5f;

	[Require] private Position.Writer positionWriter;
	[Require] private PlayerRotation.Writer playerRotationWriter;
	[Require]
    private PlayerMovement.Writer playerMovementWriter;

    public NavMeshAgent navMeshAgent;
    public Rigidbody rigidBody;

    private float lastUpdateTime;

    private void OnEnable()
    {
        navMeshAgent.enabled = true;
        rigidBody.isKinematic = true;
        UpdateTarget();
    }

    private void OnDisable()
    {
        navMeshAgent.enabled = false;
    }

    private void Update()
    {
        float dist = navMeshAgent.remainingDistance;
        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete
            && System.Math.Abs(navMeshAgent.remainingDistance) < 0.5f)
        {
            UpdateTarget();
        }

        UpdatePlayerControls();
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
        navMeshAgent.destination = randomPoint;
    }

}
