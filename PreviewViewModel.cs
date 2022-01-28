using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.Library;

namespace BannerlordBattleMod
{
    public class PreviewViewModel : ViewModel
    {
		public PreviewViewModel()
		{
			PreviewOverview = new PreviewOverviewViewModel();


		}

		public override void RefreshValues()
        {

			base.RefreshValues();


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



		public PreviewOverviewViewModel _previewOverview;

	}
}
