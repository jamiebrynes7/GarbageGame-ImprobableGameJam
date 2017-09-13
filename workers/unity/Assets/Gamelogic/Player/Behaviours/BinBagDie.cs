using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;
using Improbable.Environment;


[WorkerType(WorkerPlatform.UnityWorker)]
public class BinBagDie : MonoBehaviour
{
	[Require] private BinbagInfo.Writer BinbagInfoWriter;
	float timestamp;

	public void Start() {
		timestamp = Time.time;
	}


	private void OnTriggerEnter(Collider collision){
		if (BinbagInfoWriter == null)
			return;
		if (BinbagInfoWriter.Data.health <= 0)
			return;
		if (collision != null && collision.gameObject.tag == "Binman")
		{
			if (Time.time - timestamp < 1)
				return;
			if (BinbagInfoWriter.Data.health < 0)
				return;
			uint newHealth = BinbagInfoWriter.Data.health - 10;
			Debug.LogWarning ("HIT " + newHealth);

			BinbagInfoWriter.Send (new BinbagInfo.Update().SetHealth(newHealth));
			timestamp = Time.time;
			GameObject scoreTracker = GameObject.FindGameObjectWithTag ("ScoreTracker");
			Debug.LogWarning (scoreTracker);
			if (scoreTracker != null) {
				SpatialOS.Commands.SendCommand (BinbagInfoWriter, Score.Commands.AwardBinmanPoints.Descriptor, new AwardPoints (BinbagInfoWriter.Data.size + 1), scoreTracker.EntityId ())
					.OnSuccess (result => Debug.LogWarning ("Awarded points to Binmen"))
					.OnFailure (errorDetails => Debug.LogWarning ("Failed to award points with error: " + errorDetails.ErrorMessage));
			}
		}
			
	}
}

