using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;


[WorkerType(WorkerPlatform.UnityWorker)]
public class BinBagDie : MonoBehaviour
{
	[Require] private BinbagInfo.Writer BinbagInfoWriter;

	private void OnTriggerEnter(Collider collision){
		if (BinbagInfoWriter == null)
			return;
		if (BinbagInfoWriter.Data.health <= 0)
			return;
		Debug.LogWarning(collision.gameObject.name);
		if (collision != null && collision.gameObject.tag == "Binman")
		{
			Debug.LogWarning ("HIT");
			uint newHealth = BinbagInfoWriter.Data.health - 1;
			BinbagInfoWriter.Send (new BinbagInfo.Update().SetHealth(newHealth));
		}
	}
}

