using System.Collections.Generic;

public class ActiveSkillTrigger
{
	public bool lock_frame;

	public float trigger_delay;

	public D3DTextInt trigger_count;

	public D3DTextFloat trigger_interval;

	public D3DTextFloat trigger_lifecycle;

	public bool emplacement;

	public bool independence;

	public float camera_shake_time;

	public bool puppet_shake;

	public List<TriggerBedeckEffect> common_bedeck_effects;

	public List<TriggerBedeckEffect> lifecycle_bedeck_effects;

	public TriggerMissile trigger_missile;

	public AreaOfEffect area_of_effect;

	public List<TriggerVariable> trigger_variable;

	public TriggerSpecial trigger_special;

	public List<TriggerCrowdControl> trigger_crowd_control;

	public List<TriggerBuff> trigger_buff;

	public ActiveSkillTrigger()
	{
		camera_shake_time = 0f;
		puppet_shake = false;
	}

	public int TriggerCount(int level)
	{
		if (trigger_count == null)
		{
			return 1;
		}
		int num = 0;
		int count = trigger_count.values.Count;
		if (count == 0)
		{
			return 1;
		}
		int index = ((level <= count - 1) ? level : (count - 1));
		return trigger_count.values[index];
	}

	public float TriggerInterval(int level)
	{
		if (trigger_interval == null)
		{
			return 0f;
		}
		float num = 0f;
		int count = trigger_interval.values.Count;
		if (count == 0)
		{
			return 0f;
		}
		int index = ((level <= count - 1) ? level : (count - 1));
		return trigger_interval.values[index];
	}

	public float TriggerLifeCycle(int level)
	{
		if (trigger_lifecycle == null)
		{
			return 0f;
		}
		float num = 0f;
		int count = trigger_lifecycle.values.Count;
		if (count == 0)
		{
			return 0f;
		}
		int index = ((level <= count - 1) ? level : (count - 1));
		return trigger_lifecycle.values[index];
	}
}
