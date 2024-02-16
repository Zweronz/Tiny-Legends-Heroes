using System.Collections.Generic;

public class D3DPassiveSkill : D3DSkillBasic
{
	public const string PSK = "r@]3xG7I,WiLG65-";

	public List<D3DPassiveTriggerComplex> passive_triggers;

	public D3DPassiveSkill()
	{
		passive_triggers = new List<D3DPassiveTriggerComplex>();
	}

	public override void CreateReplaceTags()
	{
		foreach (D3DPassiveTriggerComplex passive_trigger in passive_triggers)
		{
			if (passive_trigger.fixed_value != null && string.Empty != passive_trigger.fixed_value.replace_tag)
			{
				replace_tags.Add(passive_trigger.fixed_value.replace_tag, passive_trigger.fixed_value.values);
			}
			if (passive_trigger.percent_value != null && string.Empty != passive_trigger.percent_value.replace_tag)
			{
				replace_tags.Add(passive_trigger.percent_value.replace_tag, passive_trigger.percent_value.values);
			}
		}
	}

	public bool GetTitanPower()
	{
		foreach (D3DPassiveTriggerComplex passive_trigger in passive_triggers)
		{
			if (passive_trigger.passive_type == D3DPassiveTrigger.PassiveType.TITAN_POWER)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetDualWield()
	{
		foreach (D3DPassiveTriggerComplex passive_trigger in passive_triggers)
		{
			if (passive_trigger.passive_type == D3DPassiveTrigger.PassiveType.DUAL_WIELD)
			{
				return true;
			}
		}
		return false;
	}
}
