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
			updateBinmanGUI(0);
			updateBinbagGUI (0);
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

		// Callback for whenever one or more property of the Score component is updated
		private void BinmanScoreUpdated(uint numberOfPoints)
		{
			Debug.Log ("bin man score got updated" + numberOfPoints);
			updateBinmanGUI(numberOfPoints);
		}

		private void BinbagScoreUpdated(uint numberOfPoints)
		{
			Debug.Log ("bin bag score got updated" + numberOfPoints);
			updateBinbagGUI(numberOfPoints);
		}

		void updateBinmanGUI(uint score)
		{
			binmanTotalPointsGUI.text = score.ToString();
		}

		void updateBinbagGUI(uint score)
		{
			binbagTotalPointsGUI.text = score.ToString();
		}
	}
}