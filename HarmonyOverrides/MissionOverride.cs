using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BannerlordBattleMod.HarmonyOverrides
{
    public class MissionOverride
    {

        public static int FrameCounter = 0;


        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("OnTick")]
        class OnTickOverride
        {

            static void Postfix(Mission __instance)
            {
                if (MissionOverride.FrameCounter == 0)
                {
                    try
                    {
                        __instance.GetMissionBehavior<DeploymentMissionController>().OnPlayerDeploymentFinish();

                    }
                    catch
                    {

                    }

                    MissionUtils.KillMainHero();

                }
                MissionOverride.FrameCounter++;
                MissionUtils.ManageInputKeys(__instance);


            }
        }

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("AfterStart")]
        class AfterStartOverride
        {
            static void Postfix(Mission __instance)
            {
                MissionOverride.FrameCounter = 0;
            }
        }
    }
}
