using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class D3DGamer
{
	public enum IapMenu
	{
		IAP_499 = 0,
		IAP_999 = 1,
		IAP_1999 = 2,
		IAP_4999 = 3,
		IAP_9999 = 4,
		IAP_NEWBIE = 5,
		IAP_5T = 6,
		IAP_30T = 7,
		IAP_100T = 8,
		IAP_VIP = 9,
		IAP_DISCOUNT_ACTIVE = 100,
		IAP_DISCOUNT_INACTIVE7 = 101,
		IAP_DISCOUNT_INACTIVE14 = 102,
		IAP_DISCOUNT_INACTIVE21 = 103
	}

	public enum SK
	{
		DOC = 0,
		ANIME_INDEX = 1,
		CFG = 2,
		DPR = 3,
		OPT = 4,
		PRO = 5,
		STO = 6,
		TEA = 7,
		TUT = 8,
		RST = 9,
		IAPLOG = 10
	}

	public class D3DPuppetSaveData
	{
		public string pupet_profile_id;

		public int puppet_level;

		public int puppet_current_exp;

		public bool battle_puppet;

		public D3DEquipmentSaveData[] puppet_equipments;

		public List<string> active_skills;

		public List<int> active_skill_levels;

		public List<string> active_skill_slots;

		public List<string> passive_skills;

		public List<int> passive_skill_levels;

		public List<string> passive_skill_slots;

		public D3DPuppetSaveData()
		{
			pupet_profile_id = string.Empty;
			puppet_level = 1;
			puppet_current_exp = 0;
			battle_puppet = false;
			puppet_equipments = new D3DEquipmentSaveData[10];
			active_skills = null;
			active_skill_levels = null;
			active_skill_slots = null;
			passive_skills = null;
			passive_skill_levels = null;
			passive_skill_slots = null;
		}
	}

	public class D3DEquipmentSaveData
	{
		public string equipment_id;

		public D3DMagicPowerSaveData magic_power_data;

		public D3DEquipmentSaveData Clone()
		{
			D3DEquipmentSaveData d3DEquipmentSaveData = new D3DEquipmentSaveData();
			d3DEquipmentSaveData.equipment_id = equipment_id;
			if (magic_power_data != null)
			{
				d3DEquipmentSaveData.magic_power_data = magic_power_data.Clone();
			}
			else
			{
				d3DEquipmentSaveData.magic_power_data = null;
			}
			return d3DEquipmentSaveData;
		}
	}

	private class D3DIAPLogData
	{
		public string strIAP_id;

		public int nCurMaxFloorExplored;

		public int nCurCaptainLevel;
	}

	private class BattleLogData
	{
		private int nBattleLevel;

		private string strEnemyGroupID;

		private bool bResultWin;

		private int nCaptainLevel;

		public int BattleLevel
		{
			get
			{
				return nBattleLevel;
			}
		}

		public string EnemyGroupID
		{
			get
			{
				return strEnemyGroupID;
			}
		}

		public bool ResultWin
		{
			get
			{
				return bResultWin;
			}
			set
			{
				bResultWin = value;
			}
		}

		public int CaptainLevel
		{
			get
			{
				return nCaptainLevel;
			}
		}

		public BattleLogData(int nLevel, string strId, bool bResult, int nCaptainLevel)
		{
			nBattleLevel = nLevel;
			strEnemyGroupID = strId;
			bResultWin = bResult;
			this.nCaptainLevel = nCaptainLevel;
		}
	}

	public enum EUserFisrtCome
	{
		InLogo = 0,
		InMainmenu = 1,
		InLoading = 2,
		Max = 3
	}

	public delegate void OnCurrencyChanged(string strCurrency, string strCrystal);

	public const int PuppetMaxLevel = 40;

	public const int MaxCurrency = 9999999;

	public const int MaxTCrystal = 9999;

	public const int MaxStorePage = 5;

	public const int MaxSkillSlot = 4;

	public const string SaveDataVerison = "1.2";

	private static D3DGamer instance;

	public string[] Sk = new string[11]
	{
		"fB!%EJAFvqHA+aDH", "7fx9M-mXn+rOHIew", "3x,2Z]~vBV6i8GU3", "=(mn1jSq).Io#4=X", "[P2!GdMhG6W]*ZDe", "T~4G+az([r$!5sNL", "lekMUeeDb+jaZwsC", "9[mY*6lh9)XBNjcW", "4*sEGyE*o.5gMYm)", "6XYgF62T,xla@sTE",
		"iL#+*w0kLYEmHZ@d"
	};

	private BattleLogData _LastBattleLogData;

	private List<D3DIAPLogData> _IAPDatas = new List<D3DIAPLogData>();

	private List<BattleLogData> _FlurryBattleData = new List<BattleLogData>();

	private string[] strBattleDatasFilter = new string[6] { "kuloufighter001_jingyanduilie", "kulouzhanshi01duilie_dineng", "kulougongjian01duilie_dineng", "kuloufashi01duilie_dineng", "kulouboss01duilie_dineng", "kulouzhanshi02duilie_dineng" };

	private bool[] bUserFirstCom = new bool[3];

	public List<D3DPuppetSaveData> PlayerBattleTeamData = new List<D3DPuppetSaveData>();

	public List<D3DPuppetSaveData> PlayerTeamData = new List<D3DPuppetSaveData>();

	public Dictionary<string, bool> ShopRefreshStatus = new Dictionary<string, bool>();

	public List<string> TavernPuppet = new List<string>();

	public string D3DCurrencyStr = string.Empty;

	public string TCrystalStr = string.Empty;

	private string[] PropertySk = new string[2] { ".2(~L[xib^", "5p^XkPx@j^" };

	public int D3DCurrency;

	public int TCrystal;

	public float ExpBonus;

	public float GoldBonus;

	public int ValidStorePage = 2;

	public int CurrentStorePage;

	public List<D3DEquipmentSaveData> PlayerStore = new List<D3DEquipmentSaveData>();

	public int VaildSkillSlot = 3;

	public float GamePlayedTime;

	public string Claim = string.Empty;

	public List<bool> TutorialState = new List<bool>();

	private string deceit_str = string.Empty;

	public List<int> NewGearSlotHint = new List<int>();

	public List<string> NewHeroHint = new List<string>();

	public Dictionary<string, List<string>> NewSkillHint = new Dictionary<string, List<string>>();

	public Dictionary<string, List<string>> CurrentUnlockedSkills = new Dictionary<string, List<string>>();

	private readonly string _strSaveFileName = "jdyqU6ahWocyDbc.tlh";

	private float _fSavedFileVersion;

	public static D3DGamer Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DGamer();
			}
			return instance;
		}
	}

	public int TeamMaxLevel
	{
		get
		{
			int num = 0;
			foreach (D3DPuppetSaveData playerTeamDatum in PlayerTeamData)
			{
				if (playerTeamDatum.puppet_level > num)
				{
					num = playerTeamDatum.puppet_level;
				}
			}
			return num;
		}
	}

	public int BattleTeamMaxLevel
	{
		get
		{
			int num = 0;
			foreach (D3DPuppetSaveData playerBattleTeamDatum in PlayerBattleTeamData)
			{
				if (playerBattleTeamDatum.puppet_level > num)
				{
					num = playerBattleTeamDatum.puppet_level;
				}
			}
			return num;
		}
	}

	private string SaveFileName
	{
		get
		{
			return _strSaveFileName;
		}
	}

	public float SaveFileVersion
	{
		get
		{
			return _fSavedFileVersion;
		}
	}

	public string CurrencyText
	{
		get
		{
			return XXTEAUtils.Decrypt(D3DCurrencyStr, PropertySk[1]);
		}
	}

	public string CrystalText
	{
		get
		{
			return XXTEAUtils.Decrypt(TCrystalStr, PropertySk[0]);
		}
	}

	public event OnCurrencyChanged onCurrencyChangedEvent;

	private bool FilterBattleInfo(int nLevel, string strEnemyGroupId)
	{
		if (nLevel != 1)
		{
			return false;
		}
		string[] array = strBattleDatasFilter;
		foreach (string text in array)
		{
			if (text == strEnemyGroupId)
			{
				return true;
			}
		}
		return false;
	}

	private void AddBattleList(int nLevel, string strEnemyGroupId, bool bWinner)
	{
		if (!FilterBattleInfo(nLevel, strEnemyGroupId))
		{
			return;
		}
		bool flag = false;
		foreach (BattleLogData flurryBattleDatum in _FlurryBattleData)
		{
			if (flurryBattleDatum.EnemyGroupID == strEnemyGroupId)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			BattleLogData item = new BattleLogData(nLevel, strEnemyGroupId, bWinner, PlayerBattleTeamData[0].puppet_level);
			_FlurryBattleData.Add(item);
		}
	}

	public void UpdateLastBattleInfo(int nLevel, string strEnemyGroupId, bool bWinner)
	{
		_LastBattleLogData = new BattleLogData(nLevel, strEnemyGroupId, bWinner, PlayerBattleTeamData[0].puppet_level);
		AddBattleList(nLevel, strEnemyGroupId, bWinner);
		SaveIAPLog();
	}

	public void LogIAP(string strIAP_id)
	{
		if (D3DMain.Instance.exploring_dungeon.dungeon != null)
		{
			if (strIAP_id.Contains("new"))
			{
			}
			D3DIAPLogData d3DIAPLogData = new D3DIAPLogData();
			d3DIAPLogData.strIAP_id = strIAP_id;
			d3DIAPLogData.nCurMaxFloorExplored = D3DMain.Instance.exploring_dungeon.dungeon.explored_level;
			d3DIAPLogData.nCurCaptainLevel = ((PlayerBattleTeamData.Count <= 0) ? PlayerTeamData[0].puppet_level : PlayerBattleTeamData[0].puppet_level);
			if (_IAPDatas.Count <= 2)
			{
				_IAPDatas.Add(d3DIAPLogData);
				SaveIAPLog();
			}
		}
	}

	private void LoadIAPLog()
	{
		string name = D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("IAPLog", Sk[10])) + ".tlh";
		string content = string.Empty;
		Utils.FileGetString(name, ref content);
		if (content.Length == 0)
		{
			return;
		}
		content = XXTEAUtils.Decrypt(content, Sk[10]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if (item.Name == "UserCome")
			{
				XmlElement xmlElement = (XmlElement)item;
				string text = xmlElement.GetAttribute("index").Trim();
				int num = int.Parse(xmlElement.GetAttribute("index").Trim());
				if (num < 3)
				{
					bool flag = ((!(xmlElement.GetAttribute("state").Trim() == "0")) ? true : false);
					bUserFirstCom[num] = flag;
				}
				else
				{
					Debug.LogWarning("index larger than EUserFisrtCome.Max! failed.");
				}
			}
			else if (item.Name == "IAP")
			{
				XmlElement xmlElement2 = (XmlElement)item;
				D3DIAPLogData d3DIAPLogData = new D3DIAPLogData();
				d3DIAPLogData.strIAP_id = xmlElement2.GetAttribute("id").Trim();
				d3DIAPLogData.nCurMaxFloorExplored = int.Parse(xmlElement2.GetAttribute("maxFloorExplored").Trim());
				d3DIAPLogData.nCurCaptainLevel = int.Parse(xmlElement2.GetAttribute("captainLevel").Trim());
				_IAPDatas.Add(d3DIAPLogData);
			}
			else if (item.Name == "FlurryBattleList")
			{
				XmlElement xmlElement3 = (XmlElement)item;
				int nLevel = int.Parse(xmlElement3.GetAttribute("level").Trim());
				string strId = xmlElement3.GetAttribute("EnemyGroup").Trim();
				bool bResult = ((xmlElement3.GetAttribute("Result").Trim() == "Win") ? true : false);
				int nCaptainLevel = int.Parse(xmlElement3.GetAttribute("CaptainLevel").Trim());
				_FlurryBattleData.Add(new BattleLogData(nLevel, strId, bResult, nCaptainLevel));
			}
			else if (item.Name == "LastBattle")
			{
				XmlElement xmlElement4 = (XmlElement)item;
				int nLevel2 = int.Parse(xmlElement4.GetAttribute("level").Trim());
				string strId2 = xmlElement4.GetAttribute("EnemyGroup").Trim();
				bool bResult2 = ((xmlElement4.GetAttribute("Result").Trim() == "Win") ? true : false);
				int nCaptainLevel2 = int.Parse(xmlElement4.GetAttribute("CaptainLevel").Trim());
				_LastBattleLogData = new BattleLogData(nLevel2, strId2, bResult2, nCaptainLevel2);
			}
		}
	}

	public void UserCome(EUserFisrtCome e)
	{
		if (!bUserFirstCom[(int)e])
		{
			bUserFirstCom[(int)e] = true;
			SaveIAPLog();
		}
	}

	private void SaveIAPLog()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<IAPLog>\n";
		for (int i = 0; i < 3; i++)
		{
			string text2 = text;
			text = text2 + "<UserCome index=\"" + i + "\" state=\"" + (bUserFirstCom[i] ? 1 : 0) + "\"    />\n";
		}
		foreach (D3DIAPLogData iAPData in _IAPDatas)
		{
			string text2 = text;
			text = text2 + "<IAP id=\"" + iAPData.strIAP_id + "\"   maxFloorExplored=\"" + iAPData.nCurMaxFloorExplored + "\"    captainLevel=\"" + iAPData.nCurCaptainLevel + "\"    />\n";
		}
		foreach (BattleLogData flurryBattleDatum in _FlurryBattleData)
		{
			string text2 = text;
			text = text2 + "<FlurryBattleList level=\"" + flurryBattleDatum.BattleLevel + "\" EnemyGroup=\"" + flurryBattleDatum.EnemyGroupID.ToString() + "\" CaptainLevel=\"" + flurryBattleDatum.CaptainLevel + "\" Result=\"" + ((!flurryBattleDatum.ResultWin) ? "Lose" : "Win") + "\"    />\n";
		}
		if (_LastBattleLogData != null)
		{
			string text2 = text;
			text = text2 + "<LastBattle level=\"" + _LastBattleLogData.BattleLevel + "\" EnemyGroup=\"" + _LastBattleLogData.EnemyGroupID.ToString() + "\" CaptainLevel=\"" + _LastBattleLogData.CaptainLevel + "\" Result=\"" + ((!_LastBattleLogData.ResultWin) ? "Lose" : "Win") + "\"    />\n";
		}
		text += "</IAPLog>";
		text = XXTEAUtils.Encrypt(text, Sk[10]);
		Utils.FileSaveString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("IAPLog", Sk[10])) + ".tlh", text);
	}

	public void CleanShopRefreshStatus()
	{
		ShopRefreshStatus.Clear();
	}

	public void SaveAllData()
	{
		string text = SaveTeamDataToString();
		string text2 = SaveDungeonProgressToString();
		string text3 = SaveStoreDataToString();
		string text4 = SaveProgressDataToString();
		string text5 = SaveRespawnTimeDataToString();
		string empty = string.Empty;
		do
		{
			empty = UnityEngine.Random.Range(1472, 43585468).ToString();
		}
		while (string.Empty != deceit_str && deceit_str.Length == empty.Length);
		deceit_str = empty;
		empty = text + "#" + text2 + "#" + text3 + "#" + text4 + "#" + text5;
		empty = XXTEAUtils.Encrypt(empty, Sk[deceit_str.Length]);
		Utils.FileSaveString(SaveFileName, empty + "#" + XXTEAUtils.Encrypt("trinitiact" + deceit_str, Sk[0]));
		SaveNewHints();
	}

	public void LoadAllDataNew()
	{
		if (!Utils.CheckFileExists(SaveFileName))
		{
			DefaultProgress();
			DefaultTeamData();
			DefaultStore();
			SaveAllData();
		}
		string content = string.Empty;
		Utils.FileGetString(SaveFileName, ref content);
		string[] array = content.Split('#');
		string text = XXTEAUtils.Decrypt(array[1], Sk[0]);
		deceit_str = text.Replace("trinitiact", string.Empty);
		text = XXTEAUtils.Decrypt(array[0], Sk[deceit_str.Length]);
		array = text.Split('#');
		LoadProgressFromString(array[3], false);
		LoadTeamDataFromStr(array[0]);
		LoadDungeonProgressFromString(array[1]);
		LoadStoreDataFromString(array[2]);
		LoadRespawnTimeFromString(array[4]);
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("options", Instance.Sk[4])) + ".tlh"))
		{
			TAudioManager.instance.isMusicOn = true;
			TAudioManager.instance.isSoundOn = true;
			SaveGameOptions();
		}
		LoadGameOptions();
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("tutorial", Instance.Sk[8])) + ".tlh"))
		{
			Instance.TutorialState.Clear();
			for (D3DHowTo.TutorialType tutorialType = D3DHowTo.TutorialType.BEGINNER_BATTLE; tutorialType <= D3DHowTo.TutorialType.TYPE_MAX; tutorialType++)
			{
				Instance.TutorialState.Add(false);
			}
			SaveTutorialState();
		}
		LoadTutorialState();
		LoadIAPLog();
		if (Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("newhints", Sk[8])) + ".tlh"))
		{
			LoadNewHints();
		}
		if (_fSavedFileVersion != 1.1f)
		{
			return;
		}
		foreach (string key in D3DMain.Instance.D3DDungeonManager.Keys)
		{
			int explored_level = D3DMain.Instance.D3DDungeonManager[key].explored_level;
			int explored_level2 = (int)((double)explored_level - (double)Mathf.Max((float)explored_level / 3f - 1f, 0f) * 1.15);
			D3DMain.Instance.D3DDungeonManager[key].explored_level = explored_level2;
		}
	}

	public void LoadAllDataOld()
	{
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("progress", Instance.Sk[5])) + ".tlh"))
		{
			Instance.DefaultProgress();
			Instance.SaveProgress();
		}
		Instance.LoadProgress(true);
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("team", Instance.Sk[7])) + ".tlh"))
		{
			Instance.DefaultTeamData();
			Instance.SaveTeamData();
		}
		Instance.LoadTeamData();
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("store", Instance.Sk[6])) + ".tlh"))
		{
			Instance.DefaultStore();
			Instance.SaveStore();
		}
		Instance.LoadStore();
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("options", Instance.Sk[4])) + ".tlh"))
		{
			TAudioManager.instance.isMusicOn = true;
			TAudioManager.instance.isSoundOn = true;
			Instance.SaveGameOptions();
		}
		Instance.LoadGameOptions();
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("dpr", Instance.Sk[3])) + ".tlh"))
		{
			Instance.SaveDungeonProgress();
		}
		Instance.LoadDungeonProgress();
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("tutorial", Instance.Sk[8])) + ".tlh"))
		{
			Instance.TutorialState.Clear();
			for (D3DHowTo.TutorialType tutorialType = D3DHowTo.TutorialType.BEGINNER_BATTLE; tutorialType <= D3DHowTo.TutorialType.FIRST_BOSS_GRAVE; tutorialType++)
			{
				Instance.TutorialState.Add(false);
			}
			Instance.SaveTutorialState();
		}
		Instance.LoadTutorialState();
		Instance.LoadRespawnTime();
	}

	public void SaveTeamData()
	{
	}

	public void LoadTeamData()
	{
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("team", Sk[7])) + ".tlh", ref content);
		LoadTeamDataFromStr(content);
	}

	private string SaveTeamDataToString()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<D3DTeamData version=\"1.2\">\n";
		foreach (D3DPuppetSaveData playerTeamDatum in PlayerTeamData)
		{
			string text2 = text;
			text = text2 + "<PuppetData profile_id=\"" + playerTeamDatum.pupet_profile_id + "\" puppet_level=\"" + playerTeamDatum.puppet_level.ToString() + "\" current_exp=\"" + playerTeamDatum.puppet_current_exp + "\" bp=\"" + playerTeamDatum.battle_puppet + "\">\n";
			string text3 = string.Empty;
			string text4 = string.Empty;
			string text5 = string.Empty;
			for (int i = 0; i < playerTeamDatum.puppet_equipments.Length; i++)
			{
				if (playerTeamDatum.puppet_equipments[i] == null)
				{
					text3 = text3 + string.Empty + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? "," : string.Empty);
					text4 = text4 + string.Empty + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? "," : string.Empty);
					text5 = text5 + string.Empty + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? ";" : string.Empty);
					continue;
				}
				text3 = text3 + playerTeamDatum.puppet_equipments[i].equipment_id + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? "," : string.Empty);
				if (playerTeamDatum.puppet_equipments[i].magic_power_data != null)
				{
					text4 = text4 + playerTeamDatum.puppet_equipments[i].magic_power_data.rule_id + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? "," : string.Empty);
					string text6 = string.Empty;
					foreach (int key in playerTeamDatum.puppet_equipments[i].magic_power_data.power_value.Keys)
					{
						text2 = text6;
						text6 = text2 + key + "," + playerTeamDatum.puppet_equipments[i].magic_power_data.power_value[key] + "!";
					}
					text6 = text6.Substring(0, text6.Length - 1);
					text5 = text5 + text6 + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? ";" : string.Empty);
				}
				else
				{
					text4 = text4 + string.Empty + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? "," : string.Empty);
					text5 = text5 + string.Empty + ((i != playerTeamDatum.puppet_equipments.Length - 1) ? ";" : string.Empty);
				}
			}
			text = text + "<Equipment att=\"" + text3 + "\"/>\n";
			text = text + "<MP att=\"" + text4 + "\"/>\n";
			text = text + "<VA att=\"" + text5 + "\"/>\n";
			string empty = string.Empty;
			if (playerTeamDatum.active_skills != null)
			{
				empty = string.Empty;
				for (int j = 0; j < playerTeamDatum.active_skills.Count; j++)
				{
					empty = empty + playerTeamDatum.active_skills[j] + ((j != playerTeamDatum.active_skills.Count - 1) ? "," : string.Empty);
				}
				if (string.Empty != empty)
				{
					text = text + "<ASK att=\"" + empty + "\"/>\n";
				}
			}
			if (playerTeamDatum.active_skill_levels != null)
			{
				empty = string.Empty;
				for (int k = 0; k < playerTeamDatum.active_skill_levels.Count; k++)
				{
					empty = empty + (playerTeamDatum.active_skill_levels[k] + 1) + ((k != playerTeamDatum.active_skill_levels.Count - 1) ? "," : string.Empty);
				}
				if (string.Empty != empty)
				{
					text = text + "<ASL att=\"" + empty + "\"/>\n";
				}
			}
			if (playerTeamDatum.active_skill_slots != null)
			{
				empty = string.Empty;
				for (int l = 0; l < playerTeamDatum.active_skill_slots.Count; l++)
				{
					empty = empty + playerTeamDatum.active_skill_slots[l] + ((l != playerTeamDatum.active_skill_slots.Count - 1) ? "," : string.Empty);
				}
				if (string.Empty != empty)
				{
					text = text + "<ABS att=\"" + empty + "\"/>\n";
				}
			}
			if (playerTeamDatum.passive_skills != null)
			{
				empty = string.Empty;
				for (int m = 0; m < playerTeamDatum.passive_skills.Count; m++)
				{
					empty = empty + playerTeamDatum.passive_skills[m] + ((m != playerTeamDatum.passive_skills.Count - 1) ? "," : string.Empty);
				}
				if (string.Empty != empty)
				{
					text = text + "<PSK att=\"" + empty + "\"/>\n";
				}
			}
			if (playerTeamDatum.passive_skill_levels != null)
			{
				empty = string.Empty;
				for (int n = 0; n < playerTeamDatum.passive_skill_levels.Count; n++)
				{
					empty = empty + (playerTeamDatum.passive_skill_levels[n] + 1) + ((n != playerTeamDatum.passive_skill_levels.Count - 1) ? "," : string.Empty);
				}
				if (string.Empty != empty)
				{
					text = text + "<PSL att=\"" + empty + "\"/>\n";
				}
			}
			if (playerTeamDatum.passive_skill_slots != null)
			{
				empty = string.Empty;
				for (int num = 0; num < playerTeamDatum.passive_skill_slots.Count; num++)
				{
					empty = empty + playerTeamDatum.passive_skill_slots[num] + ((num != playerTeamDatum.passive_skill_slots.Count - 1) ? "," : string.Empty);
				}
				if (string.Empty != empty)
				{
					text = text + "<PBS att=\"" + empty + "\"/>\n";
				}
			}
			text += "</PuppetData>\n";
		}
		text += "<TVH ah=\"";
		foreach (string item in TavernPuppet)
		{
			text = text + item + ",";
		}
		text += "\"/>\n";
		text += "</D3DTeamData>";
		return XXTEAUtils.Encrypt(text, Sk[7]);
	}

	private void LoadTeamDataFromStr(string save_str)
	{
		PlayerBattleTeamData.Clear();
		PlayerTeamData.Clear();
		TavernPuppet.Clear();
		save_str = XXTEAUtils.Decrypt(save_str, Sk[7]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(save_str);
		XmlNode documentElement = xmlDocument.DocumentElement;
		string s = ((XmlElement)documentElement).GetAttribute("version").Trim();
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			if ("PuppetData" != item.Name)
			{
				if (!("TVH" == item.Name))
				{
					continue;
				}
				string[] array = xmlElement.GetAttribute("ah").Trim().Split(',');
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (string.Empty != text)
					{
						TavernPuppet.Add(text);
					}
				}
				continue;
			}
			D3DPuppetSaveData d3DPuppetSaveData = new D3DPuppetSaveData();
			d3DPuppetSaveData.pupet_profile_id = xmlElement.GetAttribute("profile_id").Trim();
			d3DPuppetSaveData.puppet_level = int.Parse(xmlElement.GetAttribute("puppet_level").Trim());
			d3DPuppetSaveData.puppet_current_exp = int.Parse(xmlElement.GetAttribute("current_exp").Trim());
			d3DPuppetSaveData.battle_puppet = bool.Parse(xmlElement.GetAttribute("bp").Trim());
			int num = 0;
			_fSavedFileVersion = float.Parse(s);
			if (_fSavedFileVersion >= 1f)
			{
				string[] array3 = new string[0];
				string[] array4 = new string[0];
				string[] array5 = new string[0];
				foreach (XmlNode item2 in item)
				{
					string text2 = ((XmlElement)item2).GetAttribute("att").Trim();
					if (string.Empty == text2)
					{
						continue;
					}
					if ("Equipment" == item2.Name)
					{
						array3 = text2.Split(',');
					}
					else if ("MP" == item2.Name)
					{
						array4 = text2.Split(',');
					}
					else if ("VA" == item2.Name)
					{
						array5 = text2.Split(';');
					}
					else if ("ASK" == item2.Name)
					{
						d3DPuppetSaveData.active_skills = new List<string>(text2.Split(','));
					}
					else if ("ASL" == item2.Name)
					{
						string[] array6 = text2.Split(',');
						int[] array7 = new int[array6.Length];
						for (int j = 0; j < array6.Length; j++)
						{
							array7[j] = int.Parse(array6[j]) - 1;
						}
						d3DPuppetSaveData.active_skill_levels = new List<int>(array7);
					}
					else if ("ABS" == item2.Name)
					{
						d3DPuppetSaveData.active_skill_slots = new List<string>(text2.Split(','));
					}
					else if ("PSK" == item2.Name)
					{
						d3DPuppetSaveData.passive_skills = new List<string>(text2.Split(','));
					}
					else if ("PSL" == item2.Name)
					{
						string[] array8 = text2.Split(',');
						int[] array9 = new int[array8.Length];
						for (int k = 0; k < array8.Length; k++)
						{
							array9[k] = int.Parse(array8[k]) - 1;
						}
						d3DPuppetSaveData.passive_skill_levels = new List<int>(array9);
					}
					else if ("PBS" == item2.Name)
					{
						d3DPuppetSaveData.passive_skill_slots = new List<string>(text2.Split(','));
					}
					num++;
				}
				for (int l = 0; l < array3.Length; l++)
				{
					if (string.Empty == array3[l])
					{
						continue;
					}
					d3DPuppetSaveData.puppet_equipments[l] = new D3DEquipmentSaveData();
					d3DPuppetSaveData.puppet_equipments[l].equipment_id = array3[l];
					if (!(string.Empty != array4[l]))
					{
						continue;
					}
					d3DPuppetSaveData.puppet_equipments[l].magic_power_data = new D3DMagicPowerSaveData();
					d3DPuppetSaveData.puppet_equipments[l].magic_power_data.rule_id = array4[l];
					if (string.Empty != array5[l])
					{
						string[] array10 = array5[l].Split('!');
						string[] array11 = array10;
						foreach (string text3 in array11)
						{
							string[] array12 = text3.Split(',');
							d3DPuppetSaveData.puppet_equipments[l].magic_power_data.power_value.Add(int.Parse(array12[0]), float.Parse(array12[1]));
						}
					}
				}
			}
			else
			{
				foreach (XmlNode item3 in item)
				{
					if (!("Equipment" != item3.Name))
					{
						d3DPuppetSaveData.puppet_equipments[num] = new D3DEquipmentSaveData();
						d3DPuppetSaveData.puppet_equipments[num].equipment_id = ((XmlElement)item3).GetAttribute("eid").Trim();
						num++;
					}
				}
			}
			if (d3DPuppetSaveData.battle_puppet)
			{
				if (PlayerBattleTeamData.Count < 3)
				{
					PlayerBattleTeamData.Add(d3DPuppetSaveData);
				}
				else
				{
					d3DPuppetSaveData.battle_puppet = false;
				}
			}
			PlayerTeamData.Add(d3DPuppetSaveData);
		}
		if (PlayerBattleTeamData.Count == 0)
		{
			PlayerBattleTeamData.Add(PlayerTeamData[0]);
			PlayerBattleTeamData[0].battle_puppet = true;
		}
	}

	public void DefaultTeamData()
	{
		PlayerBattleTeamData.Clear();
		PlayerTeamData.Clear();
		for (int i = 0; i < D3DTavern.Instance.DefaultHeros.Length; i++)
		{
			if (string.Empty == D3DTavern.Instance.DefaultHeros[i])
			{
				continue;
			}
			D3DPuppetSaveData d3DPuppetSaveData = new D3DPuppetSaveData();
			d3DPuppetSaveData.pupet_profile_id = D3DTavern.Instance.DefaultHeros[i];
			d3DPuppetSaveData.puppet_level = 1;
			d3DPuppetSaveData.puppet_current_exp = 0;
			d3DPuppetSaveData.battle_puppet = true;
			D3DEquipmentSaveData[] array = D3DMain.Instance.GetProfile(D3DTavern.Instance.DefaultHeros[i]).profile_arms[0];
			d3DPuppetSaveData.puppet_equipments = new D3DEquipmentSaveData[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == null)
				{
					d3DPuppetSaveData.puppet_equipments[j] = null;
				}
				else
				{
					d3DPuppetSaveData.puppet_equipments[j] = array[j].Clone();
				}
			}
			D3DPuppetProfile profileClone = D3DMain.Instance.GetProfileClone(D3DTavern.Instance.DefaultHeros[i]);
			D3DProfileInstance d3DProfileInstance = new D3DProfileInstance();
			profileClone.SetPower(0);
			d3DProfileInstance.InitInstance(profileClone, d3DPuppetSaveData);
			d3DProfileInstance.InitSkillLevel(d3DPuppetSaveData);
			foreach (string key in d3DProfileInstance.puppet_class.active_skill_id_list.Keys)
			{
				if (d3DProfileInstance.puppet_class.active_skill_id_list[key].skill_level >= 0)
				{
					d3DProfileInstance.battle_active_slots = new string[1];
					d3DProfileInstance.battle_active_slots[0] = d3DProfileInstance.puppet_class.active_skill_id_list[key].active_skill.skill_id;
					break;
				}
			}
			d3DPuppetSaveData = d3DProfileInstance.ExtractPuppetSaveData();
			PlayerBattleTeamData.Add(d3DPuppetSaveData);
			PlayerTeamData.Add(d3DPuppetSaveData);
		}
		foreach (HeroHire item in D3DTavern.Instance.HeroHireManager)
		{
			if (string.Empty == item.unlock_group)
			{
				TavernPuppet.Add(item.puppet_id);
			}
		}
	}

	public void SaveStore()
	{
	}

	private string SaveStoreDataToString()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<D3DStore>\n";
		int num = 0;
		foreach (D3DEquipmentSaveData item in PlayerStore)
		{
			if (item != null)
			{
				string text2 = string.Empty;
				string text4;
				if (item.magic_power_data != null)
				{
					text2 = "\" mp=\"" + item.magic_power_data.rule_id;
					string text3 = string.Empty;
					foreach (int key in item.magic_power_data.power_value.Keys)
					{
						text4 = text3;
						text3 = text4 + key + "," + item.magic_power_data.power_value[key] + ";";
					}
					if (string.Empty != text3)
					{
						text2 = text2 + "\" va=\"" + text3;
					}
				}
				text4 = text;
				text = text4 + "<Item slot=\"" + num + "\" iid=\"" + item.equipment_id + text2 + "\"/>\n";
			}
			num++;
		}
		text += "</D3DStore>";
		return XXTEAUtils.Encrypt(text, Sk[6]);
	}

	public void LoadStore()
	{
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("store", Sk[6])) + ".tlh", ref content);
		LoadStoreDataFromString(content);
	}

	private void LoadStoreDataFromString(string save_str)
	{
		PlayerStore.Clear();
		int num = ValidStorePage * 12;
		for (int i = 0; i < num; i++)
		{
			PlayerStore.Add(null);
		}
		save_str = XXTEAUtils.Decrypt(save_str, Sk[6]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(save_str);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if (!("Item" == item.Name))
			{
				continue;
			}
			XmlElement xmlElement = (XmlElement)item;
			int num2 = int.Parse(xmlElement.GetAttribute("slot").Trim());
			if (num2 > PlayerStore.Count - 1)
			{
				continue;
			}
			D3DEquipmentSaveData d3DEquipmentSaveData = new D3DEquipmentSaveData();
			d3DEquipmentSaveData.equipment_id = xmlElement.GetAttribute("iid").Trim();
			string text = xmlElement.GetAttribute("mp").Trim();
			if (string.Empty != text)
			{
				d3DEquipmentSaveData.magic_power_data = new D3DMagicPowerSaveData();
				d3DEquipmentSaveData.magic_power_data.rule_id = text;
				string text2 = xmlElement.GetAttribute("va").Trim();
				string[] array = text2.Split(';');
				string[] array2 = array;
				foreach (string text3 in array2)
				{
					if (!(string.Empty == text3))
					{
						string[] array3 = text3.Split(',');
						d3DEquipmentSaveData.magic_power_data.power_value.Add(int.Parse(array3[0]), float.Parse(array3[1]));
					}
				}
			}
			PlayerStore[num2] = d3DEquipmentSaveData;
		}
	}

	public void DefaultStore()
	{
		PlayerStore.Clear();
		int num = ValidStorePage * 12;
		for (int i = 0; i < num; i++)
		{
			PlayerStore.Add(null);
		}
	}

	public void SaveGameOptions()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<D3DGameOptions>\n";
		string text2 = text;
		text = text2 + "<Audio music=\"" + TAudioManager.instance.isMusicOn + "\" sound=\"" + TAudioManager.instance.isSoundOn + "\"/>\n";
		text += "</D3DGameOptions>";
		Utils.FileSaveString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("options", Sk[4])) + ".tlh", XXTEAUtils.Encrypt(text, Sk[4]));
	}

	public void LoadGameOptions()
	{
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("options", Sk[4])) + ".tlh", ref content);
		content = XXTEAUtils.Decrypt(content, Sk[4]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if ("Audio" == item.Name)
			{
				TAudioManager.instance.isMusicOn = bool.Parse(((XmlElement)item).GetAttribute("music").Trim());
				TAudioManager.instance.isSoundOn = bool.Parse(((XmlElement)item).GetAttribute("sound").Trim());
			}
		}
	}

	public void SaveProgress()
	{
	}

	private string SaveProgressDataToString()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<D3DProgress>\n";
		string text2 = text;
		text = text2 + "<Currency count=\"" + D3DCurrencyStr + "," + TCrystalStr + "\"/>\n";
		text2 = text;
		text = text2 + "<Iap store=\"" + ValidStorePage + "\" skill=\"" + VaildSkillSlot + "\"/>\n";
		text2 = text;
		text = text2 + "<Svs b1=\"" + ExpBonus + "\" b2=\"" + GoldBonus + "\"" + ((!("696C48541164C101" == Claim) && !("94266FCA48" == Claim)) ? string.Empty : (" clm=\"" + Claim + "\"")) + "/>\n";
		text2 = text;
		text = text2 + "<Discount discount=\"" + D3DIapDiscount.Instance.LastPlayDateTick + "," + D3DIapDiscount.Instance.CurrentDiscount + "," + (int)D3DIapDiscount.Instance.IapDiscountStates[0] + "," + (int)D3DIapDiscount.Instance.IapDiscountStates[1] + "," + (int)D3DIapDiscount.Instance.IapDiscountStates[2] + "," + (int)D3DIapDiscount.Instance.IapDiscountStates[3] + "," + D3DIapDiscount.Instance.DiscountCountDownSeconds + "," + D3DIapDiscount.Instance.DiscountCountDownDateTick + "," + GamePlayedTime + "\"/>\n";
		text += "</D3DProgress>";
		return XXTEAUtils.Encrypt(text, Sk[5]);
	}

	public void LoadProgress(bool old = false)
	{
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("progress", Sk[5])) + ".tlh", ref content);
		LoadProgressFromString(content, old);
	}

	public void LoadProgressFromString(string save_str, bool old)
	{
		save_str = XXTEAUtils.Decrypt(save_str, Sk[5]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(save_str);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if ("Iap" == item.Name)
			{
				ValidStorePage = int.Parse(((XmlElement)item).GetAttribute("store").Trim());
				if (ValidStorePage > 5)
				{
					ValidStorePage = 5;
				}
				VaildSkillSlot = int.Parse(((XmlElement)item).GetAttribute("skill").Trim());
				if (VaildSkillSlot > 4)
				{
					VaildSkillSlot = 4;
				}
			}
			else if ("Svs" == item.Name)
			{
				ExpBonus = float.Parse(((XmlElement)item).GetAttribute("b1").Trim());
				if (ExpBonus > 0.2f)
				{
					ExpBonus = 0.2f;
				}
				GoldBonus = float.Parse(((XmlElement)item).GetAttribute("b2").Trim());
				if (GoldBonus > 0.1f)
				{
					GoldBonus = 0.1f;
				}
				Claim = ((XmlElement)item).GetAttribute("clm").Trim();
				if ("696C48541164C101" != Claim && "94266FCA48" != Claim)
				{
					Claim = string.Empty;
				}
			}
			else if ("Currency" == item.Name)
			{
				string[] array = ((XmlElement)item).GetAttribute("count").Trim().Split(',');
				if (old)
				{
					D3DCurrency = int.Parse(array[0]);
					if (D3DCurrency > 9999999)
					{
						D3DCurrency = 9999999;
					}
					if (array.Length < 2)
					{
						TCrystal = 0;
						continue;
					}
					TCrystal = int.Parse(array[1]);
					if (TCrystal > 9999)
					{
						TCrystal = 9999;
					}
					continue;
				}
				D3DCurrencyStr = array[0];
				if (int.Parse(CurrencyText) > 9999999)
				{
					D3DCurrencyStr = XXTEAUtils.Encrypt(9999999.ToString(), PropertySk[1]);
				}
				if (array.Length < 2)
				{
					TCrystalStr = XXTEAUtils.Encrypt("0", PropertySk[0]);
					continue;
				}
				TCrystalStr = array[1];
				if (int.Parse(CrystalText) > 9999)
				{
					TCrystalStr = XXTEAUtils.Encrypt(9999.ToString(), PropertySk[0]);
				}
			}
			else if ("Discount" == item.Name)
			{
				string[] array2 = ((XmlElement)item).GetAttribute("discount").Trim().Split(',');
				D3DIapDiscount.Instance.LastPlayDateTick = long.Parse(array2[0]);
				D3DIapDiscount.Instance.CurrentDiscount = int.Parse(array2[1]);
				D3DIapDiscount.Instance.IapDiscountStates[0] = (D3DIapDiscount.DiscountState)int.Parse(array2[2]);
				D3DIapDiscount.Instance.IapDiscountStates[1] = (D3DIapDiscount.DiscountState)int.Parse(array2[3]);
				D3DIapDiscount.Instance.IapDiscountStates[2] = (D3DIapDiscount.DiscountState)int.Parse(array2[4]);
				D3DIapDiscount.Instance.IapDiscountStates[3] = (D3DIapDiscount.DiscountState)int.Parse(array2[5]);
				D3DIapDiscount.Instance.DiscountCountDownSeconds = float.Parse(array2[6]);
				D3DIapDiscount.Instance.DiscountCountDownDateTick = long.Parse(array2[7]);
				GamePlayedTime = float.Parse(array2[8]);
			}
		}
	}

	public void DefaultProgress()
	{
		D3DCurrencyStr = XXTEAUtils.Encrypt("1000", PropertySk[1]);
		TCrystalStr = XXTEAUtils.Encrypt("0", PropertySk[0]);
		ValidStorePage = 2;
		VaildSkillSlot = 3;
		ExpBonus = 0f;
		GoldBonus = 0f;
		Claim = string.Empty;
		GamePlayedTime = 0f;
		D3DIapDiscount.Instance.Default();
	}

	public void SaveDungeonProgress()
	{
	}

	private string SaveDungeonProgressToString()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<Dp>\n";
		foreach (string key in D3DMain.Instance.D3DDungeonManager.Keys)
		{
			D3DDungeon d3DDungeon = D3DMain.Instance.D3DDungeonManager[key];
			string text2 = text;
			text = text2 + "<Dn id=\"" + key + "\" el=\"" + d3DDungeon.explored_level + "\">\n";
			if (D3DDungeonProgerssManager.Instance.DungeonProgressManager.ContainsKey(key))
			{
				Dictionary<int, D3DDungeonProgerssManager.LevelProgress> dictionary = D3DDungeonProgerssManager.Instance.DungeonProgressManager[key];
				foreach (int key2 in dictionary.Keys)
				{
					text2 = text;
					text = text2 + "<Lv lvi=\"" + key2 + "\" rds=\"" + dictionary[key2].read + "\">\n";
					foreach (int key3 in dictionary[key2].UnlockBattleList.Keys)
					{
						text2 = text;
						text = text2 + "<Up sp=\"" + key3 + "\" pp=\"" + dictionary[key2].UnlockBattleList[key3].start_read + "," + dictionary[key2].UnlockBattleList[key3].win_read + "," + dictionary[key2].UnlockBattleList[key3].win_target + "\"/>\n";
					}
					text += "</Lv>\n";
				}
			}
			text += "</Dn>\n";
		}
		text += "</Dp>";
		return XXTEAUtils.Encrypt(text, Sk[3]);
	}

	public void LoadDungeonProgress()
	{
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("dpr", Sk[3])) + ".tlh", ref content);
		LoadDungeonProgressFromString(content);
	}

	private void LoadDungeonProgressFromString(string save_str)
	{
		save_str = XXTEAUtils.Decrypt(save_str, Sk[3]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(save_str);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if (!("Dn" == item.Name))
			{
				continue;
			}
			XmlElement xmlElement = (XmlElement)item;
			string key = xmlElement.GetAttribute("id").Trim();
			if (!D3DMain.Instance.D3DDungeonManager.ContainsKey(key))
			{
				continue;
			}
			D3DDungeon d3DDungeon = D3DMain.Instance.D3DDungeonManager[key];
			d3DDungeon.explored_level = int.Parse(xmlElement.GetAttribute("el").Trim());
			if (!D3DDungeonProgerssManager.Instance.DungeonProgressManager.ContainsKey(key))
			{
				continue;
			}
			Dictionary<int, D3DDungeonProgerssManager.LevelProgress> dictionary = D3DDungeonProgerssManager.Instance.DungeonProgressManager[key];
			foreach (XmlNode item2 in item)
			{
				if (!("Lv" == item2.Name))
				{
					continue;
				}
				xmlElement = (XmlElement)item2;
				int key2 = int.Parse(xmlElement.GetAttribute("lvi").Trim());
				if (!dictionary.ContainsKey(key2))
				{
					continue;
				}
				dictionary[key2].read = bool.Parse(xmlElement.GetAttribute("rds").Trim());
				foreach (XmlNode item3 in item2)
				{
					if ("Up" == item3.Name)
					{
						xmlElement = (XmlElement)item3;
						int key3 = int.Parse(xmlElement.GetAttribute("sp").Trim());
						if (dictionary[key2].UnlockBattleList.ContainsKey(key3))
						{
							string[] array = xmlElement.GetAttribute("pp").Trim().Split(',');
							dictionary[key2].UnlockBattleList[key3].start_read = bool.Parse(array[0]);
							dictionary[key2].UnlockBattleList[key3].win_read = bool.Parse(array[1]);
							dictionary[key2].UnlockBattleList[key3].win_target = bool.Parse(array[2]);
						}
					}
				}
			}
		}
	}

	public void SaveTutorialState()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<D3DTutorial>\n";
		text += "<TS state=\"";
		foreach (bool item in TutorialState)
		{
			text = text + item + ",";
		}
		text += "\"/>\n";
		text += "</D3DTutorial>";
		Utils.FileSaveString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("tutorial", Sk[8])) + ".tlh", XXTEAUtils.Encrypt(text, Sk[8]));
	}

	public void LoadTutorialState()
	{
		TutorialState.Clear();
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("tutorial", Sk[8])) + ".tlh", ref content);
		content = XXTEAUtils.Decrypt(content, Sk[8]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if (!("TS" == item.Name))
			{
				continue;
			}
			XmlElement xmlElement = (XmlElement)item;
			string text = xmlElement.GetAttribute("state").Trim();
			string[] array = text.Split(',');
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				if (!(string.Empty == text2))
				{
					TutorialState.Add(bool.Parse(text2));
				}
			}
			if (TutorialState.Count == 10)
			{
				TutorialState.Add(false);
			}
			if (TutorialState.Count == 11)
			{
				TutorialState.Add(true);
				TutorialState.Add(true);
			}
		}
	}

	public void SaveNewHints()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<NewHints>\n";
		text += "<Gear slot=\"";
		foreach (int item in NewGearSlotHint)
		{
			text = text + item + ",";
		}
		text += "\"/>\n";
		List<string> list = new List<string>();
		foreach (string key in NewSkillHint.Keys)
		{
			if (NewSkillHint[key] == null || NewSkillHint[key].Count == 0)
			{
				list.Add(key);
				continue;
			}
			text = text + "<Skill pid=\"" + key + "\" sid=\"";
			foreach (string item2 in NewSkillHint[key])
			{
				text = text + item2 + ",";
			}
			text += "\"/>\n";
		}
		foreach (string item3 in list)
		{
			NewSkillHint.Remove(item3);
		}
		text += "<Hero pid=\"";
		foreach (string item4 in NewHeroHint)
		{
			text = text + item4 + ",";
		}
		text += "\"/>\n";
		text += "</NewHints>";
		Utils.FileSaveString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("newhints", Sk[8])) + ".tlh", XXTEAUtils.Encrypt(text, Sk[8]));
	}

	public void LoadNewHints()
	{
		NewGearSlotHint.Clear();
		NewSkillHint.Clear();
		string content = string.Empty;
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("newhints", Sk[8])) + ".tlh", ref content);
		content = XXTEAUtils.Decrypt(content, Sk[8]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if ("Gear" == item.Name)
			{
				XmlElement xmlElement = (XmlElement)item;
				string text = xmlElement.GetAttribute("slot").Trim();
				string[] array = text.Split(',');
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!(string.Empty == text2))
					{
						NewGearSlotHint.Add(int.Parse(text2));
					}
				}
			}
			else if ("Skill" == item.Name)
			{
				XmlElement xmlElement2 = (XmlElement)item;
				string key = xmlElement2.GetAttribute("pid").Trim();
				string text3 = xmlElement2.GetAttribute("sid").Trim();
				string[] array3 = text3.Split(',');
				List<string> list = new List<string>();
				string[] array4 = array3;
				foreach (string text4 in array4)
				{
					if (!(string.Empty == text4))
					{
						list.Add(text4);
					}
				}
				if (list.Count > 0)
				{
					NewSkillHint.Add(key, list);
				}
			}
			else
			{
				if (!("Hero" == item.Name))
				{
					continue;
				}
				XmlElement xmlElement3 = (XmlElement)item;
				string text5 = xmlElement3.GetAttribute("pid").Trim();
				string[] array5 = text5.Split(',');
				string[] array6 = array5;
				foreach (string text6 in array6)
				{
					if (!(string.Empty == text6))
					{
						NewHeroHint.Add(text6);
					}
				}
			}
		}
	}

	public void SaveRespawnTime()
	{
		string content = SaveRespawnTimeDataToString();
		Utils.FileSaveString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("respawnTime", Sk[9])) + ".tlh", content);
	}

	private string SaveRespawnTimeDataToString()
	{
		string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
		text += "<RespawnTime>\n";
		long num = DateTime.Now.ToFileTimeUtc();
		foreach (string key in D3DMain.Instance.D3DDungeonManager.Keys)
		{
			D3DDungeon d3DDungeon = D3DMain.Instance.D3DDungeonManager[key];
			string text2 = text;
			text = text2 + "<Dungeon nameID=\"" + key + "\" savedtime=\"" + num + "\">\n";
			text += "<EnemyRespawn>\n";
			foreach (D3DDungeonFloor dungeon_floor in d3DDungeon.dungeon_floors)
			{
				foreach (int key2 in dungeon_floor.floor_spawners.Keys)
				{
					D3DDungeonFloorSpawner d3DDungeonFloorSpawner = dungeon_floor.floor_spawners[key2];
					if (d3DDungeonFloorSpawner.bShowCDTime)
					{
						float respawnTimeLeft = d3DDungeonFloorSpawner.RespawnTimeLeft;
						int enemyValue = d3DDungeonFloorSpawner.EnemyValue;
						if (respawnTimeLeft > 0f)
						{
							int floor_index = dungeon_floor.floor_index;
							int spawner_id = d3DDungeonFloorSpawner.spawner_id;
							text2 = text;
							text = text2 + "<RespawnTarget floor=\"" + floor_index + "\" spawnid=\"" + spawner_id + "\" timeleft=\"" + respawnTimeLeft + "\" enemyvalue=\"" + enemyValue + "\"/>\n";
						}
					}
				}
			}
			text += "</EnemyRespawn>\n";
			text += "<TreasureRespawn>\n";
			foreach (D3DDungeonFloor dungeon_floor2 in d3DDungeon.dungeon_floors)
			{
				for (int i = 0; i < dungeon_floor2.floor_treasures.Count; i++)
				{
					float respawnTimeLeft2 = dungeon_floor2.floor_treasures[i].RespawnTimeLeft;
					if (respawnTimeLeft2 > 0f)
					{
						int floor_index2 = dungeon_floor2.floor_index;
						text2 = text;
						text = text2 + "<RespawnTreasure floor=\"" + floor_index2 + "\" index=\"" + i + "\" timeleft=\"" + respawnTimeLeft2 + "\"/>\n";
					}
				}
			}
			text += "</TreasureRespawn>\n";
			text += "</Dungeon>\n";
		}
		text += "</RespawnTime>\n";
		return XXTEAUtils.Encrypt(text, Sk[9]);
	}

	public void LoadRespawnTime()
	{
		string name = D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("respawnTime", Sk[9])) + ".tlh";
		string content = string.Empty;
		Utils.FileGetString(name, ref content);
		LoadRespawnTimeFromString(content);
	}

	private void LoadRespawnTimeFromString(string save_str)
	{
		if (save_str.Length == 0)
		{
			return;
		}
		save_str = XXTEAUtils.Decrypt(save_str, Sk[9]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(save_str);
		XmlNode documentElement = xmlDocument.DocumentElement;
		long num = 0L;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			string key = xmlElement.GetAttribute("nameID").Trim();
			string text = xmlElement.GetAttribute("savedtime").Trim();
			if (text.Length > 0)
			{
				long num2 = DateTime.Now.ToFileTimeUtc();
				long num3 = long.Parse(text);
			}
			foreach (XmlNode item2 in item)
			{
				if (item2.Name == "EnemyRespawn")
				{
					foreach (XmlNode item3 in item2)
					{
						string name = item3.Name;
						XmlElement xmlElement2 = (XmlElement)item3;
						int num4 = int.Parse(xmlElement2.GetAttribute("floor").Trim());
						int key2 = int.Parse(xmlElement2.GetAttribute("spawnid").Trim());
						float num5 = float.Parse(xmlElement2.GetAttribute("timeleft").Trim());
						num5 -= (float)num;
						if (num5 < 0f)
						{
							num5 = 1f;
						}
						string text2 = xmlElement2.GetAttribute("enemyvalue").Trim();
						if (text2.Length == 0)
						{
							text2 = "100";
						}
						int nEnemyValue = int.Parse(text2.Trim());
						int count = D3DMain.Instance.D3DDungeonManager[key].dungeon_floors.Count;
						if (num4 <= count && D3DMain.Instance.D3DDungeonManager[key].dungeon_floors[num4 - 1].floor_spawners.ContainsKey(key2))
						{
							D3DMain.Instance.D3DDungeonManager[key].dungeon_floors[num4 - 1].floor_spawners[key2].StartSpawnTime(num5, nEnemyValue);
						}
					}
				}
				else
				{
					if (!(item2.Name == "TreasureRespawn"))
					{
						continue;
					}
					foreach (XmlNode item4 in item2)
					{
						string name2 = item4.Name;
						XmlElement xmlElement3 = (XmlElement)item4;
						int num6 = int.Parse(xmlElement3.GetAttribute("floor").Trim());
						int num7 = int.Parse(xmlElement3.GetAttribute("index").Trim());
						float num8 = float.Parse(xmlElement3.GetAttribute("timeleft").Trim());
						num8 -= (float)num;
						if (num8 < 0f)
						{
							num8 = 1f;
						}
						int count2 = D3DMain.Instance.D3DDungeonManager[key].dungeon_floors.Count;
						if (num6 <= count2)
						{
							int count3 = D3DMain.Instance.D3DDungeonManager[key].dungeon_floors[num6 - 1].floor_treasures.Count;
							if (num7 < count3)
							{
								D3DMain.Instance.D3DDungeonManager[key].dungeon_floors[num6 - 1].floor_treasures[num7].StartSpawnTime(num8);
							}
						}
					}
				}
			}
		}
	}

	public void ConvertSaveDoc()
	{
		if (!Utils.CheckFileExists(SaveFileName) && IsVersion1point0Save())
		{
			LoadAllDataOld();
			D3DCurrencyStr = XXTEAUtils.Encrypt(D3DCurrency.ToString(), PropertySk[1]);
			TCrystalStr = XXTEAUtils.Encrypt(TCrystal.ToString(), PropertySk[0]);
			D3DIapDiscount.Instance.Default();
			GamePlayedTime = 0f;
			SaveAllData();
			Utils.DeleteFile(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("dpr", Instance.Sk[3]) + ".tlh"));
			Utils.DeleteFile(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("progress", Instance.Sk[5]) + ".tlh"));
			Utils.DeleteFile(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("store", Instance.Sk[6]) + ".tlh"));
			Utils.DeleteFile(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("team", Instance.Sk[7]) + ".tlh"));
			Utils.DeleteFile(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("respawntime", Instance.Sk[9]) + ".tlh"));
		}
	}

	private bool IsVersion1point0Save()
	{
		string content = string.Empty;
		if (!Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("progress", Instance.Sk[5])) + ".tlh") || !Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("dpr", Instance.Sk[3])) + ".tlh") || !Utils.CheckFileExists(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("store", Instance.Sk[6])) + ".tlh"))
		{
			return false;
		}
		Utils.FileGetString(D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("team", Sk[7])) + ".tlh", ref content);
		content = XXTEAUtils.Decrypt(content, Sk[7]);
		if (content.Contains("D3DTeamData version=\"1.0\""))
		{
			return true;
		}
		return false;
	}

	public void AddCurrencyChangedEvent(OnCurrencyChanged eventChanged)
	{
		this.onCurrencyChangedEvent = (OnCurrencyChanged)Delegate.Combine(this.onCurrencyChangedEvent, eventChanged);
	}

	public void RemoveCurrencyChangedEvent(OnCurrencyChanged eventChanged)
	{
		this.onCurrencyChangedEvent = (OnCurrencyChanged)Delegate.Remove(this.onCurrencyChangedEvent, eventChanged);
	}

	public void UpdateCurrency(int variable)
	{
		int num = int.Parse(XXTEAUtils.Decrypt(D3DCurrencyStr, PropertySk[1]));
		num += variable;
		if (num < 0)
		{
			num = 0;
		}
		else if (num > 9999999)
		{
			num = 9999999;
		}
		D3DCurrencyStr = XXTEAUtils.Encrypt(num.ToString(), PropertySk[1]);
		if (this.onCurrencyChangedEvent != null)
		{
			this.onCurrencyChangedEvent(CurrencyText, CrystalText);
		}
	}

	public void UpdateCrystal(int variable)
	{
		int num = int.Parse(XXTEAUtils.Decrypt(TCrystalStr, PropertySk[0]));
		num += variable;
		if (num < 0)
		{
			num = 0;
		}
		else if (num > 9999)
		{
			num = 9999;
		}
		TCrystalStr = XXTEAUtils.Encrypt(num.ToString(), PropertySk[0]);
		if (this.onCurrencyChangedEvent != null)
		{
			this.onCurrencyChangedEvent(CurrencyText, CrystalText);
		}
	}
}
