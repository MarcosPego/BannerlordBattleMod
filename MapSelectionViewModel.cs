using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;

namespace BannerlordBattleMod
{
    public class MapSelectionViewModel : ViewModel
    {
        public MapSelectionViewModel(BattleDisplayViewModel battleDisplayViewModel)
        {
            this._battleDisplayViewModel = battleDisplayViewModel;
            this._battleMaps = new List<MapItemVM>();
            this._villageMaps = new List<MapItemVM>();
            this._siegeMaps = new List<MapItemVM>();
            this._availableMaps = this._battleMaps;
            _currentBattleType = CustomBattleGameType.Battle;
            this.MapSelection = new SelectorVM<MapItemVM>(0, new Action<SelectorVM<MapItemVM>>(this.OnMapSelection));

            this.TitleText = "Map Selection";
            this.MapText = "Map";
            UpdateMapLists();
            RefreshValues();
        }
        public override void RefreshValues()
        {
            base.RefreshValues();




        }

        public void UpdateMapLists()
        {
            PrepareMapLists();

            this.MapSelection.ItemList.Clear();
            foreach (MapItemVM mapItemVM in this._availableMaps)
            {
                this.MapSelection.AddItem(new MapItemVM(mapItemVM.MapName, mapItemVM.MapId));
            }
            this.MapSelection.SelectedIndex = 0;
        }

        public void PrepareMapLists()
        {
            this._battleMaps.Clear();
            this._villageMaps.Clear();
            this._siegeMaps.Clear();

            Serializer.ParseCustomBattleScenes();

            foreach (CustomBattleSceneData customBattleSceneData in Serializer.CustomBattleScenes)
            {
                MapItemVM item = new MapItemVM(customBattleSceneData.Name.ToString(), customBattleSceneData.SceneID);

                //InformationManager.DisplayMessage(new InformationMessage(customBattleSceneData.Name.ToString()));
                //nformationManager.DisplayMessage(new InformationMessage(customBattleSceneData.SceneID.ToString()));
                this._battleMaps.Add(item);
                
            }

            if (_battleDisplayViewModel.IsDefaultMapsSelected)
            {
                Serializer.ParseCustomBattleScenes(false);

                foreach (CustomBattleSceneData customBattleSceneData in Serializer.CustomBattleScenes)
                {
                    MapItemVM item = new MapItemVM(customBattleSceneData.Name.ToString(), customBattleSceneData.SceneID);
                    if (customBattleSceneData.IsVillageMap)
                    {
                        this._villageMaps.Add(item);
                    }
                    else if (customBattleSceneData.IsSiegeMap)
                    {
                        this._siegeMaps.Add(item);
                    }
                    else if (!customBattleSceneData.IsLordsHallMap)
                    {
                        this._battleMaps.Add(item);
                    }
                }
            }

            Comparer<MapItemVM> comparer = Comparer<MapItemVM>.Create((MapItemVM x, MapItemVM y) => x.MapId.CompareTo(y.MapId));
            this._battleMaps.Sort(comparer);
            this._villageMaps.Sort(comparer);
            this._siegeMaps.Sort(comparer);

            //OnGameTypeChange(_currentBattleType);
        }

        private void OnMapSelection(SelectorVM<MapItemVM> selector)
        {
            this.SelectedMap = selector.SelectedItem;
        }

        public void OnGameTypeChange(CustomBattleGameType gameType)
        {
            this.MapSelection.ItemList.Clear();
            _currentBattleType = gameType;
            if (gameType == CustomBattleGameType.Battle)
            {

                this._availableMaps = this._battleMaps;
            }
            else if (gameType == CustomBattleGameType.Village)
            {

                this._availableMaps = this._villageMaps;
            }
            else if (gameType == CustomBattleGameType.Siege)
            {

                this._availableMaps = this._siegeMaps;
            }
            foreach (MapItemVM item in this._availableMaps)
            {
                this.MapSelection.AddItem(item);
            }
            this.MapSelection.SelectedIndex = 0;
        }


        [DataSourceProperty]
        public SelectorVM<MapItemVM> MapSelection
        {
            get
            {
                return this._mapSelection;
            }
            set
            {
                if (value != this._mapSelection)
                {
                    this._mapSelection = value;
                    base.OnPropertyChangedWithValue(value, "MapSelection");
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
        public string MapText
        {
            get
            {
                return this._mapText;
            }
            set
            {
                if (value != this._mapText)
                {
                    this._mapText = value;
                    base.OnPropertyChangedWithValue(value, "MapText");
                }
            }
        }

        public MapItemVM SelectedMap { get; private set; }
        public List<MapItemVM> _availableMaps { get; set; }

        private string _titleText;

        private string _mapText;


        private SelectorVM<MapItemVM> _mapSelection;

        private List<MapItemVM> _battleMaps { get; set; }
        private List<MapItemVM> _villageMaps { get; set; }
        private List<MapItemVM> _siegeMaps { get; set; }

        private BattleDisplayViewModel _battleDisplayViewModel;


        private CustomBattleGameType _currentBattleType;
    }
}
