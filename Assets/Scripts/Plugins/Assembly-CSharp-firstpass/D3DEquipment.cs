using System.Collections.Generic;
using UnityEngine;

public class D3DEquipment
{
	public enum EquipmentGrade
	{
		INFERIOR = 0,
		NORMAL = 1,
		SUPERIOR = 2,
		MAGIC = 3,
		RARE = 4,
		EX_RARE = 5,
		IAP = 999
	}

	public struct EquipComparePlus
	{
		public int nWhitePlus;

		public int nGreenPlus;

		public int nBluePlus;

		public int nPurplePlus;

		public int nMaxLevel;
	}

	public enum EquipmentType
	{
		None = 0,
		ONE_HAND = 10,
		OFF_HAND = 11,
		TWO_HAND = 12,
		BOW_HAND = 13,
		HELM = 1,
		ARMOR = 2,
		BELT = 3,
		BOOTS = 4,
		WRIST = 5,
		ACCESSORY = 6
	}

	public enum EquipmentClass
	{
		AXE = 0,
		SWORD = 1,
		HAMMER = 2,
		DAGGER = 3,
		CLAYMORE = 4,
		STAFF = 5,
		BOW = 6,
		SHIELD = 7,
		PLATE = 20,
		LEATHER = 21,
		ROBE = 22,
		NECKLACE = 40,
		RING = 41
	}

	public enum BasicAttribute
	{
		PYH_DAMAGE_MIN = 0,
		PYH_DAMAGE_MAX = 1,
		MAG_DAMAGE_MIN = 2,
		MAG_DAMAGE_MAX = 3,
		INTERVAL = 4,
		ARMOR = 5,
		PYH_DAMAGE_ACCURACY = 6,
		MAG_DAMAGE_ACCURACY = 8
	}

	public const string ESK = "s1f96TCw5+QfDpWs";

	private static string[] EquipmentGradeStr = new string[7] { "Inferior", "normal", "superior", "magic", "rare", "ex_rar", "iap" };

	public static EquipComparePlus _EquipComparePlus;

	public string equipment_id;

	public string equipment_name;

	public EquipmentGrade equipment_grade;

	public EquipmentType equipment_type;

	public EquipmentClass equipment_class;

	public int require_level;

	public string set_item_id;

	public string use_icon;

	public string use_model;

	public List<string> use_textures;

	public List<string[]> equipment_effects;

	public List<string> explicit_popedom_classes;

	public Dictionary<BasicAttribute, float> basic_attributes;

	public Dictionary<D3DPassiveTrigger.PassiveType, D3DPassiveTriggerSimple> extra_attributes;

	public Dictionary<D3DStateTrigger.TriggerType, D3DStateTriggerSimple> equipment_triggers;

	public D3DMagicPowerSaveData magic_power_data;

	public int buy_price;

	public int buy_price_crystal;

	public List<string> description;

	public int SellPrice
	{
		get
		{
			int num = (int)((float)buy_price * 0.1f);
			return (num < 1) ? 1 : num;
		}
	}

	public D3DEquipment()
	{
		equipment_id = string.Empty;
		equipment_name = string.Empty;
		equipment_grade = EquipmentGrade.INFERIOR;
		equipment_class = EquipmentClass.AXE;
		equipment_type = EquipmentType.ONE_HAND;
		require_level = 99;
		set_item_id = string.Empty;
		use_icon = string.Empty;
		use_model = string.Empty;
		use_textures = new List<string>();
		equipment_effects = new List<string[]>();
		explicit_popedom_classes = new List<string>();
		basic_attributes = new Dictionary<BasicAttribute, float>();
		extra_attributes = new Dictionary<D3DPassiveTrigger.PassiveType, D3DPassiveTriggerSimple>();
		equipment_triggers = new Dictionary<D3DStateTrigger.TriggerType, D3DStateTriggerSimple>();
		magic_power_data = null;
		buy_price = 0;
		description = new List<string>();
	}

	public static string GetStringByGrade(EquipmentGrade eGrade)
	{
		if ((int)eGrade < EquipmentGradeStr.Length)
		{
			return EquipmentGradeStr[(int)eGrade];
		}
		return "not found";
	}

	public static string GetStringFromEquipmentType(EquipmentType eType)
	{
		string empty = string.Empty;
		switch (eType)
		{
		case EquipmentType.ONE_HAND:
			return "one_hand";
		case EquipmentType.OFF_HAND:
			return "off_hand";
		case EquipmentType.TWO_HAND:
			return "tow_hand";
		case EquipmentType.BOW_HAND:
			return "bow_hand";
		case EquipmentType.HELM:
			return "helm";
		case EquipmentType.ARMOR:
			return "armor";
		case EquipmentType.BELT:
			return "belt";
		case EquipmentType.BOOTS:
			return "boots";
		case EquipmentType.WRIST:
			return "wrist";
		case EquipmentType.ACCESSORY:
			return "accessory";
		default:
			return "notFound";
		}
	}

	~D3DEquipment()
	{
	}

	public D3DEquipment Clone()
	{
		D3DEquipment d3DEquipment = new D3DEquipment();
		d3DEquipment.equipment_id = equipment_id;
		d3DEquipment.equipment_name = equipment_name;
		d3DEquipment.equipment_grade = equipment_grade;
		d3DEquipment.equipment_class = equipment_class;
		d3DEquipment.equipment_type = equipment_type;
		d3DEquipment.require_level = require_level;
		d3DEquipment.set_item_id = set_item_id;
		d3DEquipment.use_icon = use_icon;
		d3DEquipment.use_model = use_model;
		d3DEquipment.use_textures = new List<string>(use_textures);
		d3DEquipment.equipment_effects = new List<string[]>(equipment_effects);
		d3DEquipment.explicit_popedom_classes = new List<string>(explicit_popedom_classes);
		d3DEquipment.basic_attributes = new Dictionary<BasicAttribute, float>(basic_attributes);
		d3DEquipment.extra_attributes.Clear();
		foreach (D3DPassiveTrigger.PassiveType key in extra_attributes.Keys)
		{
			d3DEquipment.extra_attributes.Add(key, extra_attributes[key].Clone());
		}
		d3DEquipment.equipment_triggers.Clear();
		foreach (D3DStateTrigger.TriggerType key2 in equipment_triggers.Keys)
		{
			d3DEquipment.equipment_triggers.Add(key2, equipment_triggers[key2].Clone());
		}
		d3DEquipment.buy_price = buy_price;
		d3DEquipment.buy_price_crystal = buy_price_crystal;
		d3DEquipment.description = description;
		return d3DEquipment;
	}

	public bool IsEquipmentUseableExplicit(D3DClass puppet_class)
	{
		if (explicit_popedom_classes.Contains(puppet_class.class_id))
		{
			return true;
		}
		return false;
	}

	public bool IsEquipmentUseableImplicit(D3DClass puppet_class)
	{
		if (!D3DMain.Instance.D3DImplicitEquipPopedom.ContainsKey(equipment_class))
		{
			return true;
		}
		List<string> list = D3DMain.Instance.D3DImplicitEquipPopedom[equipment_class];
		if (list.Count == 0)
		{
			return true;
		}
		return list.Contains(puppet_class.class_id);
	}

	private bool IsEquipmentUseable(int nPuppetLevel, string classId)
	{
		if (explicit_popedom_classes.Contains(classId))
		{
			return true;
		}
		if (!D3DMain.Instance.D3DImplicitEquipPopedom.ContainsKey(equipment_class))
		{
			return true;
		}
		List<string> list = D3DMain.Instance.D3DImplicitEquipPopedom[equipment_class];
		if (list.Count == 0)
		{
			return true;
		}
		return list.Contains(classId);
	}

	public bool IsEquipmentUseable(D3DGamer.D3DPuppetSaveData saveData)
	{
		D3DPuppetProfile profile = D3DMain.Instance.GetProfile(saveData.pupet_profile_id);
		D3DClass @class = D3DMain.Instance.GetClass(profile.profile_class);
		return IsEquipmentUseable(saveData.puppet_level, @class.class_id);
	}

	public bool IsEquipmentUseable(D3DProfileInstance profile_instance)
	{
		return IsEquipmentUseable(profile_instance.puppet_level, profile_instance.puppet_class.class_id);
	}

	public bool CheckEquipmentEquipLegal(D3DProfileInstance profile_instance, D3DPuppetProfile.PuppetArms arm_type)
	{
		if (profile_instance == null)
		{
			return false;
		}
		if (profile_instance.puppet_class == null)
		{
			return false;
		}
		if (!IsEquipmentUseable(profile_instance))
		{
			return false;
		}
		switch (arm_type)
		{
		case D3DPuppetProfile.PuppetArms.RIGHT_HAND:
			if (equipment_type == EquipmentType.ONE_HAND)
			{
				D3DEquipment d3DEquipment4 = profile_instance.puppet_arms[1];
				if (d3DEquipment4 == null)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, null);
					return true;
				}
				if (d3DEquipment4.equipment_type == EquipmentType.TWO_HAND)
				{
					if (profile_instance.GetTitanPower())
					{
						return true;
					}
					return false;
				}
				return true;
			}
			if (equipment_type == EquipmentType.TWO_HAND)
			{
				D3DEquipment d3DEquipment5 = profile_instance.puppet_arms[1];
				if (d3DEquipment5 == null)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, null);
					return true;
				}
				if (profile_instance.GetTitanPower())
				{
					return true;
				}
				return false;
			}
			if (equipment_type == EquipmentType.BOW_HAND)
			{
				D3DEquipment d3DEquipment6 = profile_instance.puppet_arms[1];
				if (d3DEquipment6 == null)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, null);
					return true;
				}
				return false;
			}
			return false;
		case D3DPuppetProfile.PuppetArms.LEFT_HAND:
			if (equipment_type == EquipmentType.OFF_HAND)
			{
				D3DEquipment d3DEquipment = profile_instance.puppet_arms[0];
				if (d3DEquipment == null)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, null);
					return true;
				}
				if (d3DEquipment.equipment_type == EquipmentType.TWO_HAND)
				{
					if (profile_instance.GetTitanPower())
					{
						return true;
					}
					return false;
				}
				if (d3DEquipment.equipment_type == EquipmentType.BOW_HAND)
				{
					return false;
				}
				return true;
			}
			if (equipment_type == EquipmentType.ONE_HAND)
			{
				if (!profile_instance.GetDualWield() && !profile_instance.GetTitanPower())
				{
					return false;
				}
				D3DEquipment d3DEquipment2 = profile_instance.puppet_arms[0];
				if (d3DEquipment2 == null)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, null);
					return true;
				}
				if (d3DEquipment2.equipment_type == EquipmentType.TWO_HAND)
				{
					if (profile_instance.GetTitanPower())
					{
						return true;
					}
					return false;
				}
				if (d3DEquipment2.equipment_type == EquipmentType.BOW_HAND)
				{
					return false;
				}
				return true;
			}
			if (equipment_type == EquipmentType.TWO_HAND)
			{
				if (!profile_instance.GetTitanPower())
				{
					return false;
				}
				D3DEquipment d3DEquipment3 = profile_instance.puppet_arms[0];
				if (d3DEquipment3 == null)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, null);
					return true;
				}
				if (d3DEquipment3.equipment_type == EquipmentType.BOW_HAND)
				{
					return false;
				}
				return true;
			}
			return false;
		case D3DPuppetProfile.PuppetArms.HELM:
			if (equipment_type == EquipmentType.HELM)
			{
				return true;
			}
			return false;
		case D3DPuppetProfile.PuppetArms.ARMOR:
			if (equipment_type == EquipmentType.ARMOR)
			{
				return true;
			}
			return false;
		case D3DPuppetProfile.PuppetArms.BOOTS:
			if (equipment_type == EquipmentType.BOOTS)
			{
				return true;
			}
			return false;
		case D3DPuppetProfile.PuppetArms.NECKLANCE:
			if (equipment_type != EquipmentType.ACCESSORY)
			{
				return false;
			}
			if (equipment_class == EquipmentClass.NECKLACE)
			{
				return true;
			}
			return false;
		case D3DPuppetProfile.PuppetArms.RING1:
		case D3DPuppetProfile.PuppetArms.RING2:
			if (equipment_type != EquipmentType.ACCESSORY)
			{
				return false;
			}
			if (equipment_class == EquipmentClass.RING)
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	private D3DFloat CalcAccrAttr(BasicAttribute attrType)
	{
		int num = 5;
		D3DFloat basicAttributes = GetBasicAttributes(attrType - num);
		if (basicAttributes != null)
		{
			basicAttributes.value *= 0.8f;
			return basicAttributes;
		}
		return null;
	}

	public D3DFloat GetBasicAttributes(BasicAttribute attribute)
	{
		switch (attribute)
		{
		case BasicAttribute.PYH_DAMAGE_ACCURACY:
		case BasicAttribute.MAG_DAMAGE_ACCURACY:
			return CalcAccrAttr(attribute);
		case BasicAttribute.PYH_DAMAGE_MIN:
			if (basic_attributes.ContainsKey(attribute))
			{
				return new D3DFloat(basic_attributes[attribute]);
			}
			if (basic_attributes.ContainsKey(BasicAttribute.PYH_DAMAGE_MAX))
			{
				return new D3DFloat(basic_attributes[BasicAttribute.PYH_DAMAGE_MAX]);
			}
			return null;
		case BasicAttribute.PYH_DAMAGE_MAX:
			if (basic_attributes.ContainsKey(attribute))
			{
				return new D3DFloat(basic_attributes[attribute]);
			}
			if (basic_attributes.ContainsKey(BasicAttribute.PYH_DAMAGE_MIN))
			{
				return new D3DFloat(basic_attributes[BasicAttribute.PYH_DAMAGE_MIN]);
			}
			return null;
		case BasicAttribute.MAG_DAMAGE_MIN:
			if (basic_attributes.ContainsKey(attribute))
			{
				return new D3DFloat(basic_attributes[attribute]);
			}
			if (basic_attributes.ContainsKey(BasicAttribute.MAG_DAMAGE_MAX))
			{
				return new D3DFloat(basic_attributes[BasicAttribute.MAG_DAMAGE_MAX]);
			}
			return null;
		case BasicAttribute.MAG_DAMAGE_MAX:
			if (basic_attributes.ContainsKey(attribute))
			{
				return new D3DFloat(basic_attributes[attribute]);
			}
			if (basic_attributes.ContainsKey(BasicAttribute.MAG_DAMAGE_MIN))
			{
				return new D3DFloat(basic_attributes[BasicAttribute.MAG_DAMAGE_MIN]);
			}
			return null;
		case BasicAttribute.INTERVAL:
			if (basic_attributes.ContainsKey(attribute))
			{
				return new D3DFloat(basic_attributes[attribute]);
			}
			return null;
		case BasicAttribute.ARMOR:
			if (basic_attributes.ContainsKey(attribute))
			{
				return new D3DFloat(basic_attributes[attribute]);
			}
			return null;
		default:
			return null;
		}
	}

	public D3DFloat GetExtraAttributes(D3DPassiveTrigger.PassiveType type, bool fixed_value)
	{
		if (!extra_attributes.ContainsKey(type))
		{
			return null;
		}
		if (fixed_value)
		{
			return extra_attributes[type].fixed_value;
		}
		return extra_attributes[type].percent_value;
	}

	public void EndowRandomMagicPower()
	{
	}

	public void EnableMagicPower()
	{
		if (magic_power_data == null || equipment_grade != EquipmentGrade.SUPERIOR)
		{
			return;
		}
		float num = 0f;
		switch (equipment_type)
		{
		case EquipmentType.ARMOR:
			num = D3DMagicPower.Instance.PowerRatioCoe[0];
			break;
		case EquipmentType.HELM:
			num = D3DMagicPower.Instance.PowerRatioCoe[1];
			break;
		case EquipmentType.BOOTS:
			num = D3DMagicPower.Instance.PowerRatioCoe[2];
			break;
		case EquipmentType.OFF_HAND:
			if (equipment_class == EquipmentClass.SHIELD)
			{
				num = D3DMagicPower.Instance.PowerRatioCoe[5];
			}
			break;
		case EquipmentType.TWO_HAND:
		case EquipmentType.BOW_HAND:
			num = D3DMagicPower.Instance.PowerRatioCoe[4];
			break;
		case EquipmentType.ONE_HAND:
			num = D3DMagicPower.Instance.PowerRatioCoe[3];
			break;
		case EquipmentType.ACCESSORY:
			if (equipment_class == EquipmentClass.NECKLACE)
			{
				num = D3DMagicPower.Instance.PowerRatioCoe[6];
			}
			else if (equipment_class == EquipmentClass.RING)
			{
				num = D3DMagicPower.Instance.PowerRatioCoe[7];
			}
			break;
		}
		float[] array = new float[2];
		float num2 = 1f;
		switch (equipment_class)
		{
		case EquipmentClass.NECKLACE:
			array = D3DMagicPower.Instance.NecklaceCoe;
			num2 = D3DMagicPower.Instance.StaminaAdjustCoe[3];
			break;
		case EquipmentClass.RING:
			array = D3DMagicPower.Instance.RingCoe;
			num2 = D3DMagicPower.Instance.StaminaAdjustCoe[3];
			break;
		default:
			array = D3DMagicPower.Instance.CommonCoe;
			num2 = ((equipment_class != EquipmentClass.PLATE) ? ((equipment_class != EquipmentClass.LEATHER) ? ((equipment_class != EquipmentClass.ROBE) ? D3DMagicPower.Instance.StaminaAdjustCoe[3] : D3DMagicPower.Instance.StaminaAdjustCoe[2]) : D3DMagicPower.Instance.StaminaAdjustCoe[1]) : D3DMagicPower.Instance.StaminaAdjustCoe[0]);
			break;
		}
		float num3 = 0f;
		if (extra_attributes.ContainsKey(D3DPassiveTrigger.PassiveType.STR) && extra_attributes[D3DPassiveTrigger.PassiveType.STR].fixed_value != null)
		{
			num3 = extra_attributes[D3DPassiveTrigger.PassiveType.STR].fixed_value.value;
		}
		float num4 = 0f;
		if (extra_attributes.ContainsKey(D3DPassiveTrigger.PassiveType.STA) && extra_attributes[D3DPassiveTrigger.PassiveType.STA].fixed_value != null)
		{
			num4 = extra_attributes[D3DPassiveTrigger.PassiveType.STA].fixed_value.value;
		}
		float num5 = 0f;
		if (extra_attributes.ContainsKey(D3DPassiveTrigger.PassiveType.MAGIC_POWER) && extra_attributes[D3DPassiveTrigger.PassiveType.MAGIC_POWER].fixed_value != null)
		{
			num5 = extra_attributes[D3DPassiveTrigger.PassiveType.MAGIC_POWER].fixed_value.value;
		}
		extra_attributes.Clear();
		if (equipment_class == EquipmentClass.NECKLACE || equipment_class == EquipmentClass.RING)
		{
			num3 += array[0] * (float)(require_level - 1);
			num4 += array[1] * (float)(require_level - 1);
		}
		else
		{
			num3 = (num3 + array[0] * (float)(require_level - 1)) * num;
			num4 = (num4 + array[1] * (float)(require_level - 1)) * num * num2;
		}
		num5 = (num5 + D3DMagicPower.Instance.MPCoe * (float)(require_level - 1)) * num;
		float num6 = 0f;
		PowerRule powerRule = D3DMagicPower.Instance.RuleManager[magic_power_data.rule_id];
		equipment_name = equipment_name + " " + powerRule.affix;
		if (powerRule.ContainsPowerType(3))
		{
			if (powerRule.power_value.Count == 1)
			{
				num6 = num3 * D3DMagicPower.Instance.RedressCoe[4];
				AddMagicPowerValue(D3DPassiveTrigger.PassiveType.STA, num4 + num6);
				return;
			}
			AddMagicPowerValue(D3DPassiveTrigger.PassiveType.STA, num4);
			num4 = 0f;
		}
		else if (extra_attributes.ContainsKey(D3DPassiveTrigger.PassiveType.STA) && extra_attributes[D3DPassiveTrigger.PassiveType.STA].fixed_value != null)
		{
			extra_attributes[D3DPassiveTrigger.PassiveType.STA].fixed_value = null;
		}
		if (powerRule.ContainsPowerType(0))
		{
			num6 = num4 * D3DMagicPower.Instance.RedressCoe[0];
			AddMagicPowerValue(D3DPassiveTrigger.PassiveType.STR, num3 + num6);
		}
		else if (extra_attributes.ContainsKey(D3DPassiveTrigger.PassiveType.STR) && extra_attributes[D3DPassiveTrigger.PassiveType.STR].fixed_value != null)
		{
			extra_attributes[D3DPassiveTrigger.PassiveType.STR].fixed_value = null;
		}
		if (powerRule.ContainsPowerType(1))
		{
			num6 = num4 * D3DMagicPower.Instance.RedressCoe[1];
			AddMagicPowerValue(D3DPassiveTrigger.PassiveType.AGI, num3 + num6);
		}
		bool flag = false;
		if (powerRule.ContainsPowerType(4))
		{
			flag = true;
			num6 = num4 * D3DMagicPower.Instance.RedressCoe[2];
			AddMagicPowerValue(D3DPassiveTrigger.PassiveType.INT, num3 + num6);
		}
		if (powerRule.ContainsPowerType(2))
		{
			flag = true;
			num6 = num4 * D3DMagicPower.Instance.RedressCoe[3];
			AddMagicPowerValue(D3DPassiveTrigger.PassiveType.SPI, num3 + num6);
		}
		if (flag)
		{
			AddMagicPowerValue(D3DPassiveTrigger.PassiveType.MAGIC_POWER, num5);
		}
	}

	private void AddMagicPowerValue(D3DPassiveTrigger.PassiveType value_type, float value)
	{
		if (!extra_attributes.ContainsKey(value_type))
		{
			extra_attributes.Add(value_type, new D3DPassiveTriggerSimple());
		}
		if (extra_attributes[value_type].fixed_value == null)
		{
			extra_attributes[value_type].fixed_value = new D3DFloat(0f);
		}
		float num = 1f;
		if (value_type != D3DPassiveTrigger.PassiveType.MAGIC_POWER)
		{
			num = magic_power_data.power_value[(int)value_type];
		}
		extra_attributes[value_type].fixed_value.value = Mathf.Round(value * num);
		if (extra_attributes[value_type].fixed_value.value < 1f)
		{
			extra_attributes[value_type].fixed_value.value = 1f;
		}
	}

	public D3DPuppetProfile.PuppetArms GetDefaultArm()
	{
		switch (equipment_type)
		{
		case EquipmentType.ONE_HAND:
		case EquipmentType.TWO_HAND:
		case EquipmentType.BOW_HAND:
			return D3DPuppetProfile.PuppetArms.RIGHT_HAND;
		case EquipmentType.OFF_HAND:
			return D3DPuppetProfile.PuppetArms.LEFT_HAND;
		case EquipmentType.HELM:
			return D3DPuppetProfile.PuppetArms.HELM;
		case EquipmentType.ARMOR:
			return D3DPuppetProfile.PuppetArms.ARMOR;
		case EquipmentType.BOOTS:
			return D3DPuppetProfile.PuppetArms.BOOTS;
		case EquipmentType.ACCESSORY:
			if (equipment_class == EquipmentClass.NECKLACE)
			{
				return D3DPuppetProfile.PuppetArms.NECKLANCE;
			}
			return D3DPuppetProfile.PuppetArms.RING1;
		default:
			return D3DPuppetProfile.PuppetArms.BELT;
		}
	}

	public bool ConvertPassiveExtraToUnique(D3DPassiveTrigger.PassiveType type, out List<string> strDescript, out List<float> fValueList)
	{
		fValueList = new List<float>();
		strDescript = new List<string>();
		float num = 0.5f;
		D3DFloat extraAttributes = GetExtraAttributes(type, true);
		switch (type)
		{
		case D3DPassiveTrigger.PassiveType.STR:
		case D3DPassiveTrigger.PassiveType.AGI:
			fValueList.Add(extraAttributes.value * num);
			strDescript.Add("Damage");
			break;
		case D3DPassiveTrigger.PassiveType.STA:
			fValueList.Add(extraAttributes.value * 10f);
			strDescript.Add("HP");
			break;
		case D3DPassiveTrigger.PassiveType.INT:
			fValueList.Add(extraAttributes.value * 10f);
			strDescript.Add("MP");
			fValueList.Add(extraAttributes.value * num);
			strDescript.Add("Magic Damage");
			break;
		case D3DPassiveTrigger.PassiveType.SPI:
			fValueList.Add(extraAttributes.value * 0.05f * 10f);
			strDescript.Add("MP Every 10 Sec");
			break;
		case D3DPassiveTrigger.PassiveType.ATTACK_POWER:
			fValueList.Add(extraAttributes.value * 0.25f);
			strDescript.Add("Damage");
			break;
		case D3DPassiveTrigger.PassiveType.MAGIC_POWER:
			fValueList.Add(extraAttributes.value * 0.25f);
			strDescript.Add("Magic Damage");
			break;
		default:
			DebugX.LogError("Not support this attribute in equipment!");
			break;
		}
		return fValueList.Count != 0;
	}

	private bool IsWeaponEquipment()
	{
		return equipment_class < EquipmentClass.SHIELD;
	}

	private bool IsAmorEquipment()
	{
		return equipment_class == EquipmentClass.SHIELD || equipment_class == EquipmentClass.PLATE || equipment_class == EquipmentClass.LEATHER || equipment_class == EquipmentClass.ROBE;
	}

	private bool IsAccessory()
	{
		return equipment_class == EquipmentClass.NECKLACE || equipment_class == EquipmentClass.RING;
	}

	public int GetCompareLevel()
	{
		int num = require_level;
		switch (equipment_grade)
		{
		case EquipmentGrade.NORMAL:
			num += _EquipComparePlus.nWhitePlus;
			break;
		case EquipmentGrade.SUPERIOR:
			num += _EquipComparePlus.nGreenPlus;
			break;
		case EquipmentGrade.MAGIC:
			num += _EquipComparePlus.nBluePlus;
			break;
		case EquipmentGrade.RARE:
		case EquipmentGrade.EX_RARE:
		case EquipmentGrade.IAP:
			num += _EquipComparePlus.nPurplePlus;
			break;
		default:
			DebugX.LogError("Equipment is supported right now!" + equipment_grade);
			break;
		}
		if (num > _EquipComparePlus.nMaxLevel)
		{
			num -= _EquipComparePlus.nMaxLevel;
		}
		return num;
	}

	public int CompareEquip(D3DEquipment quipToCompare)
	{
		int compareLevel = GetCompareLevel();
		int compareLevel2 = quipToCompare.GetCompareLevel();
		return GetReturnValueForCompare(compareLevel, compareLevel2);
	}

	private int GetReturnValueForCompare(float fCompared, float fToComapre)
	{
		if (fCompared > fToComapre)
		{
			return 1;
		}
		if (fCompared == fToComapre)
		{
			return 0;
		}
		return -1;
	}
}
