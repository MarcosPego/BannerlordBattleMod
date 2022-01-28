using Helpers;
using SandBox;
using SandBox.Source.Missions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace BannerlordBattleMod
{
    public class BattleMissionManager
    {
        [MissionMethod]
        public static Mission OpenBattleMission(string scene)
        {
            return OpenBattleMission(SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false));
        }

		[MissionMethod]
		public static Mission OpenBattleMission(MissionInitializerRecord rec)
		{
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
			bool isPlayerAttacker = !(from p in MobileParty.MainParty.MapEvent.AttackerSide.Parties
									  where p.Party == MobileParty.MainParty.Party
									  select p).IsEmpty<MapEventParty>();
			if (isPlayerInArmy)
			{
				bool flag = MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty;
			}
			return MissionState.OpenNew("Battle", rec, delegate (Mission mission)
			{
				MissionBehavior[] array = new MissionBehavior[25];
                array[0] = CreateCampaignMissionAgentSpawnLogic(false);
				array[1] = new BattleSpawnLogic("battle_set");
				array[2] = new BaseMissionTroopSpawnHandler();
				array[3] = new CampaignMissionComponent();
				array[4] = new BattleAgentLogic();
				array[5] = new MountAgentLogic();
				array[6] = new MissionOptionsComponent();
				array[7] = new BattleEndLogic();
				array[8] = new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
				array[9] = new BattleObserverMissionLogic();
				array[10] = new AgentHumanAILogic();
				array[11] = new AgentVictoryLogic();
				array[12] = new MissionAgentPanicHandler();
				array[13] = new BattleMissionAgentInteractionLogic();
				array[14] = new AgentMoraleInteractionLogic();
				array[15] = new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations);
				int num = 16;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				TextObject attackerGeneralName = (leaderHero != null) ? leaderHero.Name : null;
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				array[num] = new CreateBodyguardMissionBehavior(attackerGeneralName, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, true);
				array[17] = new EquipmentControllerLeaveLogic();
				array[18] = new MissionHardBorderPlacer();
				array[19] = new MissionBoundaryPlacer();
				array[20] = new MissionBoundaryCrossingHandler();
				array[21] = new HighlightsController();
				array[22] = new BattleHighlightsController();
				array[23] = new DeploymentMissionController(isPlayerAttacker);
				array[24] = new BattleDeploymentHandler(false);
				return array;
			}, true, true);
		}


		private static MissionAgentSpawnLogic CreateCampaignMissionAgentSpawnLogic(bool isSiege = false)
		{
			return new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
			{
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
			}, PartyBase.MainParty.Side, isSiege);
		}
	}
}
