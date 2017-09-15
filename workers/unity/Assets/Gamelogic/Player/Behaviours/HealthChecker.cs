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
using Assets.Gamelogic.Utils;
using UnityEngine.UI;

namespace Assets.Gamelogic.Player.Behaviours
{
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
		private void OnCurrentHealthUpdated(int currentHealth)
		{
			if (currentHealth <= 0)
			{
				Respawn ();
			}
		}
			
		private void Respawn() 
		{	
			Vector3 position;
			if (this.gameObject.GetComponent<BinbagClientNPCInfo>().IsNPCBinBag())
			{
				position = PositionUtils.GetRandomPosition();
			} 
			else
			{
				// Place bin bag above ground somewhere. 
				position = new Vector3(0f,9000f, 0f);
			}

			this.gameObject.transform.position = position;

			SpatialOS.Commands.SendCommand (BinbagInfoWriter, PlayerMovement.Commands.Respawn.Descriptor, new SpawnPosition (position.ToSpatialVector3d ()), this.gameObject.EntityId ())
				.OnSuccess (OnSpawnSuccess)
				.OnFailure (errorDetails => Debug.LogWarning ("Failed to respawn with error: " + errorDetails.ErrorMessage));
		}

		private void OnSpawnSuccess(MovementResponse id) {
			BinbagInfoWriter.Send (new BinbagInfo.Update ().SetHealth (10).SetSize (0));
			this.transform.FindChild ("Model").gameObject.SetActive (true);
		}
	}
}