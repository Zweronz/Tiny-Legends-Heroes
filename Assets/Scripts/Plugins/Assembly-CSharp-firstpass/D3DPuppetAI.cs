using System.Collections.Generic;

public class D3DPuppetAI
{
	public class AutoRevive
	{
		public int revive_count;

		public float revive_delay;

		public List<string> friend_conditions;
	}

	public class SkillBehaviour
	{
		public string trigger_skill;

		public List<string> remove_skills_from_list;

		public Dictionary<string, int> add_skills_to_list;
	}

	public class DisruptHatred
	{
		public float cycle_time;

		public float keep_time;

		public string tirgger_skill;
	}

	public const string ASK = "NX9HR~]AJtVh,nGl";

	public string puppet_id;

	public float entrance_skill_cd;

	public float chain_skill_cd_min;

	public float chain_skill_cd_max;

	public bool add_all_skills;

	public Dictionary<string, int> battle_skill_list;

	public string on_entrance_skill;

	public SortedDictionary<float, string> on_hp_decreased_skill;

	public string on_dead_skill;

	public AutoRevive auto_revive;

	public Dictionary<int, SkillBehaviour> on_friend_count_changed;

	public Dictionary<int, SkillBehaviour> on_enemy_count_changed;

	public Dictionary<int, string> on_summoned_count_changed;

	public DisruptHatred discrupt_hatred;

	public Dictionary<float, SkillBehaviour> survival_time_out;

	public Dictionary<float, string> loop_clock;
}
