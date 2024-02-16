public class D3DStateTriggerSimple : D3DStateTrigger
{
	public D3DFloat odds;

	public D3DFloat radius;

	public D3DFloat fixed_value;

	public D3DFloat percent_value;

	public D3DInt vaild_count;

	public D3DFloat vaild_time;

	public D3DFloat vaild_interval;

	public D3DStateTriggerSimple()
	{
		odds = null;
		radius = null;
		fixed_value = null;
		percent_value = null;
		vaild_count = null;
		vaild_time = null;
		vaild_interval = null;
	}

	~D3DStateTriggerSimple()
	{
	}

	public D3DStateTriggerSimple Clone()
	{
		D3DStateTriggerSimple d3DStateTriggerSimple = new D3DStateTriggerSimple();
		d3DStateTriggerSimple.trigger_type = trigger_type;
		d3DStateTriggerSimple.trigger_faction = trigger_faction;
		if (odds != null)
		{
			d3DStateTriggerSimple.odds = new D3DFloat(odds.value);
		}
		if (radius != null)
		{
			d3DStateTriggerSimple.radius = new D3DFloat(radius.value);
		}
		if (fixed_value != null)
		{
			d3DStateTriggerSimple.fixed_value = new D3DFloat(fixed_value.value);
		}
		if (percent_value != null)
		{
			d3DStateTriggerSimple.percent_value = new D3DFloat(percent_value.value);
		}
		if (vaild_count != null)
		{
			d3DStateTriggerSimple.vaild_count = new D3DInt(vaild_count.value);
		}
		if (vaild_time != null)
		{
			d3DStateTriggerSimple.vaild_time = new D3DFloat(vaild_time.value);
		}
		if (vaild_interval != null)
		{
			d3DStateTriggerSimple.vaild_interval = new D3DFloat(vaild_interval.value);
		}
		return d3DStateTriggerSimple;
	}

	public void CreateTriggerData(TriggerDataType data_type, string value_data)
	{
		switch (data_type)
		{
		case TriggerDataType.ODDS:
			odds = new D3DFloat(float.Parse(value_data));
			break;
		case TriggerDataType.RADIUS:
			radius = new D3DFloat(float.Parse(value_data));
			break;
		case TriggerDataType.FIXED_VALUE:
			fixed_value = new D3DFloat(float.Parse(value_data));
			break;
		case TriggerDataType.PERCENT_VALUE:
			percent_value = new D3DFloat(float.Parse(value_data));
			break;
		case TriggerDataType.COUNT:
			vaild_count = new D3DInt(int.Parse(value_data));
			break;
		case TriggerDataType.TIME:
			vaild_time = new D3DFloat(float.Parse(value_data));
			break;
		case TriggerDataType.INTERVAL:
			vaild_interval = new D3DFloat(float.Parse(value_data));
			break;
		}
	}
}
