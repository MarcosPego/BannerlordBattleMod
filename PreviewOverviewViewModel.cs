using BannerlordBattleMod.HarmonyOverrides;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerlordBattleMod
{
    public class PreviewOverviewViewModel : ViewModel
    {
		public PreviewOverviewViewModel(PreviewViewModel previewViewModel)
        {
			this._exitOnSaveOver = false;
			_previewViewModel = previewViewModel;

			PreviewItems = new MBBindingList<PreviewItemViewModel>();

			this.CharacterHelmSlot = new DisplayItemVM();
			this.CharacterCloakSlot = new DisplayItemVM();
			this.CharacterTorsoSlot = new DisplayItemVM();
			this.CharacterGloveSlot = new DisplayItemVM();
			this.CharacterBootSlot = new DisplayItemVM();
			this.CharacterMountSlot = new DisplayItemVM();
			this.CharacterMountArmorSlot = new DisplayItemVM();
			this.CharacterWeapon1Slot = new DisplayItemVM();
			this.CharacterWeapon2Slot = new DisplayItemVM();
			this.CharacterWeapon3Slot = new DisplayItemVM();
			this.CharacterWeapon4Slot = new DisplayItemVM();



			RefreshChosenHeroesList();

			//this._faction = Hero.MainHero.Clan;

			CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool,string>(this.OnSaveOver));
			RefreshValues();

		
			this.ItemMenu = new DisplayItemMenuVM();
			RefreshTimerCallbaks();
			/*TimerHandler.ResetTimers();
			TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerCallback, _previewViewModel, 0, 1000);*/
		}

		private void OnSaveOver(bool isSuccessful, string saveName)
		{
			if (this._exitOnSaveOver)
			{
				MBGameManager.EndGame();
			}
		}

		public void RefreshCallbacks()
		{
			//Not Working
			//ItemVM.ProcessItemTooltip = new Action<ItemVM>(this.ProcessItemTooltip);

		}

		public void RefreshTimerCallbaks()
		{
			TimerHandler.ResetTimers();
			TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerCallback, _previewViewModel, 0, 1000);

		}

		private static IMarketData GetCurrentMarketData()
		{
			IMarketData marketData = null;
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				Settlement settlement = MobileParty.MainParty.CurrentSettlement;
				if (settlement == null)
				{
					settlement = SettlementHelper.FindNearestTown(null, null);
				}
				if (settlement != null)
				{
					if (settlement.IsVillage)
					{
						marketData = settlement.Village.MarketData;
					}
					else if (settlement.IsTown)
					{
						marketData = settlement.Town.MarketData;
					}
				}
			}

			return marketData;
		}

		public void ProcessItemTooltip(ItemVM item)
		{

			InformationManager.DisplayMessage(new InformationMessage(" test"));

			if (item == null || string.IsNullOrEmpty(item.StringId))
			{
				return;
			}
			//this._selectedItem = (item as SPItemVM);
			//ItemVM comparedItem = this.ProcessCompareItem(item, 0);
			this.ItemMenu.SetItem(item as DisplayItemVM, this._currentSelectedHero.CharacterObject);
			/*this.RefreshTransactionCost(1);
			this._selectedItem.UpdateCanBeSlaughtered();
			if (this._selectedItem.IsNew)
			{
				this._selectedItem.UpdateIsNew();
			}*/
		}


		public void RefreshChosenHeroesList()
		{
			this.PreviewItems.Clear();

			TroopRoster troopRoster = RosterHelper.GetHeroesTroopRoster();


			//InformationManager.DisplayMessage(new InformationMessage(max.ToString()));

			for (int i = 1; i < troopRoster.Count; i++)
            {
				PreviewItemViewModel previewItemViewModel;

                if (Campaign.Current.EncyclopediaManager.ViewDataTracker.EncyclopediaIsBookmarked(troopRoster.GetCharacterAtIndex(i).HeroObject))
                {
					previewItemViewModel = new PreviewItemViewModel(this, troopRoster.GetCharacterAtIndex(i).HeroObject, false, true);
					PreviewItems.Add(previewItemViewModel);
				}

				/*if (i == 1)
                {
					//troopRoster.GetCharacterAtIndex(i)
					previewItemViewModel = new PreviewItemViewModel(this, troopRoster.GetCharacterAtIndex(i).HeroObject, true, true);
					PreviewItems.Add(previewItemViewModel);
					_currentSelectedHero = troopRoster.GetCharacterAtIndex(i).HeroObject;
				} else
                {
					previewItemViewModel = new PreviewItemViewModel(this, troopRoster.GetCharacterAtIndex(i).HeroObject, false, true);
					PreviewItems.Add(previewItemViewModel);
				}
				*/

			}

            /*Campaign.Current.EncyclopediaManager.ViewDataTracker.EncyclopediaIsBookmarked(PartyBase.MainParty.LeaderHero);



			foreach (Hero bookmarkeHero in ViewDataTrackerOverride._bookMarkedHeroes)
            {
				PreviewItemViewModel previewItemViewModel;
				previewItemViewModel = new PreviewItemViewModel(this, bookmarkeHero, true, true);
				PreviewItems.Add(previewItemViewModel);
			}*/

            if (PreviewItems.Any())
            {
				PreviewItems.FirstOrDefault().IsHighlighted = true;
				PreviewItems.FirstOrDefault()._isSelected = true;
				_currentSelectedHero = PreviewItems.FirstOrDefault()._itemHero;

				CharacterName = _currentSelectedHero.Name.ToString() ;


			} else
            {
				_currentSelectedHero = PartyBase.MainParty.LeaderHero;

			}

		}


		public void ExectuteLeaveGame()
        {
			InformationManager.DisplayMessage(new InformationMessage("Exiting to Main Menu!"));
			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			//this._exitOnSaveOver = true;

			MBGameManager.EndGame();
		}

		public void ExectuteRefreshAndStart()
		{
			RefreshChosenHeroesList();
			RefreshValues();
			TimerHandler.ResetTimers();
			TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerCallback, _previewViewModel, 0, 1000);

		}

		public void ExectuteAbortTimer()
		{

			TimerHandler.ResetTimers();
			RefreshValues();
			_previewViewModel.RefreshValues();
		}

		public void ExectuteAdvanceNext()
		{

			TimerHandler.ResetTimers();
			RefreshValues();
			_previewViewModel.RefreshValues();
			_previewViewModel.AdvanceScreen();
		}

		public override void RefreshValues()
		{

			base.RefreshValues();
			RefreshPreviews();
			SelectMainHero();
			UpdateCharacterEquipment();


			this.EquipmentHelmSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_helm_slot", null), null);
			this.EquipmentArmorSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_armor_slot", null), null);
			this.EquipmentBootSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_boot_slot", null), null);
			this.EquipmentCloakSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_cloak_slot", null), null);
			this.EquipmentGloveSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_glove_slot", null), null);
			this.EquipmentHarnessSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_mount_armor_slot", null), null);
			this.EquipmentMountSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_mount_slot", null), null);
			this.EquipmentWeaponSlotHint = new HintViewModel(GameTexts.FindText("str_inventory_filter_weapons", null), null);
		}

		public void SelectFirstOrDefault()
		{

			_currentSelectedHero = PreviewItems.FirstOrDefault()._itemHero;
			PreviewItems.FirstOrDefault()._isSelected = true;
			CharacterName = _currentSelectedHero.Name.ToString();
		}

		public void SelectNextHero()
        {
			
			foreach(PreviewItemViewModel previewItemViewModel in PreviewItems)
            {
                if (previewItemViewModel._isSelected == true)
                {
					int index = PreviewItems.IndexOf(previewItemViewModel);

					PreviewItems.ElementAt(index ==  PreviewItems.Count - 1 ? 0 : index + 1).MakeNormalAutoScroll();
					return;
				}

            }

		}


		public void ClearSelected()
        {
			foreach(PreviewItemViewModel previewItemViewModel in PreviewItems)
            {
				previewItemViewModel._isSelected = false;
				previewItemViewModel.IsHighlighted = false;
			}
        }

		public void RefreshPreviews()
		{
			foreach (PreviewItemViewModel previewItemViewModel in PreviewItems)
			{
				previewItemViewModel.RefreshValues();

			}
		}

		public void SelectMainHero()
		{
			//this.Family.Add(new ClanLordItemVM(Hero.MainHero, new Action<ClanLordItemVM>(this.OnMemberSelection)));
			//ClanLordItemVM member = new ClanLordItemVM(Hero.MainHero, new Action<ClanLordItemVM>(this.OnMemberSelection));
			ClanLordItemVM member = new ClanLordItemVM(_currentSelectedHero, new Action<ClanLordItemVM>(this.OnMemberSelection));
			this.CurrentSelectedMember = member;
		}

		private void OnMemberSelection(ClanLordItemVM member)
		{
			if (this.CurrentSelectedMember != null)
			{
				this.CurrentSelectedMember.IsSelected = false;
			}
			this.CurrentSelectedMember = member;
			//bool flag = member.GetHero() == Hero.MainHero;
			//bool flag2 = this._faction.Companions.Contains(member.GetHero());
			//bool flag3 = TaleWorlds.CampaignSystem.Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(member.GetHero());
			//this.CanKickCurrentMemberFromClan = (!flag && flag2 && !flag3);
			//string kickFromClanReasonString = CampaignUIHelper.GetKickFromClanReasonString(flag, flag2, flag3);
			//this.KickFromClanActionHint.HintText = (string.IsNullOrEmpty(kickFromClanReasonString) ? TextObject.Empty : new TextObject("{=!}" + kickFromClanReasonString, null));
			if (member != null)
			{
				member.IsSelected = true;
			}
		}

		private void UpdateCharacterEquipment()
		{
			this.CharacterHelmSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.NumAllWeaponSlots), 1), EquipmentIndex.NumAllWeaponSlots);
			this.CharacterCloakSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Cape), 1), EquipmentIndex.Cape);
			this.CharacterTorsoSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Body), 1), EquipmentIndex.Body);
			this.CharacterGloveSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Gloves), 1), EquipmentIndex.Gloves);
			this.CharacterBootSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Leg), 1), EquipmentIndex.Leg);
			this.CharacterMountSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.ArmorItemEndSlot), 1), EquipmentIndex.ArmorItemEndSlot);
			this.CharacterMountArmorSlot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.HorseHarness), 1), EquipmentIndex.HorseHarness);
			this.CharacterWeapon1Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.WeaponItemBeginSlot), 1), EquipmentIndex.WeaponItemBeginSlot);
			this.CharacterWeapon2Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon1), 1), EquipmentIndex.Weapon1);
			this.CharacterWeapon3Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon2), 1), EquipmentIndex.Weapon2);
			this.CharacterWeapon4Slot = this.InitializeCharacterEquipmentSlot(new ItemRosterElement(this.ActiveEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon3), 1), EquipmentIndex.Weapon3);
			//this.MainCharacter.SetEquipment(this.ActiveEquipment);
		}

		private DisplayItemVM InitializeCharacterEquipmentSlot(ItemRosterElement itemRosterElement, EquipmentIndex equipmentIndex)
		{
			DisplayItemVM spitemVM;
			if (!itemRosterElement.IsEmpty)
			{


				spitemVM = new DisplayItemVM(itemRosterElement);
				//spitemVM = new SPItemVM();
				//spitemVM.RefreshWith(null, InventoryLogic.InventorySide.Equipment);
			}
			else
			{
				spitemVM = new DisplayItemVM();
				spitemVM.RefreshWith(null);
			}
			return spitemVM;
		}

		private bool CanCharacterUseItemBasedOnSkills(ItemRosterElement itemRosterElement)
		{
			return CharacterHelper.CanUseItemBasedOnSkill(_currentSelectedMember.GetHero().CharacterObject, itemRosterElement.EquipmentElement);
		}


		[DataSourceProperty]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (value != this._isNew)
				{
					this._isNew = value;
					base.OnPropertyChangedWithValue(value, "IsNew");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemMenuVM ItemMenu
		{
			get
			{
				return this._itemMenu;
			}
			set
			{
				if (value != this._itemMenu)
				{
					this._itemMenu = value;
					base.OnPropertyChangedWithValue(value, "ItemMenu");
				}
			}
		}

		[DataSourceProperty]
		public MBBindingList<PreviewItemViewModel> PreviewItems
		{
			get
			{
				return this._previewItems;
			}
			set
			{
				if (value != this._previewItems)
				{
					this._previewItems = value;
					base.OnPropertyChangedWithValue(value, "PreviewItems");
				}
			}
		}

		[DataSourceProperty]
		public ClanLordItemVM CurrentSelectedMember
		{
			get
			{
				return this._currentSelectedMember;
			}
			set
			{
				if (value != this._currentSelectedMember)
				{
					this._currentSelectedMember = value;
					base.OnPropertyChangedWithValue(value, "CurrentSelectedMember");
					this.IsAnyValidMemberSelected = (value != null);
				}
			}
		}

		[DataSourceProperty]
		public bool IsAnyValidMemberSelected
		{
			get
			{
				return this._isAnyValidMemberSelected;
			}
			set
			{
				if (value != this._isAnyValidMemberSelected)
				{
					this._isAnyValidMemberSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidMemberSelected");
				}
			}
		}


		[DataSourceProperty]
		public string LastSelectedItemId
		{
			get
			{
				return this._lastSelectedItemId;
			}
			set
			{
				if (value != this._lastSelectedItemId)
				{
					this._lastSelectedItemId = value;
					base.OnPropertyChangedWithValue(value, "LastSelectedItemId");
				}
			}
		}


		private Equipment ActiveEquipment
		{
			get
			{
				/*if (!this.IsInWarSet)
				{
					return _currentSelectedHero.CharacterObject.FirstCivilianEquipment;
				}*/
				return this._currentSelectedHero.CharacterObject.FirstBattleEquipment;
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterHelmSlot
		{
			get
			{
				return this._characterHelmSlot;
			}
			set
			{
				if (value != this._characterHelmSlot)
				{
					this._characterHelmSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterHelmSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterCloakSlot
		{
			get
			{
				return this._characterCloakSlot;
			}
			set
			{
				if (value != this._characterCloakSlot)
				{
					this._characterCloakSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterCloakSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterTorsoSlot
		{
			get
			{
				return this._characterTorsoSlot;
			}
			set
			{
				if (value != this._characterTorsoSlot)
				{
					this._characterTorsoSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterTorsoSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterGloveSlot
		{
			get
			{
				return this._characterGloveSlot;
			}
			set
			{
				if (value != this._characterGloveSlot)
				{
					this._characterGloveSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterGloveSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterBootSlot
		{
			get
			{
				return this._characterBootSlot;
			}
			set
			{
				if (value != this._characterBootSlot)
				{
					this._characterBootSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterBootSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterMountSlot
		{
			get
			{
				return this._characterMountSlot;
			}
			set
			{
				if (value != this._characterMountSlot)
				{
					this._characterMountSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterMountSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterMountArmorSlot
		{
			get
			{
				return this._characterMountArmorSlot;
			}
			set
			{
				if (value != this._characterMountArmorSlot)
				{
					this._characterMountArmorSlot = value;
					base.OnPropertyChangedWithValue(value, "CharacterMountArmorSlot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterWeapon1Slot
		{
			get
			{
				return this._characterWeapon1Slot;
			}
			set
			{
				if (value != this._characterWeapon1Slot)
				{
					this._characterWeapon1Slot = value;
					base.OnPropertyChangedWithValue(value, "CharacterWeapon1Slot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterWeapon2Slot
		{
			get
			{
				return this._characterWeapon2Slot;
			}
			set
			{
				if (value != this._characterWeapon2Slot)
				{
					this._characterWeapon2Slot = value;
					base.OnPropertyChangedWithValue(value, "CharacterWeapon2Slot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterWeapon3Slot
		{
			get
			{
				return this._characterWeapon3Slot;
			}
			set
			{
				if (value != this._characterWeapon3Slot)
				{
					this._characterWeapon3Slot = value;
					base.OnPropertyChangedWithValue(value, "CharacterWeapon3Slot");
				}
			}
		}

		[DataSourceProperty]
		public DisplayItemVM CharacterWeapon4Slot
		{
			get
			{
				return this._characterWeapon4Slot;
			}
			set
			{
				if (value != this._characterWeapon4Slot)
				{
					this._characterWeapon4Slot = value;
					base.OnPropertyChangedWithValue(value, "CharacterWeapon4Slot");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentHelmSlotHint
		{
			get
			{
				return this._equipmentHelmSlotHint;
			}
			set
			{
				if (value != this._equipmentHelmSlotHint)
				{
					this._equipmentHelmSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentHelmSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentArmorSlotHint
		{
			get
			{
				return this._equipmentArmorSlotHint;
			}
			set
			{
				if (value != this._equipmentArmorSlotHint)
				{
					this._equipmentArmorSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentArmorSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentBootSlotHint
		{
			get
			{
				return this._equipmentBootSlotHint;
			}
			set
			{
				if (value != this._equipmentBootSlotHint)
				{
					this._equipmentBootSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentBootSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentCloakSlotHint
		{
			get
			{
				return this._equipmentCloakSlotHint;
			}
			set
			{
				if (value != this._equipmentCloakSlotHint)
				{
					this._equipmentCloakSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentCloakSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentGloveSlotHint
		{
			get
			{
				return this._equipmentGloveSlotHint;
			}
			set
			{
				if (value != this._equipmentGloveSlotHint)
				{
					this._equipmentGloveSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentGloveSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentHarnessSlotHint
		{
			get
			{
				return this._equipmentHarnessSlotHint;
			}
			set
			{
				if (value != this._equipmentHarnessSlotHint)
				{
					this._equipmentHarnessSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentHarnessSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentMountSlotHint
		{
			get
			{
				return this._equipmentMountSlotHint;
			}
			set
			{
				if (value != this._equipmentMountSlotHint)
				{
					this._equipmentMountSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentMountSlotHint");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel EquipmentWeaponSlotHint
		{
			get
			{
				return this._equipmentWeaponSlotHint;
			}
			set
			{
				if (value != this._equipmentWeaponSlotHint)
				{
					this._equipmentWeaponSlotHint = value;
					base.OnPropertyChangedWithValue(value, "EquipmentWeaponSlotHint");
				}
			}
		}



		[DataSourceProperty]
		public string CharacterName
		{
			get
			{
				return this._characterName;
			}
			set
			{
				if (value != this._characterName)
				{
					this._characterName = value;
					base.OnPropertyChangedWithValue(value, "CharacterName");
				}
			}
		}


		private MBBindingList<PreviewItemViewModel> _previewItems;

		private ClanLordItemVM _currentSelectedMember;

		//private readonly Clan _faction;

		private bool _isAnyValidMemberSelected;
		private DisplayItemMenuVM _itemMenu;


		private InventoryLogic _inventoryLogic;
		public Hero _currentSelectedHero;

		private string _lastSelectedItemId;
		private string _characterName;

		private DisplayItemVM _characterHelmSlot;

		private DisplayItemVM _characterCloakSlot;
		private DisplayItemVM _characterTorsoSlot;
		private DisplayItemVM _characterGloveSlot;
		private DisplayItemVM _characterBootSlot;
		private DisplayItemVM _characterMountSlot;
		private DisplayItemVM _characterMountArmorSlot;
		private DisplayItemVM _characterWeapon1Slot;
		private DisplayItemVM _characterWeapon2Slot;
		private DisplayItemVM _characterWeapon3Slot;
		private DisplayItemVM _characterWeapon4Slot;

		private HintViewModel _equipmentHelmSlotHint;
		private HintViewModel _equipmentArmorSlotHint;
		private HintViewModel _equipmentBootSlotHint;
		private HintViewModel _equipmentCloakSlotHint;
		private HintViewModel _equipmentGloveSlotHint;
		private HintViewModel _equipmentHarnessSlotHint;
		private HintViewModel _equipmentMountSlotHint;
		private HintViewModel _equipmentWeaponSlotHint;

		private bool _isNew;

		private PreviewViewModel _previewViewModel;
		private bool _exitOnSaveOver;
	}
}
