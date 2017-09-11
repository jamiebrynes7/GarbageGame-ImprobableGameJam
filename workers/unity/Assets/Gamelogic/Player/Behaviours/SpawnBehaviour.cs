using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Gamelogic.Player.Behaviours
{
	// Add this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityClient)]
	public class SpawnBehaviour : MonoBehaviour
	{
		// Inject access to the entity's Health component
		[Require] private BinbagInfo.Reader binbagInfoReader;

		public Animation SinkingAnimation;
		public GameObject ShipWashVfx;

		private bool alreadySunk = false;

		private void OnEnable()
		{
			alreadySunk = false;
			binbagInfoReader.HealthUpdated.Add(OnCurrentHealthUpdated);
		}

		private void OnDisable()
		{
			binbagInfoReader.HealthUpdated.Remove(OnCurrentHealthUpdated);
		}

		// Callback for whenever the CurrentHealth property of the Health component is updated
		private void OnCurrentHealthUpdated(uint currentHealth)
		{
			if (currentHealth <= 0)
			{
				Respawn ();
			}
		}

		private void Respawn()
		{
			Vector3 position = new Vector3(Random.Range(-200,200), 0, Random.Range(-200, 200));
			NavMeshHit hit;
			NavMesh.SamplePosition(position, out hit, 10, 1);
			position = hit.position;
			this.transform.position = position;
		}
	}
}