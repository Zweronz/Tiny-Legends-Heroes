using System.Collections.Generic;

public class D3DClassBattleSkillStatus : D3DClassActiveSkillStatus
{
	public D3DPuppetProperty puppet_property;

	public float cast_delta;

	public float cd_delta;

	public D3DFloat common_cd_delta;

	public D3DFloat common_cd;

	public D3DFloat temp_cd;

	public D3DBattleSkillUI battle_ui;

	public bool forbidden;

	public bool freeze;

	public bool cancelable;

	public float MPConsume
	{
		get
		{
			if (active_skill.mp_consume.Count == 0)
			{
				return 0f;
			}
			int num = active_skill.mp_consume.Count - 1;
			int index = ((skill_level <= num) ? skill_level : num);
			return active_skill.mp_consume[index];
		}
	}

	public float Distance
	{
		get
		{
			if (active_skill.distance.Count == 0)
			{
				return -1f;
			}
			int num = active_skill.distance.Count - 1;
			int index = ((skill_level <= num) ? skill_level : num);
			return active_skill.distance[index];
		}
	}

	public float CD
	{
		get
		{
			if (active_skill.cd.Count == 0)
			{
				return -1f;
			}
			int num = active_skill.cd.Count - 1;
			int index = ((skill_level <= num) ? skill_level : num);
			return (!(active_skill.cd[index] < 0f)) ? active_skill.cd[index] : common_cd.value;
		}
	}

	public float CDPercent
	{
		get
		{
			if (active_skill.cd.Count == 0)
			{
				if (common_cd.value <= 0f)
				{
					return 0f;
				}
				return common_cd_delta.value / common_cd.value;
			}
			int num = active_skill.cd.Count - 1;
			int index = ((skill_level <= num) ? skill_level : num);
			if (active_skill.cd[index] < 0f)
			{
				if (common_cd.value <= 0f)
				{
					return 0f;
				}
				return common_cd_delta.value / common_cd.value;
			}
			if (active_skill.cd[index] == 0f)
			{
				return 0f;
			}
			return cd_delta / active_skill.cd[index];
		}
	}

	public bool Enable
	{
		get
		{
			if (puppet_property.mp < MPConsume)
			{
				return false;
			}
			if (!IsActivateSkill())
			{
				if (forbidden)
				{
					return false;
				}
				if (freeze)
				{
					return false;
				}
				return CDPercent <= 0f;
			}
			return true;
		}
	}

	public override bool Init()
	{
		cast_delta = 0f;
		cd_delta = 0f;
		active_skill = D3DMain.Instance.GetActiveSkill(skill_id);
		if (active_skill == null || skill_id != active_skill.skill_id)
		{
			return false;
		}
		if (active_skill.active_type == D3DActiveSkill.ActiveType.PROMPT)
		{
			cancelable = false;
		}
		else if (active_skill.activation)
		{
			cancelable = true;
		}
		else
		{
			cancelable = false;
		}
		return true;
	}

	public void SetStatusProperties(D3DClassActiveSkillStatus active_status)
	{
		skill_id = active_status.skill_id;
		cast_time = active_status.cast_time;
		skill_level = active_status.skill_level;
		animation_clip_index = active_status.animation_clip_index;
		clip_frames = active_status.clip_frames;
	}

	public void SetSkillCommonCD(D3DFloat cd_delta, D3DFloat cd)
	{
		common_cd_delta = cd_delta;
		common_cd = cd;
	}

	public void FillCD()
	{
		if (active_skill.cd.Count == 0)
		{
			common_cd_delta.value = common_cd.value;
			return;
		}
		int num = active_skill.cd.Count - 1;
		int index = ((skill_level <= num) ? skill_level : num);
		if (active_skill.cd[index] < 0f)
		{
			common_cd_delta.value = common_cd.value;
		}
		else
		{
			cd_delta = active_skill.cd[index];
		}
	}

	public void RefreshCD()
	{
		if (active_skill.cd.Count != 0)
		{
			int num = active_skill.cd.Count - 1;
			int index = ((skill_level <= num) ? skill_level : num);
			if (active_skill.cd[index] >= 0f)
			{
				cd_delta = 0f;
			}
		}
	}

	public bool IsActivateSkill()
	{
		if ((active_skill.active_type == D3DActiveSkill.ActiveType.TAP_ENEMY || active_skill.active_type == D3DActiveSkill.ActiveType.TAP_FRIEND || active_skill.active_type == D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME) && active_skill.activation)
		{
			return true;
		}
		return false;
	}

	public ActiveSkillTrigger GetFrameTrigger(int frame, int animation_type)
	{
		if (clip_frames == null)
		{
			return null;
		}
		int num = 0;
		foreach (List<int> item in clip_frames[animation_type])
		{
			if (item == null || item.Count == 0)
			{
				num++;
				continue;
			}
			foreach (int item2 in item)
			{
				if (item2 == frame)
				{
					if (num > active_skill.skill_triggers.Count - 1)
					{
						return null;
					}
					return active_skill.skill_triggers[num];
				}
			}
			num++;
		}
		return null;
	}

	public ActiveSkillTrigger GetFrameTrigger()
	{
		return active_skill.skill_triggers[0];
	}
}
