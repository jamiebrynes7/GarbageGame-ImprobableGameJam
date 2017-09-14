using Improbable;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Gamelogic.Utils
{
	public static class PositionUtils {

		public static Vector3 GetRandomPosition()
		{
			Vector3 position = new Vector3(Random.Range(-200, 200), 2, Random.Range(-200, 200));
			NavMeshHit hit;
			NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas);
			position = hit.position;
			return position;
		}

	}

}
