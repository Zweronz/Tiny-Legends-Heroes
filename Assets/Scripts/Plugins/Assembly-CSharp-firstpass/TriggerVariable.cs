using System.Collections.Generic;

public class TriggerVariable
{
	public enum VariableType
	{
		HP_DAMAGE = 0,
		HP_RECOVER = 1,
		MP_DAMAGE = 2,
		MP_RECOVER = 3
	}

	public class VariableValue
	{
		public enum VariableSource
		{
			FIXED_VALUE = 0,
			FIXED_MAGICAL = 1,
			PERCENT_TARGET_MAXHP = 2,
			PERCENT_TARGET_MAXMP = 3,
			PERCENT_CASTER_MAXHP = 4,
			PERCENT_CASTER_MAXMP = 5,
			PERCENT_CASTER_ATTACKPOWER = 6,
			PERCENT_CASTER_MAGICPOWNER = 7,
			PERCENT_CASTER_MAIN_WEAPON_PHY = 8,
			PERCENT_CASTER_SUB_WEAPON_PHY = 9,
			PERCENT_CASTER_MAIN_WEAPON_MAG = 10,
			PERCENT_CASTER_SUB_WEAPON_MAG = 11,
			PERCENT_CASTER_DPS_PHY = 12,
			PERCENT_CASTER_DPS_MAG = 13,
			PERCENT_CASTER_TALENT_PHY = 14,
			PERCENT_CASTER_TALENT_MAG = 15
		}

		public VariableSource variable_source;

		public D3DTextFloat values;

		public float GetValue(int level)
		{
			if (values == null || values.values.Count == 0)
			{
				return 0f;
			}
			int index = ((level <= values.values.Count - 1) ? level : (values.values.Count - 1));
			return values.values[index];
		}
	}

	public class DotConfig
	{
		public class ExtraVariable
		{
			public string extra_effect;

			public string mount_point;

			public string extra_sfx;

			public List<VariableValue> extra_values;

			public ExtraVariable()
			{
				extra_effect = string.Empty;
				extra_sfx = string.Empty;
			}

			public void StatVariable(D3DPuppetVariableData variable_data, int skill_level, ref float phy_value, ref float mag_value, ref float target_hpmax_percent, ref float target_mpmax_percent, ref List<float[]> random_phy_values, ref List<float[]> random_mag_values)
			{
				foreach (VariableValue extra_value in extra_values)
				{
					float value = extra_value.GetValue(skill_level);
					switch (extra_value.variable_source)
					{
					case VariableValue.VariableSource.FIXED_VALUE:
						phy_value += value;
						break;
					case VariableValue.VariableSource.FIXED_MAGICAL:
						mag_value += value;
						break;
					case VariableValue.VariableSource.PERCENT_TARGET_MAXHP:
						target_hpmax_percent += value;
						break;
					case VariableValue.VariableSource.PERCENT_TARGET_MAXMP:
						target_mpmax_percent += value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_MAXHP:
						mag_value += variable_data.hp_max * value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_MAXMP:
						mag_value += variable_data.mp_max * value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_ATTACKPOWER:
						phy_value += variable_data.attack_power * value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_MAGICPOWNER:
						mag_value += variable_data.magic_power * value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_MAIN_WEAPON_PHY:
						random_phy_values.Add(new float[2]
						{
							variable_data.main_weapon_physical_dmg[0] * value,
							variable_data.main_weapon_physical_dmg[1] * value
						});
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_SUB_WEAPON_PHY:
						if (variable_data.sub_weapon_physical_dmg != null)
						{
							random_phy_values.Add(new float[2]
							{
								variable_data.sub_weapon_physical_dmg[0] * value,
								variable_data.sub_weapon_physical_dmg[1] * value
							});
						}
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_MAIN_WEAPON_MAG:
						random_mag_values.Add(new float[2]
						{
							variable_data.main_weapon_magical_dmg[0] * value,
							variable_data.main_weapon_magical_dmg[1] * value
						});
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_SUB_WEAPON_MAG:
						if (variable_data.sub_weapon_magical_dmg != null)
						{
							random_mag_values.Add(new float[2]
							{
								variable_data.sub_weapon_magical_dmg[0] * value,
								variable_data.sub_weapon_magical_dmg[1] * value
							});
						}
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_DPS_PHY:
						phy_value += variable_data.physical_dps_dmg * value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_DPS_MAG:
						mag_value += variable_data.magical_dps_dmg * value;
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_TALENT_PHY:
						random_phy_values.Add(new float[2]
						{
							variable_data.puppet_physical_dmg[0] * value,
							variable_data.puppet_physical_dmg[1] * value
						});
						break;
					case VariableValue.VariableSource.PERCENT_CASTER_TALENT_MAG:
						random_mag_values.Add(new float[2]
						{
							variable_data.puppet_magical_dmg[0] * value,
							variable_data.puppet_magical_dmg[1] * value
						});
						break;
					}
				}
			}
		}

		public D3DTextFloat dot_time;

		public D3DTextFloat dot_interval;

		public string dot_effect;

		public string mount_point;

		public string dot_sfx;

		public bool remain_effect;

		public ExtraVariable extra_variable;

		public float DotTime(int level)
		{
			if (dot_time == null || dot_time.values.Count == 0)
			{
				return -1f;
			}
			int index = ((level <= dot_time.values.Count - 1) ? level : (dot_time.values.Count - 1));
			return dot_time.values[index];
		}

		public float DotInterval(int level)
		{
			if (dot_interval == null || dot_interval.values.Count == 0)
			{
				return -1f;
			}
			int index = ((level <= dot_interval.values.Count - 1) ? level : (dot_interval.values.Count - 1));
			return dot_interval.values[index];
		}
	}

	public VariableType variable_type;

	public List<VariableValue> variable_values;

	public DotConfig dot_config;

	public VariableOutputConfig output_config;

	public AureoleConfig aureole_config;

	public void StatVariable(D3DPuppetVariableData variable_data, int skill_level, ref float phy_value, ref float mag_value, ref float target_hpmax_percent, ref float target_mpmax_percent, ref List<float[]> random_phy_values, ref List<float[]> random_mag_values)
	{
		foreach (VariableValue variable_value in variable_values)
		{
			float value = variable_value.GetValue(skill_level);
			switch (variable_value.variable_source)
			{
			case VariableValue.VariableSource.FIXED_VALUE:
				phy_value += value;
				break;
			case VariableValue.VariableSource.FIXED_MAGICAL:
				mag_value += value;
				break;
			case VariableValue.VariableSource.PERCENT_TARGET_MAXHP:
				target_hpmax_percent += value;
				break;
			case VariableValue.VariableSource.PERCENT_TARGET_MAXMP:
				target_mpmax_percent += value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_MAXHP:
				mag_value += variable_data.hp_max * value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_MAXMP:
				mag_value += variable_data.mp_max * value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_ATTACKPOWER:
				phy_value += variable_data.attack_power * value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_MAGICPOWNER:
				mag_value += variable_data.magic_power * value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_MAIN_WEAPON_PHY:
				random_phy_values.Add(new float[2]
				{
					variable_data.main_weapon_physical_dmg[0] * value,
					variable_data.main_weapon_physical_dmg[1] * value
				});
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_SUB_WEAPON_PHY:
				if (variable_data.sub_weapon_physical_dmg != null)
				{
					random_phy_values.Add(new float[2]
					{
						variable_data.sub_weapon_physical_dmg[0] * value,
						variable_data.sub_weapon_physical_dmg[1] * value
					});
				}
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_MAIN_WEAPON_MAG:
				random_mag_values.Add(new float[2]
				{
					variable_data.main_weapon_magical_dmg[0] * value,
					variable_data.main_weapon_magical_dmg[1] * value
				});
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_SUB_WEAPON_MAG:
				if (variable_data.sub_weapon_magical_dmg != null)
				{
					random_mag_values.Add(new float[2]
					{
						variable_data.sub_weapon_magical_dmg[0] * value,
						variable_data.sub_weapon_magical_dmg[1] * value
					});
				}
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_DPS_PHY:
				phy_value += variable_data.physical_dps_dmg * value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_DPS_MAG:
				mag_value += variable_data.magical_dps_dmg * value;
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_TALENT_PHY:
				random_phy_values.Add(new float[2]
				{
					variable_data.puppet_physical_dmg[0] * value,
					variable_data.puppet_physical_dmg[1] * value
				});
				break;
			case VariableValue.VariableSource.PERCENT_CASTER_TALENT_MAG:
				random_mag_values.Add(new float[2]
				{
					variable_data.puppet_magical_dmg[0] * value,
					variable_data.puppet_magical_dmg[1] * value
				});
				break;
			}
		}
	}
}
