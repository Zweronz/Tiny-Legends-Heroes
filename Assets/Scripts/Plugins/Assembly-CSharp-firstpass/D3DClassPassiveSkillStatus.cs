using System.Collections.Generic;

public class D3DClassPassiveSkillStatus : D3DClassSkillStatus
{
	public D3DPassiveSkill passive_skill;

	public bool _bDeprecated;

	public D3DClassPassiveSkillStatus Clone()
	{
		D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = new D3DClassPassiveSkillStatus();
		d3DClassPassiveSkillStatus.skill_id = skill_id;
		d3DClassPassiveSkillStatus.unlock_level = unlock_level;
		d3DClassPassiveSkillStatus.upgrade_difference = new List<int>(upgrade_difference);
		d3DClassPassiveSkillStatus.upgrade_cost = new List<int>(upgrade_cost);
		d3DClassPassiveSkillStatus.upgrade_crystal = new List<int>(upgrade_crystal);
		d3DClassPassiveSkillStatus._bDeprecated = _bDeprecated;
		return d3DClassPassiveSkillStatus;
	}

	public bool Init()
	{
		passive_skill = D3DMain.Instance.GetPassiveSkill(skill_id);
		if (passive_skill != null)
		{
			max_level = passive_skill.max_level;
			return true;
		}
		return false;
	}
}
