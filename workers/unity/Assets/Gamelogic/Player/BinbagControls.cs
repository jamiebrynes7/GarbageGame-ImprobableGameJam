using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Improbable.Entity.Component;
using Improbable.Unity.Common.Core.Math;

namespace Assets.Gamelogic.Player
{
	[WorkerType(WorkerPlatform.UnityClient)]
    public class BinbagControls : ThirdPersonPlayerControls
	{
        private static float ROTATION_SPEED = 0.08f;

        [Require]
        private BinbagVisuals.Writer binbagVisualsWriter;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Building"){
                //transform.rotation = Quaternion.Euler(0f, 180f, 0f) * transform.rotation;
                transform.Rotate(0f, 180f, 0f);
            }
        }

        override protected void UpdateDesiredMovementDirection()
        {
            float controlValue = Input.GetAxis("Horizontal");
            SetBinbagLean(controlValue);
            Vector3 inputDirection = new Vector3(controlValue * ROTATION_SPEED, 0, 1f);
            Vector3 movementDirection = (transform.rotation * inputDirection).FlattenVector().normalized;
            targetVelocity = movementDirection * GetMovementSpeed();
        }

        private float GetMovementSpeed(){
            return 10f;
        }

        private void SetBinbagLean(float controlValue){
            Lean leanVal = Lean.NONE;
            if (controlValue > 0.5f){
                leanVal = Lean.RIGHT;
            }else if (controlValue < -0.5f){
                leanVal = Lean.LEFT;
            }
            if(leanVal != binbagVisualsWriter.Data.bagLean){
                binbagVisualsWriter.Send(new BinbagVisuals.Update().SetBagLean(leanVal));
            }
        }
	}
}