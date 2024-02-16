using System.Collections.Generic;

public class D3DActiveSkill : D3DSkillBasic
{
	public enum ActiveType
	{
		TAP_ENEMY = 0,
		TAP_FRIEND = 1,
		TAP_FRIEND_EXCLUDE_ME = 2,
		PROMPT = 3
	}

	public const string ASK = "KQ[-ix1,#t,Wm)6V";

	public ActiveType active_type;

	public bool activation;

	public List<float> mp_consume;

	public List<float> cd;

	public List<float> distance;

	public List<ActiveSkillTrigger> skill_triggers;

	public D3DActiveSkill()
	{
		activation = false;
		mp_consume = new List<float>();
		cd = new List<float>();
		distance = new List<float>();
	}

	public override void CreateReplaceTags()
	{
		foreach (ActiveSkillTrigger skill_trigger in skill_triggers)
		{
			if (skill_trigger.trigger_count != null && string.Empty != skill_trigger.trigger_count.replace_tag)
			{
				replace_tags.Add(skill_trigger.trigger_count.replace_tag, skill_trigger.trigger_count.values);
			}
			if (skill_trigger.trigger_interval != null && string.Empty != skill_trigger.trigger_interval.replace_tag)
			{
				replace_tags.Add(skill_trigger.trigger_interval.replace_tag, skill_trigger.trigger_interval.values);
			}
			if (skill_trigger.trigger_lifecycle != null && string.Empty != skill_trigger.trigger_lifecycle.replace_tag)
			{
				replace_tags.Add(skill_trigger.trigger_lifecycle.replace_tag, skill_trigger.trigger_lifecycle.values);
			}
			if (skill_trigger.area_of_effect != null && skill_trigger.area_of_effect.range_description != null && string.Empty != skill_trigger.area_of_effect.range_description.replace_tag)
			{
				replace_tags.Add(skill_trigger.area_of_effect.range_description.replace_tag, skill_trigger.area_of_effect.range_description.values);
			}
			if (skill_trigger.trigger_variable != null)
			{
				foreach (TriggerVariable item in skill_trigger.trigger_variable)
				{
					if (item.variable_values != null)
					{
						foreach (TriggerVariable.VariableValue variable_value in item.variable_values)
						{
							if (variable_value != null && variable_value.values != null && string.Empty != variable_value.values.replace_tag)
							{
								replace_tags.Add(variable_value.values.replace_tag, variable_value.values.values);
							}
						}
					}
					if (item.dot_config != null)
					{
						if (item.dot_config.dot_time != null && string.Empty != item.dot_config.dot_time.replace_tag)
						{
							replace_tags.Add(item.dot_config.dot_time.replace_tag, item.dot_config.dot_time.values);
						}
						if (item.dot_config.dot_interval != null && string.Empty != item.dot_config.dot_interval.replace_tag)
						{
							replace_tags.Add(item.dot_config.dot_interval.replace_tag, item.dot_config.dot_interval.values);
						}
						if (item.dot_config.extra_variable != null && item.dot_config.extra_variable.extra_values != null)
						{
							foreach (TriggerVariable.VariableValue extra_value in item.dot_config.extra_variable.extra_values)
							{
								if (extra_value != null && extra_value.values != null && string.Empty != extra_value.values.replace_tag)
								{
									replace_tags.Add(extra_value.values.replace_tag, extra_value.values.values);
								}
							}
						}
					}
					if (item.output_config != null && item.output_config.imbibe_config != null && item.output_config.imbibe_config.imbibe_percent != null && string.Empty != item.output_config.imbibe_config.imbibe_percent.replace_tag)
					{
						replace_tags.Add(item.output_config.imbibe_config.imbibe_percent.replace_tag, item.output_config.imbibe_config.imbibe_percent.values);
					}
					if (item.aureole_config != null)
					{
						if (item.aureole_config.aureole_radius != null && string.Empty != item.aureole_config.aureole_radius.replace_tag)
						{
							replace_tags.Add(item.aureole_config.aureole_radius.replace_tag, item.aureole_config.aureole_radius.values);
						}
						if (item.aureole_config.aureole_time != null && string.Empty != item.aureole_config.aureole_time.replace_tag)
						{
							replace_tags.Add(item.aureole_config.aureole_time.replace_tag, item.aureole_config.aureole_time.values);
						}
					}
				}
			}
			if (skill_trigger.trigger_special != null)
			{
				if (skill_trigger.trigger_special.revive != null)
				{
					if (skill_trigger.trigger_special.revive.recover_hp != null && string.Empty != skill_trigger.trigger_special.revive.recover_hp.replace_tag)
					{
						replace_tags.Add(skill_trigger.trigger_special.revive.recover_hp.replace_tag, skill_trigger.trigger_special.revive.recover_hp.values);
					}
					if (skill_trigger.trigger_special.revive.recover_mp != null && string.Empty != skill_trigger.trigger_special.revive.recover_mp.replace_tag)
					{
						replace_tags.Add(skill_trigger.trigger_special.revive.recover_mp.replace_tag, skill_trigger.trigger_special.revive.recover_mp.values);
					}
				}
				if (skill_trigger.trigger_special.dispel != null && skill_trigger.trigger_special.dispel.dispel_count != null && string.Empty != skill_trigger.trigger_special.dispel.dispel_count.replace_tag)
				{
					replace_tags.Add(skill_trigger.trigger_special.dispel.dispel_count.replace_tag, skill_trigger.trigger_special.dispel.dispel_count.values);
				}
				if (skill_trigger.trigger_special.summon != null)
				{
					if (skill_trigger.trigger_special.summon.summon_id != null && string.Empty != skill_trigger.trigger_special.summon.summon_id.replace_tag)
					{
						replace_tags.Add(skill_trigger.trigger_special.summon.summon_id.replace_tag, skill_trigger.trigger_special.summon.summon_id.values);
					}
					if (skill_trigger.trigger_special.summon.summon_level != null && string.Empty != skill_trigger.trigger_special.summon.summon_level.replace_tag)
					{
						replace_tags.Add(skill_trigger.trigger_special.summon.summon_level.replace_tag, skill_trigger.trigger_special.summon.summon_level.values);
					}
					if (skill_trigger.trigger_special.summon.summon_life != null && string.Empty != skill_trigger.trigger_special.summon.summon_life.replace_tag)
					{
						replace_tags.Add(skill_trigger.trigger_special.summon.summon_life.replace_tag, skill_trigger.trigger_special.summon.summon_life.values);
					}
					if (skill_trigger.trigger_special.summon.summon_count != null && string.Empty != skill_trigger.trigger_special.summon.summon_count.replace_tag)
					{
						replace_tags.Add(skill_trigger.trigger_special.summon.summon_count.replace_tag, skill_trigger.trigger_special.summon.summon_count.values);
					}
				}
			}
			if (skill_trigger.trigger_crowd_control != null)
			{
				foreach (TriggerCrowdControl item2 in skill_trigger.trigger_crowd_control)
				{
					if (item2.odds != null && string.Empty != item2.odds.replace_tag)
					{
						replace_tags.Add(item2.odds.replace_tag, item2.odds.values);
					}
					if (item2.time != null && string.Empty != item2.time.replace_tag)
					{
						replace_tags.Add(item2.time.replace_tag, item2.time.values);
					}
				}
			}
			if (skill_trigger.trigger_buff == null)
			{
				continue;
			}
			foreach (TriggerBuff item3 in skill_trigger.trigger_buff)
			{
				if (item3.odds != null && string.Empty != item3.odds.replace_tag)
				{
					replace_tags.Add(item3.odds.replace_tag, item3.odds.values);
				}
				if (item3.time != null && string.Empty != item3.time.replace_tag)
				{
					replace_tags.Add(item3.time.replace_tag, item3.time.values);
				}
				if (item3.buff_value != null && string.Empty != item3.buff_value.replace_tag)
				{
					replace_tags.Add(item3.buff_value.replace_tag, item3.buff_value.values);
				}
				if (item3.aureole_config != null)
				{
					if (item3.aureole_config.aureole_radius != null && string.Empty != item3.aureole_config.aureole_radius.replace_tag)
					{
						replace_tags.Add(item3.aureole_config.aureole_radius.replace_tag, item3.aureole_config.aureole_radius.values);
					}
					if (item3.aureole_config.aureole_time != null && string.Empty != item3.aureole_config.aureole_time.replace_tag)
					{
						replace_tags.Add(item3.aureole_config.aureole_time.replace_tag, item3.aureole_config.aureole_time.values);
					}
				}
			}
		}
	}
}
