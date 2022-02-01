using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;


namespace BannerlordBattleMod
{
    public class RosterHelper
    {
		public static List<TroopRoster> RandomRostersFromRosters(TroopRoster oldLeft, TroopRoster oldRight)
        {
            if (false)
            {

            }

			BattleHelper.RemoveAllExtraElements();
			TroopRoster union = new TroopRoster(PartyBase.MainParty);

			foreach (var element in oldLeft.GetTroopRoster())
			{
				TryAddCharacterObjectToRoster(union, element.Character, 1);

			}
			foreach (var element in oldRight.GetTroopRoster())
			{
				TryAddCharacterObjectToRoster(union, element.Character, 1);

			}


			TroopRoster left = new TroopRoster(PartyBase.MainParty);
			TroopRoster right = new TroopRoster(PartyBase.MainParty);
			var rnd = new Random();

			var randomizeRoster = union.GetTroopRoster();
			randomizeRoster.Randomize();

			int biggerHalf = randomizeRoster.Count() - randomizeRoster.Count() / 2;
			int smallerHalf = randomizeRoster.Count() / 2;



			for (int i = 0; i < randomizeRoster.Count(); i++)
			{
				if (i < biggerHalf)
				{
					TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, randomizeRoster.ElementAt(i).Character, 1);
					TryAddCharacterObjectToRoster(left, randomizeRoster.ElementAt(i).Character, 1);

				}
				else
				{
					TryAddCharacterObjectToRoster(right, randomizeRoster.ElementAt(i).Character, 1);
				}
			}

			return FixUpTo16(left, right);


		}



		public static List<TroopRoster> RandomRostersFromList(MBBindingList<PreviewItemViewModel> PreviewItems)
        {
			BattleHelper.RemoveAllExtraElements();
			TroopRoster left = new TroopRoster(PartyBase.MainParty);
			TroopRoster right = new TroopRoster(PartyBase.MainParty);
			var rnd = new Random();
			//var randomized = PreviewItems.Randomize().Take();
			PreviewItems.Randomize();

			int biggerHalf = PreviewItems.Count() - PreviewItems.Count() / 2;
			int smallerHalf = PreviewItems.Count() / 2;

			for (int i=0; i < PreviewItems.Count(); i++)
            {
				if(i < biggerHalf)
                {

					TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, PreviewItems.ElementAt(i)._itemHero.CharacterObject, 1);
					TryAddCharacterObjectToRoster(left, PreviewItems.ElementAt(i)._itemHero.CharacterObject, 1);

				} else
                {
					TryAddCharacterObjectToRoster(right, PreviewItems.ElementAt(i)._itemHero.CharacterObject, 1);
				}
            }
			/*
			foreach (PreviewItemViewModel pivm in split.ElementAt(0))
			{

				
				TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, pivm._itemHero.CharacterObject, 1);
				TryAddCharacterObjectToRoster(left, pivm._itemHero.CharacterObject, 1);
			}

			InformationManager.DisplayMessage(new InformationMessage("Side 1: " + split.ElementAt(0).Count().ToString()));

			foreach (PreviewItemViewModel pivm in split.ElementAt(1))
			{
				TryAddCharacterObjectToRoster(right, pivm._itemHero.CharacterObject, 1);
			}
			InformationManager.DisplayMessage(new InformationMessage("Side 2: " + split.ElementAt(1).Count().ToString()));*/
			return FixUpTo16(left, right);
		}

		public static List<TroopRoster> FixUpTo16(TroopRoster left, TroopRoster right)
        {
			TroopRoster troopRoster = RosterHelper.GetHeroesTroopRoster();


			var rnd = new Random();
			var randomized = troopRoster.GetTroopRoster();
			randomized.Randomize();


			if (true)
            {
				if (left.Count < 16 || right.Count < 16 || left.Count != right.Count)
				{
					foreach (var hero in randomized)
					{
						if (!Campaign.Current.EncyclopediaManager.ViewDataTracker.EncyclopediaIsBookmarked(hero.Character.HeroObject))
						{

							if (PartyBase.MainParty.LeaderHero != hero.Character.HeroObject)
							{
								if (left.Count < 16)
								{
									TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, hero.Character, 1);
									TryAddCharacterObjectToRoster(left, hero.Character, 1);
								}
								else if (right.Count < 16)
								{
									//TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, hero.Character, 1);
									TryAddCharacterObjectToRoster(right, hero.Character, 1);
								}
								else if (left.Count != right.Count)
								{

									if (left.Count > right.Count)
									{
										TryAddCharacterObjectToRoster(right, hero.Character, 1);
									}
									else
									{
										TryAddCharacterObjectToRoster(MobileParty.MainParty.MemberRoster, hero.Character, 1);
										TryAddCharacterObjectToRoster(left, hero.Character, 1);
									}

								}
								else
								{
									break;
								}
							}

						}
					}
				}
			}


			List<TroopRoster> troopRosters = new List<TroopRoster>();
			troopRosters.Add(MobileParty.MainParty.MemberRoster);
			troopRosters.Add(right);

			//InformationManager.DisplayMessage(new InformationMessage("Roster1: " + troopRosters.ElementAt(0).Count.ToString()));
			//InformationManager.DisplayMessage(new InformationMessage("Roster2: " + troopRosters.ElementAt(1).Count.ToString()));

			return troopRosters;
		}


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
