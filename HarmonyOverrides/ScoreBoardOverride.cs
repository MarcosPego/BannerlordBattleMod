using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace BannerlordBattleMod.HarmonyOverrides
{
    public class ScoreBoardOverride
    {
        public static bool CallTimerOnce = true;


        [HarmonyPatch(typeof(ScoreboardVM))]
        [HarmonyPatch("UpdateQuitText")]
        class EncyclopediaAddBookmarkToItemOverride
        {
            static void Postfix(ScoreboardVM __instance)
            {
                if (__instance.IsOver)
                {
                    __instance.QuitText = "Test";
                    if (CallTimerOnce)
                    {
                        TimerHandler.ResetTimers();
                        TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerDoneCallback, __instance, 0, 1000);
                    }
                }


            }
        }

    }
}
