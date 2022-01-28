using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerlordBattleMod
{
    internal class MainManager : CampaignBehaviorBase
    {

        public MainManager()
        {

        }

        public void TickCampaignBehavior()
        {
            if (PlayerEncounter.Current != null)
            {
                if (PlayerEncounter.Current.EncounterState == PlayerEncounterState.Wait)
                {


                    BattleHelper.HandlePostBattle();
                }
                //InformationManager.DisplayMessage(new InformationMessage(PlayerEncounter.Current.EncounterState.ToString()));
            }


            if (Mission.Current == null )
            {




                if (Input.IsKeyDown(InputKey.LeftControl))
                {
                    if (Input.IsKeyReleased(InputKey.T))
                    {
                        //Hero.MainHero.MakeWounded();
                        //PartyBase.MainParty.LeaderHero.MakeWounded
                        //InformationManager.DisplayMessage(new InformationMessage("Hi whassup"));

                        TroopRoster troopRoster = RosterHelper.GetHeroesTroopRoster();

                        TroopRoster leftTroopRoster = new TroopRoster(PartyBase.MainParty);
                        TroopRoster rightTroopRoster = new TroopRoster(PartyBase.MainParty);


                        for (int i = 1; i < 9; i++)
                        {
                            troopRoster.GetCharacterAtIndex(i).HeroObject.HitPoints = troopRoster.GetCharacterAtIndex(i).MaxHitPoints();
                            RosterHelper.TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, troopRoster.GetCharacterAtIndex(i), 1);
                        }

                        for (int i = 9; i < 17; i++)
                        {
                            troopRoster.GetCharacterAtIndex(i).HeroObject.HitPoints = troopRoster.GetCharacterAtIndex(i).MaxHitPoints();
                            RosterHelper.TryAddCharacterObjectToRoster(rightTroopRoster, troopRoster.GetCharacterAtIndex(i), 1);
                        }


                        //PartySpawnHelper.SpawnTwoPartiesFromRoster(leftTroopRoster, rightTroopRoster, PartyBase.MainParty.Position2D, PartyBase.MainParty.Position2D);
                        PartySpawnHelper.SpawnPartyFromRoster(rightTroopRoster, PartyBase.MainParty.Position2D);
                        //RosterHelper.OpenGarrisonRoster(leftTroopRoster, rightTroopRoster);
                    }
                    if (Input.IsKeyReleased(InputKey.Y))
                    {
                        Hero.MainHero.MakeWounded();
                    }
                    if (Input.IsKeyReleased(InputKey.R))
                    {
                        _previewScreen = new PreviewScreen();


                        ScreenManager.PushScreen(_previewScreen);
                    }
                    if (Input.IsKeyReleased(InputKey.G))
                    {
                        TroopRoster troopRoster = RosterHelper.GetHeroesTroopRoster();

                        TroopRoster leftTroopRoster = new TroopRoster(PartyBase.MainParty);
                        TroopRoster rightTroopRoster = new TroopRoster(PartyBase.MainParty);


                        for (int i = 1; i < 9; i++)
                        {
                            RosterHelper.TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, troopRoster.GetCharacterAtIndex(i), 1);
                        }

                        for (int i = 9; i < 17; i++)
                        {
                            RosterHelper.TryAddCharacterObjectToRoster(rightTroopRoster, troopRoster.GetCharacterAtIndex(i), 1);
                        }


                        BattleDisplayScreen battleDisplayScreen = new BattleDisplayScreen(MobileParty.MainParty.MemberRoster, rightTroopRoster);


                        ScreenManager.PushScreen(battleDisplayScreen);
                    }

                }
               
            }


            /*if (ScreenManager.TopScreen is MapScreen)
            {

                //ScreenManager.PopScreen();
                //GameMetrics.garrisonScreenOpened++;
                //ScreenManager.PushScreen(new PartyManagerScreen(this._partyManagerLogic));
                //PartyScreenState.currentState = PartyScreenStateEnum.GarrisonScreen;
                // PartyUtilsHandler.OpenGarrisonRoster();
                RosterHelper.OpenGarrisonRoster(RosterHelper.GetHeroesTroopRoster());
                //InformationManager.DisplayMessage(new InformationMessage());
            }*/
        }

        public override void RegisterEvents()
        {
        }
        public override void SyncData(IDataStore dataStore)
        {
        }


        public PreviewScreen _previewScreen;
    }
}
