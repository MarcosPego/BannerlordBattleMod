using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerlordBattleMod
{
    public class PartyScreenHelper
    {
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainParty(TroopRoster leftMemberRoster, TroopRoster rightMemberRoster, TroopRoster leftPrisonerRoster, 
			PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, 
			PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, 
			PartyBase leftOwnerParty = null, TextObject leftPartyName = null, TextObject header = null, Hero leftLeaderHero = null, Hero rightLeaderHero = null,
			int leftPartyMembersSizeLimit = 0, int leftPartyPrisonersSizeLimit = 0, 
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, 
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, 
			PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, 
			PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, 
			PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, 
			bool isTroopUpgradesDisabled = false, bool showProgressBar = false)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = leftOwnerParty,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = leftMemberRoster,
				LeftPrisonerRoster = leftPrisonerRoster,
				RightMemberRoster = rightMemberRoster,
				RightPrisonerRoster = leftPrisonerRoster,
				LeftLeaderHero = leftLeaderHero,
				RightLeaderHero = rightLeaderHero,
				LeftPartyMembersSizeLimit = leftPartyMembersSizeLimit,
				LeftPartyPrisonersSizeLimit = leftPartyPrisonersSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = leftPartyName,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState
			};
		}



		private static bool ManageTroopsAndPrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
        {
            return true;
        }

        public static void OpenScreenDoubleParty(TroopRoster leftMemberRoster, TroopRoster rightMemberRoster, TextObject leftPartyName, int leftPartySizeLimit, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null) {

			_partyScreenLogic =  new PartyScreenLogic();
			TroopRoster leftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
            PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
            PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.NotTransferable;
            PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
            PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(ManageTroopsAndPrisonersDoneHandler);
            PartyBase leftOwnerParty = null;
            PartyScreenLogicInitializationData initializationData = CreateBasicInitDataWithMainParty(leftMemberRoster, rightMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, isTroopTransferable, 
                leftOwnerParty, leftPartyName, new TextObject("{=nZaeTlj8}Exchange Troops", null), null, null, leftPartySizeLimit, 0, 
                partyPresentationDoneButtonDelegate, doneButtonCondition, null, partyPresentationCancelButtonActivateDelegate, onPartyScreenClosed, 
                false, true, false, true);

            _partyScreenLogic.Initialize(initializationData);
            //PartyScreenManager.Instance.IsDonating = false;
            PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
            partyState.InitializeLogic(_partyScreenLogic);
            Game.Current.GameStateManager.PushState(partyState, 0);
        }

        private static PartyScreenLogic _partyScreenLogic;
    }
}
