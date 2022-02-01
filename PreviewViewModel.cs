using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace BannerlordBattleMod
{
    public class PreviewViewModel : ViewModel
    {
		public PreviewViewModel()
		{
			PreviewOverview = new PreviewOverviewViewModel(this);

			RefreshValues();
		}

		public override void RefreshValues()
        {

			base.RefreshValues();

			TimeSpan result = TimeSpan.FromMilliseconds(TimerHandler.TotalPreviewTimer);
		
			this.Timer = String.Format("{0:00}:{1:00}", result.Minutes, result.Seconds);
		}


		public void AdvanceScreen()
        {
			
			List<TroopRoster> troopRosters = RosterHelper.RandomRostersFromList(_previewOverview.PreviewItems);

			ScreenManager.PushScreen(new BattleDisplayScreen(this, troopRosters[0], troopRosters[1]));

		}

		[DataSourceProperty]
		public string Timer
		{
			get
			{
				return this._timer;
			}
			set
			{
				if (value != this._timer)
				{
					this._timer = value;
					base.OnPropertyChangedWithValue(value, "Timer");
				}
			}
		}




		[DataSourceProperty]
		public PreviewOverviewViewModel PreviewOverview
		{
			get
			{
				return this._previewOverview;
			}
			set
			{
				if (value != this._previewOverview)
				{
					this._previewOverview = value;
					base.OnPropertyChangedWithValue(value, "PreviewOverview");
				}
			}
		}


		private string _timer;
		public PreviewOverviewViewModel _previewOverview;

	}
}
