using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace BannerlordBattleMod
{
    public class CharacterVM : ViewModel
    {

		public TroopRosterElement Troop
		{
			get
			{
				return this._troop;
			}
			set
			{
				this._troop = value;
				this.Character = value.Character;
				this.TroopID = this.Character.StringId;
				//this.CheckTransferAmountDefaultValue();
				//this.TroopXPTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopXPTooltip(value));
				//this.TroopConformityTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopConformityTooltip(value));
			}
		}

		public CharacterObject Character
		{
			get
			{
				return this._character;
			}
			set
			{
				this._character = value;
				CharacterCode characterCode = this.GetCharacterCode(value, PartyScreenLogic.TroopType.Member, PartyScreenLogic.PartyRosterSide.Left);
				this.Code = new ImageIdentifierVM(characterCode);
				/*if (this._character.UpgradeTargets.Length != 0)
				{
					this.Upgrades = new MBBindingList<UpgradeTargetVM>();
					for (int i = 0; i < this._character.UpgradeTargets.Length; i++)
					{
						CharacterCode characterCode2 = this.GetCharacterCode(this._character.UpgradeTargets[i], this.Type, this.Side);
						this.Upgrades.Add(new UpgradeTargetVM(i, value, characterCode2, new Action<int, int>(this.Upgrade)));
					}
					//this.HasMoreThanTwoUpgrades = (this.Upgrades.Count > 2);
				}*/
				//this.CheckTransferAmountDefaultValue();
			}
		}

		public CharacterVM(TroopRoster troops, int index)
		{

			this.Troop = troops.GetElementCopyAtIndex(index);
			this.Index = index;
			this.IsHero = this.Troop.Character.IsHero;
			IsMainHero = true;



			RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Troop.Character.Name.ToString();

		}

		public void ThrowOnPropertyChanged()
		{
			base.OnPropertyChanged("Name");
			base.OnPropertyChanged("Number");
			/*base.OnPropertyChanged("WoundedCount");
			base.OnPropertyChanged("IsTroopTransferrable");
			base.OnPropertyChanged("MaxCount");
			base.OnPropertyChanged("AmountOfUpgrades");
			base.OnPropertyChanged("Level");
			base.OnPropertyChanged("PartyIndex");
			base.OnPropertyChanged("Index");
			base.OnPropertyChanged("TroopNum");
			base.OnPropertyChanged("TransferString");
			base.OnPropertyChanged("CanTalk");*/
		}

		[DataSourceProperty]
		public string TroopID
		{
			get
			{
				return this._troopID;
			}
			set
			{
				if (value != this._troopID)
				{
					this._troopID = value;
					base.OnPropertyChangedWithValue(value, "TroopID");
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
		public ImageIdentifierVM Code
		{
			get
			{
				return this._code;
			}
			set
			{
				if (value != this._code)
				{
					this._code = value;
					base.OnPropertyChangedWithValue(value, "Code");
				}
			}
		}

		private CharacterCode GetCharacterCode(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side)
		{
			IFaction faction = null;
			if (type != PartyScreenLogic.TroopType.Prisoner)
			{
				/*if (side == PartyScreenLogic.PartyRosterSide.Left && this._partyScreenLogic.LeftOwnerParty != null)
				{
					faction = this._partyScreenLogic.LeftOwnerParty.MapFaction;
				}
				else if (this.Side == PartyScreenLogic.PartyRosterSide.Right && this._partyScreenLogic.RightOwnerParty != null)
				{
					faction = this._partyScreenLogic.RightOwnerParty.MapFaction;
				}*/
			}
			uint color = Color.White.ToUnsignedInteger();
			uint color2 = Color.White.ToUnsignedInteger();
			if (faction != null)
			{
				color = faction.Color;
				color2 = faction.Color2;
			}
			else if (character.Culture != null)
			{
				color = character.Culture.Color;
				color2 = character.Culture.Color2;
			}
			Equipment equipment = character.Equipment;
			string equipmentCode = (equipment != null) ? equipment.CalculateEquipmentCode() : null;
			BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
			return CharacterCode.CreateFrom(equipmentCode, bodyProperties, character.IsFemale, character.IsHero, color, color2, character.DefaultFormationClass);
		}


		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (this._index != value)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		[DataSourceProperty]
		public bool IsHero
		{
			get
			{
				return this._isHero;
			}
			set
			{
				if (value != this._isHero)
				{
					this._isHero = value;
					base.OnPropertyChangedWithValue(value, "IsHero");
				}
			}
		}

		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		[DataSourceProperty]
		public int HeroHealth
		{
			get
			{
				CharacterObject character = this.Character;
				if (character != null && character.IsHero)
				{
					return MathF.Ceiling((float)this.Character.HeroObject.HitPoints * 100f / (float)this.Character.MaxHitPoints());
				}
				return 0;
			}
		}

		private bool _isHero;

		private int _index = -2;

		private string _troopID;
		private string _name;

		private StringItemWithHintVM _typeIconData;
		private StringItemWithHintVM _tierIconData;

		private TroopRosterElement _troop;
		private CharacterObject _character;
		private ImageIdentifierVM _code = new ImageIdentifierVM(ImageIdentifierType.Null);
        private bool _isMainHero;
    }
}
