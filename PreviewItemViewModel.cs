using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerlordBattleMod
{
    public class PreviewItemViewModel : ViewModel
    {
		public static string DefaultBrush = "Clan.Tuple.Name.Text";

		public static string HighlightBrush = "NameTitle.Highlight";


		public PreviewItemViewModel(PreviewOverviewViewModel previewOverviewViewModel, Hero hero, bool isSelected = false, bool isBookmarked = false)
		{
			_previewOverviewViewModel = previewOverviewViewModel;
			_isSelected = isSelected;
			_isHighlighted = isSelected;
			//UpdateSelected();
			_itemHero = hero;
			Name = _itemHero.Name.ToString();

			IsBookmarked = isBookmarked;

			RefreshValues();
		}
		public override void RefreshValues()
		{
			base.RefreshValues();
			//UpdateSelected();
		}

		public void UpdateSelected()
        {
            if (_isSelected)
            {
				NameSelectionBrush = HighlightBrush;

			}
            else
            {
				NameSelectionBrush = DefaultBrush;
			}
        }

		public void ExecuteSelection()
		{
			_previewOverviewViewModel.ClearSelected();
			_isSelected = true;
			//InformationManager.DisplayMessage(new InformationMessage( " id not found."));
			_previewOverviewViewModel._currentSelectedHero = _itemHero;
			_previewOverviewViewModel.CharacterName = _previewOverviewViewModel._currentSelectedHero.Name.ToString();
			IsHighlighted = true;
			//NameSelectionBrush = HighlightBrush;
			_previewOverviewViewModel.RefreshValues();

			RefreshValues();
		}

		public void MakeNormalAutoScroll()
        {
			_previewOverviewViewModel.ClearSelected();
			_isSelected = true;
			//InformationManager.DisplayMessage(new InformationMessage( " id not found."));
			_previewOverviewViewModel._currentSelectedHero = _itemHero;
			_previewOverviewViewModel.CharacterName = _previewOverviewViewModel._currentSelectedHero.Name.ToString();
			IsHighlighted = true;
			//NameSelectionBrush = HighlightBrush;
			_previewOverviewViewModel.RefreshValues();

			RefreshValues();
		}


		[DataSourceProperty]
		public bool IsBookmarked
		{
			get
			{
				return this._isBookmarked;
			}
			set
			{
				if (value != this._isBookmarked)
				{
					this._isBookmarked = value;
					base.OnPropertyChangedWithValue(value, "IsBookmarked");
				}
			}
		}


		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue(value, "Name");
				}
			}
		}

		[DataSourceProperty]
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (value != this._id)
				{
					this._id = value;
					base.OnPropertyChangedWithValue(value, "Id");
				}
			}
		}


		//Brushes
		[DataSourceProperty]
		public string NameSelectionBrush
		{
			get
			{
				return this._nameSelectionBrush;
			}
			set
			{
				if (value != this._nameSelectionBrush)
				{
					this._nameSelectionBrush = value;
					base.OnPropertyChangedWithValue(value, "NameSelectionBrush");
				}
			}
		}

		[DataSourceProperty]
		public bool IsHighlighted
		{
			get
			{
				return this._isHighlighted;
			}
			set
			{
				if (value != this._isHighlighted)
				{
					this._isHighlighted = value;
					base.OnPropertyChangedWithValue(value, "IsHighlighted");
				}
			}
		}


		public bool _isBookmarked;


		public string _id;

		public string _name;
		public string _comparedValue;

		public Hero _itemHero;
		public PreviewOverviewViewModel _previewOverviewViewModel;

		public bool _isSelected;
		public bool _isHighlighted;

		private string _nameSelectionBrush;
	}
}
