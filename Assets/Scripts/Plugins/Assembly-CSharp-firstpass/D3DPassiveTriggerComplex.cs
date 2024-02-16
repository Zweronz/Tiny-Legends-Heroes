public class D3DPassiveTriggerComplex : D3DPassiveTrigger
{
	public D3DTextFloat fixed_value;

	public D3DTextFloat percent_value;

	public D3DPassiveTriggerComplex()
	{
		fixed_value = null;
		percent_value = null;
	}

	~D3DPassiveTriggerComplex()
	{
	}

	public void CreateTriggerData(PassiveDataType data_type, string value_data)
	{
		if (passive_type == PassiveType.DUAL_WIELD || passive_type == PassiveType.TITAN_POWER)
		{
			return;
		}
		switch (data_type)
		{
		case PassiveDataType.FIXED_VALUE:
			if (fixed_value == null)
			{
				fixed_value = new D3DTextFloat();
			}
			fixed_value.values.Add(float.Parse(value_data));
			break;
		case PassiveDataType.PERCENT_VALUE:
			if (percent_value == null)
			{
				percent_value = new D3DTextFloat();
			}
			percent_value.values.Add(float.Parse(value_data));
			break;
		}
	}

	public void SetDataContentTag(PassiveDataType data_type, string content_tag)
	{
		if (passive_type == PassiveType.DUAL_WIELD || passive_type == PassiveType.TITAN_POWER)
		{
			return;
		}
		switch (data_type)
		{
		case PassiveDataType.FIXED_VALUE:
			if (fixed_value == null)
			{
				fixed_value = new D3DTextFloat();
			}
			fixed_value.replace_tag = content_tag;
			break;
		case PassiveDataType.PERCENT_VALUE:
			if (percent_value == null)
			{
				percent_value = new D3DTextFloat();
			}
			percent_value.replace_tag = content_tag;
			break;
		}
	}
}
