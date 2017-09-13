using Improbable.Entity.Component;
using Improbable.Environment;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Pirates.Behaviours
{
	// Add this MonoBehaviour on UnityWorker (server-side) workers only
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class ScoreTracker : MonoBehaviour
	{
		/*
         * An entity with this MonoBehaviour will only be enabled for the single UnityWorker
         * which has write access for its Score component.
         */
		[Require] private Score.Writer ScoreWriter;

		void OnEnable()
		{
			// Register command callback
			ScoreWriter.CommandReceiver.OnAwardBinbagPoints.RegisterResponse(OnAwardBinbagPoints);
			ScoreWriter.CommandReceiver.OnAwardBinmanPoints.RegisterResponse(OnAwardBinmanPoints);
		}

		private void OnDisable()
		{
			// Deregister command callbacks
			ScoreWriter.CommandReceiver.OnAwardBinbagPoints.DeregisterResponse();
			ScoreWriter.CommandReceiver.OnAwardBinmanPoints.DeregisterResponse();
		}

		// Command callback for handling points awarded by other entities when they sink
		private AwardResponse OnAwardBinmanPoints(AwardPoints request, ICommandCallerInfo callerInfo)
		{
			Debug.LogWarning ("Awarding bin man points");
			uint newScore = ScoreWriter.Data.binmanScore + (uint)request.amount;
			ScoreWriter.Send(new Score.Update().SetBinmanScore(newScore));
			// Acknowledge command receipt
			return new AwardResponse(request.amount);
		}

		// Command callback for handling points awarded by other entities when they sink
		private AwardResponse OnAwardBinbagPoints(AwardPoints request, ICommandCallerInfo callerInfo)
		{
			uint newScore = ScoreWriter.Data.binbagScore + (uint)request.amount;
			ScoreWriter.Send(new Score.Update().SetBinbagScore(newScore));
			// Acknowledge command receipt
			return new AwardResponse(request.amount);
		}
	}
}