public class D3DPassiveTriggerSimple : D3DPassiveTrigger
{
	public D3DFloat fixed_value;

	public D3DFloat percent_value;

	public D3DPassiveTriggerSimple()
	{
		fixed_value = null;
		percent_value = null;
	}

	~D3DPassiveTriggerSimple()
	{
	}

	public D3DPassiveTriggerSimple Clone()
	{
		D3DPassiveTriggerSimple d3DPassiveTriggerSimple = new D3DPassiveTriggerSimple();
		d3DPassiveTriggerSimple.passive_type = passive_type;
		if (fixed_value != null)
		{
			d3DPassiveTriggerSimple.fixed_value = new D3DFloat(fixed_value.value);
		}
		if (percent_value != null)
		{
			d3DPassiveTriggerSimple.percent_value = new D3DFloat(percent_value.value);
		}
		return d3DPassiveTriggerSimple;
	}

	public void CreateTriggerData(PassiveDataType data_type, string value_data)
	{
		switch (data_type)
		{
		case PassiveDataType.FIXED_VALUE:
			fixed_value = new D3DFloat(float.Parse(value_data));
			break;
		case PassiveDataType.PERCENT_VALUE:
			percent_value = new D3DFloat(float.Parse(value_data));
			break;
		}
	}
}
