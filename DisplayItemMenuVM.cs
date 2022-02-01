using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordBattleMod
{
    public class DisplayItemMenuVM : ViewModel
    {
		public DisplayItemMenuVM()
		{
			//this._resetComparedItems = resetComparedItems;
			_isComparing = false;
			this._comparedItemProperties = new MBBindingList<ItemMenuTooltipPropertyVM>();
			this._targetItemProperties = new MBBindingList<ItemMenuTooltipPropertyVM>();
			//this._getItemUsageSetFlags = getItemUsageSetFlags;
			//this._getEquipmentAtIndex = getEquipmentAtIndex;
			this.TargetItemFlagList = new MBBindingList<ItemFlagVM>();
			//this.ComparedItemFlagList = new MBBindingList<ItemFlagVM>();
			//this.AlternativeUsages = new MBBindingList<StringItemWithHintVM>();
			//this._tradeRumorsBehavior = Campaign.Current.GetCampaignBehavior<ITradeRumorCampaignBehavior>();
		}

		public void SetItem(DisplayItemVM item, BasicCharacterObject character = null, int alternativeUsageIndex = 0)
		{
			this.IsInitializationOver = false;
			this._targetItem = item;

			ItemVM comparedItem2 = this._comparedItem;
			object obj;
			EquipmentElement equipmentElement;

			this._character = character;
			this.ImageIdentifier = item.ImageIdentifier;
			this.ItemName = item.ItemDescription;
			this.TargetItemProperties.Clear();
			this.ComparedItemProperties.Clear();
			this.TargetItemFlagList.Clear();
			equipmentElement = item.ItemRosterElement.EquipmentElement;
			if (equipmentElement.Item.HasArmorComponent)
			{
				this.SetArmorComponentTooltip();
			}
			else
			{
				equipmentElement = item.ItemRosterElement.EquipmentElement;
				if (equipmentElement.Item.WeaponComponent != null)
				{
					equipmentElement = this._targetItem.ItemRosterElement.EquipmentElement;
					//this.SetWeaponComponentTooltip(equipmentElement, alternativeUsageIndex, EquipmentElement.Invalid, -1, true);
				}
				else
				{
					equipmentElement = item.ItemRosterElement.EquipmentElement;
					if (equipmentElement.Item.HasHorseComponent)
					{
						//this.SetHorseComponentTooltip();
					}
				}
			}
			equipmentElement = item.ItemRosterElement.EquipmentElement;


			this.IsInitializationOver = true;
		}


		private void AddIntProperty(TextObject description, int targetValue, int? comparedValue)
		{
			string value = targetValue.ToString();
			/*if (this.IsComparing && comparedValue != null)
			{
				string value2 = comparedValue.Value.ToString();
				int result = this.CompareValues(targetValue, comparedValue.Value);
				this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				this.CreateColoredProperty(this.ComparedItemProperties, " ", value2, this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}*/
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(0, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00015284 File Offset: 0x00013484
		private void AddFloatProperty(TextObject description, Func<EquipmentElement, float> func, bool reversedCompare = false)
		{
			float targetValue = func(this._targetItem.ItemRosterElement.EquipmentElement);
			float? comparedValue = null;
			/*if (this.IsComparing && this._comparedItem != null)
			{
				comparedValue = new float?(func(this._comparedItem.ItemRosterElement.EquipmentElement));
			}*/
			this.AddFloatProperty(description, targetValue, comparedValue, reversedCompare);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000152E8 File Offset: 0x000134E8
		private void AddFloatProperty(TextObject description, float targetValue, float? comparedValue, bool reversedCompare = false)
		{
			string value = targetValue.ToString("0.0");
			/*if (this.IsComparing && comparedValue != null)
			{
				string value2 = comparedValue.Value.ToString("0.0");
				int num = this.CompareValues(targetValue, comparedValue.Value);
				if (reversedCompare)
				{
					num *= -1;
				}
				this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(num, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				this.CreateColoredProperty(this.ComparedItemProperties, " ", value2, this.GetColorFromComparison(num, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}*/
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(0, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		private void AddSwingDamageProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedSwingDamageForUsage = equipmentElement.GetModifiedSwingDamageForUsage(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			string value = ItemHelper.GetSwingDamageText(weaponWithUsageIndex, equipmentElement.ItemModifier).ToString();
			/*if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedSwingDamageForUsage2 = equipmentElement.GetModifiedSwingDamageForUsage(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					WeaponComponentData weaponWithUsageIndex2 = equipmentElement.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					string value2 = ItemHelper.GetSwingDamageText(weaponWithUsageIndex2, equipmentElement.ItemModifier).ToString();
					int result = this.CompareValues(modifiedSwingDamageForUsage, modifiedSwingDamageForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", value2, this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}*/
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}


		private Color GetColorFromComparison(int result, bool isCompared)
		{
			if (result != -1)
			{
				if (result != 1)
				{
					return Colors.Black;
				}
				if (!isCompared)
				{
					return UIColors.PositiveIndicator;
				}
				return UIColors.NegativeIndicator;
			}
			else
			{
				if (!isCompared)
				{
					return UIColors.NegativeIndicator;
				}
				return UIColors.PositiveIndicator;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00015594 File Offset: 0x00013794
		private void AddMissileDamageProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedMissileDamageForUsage = equipmentElement.GetModifiedMissileDamageForUsage(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			string value = ItemHelper.GetMissileDamageText(weaponWithUsageIndex, equipmentElement.ItemModifier).ToString();
			/*if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedMissileDamageForUsage2 = equipmentElement.GetModifiedMissileDamageForUsage(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					WeaponComponentData weaponWithUsageIndex2 = equipmentElement.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					string value2 = ItemHelper.GetMissileDamageText(weaponWithUsageIndex2, equipmentElement.ItemModifier).ToString();
					int result = this.CompareValues(modifiedMissileDamageForUsage, modifiedMissileDamageForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", value2, this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}*/
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000156AC File Offset: 0x000138AC
		private void AddThrustDamageProperty(TextObject description, in EquipmentElement targetWeapon, int targetWeaponUsageIndex, in EquipmentElement comparedWeapon, int comparedWeaponUsageIndex)
		{
			EquipmentElement equipmentElement = targetWeapon;
			int modifiedThrustDamageForUsage = equipmentElement.GetModifiedThrustDamageForUsage(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			WeaponComponentData weaponWithUsageIndex = equipmentElement.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
			equipmentElement = targetWeapon;
			string value = ItemHelper.GetThrustDamageText(weaponWithUsageIndex, equipmentElement.ItemModifier).ToString();
			/*if (this.IsComparing)
			{
				equipmentElement = comparedWeapon;
				if (!equipmentElement.IsEmpty)
				{
					equipmentElement = comparedWeapon;
					int modifiedThrustDamageForUsage2 = equipmentElement.GetModifiedThrustDamageForUsage(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					WeaponComponentData weaponWithUsageIndex2 = equipmentElement.Item.GetWeaponWithUsageIndex(comparedWeaponUsageIndex);
					equipmentElement = comparedWeapon;
					string value2 = ItemHelper.GetThrustDamageText(weaponWithUsageIndex2, equipmentElement.ItemModifier).ToString();
					int result = this.CompareValues(modifiedThrustDamageForUsage, modifiedThrustDamageForUsage2);
					this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					this.CreateColoredProperty(this.ComparedItemProperties, " ", value2, this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					return;
				}
			}*/
			this.CreateColoredProperty(this.TargetItemProperties, description.ToString(), value, this.GetColorFromComparison(0, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
		}


		private void SetArmorComponentTooltip()
		{
			int value = 0;
			if (this._comparedItem != null && this._comparedItem.ItemRosterElement.EquipmentElement.Item != null)
			{
				value = (int)(this._comparedItem.ItemRosterElement.EquipmentElement.Item.Tier + 1);
			}
			this.AddIntProperty(this._armorTierText, (int)(this._targetItem.ItemRosterElement.EquipmentElement.Item.Tier + 1), new int?(value));
			this.CreateProperty(this.TargetItemProperties, this._typeText.ToString(), GameTexts.FindText("str_inventory_type_" + (int)this._targetItem.ItemRosterElement.EquipmentElement.Item.Type, null).ToString(), 0, null);
			if (this.IsComparing)
			{
				this.CreateProperty(this.ComparedItemProperties, " ", GameTexts.FindText("str_inventory_type_" + (int)this._targetItem.ItemRosterElement.EquipmentElement.Item.Type, null).ToString(), 0, null);
			}
			ArmorComponent armorComponent = this._targetItem.ItemRosterElement.EquipmentElement.Item.ArmorComponent;
			ArmorComponent armorComponent2 = this.IsComparing ? this._comparedItem.ItemRosterElement.EquipmentElement.Item.ArmorComponent : null;
			if (armorComponent.HeadArmor != 0 || (this.IsComparing && armorComponent2.HeadArmor != 0))
			{
				int result = this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor()) : 0;
				this.CreateColoredProperty(this.TargetItemProperties, this._headArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor().ToString(), this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedHeadArmor().ToString(), this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (armorComponent.BodyArmor != 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor() != 0))
			{
				if (this._targetItem.ItemType == EquipmentIndex.HorseHarness)
				{
					int result = this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor()) : 0;
					this.CreateColoredProperty(this.TargetItemProperties, this._horseArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor().ToString(), this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					if (this.IsComparing)
					{
						this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedMountBodyArmor().ToString(), this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
				else
				{
					int result = this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor()) : 0;
					this.CreateColoredProperty(this.TargetItemProperties, this._bodyArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor().ToString(), this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					if (this.IsComparing)
					{
						this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedBodyArmor().ToString(), this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
					}
				}
			}
			if (this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor() != 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor() != 0))
			{
				int result = this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor()) : 0;
				this.CreateColoredProperty(this.TargetItemProperties, this._legArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor().ToString(), this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedLegArmor().ToString(), this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}
			if (this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor() != 0 || (this.IsComparing && this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor() != 0))
			{
				int result = this.IsComparing ? this.CompareValues(this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor(), this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor()) : 0;
				this.CreateColoredProperty(this.TargetItemProperties, this._armArmorText.ToString(), this._targetItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor().ToString(), this.GetColorFromComparison(result, false), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				if (this.IsComparing)
				{
					this.CreateColoredProperty(this.ComparedItemProperties, " ", this._comparedItem.ItemRosterElement.EquipmentElement.GetModifiedArmArmor().ToString(), this.GetColorFromComparison(result, true), 0, null, TooltipProperty.TooltipPropertyFlags.None);
				}
			}

		}
		private int CompareValues(float currentValue, float comparedValue)
		{
			int num = (int)(currentValue * 10000f);
			int num2 = (int)(comparedValue * 10000f);
			if ((num != 0 && (float)MathF.Abs(num) <= MathF.Abs(currentValue)) || (num2 != 0 && (float)MathF.Abs(num2) <= MathF.Abs(currentValue)))
			{
				return 0;
			}
			return this.CompareValues(num, num2);
		}

		private ItemMenuTooltipPropertyVM CreateProperty(MBBindingList<ItemMenuTooltipPropertyVM> targetList, string definition, string value, int textHeight = 0, HintViewModel hint = null)
		{
			ItemMenuTooltipPropertyVM itemMenuTooltipPropertyVM = new ItemMenuTooltipPropertyVM(definition, value, textHeight, false, hint);
			targetList.Add(itemMenuTooltipPropertyVM);
			return itemMenuTooltipPropertyVM;
		}

		private ItemMenuTooltipPropertyVM CreateColoredProperty(MBBindingList<ItemMenuTooltipPropertyVM> targetList, string definition, string value, Color color, int textHeight = 0, HintViewModel hint = null, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			if (color == Colors.Black)
			{
				this.CreateProperty(targetList, definition, value, textHeight, hint);
				return null;
			}
			ItemMenuTooltipPropertyVM itemMenuTooltipPropertyVM = new ItemMenuTooltipPropertyVM(definition, value, textHeight, color, false, hint, propertyFlags);
			targetList.Add(itemMenuTooltipPropertyVM);
			return itemMenuTooltipPropertyVM;
		}

		[DataSourceProperty]
		public bool IsComparing
		{
			get
			{
				return this._isComparing;
			}
			set
			{
				if (value != this._isComparing)
				{
					this._isComparing = value;
					base.OnPropertyChangedWithValue(value, "IsComparing");
				}
			}
		}
		[DataSourceProperty]
		public MBBindingList<ItemMenuTooltipPropertyVM> ComparedItemProperties
		{
			get
			{
				return this._comparedItemProperties;
			}
			set
			{
				if (value != this._comparedItemProperties)
				{
					this._comparedItemProperties = value;
					base.OnPropertyChangedWithValue(value, "ComparedItemProperties");
				}
			}
		}


		[DataSourceProperty]
		public bool IsInitializationOver
		{
			get
			{
				return this._isInitializationOver;
			}
			set
			{
				if (value != this._isInitializationOver)
				{
					this._isInitializationOver = value;
					base.OnPropertyChangedWithValue(value, "IsInitializationOver");
				}
			}
		}


		[DataSourceProperty]
		public ImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue(value, "ImageIdentifier");
				}
			}
		}


		[DataSourceProperty]
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value != this._itemName)
				{
					this._itemName = value;
					base.OnPropertyChangedWithValue(value, "ItemName");
				}
			}
		}

		[DataSourceProperty]
		public MBBindingList<ItemMenuTooltipPropertyVM> TargetItemProperties
		{
			get
			{
				return this._targetItemProperties;
			}
			set
			{
				if (value != this._targetItemProperties)
				{
					this._targetItemProperties = value;
					base.OnPropertyChangedWithValue(value, "TargetItemProperties");
				}
			}
		}


		[DataSourceProperty]
		public MBBindingList<ItemFlagVM> TargetItemFlagList
		{
			get
			{
				return this._targetItemFlagList;
			}
			set
			{
				if (value != this._targetItemFlagList)
				{
					this._targetItemFlagList = value;
					base.OnPropertyChangedWithValue(value, "TargetItemFlagList");
				}
			}
		}


		private MBBindingList<ItemMenuTooltipPropertyVM> _comparedItemProperties;

		private bool _isComparing;
		private bool _isInitializationOver;
		private ImageIdentifierVM _imageIdentifier;
		private string _itemName;
		private MBBindingList<ItemMenuTooltipPropertyVM> _targetItemProperties;
		private MBBindingList<ItemFlagVM> _targetItemFlagList;
		private ItemVM _targetItem;
		private BasicCharacterObject _character;

		private ItemVM _comparedItem;

		private readonly TextObject _swingDamageText = GameTexts.FindText("str_swing_damage", null);
		private readonly TextObject _swingSpeedText = new TextObject("{=345a87fcc69f626ae3916939ef2fc135}Swing Speed: ", null);
		private readonly TextObject _weaponTierText = new TextObject("{=weaponTier}Weapon Tier: ", null);
		private readonly TextObject _armorTierText = new TextObject("{=armorTier}Armor Tier: ", null);
		private readonly TextObject _horseTierText = new TextObject("{=mountTier}Mount Tier: ", null);
		private readonly TextObject _horseTypeText = new TextObject("{=9sxECG6e}Mount Type: ", null);
		private readonly TextObject _chargeDamageText = new TextObject("{=c7638a0869219ae845de0f660fd57a9d}Charge Damage: ", null);
		private readonly TextObject _hitPointsText = GameTexts.FindText("str_hit_points", null);
		private readonly TextObject _speedText = new TextObject("{=74dc1908cb0b990e80fb977b5a0ef10d}Speed: ", null);
		private readonly TextObject _maneuverText = new TextObject("{=3025020b83b218707499f0de3135ed0a}Maneuver: ", null);
		private readonly TextObject _thrustSpeedText = GameTexts.FindText("str_thrust_speed", null);
		private readonly TextObject _thrustDamageText = GameTexts.FindText("str_thrust_damage", null);
		private readonly TextObject _lengthText = new TextObject("{=c6e4c8588ca9e42f6e1b47b11f0f367b}Length: ", null);
		private readonly TextObject _weightText = GameTexts.FindText("str_weight_text", null);
		private readonly TextObject _handlingText = new TextObject("{=ca8b1e8956057b831dfc665f54bae4b0}Handling: ", null);
		private readonly TextObject _weaponLengthText = new TextObject("{=5fa36d2798479803b4518a64beb4d732}Weapon Length: ", null);
		private readonly TextObject _damageText = new TextObject("{=c9c5dfed2ca6bcb7a73d905004c97b23}Damage: ", null);
		private readonly TextObject _missileSpeedText = GameTexts.FindText("str_missile_speed", null);
		private readonly TextObject _accuracyText = new TextObject("{=5dec16fa0be433ade3c4cb0074ef366d}Accuracy: ", null);
		private readonly TextObject _stackAmountText = new TextObject("{=05fdfc6e238429753ef282f2ce97c1f8}Stack Amount: ", null);
		private readonly TextObject _ammoLimitText = new TextObject("{=6adabc1f82216992571c3e22abc164d7}Ammo Limit: ", null);
		private readonly TextObject _requiresText = new TextObject("{=154a34f8caccfc833238cc89d38861e8}Requires: ", null);
		private readonly TextObject _foodText = new TextObject("{=qSi4DlT4}Food", null);
		private readonly TextObject _partyMoraleText = new TextObject("{=a241aacb1780599430c79fd9f667b67f}Party Morale: ", null);
		private readonly TextObject _typeText = new TextObject("{=08abd5af7774d311cadc3ed900b47754}Type: ", null);
		private readonly TextObject _tradeRumorsText = new TextObject("{=f2971dc587a9777223ad2d7be236fb05}Trade Rumors", null);
		private readonly TextObject _classText = new TextObject("{=8cad4a279770f269c4bb0dc7a357ee1e}Class: ", null);
		private readonly TextObject _headArmorText = GameTexts.FindText("str_head_armor", null);
		private readonly TextObject _horseArmorText = new TextObject("{=305cf7f98458b22e9af72b60a131714f}Horse Armor: ", null);
		private readonly TextObject _bodyArmorText = GameTexts.FindText("str_body_armor", null);
		private readonly TextObject _legArmorText = GameTexts.FindText("str_leg_armor", null);
		private readonly TextObject _armArmorText = new TextObject("{=cf61cce254c7dca65be9bebac7fb9bf5}Arm Armor: ", null);
	}
}
