using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordBattleMod
{
    public class DisplayItemVM : ItemVM
	{

		public DisplayItemVM()
		{
			base.StringId = "";
			base.ImageIdentifier = new ImageIdentifierVM(ImageIdentifierType.Null);
			this._itemType = EquipmentIndex.None;
		}

		public DisplayItemVM(ItemRosterElement newItem)
        {
            this.ItemRosterElement = new ItemRosterElement(newItem.EquipmentElement, newItem.Amount);
            this.ItemCount = newItem.Amount;
            this.ItemLevel = newItem.EquipmentElement.Item.Difficulty;
			base.ItemDescription = newItem.EquipmentElement.GetModifiedItemName().ToString();

			ItemObject item = newItem.EquipmentElement.Item;
			SetItemTypeId();
			base.ItemDescription = newItem.EquipmentElement.GetModifiedItemName().ToString();
			base.StringId = CampaignUIHelper.GetItemLockStringID(newItem.EquipmentElement);
			base.ImageIdentifier = new ImageIdentifierVM(item, null);

			this.UpdateHintTexts();
			RefreshValues();
		}

		protected void SetItemTypeId()
		{
			base.TypeId = (int)this.ItemRosterElement.EquipmentElement.Item.Type;
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.ItemRosterElement.EquipmentElement.Item != null)
			{
				TextObject modifiedItemName = this.ItemRosterElement.EquipmentElement.GetModifiedItemName();
				base.ItemDescription = (((modifiedItemName != null) ? modifiedItemName.ToString() : null) ?? "");
				return;
			}
			base.ItemDescription = "";
		}

		public void RefreshWith(SPItemVM itemVM)
        {
			if (itemVM == null)
			{
				this.Reset();
				return;
			}

			base.ItemDescription = itemVM.ItemDescription;
			base.ItemCost = itemVM.ItemCost;
			base.TypeId = itemVM.TypeId;
			this._itemType = itemVM.ItemType;
			this.ItemCount = itemVM.ItemCount;
			//this.TransactionCount = itemVM.TransactionCount;
			this.ItemLevel = itemVM.ItemLevel;
			base.StringId = itemVM.StringId;
			base.ImageIdentifier = itemVM.ImageIdentifier.Clone();
			this.ItemRosterElement = itemVM.ItemRosterElement;
			/*this.IsCivilianItem = itemVM.IsCivilianItem;
			this.IsGenderDifferent = itemVM.IsGenderDifferent;
			this.IsEquipableItem = itemVM.IsEquipableItem;
			this.CanCharacterUseItem = this.CanCharacterUseItem;
			this.IsArtifact = itemVM.IsArtifact;
			this.UpdateCanBeSlaughtered();*/
			this.UpdateHintTexts();
			/*InventoryLogic inventoryLogic = this._inventoryLogic;
			this.CanBeDonated = (inventoryLogic != null && inventoryLogic.CanDonateItem(this.ItemRosterElement, this.InventorySide));
			this.TradeData = new InventoryTradeVM(this._inventoryLogic, itemVM.ItemRosterElement, inventorySide, new Action<int, bool>(this.OnTradeApplyTransaction));
			this.UpdateIsNew();
			this.UpdateProfitType();*/
		}

		private void Reset()
		{
			base.ItemDescription = "";
			base.ItemCost = 0;
			base.TypeId = 0;
			this._itemType = EquipmentIndex.None;
			this.ItemCount = 0;
			//this.TransactionCount = 0;
			this.ItemLevel = 0;
			base.StringId = "";
			base.ImageIdentifier = new ImageIdentifierVM(ImageIdentifierType.Null);
			this.ItemRosterElement = default(ItemRosterElement);
			//this.ProfitType = 0;
			//this.IsCivilianItem = true;
			//this.IsGenderDifferent = false;
			//this.IsEquipableItem = true;
			//this.IsArtifact = false;
			//this.TradeData = new InventoryTradeVM(this._inventoryLogic, this.ItemRosterElement, InventoryLogic.InventorySide.None, new Action<int, bool>(this.OnTradeApplyTransaction));
			//this.UpdateIsNew();
		}

		public void UpdateHintTexts()
		{
			base.PreviewHint = new HintViewModel(GameTexts.FindText("str_inventory_preview", null), null);
			base.EquipHint = new HintViewModel(GameTexts.FindText("str_inventory_equip", null), null);
		}





		[DataSourceProperty]
		public int ItemCount
		{
			get
			{
				return this._count;
			}
			set
			{
				if (value != this._count)
				{
					this._count = value;
					base.OnPropertyChangedWithValue(value, "ItemCount");
					//this.UpdateTotalCost();
					//this.UpdateTradeData(false);
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001C975 File Offset: 0x0001AB75
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0001C97D File Offset: 0x0001AB7D
		[DataSourceProperty]
		public int ItemLevel
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "ItemLevel");
				}
			}
		}


		[DataSourceProperty]
        public int TypeId
        {
            get
            {
                return this._typeId;
            }
            set
            {
                if (value != this._typeId)
                {
                    this._typeId = value;
                    base.OnPropertyChangedWithValue(value, "TypeId");
                }
            }
        }

		private int _count;
		private int _level;

		private int _typeId;
        public ItemRosterElement ItemRosterElement;
    }
}
