using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;
using Improbable.Environment;
using Improbable.Entity.Component;


[WorkerType(WorkerPlatform.UnityWorker)]
public class BinBagDie : MonoBehaviour
{
	[Require] private BinbagInfo.Writer BinbagInfoWriter;
	[Require] private BinbagVisuals.Reader BinbagVisualsReader;

	float timestamp;
	bool isColliding = false;

	public void Start() {
		timestamp = Time.time;
		isColliding = false;
	}

	void Update() {
		isColliding = false;
	}


	private void OnTriggerEnter(Collider collision){
		if (isColliding)
			return;
		isColliding = true;

		if (BinbagInfoWriter == null)
			return;
		if (BinbagInfoWriter.Data.health <= 0)
			return;
		if (collision != null && collision.gameObject.tag == "Binman")
		{
			if (Time.time - timestamp < 1)
				return;
			timestamp = Time.time;

			BinmanCollision ();

		}
		if (collision != null && collision.gameObject.tag == "RubbishTipWtf")
		{
			// We want to award points to the binbags and increment the counter on the tips.
			// Also want to respawn player.


			// Force player to respawn
			int newHealth = BinbagInfoWriter.Data.health - 10;
			BinbagInfoWriter.Send( new BinbagInfo.Update().SetHealth(newHealth));

			// Give points to the binbags.
			GameObject scoreTracker = GameObject.FindGameObjectWithTag ("ScoreTracker");
			if (scoreTracker != null) {
				SpatialOS.Commands.SendCommand(BinbagInfoWriter, Score.Commands.AwardBinbagPoints.Descriptor, new AwardPoints(BinbagInfoWriter.Data.size + 1), scoreTracker.EntityId())
					.OnSuccess( result => Debug.LogWarning("Awarded points to the binbags."))
					.OnFailure( errorDetails => Debug.LogWarning("Failed to award points with error: " + errorDetails.ErrorMessage));
			}

			// Increment counter on the tip
			SpatialOS.Commands.SendCommand(BinbagInfoWriter, RubbishTipInfo.Commands.IncrementTip.Descriptor, new IncrementTipRequest(), collision.gameObject.EntityId())
				.OnSuccess( result => Debug.LogWarning("Incremented tip"))
				.OnFailure( errorDetails => Debug.LogWarning("Failed to increment tip."));

		}
	}

	private void OnEnable() {
		BinbagInfoWriter.CommandReceiver.OnBinmanTriggered.RegisterResponse (OnPickedUpTriggered);
	}

	private void OnDisable() {
		BinbagInfoWriter.CommandReceiver.OnBinmanTriggered.DeregisterResponse ();
	}

	private TriggerResponse OnPickedUpTriggered(TriggerData args, ICommandCallerInfo callerInfo) {
		Debug.Log ("STILL COLLIDING: " + isColliding);
		if (Vector3.Distance (this.transform.position, args.position.ToUnityVector()) < 4f && !isColliding) {
			BinmanCollision ();
		}
		return new TriggerResponse (args.position);
	}


	private void BinmanCollision() {
		Debug.LogWarning ("COLLIDING");
		int newHealth = BinbagInfoWriter.Data.health - 10;
		BinbagInfoWriter.Send (new BinbagInfo.Update().SetHealth(newHealth));
		GameObject scoreTracker = GameObject.FindGameObjectWithTag ("ScoreTracker");
		if (scoreTracker != null) {
			SpatialOS.Commands.SendCommand (BinbagInfoWriter, Score.Commands.AwardBinmanPoints.Descriptor, new AwardPoints (BinbagInfoWriter.Data.size + 1), scoreTracker.EntityId ())
				.OnSuccess (result => Debug.LogWarning ("Awarded points to Binmen"))
				.OnFailure (errorDetails => Debug.LogWarning ("Failed to award points with error: " + errorDetails.ErrorMessage));
		}
	}
}

