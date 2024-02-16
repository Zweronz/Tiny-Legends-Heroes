using System.Collections.Generic;

public class D3DMagicPowerSaveData
{
	public string rule_id;

	public Dictionary<int, float> power_value = new Dictionary<int, float>();

	public D3DMagicPowerSaveData Clone()
	{
		D3DMagicPowerSaveData d3DMagicPowerSaveData = new D3DMagicPowerSaveData();
		d3DMagicPowerSaveData.rule_id = rule_id;
		foreach (int key in power_value.Keys)
		{
			d3DMagicPowerSaveData.power_value.Add(key, power_value[key]);
		}
		return d3DMagicPowerSaveData;
	}
}
