using System.Collections.Generic;
using UnityEngine;

public class PowerRule
{
	public class PowerValue
	{
		public int power_type;

		public float min;

		public float max;

		public float RandomValue
		{
			get
			{
				return Random.Range(min, max);
			}
		}
	}

	public string rule_id;

	public string affix;

	public List<PowerValue> power_value = new List<PowerValue>();

	public D3DMagicPowerSaveData RandomPower()
	{
		D3DMagicPowerSaveData d3DMagicPowerSaveData = new D3DMagicPowerSaveData();
		d3DMagicPowerSaveData.rule_id = rule_id;
		foreach (PowerValue item in power_value)
		{
			d3DMagicPowerSaveData.power_value.Add(item.power_type, item.RandomValue);
		}
		return d3DMagicPowerSaveData;
	}

	public bool ContainsPowerType(int power_type)
	{
		foreach (PowerValue item in power_value)
		{
			if (item.power_type == power_type)
			{
				return true;
			}
		}
		return false;
	}
}
