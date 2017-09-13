using System.Collections;
using System.Collections.Generic;
using Assets.Gamelogic.Player;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinmanDrunkVisualiser : MonoBehaviour {

    [Require]
    private BinmanInfo.Reader binmanInfoReader;

    [Require]
    private ClientAuthorityCheck.Writer authorityCheck;

    private void OnEnable()
    {
        binmanInfoReader.IsDrunkUpdated.Add(IsDrunkChanged);
    }

	private void OnDisable()
	{
		binmanInfoReader.IsDrunkUpdated.Remove(IsDrunkChanged);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "BinJuice")
		{
			other.gameObject.SetActive(false);
		}
	}

    private void IsDrunkChanged(bool isDrunk){
        Debug.Log("IS DRUNK: " + isDrunk);
        GetComponent<ThirdPersonPlayerControls>().SetIsDrunk(isDrunk);
    }
}
