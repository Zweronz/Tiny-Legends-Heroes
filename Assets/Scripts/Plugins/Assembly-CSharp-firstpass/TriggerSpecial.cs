using System.Collections.Generic;

public class TriggerSpecial
{
	public class Revive
	{
		public D3DTextFloat recover_hp;

		public D3DTextFloat recover_mp;

		public float RecoverHP(int skill_level)
		{
			if (recover_hp == null || recover_hp.values.Count == 0)
			{
				return 0f;
			}
			int index = ((skill_level <= recover_hp.values.Count - 1) ? skill_level : (recover_hp.values.Count - 1));
			return recover_hp.values[index];
		}

		public float RecoverMP(int skill_level)
		{
			if (recover_mp == null || recover_mp.values.Count == 0)
			{
				return 0f;
			}
			int index = ((skill_level <= recover_mp.values.Count - 1) ? skill_level : (recover_mp.values.Count - 1));
			return recover_mp.values[index];
		}
	}

	public class Dispel
	{
		public D3DTextInt dispel_count;

		public int DispelCount(int skill_level)
		{
			if (dispel_count == null || dispel_count.values.Count == 0)
			{
				return 0;
			}
			int index = ((skill_level <= dispel_count.values.Count - 1) ? skill_level : (dispel_count.values.Count - 1));
			return dispel_count.values[index];
		}
	}

	public class Summon
	{
		public D3DTextString summon_id;

		public D3DTextInt summon_level;

		public D3DTextInt summon_count;

		public D3DTextFloat summon_life;

		public List<string> summon_effect;

		public string SummonID(int skill_level)
		{
			if (summon_id == null || summon_id.values.Count == 0)
			{
				return string.Empty;
			}
			int index = ((skill_level <= summon_id.values.Count - 1) ? skill_level : (summon_id.values.Count - 1));
			return summon_id.values[index];
		}

		public int SummonLevel(int skill_level)
		{
			if (summon_level == null || summon_level.values.Count == 0)
			{
				return 1;
			}
			int index = ((skill_level <= summon_level.values.Count - 1) ? skill_level : (summon_level.values.Count - 1));
			return summon_level.values[index];
		}

		public int SummonCount(int skill_level)
		{
			if (summon_count == null || summon_count.values.Count == 0)
			{
				return 0;
			}
			int index = ((skill_level <= summon_count.values.Count - 1) ? skill_level : (summon_count.values.Count - 1));
			return summon_count.values[index];
		}

		public float SummonLife(int skill_level)
		{
			if (summon_life == null || summon_life.values.Count == 0)
			{
				return -1f;
			}
			int index = ((skill_level <= summon_life.values.Count - 1) ? skill_level : (summon_life.values.Count - 1));
			return summon_life.values[index];
		}

		public string SummonEffect(int skill_level)
		{
			if (summon_effect == null || summon_effect.Count == 0)
			{
				return string.Empty;
			}
			int index = ((skill_level <= summon_effect.Count - 1) ? skill_level : (summon_effect.Count - 1));
			return summon_effect[index];
		}
	}

	public D3DTextInt taunt;

	public Revive revive;

	public Dispel dispel;

	public Summon summon;

	public int GetTaunt(int level)
	{
		if (taunt == null || taunt.values.Count == 0)
		{
			return -1;
		}
		int index = ((level <= taunt.values.Count - 1) ? level : (taunt.values.Count - 1));
		return taunt.values[index];
	}
}
