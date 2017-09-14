using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Improbable.Unity;
using Improbable.Unity.Visualizer;

using Improbable.Player;
using Improbable.Core;

[WorkerType(WorkerPlatform.UnityClient)]
public class HealthGuiController : MonoBehaviour {

	private Slider slider;

	[Require] private BinbagInfo.Reader BinbagInfoReader;
	[Require] private ClientAuthorityCheck.Writer ClientAuthorityCheckWriter;

	private void OnEnable()
	{
		BinbagInfoReader.HealthUpdated.Add(OnHealthUpdated);
		GameObject c = GameObject.Find("Canvas");
		GameObject sliderContainer = c.transform.Find("HealthDisplay").gameObject;
		sliderContainer.SetActive(true);
		slider = sliderContainer.GetComponent<Slider>();
	}

	private void OnDisable()
	{
		BinbagInfoReader.HealthUpdated.Add(OnHealthUpdated);
	}

	private void OnHealthUpdated(int health)
	{
		float normValue = Mathf.Clamp01(health/10f);
		if (slider != null)
		{
			slider.normalizedValue = normValue;
		}
	}
}
