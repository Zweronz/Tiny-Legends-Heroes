using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class D3DAudioManager
{
	public enum CommonAudio
	{
		THEME = 0,
		BATTLE_WIN = 1,
		BATTLE_LOSE = 2,
		BUTTON_ROUND = 3,
		BUTTON_SQUARE = 4,
		SKILL_UP = 5,
		SKILL_DOWN = 6,
		OPEN_SHOP = 7,
		SCROLL_SLIP = 8,
		TAVERN_CHARACTER_OPERATE = 9,
		ITEM_LOOT = 10,
		ITEM_COMPARE = 11,
		ITEM_DESTORY = 12,
		SKILL_LV_UP = 13,
		MONEY_GET = 14,
		COMMON_BATTLE = 15,
		BOSS_BATTLE = 16,
		BIG_BOSS_BATTLE = 17
	}

	private static D3DAudioManager instance;

	private List<string> D3DCommonAudio;

	private List<D3DAudioStringFinder> D3DDungeonTownMusic;

	private List<D3DAudioStringFinder> D3DDungeonAmb;

	private List<D3DAudioStringFinder> D3DDungeonFootStep;

	private List<D3DAudioIntFinder> D3DEquipmentMaterialPickUp;

	private List<D3DAudioIntFinder> D3DEquipmentMaterialPutDown;

	public D3DAudioBehaviour ThemeAudio;

	public D3DAudioBehaviour DungeonTownAudio;

	public D3DAudioBehaviour DungeonAmbAudio;

	public D3DAudioBehaviour ArenaAudio;

	public static D3DAudioManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DAudioManager();
			}
			return instance;
		}
	}

	public void LoadD3DAudioCfg()
	{
		D3DCommonAudio = new List<string>();
		D3DDungeonTownMusic = new List<D3DAudioStringFinder>();
		D3DDungeonAmb = new List<D3DAudioStringFinder>();
		D3DDungeonFootStep = new List<D3DAudioStringFinder>();
		D3DEquipmentMaterialPickUp = new List<D3DAudioIntFinder>();
		D3DEquipmentMaterialPutDown = new List<D3DAudioIntFinder>();
		TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DAudio", D3DGamer.Instance.Sk[0]))) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]));
		XmlElement documentElement = xmlDocument.DocumentElement;
		XmlNode xmlNode = documentElement;
		string[] array = new string[15]
		{
			"ThemeMusic", "BattleWinMusic", "BattleLoseMusic", "ButtonRoundSfx", "ButtonSquareSfx", "SkillUpSfx", "SkillDownSfx", "OpenShopSfx", "ScrollSlipSfx", "TavernCharacterSfx",
			"LootItemSfx", "ItemCompareSfx", "ItemDestorySfx", "SkillLvUpSfx", "MoneyGetSfx"
		};
		string[] array2 = array;
		foreach (string text in array2)
		{
			D3DCommonAudio.Add(documentElement.GetAttribute("my:" + text).Trim());
		}
		foreach (XmlNode item in xmlNode)
		{
			if ("my:DungeonTownMusic" == item.Name)
			{
				D3DAudioStringFinder d3DAudioStringFinder = new D3DAudioStringFinder();
				d3DAudioStringFinder.AudioPrefab = ((XmlElement)item).GetAttribute("my:TownMusic").Trim();
				foreach (XmlNode item2 in item)
				{
					if ("my:DungeonID" == item2.Name)
					{
						d3DAudioStringFinder.Indexer.Add(item2.FirstChild.Value);
					}
				}
				D3DDungeonTownMusic.Add(d3DAudioStringFinder);
			}
			else if ("my:DungeonAmbPrefab" == item.Name)
			{
				D3DAudioStringFinder d3DAudioStringFinder2 = new D3DAudioStringFinder();
				d3DAudioStringFinder2.AudioPrefab = ((XmlElement)item).GetAttribute("my:AmbName").Trim();
				foreach (XmlNode item3 in item)
				{
					if ("my:DungeonNo" == item3.Name)
					{
						d3DAudioStringFinder2.Indexer.Add(item3.FirstChild.Value);
					}
				}
				D3DDungeonAmb.Add(d3DAudioStringFinder2);
			}
			else if ("my:DungeonFootStepPrefab" == item.Name)
			{
				D3DAudioStringFinder d3DAudioStringFinder3 = new D3DAudioStringFinder();
				d3DAudioStringFinder3.AudioPrefab = ((XmlElement)item).GetAttribute("my:FootStepName").Trim();
				foreach (XmlNode item4 in item)
				{
					if ("my:DungeonNo" == item4.Name)
					{
						d3DAudioStringFinder3.Indexer.Add(item4.FirstChild.Value);
					}
				}
				D3DDungeonFootStep.Add(d3DAudioStringFinder3);
			}
			else if ("my:GearSfx" == item.Name)
			{
				D3DAudioIntFinder d3DAudioIntFinder = new D3DAudioIntFinder();
				D3DAudioIntFinder d3DAudioIntFinder2 = new D3DAudioIntFinder();
				d3DAudioIntFinder.AudioPrefab = ((XmlElement)item).GetAttribute("my:GearUpSfx").Trim();
				d3DAudioIntFinder2.AudioPrefab = ((XmlElement)item).GetAttribute("my:GearDownSfx").Trim();
				foreach (XmlNode item5 in item)
				{
					if ("my:GearType" == item5.Name)
					{
						d3DAudioIntFinder.Indexer.Add(int.Parse(item5.FirstChild.Value));
						d3DAudioIntFinder2.Indexer.Add(int.Parse(item5.FirstChild.Value));
					}
				}
				D3DEquipmentMaterialPickUp.Add(d3DAudioIntFinder);
				D3DEquipmentMaterialPutDown.Add(d3DAudioIntFinder2);
			}
			else if ("my:BattleMusic" == item.Name)
			{
				D3DCommonAudio.Add(((XmlElement)item).GetAttribute("my:NormalBattle").Trim());
				D3DCommonAudio.Add(((XmlElement)item).GetAttribute("my:BossBattle").Trim());
				D3DCommonAudio.Add(((XmlElement)item).GetAttribute("my:BigBossBattle").Trim());
			}
		}
	}

	public string GetCommonAudio(CommonAudio audio)
	{
		return D3DCommonAudio[(int)audio];
	}

	public string GetTownMusic(string dungeon_id)
	{
		foreach (D3DAudioStringFinder item in D3DDungeonTownMusic)
		{
			if (!item.Indexer.Contains(dungeon_id))
			{
				continue;
			}
			return item.AudioPrefab;
		}
		return string.Empty;
	}

	public string GetDungeonAmb(string dungeon_postfix)
	{
		foreach (D3DAudioStringFinder item in D3DDungeonAmb)
		{
			if (!item.Indexer.Contains(dungeon_postfix))
			{
				continue;
			}
			return item.AudioPrefab;
		}
		return string.Empty;
	}

	public string GetDungeonFootStep(string dungeon_postfix)
	{
		foreach (D3DAudioStringFinder item in D3DDungeonFootStep)
		{
			if (!item.Indexer.Contains(dungeon_postfix))
			{
				continue;
			}
			return item.AudioPrefab;
		}
		return string.Empty;
	}

	public string GetEquipmentPickUpSfx(int equipment_type)
	{
		foreach (D3DAudioIntFinder item in D3DEquipmentMaterialPickUp)
		{
			if (!item.Indexer.Contains(equipment_type))
			{
				continue;
			}
			return item.AudioPrefab;
		}
		return string.Empty;
	}

	public string GetEquipmentPutDownSfx(int equipment_type)
	{
		foreach (D3DAudioIntFinder item in D3DEquipmentMaterialPutDown)
		{
			if (!item.Indexer.Contains(equipment_type))
			{
				continue;
			}
			return item.AudioPrefab;
		}
		return string.Empty;
	}

	public void PlayAudio(string audio_name, GameObject audio_player, bool bind_player, bool sync_player, bool auto_destroy = true)
	{
		if (!(string.Empty == audio_name))
		{
			Object @object = Resources.Load("SoundEvent/" + audio_name);
			if (!(null == @object))
			{
				GameObject gameObject = Object.Instantiate(@object) as GameObject;
				D3DAudioBehaviour d3DAudioBehaviour = gameObject.AddComponent<D3DAudioBehaviour>();
				d3DAudioBehaviour.Init(audio_player, bind_player, sync_player, auto_destroy);
			}
		}
	}

	public void PlayAudio(string audio_name, ref D3DAudioBehaviour behaviour, GameObject audio_player, bool bind_player, bool sync_player, bool auto_destroy = true)
	{
		if (!(string.Empty == audio_name))
		{
			Object @object = Resources.Load("SoundEvent/" + audio_name);
			if (!(null == @object))
			{
				GameObject gameObject = Object.Instantiate(@object) as GameObject;
				gameObject.name = audio_name;
				behaviour = gameObject.AddComponent<D3DAudioBehaviour>();
				behaviour.Init(audio_player, bind_player, sync_player, auto_destroy);
			}
		}
	}
}
