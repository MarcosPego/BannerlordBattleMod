using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;

namespace BannerlordBattleMod
{
    public class MapSelectionViewModel : ViewModel
    {
        public MapSelectionViewModel()
        {
            this._battleMaps = new List<MapItemVM>();
            this._villageMaps = new List<MapItemVM>();
            this._siegeMaps = new List<MapItemVM>();
            this._availableMaps = this._battleMaps;
            this.MapSelection = new SelectorVM<MapItemVM>(0, new Action<SelectorVM<MapItemVM>>(this.OnMapSelection));
            RefreshValues();
        }
        public override void RefreshValues()
        {
            base.RefreshValues();
            PrepareMapLists();
            this.TitleText = "Test1";
            this.MapText = "Test2";

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

            Comparer<MapItemVM> comparer = Comparer<MapItemVM>.Create((MapItemVM x, MapItemVM y) => x.MapName.CompareTo(y.MapName));
            this._battleMaps.Sort(comparer);
            this._villageMaps.Sort(comparer);
            this._siegeMaps.Sort(comparer);
        }

        private void OnMapSelection(SelectorVM<MapItemVM> selector)
        {
            this.SelectedMap = selector.SelectedItem;
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
        private List<MapItemVM> _availableMaps { get; set; }

        private string _titleText;

        private string _mapText;


        private SelectorVM<MapItemVM> _mapSelection;

        private List<MapItemVM> _battleMaps { get; set; }
        private List<MapItemVM> _villageMaps { get; set; }
        private List<MapItemVM> _siegeMaps { get; set; }
    }
}
