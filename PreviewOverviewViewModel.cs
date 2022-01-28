using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerlordBattleMod
{
    public class PreviewOverviewViewModel : ViewModel
    {
		public PreviewOverviewViewModel()
        {
			PreviewItems = new MBBindingList<PreviewItemViewModel>();

			RefreshChosenHeroesList();

			//this._faction = Hero.MainHero.Clan;

			RefreshValues();
		}

		public void RefreshChosenHeroesList()
		{
			this.PreviewItems.Clear();

			TroopRoster troopRoster = RosterHelper.GetHeroesTroopRoster();

			int max = 17;
			if(troopRoster.Count < 17)
            {
				max = troopRoster.Count;

			}

			//InformationManager.DisplayMessage(new InformationMessage(max.ToString()));

			for (int i = 1; i < max; i++)
            {
				PreviewItemViewModel previewItemViewModel;
				if (i == 1)
                {
					previewItemViewModel = new PreviewItemViewModel(this, troopRoster.GetCharacterAtIndex(i).HeroObject, true, true);
					PreviewItems.Add(previewItemViewModel);
					currentSelectedHero = troopRoster.GetCharacterAtIndex(i).HeroObject;
				} else
                {
					previewItemViewModel = new PreviewItemViewModel(this, troopRoster.GetCharacterAtIndex(i).HeroObject, false, true);
					PreviewItems.Add(previewItemViewModel);
				}
				

			}

		}

		public override void RefreshValues()
		{

			base.RefreshValues();
			RefreshPreviews();
			SelectMainHero();

		}

		public void SelectFirstOrDefault()
		{

			currentSelectedHero = PreviewItems.FirstOrDefault()._itemHero;
			PreviewItems.FirstOrDefault()._isSelected = true;
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
			ClanLordItemVM member = new ClanLordItemVM(currentSelectedHero, new Action<ClanLordItemVM>(this.OnMemberSelection));
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



		private MBBindingList<PreviewItemViewModel> _previewItems;

		private ClanLordItemVM _currentSelectedMember;

		//private readonly Clan _faction;

		private bool _isAnyValidMemberSelected;



		public Hero currentSelectedHero;

		private string _lastSelectedItemId;

	}
}
