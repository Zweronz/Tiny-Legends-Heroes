using System.Collections.Generic;

public class D3DTutorialTeam
{
	private static D3DTutorialTeam _instance;

	private List<D3DGamer.D3DPuppetSaveData> _playerBattleTeamData = new List<D3DGamer.D3DPuppetSaveData>();

	private readonly int _nPlayerLevel = 40;

	private readonly int _nSkillLevel = 5;

	private readonly string[] _puppetIds = new string[3] { "paladin_human_002", "barbarian_orcs_002", "firemage_elves" };

	private string[] _paladinEquipsId = new string[6]
	{
		"hammer_002_003",
		"sheild_002_004",
		"armor_plate_006_001",
		"helm_light_019_001",
		string.Empty,
		string.Empty
	};

	private string[] _barbarianEquipsId = new string[6]
	{
		"axe_002_003",
		"axe_002_003",
		"armor_leather_001_014",
		"helm_light_017_001",
		string.Empty,
		string.Empty
	};

	private string[] _maigcEquipsId = new string[6]
	{
		"staff_008_001",
		string.Empty,
		"armor_robe_007_001",
		"helm_light_015_001",
		string.Empty,
		string.Empty
	};

	private List<string[]> _equipsId = new List<string[]>();

	private string[] _fighterSkills = new string[2] { "poxiezhan", "yongqiguanghuan" };

	private string[] _barbarianSkills = new string[2] { "shixie", "xuanfengzhan" };

	private string[] _maigcSkills = new string[2] { "huoqiu", "lianshehuoqiu" };

	private string[] _passiveSKill = new string[1] { "shuangchi" };

	private List<string[]> _skillsId = new List<string[]>();

	public static D3DTutorialTeam Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new D3DTutorialTeam();
				_instance.CreatTeamData();
			}
			return _instance;
		}
	}

	public List<D3DGamer.D3DPuppetSaveData> PlayerDatas
	{
		get
		{
			return _playerBattleTeamData;
		}
	}

	private void BuildEquips()
	{
		_equipsId.Add(_paladinEquipsId);
		_equipsId.Add(_barbarianEquipsId);
		_equipsId.Add(_maigcEquipsId);
	}

	private void BuildSkills()
	{
		_skillsId.Add(_fighterSkills);
		_skillsId.Add(_barbarianSkills);
		_skillsId.Add(_maigcSkills);
	}

	private void BuildConfigData()
	{
		BuildSkills();
		BuildEquips();
	}

	private void CreatTeamData()
	{
		BuildConfigData();
		for (int i = 0; i < _puppetIds.Length; i++)
		{
			if (!(string.Empty == D3DTavern.Instance.DefaultHeros[i]))
			{
				D3DGamer.D3DPuppetSaveData saveData = new D3DGamer.D3DPuppetSaveData();
				saveData.pupet_profile_id = _puppetIds[i];
				saveData.puppet_level = _nPlayerLevel;
				TryEquipmemnts(ref saveData, i);
				TrySkills(ref saveData, i);
				_playerBattleTeamData.Add(saveData);
			}
		}
		D3DGamer.Instance.VaildSkillSlot = 4;
	}

	private void TryEquipmemnts(ref D3DGamer.D3DPuppetSaveData saveData, int nIndex)
	{
		for (int i = 0; i < _equipsId[nIndex].Length; i++)
		{
			string text = _equipsId[nIndex][i];
			if (!(text == string.Empty))
			{
				saveData.puppet_equipments[i] = new D3DGamer.D3DEquipmentSaveData();
				saveData.puppet_equipments[i].equipment_id = text;
			}
		}
	}

	private void TrySkills(ref D3DGamer.D3DPuppetSaveData saveData, int nIndex)
	{
		saveData.active_skill_slots = new List<string>();
		saveData.active_skill_levels = new List<int>();
		saveData.active_skills = new List<string>();
		saveData.passive_skill_slots = new List<string>();
		saveData.passive_skill_levels = new List<int>();
		saveData.passive_skills = new List<string>();
		string[] array = _skillsId[nIndex];
		foreach (string item in array)
		{
			saveData.active_skill_slots.Add(item);
			saveData.active_skill_levels.Add(_nSkillLevel);
			saveData.active_skills.Add(item);
		}
		string[] passiveSKill = _passiveSKill;
		foreach (string item2 in passiveSKill)
		{
			saveData.passive_skill_slots.Add(item2);
			saveData.passive_skill_levels.Add(0);
			saveData.passive_skills.Add(item2);
		}
	}
}
