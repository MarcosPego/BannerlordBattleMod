using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;

namespace BannerlordBattleMod
{
    public class PartySpawnHelper
    {
        public static void SpawnPartyFromRoster(TroopRoster roster, Vec2 pos, string scene, bool enterSettlement = true)
        {
            if (enterSettlement)
            {
                var randomSettlement = SettlementHelper.FindNearestSettlementToPoint(MobileParty.MainParty.Position2D);

                EnterSettlementAction.ApplyForParty(MobileParty.MainParty, randomSettlement);
            }


            TroopRoster emptyRoster = new TroopRoster(PartyBase.MainParty);


            BattleHelper.CurrentOpponentParty = MobilePartyHelper.SpawnLordParty(roster.GetCharacterAtIndex(0).HeroObject, pos, 2);
            //MobileParty rightParty = MobilePartyHelper.SpawnLordParty(rightRoster.GetCharacterAtIndex(0).HeroObject, pos2, 2);
            BattleHelper.CurrentOpponentParty.InitializeMobilePartyAtPosition(roster, emptyRoster, BattleHelper.CurrentOpponentParty.Position2D);

            BattleHelper.CurrentOpponentParty.MemberRoster.RemoveIf((TroopRosterElement t) => !t.Character.IsHero);

            //rightParty.InitializeMobilePartyAtPosition(rightRoster, emptyRoster, rightParty.Position2D);

            PlayerEncounter.Start();

            PlayerEncounter.Current.SetupFields(PartyBase.MainParty, BattleHelper.CurrentOpponentParty.Party );
            PlayerEncounter.StartBattle();
            MapPatchData mapPatchAtPosition = Campaign.Current.MapSceneWrapper.GetMapPatchAtPosition(MobileParty.MainParty.Position2D);
            string battleSceneForMapPatch = PlayerEncounter.GetBattleSceneForMapPatch(mapPatchAtPosition);
            BattleMissionManager.OpenBattleMission(scene);
        }


        public static void SpawnTwoPartiesFromRoster(TroopRoster leftRoster, TroopRoster rightRoster, Vec2 pos1, Vec2 pos2)
        {
            TroopRoster emptyRoster = new TroopRoster(PartyBase.MainParty);

            MobileParty leftParty = MobilePartyHelper.SpawnLordParty(leftRoster.GetCharacterAtIndex(0).HeroObject, pos1, 2);
            MobileParty rightParty = MobilePartyHelper.SpawnLordParty(rightRoster.GetCharacterAtIndex(0).HeroObject, pos2, 2);
            leftParty.InitializeMobilePartyAtPosition(leftRoster, emptyRoster, leftParty.Position2D);
            rightParty.InitializeMobilePartyAtPosition(rightRoster, emptyRoster, rightParty.Position2D);

            PlayerEncounter.Start();

            PlayerEncounter.Current.SetupFields(leftParty.Party, rightParty.Party);
            PlayerEncounter.StartBattle();
            MapPatchData mapPatchAtPosition = Campaign.Current.MapSceneWrapper.GetMapPatchAtPosition(MobileParty.MainParty.Position2D);
            string battleSceneForMapPatch = PlayerEncounter.GetBattleSceneForMapPatch(mapPatchAtPosition);
            /*CampaignMission.OpenBattleMission(battleSceneForMapPatch);*/
        }
    }
}
