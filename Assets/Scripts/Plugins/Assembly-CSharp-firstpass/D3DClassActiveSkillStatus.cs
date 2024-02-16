using System.Collections.Generic;

public class D3DClassActiveSkillStatus : D3DClassSkillStatus
{
	public D3DInt[] animation_clip_index;

	public List<List<int>>[] clip_frames;

	public D3DActiveSkill active_skill;

	public float cast_time;

	public D3DClassActiveSkillStatus()
	{
		animation_clip_index = new D3DInt[3];
		clip_frames = new List<List<int>>[3];
	}

	public D3DClassActiveSkillStatus Clone()
	{
		D3DClassActiveSkillStatus d3DClassActiveSkillStatus = new D3DClassActiveSkillStatus();
		d3DClassActiveSkillStatus.skill_id = skill_id;
		for (int i = 0; i < 3; i++)
		{
			if (animation_clip_index[i] == null)
			{
				d3DClassActiveSkillStatus.animation_clip_index[i] = null;
			}
			else
			{
				d3DClassActiveSkillStatus.animation_clip_index[i] = new D3DInt(animation_clip_index[i].value);
			}
		}
		for (int j = 0; j < 3; j++)
		{
			if (clip_frames[j] == null)
			{
				d3DClassActiveSkillStatus.clip_frames[j] = null;
				continue;
			}
			d3DClassActiveSkillStatus.clip_frames[j] = new List<List<int>>();
			foreach (List<int> item in clip_frames[j])
			{
				List<int> list = new List<int>();
				list.AddRange(item);
				d3DClassActiveSkillStatus.clip_frames[j].Add(list);
			}
		}
		d3DClassActiveSkillStatus.cast_time = cast_time;
		d3DClassActiveSkillStatus.unlock_level = unlock_level;
		d3DClassActiveSkillStatus.upgrade_difference = new List<int>(upgrade_difference);
		d3DClassActiveSkillStatus.upgrade_cost = new List<int>(upgrade_cost);
		d3DClassActiveSkillStatus.upgrade_crystal = new List<int>(upgrade_crystal);
		return d3DClassActiveSkillStatus;
	}

	public virtual bool Init()
	{
		active_skill = D3DMain.Instance.GetActiveSkill(skill_id);
		if (active_skill != null)
		{
			max_level = active_skill.max_level;
			return true;
		}
		return false;
	}
}
