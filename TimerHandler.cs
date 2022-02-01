using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace BannerlordBattleMod
{
    public class TimerHandler
    {
        public static int MaxAutoScrollCount = 10;
        public static int AutoScrollCount = MaxAutoScrollCount;

        public static int MaxPreviewTimer = 60 * 1000;
        public static int MaxBattleDisplayTimer = 60 * 1000;
        public static int MaxDoneTimer = 60 * 1000;

        public static int TotalPreviewTimer = MaxPreviewTimer;
        public static int TotalBattleDisplayTimer = MaxBattleDisplayTimer;
        public static int TotalDoneTimer = MaxBattleDisplayTimer;
        public static System.Threading.Timer timerRunning;

        public static BattleDisplayViewModel CurrentBattleDisplayViewModel = null;

        //Timer t = new Timer(TimerCallback, previewViewModel, 0, TotalPreviewTimer);

        public static void TimerCallback(Object o)
        {

            TotalPreviewTimer -= 1000;
            AutoScrollCount -= 1;
            PreviewViewModel previewViewModel = (PreviewViewModel) o;
            

            if(AutoScrollCount <= 0)
            {


                previewViewModel._previewOverview.SelectNextHero();
                AutoScrollCount = MaxAutoScrollCount;
            }

            if (TotalPreviewTimer < 0)
            {
                ResetTimers();
                previewViewModel.AdvanceScreen();

                return;
            }

            previewViewModel.RefreshValues();
        }

        public static void TimerBattleDisplayCallback(Object o)
        {

            TotalBattleDisplayTimer -= 1000;
            BattleDisplayViewModel battleDisplayViewModel = (BattleDisplayViewModel) o;


            if (TotalBattleDisplayTimer < 0)
            {
                ResetTimers();
                battleDisplayViewModel._startBattle = true;
                //InformationManager.DisplayMessage(new InformationMessage("Timer up!"));
                //return;
            }

            battleDisplayViewModel.RefreshValues();
        }

        public static void TimerDoneCallback(Object o)
        {

            TotalDoneTimer -= 1000;
            ScoreboardVM scoreboardVM = (ScoreboardVM)o;

            TimeSpan result = TimeSpan.FromMilliseconds(TimerHandler.TotalDoneTimer);

            scoreboardVM.QuitText = String.Format("{0:00}:{1:00}", result.Minutes, result.Seconds);

            if (TotalDoneTimer < 0)
            {
                ResetTimers();
                scoreboardVM.ExecuteQuitAction();
            }



        }

        public static void ResetTimers()
        {
            if(timerRunning != null) timerRunning.Dispose();
            TotalPreviewTimer = MaxPreviewTimer;
            TotalDoneTimer = MaxDoneTimer;
            TotalBattleDisplayTimer = MaxBattleDisplayTimer;
            AutoScrollCount = MaxAutoScrollCount;
        }

        public static void TickForTimePreview(PreviewViewModel previewViewModel)
        {
            /*TotalPreviewTimer--;
            previewViewModel.RefreshValues();
            if (TotalPreviewTimer < 0)
            {
                previewViewModel.AdvanceScreen();


            }*/
        }

        public static void TickForTimeBattleDisplay(BattleDisplayViewModel battleDisplayViewModel)
        {
            TotalBattleDisplayTimer--;
            battleDisplayViewModel.RefreshValues();
            if (TotalBattleDisplayTimer < 0)
            {

                //InformationManager.DisplayMessage(new InformationMessage(DateTime.Now.ToString()));

                //if (CheckIfFirstTimer()) RecoverTroopGroupReforged();
                //mainManagerScreen._viewModel.MainOverview.RefreshValues();
            }
        }
    }
}
