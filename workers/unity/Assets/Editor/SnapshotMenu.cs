﻿using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityTemplates;
using Improbable;
using Improbable.Worker;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Assets.Gamelogic.Utils;

namespace Assets.Editor 
{
	public class SnapshotMenu : MonoBehaviour
	{
		[MenuItem("Improbable/Snapshots/Generate Default Snapshot")]
		[UsedImplicitly]
		private static void GenerateDefaultSnapshot()
		{
			var snapshotEntities = new Dictionary<EntityId, Entity>();
			var currentEntityId = 1;

			snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreatePlayerCreatorTemplate());
			//snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateCubeTemplate());
			snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateWorldTrackerTemplate());

			for (var i = 0; i < 40; i++)
			{
				var position = PositionUtils.GetRandomPosition();
				position.y = 0;
				snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateBinbagNPCTemplate(position));
			}

			for (var i = 0; i < 40; i++)
			{
				var position = PositionUtils.GetRandomPosition();
				position.y = 0;
                //position = Vector3.zero;
				snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateBinmanNPCTemplate(position));
			}

			for (var i = 0; i < 0; i++)
			{
                uint rubbishIndex = (uint) Random.Range(0, RubbishChooser.RUBBISH_NUM);
				snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateRubbishTemplate(Vector3.forward * 3 * i, rubbishIndex));
			}

			for (var i = 0; i < 0; i++)
			{
				snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateBinJuiceTemplate(Vector3.forward * 3 * i));
			}

			for (var i = 0; i < 0; i++)
			{
				snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateStoneTemplate(Vector3.forward * 3 * i));
			}

			for (var i = 0; i < 5; i++)
			{
                var position = PositionUtils.GetRandomPosition();
                position.y = 0;
                snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateRubbishTipTemplate(position));
			}

			SaveSnapshot(snapshotEntities);
		}

		private static void SaveSnapshot(IDictionary<EntityId, Entity> snapshotEntities)
		{
			File.Delete(SimulationSettings.DefaultSnapshotPath);
			var maybeError = Snapshot.Save(SimulationSettings.DefaultSnapshotPath, snapshotEntities);

			if (maybeError.HasValue)
			{
				Debug.LogErrorFormat("Failed to generate initial world snapshot: {0}", maybeError.Value);
			}
			else
			{
				Debug.LogFormat("Successfully generated initial world snapshot at {0}", SimulationSettings.DefaultSnapshotPath);
			}
		}
	}
}