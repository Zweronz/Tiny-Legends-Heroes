using System.Collections.Generic;

public class D3DPuppetProfile
{
	public enum ProfileType
	{
		AVATAR = 0,
		SPECIAL = 1
	}

	public enum PuppetAbility
	{
		STR = 0,
		AGI = 1,
		SPI = 2,
		STA = 3,
		INT = 4,
		HP = 5,
		MP = 6,
		ARMOR = 7,
		PHY_DMG = 8,
		MAG_DMG = 9,
		AKT_SPD = 10,
		MOVE_SPD = 11
	}

	public enum PuppetArms
	{
		RIGHT_HAND = 0,
		LEFT_HAND = 1,
		ARMOR = 2,
		HELM = 3,
		BELT = 4,
		BOOTS = 5,
		WRIST = 6,
		NECKLANCE = 7,
		RING1 = 8,
		RING2 = 9
	}

	public const string PSK = "pMBAA#-HNb@#hKH(";

	public string profile_id;

	public string profile_name;

	public ProfileType profile_type;

	public string profile_class;

	public string feature_model;

	public string feature_skin;

	public List<string> feature_textures;

	public List<string> other_textures;

	public List<string[]> puppet_effects;

	public List<D3DPuppetTalent> profile_talent;

	public float[] percent_bonus;

	public int[] fixed_bonus;

	public List<D3DGamer.D3DEquipmentSaveData[]> profile_arms;

	public float custom_scale;

	public string profile_default_weapon;

	public string profile_default_armor;

	public string custom_class_name;

	public string[] profile_animations;

	public string[] profile_basic_skill_id;

	public List<List<int>>[] basic_attack1_frames;

	public List<List<int>>[] basic_attack2_frames;

	public Dictionary<string, D3DClassActiveSkillStatus> profile_active_skill_id_list;

	public Dictionary<string, D3DClassPassiveSkillStatus> profile_passive_skill_id_list;

	public int current_power;

	public D3DPuppetProfile()
	{
		profile_id = string.Empty;
		profile_name = string.Empty;
		profile_type = ProfileType.AVATAR;
		profile_class = string.Empty;
		feature_model = string.Empty;
		feature_skin = string.Empty;
		feature_textures = new List<string>();
		other_textures = new List<string>();
		puppet_effects = new List<string[]>();
		profile_talent = new List<D3DPuppetTalent>();
		profile_arms = new List<D3DGamer.D3DEquipmentSaveData[]>();
		for (int i = 0; i < 9; i++)
		{
			D3DPuppetTalent item = new D3DPuppetTalent();
			profile_talent.Add(item);
			D3DGamer.D3DEquipmentSaveData[] item2 = new D3DGamer.D3DEquipmentSaveData[10];
			profile_arms.Add(item2);
		}
		percent_bonus = new float[2] { 1f, 1f };
		fixed_bonus = new int[2];
		custom_scale = 1f;
		profile_default_weapon = string.Empty;
		profile_default_armor = string.Empty;
		custom_class_name = string.Empty;
		profile_animations = new string[3]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};
		profile_basic_skill_id = new string[3]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};
		basic_attack1_frames = new List<List<int>>[3];
		basic_attack2_frames = new List<List<int>>[3];
		profile_active_skill_id_list = new Dictionary<string, D3DClassActiveSkillStatus>();
		profile_passive_skill_id_list = new Dictionary<string, D3DClassPassiveSkillStatus>();
		current_power = 0;
	}

	~D3DPuppetProfile()
	{
	}

	public D3DPuppetProfile Clone()
	{
		D3DPuppetProfile d3DPuppetProfile = new D3DPuppetProfile();
		d3DPuppetProfile.profile_id = profile_id;
		d3DPuppetProfile.profile_name = profile_name;
		d3DPuppetProfile.profile_type = profile_type;
		d3DPuppetProfile.profile_class = profile_class;
		d3DPuppetProfile.feature_model = feature_model;
		d3DPuppetProfile.feature_skin = feature_skin;
		d3DPuppetProfile.feature_textures = new List<string>(feature_textures);
		d3DPuppetProfile.other_textures = new List<string>(other_textures);
		d3DPuppetProfile.puppet_effects = new List<string[]>(puppet_effects);
		for (int i = 0; i < profile_talent.Count; i++)
		{
			d3DPuppetProfile.profile_talent[i] = profile_talent[i].Clone();
			d3DPuppetProfile.profile_arms[i] = new D3DGamer.D3DEquipmentSaveData[profile_arms[i].Length];
			for (int j = 0; j < profile_arms[i].Length; j++)
			{
				if (profile_arms[i][j] == null)
				{
					d3DPuppetProfile.profile_arms[i][j] = null;
				}
				else
				{
					d3DPuppetProfile.profile_arms[i][j] = profile_arms[i][j].Clone();
				}
			}
		}
		d3DPuppetProfile.custom_scale = custom_scale;
		d3DPuppetProfile.profile_default_weapon = profile_default_weapon;
		d3DPuppetProfile.profile_default_armor = profile_default_armor;
		d3DPuppetProfile.custom_class_name = custom_class_name;
		d3DPuppetProfile.profile_animations = profile_animations.Clone() as string[];
		d3DPuppetProfile.profile_basic_skill_id = profile_basic_skill_id.Clone() as string[];
		d3DPuppetProfile.percent_bonus = percent_bonus.Clone() as float[];
		d3DPuppetProfile.fixed_bonus = fixed_bonus.Clone() as int[];
		for (int k = 0; k < 3; k++)
		{
			if (basic_attack1_frames[k] == null)
			{
				d3DPuppetProfile.basic_attack1_frames[k] = null;
				continue;
			}
			d3DPuppetProfile.basic_attack1_frames[k] = new List<List<int>>();
			foreach (List<int> item in basic_attack1_frames[k])
			{
				if (item == null)
				{
					d3DPuppetProfile.basic_attack1_frames[k].Add(null);
					continue;
				}
				List<int> list = new List<int>();
				list.AddRange(item);
				d3DPuppetProfile.basic_attack1_frames[k].Add(list);
			}
		}
		for (int l = 0; l < 3; l++)
		{
			if (basic_attack2_frames[l] == null)
			{
				d3DPuppetProfile.basic_attack2_frames[l] = null;
				continue;
			}
			d3DPuppetProfile.basic_attack2_frames[l] = new List<List<int>>();
			foreach (List<int> item2 in basic_attack2_frames[l])
			{
				if (item2 == null)
				{
					d3DPuppetProfile.basic_attack2_frames[l].Add(null);
					continue;
				}
				List<int> list2 = new List<int>();
				list2.AddRange(item2);
				d3DPuppetProfile.basic_attack2_frames[l].Add(list2);
			}
		}
		d3DPuppetProfile.profile_active_skill_id_list.Clear();
		foreach (string key in profile_active_skill_id_list.Keys)
		{
			d3DPuppetProfile.profile_active_skill_id_list.Add(key, profile_active_skill_id_list[key].Clone());
		}
		d3DPuppetProfile.profile_passive_skill_id_list.Clear();
		foreach (string key2 in profile_passive_skill_id_list.Keys)
		{
			d3DPuppetProfile.profile_passive_skill_id_list.Add(key2, profile_passive_skill_id_list[key2].Clone());
		}
		return d3DPuppetProfile;
	}

	public void SetPower(int power_level)
	{
		if (power_level >= profile_talent.Count)
		{
			current_power = profile_talent.Count - 1;
		}
		else
		{
			current_power = power_level;
		}
	}

	public void CheckArmsID()
	{
		for (int i = 0; i <= 9; i++)
		{
			if (i == 4 || i == 6 || profile_arms[current_power][i] == null)
			{
				profile_arms[current_power][i] = null;
			}
			else if (!D3DMain.Instance.CheckEquipmentID(profile_arms[current_power][i].equipment_id))
			{
				profile_arms[current_power][i] = null;
			}
		}
	}
}
