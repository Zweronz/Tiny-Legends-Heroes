using System.Collections.Generic;

public class D3DClass
{
	public enum ClassType
	{
		STR_MAIN = 0,
		AGI_MAIN = 1,
		INT_MAIN = 2
	}

	public enum ClassAnimations
	{
		BASIC_ANIMATION = 0,
		TWO_HAND_ANIMATION = 1,
		DUAL_ANIMATION = 2
	}

	public const string CSK = "]$XUQ(OVTmM*gO,M";

	public string class_id;

	public string class_name;

	public ClassType class_main_type;

	public ClassType class_sub_type;

	public bool editable;

	public string default_weapon;

	public string default_armor;

	public bool sp_class;

	public string[] class_animations;

	public int player_hatred_send;

	public int player_hatred_resist;

	public int enemy_hatred_send;

	public int enemy_hatred_resist;

	public int apply_hatred_send;

	public int apply_hatred_resist;

	public D3DPuppetTalent class_talent;

	public string[] basic_skill_id;

	public List<List<int>>[] basic_attack1_frames;

	public List<List<int>>[] basic_attack2_frames;

	public Dictionary<string, D3DClassActiveSkillStatus> active_skill_id_list;

	public Dictionary<string, D3DClassPassiveSkillStatus> passive_skill_id_list;

	public D3DClass()
	{
		class_id = string.Empty;
		class_name = string.Empty;
		class_main_type = ClassType.STR_MAIN;
		class_sub_type = ClassType.INT_MAIN;
		editable = true;
		default_weapon = string.Empty;
		default_armor = string.Empty;
		sp_class = true;
		class_animations = new string[3]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};
		player_hatred_send = 0;
		player_hatred_resist = 0;
		enemy_hatred_send = 0;
		enemy_hatred_resist = 0;
		apply_hatred_send = 0;
		apply_hatred_resist = 0;
		class_talent = new D3DPuppetTalent();
		basic_skill_id = new string[3]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};
		basic_attack1_frames = new List<List<int>>[3];
		basic_attack2_frames = new List<List<int>>[3];
		active_skill_id_list = new Dictionary<string, D3DClassActiveSkillStatus>();
		passive_skill_id_list = new Dictionary<string, D3DClassPassiveSkillStatus>();
	}

	~D3DClass()
	{
	}

	public D3DClass Clone()
	{
		D3DClass d3DClass = new D3DClass();
		d3DClass.class_id = class_id;
		d3DClass.class_name = class_name;
		d3DClass.class_main_type = class_main_type;
		d3DClass.class_sub_type = class_sub_type;
		d3DClass.editable = editable;
		d3DClass.default_weapon = default_weapon;
		d3DClass.default_armor = default_armor;
		d3DClass.sp_class = sp_class;
		d3DClass.class_animations = class_animations.Clone() as string[];
		d3DClass.player_hatred_send = player_hatred_send;
		d3DClass.player_hatred_resist = player_hatred_resist;
		d3DClass.enemy_hatred_send = enemy_hatred_send;
		d3DClass.enemy_hatred_resist = enemy_hatred_resist;
		d3DClass.apply_hatred_send = apply_hatred_send;
		d3DClass.apply_hatred_resist = apply_hatred_resist;
		d3DClass.class_talent = class_talent.Clone();
		d3DClass.basic_skill_id = basic_skill_id.Clone() as string[];
		for (int i = 0; i < 3; i++)
		{
			if (basic_attack1_frames[i] == null)
			{
				d3DClass.basic_attack1_frames[i] = null;
				continue;
			}
			d3DClass.basic_attack1_frames[i] = new List<List<int>>();
			foreach (List<int> item in basic_attack1_frames[i])
			{
				if (item == null)
				{
					d3DClass.basic_attack1_frames[i].Add(null);
					continue;
				}
				List<int> list = new List<int>();
				list.AddRange(item);
				d3DClass.basic_attack1_frames[i].Add(list);
			}
		}
		for (int j = 0; j < 3; j++)
		{
			if (basic_attack2_frames[j] == null)
			{
				d3DClass.basic_attack2_frames[j] = null;
				continue;
			}
			d3DClass.basic_attack2_frames[j] = new List<List<int>>();
			foreach (List<int> item2 in basic_attack2_frames[j])
			{
				if (item2 == null)
				{
					d3DClass.basic_attack2_frames[j].Add(null);
					continue;
				}
				List<int> list2 = new List<int>();
				list2.AddRange(item2);
				d3DClass.basic_attack2_frames[j].Add(list2);
			}
		}
		d3DClass.active_skill_id_list.Clear();
		foreach (string key in active_skill_id_list.Keys)
		{
			d3DClass.active_skill_id_list.Add(key, active_skill_id_list[key].Clone());
		}
		d3DClass.passive_skill_id_list.Clear();
		foreach (string key2 in passive_skill_id_list.Keys)
		{
			d3DClass.passive_skill_id_list.Add(key2, passive_skill_id_list[key2].Clone());
		}
		return d3DClass;
	}

	public void UniteClassFromProfile(D3DPuppetProfile profile)
	{
		if (string.Empty != profile.profile_default_weapon)
		{
			default_weapon = profile.profile_default_weapon;
		}
		if (string.Empty != profile.profile_default_armor)
		{
			default_armor = profile.profile_default_armor;
		}
		if (string.Empty != profile.custom_class_name)
		{
			class_name = profile.custom_class_name;
		}
		for (int i = 0; i < 3; i++)
		{
			if (string.Empty != profile.profile_animations[i])
			{
				class_animations[i] = profile.profile_animations[i];
			}
			if (string.Empty != profile.profile_basic_skill_id[i])
			{
				basic_skill_id[i] = profile.profile_basic_skill_id[i];
			}
			if (profile.basic_attack1_frames[i] != null)
			{
				basic_attack1_frames[i] = profile.basic_attack1_frames[i];
			}
			if (profile.basic_attack2_frames[i] != null)
			{
				basic_attack2_frames[i] = profile.basic_attack2_frames[i];
			}
		}
		foreach (string key in profile.profile_active_skill_id_list.Keys)
		{
			if (active_skill_id_list.ContainsKey(key))
			{
				active_skill_id_list[key] = profile.profile_active_skill_id_list[key].Clone();
			}
			else
			{
				active_skill_id_list.Add(key, profile.profile_active_skill_id_list[key].Clone());
			}
		}
		foreach (string key2 in profile.profile_passive_skill_id_list.Keys)
		{
			if (passive_skill_id_list.ContainsKey(key2))
			{
				passive_skill_id_list[key2] = profile.profile_passive_skill_id_list[key2].Clone();
			}
			else
			{
				passive_skill_id_list.Add(key2, profile.profile_passive_skill_id_list[key2].Clone());
			}
		}
	}

	public void CheckSkillInerrability()
	{
		List<string> list = new List<string>();
		foreach (string key in active_skill_id_list.Keys)
		{
			if (!active_skill_id_list[key].Init())
			{
				list.Add(key);
			}
		}
		foreach (string item in list)
		{
			active_skill_id_list.Remove(item);
		}
		list.Clear();
		foreach (string key2 in passive_skill_id_list.Keys)
		{
			if (!passive_skill_id_list[key2].Init())
			{
				list.Add(key2);
			}
		}
		foreach (string item2 in list)
		{
			passive_skill_id_list.Remove(item2);
		}
	}
}
