using System.Collections;
using System.Collections.Generic;
using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinbagSizeVisualiser : MonoBehaviour {

    private float minSize = 1f;
    private float rubbishAdd = 0.1f;

    [Require]
    private BinbagInfo.Reader binbagInfoReader;

    [SerializeField]
    private Transform bagTransform;

    private void OnEnable()
    {
        binbagInfoReader.SizeUpdated.AddAndInvoke(SizeUpdated);
    }

	private void OnDisable()
	{
		binbagInfoReader.SizeUpdated.Remove(SizeUpdated);
	}

    private void SizeUpdated(uint size){
        var scale = minSize + (size * rubbishAdd);
        if(scale > 5){
            scale = 5;
        }
        bagTransform.localScale = Vector3.one * scale;
    }

}
