using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerlordBattleMod
{
    public class RosterHelper
    {



		private static Tuple<bool, TextObject> PartyScreenDoneCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			TextObject item = new TextObject("Test", null);
			return new Tuple<bool, TextObject>(true, item);
		}

		private static void PartyScreenDoneClicked(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			//Campaign.Current.ConversationManager.ContinueConversation();
		}

		private static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase leftOwnerParty)
		{
			return true;
		}



		public static void OpenGarrisonRoster(TroopRoster leftRoster, TroopRoster rightRoster)
		{
			PartyScreenHelper.OpenScreenDoubleParty(leftRoster, rightRoster, new TextObject("Test", null), 200, new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition), new PartyScreenClosedDelegate(PartyScreenDoneClicked), new IsTroopTransferableDelegate(TroopTransferableDelegate), null);
			return;
		}


		public static TroopRoster GetHeroesTroopRoster()
        {
			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
			IEnumerable<Hero> allHeroes = FindAllHeroes();

			//InformationManager.DisplayMessage(new InformationMessage("Heroes: " + allHeroes.Count().ToString()));

			foreach (Hero hero in allHeroes)
            {
				TryAddCharacterObjectToRoster(troopRoster, hero.CharacterObject, 1);
				//return troopRoster;
			}

			//InformationManager.DisplayMessage(new InformationMessage("Heroes in roster: " + troopRoster.Count.ToString()));
			return troopRoster;
		}


		public static IEnumerable<Hero> FindAllHeroes()
        {
			return from x in Campaign.Current.Characters
				   where x.IsHero
				   select x.HeroObject;
		}
	

		public static void TryAddCharacterObjectToRoster(TroopRoster troopRoster, CharacterObject characterObject, int count)
		{
			if (characterObject != null)
			{
				//InformationManager.DisplayMessage(new InformationMessage("Chegou" + characterId));
				troopRoster.AddToCounts(characterObject, count, false, 0, 0, true, -1);

			}
			else
			{
				InformationManager.DisplayMessage(new InformationMessage("CustomTroopRoster: " + characterObject + " id not found."));
			}
		}


		public static void TryAddCharacterToRoster(TroopRoster troopRoster, string characterId, int count)
		{

			CharacterObject characterObject = CharacterObject.Find(characterId);
			if (characterObject != null)
			{
				//InformationManager.DisplayMessage(new InformationMessage("Chegou" + characterId));
				troopRoster.AddToCounts(characterObject, count, false, 0, 0, true, -1);

			}
			else
			{
				//InformationManager.DisplayMessage(new InformationMessage(characterId + " id not found."));
			}
		}

		public static void TryAddItemToRoster(ItemRoster itemRoster, string itemId, int count)
		{
			foreach (ItemObject item in Items.All)
			{
				if (item.StringId == itemId)
				{
					itemRoster.AddToCounts(item, 999);
					return;
				}

			}

			//InformationManager.DisplayMessage(new InformationMessage("Item Id: " + itemId + " id not found."));
		}
	}
}
