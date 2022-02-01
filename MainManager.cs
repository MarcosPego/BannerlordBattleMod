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
                    ScreenManager.PopScreen();
                    _previewScreen._viewModel.RefreshValues();
                    _previewScreen._viewModel._previewOverview.RefreshTimerCallbaks();
                }
                //InformationManager.DisplayMessage(new InformationMessage(PlayerEncounter.Current.EncounterState.ToString()));
            }

            //if (ScreenManager.TopScreen is PreviewScreen) { TimerHandler.TickForTimePreview(_previewScreen._viewModel); }
            if (ScreenManager.TopScreen is BattleDisplayScreen) {

                if (TimerHandler.CurrentBattleDisplayViewModel != null)
                {

                    TimerHandler.CurrentBattleDisplayViewModel.ShouldStartBattle();
                }
            }

            if (Mission.Current == null )
            {




                if (Input.IsKeyDown(InputKey.LeftControl))
                {
                    if (Input.IsKeyReleased(InputKey.T))
                    {
                        if (ScreenManager.TopScreen is PreviewScreen) { _previewScreen._viewModel.AdvanceScreen(); }
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
