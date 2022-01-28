using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BannerlordBattleMod
{
    public class SubModule : MBSubModuleBase
    {
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			try
			{
				new Harmony("mod.bannerlordbattlemod").PatchAll();
			}
			catch (Exception ex)
			{
				InformationManager.DisplayMessage(new InformationMessage(ex.Message));//GenericHelpers.LogException("Patch Failed", ex);
			}
		}


		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);

			CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
			if (campaignGameStarter != null)
			{
				
				this._mainManager = new MainManager();
				campaignGameStarter.AddBehavior(this._mainManager);

			}
		}

		public override void OnCampaignStart(Game game, object gameStarterObject)
		{
			base.OnCampaignStart(game, gameStarterObject);
			CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
			if (campaignGameStarter != null)
			{
				this._mainManager = new MainManager();
				campaignGameStarter.AddBehavior(this._mainManager);

			}
		}

		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			this._mainManager = null;
		}


		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			if (this._mainManager != null)
			{
				this._mainManager.TickCampaignBehavior();
			}
		}

		private MainManager _mainManager;
	}
}