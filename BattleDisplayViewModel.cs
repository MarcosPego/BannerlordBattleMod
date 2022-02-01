using SandBox.ViewModelCollection.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

namespace BannerlordBattleMod
{
    public class BattleDisplayViewModel : ViewModel
    {
		//TroopRoster leftSide, TroopRoster rigthSide
		public BattleDisplayViewModel(PreviewViewModel previewViewModel, TroopRoster leftSide, TroopRoster rightSide)
        {
			_startBattle = false;

			_previewViewModel = previewViewModel;
			this._isDefaultMapsSelected = true;
			_leftRoster = leftSide;
			_rightRoster = rightSide;
			//_rightRoster = new TroopRoster(PartyBase.MainParty);
			this.OtherPartyTroops = new MBBindingList<CharacterVM>();
			this.MainPartyTroops = new MBBindingList<CharacterVM>();


			this.MapSelectionGroup = new MapSelectionViewModel(this);
            this.GameTypeSelectionGroup = new GameTypeSelectionGroupVM(new Action<CustomBattlePlayerType>(this.OnPlayerTypeChange), new Action<CustomBattleGameType>(this.OnGameTypeChange));

			InitializePartyList(OtherPartyTroops, _leftRoster);
			InitializePartyList(MainPartyTroops, _rightRoster);
			this.TitleText = "BattleDisplay";
			this.RandomizeButtonText = GameTexts.FindText("str_randomize", null).ToString();
			this.StartButtonText = GameTexts.FindText("str_start", null).ToString();
			this.BackButtonText = GameTexts.FindText("str_back", null).ToString();


			TimerHandler.ResetTimers();
			TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerBattleDisplayCallback, this, 0, 1000);
			RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();




			TimeSpan result = TimeSpan.FromMilliseconds(TimerHandler.TotalBattleDisplayTimer);

			this.Timer = String.Format("{0:00}:{1:00}", result.Minutes, result.Seconds);


			
			
			this.MapSelectionGroup.RefreshValues();

			this.MainPartyTroopsLbl = "Team B: " + MainPartyTroops.Count;
			this.OtherPartyTroopsLbl = "Team A: " + OtherPartyTroops.Count;
		}



		private void OnPlayerTypeChange(CustomBattlePlayerType playerType)
		{
			//this.PlayerSide.OnPlayerTypeChange(playerType);
		}

		private void OnGameTypeChange(CustomBattleGameType gameType)
		{
			this.MapSelectionGroup.OnGameTypeChange(gameType);
		}

		private void InitializePartyList(MBBindingList<CharacterVM> partyList, TroopRoster currentTroopRoster)
		{
			partyList.Clear();
			for (int i = 0; i < currentTroopRoster.Count; i++)
			{
				if(currentTroopRoster.GetElementCopyAtIndex(i).Character.HeroObject != PartyBase.MainParty.LeaderHero)
                {
					TroopRosterElement elementCopyAtIndex = currentTroopRoster.GetElementCopyAtIndex(i);
					CharacterVM partyCharacterVM = new CharacterVM(currentTroopRoster, i);
					//PartyCharacterVM partyCharacterVM = new PartyCharacterVM(this.PartyScreenLogic, new Action<PartyCharacterVM, bool>(this.ProcessCharacterLock), new Action<PartyCharacterVM>(this.SetSelectedCharacter), new Action<PartyCharacterVM, int, int, PartyScreenLogic.PartyRosterSide>(this.OnTransferTroop), null, this, currentTroopRoster, i, type, (PartyScreenLogic.PartyRosterSide)side, this.PartyScreenLogic.IsTroopTransferable(type, elementCopyAtIndex.Character, side), this._fiveStackShortcutkeyText, this._entireStackShortcutkeyText);
					partyList.Add(partyCharacterVM);
					partyCharacterVM.ThrowOnPropertyChanged();
					//partyCharacterVM.IsLocked = (partyCharacterVM.Side == PartyScreenLogic.PartyRosterSide.Right && this.IsTroopLocked(partyCharacterVM.Troop, partyCharacterVM.IsPrisoner));
				}


			}
		}

		public void ShouldStartBattle()
        {
			if (_startBattle)
			{
				StartBattle();
				_startBattle = false;
			}
		}

		public void StartBattle()
        {
			//InformationManager.DisplayMessage(new InformationMessage(_rightRoster.Count.ToString()));
			//InformationManager.DisplayMessage(new InformationMessage(PartyBase.MainParty.Position2D.ToString()));
			//InformationManager.DisplayMessage(new InformationMessage(MapSelectionGroup.SelectedMap.MapId.ToString()));
			PartySpawnHelper.SpawnPartyFromRoster(_rightRoster, PartyBase.MainParty.Position2D, MapSelectionGroup.SelectedMap.MapId, false);
		}

		public void ExecuteStart()
		{
			//TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerBattleDisplayCallback, this, 0, 1000);
			TimerHandler.ResetTimers();
			StartBattle();
			//TimerHandler.timerRunning = new System.Threading.Timer(TimerHandler.TimerBattleDisplayCallback, this, 0, 1000);
			//StartBattle();
		}

		public void ExecuteBack()
		{
			_previewViewModel.RefreshValues();
			_previewViewModel._previewOverview.RefreshTimerCallbaks();
			ScreenManager.PopScreen();
		}

		public void ExecuteRandomize()
		{
			//PartySpawnHelper.SpawnPartyFromRoster(_rightRoster, PartyBase.MainParty.Position2D);
			var toopRosters = RosterHelper.RandomRostersFromRosters( _leftRoster,  _rightRoster);
			_leftRoster = toopRosters[0];
			_rightRoster = toopRosters[1];
			InitializePartyList(OtherPartyTroops, _leftRoster);
			InitializePartyList(MainPartyTroops, _rightRoster);

			Random rnd = new Random();
			this.MapSelectionGroup.MapSelection.SelectedIndex = rnd.Next(0, this.MapSelectionGroup._availableMaps.Count);
			RefreshValues();
		}


		public void IsDefaultMaps()
		{
			_isDefaultMapsSelected = !_isDefaultMapsSelected;
			this.MapSelectionGroup.UpdateMapLists();
			//InformationManager.DisplayMessage(new InformationMessage(IsDefaultMapsSelected.ToString()));
		}


		public void SetStartInputKey(HotKey hotkey)
		{
			this.StartInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
			//TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this.TroopTypeSelectionPopUp;
			/*if (troopTypeSelectionPopUp == null)
			{
				return;
			}
			troopTypeSelectionPopUp.SetCancelInputKey(hotkey);*/
		}

		public void SetRandomizeInputKey(HotKey hotkey)
		{
			this.RandomizeInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
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
		public bool IsDefaultMapsSelected
		{
			get
			{
				return this._isDefaultMapsSelected;
			}
			set
			{
				if (value != this._isDefaultMapsSelected)
				{
					this._isDefaultMapsSelected = value;
					base.OnPropertyChangedWithValue(value, "IsDefaultMapsSelected");
				}
			}
		}

		[DataSourceProperty]
		public MapSelectionViewModel MapSelectionGroup
		{
			get
			{
				return this._mapSelectionGroup;
			}
			set
			{
				if (value != this._mapSelectionGroup)
				{
					this._mapSelectionGroup = value;
					base.OnPropertyChangedWithValue(value, "MapSelectionGroup");
				}
			}
		}

		[DataSourceProperty]
		public GameTypeSelectionGroupVM GameTypeSelectionGroup
		{
			get
			{
				return this._gameTypeSelectionGroup;
			}
			set
			{
				if (value != this._gameTypeSelectionGroup)
				{
					this._gameTypeSelectionGroup = value;
					base.OnPropertyChangedWithValue(value, "GameTypeSelectionGroup");
				}
			}
		}

		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue(value, "TitleText");
				}
			}
		}

		[DataSourceProperty]
		public string RandomizeButtonText
		{
			get
			{
				return this._randomizeButtonText;
			}
			set
			{
				if (value != this._randomizeButtonText)
				{
					this._randomizeButtonText = value;
					base.OnPropertyChangedWithValue(value, "RandomizeButtonText");
				}
			}
		}

		[DataSourceProperty]
		public string BackButtonText
		{
			get
			{
				return this._backButtonText;
			}
			set
			{
				if (value != this._backButtonText)
				{
					this._backButtonText = value;
					base.OnPropertyChangedWithValue(value, "BackButtonText");
				}
			}
		}


		[DataSourceProperty]
		public string StartButtonText
		{
			get
			{
				return this._startButtonText;
			}
			set
			{
				if (value != this._startButtonText)
				{
					this._startButtonText = value;
					base.OnPropertyChangedWithValue(value, "StartButtonText");
				}
			}
		}


		//Troops

		[DataSourceProperty]
		public MBBindingList<CharacterVM> OtherPartyTroops
		{
			get
			{
				return this._otherPartyTroops;
			}
			set
			{
				if (value != this._otherPartyTroops)
				{
					this._otherPartyTroops = value;
					base.OnPropertyChangedWithValue(value, "OtherPartyTroops");
				}
			}
		}
		[DataSourceProperty]
		public MBBindingList<CharacterVM> MainPartyTroops
		{
			get
			{
				return this._mainPartyTroops;
			}
			set
			{
				if (value != this._mainPartyTroops)
				{
					this._mainPartyTroops = value;
					base.OnPropertyChangedWithValue(value, "MainPartyTroops");
				}
			}
		}


		[DataSourceProperty]
		public string TroopsLabel
		{
			get
			{
				return this._troopsLbl;
			}
			set
			{
				if (value != this._troopsLbl)
				{
					this._troopsLbl = value;
					base.OnPropertyChangedWithValue(value, "TroopsLabel");
				}
			}
		}


		[DataSourceProperty]
		public string MainPartyTroopsLbl
		{
			get
			{
				return this._mainPartyTroopsLbl;
			}
			set
			{
				if (value != this._mainPartyTroopsLbl)
				{
					this._mainPartyTroopsLbl = value;
					base.OnPropertyChangedWithValue(value, "MainPartyTroopsLbl");
				}
			}
		}


		[DataSourceProperty]
		public string OtherPartyTroopsLbl
		{
			get
			{
				return this._otherPartyTroopsLbl;
			}
			set
			{
				if (value != this._otherPartyTroopsLbl)
				{
					this._otherPartyTroopsLbl = value;
					base.OnPropertyChangedWithValue(value, "OtherPartyTroopsLbl");
				}
			}
		}


		public InputKeyItemVM StartInputKey
		{
			get
			{
				return this._startInputKey;
			}
			set
			{
				if (value != this._startInputKey)
				{
					this._startInputKey = value;
					base.OnPropertyChangedWithValue(value, "StartInputKey");
				}
			}
		}

		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue(value, "CancelInputKey");
				}
			}
		}

		public InputKeyItemVM RandomizeInputKey
		{
			get
			{
				return this._randomizeInputKey;
			}
			set
			{
				if (value != this._randomizeInputKey)
				{
					this._randomizeInputKey = value;
					base.OnPropertyChangedWithValue(value, "RandomizeInputKey");
				}
			}
		}


		private MBBindingList<CharacterVM> _otherPartyTroops;
		private MBBindingList<CharacterVM> _mainPartyTroops;



		private GameTypeSelectionGroupVM _gameTypeSelectionGroup;


		private MapSelectionViewModel _mapSelectionGroup;

		private bool _isDefaultMapsSelected;

		private string _titleText;
		private string _randomizeButtonText;
		private string _backButtonText;
		private string _startButtonText;

		private string _troopsLbl;
		private string _otherPartyTroopsLbl;
		private string _mainPartyTroopsLbl;

		private string _timer;

		private TroopRoster _leftRoster;
		private TroopRoster _rightRoster;

		private InputKeyItemVM _startInputKey;
		private InputKeyItemVM _cancelInputKey;
		private InputKeyItemVM _randomizeInputKey;

		private PreviewViewModel _previewViewModel;

		public bool _startBattle;
	}
}
