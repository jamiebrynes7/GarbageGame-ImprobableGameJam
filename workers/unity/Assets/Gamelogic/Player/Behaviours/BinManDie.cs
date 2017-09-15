using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable;
using Improbable.Core;
using Improbable.Environment;
using Improbable.Entity.Component;
using UnityEngine.UI;
using Assets.Gamelogic.Utils;
using System.Collections;
using System.Collections.Generic;


[WorkerType(WorkerPlatform.UnityClient)]
public class BinManDie : MonoBehaviour
{
	float isFiredTimer;
	float firedThreshold = 600;
	int nofBinbags = 5;
	Queue<float> lastBinbags = new Queue<float>();


	public GameObject firedCanvas;

	public void Start() {
		isFiredTimer = 0;
		firedCanvas = GameObject.Find("Canvas");
		lastBinbags.Enqueue (Time.time);
		isColliding = false;
	}

	bool isColliding = false;

	private void OnTriggerEnter(Collider collision){
		if (isColliding)
			return;
		isColliding = true;
		if (collision.tag == "Binbag") {
			if (lastBinbags.Count > nofBinbags)
				lastBinbags.Dequeue ();
			lastBinbags.Enqueue (Time.time);
		}
	}

	void Update() {
		isColliding = false;
		if (firedCanvas == null)
			return;
		
		if ((Time.time - lastBinbags.Peek()) >= firedThreshold && !firedCanvas.GetComponent<RawImage> ().enabled) {
			ToggleScreens (true);
			this.transform.position = PositionUtils.GetRandomPosition();
		} else if (firedCanvas.GetComponent<RawImage> ().enabled) {
			if (isFiredTimer < 3) {
				isFiredTimer += Time.deltaTime;
			} else if (firedCanvas.activeSelf) {
				ToggleScreens(false);
				isFiredTimer = 0;
				lastBinbags.Clear ();
				lastBinbags.Enqueue (Time.time);
			}
		}
	}

	private void ToggleScreens(bool isFired) {
		firedCanvas.GetComponent<RawImage> ().enabled = isFired;
		for (int i = 0; i < firedCanvas.transform.childCount; i++) {
			firedCanvas.transform.GetChild (i).gameObject.SetActive(!isFired);
		}
	}
}


