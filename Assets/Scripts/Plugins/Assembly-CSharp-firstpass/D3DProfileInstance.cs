using System.Collections.Generic;
using UnityEngine;

public class D3DProfileInstance
{
	private D3DPuppetProfile puppet_profile;

	public D3DClass puppet_class;

	public D3DEquipment[] puppet_arms;

	public int[] property_color_state;

	public D3DEquipment puppet_default_body;

	public D3DEquipment puppet_default_weapon;

	public int puppet_level;

	public int current_exp;

	public D3DPuppetProperty puppet_property;

	private D3DFormulas puppet_formula;

	public string[] battle_active_slots;

	public string[] battle_passive_slots;

	public D3DClassBattleSkillStatus basic_skill;

	public Dictionary<string, D3DClassBattleSkillStatus> battle_active_skills;

	public bool battle_puppet = true;

	public string ProfileID
	{
		get
		{
			return puppet_profile.profile_id;
		}
	}

	public string ProfileName
	{
		get
		{
			return puppet_profile.profile_name;
		}
	}

	public D3DPuppetProfile.ProfileType PuppetType
	{
		get
		{
			return puppet_profile.profile_type;
		}
	}

	public D3DPuppetProfile Profile
	{
		get
		{
			return puppet_profile;
		}
	}

	public string PuppetFeatureModel
	{
		get
		{
			return puppet_profile.feature_model;
		}
	}

	public List<string[]> PuppetEffects
	{
		get
		{
			return puppet_profile.puppet_effects;
		}
	}

	public List<string> PuppetFeatureTextures
	{
		get
		{
			return puppet_profile.feature_textures;
		}
	}

	public List<string> PuppetOtherTextures
	{
		get
		{
			return puppet_profile.other_textures;
		}
	}

	public string PuppetFeatureSkin
	{
		get
		{
			return puppet_profile.feature_skin;
		}
	}

	public float PuppetScale
	{
		get
		{
			return puppet_profile.custom_scale;
		}
	}

	public float HPScale
	{
		get
		{
			return puppet_property.hp / puppet_property.hp_max;
		}
	}

	public float MPScale
	{
		get
		{
			return puppet_property.mp / puppet_property.mp_max;
		}
	}

	public float ExpScale
	{
		get
		{
			return (float)current_exp / (float)D3DFormulas.ConvertLevelUpExp(puppet_level);
		}
	}

	public D3DProfileInstance()
	{
		puppet_profile = null;
		puppet_class = null;
		puppet_arms = new D3DEquipment[10];
		puppet_default_body = null;
		puppet_default_weapon = null;
		property_color_state = new int[7];
		puppet_level = 1;
		current_exp = 0;
		puppet_property = new D3DPuppetProperty();
		puppet_formula = new D3DFormulas();
		battle_active_slots = new string[0];
		battle_passive_slots = new string[0];
		basic_skill = null;
		battle_active_skills = null;
	}

	public bool InitInstance(D3DPuppetProfile profile, D3DGamer.D3DPuppetSaveData save_data)
	{
		if (profile == null)
		{
			return false;
		}
		puppet_profile = profile;
		if (D3DMain.Instance.CheckClassID(puppet_profile.profile_class))
		{
			puppet_class = D3DMain.Instance.GetClassClone(profile.profile_class);
			InitFormula();
			puppet_class.UniteClassFromProfile(puppet_profile);
			puppet_class.CheckSkillInerrability();
			if (save_data != null && puppet_profile.profile_id == save_data.pupet_profile_id)
			{
				puppet_level = ((save_data.puppet_level <= 40) ? save_data.puppet_level : 40);
				current_exp = save_data.puppet_current_exp;
				battle_puppet = save_data.battle_puppet;
				if (puppet_class.editable)
				{
					puppet_profile.profile_arms[puppet_profile.current_power] = new D3DGamer.D3DEquipmentSaveData[save_data.puppet_equipments.Length];
					for (int i = 0; i < save_data.puppet_equipments.Length; i++)
					{
						if (save_data.puppet_equipments[i] == null)
						{
							puppet_profile.profile_arms[puppet_profile.current_power][i] = null;
						}
						else
						{
							puppet_profile.profile_arms[puppet_profile.current_power][i] = save_data.puppet_equipments[i].Clone();
						}
					}
				}
			}
			if (D3DMain.Instance.CheckEquipmentID(puppet_class.default_armor))
			{
				puppet_default_body = D3DMain.Instance.GetEquipmentClone(puppet_class.default_armor);
				if (puppet_default_body == null)
				{
					puppet_class.default_armor = string.Empty;
				}
			}
			else
			{
				puppet_class.default_armor = string.Empty;
				puppet_default_body = null;
			}
			if (D3DMain.Instance.CheckEquipmentID(puppet_class.default_weapon))
			{
				puppet_default_weapon = D3DMain.Instance.GetEquipmentClone(puppet_class.default_weapon);
				if (puppet_default_weapon == null)
				{
					puppet_class.default_weapon = string.Empty;
				}
			}
			else
			{
				puppet_class.default_weapon = string.Empty;
				puppet_default_weapon = null;
			}
			SetPuppetArms();
			return true;
		}
		return false;
	}

	public bool InitInstance(D3DPuppetProfile profile, int level)
	{
		if (profile == null)
		{
			return false;
		}
		puppet_profile = profile;
		if (D3DMain.Instance.CheckClassID(puppet_profile.profile_class))
		{
			puppet_class = D3DMain.Instance.GetClassClone(profile.profile_class);
			InitFormula();
			puppet_class.UniteClassFromProfile(puppet_profile);
			puppet_class.CheckSkillInerrability();
			puppet_level = ((level <= 40) ? level : 40);
			if (D3DMain.Instance.CheckEquipmentID(puppet_class.default_armor))
			{
				puppet_default_body = D3DMain.Instance.GetEquipmentClone(puppet_class.default_armor);
				if (puppet_default_body == null)
				{
					puppet_class.default_armor = string.Empty;
				}
			}
			else
			{
				puppet_class.default_armor = string.Empty;
				puppet_default_body = null;
			}
			if (D3DMain.Instance.CheckEquipmentID(puppet_class.default_weapon))
			{
				puppet_default_weapon = D3DMain.Instance.GetEquipmentClone(puppet_class.default_weapon);
				if (puppet_default_weapon == null)
				{
					puppet_class.default_weapon = string.Empty;
				}
			}
			else
			{
				puppet_class.default_weapon = string.Empty;
				puppet_default_weapon = null;
			}
			SetPuppetArms();
			return true;
		}
		return false;
	}

	public bool IsLevelMax()
	{
		return puppet_level >= 40;
	}

	public void SetPuppetArms()
	{
		puppet_profile.CheckArmsID();
		for (int i = 0; i <= 9; i++)
		{
			if (i != 4 && i != 6 && puppet_profile.profile_arms[puppet_profile.current_power][i] != null)
			{
				puppet_arms[i] = D3DMain.Instance.GetEquipmentClone(puppet_profile.profile_arms[puppet_profile.current_power][i].equipment_id);
				if (puppet_arms[i] == null)
				{
					puppet_profile.profile_arms[puppet_profile.current_power][i] = null;
					continue;
				}
				puppet_arms[i].magic_power_data = puppet_profile.profile_arms[puppet_profile.current_power][i].magic_power_data;
				puppet_arms[i].EnableMagicPower();
			}
		}
	}

	public void ChangeArms(D3DPuppetProfile.PuppetArms arm_part, D3DEquipment equipment)
	{
		if (equipment == null)
		{
			puppet_profile.profile_arms[puppet_profile.current_power][(int)arm_part] = null;
			puppet_arms[(int)arm_part] = null;
			return;
		}
		if (puppet_profile.profile_arms[puppet_profile.current_power][(int)arm_part] == null)
		{
			puppet_profile.profile_arms[puppet_profile.current_power][(int)arm_part] = new D3DGamer.D3DEquipmentSaveData();
		}
		puppet_profile.profile_arms[puppet_profile.current_power][(int)arm_part].equipment_id = equipment.equipment_id;
		puppet_profile.profile_arms[puppet_profile.current_power][(int)arm_part].magic_power_data = equipment.magic_power_data;
		puppet_arms[(int)arm_part] = equipment;
	}

	public D3DEquipment GetCompareGear(D3DEquipment selected_gear)
	{
		int num = -1;
		switch (selected_gear.equipment_type)
		{
		case D3DEquipment.EquipmentType.ONE_HAND:
		case D3DEquipment.EquipmentType.TWO_HAND:
		case D3DEquipment.EquipmentType.BOW_HAND:
			num = 0;
			break;
		case D3DEquipment.EquipmentType.OFF_HAND:
			num = 1;
			if (puppet_arms[num] == null || puppet_arms[num].equipment_class != D3DEquipment.EquipmentClass.SHIELD)
			{
				num = -1;
			}
			break;
		case D3DEquipment.EquipmentType.ARMOR:
			num = 2;
			break;
		case D3DEquipment.EquipmentType.HELM:
			num = 3;
			break;
		case D3DEquipment.EquipmentType.BOOTS:
			num = 5;
			break;
		case D3DEquipment.EquipmentType.ACCESSORY:
			if (selected_gear.equipment_class == D3DEquipment.EquipmentClass.NECKLACE)
			{
				num = 7;
			}
			else if (selected_gear.equipment_class == D3DEquipment.EquipmentClass.RING)
			{
				num = 8;
			}
			break;
		}
		if (num != -1)
		{
			return puppet_arms[num];
		}
		return null;
	}

	public void InitFormula()
	{
		puppet_formula.InitFormulaCoe(puppet_class.class_id);
	}

	public void InitProperties()
	{
		puppet_level = ((puppet_level <= 40) ? puppet_level : 40);
		float[] array = new float[22];
		float[] array2 = new float[22];
		for (int i = 0; i <= 9; i++)
		{
			if (i == 4 || i == 6 || puppet_arms[i] == null || !puppet_arms[i].CheckEquipmentEquipLegal(this, (D3DPuppetProfile.PuppetArms)i))
			{
				continue;
			}
			for (D3DPassiveTrigger.PassiveType passiveType = D3DPassiveTrigger.PassiveType.STR; passiveType <= D3DPassiveTrigger.PassiveType.EXP_UP; passiveType++)
			{
				if (passiveType != D3DPassiveTrigger.PassiveType.MAG_DEF)
				{
					D3DFloat extraAttributes = puppet_arms[i].GetExtraAttributes(passiveType, true);
					if (extraAttributes != null)
					{
						array[(int)passiveType] += extraAttributes.value;
					}
					extraAttributes = puppet_arms[i].GetExtraAttributes(passiveType, false);
					if (extraAttributes != null)
					{
						array2[(int)passiveType] += extraAttributes.value;
					}
				}
			}
		}
		string[] array3 = battle_passive_slots;
		foreach (string key in array3)
		{
			if (!puppet_class.passive_skill_id_list.ContainsKey(key))
			{
				continue;
			}
			D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = puppet_class.passive_skill_id_list[key];
			if (d3DClassPassiveSkillStatus.passive_skill.GetDualWield() || d3DClassPassiveSkillStatus.passive_skill.GetTitanPower())
			{
				continue;
			}
			foreach (D3DPassiveTriggerComplex passive_trigger in d3DClassPassiveSkillStatus.passive_skill.passive_triggers)
			{
				if (passive_trigger.passive_type >= D3DPassiveTrigger.PassiveType.STR && passive_trigger.passive_type < D3DPassiveTrigger.PassiveType.EXP_UP && passive_trigger.passive_type != D3DPassiveTrigger.PassiveType.MAG_DEF)
				{
					if (passive_trigger.fixed_value != null)
					{
						array[(int)passive_trigger.passive_type] += passive_trigger.fixed_value.values[d3DClassPassiveSkillStatus.skill_level];
					}
					if (passive_trigger.percent_value != null)
					{
						array2[(int)passive_trigger.passive_type] += passive_trigger.percent_value.values[d3DClassPassiveSkillStatus.skill_level];
					}
				}
			}
		}
		D3DPuppetProperty d3DPuppetProperty = new D3DPuppetProperty();
		d3DPuppetProperty.strength = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STR) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STR);
		d3DPuppetProperty.strength = Mathf.Round(d3DPuppetProperty.strength);
		if (d3DPuppetProperty.strength < 0f)
		{
			d3DPuppetProperty.strength = 0f;
		}
		d3DPuppetProperty.agility = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.AGI) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.AGI);
		d3DPuppetProperty.agility = Mathf.Round(d3DPuppetProperty.agility);
		if (d3DPuppetProperty.agility < 0f)
		{
			d3DPuppetProperty.agility = 0f;
		}
		d3DPuppetProperty.spirit = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.SPI) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.SPI);
		d3DPuppetProperty.spirit = Mathf.Round(d3DPuppetProperty.spirit);
		if (d3DPuppetProperty.spirit < 0f)
		{
			d3DPuppetProperty.spirit = 0f;
		}
		d3DPuppetProperty.stamina = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STA) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STA);
		d3DPuppetProperty.stamina = Mathf.Round(d3DPuppetProperty.stamina);
		if (d3DPuppetProperty.stamina < 0f)
		{
			d3DPuppetProperty.stamina = 0f;
		}
		d3DPuppetProperty.intelligence = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.INT) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.INT);
		d3DPuppetProperty.intelligence = Mathf.Round(d3DPuppetProperty.intelligence);
		if (d3DPuppetProperty.intelligence < 0f)
		{
			d3DPuppetProperty.intelligence = 0f;
		}
		if (puppet_class.class_main_type == D3DClass.ClassType.INT_MAIN)
		{
			d3DPuppetProperty.magic_power = puppet_formula.ConvertMainPower(puppet_class.class_main_type, d3DPuppetProperty, puppet_level);
			d3DPuppetProperty.magic_power = Mathf.Round(d3DPuppetProperty.magic_power);
			d3DPuppetProperty.attack_power = puppet_formula.ConvertSubPower(puppet_class.class_sub_type, d3DPuppetProperty, puppet_level);
			d3DPuppetProperty.attack_power = Mathf.Round(d3DPuppetProperty.attack_power);
		}
		else
		{
			d3DPuppetProperty.attack_power = puppet_formula.ConvertMainPower(puppet_class.class_main_type, d3DPuppetProperty, puppet_level);
			d3DPuppetProperty.attack_power = Mathf.Round(d3DPuppetProperty.attack_power);
			d3DPuppetProperty.magic_power = puppet_formula.ConvertSubPower(puppet_class.class_sub_type, d3DPuppetProperty, puppet_level);
			d3DPuppetProperty.magic_power = Mathf.Round(d3DPuppetProperty.magic_power);
		}
		if (d3DPuppetProperty.attack_power < 0f)
		{
			d3DPuppetProperty.attack_power = 0f;
		}
		if (d3DPuppetProperty.magic_power < 0f)
		{
			d3DPuppetProperty.magic_power = 0f;
		}
		puppet_property.dual_adjust = 1f;
		puppet_property.strength = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STR) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STR) + array[0];
		puppet_property.strength *= 1f + array2[0];
		puppet_property.strength = Mathf.Round(puppet_property.strength);
		if (puppet_property.strength < 0f)
		{
			puppet_property.strength = 0f;
		}
		puppet_property.agility = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.AGI) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.AGI) + array[1];
		puppet_property.agility *= 1f + array2[1];
		puppet_property.agility = Mathf.Round(puppet_property.agility);
		if (puppet_property.agility < 0f)
		{
			puppet_property.agility = 0f;
		}
		puppet_property.spirit = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.SPI) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.SPI) + array[2];
		puppet_property.spirit *= 1f + array2[2];
		puppet_property.spirit = Mathf.Round(puppet_property.spirit);
		if (puppet_property.spirit < 0f)
		{
			puppet_property.spirit = 0f;
		}
		puppet_property.stamina = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STA) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.STA) + array[3];
		puppet_property.stamina *= 1f + array2[3];
		puppet_property.stamina = Mathf.Round(puppet_property.stamina);
		if (puppet_property.stamina < 0f)
		{
			puppet_property.stamina = 0f;
		}
		puppet_property.intelligence = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.INT) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.INT) + array[4];
		puppet_property.intelligence *= 1f + array2[4];
		puppet_property.intelligence = Mathf.Round(puppet_property.intelligence);
		if (puppet_property.intelligence < 0f)
		{
			puppet_property.intelligence = 0f;
		}
		puppet_property.hp = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.HP) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.HP) + puppet_formula.ConvertHP(puppet_property) + array[5];
		puppet_property.hp *= 1f + array2[5];
		puppet_property.hp = Mathf.Round(puppet_property.hp);
		if (puppet_class.sp_class)
		{
			puppet_property.mp = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.MP) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.MP) + array[6];
		}
		else
		{
			puppet_property.mp = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.MP) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.MP) + puppet_formula.ConvertMP(puppet_property) + array[6];
		}
		puppet_property.mp *= 1f + array2[6];
		puppet_property.mp = Mathf.Round(puppet_property.mp);
		float num = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.PHY_DMG) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.PHY_DMG);
		float num2 = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.MAG_DMG) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.MAG_DMG);
		puppet_property.puppet_physical_dmg = new float[2]
		{
			num * puppet_formula.damage_range[0],
			num * puppet_formula.damage_range[1]
		};
		puppet_property.puppet_magical_dmg = new float[2]
		{
			num2 * puppet_formula.damage_range[0],
			num2 * puppet_formula.damage_range[1]
		};
		if (puppet_class.class_main_type == D3DClass.ClassType.INT_MAIN)
		{
			puppet_property.magic_power = puppet_formula.ConvertMainPower(puppet_class.class_main_type, puppet_property, puppet_level) + array[8];
			puppet_property.magic_power *= 1f + array2[8];
			puppet_property.magic_power = Mathf.Round(puppet_property.magic_power);
			puppet_property.attack_power = puppet_formula.ConvertSubPower(puppet_class.class_sub_type, puppet_property, puppet_level) + array[7];
			puppet_property.attack_power *= 1f + array2[7];
			puppet_property.attack_power = Mathf.Round(puppet_property.attack_power);
		}
		else
		{
			puppet_property.attack_power = puppet_formula.ConvertMainPower(puppet_class.class_main_type, puppet_property, puppet_level) + array[7];
			puppet_property.attack_power *= 1f + array2[7];
			puppet_property.attack_power = Mathf.Round(puppet_property.attack_power);
			puppet_property.magic_power = puppet_formula.ConvertSubPower(puppet_class.class_sub_type, puppet_property, puppet_level) + array[8];
			puppet_property.magic_power *= 1f + array2[8];
			puppet_property.magic_power = Mathf.Round(puppet_property.magic_power);
		}
		if (puppet_property.attack_power < 0f)
		{
			puppet_property.attack_power = 0f;
		}
		if (puppet_property.magic_power < 0f)
		{
			puppet_property.magic_power = 0f;
		}
		puppet_property.hp_recover = puppet_formula.ConvertHpRecover(puppet_property) + array[11];
		puppet_property.hp_recover *= 1f + array2[11];
		puppet_property.hp_recover = Mathf.Round(puppet_property.hp_recover);
		if (puppet_property.hp_recover < 0f)
		{
			puppet_property.hp_recover = 0f;
		}
		puppet_property.mp_recover = puppet_formula.ConvertMpRecover(puppet_property) + array[12];
		puppet_property.mp_recover *= 1f + array2[12];
		puppet_property.mp_recover = Mathf.Round(puppet_property.mp_recover);
		if (puppet_property.mp_recover < 0f)
		{
			puppet_property.mp_recover = 0f;
		}
		puppet_property.attack_interval = puppet_profile.profile_talent[puppet_profile.current_power].talent_ability[10] + puppet_class.class_talent.talent_ability[10];
		puppet_property.move_speed = puppet_profile.profile_talent[puppet_profile.current_power].talent_ability[11] + puppet_class.class_talent.talent_ability[11] + array[13];
		puppet_property.move_speed *= 1f + array2[13];
		puppet_property.move_speed_coe = puppet_property.move_speed;
		puppet_property.dodge_chance = array[14];
		puppet_property.dodge_chance *= 1f + array2[14];
		puppet_property.critical_chance = array[15];
		puppet_property.critical_chance *= 1f + array2[15];
		puppet_property.dmg_reduce = array[16];
		puppet_property.dmg_reduce *= 1f + array2[16];
		puppet_property.exp_up = array[21];
		puppet_property.exp_percent = 1f + array2[21];
		puppet_property.stun_resist = array[17];
		puppet_property.stun_resist *= 1f + array2[17];
		puppet_property.fear_resist = array[18];
		puppet_property.fear_resist *= 1f + array2[18];
		puppet_property.trady_resist = array[19];
		puppet_property.trady_resist *= 1f + array2[19];
		puppet_property.stakme_resist = array[20];
		puppet_property.stakme_resist *= 1f + array2[20];
		puppet_property.main_weapon_physical_dmg = new float[2];
		puppet_property.main_weapon_magical_dmg = new float[2];
		puppet_property.sub_weapon_physical_dmg = null;
		puppet_property.sub_weapon_magical_dmg = null;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = puppet_property.attack_interval;
		float num6 = 0f;
		D3DPuppetProfile.PuppetArms puppetArms = D3DPuppetProfile.PuppetArms.RIGHT_HAND;
		int num7 = (int)puppetArms;
		if (puppet_arms[num7] != null && puppet_arms[num7].CheckEquipmentEquipLegal(this, puppetArms))
		{
			if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MIN) != null || puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MAX) != null)
			{
				puppet_property.main_weapon_physical_dmg[0] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MIN).value;
				puppet_property.main_weapon_physical_dmg[1] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MAX).value;
			}
			if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MIN) != null || puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MAX) != null)
			{
				puppet_property.main_weapon_magical_dmg[0] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MIN).value;
				puppet_property.main_weapon_magical_dmg[1] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MAX).value;
			}
			if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.INTERVAL) != null)
			{
				num5 = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.INTERVAL).value;
			}
			num3 = ((puppet_arms[num7].equipment_class != D3DEquipment.EquipmentClass.STAFF) ? ((puppet_property.main_weapon_physical_dmg[0] + puppet_property.main_weapon_physical_dmg[1]) / (2f * num5)) : ((puppet_property.main_weapon_magical_dmg[0] + puppet_property.main_weapon_magical_dmg[1]) / (2f * num5)));
		}
		puppetArms = D3DPuppetProfile.PuppetArms.LEFT_HAND;
		num7 = (int)puppetArms;
		if (puppet_arms[num7] != null && puppet_arms[num7].CheckEquipmentEquipLegal(this, puppetArms))
		{
			if (puppet_arms[num7].equipment_class == D3DEquipment.EquipmentClass.SHIELD)
			{
				if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.ARMOR) != null)
				{
					num6 += puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.ARMOR).value;
				}
			}
			else
			{
				if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MIN) != null || puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MAX) != null)
				{
					puppet_property.sub_weapon_physical_dmg = new float[2];
					puppet_property.sub_weapon_physical_dmg[0] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MIN).value * D3DFormulas.dual_adjust;
					puppet_property.sub_weapon_physical_dmg[1] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_MAX).value * D3DFormulas.dual_adjust;
				}
				if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MIN) != null || puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MAX) != null)
				{
					puppet_property.sub_weapon_magical_dmg = new float[2];
					puppet_property.sub_weapon_magical_dmg[0] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MIN).value * D3DFormulas.dual_adjust;
					puppet_property.sub_weapon_magical_dmg[1] = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_MAX).value * D3DFormulas.dual_adjust;
				}
				float num8 = puppet_property.attack_interval;
				if (puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.INTERVAL) != null)
				{
					num8 = puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.INTERVAL).value;
				}
				num4 = ((puppet_arms[num7].equipment_class == D3DEquipment.EquipmentClass.STAFF) ? ((puppet_property.sub_weapon_magical_dmg == null) ? 0f : ((puppet_property.sub_weapon_magical_dmg[0] + puppet_property.sub_weapon_magical_dmg[1]) / (2f * num8))) : ((puppet_property.sub_weapon_physical_dmg == null) ? 0f : ((puppet_property.sub_weapon_physical_dmg[0] + puppet_property.sub_weapon_physical_dmg[1]) / (2f * num8))));
				puppet_property.dual_adjust = D3DFormulas.dual_dmg_adjust;
				num5 = (num3 * num5 + num4 * num8) / (num3 + num4);
			}
		}
		puppet_property.attack_interval = num5;
		puppet_property.attack_interval_coe = num5;
		for (puppetArms = D3DPuppetProfile.PuppetArms.ARMOR; puppetArms <= D3DPuppetProfile.PuppetArms.BOOTS; puppetArms++)
		{
			if (puppetArms != D3DPuppetProfile.PuppetArms.BELT && puppetArms != D3DPuppetProfile.PuppetArms.WRIST)
			{
				num7 = (int)puppetArms;
				if (puppet_arms[num7] != null && puppet_arms[num7].CheckEquipmentEquipLegal(this, puppetArms) && puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.ARMOR) != null)
				{
					num6 += puppet_arms[num7].GetBasicAttributes(D3DEquipment.BasicAttribute.ARMOR).value;
				}
			}
		}
		puppet_property.armor = puppet_profile.profile_talent[puppet_profile.current_power].GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.ARMOR) + puppet_class.class_talent.GetAbilityByLevel(puppet_level, D3DPuppetTalent.TalentAbility.ARMOR) + puppet_formula.ConvertDefense(puppet_property) + array[9];
		puppet_property.armor += num6;
		puppet_property.armor *= 1f + array2[9];
		puppet_property.armor = Mathf.Round(puppet_property.armor);
		if (puppet_property.armor < 0f)
		{
			puppet_property.armor = 0f;
		}
		puppet_property.hp_max = puppet_property.hp;
		puppet_property.mp_max = puppet_property.mp;
		if (d3DPuppetProperty.strength > puppet_property.strength)
		{
			property_color_state[0] = -1;
		}
		else if (d3DPuppetProperty.strength < puppet_property.strength)
		{
			property_color_state[0] = 1;
		}
		else
		{
			property_color_state[0] = 0;
		}
		if (d3DPuppetProperty.agility > puppet_property.agility)
		{
			property_color_state[1] = -1;
		}
		else if (d3DPuppetProperty.agility < puppet_property.agility)
		{
			property_color_state[1] = 1;
		}
		else
		{
			property_color_state[1] = 0;
		}
		if (d3DPuppetProperty.spirit > puppet_property.spirit)
		{
			property_color_state[2] = -1;
		}
		else if (d3DPuppetProperty.spirit < puppet_property.spirit)
		{
			property_color_state[2] = 1;
		}
		else
		{
			property_color_state[2] = 0;
		}
		if (d3DPuppetProperty.stamina > puppet_property.stamina)
		{
			property_color_state[3] = -1;
		}
		else if (d3DPuppetProperty.stamina < puppet_property.stamina)
		{
			property_color_state[3] = 1;
		}
		else
		{
			property_color_state[3] = 0;
		}
		if (d3DPuppetProperty.intelligence > puppet_property.intelligence)
		{
			property_color_state[4] = -1;
		}
		else if (d3DPuppetProperty.intelligence < puppet_property.intelligence)
		{
			property_color_state[4] = 1;
		}
		else
		{
			property_color_state[4] = 0;
		}
		if (d3DPuppetProperty.attack_power > puppet_property.attack_power)
		{
			property_color_state[5] = -1;
		}
		else if (d3DPuppetProperty.attack_power < puppet_property.attack_power)
		{
			property_color_state[5] = 1;
		}
		else
		{
			property_color_state[5] = 0;
		}
		if (d3DPuppetProperty.magic_power > puppet_property.magic_power)
		{
			property_color_state[6] = -1;
		}
		else if (d3DPuppetProperty.magic_power < puppet_property.magic_power)
		{
			property_color_state[6] = 1;
		}
		else
		{
			property_color_state[6] = 0;
		}
	}

	public D3DPuppetVariableData GetVariableData()
	{
		D3DPuppetVariableData d3DPuppetVariableData = new D3DPuppetVariableData();
		d3DPuppetVariableData.hp_max = puppet_property.hp_max;
		d3DPuppetVariableData.mp_max = puppet_property.mp_max;
		d3DPuppetVariableData.puppet_physical_dmg = new float[2]
		{
			puppet_property.puppet_physical_dmg[0] * puppet_property.dual_adjust,
			puppet_property.puppet_physical_dmg[1] * puppet_property.dual_adjust
		};
		d3DPuppetVariableData.puppet_magical_dmg = new float[2]
		{
			puppet_property.puppet_magical_dmg[0] * puppet_property.dual_adjust,
			puppet_property.puppet_magical_dmg[1] * puppet_property.dual_adjust
		};
		d3DPuppetVariableData.main_weapon_physical_dmg = new float[2]
		{
			puppet_property.main_weapon_physical_dmg[0] * puppet_property.dual_adjust,
			puppet_property.main_weapon_physical_dmg[1] * puppet_property.dual_adjust
		};
		d3DPuppetVariableData.main_weapon_magical_dmg = new float[2]
		{
			puppet_property.main_weapon_magical_dmg[0] * puppet_property.dual_adjust,
			puppet_property.main_weapon_magical_dmg[1] * puppet_property.dual_adjust
		};
		if (puppet_property.sub_weapon_physical_dmg != null)
		{
			d3DPuppetVariableData.sub_weapon_physical_dmg = new float[2]
			{
				puppet_property.sub_weapon_physical_dmg[0] * puppet_property.dual_adjust,
				puppet_property.sub_weapon_physical_dmg[1] * puppet_property.dual_adjust
			};
		}
		if (puppet_property.sub_weapon_magical_dmg != null)
		{
			d3DPuppetVariableData.sub_weapon_magical_dmg = new float[2]
			{
				puppet_property.sub_weapon_magical_dmg[0] * puppet_property.dual_adjust,
				puppet_property.sub_weapon_magical_dmg[1] * puppet_property.dual_adjust
			};
		}
		d3DPuppetVariableData.attack_power = puppet_property.attack_power;
		d3DPuppetVariableData.magic_power = puppet_property.magic_power;
		d3DPuppetVariableData.physical_dps_dmg = D3DFormulas.GetPhysicalDps(puppet_property) * puppet_property.attack_interval * puppet_property.dual_adjust;
		d3DPuppetVariableData.magical_dps_dmg = D3DFormulas.GetMagicalDps(puppet_property) * puppet_property.attack_interval * puppet_property.dual_adjust;
		d3DPuppetVariableData.dmg_extra = puppet_property.DamageExtra;
		d3DPuppetVariableData.fixed_dmg_extra = puppet_property.fixed_dmg_extra;
		d3DPuppetVariableData.critical_chance = puppet_property.critical_chance;
		d3DPuppetVariableData.hatred_send = puppet_class.apply_hatred_send;
		return d3DPuppetVariableData;
	}

	public D3DGamer.D3DPuppetSaveData ExtractPuppetSaveData()
	{
		D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = new D3DGamer.D3DPuppetSaveData();
		d3DPuppetSaveData.puppet_level = puppet_level;
		d3DPuppetSaveData.pupet_profile_id = puppet_profile.profile_id;
		d3DPuppetSaveData.puppet_current_exp = current_exp;
		d3DPuppetSaveData.battle_puppet = battle_puppet;
		if (puppet_class.editable)
		{
			D3DGamer.D3DEquipmentSaveData[] array = puppet_profile.profile_arms[0];
			d3DPuppetSaveData.puppet_equipments = new D3DGamer.D3DEquipmentSaveData[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					d3DPuppetSaveData.puppet_equipments[i] = null;
				}
				else
				{
					d3DPuppetSaveData.puppet_equipments[i] = array[i].Clone();
				}
			}
			for (int j = 0; j < puppet_arms.Length; j++)
			{
				if (puppet_arms[j] == null)
				{
				}
			}
			d3DPuppetSaveData.active_skills = new List<string>();
			d3DPuppetSaveData.active_skill_levels = new List<int>();
			int num = 0;
			foreach (string key in puppet_class.active_skill_id_list.Keys)
			{
				d3DPuppetSaveData.active_skills.Add(key);
				d3DPuppetSaveData.active_skill_levels.Add(puppet_class.active_skill_id_list[key].skill_level);
				num++;
			}
			d3DPuppetSaveData.passive_skills = new List<string>();
			d3DPuppetSaveData.passive_skill_levels = new List<int>();
			num = 0;
			foreach (string key2 in puppet_class.passive_skill_id_list.Keys)
			{
				d3DPuppetSaveData.passive_skills.Add(key2);
				d3DPuppetSaveData.passive_skill_levels.Add(puppet_class.passive_skill_id_list[key2].skill_level);
				num++;
			}
		}
		if (battle_active_slots != null && battle_active_slots.Length > 0)
		{
			d3DPuppetSaveData.active_skill_slots = new List<string>();
			string[] array2 = battle_active_slots;
			foreach (string text in array2)
			{
				if (string.Empty != text)
				{
					d3DPuppetSaveData.active_skill_slots.Add(text);
				}
			}
		}
		if (battle_passive_slots != null && battle_passive_slots.Length > 0)
		{
			d3DPuppetSaveData.passive_skill_slots = new List<string>();
			string[] array3 = battle_passive_slots;
			foreach (string text2 in array3)
			{
				if (string.Empty != text2)
				{
					d3DPuppetSaveData.passive_skill_slots.Add(text2);
				}
			}
		}
		return d3DPuppetSaveData;
	}

	public void GetLevelUpExp(int exp)
	{
		exp += current_exp;
		while (true)
		{
			int num = D3DFormulas.ConvertLevelUpExp(puppet_level);
			if (exp >= num)
			{
				puppet_level++;
				exp -= num;
				continue;
			}
			break;
		}
		current_exp = exp;
	}

	public void OutputInformation()
	{
		string text = "Puppet Information\n-------------------\n";
		string text2 = text;
		text = text2 + "Puppet id:" + ProfileID + " | Class id:" + puppet_class.class_id + " | Class name:" + puppet_class.class_name + " | Power:" + Profile.current_power + "\n-------------------\n";
		text2 = text;
		text = text2 + "Level:" + puppet_level + "\n";
		text = text + "Right hand:" + ((puppet_arms[0] != null) ? puppet_arms[0].equipment_id : string.Empty) + "\n";
		text = text + "Left hand:" + ((puppet_arms[1] != null) ? puppet_arms[1].equipment_id : string.Empty) + "\n";
		text = text + "Armor:" + ((puppet_arms[2] != null) ? puppet_arms[2].equipment_id : string.Empty) + "\n";
		text = text + "Helm:" + ((puppet_arms[3] != null) ? puppet_arms[3].equipment_id : string.Empty) + "\n";
		text = text + "Boots:" + ((puppet_arms[5] != null) ? puppet_arms[5].equipment_id : string.Empty) + "\n";
		text = text + "Necklance:" + ((puppet_arms[7] != null) ? puppet_arms[7].equipment_id : string.Empty) + "\n";
		text = text + "Ring1:" + ((puppet_arms[8] != null) ? puppet_arms[8].equipment_id : string.Empty) + "\n";
		text = text + "Ring2:" + ((puppet_arms[9] != null) ? puppet_arms[9].equipment_id : string.Empty) + "\n-------------------\n";
		text2 = text;
		text = text2 + "HP:" + puppet_property.hp + "/" + puppet_property.hp_max + "\n";
		text2 = text;
		text = text2 + "SP/MP:" + puppet_property.mp + "/" + puppet_property.mp_max + "\n";
		text2 = text;
		text = text2 + "STR:" + puppet_property.strength + "\n";
		text2 = text;
		text = text2 + "AGI:" + puppet_property.agility + "\n";
		text2 = text;
		text = text2 + "SPI:" + puppet_property.spirit + "\n";
		text2 = text;
		text = text2 + "STA:" + puppet_property.stamina + "\n";
		text2 = text;
		text = text2 + "INT:" + puppet_property.intelligence + "\n-------------------\n";
		text2 = text;
		text = text2 + "ATTACK INTERVAL:" + puppet_property.attack_interval + "\n\n";
		text2 = text;
		text = text2 + "ATTACK POWER:" + puppet_property.attack_power + "\n";
		text2 = text;
		text = text2 + "PHYSICAL DPS:" + D3DFormulas.GetPhysicalDps(puppet_property) + "\n";
		text2 = text;
		text = text2 + "PHYSICAL DPS DAMAGE:" + D3DFormulas.GetPhysicalDps(puppet_property) * puppet_property.attack_interval_coe + "\n\n";
		text2 = text;
		text = text2 + "MAGIC POWER:" + puppet_property.magic_power + "\n";
		text2 = text;
		text = text2 + "MAGICAL DPS:" + D3DFormulas.GetMagicalDps(puppet_property) + "\n";
		text2 = text;
		text = text2 + "MAGICAL DPS DAMAGE:" + D3DFormulas.GetMagicalDps(puppet_property) * puppet_property.attack_interval_coe + "\n\n";
		text2 = text;
		text = text2 + "DAMAGE RANGE:" + puppet_formula.damage_range[0] + "-" + puppet_formula.damage_range[1] + "\n";
		text2 = text;
		text = text2 + "PHY DMG BY GROWTH:" + puppet_property.puppet_physical_dmg[0] + " - " + puppet_property.puppet_physical_dmg[1] + "\n";
		text2 = text;
		text = text2 + "MAG DMG BY GROWTH:" + puppet_property.puppet_magical_dmg[0] + " - " + puppet_property.puppet_magical_dmg[1] + "\n";
		text2 = text;
		text = text2 + "MAIN WEAPON PHYSICAL DAMAGE:" + puppet_property.main_weapon_physical_dmg[0] + " - " + puppet_property.main_weapon_physical_dmg[1] + "\n";
		if (puppet_property.sub_weapon_physical_dmg != null)
		{
			text2 = text;
			text = text2 + "SUB WEAPON PHYSICAL DAMAGE:" + puppet_property.sub_weapon_physical_dmg[0] + " - " + puppet_property.sub_weapon_physical_dmg[1] + "\n";
		}
		else
		{
			text += "SUB WEAPON PHYSICAL DAMAGE:NONE\n";
		}
		text2 = text;
		text = text2 + "MAIN WEAPON MAGICAL DAMAGE:" + puppet_property.main_weapon_magical_dmg[0] + " - " + puppet_property.main_weapon_magical_dmg[1] + "\n";
		if (puppet_property.sub_weapon_magical_dmg != null)
		{
			text2 = text;
			text = text2 + "SUB WEAPON MAGICAL DAMAGE:" + puppet_property.sub_weapon_magical_dmg[0] + " - " + puppet_property.sub_weapon_magical_dmg[1] + "\n-------------------\n";
		}
		else
		{
			text += "SUB WEAPON MAGICAL DAMAGE:NONE\n-------------------\n";
		}
		text2 = text;
		text = text2 + "ARMOR:" + puppet_property.armor + "\n";
		text2 = text;
		text = text2 + "ARMOR REDUCE DMG FROM SELF LEVEL:" + (1f - D3DFormulas.ConvertReduceDmgPercent(puppet_property, puppet_level)) + "\n";
		text2 = text;
		text = text2 + "HP RECOVER:" + puppet_property.hp_recover + "\n";
		text2 = text;
		text = text2 + "MP RECOVER:" + puppet_property.mp_recover + "\n";
		text2 = text;
		text = text2 + "MOVE SPEED:" + puppet_property.move_speed + "\n";
		text2 = text;
		text = text2 + "ROTATE SPEED:" + puppet_property.move_speed * D3DFormulas.RotateCoe + "\n";
		text2 = text;
		text = text2 + "DODGE CHANCE:" + puppet_property.dodge_chance + "\n";
		text2 = text;
		text = text2 + "CRITICAL CHANCE:" + puppet_property.critical_chance + "\n";
		text2 = text;
		text = text2 + "DMG OUTPUT:" + puppet_property.dmg_extra + "\n";
		text2 = text;
		text = text2 + "REDUCE LAST DMG:" + puppet_property.dmg_reduce + "\n";
		text2 = text;
		text = text2 + "EXP BONUS:" + puppet_property.exp_up + "\n";
		text2 = text;
		text = text2 + "STUN RESIST:" + puppet_property.stun_resist + "\n";
		text2 = text;
		text = text2 + "FEAR RESIST:" + puppet_property.fear_resist + "\n";
		text2 = text;
		text = text2 + "TREADY RESIST:" + puppet_property.trady_resist + "\n";
		text2 = text;
		text = text2 + "STAKME RESIST:" + puppet_property.stakme_resist + "\n";
	}

	public void InitSkillLevel(D3DGamer.D3DPuppetSaveData save_data)
	{
		foreach (string key in puppet_class.active_skill_id_list.Keys)
		{
			if (!puppet_class.editable)
			{
				puppet_class.active_skill_id_list[key].FillSkillLevel(puppet_level);
				continue;
			}
			if (save_data == null || save_data.active_skills == null || !save_data.active_skills.Contains(key))
			{
				puppet_class.active_skill_id_list[key].ResetSkillLevel();
				continue;
			}
			int num = save_data.active_skills.IndexOf(key);
			if (num < 0 || num > save_data.active_skill_levels.Count - 1)
			{
				puppet_class.active_skill_id_list[key].ResetSkillLevel();
			}
			else
			{
				puppet_class.active_skill_id_list[key].SetSkillLevel(puppet_level, save_data.active_skill_levels[num]);
			}
		}
		foreach (string key2 in puppet_class.passive_skill_id_list.Keys)
		{
			if (!puppet_class.editable)
			{
				puppet_class.passive_skill_id_list[key2].FillSkillLevel(puppet_level);
				continue;
			}
			if (save_data == null || save_data.passive_skills == null || !save_data.passive_skills.Contains(key2))
			{
				puppet_class.passive_skill_id_list[key2].ResetSkillLevel();
				continue;
			}
			int num2 = save_data.passive_skills.IndexOf(key2);
			if (num2 < 0 || num2 > save_data.passive_skill_levels.Count - 1)
			{
				puppet_class.passive_skill_id_list[key2].ResetSkillLevel();
			}
			else
			{
				puppet_class.passive_skill_id_list[key2].SetSkillLevel(puppet_level, save_data.passive_skill_levels[num2]);
			}
		}
	}

	public void FillSkillLevel()
	{
		foreach (string key in puppet_class.active_skill_id_list.Keys)
		{
			puppet_class.active_skill_id_list[key].FillSkillLevel(puppet_level);
		}
		foreach (string key2 in puppet_class.passive_skill_id_list.Keys)
		{
			puppet_class.passive_skill_id_list[key2].FillSkillLevel(puppet_level);
		}
	}

	public void InitSkillSlots(D3DGamer.D3DPuppetSaveData save_data)
	{
		List<string> list = new List<string>();
		int num = 0;
		if (save_data.active_skill_slots != null)
		{
			foreach (string active_skill_slot in save_data.active_skill_slots)
			{
				if (puppet_class.active_skill_id_list.ContainsKey(active_skill_slot) && puppet_class.active_skill_id_list[active_skill_slot].skill_level >= 0)
				{
					list.Add(active_skill_slot);
				}
			}
			num = ((list.Count <= D3DGamer.Instance.VaildSkillSlot) ? list.Count : D3DGamer.Instance.VaildSkillSlot);
			if (num > 4)
			{
				num = 4;
			}
			battle_active_slots = new string[num];
			for (int i = 0; i < num; i++)
			{
				battle_active_slots[i] = list[i];
			}
		}
		list.Clear();
		if (save_data.passive_skill_slots == null)
		{
			return;
		}
		foreach (string passive_skill_slot in save_data.passive_skill_slots)
		{
			if (puppet_class.passive_skill_id_list.ContainsKey(passive_skill_slot) && puppet_class.passive_skill_id_list[passive_skill_slot].skill_level >= 0)
			{
				list.Add(passive_skill_slot);
			}
		}
		num = ((list.Count <= D3DGamer.Instance.VaildSkillSlot) ? list.Count : D3DGamer.Instance.VaildSkillSlot);
		if (num > 4)
		{
			num = 4;
		}
		battle_passive_slots = new string[num];
		for (int j = 0; j < num; j++)
		{
			battle_passive_slots[j] = list[j];
		}
	}

	public void InitSkillSlots()
	{
		List<string> list = new List<string>();
		foreach (string key in puppet_class.active_skill_id_list.Keys)
		{
			if (puppet_class.active_skill_id_list[key].skill_level >= 0)
			{
				list.Add(key);
			}
		}
		battle_active_slots = new string[list.Count];
		for (int i = 0; i < battle_active_slots.Length; i++)
		{
			battle_active_slots[i] = list[i];
		}
		list.Clear();
		foreach (string key2 in puppet_class.passive_skill_id_list.Keys)
		{
			if (puppet_class.passive_skill_id_list[key2].skill_level >= 0)
			{
				list.Add(key2);
			}
		}
		battle_passive_slots = new string[list.Count];
		for (int j = 0; j < battle_passive_slots.Length; j++)
		{
			battle_passive_slots[j] = list[j];
		}
	}

	public void AddActiveSkillInSlots(string skill_id, int index)
	{
		if (index > battle_active_slots.Length - 1)
		{
			int num = index + 1;
			if (num > 4)
			{
				num = 4;
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				if (i > battle_active_slots.Length - 1)
				{
					array[i] = string.Empty;
				}
				else
				{
					array[i] = battle_active_slots[i];
				}
			}
			battle_active_slots = array;
		}
		battle_active_slots[index] = skill_id;
	}

	public void RemoveActiveSkillInSlots(int index)
	{
		if (index <= battle_active_slots.Length - 1)
		{
			battle_active_slots[index] = string.Empty;
		}
	}

	public void ExchangeActiveSkillInSlots(int source, int dest)
	{
		if (source != dest)
		{
			string skill_id = string.Empty;
			if (source <= battle_active_slots.Length - 1)
			{
				skill_id = battle_active_slots[source];
			}
			string skill_id2 = string.Empty;
			if (dest <= battle_active_slots.Length - 1)
			{
				skill_id2 = battle_active_slots[dest];
			}
			AddActiveSkillInSlots(skill_id, dest);
			AddActiveSkillInSlots(skill_id2, source);
		}
	}

	public int CheckActiveSkillExistsInSlot(string check_id)
	{
		for (int i = 0; i < battle_active_slots.Length && i <= D3DGamer.Instance.VaildSkillSlot - 1; i++)
		{
			if (battle_active_slots[i] == check_id)
			{
				return i;
			}
		}
		return -1;
	}

	public void AddPassiveSkillInSlots(string skill_id, int index)
	{
		if (index > battle_passive_slots.Length - 1)
		{
			int num = index + 1;
			if (num > 4)
			{
				num = 4;
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				if (i > battle_passive_slots.Length - 1)
				{
					array[i] = string.Empty;
				}
				else
				{
					array[i] = battle_passive_slots[i];
				}
			}
			battle_passive_slots = array;
		}
		battle_passive_slots[index] = skill_id;
	}

	public void RemovePassiveSkillInSlots(int index)
	{
		if (index <= battle_passive_slots.Length - 1)
		{
			battle_passive_slots[index] = string.Empty;
		}
	}

	public void ExchangePassiveSkillInSlots(int source, int dest)
	{
		if (source != dest)
		{
			string skill_id = string.Empty;
			if (source <= battle_passive_slots.Length - 1)
			{
				skill_id = battle_passive_slots[source];
			}
			string skill_id2 = string.Empty;
			if (dest <= battle_passive_slots.Length - 1)
			{
				skill_id2 = battle_passive_slots[dest];
			}
			AddPassiveSkillInSlots(skill_id, dest);
			AddPassiveSkillInSlots(skill_id2, source);
		}
	}

	public int CheckPassiveSkillExistsInSlot(string check_id)
	{
		for (int i = 0; i < battle_passive_slots.Length && i <= D3DGamer.Instance.VaildSkillSlot - 1; i++)
		{
			if (battle_passive_slots[i] == check_id)
			{
				return i;
			}
		}
		return -1;
	}

	public bool CheckBattleActiveSkill(string skill_id)
	{
		if (battle_active_skills == null)
		{
			return false;
		}
		if (battle_active_skills.ContainsKey(skill_id))
		{
			return true;
		}
		return false;
	}

	public bool CheckBattleActiveSkill(D3DClassBattleSkillStatus skill_status)
	{
		if (battle_active_skills == null)
		{
			return false;
		}
		if (battle_active_skills.ContainsValue(skill_status))
		{
			return true;
		}
		return false;
	}

	public bool GetTitanPower()
	{
		string[] array = battle_passive_slots;
		foreach (string key in array)
		{
			if (puppet_class.passive_skill_id_list.ContainsKey(key))
			{
				D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = puppet_class.passive_skill_id_list[key];
				if (d3DClassPassiveSkillStatus.passive_skill.GetTitanPower())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool GetDualWield()
	{
		string[] array = battle_passive_slots;
		foreach (string key in array)
		{
			if (puppet_class.passive_skill_id_list.ContainsKey(key))
			{
				D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = puppet_class.passive_skill_id_list[key];
				if (d3DClassPassiveSkillStatus.passive_skill.GetDualWield())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckCurrentPassiveIsTitanPower(int index)
	{
		if (index > battle_passive_slots.Length - 1)
		{
			return false;
		}
		if (puppet_class.passive_skill_id_list[battle_passive_slots[index]].passive_skill.GetTitanPower())
		{
			return true;
		}
		return false;
	}

	public bool CheckCurrentPassiveIsDualWield(int index)
	{
		if (index > battle_passive_slots.Length - 1)
		{
			return false;
		}
		if (puppet_class.passive_skill_id_list[battle_passive_slots[index]].passive_skill.GetDualWield())
		{
			return true;
		}
		return false;
	}

	public bool CheckNewSkill(bool bCheckLower = false)
	{
		List<string> list = new List<string>();
		foreach (string key in puppet_class.active_skill_id_list.Keys)
		{
			if ((bCheckLower || puppet_class.active_skill_id_list[key].UpgradeRequireLevel > puppet_level - 1) && puppet_class.active_skill_id_list[key].UpgradeRequireLevel <= puppet_level)
			{
				list.Add(key);
			}
		}
		foreach (string key2 in puppet_class.passive_skill_id_list.Keys)
		{
			if ((bCheckLower || puppet_class.passive_skill_id_list[key2].UpgradeRequireLevel > puppet_level - 1) && puppet_class.passive_skill_id_list[key2].UpgradeRequireLevel <= puppet_level && !puppet_class.passive_skill_id_list[key2]._bDeprecated)
			{
				list.Add(key2);
			}
		}
		if (list.Count > 0)
		{
			if (D3DGamer.Instance.NewSkillHint.ContainsKey(ProfileID))
			{
				if (D3DGamer.Instance.NewSkillHint[ProfileID] != null)
				{
					foreach (string item in list)
					{
						if (!D3DGamer.Instance.NewSkillHint[ProfileID].Contains(item))
						{
							D3DGamer.Instance.NewSkillHint[ProfileID].Add(item);
						}
					}
				}
				else
				{
					D3DGamer.Instance.NewSkillHint[ProfileID] = list;
				}
			}
			else
			{
				D3DGamer.Instance.NewSkillHint.Add(ProfileID, list);
			}
			if (D3DGamer.Instance.CurrentUnlockedSkills.ContainsKey(ProfileID))
			{
				if (D3DGamer.Instance.CurrentUnlockedSkills[ProfileID] != null)
				{
					foreach (string item2 in list)
					{
						if (!D3DGamer.Instance.CurrentUnlockedSkills[ProfileID].Contains(item2))
						{
							D3DGamer.Instance.CurrentUnlockedSkills[ProfileID].Add(item2);
						}
					}
				}
				else
				{
					D3DGamer.Instance.CurrentUnlockedSkills[ProfileID] = list;
				}
			}
			else
			{
				D3DGamer.Instance.CurrentUnlockedSkills.Add(ProfileID, list);
			}
			return true;
		}
		return false;
	}
}
