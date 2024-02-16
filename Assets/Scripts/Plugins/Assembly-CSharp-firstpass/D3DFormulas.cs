using System.Collections.Generic;
using UnityEngine;

public class D3DFormulas
{
	public static float RotateCoe = 135f;

	public float hp_coe;

	public float mp_coe;

	public float def_coe;

	public float[] main_power_coe = new float[3];

	public float[] sub_power_coe = new float[3];

	public float[] hp_recover_coe = new float[2];

	public float[] mp_recover_coe = new float[2];

	public float[] damage_range = new float[2] { 1f, 1f };

	public static float reduce_dmg_limit = 0f;

	public static float[] reduce_dmg_coe = new float[2];

	public static float[] dps_coe = new float[2];

	public static float dual_adjust = 0.8f;

	public static float dual_dmg_adjust = 0.55f;

	public static float Critical = 1.5f;

	public static float[] exp_bonus_coe = new float[2];

	public static float[] gold_bonus_coe = new float[2];

	public static float[] exp_coe = new float[5];

	public static Dictionary<int, float> ExpPunitive = new Dictionary<int, float>();

	public float ConvertHP(D3DPuppetProperty puppet_property)
	{
		return hp_coe * puppet_property.stamina;
	}

	public float ConvertMP(D3DPuppetProperty puppet_property)
	{
		return mp_coe * puppet_property.intelligence;
	}

	public float ConvertDefense(D3DPuppetProperty puppet_property)
	{
		return def_coe * puppet_property.agility;
	}

	public float ConvertMainPower(D3DClass.ClassType class_type, D3DPuppetProperty puppet_property, int puppet_level)
	{
		float num = 0f;
		switch (class_type)
		{
		case D3DClass.ClassType.STR_MAIN:
			num = puppet_property.strength;
			break;
		case D3DClass.ClassType.AGI_MAIN:
			num = puppet_property.agility;
			break;
		case D3DClass.ClassType.INT_MAIN:
			num = puppet_property.intelligence;
			break;
		}
		return (float)puppet_level * main_power_coe[0] + num * main_power_coe[1] + main_power_coe[2];
	}

	public float ConvertSubPower(D3DClass.ClassType class_type, D3DPuppetProperty puppet_property, int puppet_level)
	{
		float num = 0f;
		switch (class_type)
		{
		case D3DClass.ClassType.STR_MAIN:
			num = puppet_property.strength;
			break;
		case D3DClass.ClassType.AGI_MAIN:
			num = puppet_property.agility;
			break;
		case D3DClass.ClassType.INT_MAIN:
			num = puppet_property.intelligence;
			break;
		}
		return (float)puppet_level * sub_power_coe[0] + num * sub_power_coe[1] + sub_power_coe[2];
	}

	public float ConvertHpRecover(D3DPuppetProperty puppet_property)
	{
		return puppet_property.spirit * hp_recover_coe[0] + hp_recover_coe[1];
	}

	public float ConvertMpRecover(D3DPuppetProperty puppet_property)
	{
		return puppet_property.spirit * mp_recover_coe[0] + mp_recover_coe[1];
	}

	public static float ConvertReduceDmgPercent(D3DPuppetProperty puppet_property, int attacker_level)
	{
		float num = ((!(puppet_property.armor < 0f)) ? puppet_property.armor : 0f);
		float num2 = reduce_dmg_coe[0] * num / ((float)attacker_level + reduce_dmg_coe[1]);
		if (num2 > reduce_dmg_limit)
		{
			return reduce_dmg_limit;
		}
		return num2;
	}

	public static float GetPhysicalDps(D3DPuppetProperty puppet_property)
	{
		float num = ((!(puppet_property.attack_power < 0f)) ? puppet_property.attack_power : 0f);
		return num * dps_coe[0];
	}

	public static float GetMagicalDps(D3DPuppetProperty puppet_property)
	{
		float num = ((!(puppet_property.magic_power < 0f)) ? puppet_property.magic_power : 0f);
		return num * dps_coe[1];
	}

	public static int GetExpBonus(int battle_level)
	{
		return Mathf.RoundToInt(exp_bonus_coe[0] * (float)(battle_level - 1) + exp_bonus_coe[1]);
	}

	public static int GetGoldBonus(int battle_level)
	{
		return Mathf.RoundToInt(gold_bonus_coe[0] * (float)(battle_level - 1) + gold_bonus_coe[1]);
	}

	public static int ConvertLevelUpExp(int puppet_level)
	{
		return Mathf.RoundToInt((exp_coe[0] * Mathf.Pow(puppet_level, exp_coe[1]) + exp_coe[2]) * (exp_coe[3] * (float)(puppet_level - 1) + exp_coe[4]) / 3f);
	}

	public void InitFormulaCoe(string class_id)
	{
		D3DFormulaCoe d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item in D3DMain.Instance.hp_coe_list)
		{
			if (item.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.hp_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			hp_coe = d3DFormulaCoe.formula_coes[0];
		}
		d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item2 in D3DMain.Instance.mp_coe_list)
		{
			if (item2.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item2;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.mp_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			mp_coe = d3DFormulaCoe.formula_coes[0];
		}
		d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item3 in D3DMain.Instance.def_coe_list)
		{
			if (item3.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item3;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.def_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			def_coe = d3DFormulaCoe.formula_coes[0];
		}
		d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item4 in D3DMain.Instance.main_power_coe_list)
		{
			if (item4.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item4;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.main_power_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			main_power_coe[0] = d3DFormulaCoe.formula_coes[0];
			if (d3DFormulaCoe.formula_coes.Count > 1)
			{
				main_power_coe[1] = d3DFormulaCoe.formula_coes[1];
				if (d3DFormulaCoe.formula_coes.Count > 2)
				{
					main_power_coe[2] = d3DFormulaCoe.formula_coes[2];
				}
			}
		}
		d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item5 in D3DMain.Instance.sub_power_coe_list)
		{
			if (item5.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item5;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.sub_power_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			sub_power_coe[0] = d3DFormulaCoe.formula_coes[0];
			if (d3DFormulaCoe.formula_coes.Count > 1)
			{
				sub_power_coe[1] = d3DFormulaCoe.formula_coes[1];
				if (d3DFormulaCoe.formula_coes.Count > 2)
				{
					sub_power_coe[2] = d3DFormulaCoe.formula_coes[2];
				}
			}
		}
		d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item6 in D3DMain.Instance.mp_recover_coe_list)
		{
			if (item6.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item6;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.mp_recover_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			mp_recover_coe[0] = d3DFormulaCoe.formula_coes[0];
			if (d3DFormulaCoe.formula_coes.Count > 1)
			{
				mp_recover_coe[1] = d3DFormulaCoe.formula_coes[1];
			}
		}
		d3DFormulaCoe = null;
		foreach (D3DFormulaCoe item7 in D3DMain.Instance.damage_range_coe_list)
		{
			if (item7.formula_class.Contains(class_id))
			{
				d3DFormulaCoe = item7;
				break;
			}
		}
		if (d3DFormulaCoe == null)
		{
			d3DFormulaCoe = D3DMain.Instance.damage_range_default_coe;
		}
		if (d3DFormulaCoe.formula_coes.Count > 0)
		{
			damage_range[0] = d3DFormulaCoe.formula_coes[0];
			if (d3DFormulaCoe.formula_coes.Count > 1)
			{
				damage_range[1] = d3DFormulaCoe.formula_coes[1];
			}
		}
	}

	public static float GetExpPunitive(int level_diff)
	{
		float result = 1f;
		foreach (int key in ExpPunitive.Keys)
		{
			if (level_diff >= key)
			{
				result = ExpPunitive[key];
			}
		}
		return result;
	}
}
