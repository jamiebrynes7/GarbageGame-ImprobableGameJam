using Improbable.Unity.Entity;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using UnityEngine.UI;
using Improbable.Environment;

namespace Assets.Gamelogic.World
{
	// Add this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityClient)]
	public class ScoreGUI : MonoBehaviour
	{
		[Require] private Score.Reader ScoreReader;

		private Text binbagTotalPointsGUI;
		private Text binmanTotalPointsGUI;

		private void Start()
		{
			binbagTotalPointsGUI = GameObject.Find ("BinbagScore").GetComponent<Text>();
			binmanTotalPointsGUI = GameObject.Find ("BinmanScore").GetComponent<Text>();
			updateBinmanGUI(ScoreReader.Data.binmanScore);
			updateBinbagGUI (ScoreReader.Data.binbagScore);
		}

		private void OnEnable()
		{
			// Register callback for when components change
			ScoreReader.BinmanScoreUpdated.Add(BinmanScoreUpdated);
			ScoreReader.BinbagScoreUpdated.Add(BinbagScoreUpdated);

		}

		private void OnDisable()
		{
			// Deregister callback for when components change
			ScoreReader.BinmanScoreUpdated.Remove(BinmanScoreUpdated);
			ScoreReader.BinbagScoreUpdated.Remove(BinbagScoreUpdated);
		}

		private void Initialize() {
			var binbagScore = GameObject.Find ("BinbagScore");
			if (binbagScore != null)
				binbagTotalPointsGUI = binbagScore.GetComponent<Text>();
			var binmanScore = GameObject.Find ("BinmanScore");
			if (binmanScore != null)
				binmanTotalPointsGUI = GameObject.Find ("BinmanScore").GetComponent<Text>();
		}

		// Callback for whenever one or more property of the Score component is updated
		private void BinmanScoreUpdated(uint numberOfPoints)
		{
			updateBinmanGUI(numberOfPoints);
		}

		private void BinbagScoreUpdated(uint numberOfPoints)
		{
			updateBinbagGUI(numberOfPoints);
		}

		void updateBinmanGUI(uint score)
		{
			if (binmanTotalPointsGUI == null)
				Initialize ();
			if (binmanTotalPointsGUI != null)
				binmanTotalPointsGUI.text = score.ToString();
		}

		void updateBinbagGUI(uint score)
		{
			if (binbagTotalPointsGUI == null)
				Initialize ();
			if (binbagTotalPointsGUI != null)
				binbagTotalPointsGUI.text = score.ToString();
		}
	}
}