using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Core;
using Improbable.Misc;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using Improbable.Unity.Entity;
using Improbable.Worker;
using UnityEngine;

namespace Assets.Gamelogic.EntityTemplates
{
	public class EntityTemplateFactory : MonoBehaviour
	{
		public static Entity CreatePlayerCreatorTemplate()
		{
			var template = EntityBuilder.Begin()
				.AddPositionComponent(Vector3.zero, CommonRequirementSets.PhysicsOnly)
				.AddMetadataComponent(SimulationSettings.PlayerCreatorPrefabName)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new PlayerCreation.Data(), CommonRequirementSets.PhysicsOnly)
				.Build();

			return template;
		}

		public static Entity CreatePlayerTemplate(string clientId)
		{
			var template = EntityBuilder.Begin()
				.AddPositionComponent(Vector3.zero, CommonRequirementSets.SpecificClientOnly(clientId))
				.AddMetadataComponent(SimulationSettings.PlayerPrefabName)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new ClientAuthorityCheck.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new ClientConnection.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new PlayerRotation.Data(yaw: 0), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new PlayerMovement.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.Build();

			return template;
		}

		public static Entity CreateBinbagTemplate(string clientId)
		{
			var template = EntityBuilder.Begin()
				.AddPositionComponent(Vector3.zero, CommonRequirementSets.SpecificClientOnly(clientId))
				.AddMetadataComponent(SimulationSettings.BinbagPrefabName)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new ClientAuthorityCheck.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new ClientConnection.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new PlayerRotation.Data(yaw: 0), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new PlayerMovement.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new BinbagVisuals.Data(Lean.NONE), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new BinbagInfo.Data(10, 0), CommonRequirementSets.PhysicsOnly)
				.Build();

			return template;
		}

		public static Entity CreateBinbagNPCTemplate(Vector3 position)
		{
			var template = EntityBuilder.Begin()
				.AddPositionComponent(Vector3.zero, CommonRequirementSets.PhysicsOnly)
				.AddMetadataComponent(SimulationSettings.BinbagPrefabName)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new PlayerRotation.Data(yaw: 0), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new PlayerMovement.Data(), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new BinbagVisuals.Data(Lean.NONE), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new BinbagInfo.Data(10, 0), CommonRequirementSets.PhysicsOnly)
				.Build();

			return template;
		}

		public static Entity CreateStoneTemplate(Vector3 position)
		{
			var template = EntityBuilder.Begin()
				.AddPositionComponent(position, CommonRequirementSets.PhysicsOnly)
				.AddMetadataComponent("Stone")
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.Build();

			return template;
		}

		public static Entity CreateBinmanTemplate(string clientId)
		{
			var template = EntityBuilder.Begin()
				.AddPositionComponent(Vector3.zero, CommonRequirementSets.SpecificClientOnly(clientId))
				.AddMetadataComponent(SimulationSettings.BinmanPrefabName)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new ClientAuthorityCheck.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new ClientConnection.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new PlayerRotation.Data(yaw: 0), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new PlayerMovement.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.Build();

			return template;
		}

        public static Entity CreateRubbishTemplate(Vector3 position, uint rubbishIndex){
            var template = EntityBuilder.Begin()
                .AddPositionComponent(position, CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent("Rubbish")
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new RubbishInfo.Data(rubbishIndex), CommonRequirementSets.PhysicsOnly)
				.Build();

			return template;
        }

        public static Entity CreateCubeTemplate()
        {
            var template = EntityBuilder.Begin()
                .AddPositionComponent(new Vector3(0,1,5), CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent(SimulationSettings.CubePrefabName)
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .Build();

			return template;
		}
	}
}