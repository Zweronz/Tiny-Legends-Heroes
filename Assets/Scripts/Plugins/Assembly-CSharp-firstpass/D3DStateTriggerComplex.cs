using System.Collections.Generic;

public class D3DStateTriggerComplex : D3DStateTrigger
{
	public List<float> odds;

	public List<float> radius;

	public List<float> fixed_value;

	public List<float> percent_value;

	public List<int> vaild_count;

	public List<float> vaild_time;

	public List<float> vaild_interval;

	public D3DStateTriggerComplex()
	{
		odds = null;
		radius = null;
		fixed_value = null;
		percent_value = null;
		vaild_count = null;
		vaild_time = null;
		vaild_interval = null;
	}

	~D3DStateTriggerComplex()
	{
	}

	public D3DStateTriggerComplex Clone()
	{
		D3DStateTriggerComplex d3DStateTriggerComplex = new D3DStateTriggerComplex();
		d3DStateTriggerComplex.trigger_type = trigger_type;
		d3DStateTriggerComplex.trigger_faction = trigger_faction;
		if (odds != null)
		{
			d3DStateTriggerComplex.odds = new List<float>(odds);
		}
		if (radius != null)
		{
			d3DStateTriggerComplex.radius = new List<float>(radius);
		}
		if (fixed_value != null)
		{
			d3DStateTriggerComplex.fixed_value = new List<float>(fixed_value);
		}
		if (percent_value != null)
		{
			d3DStateTriggerComplex.percent_value = new List<float>(percent_value);
		}
		if (vaild_count != null)
		{
			d3DStateTriggerComplex.vaild_count = new List<int>(vaild_count);
		}
		if (vaild_time != null)
		{
			d3DStateTriggerComplex.vaild_time = new List<float>(vaild_time);
		}
		if (vaild_interval != null)
		{
			d3DStateTriggerComplex.vaild_interval = new List<float>(vaild_interval);
		}
		return d3DStateTriggerComplex;
	}

	public void CreateTriggerData(TriggerDataType data_type, string value_data)
	{
		switch (data_type)
		{
		case TriggerDataType.ODDS:
			if (odds == null)
			{
				odds = new List<float>();
			}
			odds.Add(float.Parse(value_data));
			break;
		case TriggerDataType.RADIUS:
			if (radius == null)
			{
				radius = new List<float>();
			}
			radius.Add(float.Parse(value_data));
			break;
		case TriggerDataType.FIXED_VALUE:
			if (fixed_value == null)
			{
				fixed_value = new List<float>();
			}
			fixed_value.Add(float.Parse(value_data));
			break;
		case TriggerDataType.PERCENT_VALUE:
			if (percent_value == null)
			{
				percent_value = new List<float>();
			}
			percent_value.Add(float.Parse(value_data));
			break;
		case TriggerDataType.COUNT:
			if (vaild_count == null)
			{
				vaild_count = new List<int>();
			}
			vaild_count.Add(int.Parse(value_data));
			break;
		case TriggerDataType.TIME:
			if (vaild_time == null)
			{
				vaild_time = new List<float>();
			}
			vaild_time.Add(float.Parse(value_data));
			break;
		case TriggerDataType.INTERVAL:
			if (vaild_interval == null)
			{
				vaild_interval = new List<float>();
			}
			vaild_interval.Add(float.Parse(value_data));
			break;
		}
	}
}
