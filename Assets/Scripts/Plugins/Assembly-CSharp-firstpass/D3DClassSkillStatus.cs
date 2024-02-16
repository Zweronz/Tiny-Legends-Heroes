using System.Collections.Generic;

public class D3DClassSkillStatus
{
	public string skill_id;

	public int unlock_level;

	public List<int> upgrade_difference;

	public List<int> upgrade_cost;

	public List<int> upgrade_crystal;

	public int skill_level;

	protected int max_level;

	public int MaxLevel
	{
		get
		{
			return max_level;
		}
		set
		{
			max_level = value;
		}
	}

	public bool SkillMax
	{
		get
		{
			return skill_level >= max_level - 1;
		}
	}

	public int UpgradeRequireLevel
	{
		get
		{
			int num = unlock_level;
			if (num < 0)
			{
				num = 1;
			}
			int num2 = skill_level + 1;
			if (num2 > max_level - 1)
			{
				num2 = max_level - 1;
			}
			for (int i = 0; i < num2; i++)
			{
				int index = ((i <= upgrade_difference.Count - 1) ? i : (upgrade_difference.Count - 1));
				num += upgrade_difference[index];
			}
			return num;
		}
	}

	public int UpgradeCost
	{
		get
		{
			if (upgrade_cost.Count == 0)
			{
				return 0;
			}
			int num = skill_level + 1;
			if (num > max_level - 1)
			{
				num = max_level - 1;
			}
			num = ((num <= upgrade_cost.Count - 1) ? num : (upgrade_cost.Count - 1));
			return upgrade_cost[num];
		}
	}

	public int UpgradeCrystal
	{
		get
		{
			if (upgrade_crystal.Count == 0)
			{
				return 0;
			}
			int num = skill_level + 1;
			if (num > max_level - 1)
			{
				num = max_level - 1;
			}
			num = ((num <= upgrade_crystal.Count - 1) ? num : (upgrade_crystal.Count - 1));
			return upgrade_crystal[num];
		}
	}

	public D3DClassSkillStatus()
	{
		skill_id = string.Empty;
		unlock_level = 0;
		upgrade_difference = new List<int>();
		upgrade_cost = new List<int>();
		upgrade_crystal = new List<int>();
		skill_level = -1;
		max_level = 1;
	}

	public void ResetSkillLevel()
	{
		if (unlock_level < 0)
		{
			skill_level = 0;
		}
		else
		{
			skill_level = -1;
		}
	}

	public void FillSkillLevel(int puppet_level)
	{
		int num = unlock_level;
		if (num < 0)
		{
			num = 1;
		}
		if (puppet_level < num)
		{
			ResetSkillLevel();
			return;
		}
		skill_level = 0;
		do
		{
			int num2 = skill_level;
			num2 = ((num2 <= upgrade_difference.Count - 1) ? num2 : (upgrade_difference.Count - 1));
			int num3 = ((num2 >= 0) ? upgrade_difference[num2] : 0);
			num += num3;
			if (num > puppet_level)
			{
				return;
			}
			skill_level++;
		}
		while (skill_level <= max_level - 1);
		skill_level = max_level - 1;
	}

	public void SetSkillLevel(int puppet_level, int skill_lv)
	{
		int num = unlock_level;
		if (num < 0)
		{
			num = 1;
		}
		if (puppet_level < num)
		{
			ResetSkillLevel();
		}
		else
		{
			skill_level = 0;
			while (true)
			{
				int num2 = skill_level;
				num2 = ((num2 <= upgrade_difference.Count - 1) ? num2 : (upgrade_difference.Count - 1));
				int num3 = ((num2 >= 0) ? upgrade_difference[num2] : 0);
				num += num3;
				if (num > puppet_level)
				{
					break;
				}
				skill_level++;
				if (skill_level > max_level - 1)
				{
					skill_level = max_level - 1;
					break;
				}
			}
		}
		if (skill_lv > skill_level || skill_lv < 0)
		{
			ResetSkillLevel();
		}
		else
		{
			skill_level = skill_lv;
		}
	}
}
