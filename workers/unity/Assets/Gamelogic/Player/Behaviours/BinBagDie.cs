using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;

namespace Assets.Gamelogic.Pirates.Behaviours
{
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class BinBagDie : MonoBehaviour
	{
		[Require] private BinbagInfo.Writer BinbagInfoWriter;

		private void OnTriggerEnter(Collider other)
		{
			if (BinbagInfoWriter == null)
				return;

			if (BinbagInfoWriter.Data.health <= 0)
				return;
			if (other != null && other.gameObject.name == "Binman")
			{
				Debug.Log ("HIT");
				uint newHealth = BinbagInfoWriter.Data.health - 1;
				BinbagInfoWriter.Send (new BinbagInfo.Update().SetHealth(newHealth));
			}
		}
	}
}
