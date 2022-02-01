using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace BannerlordBattleMod.HarmonyOverrides
{
    public class ViewDataTrackerOverride
    {

        public static List<Hero> _bookMarkedHeroes = new List<Hero>();

        [HarmonyPatch(typeof(ViewDataTracker))]
        [HarmonyPatch("EncyclopediaAddBookmarkToItem")]
        class EncyclopediaAddBookmarkToItemOverride
        {
            static void Prefix(ViewDataTracker __instance, ref List<Hero> ____encyclopediaBookmarkedHeroes)
            {
                _bookMarkedHeroes = ____encyclopediaBookmarkedHeroes;

                //InformationManager.DisplayMessage(new InformationMessage("Chamado"));
            }
        }

        [HarmonyPatch(typeof(ViewDataTracker))]
        [HarmonyPatch("EncyclopediaRemoveBookmarkFromItem")]
        class EncyclopediaRemoveBookmarkFromItemOverride
        {
            static void Prefix(ViewDataTracker __instance, ref List<Hero> ____encyclopediaBookmarkedHeroes)
            {
                _bookMarkedHeroes = ____encyclopediaBookmarkedHeroes;

                //InformationManager.DisplayMessage(new InformationMessage("Chamado"));
            }
        }

        [HarmonyPatch(typeof(ViewDataTracker))]
        [HarmonyPatch("EncyclopediaIsBookmarked")]
        class EncyclopediaIsBookmarkedOverride
        {
            static void Prefix(ViewDataTracker __instance, ref List<Hero> ____encyclopediaBookmarkedHeroes)
            {
                _bookMarkedHeroes = ____encyclopediaBookmarkedHeroes;

                //InformationManager.DisplayMessage(new InformationMessage("Chamado"));
            }
        }
    }


}
