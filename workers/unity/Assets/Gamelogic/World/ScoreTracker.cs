using Improbable.Entity.Component;
using Improbable.Environment;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Pirates.Behaviours
{
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class ScoreTracker : MonoBehaviour
	{
		[Require] private Score.Writer ScoreWriter;

		void OnEnable()
		{
			ScoreWriter.CommandReceiver.OnAwardBinbagPoints.RegisterResponse(OnAwardBinbagPoints);
			ScoreWriter.CommandReceiver.OnAwardBinmanPoints.RegisterResponse(OnAwardBinmanPoints);
		}

		private void OnDisable()
		{
			ScoreWriter.CommandReceiver.OnAwardBinbagPoints.DeregisterResponse();
			ScoreWriter.CommandReceiver.OnAwardBinmanPoints.DeregisterResponse();
		}

		private AwardResponse OnAwardBinmanPoints(AwardPoints request, ICommandCallerInfo callerInfo)
		{
			uint newScore = ScoreWriter.Data.binmanScore + (uint)request.amount;
			ScoreWriter.Send(new Score.Update().SetBinmanScore(newScore));
			return new AwardResponse(request.amount);
		}

		private AwardResponse OnAwardBinbagPoints(AwardPoints request, ICommandCallerInfo callerInfo)
		{
			uint newScore = ScoreWriter.Data.binbagScore + (uint)request.amount;
			ScoreWriter.Send(new Score.Update().SetBinbagScore(newScore));
			return new AwardResponse(request.amount);
		}
	}
}