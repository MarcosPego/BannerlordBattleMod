using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace BannerlordBattleMod
{
    public class BattleHelper
    {
        public static MobileParty CurrentOpponentParty = null;


        public static void StartBattle()
        {
            PlayerEncounter.Start();

            //InformationManager.DisplayMessage(new InformationMessage(PartyBase.MainParty.IsSettlement.ToString()));
            //PlayerEncounter.Current.SetupFields(PartyBase.MainParty, OpponentPartyHandler.CurrentOpponentParty.Party);
            PlayerEncounter.StartBattle();
            //CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
        }


        public static void HandlePostBattle()
        {
            if (MobileParty.MainParty.CurrentSettlement != null) LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
            PlayerEncounter.Finish(false);
            HandleHeroesAfterBattleHealth();
            RemoveOpponentParty();
            RemoveAllExtraElements();
            HandleAfterBattleHealth();
        }


        public static void RemoveOpponentParty()
        {
            if (CurrentOpponentParty != null)
            {
                if (CurrentOpponentParty.MemberRoster.TotalManCount > 0)
                    CurrentOpponentParty.RemoveParty();
            }
            CurrentOpponentParty = null;
        }

        public static void HandleAfterBattleHealth()
        {
            PartyBase.MainParty.LeaderHero.HitPoints = PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints();
           
        }

        public static void HandleHeroesAfterBattleHealth()
        {

            foreach (var element in PartyBase.MainParty.MemberRoster.GetTroopRoster())
            {
                element.Character.HeroObject.HitPoints = element.Character.MaxHitPoints();
            }

            foreach (var element in CurrentOpponentParty.MemberRoster.GetTroopRoster())
            {
                element.Character.HeroObject.HitPoints = element.Character.MaxHitPoints();
            }


        }


        public static void RemoveAllExtraElements()
        {
            
            foreach(TroopRosterElement troop in  PartyBase.MainParty.MemberRoster.GetTroopRoster() )
            {
                if(Hero.MainHero.CharacterObject != troop.Character)
                {
                    PartyBase.MainParty.MemberRoster.RemoveTroop(troop.Character);
                }
            }

        }
    }
}
