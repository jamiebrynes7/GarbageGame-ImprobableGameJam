using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Improbable.Unity.Visualizer;
using Improbable.Unity;

using Improbable.Player;


[WorkerType(WorkerPlatform.UnityClient)]
public class NameplateController : MonoBehaviour {

	[Require] private Nameplate.Reader NameplateReader;

	public GameObject NameplateTextHolder;

	private Camera camera;
	private TextMesh NameplateText;

	private void OnEnable()
	{
		NameplateTextHolder.SetActive(true);
		NameplateText = NameplateTextHolder.transform.Find("Nameplate").gameObject.GetComponent<TextMesh>() as TextMesh;

		camera = FindCamera();
		NameplateText.text = NameplateReader.Data.name;
	}

	private Camera FindCamera()
	{
		return Camera.main;
	}

	void LateUpdate()
	{
		if (camera != null) {
            // Only want to rotate along Y-axis
            Vector3 targetPos = NameplateTextHolder.transform.position + new Vector3(0f, camera.transform.position.y, -10f);
            NameplateTextHolder.transform.LookAt(targetPos);
		}
		else {
			camera = FindCamera();
		}
	}
}
