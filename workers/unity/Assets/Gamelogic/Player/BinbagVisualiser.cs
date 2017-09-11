using System.Collections;
using System.Collections.Generic;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityClient)]
public class BinbagVisualiser : MonoBehaviour {

    private static float LEAN_ANGLE = 15f;
    private static float LEAN_SPEED = 70f;

    private static float SPIN_SPEED = 700f;

    [Require]
    private BinbagVisuals.Reader binbagVisualsReader;

    public Transform modelTransform;

    public Transform spinTransform;

    private Quaternion targetLean = Quaternion.identity;

    private void OnEnable()
    {
        binbagVisualsReader.BagLeanUpdated.AddAndInvoke(LeanUpdated);
    }

	private void OnDisable()
	{
		binbagVisualsReader.BagLeanUpdated.Remove(LeanUpdated);
	}

    private void Update()
    {
        modelTransform.localRotation = Quaternion.RotateTowards(modelTransform.localRotation, targetLean, LEAN_SPEED * Time.deltaTime);
        spinTransform.Rotate(Vector3.right * Time.deltaTime * SPIN_SPEED);
    }

    private void LeanUpdated(Lean lean){
        float leanAngle = LEAN_ANGLE;
        switch(lean){
            case Lean.RIGHT:
                leanAngle *= -1f;
                break;
            case Lean.NONE:
                leanAngle = 0f;
                break;
        }
        targetLean = Quaternion.Euler(0f, 0f, leanAngle);
    }
}
