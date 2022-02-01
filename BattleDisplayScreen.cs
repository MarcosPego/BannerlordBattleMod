using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace BannerlordBattleMod
{
    class BattleDisplayScreen : ScreenBase, IGameStateListener
    {

		public BattleDisplayScreen(PreviewViewModel previewViewModel, TroopRoster leftSide, TroopRoster rightSide)
		{
			_previewViewModel = previewViewModel;
			_leftSide = leftSide;
			_rightSide = rightSide;
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._viewModel = new BattleDisplayViewModel(_previewViewModel, _leftSide, _rightSide);
			TimerHandler.CurrentBattleDisplayViewModel = this._viewModel;

			//this._viewModel.SetStartInputKey(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory").RegisteredHotKeys.FirstOrDefault((HotKey g) => ((g != null) ? g.Id : null) == "Start"));
			//this._viewModel.SetCancelInputKey(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory").RegisteredHotKeys.FirstOrDefault((HotKey g) => ((g != null) ? g.Id : null) == "Exit"));
			//this._viewModel.SetRandomizeInputKey(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory").RegisteredHotKeys.FirstOrDefault((HotKey g) => ((g != null) ? g.Id : null) == "Randomize"));

			this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
			this._gauntletLayer.LoadMovie("BattleDisplay", this._viewModel);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, TaleWorlds.Library.InputUsageMask.All);
			base.AddLayer(this._gauntletLayer);

		}

		void IGameStateListener.OnActivate()
		{
		}

		void IGameStateListener.OnDeactivate()
		{
		}

		void IGameStateListener.OnInitialize()
		{
		}

		void IGameStateListener.OnFinalize()
		{
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			SpriteData spriteData = UIResourceManager.SpriteData;
			TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
			ResourceDepot uiresourceDepot = UIResourceManager.UIResourceDepot;

			this._clanCategory = spriteData.SpriteCategories["ui_clan"];
			this._clanCategory.Load(resourceContext, uiresourceDepot);

			this._partyscreenCategory = spriteData.SpriteCategories["ui_partyscreen"];
			this._partyscreenCategory.Load(resourceContext, uiresourceDepot);

			this._encyclopediaCategory = spriteData.SpriteCategories["ui_encyclopedia"];
			this._encyclopediaCategory.Load(resourceContext, uiresourceDepot);

			ScreenManager.TrySetFocus(_gauntletLayer);
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(_gauntletLayer);
		}

		protected override void OnFinalize()
		{
			base.OnFinalize();

			this._partyscreenCategory.Unload();
			this._clanCategory.Unload();
			this._encyclopediaCategory.Unload();
            try
			{
				base.RemoveLayer(this._gauntletLayer);
			}
            catch
            {

            }

			this._gauntletLayer = null;
			this._viewModel = null;
			TimerHandler.CurrentBattleDisplayViewModel = null;
		}

		private GauntletLayer _gauntletLayer;
		public BattleDisplayViewModel _viewModel;
		//private PartyManagerLogic _partyManagerLogic;
		private SpriteCategory _partyscreenCategory;
		private SpriteCategory _clanCategory;
		private SpriteCategory _encyclopediaCategory;

		private MapNavigationHandler _mapNavigationHandler;

		private TroopRoster _leftSide;
		private TroopRoster _rightSide;

		private PreviewViewModel _previewViewModel;
	}
}
