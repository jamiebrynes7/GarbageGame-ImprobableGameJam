using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;


[WorkerType(WorkerPlatform.UnityWorker)]
public class BinBagDie : MonoBehaviour
{
	[Require] private BinbagInfo.Writer BinbagInfoWriter;

	private void OnTriggerEnter(Collider collision){
		if (BinbagInfoWriter == null)
			return;
		if (BinbagInfoWriter.Data.health <= 0)
			return;
		if (collision != null && collision.gameObject.tag == "Binman")
		{

			uint newHealth = BinbagInfoWriter.Data.health - 10;
			Debug.LogWarning ("HIT " + newHealth);

			BinbagInfoWriter.Send (new BinbagInfo.Update().SetHealth(newHealth));
		}
			
	}
}

