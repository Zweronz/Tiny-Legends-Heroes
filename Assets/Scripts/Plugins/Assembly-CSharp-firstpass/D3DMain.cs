using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class D3DMain
{
	public enum ANDROID_PLATFORM
	{
		AMAZON = 0,
		GOOGLE_PLAY = 1
	}

	public const int SCENE_FLOOR_LAYER = 8;

	public const int PUPPET_LAYER = 9;

	public const int PROPS_LAYER = 10;

	public const int MAP_TRIGGER_LAYER = 11;

	public const int PUPPET_INTERACTIVE_NPC = 12;

	public const int WORLD_LIMIT_LAYER = 13;

	public const int UI_LAYER = 15;

	public const int FEATURE_CAMERA_LAYER = 16;

	public const int FLOOR_MASK = 256;

	public const int PUPPET_MASK = 512;

	public const int PUPPET_INTERACTIVE_NPC_MASK = 4096;

	public const int WORLD_LIMIT_MASK = 8192;

	public const int GRAVE_MASK = 131072;

	private static D3DMain instance;

	public ANDROID_PLATFORM AndroidPlatform;

	public bool bDefeatedByBoss;

	public bool ShowFps;

	public bool TriggerApplicationPause;

	public int LoadingScene;

	public int CurrentScene;

	public ArenaBattleType BattleType;

	public List<UIHelper> D3DUIList = new List<UIHelper>();

	public int current_level;

	public int level_map;

	public int HD_SIZE = 1;

	public GameObject HD_BOARD_OBJ;

	public D3DFontManager GameFont1;

	public D3DFontManager GameFont2;

	public Color CommonFontColor;

	private Dictionary<string, D3DPassiveSkill> D3DPassiveSkillManager = new Dictionary<string, D3DPassiveSkill>();

	private Dictionary<string, D3DActiveSkill> D3DActiveSkillManager = new Dictionary<string, D3DActiveSkill>();

	private Dictionary<string, D3DClass> D3DClassManager = new Dictionary<string, D3DClass>();

	public Dictionary<string, D3DEquipment> D3DEquipmentManager = new Dictionary<string, D3DEquipment>();

	public Dictionary<D3DEquipment.EquipmentClass, List<string>> D3DImplicitEquipPopedom = new Dictionary<D3DEquipment.EquipmentClass, List<string>>();

	private Dictionary<string, D3DPuppetProfile> D3DPuppetProfileManager = new Dictionary<string, D3DPuppetProfile>();

	private Dictionary<string, D3DEnemyGroup> D3DEnemyGroupManager = new Dictionary<string, D3DEnemyGroup>();

	public ExploringDungeon exploring_dungeon = new ExploringDungeon();

	public Dictionary<string, D3DDungeon> D3DDungeonManager = new Dictionary<string, D3DDungeon>();

	public D3DFormulaCoe hp_default_coe;

	public List<D3DFormulaCoe> hp_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe mp_default_coe;

	public List<D3DFormulaCoe> mp_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe def_default_coe;

	public List<D3DFormulaCoe> def_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe main_power_default_coe;

	public List<D3DFormulaCoe> main_power_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe sub_power_default_coe;

	public List<D3DFormulaCoe> sub_power_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe hp_recover_default_coe;

	public List<D3DFormulaCoe> hp_recover_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe mp_recover_default_coe;

	public List<D3DFormulaCoe> mp_recover_coe_list = new List<D3DFormulaCoe>();

	public D3DFormulaCoe damage_range_default_coe;

	public List<D3DFormulaCoe> damage_range_coe_list = new List<D3DFormulaCoe>();

	private D3DPuppetAI DefaultPuppetAI;

	private Dictionary<string, D3DPuppetAI> D3DAIManager = new Dictionary<string, D3DPuppetAI>();

	public D3DPuppetTransformCfg DefaultPuppetTransform;

	public Dictionary<string, D3DPuppetTransformCfg> PuppetTransformManager = new Dictionary<string, D3DPuppetTransformCfg>();

	public Dictionary<string, D3DCustomLoot> D3DCustomLootManager = new Dictionary<string, D3DCustomLoot>();

	public List<D3DEquipment> LootEquipments = new List<D3DEquipment>();

	public List<D3DInt> LootGoldBag = new List<D3DInt>();

	public List<D3DInt> MutationSlot = new List<D3DInt>();

	public Dictionary<string, D3DTreasure> D3DTreasureManager = new Dictionary<string, D3DTreasure>();

	public Dictionary<string, D3DHelmConfig> D3DHelmConfigManager = new Dictionary<string, D3DHelmConfig>();

	public static D3DMain Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DMain();
			}
			return instance;
		}
	}

	public float RealTimeScale
	{
		get
		{
			return (Time.timeScale != 0f) ? (1f / Time.timeScale) : 0f;
		}
	}

	public void CreateFontManagers()
	{
		Dictionary<int, float[]> dictionary = new Dictionary<int, float[]>();
		dictionary.Add(6, new float[2] { 2f, -1f });
		dictionary.Add(7, new float[2] { 2f, -1f });
		dictionary.Add(8, new float[2] { 2f, -1f });
		dictionary.Add(9, new float[2] { 2f, -1f });
		dictionary.Add(12, new float[2] { 2f, -2f });
		dictionary.Add(14, new float[2] { 2f, -2f });
		dictionary.Add(16, new float[2] { 2f, -2f });
		dictionary.Add(18, new float[2] { 2f, -2f });
		dictionary.Add(22, new float[2] { 2f, -2f });
		GameFont1 = new D3DFontManager("A.C.M.E. Explosive ", dictionary);
		dictionary = new Dictionary<int, float[]>();
		dictionary.Add(9, new float[2] { 2f, -1.5f });
		dictionary.Add(11, new float[2] { 2f, -1.5f });
		dictionary.Add(13, new float[2] { 2f, -1.5f });
		dictionary.Add(18, new float[2] { 2f, -2.5f });
		dictionary.Add(22, new float[2] { 2f, -2.5f });
		dictionary.Add(26, new float[2] { 2f, -2.5f });
		dictionary.Add(222, new float[2] { 2f, -2.5f });
		dictionary.Add(262, new float[2] { 8f, 0f });
		GameFont2 = new D3DFontManager("Sergeant SixPack ", dictionary);
		CommonFontColor = new Color(0.88235295f, 0.75686276f, 0.47843137f);
	}

	public void LoadD3DPassiveSkillFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DPassiveSkill File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "r@]3xG7I,WiLG65-");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DPassiveSkill d3DPassiveSkill = new D3DPassiveSkill();
			XmlElement xmlElement = (XmlElement)item;
			d3DPassiveSkill.skill_id = xmlElement.GetAttribute("my:SkillID").Trim();
			if (string.Empty == d3DPassiveSkill.skill_id)
			{
				continue;
			}
			if (!(string.Empty != d3DPassiveSkill.skill_id))
			{
				if (diagnose)
				{
					num++;
				}
				continue;
			}
			d3DPassiveSkill.skill_name = xmlElement.GetAttribute("my:SkillName").Trim();
			d3DPassiveSkill.skill_icon = xmlElement.GetAttribute("my:SkillIcon").Trim();
			foreach (XmlNode item2 in item)
			{
				xmlElement = (XmlElement)item2;
				if ("my:MasterLevels" == item2.Name)
				{
					d3DPassiveSkill.max_level = int.Parse(xmlElement.GetAttribute("my:MaxLevel").Trim());
				}
				else if ("my:SkillTriggers" == item2.Name)
				{
					foreach (XmlNode item3 in item2)
					{
						if (!("my:TriggerDetial" == item3.Name))
						{
							continue;
						}
						D3DPassiveTriggerComplex d3DPassiveTriggerComplex = new D3DPassiveTriggerComplex();
						XmlElement xmlElement2 = (XmlElement)item3;
						d3DPassiveTriggerComplex.passive_type = (D3DPassiveTrigger.PassiveType)int.Parse(xmlElement2.GetAttribute("my:TriggerType").Trim());
						foreach (XmlNode item4 in item3)
						{
							if (!("my:TriggerData" == item4.Name))
							{
								continue;
							}
							XmlElement xmlElement3 = (XmlElement)item4;
							D3DPassiveTrigger.PassiveDataType passiveDataType = (D3DPassiveTrigger.PassiveDataType)int.Parse(xmlElement3.GetAttribute("my:DataType").Trim());
							switch (passiveDataType)
							{
							case D3DPassiveTrigger.PassiveDataType.FIXED_VALUE:
								if (d3DPassiveTriggerComplex.fixed_value != null)
								{
									continue;
								}
								break;
							case D3DPassiveTrigger.PassiveDataType.PERCENT_VALUE:
								if (d3DPassiveTriggerComplex.percent_value != null)
								{
									continue;
								}
								break;
							}
							d3DPassiveTriggerComplex.SetDataContentTag(passiveDataType, xmlElement3.GetAttribute("my:ContentIndex").Trim());
							foreach (XmlNode item5 in item4)
							{
								if (!("my:Datas" == item5.Name))
								{
									continue;
								}
								foreach (XmlNode item6 in item5)
								{
									if ("my:DataValue" == item6.Name && item6.ChildNodes[0] != null)
									{
										d3DPassiveTriggerComplex.CreateTriggerData(passiveDataType, item6.ChildNodes[0].Value);
									}
								}
							}
						}
						d3DPassiveSkill.passive_triggers.Add(d3DPassiveTriggerComplex);
					}
				}
				else
				{
					if (!("my:Description" == item2.Name))
					{
						continue;
					}
					foreach (XmlNode item7 in item2)
					{
						if ("my:Content" == item7.Name && item7.ChildNodes[0] != null && string.Empty != item7.ChildNodes[0].Value)
						{
							d3DPassiveSkill.description.Add(item7.ChildNodes[0].Value);
						}
					}
				}
			}
			d3DPassiveSkill.CreateReplaceTags();
			D3DPassiveSkillManager.Add(d3DPassiveSkill.skill_id, d3DPassiveSkill);
		}
		if (!diagnose)
		{
			return;
		}
		string text3 = text;
		text = text3 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item8 in list)
		{
			text = text + item8 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DPassiveSkillsBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DPassiveSkillFromFile(folder_path + "/" + @object.name, true);
		}
		OutputD3DPassiveSkills();
	}

	public void OutputD3DPassiveSkills()
	{
		int num = 1;
		foreach (string key in D3DPassiveSkillManager.Keys)
		{
			D3DPassiveSkill d3DPassiveSkill = D3DPassiveSkillManager[key];
			string empty = string.Empty;
			string text = empty;
			empty = text + "=================No." + num + "=================\n";
			empty = empty + "Skill ID:" + d3DPassiveSkill.skill_id + "\n";
			empty = empty + "Skill Name:" + d3DPassiveSkill.skill_name + "\n";
			empty = empty + "Skill Icon:" + d3DPassiveSkill.skill_icon + "\n";
			text = empty;
			empty = text + "Max Level:" + d3DPassiveSkill.max_level + "\n";
			empty += "Level Data:\n";
			int num2 = 1;
			foreach (D3DPassiveTriggerComplex passive_trigger in d3DPassiveSkill.passive_triggers)
			{
				text = empty;
				empty = string.Concat(text, "----Passive:", passive_trigger.passive_type, "\n");
				if (passive_trigger.fixed_value == null)
				{
					continue;
				}
				empty = empty + "Fixed Value:\nReplace Tag:" + passive_trigger.fixed_value.replace_tag + "\nValues:\n";
				foreach (float value in passive_trigger.fixed_value.values)
				{
					float num3 = value;
					text = empty;
					empty = text + "Lv" + num2 + ":" + num3 + "\n";
				}
			}
			empty += "\nDescription:\n";
			foreach (string item in d3DPassiveSkill.description)
			{
				empty = empty + "\n" + item;
			}
			num++;
		}
	}

	public bool CheckPassiveSkillID(string skill_id)
	{
		if (D3DPassiveSkillManager.ContainsKey(skill_id))
		{
			return true;
		}
		return false;
	}

	public D3DPassiveSkill GetPassiveSkill(string skill_id)
	{
		if (!CheckPassiveSkillID(skill_id))
		{
			return null;
		}
		return D3DPassiveSkillManager[skill_id];
	}

	public void LoadD3DActiveSkillFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DActiveSkill File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "KQ[-ix1,#t,Wm)6V");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item2 in documentElement)
		{
			D3DActiveSkill d3DActiveSkill = new D3DActiveSkill();
			XmlElement xmlElement = (XmlElement)item2;
			d3DActiveSkill.skill_id = xmlElement.GetAttribute("my:SkillID").Trim();
			if (!(string.Empty != d3DActiveSkill.skill_id))
			{
				if (diagnose)
				{
					num++;
				}
				continue;
			}
			d3DActiveSkill.active_type = (D3DActiveSkill.ActiveType)int.Parse(xmlElement.GetAttribute("my:SkillType").Trim());
			if (d3DActiveSkill.active_type < D3DActiveSkill.ActiveType.TAP_ENEMY || d3DActiveSkill.active_type > D3DActiveSkill.ActiveType.PROMPT)
			{
				if (diagnose)
				{
					list.Add(d3DActiveSkill.skill_id);
				}
				continue;
			}
			d3DActiveSkill.skill_name = xmlElement.GetAttribute("my:SkillName").Trim();
			d3DActiveSkill.skill_icon = xmlElement.GetAttribute("my:SkillIcon").Trim();
			d3DActiveSkill.activation = bool.Parse(xmlElement.GetAttribute("my:Activation").Trim());
			foreach (XmlNode item3 in item2)
			{
				xmlElement = (XmlElement)item3;
				if ("my:MasterLevels" == item3.Name)
				{
					d3DActiveSkill.max_level = int.Parse(xmlElement.GetAttribute("my:MaxLevel").Trim());
					foreach (XmlNode item4 in item3)
					{
						if ("my:LevelData" == item4.Name)
						{
							XmlElement xmlElement2 = (XmlElement)item4;
							float item = float.Parse(xmlElement2.GetAttribute("my:MPCost").Trim());
							d3DActiveSkill.mp_consume.Add(item);
							item = float.Parse(xmlElement2.GetAttribute("my:CD").Trim());
							d3DActiveSkill.cd.Add(item);
							item = float.Parse(xmlElement2.GetAttribute("my:Distance").Trim());
							d3DActiveSkill.distance.Add(item);
						}
					}
				}
				else if ("my:ActiveTrigger" == item3.Name)
				{
					ActiveSkillTrigger activeSkillTrigger = new ActiveSkillTrigger();
					activeSkillTrigger.lock_frame = bool.Parse(xmlElement.GetAttribute("my:LockFrame").Trim());
					activeSkillTrigger.trigger_delay = float.Parse(xmlElement.GetAttribute("my:Delay").Trim());
					activeSkillTrigger.camera_shake_time = float.Parse(xmlElement.GetAttribute("my:CameraShake").Trim());
					if (activeSkillTrigger.camera_shake_time < 0f)
					{
						activeSkillTrigger.camera_shake_time = 0f;
					}
					activeSkillTrigger.puppet_shake = bool.Parse(xmlElement.GetAttribute("my:PuppetShake").Trim());
					activeSkillTrigger.emplacement = bool.Parse(xmlElement.GetAttribute("my:Emplacement").Trim());
					activeSkillTrigger.independence = bool.Parse(xmlElement.GetAttribute("my:Independence").Trim());
					string empty = string.Empty;
					foreach (XmlNode item5 in item3)
					{
						if ("my:Duration" == item5.Name)
						{
							foreach (XmlNode item6 in item5)
							{
								if ("my:DurCount" == item6.Name)
								{
									activeSkillTrigger.trigger_count = new D3DTextInt();
									activeSkillTrigger.trigger_count.replace_tag = ((XmlElement)item6).GetAttribute("my:StrIndex").Trim();
									foreach (XmlNode item7 in item6)
									{
										if ("my:Value" == item7.Name)
										{
											activeSkillTrigger.trigger_count.values.Add(int.Parse(((XmlElement)item7).GetAttribute("my:ValueData").Trim()));
										}
									}
								}
								else if ("my:DurInterval" == item6.Name)
								{
									activeSkillTrigger.trigger_interval = new D3DTextFloat();
									activeSkillTrigger.trigger_interval.replace_tag = ((XmlElement)item6).GetAttribute("my:StrIndex").Trim();
									foreach (XmlNode item8 in item6)
									{
										if ("my:Value" == item8.Name)
										{
											activeSkillTrigger.trigger_interval.values.Add(float.Parse(((XmlElement)item8).GetAttribute("my:ValueData").Trim()));
										}
									}
								}
								else
								{
									if (!("my:DurLife" == item6.Name))
									{
										continue;
									}
									activeSkillTrigger.trigger_lifecycle = new D3DTextFloat();
									activeSkillTrigger.trigger_lifecycle.replace_tag = ((XmlElement)item6).GetAttribute("my:StrIndex").Trim();
									foreach (XmlNode item9 in item6)
									{
										if ("my:Value" == item9.Name)
										{
											activeSkillTrigger.trigger_lifecycle.values.Add(float.Parse(((XmlElement)item9).GetAttribute("my:ValueData").Trim()));
										}
									}
								}
							}
						}
						else if ("my:Effect" == item5.Name)
						{
							XmlElement xmlElement3 = (XmlElement)item5;
							TriggerBedeckEffect triggerBedeckEffect = new TriggerBedeckEffect();
							triggerBedeckEffect.effect_name = xmlElement3.GetAttribute("my:EffectName").Trim();
							triggerBedeckEffect.bedeck_type = (TriggerBedeckEffect.BedeckType)int.Parse(xmlElement3.GetAttribute("my:EffectType").Trim());
							triggerBedeckEffect.bedeck_player = (TriggerBedeckEffect.BedeckPlayer)int.Parse(xmlElement3.GetAttribute("my:Faction").Trim());
							triggerBedeckEffect.bind = bool.Parse(xmlElement3.GetAttribute("my:BindPosition").Trim());
							empty = xmlElement3.GetAttribute("my:Offset").Trim();
							if (string.Empty != empty)
							{
								string[] array = empty.Split(',');
								triggerBedeckEffect.effect_offset = new Vector3(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							}
							else
							{
								triggerBedeckEffect.effect_offset = Vector3.zero;
							}
							triggerBedeckEffect.auto_size = bool.Parse(xmlElement3.GetAttribute("my:AutoSize").Trim());
							triggerBedeckEffect.delay_time = float.Parse(xmlElement3.GetAttribute("my:Delay").Trim());
							triggerBedeckEffect.mount_point = xmlElement3.GetAttribute("my:MountPoint").Trim();
							triggerBedeckEffect.include_puppet_radius = bool.Parse(xmlElement3.GetAttribute("my:AddPuppetRadius").Trim());
							foreach (XmlNode item10 in item5)
							{
								if ("my:EffectSfx" == item10.Name)
								{
									if (triggerBedeckEffect.sfx_list == null)
									{
										triggerBedeckEffect.sfx_list = new List<string>();
									}
									if (item10.ChildNodes[0] != null)
									{
										triggerBedeckEffect.sfx_list.Add(item10.ChildNodes[0].Value);
									}
								}
							}
							if (activeSkillTrigger.common_bedeck_effects == null)
							{
								activeSkillTrigger.common_bedeck_effects = new List<TriggerBedeckEffect>();
							}
							if (activeSkillTrigger.lifecycle_bedeck_effects == null)
							{
								activeSkillTrigger.lifecycle_bedeck_effects = new List<TriggerBedeckEffect>();
							}
							if (string.Empty != triggerBedeckEffect.effect_name || triggerBedeckEffect.sfx_list.Count > 0)
							{
								if (triggerBedeckEffect.bedeck_type == TriggerBedeckEffect.BedeckType.ONE_SHOT)
								{
									activeSkillTrigger.common_bedeck_effects.Add(triggerBedeckEffect);
								}
								else if (triggerBedeckEffect.bedeck_type == TriggerBedeckEffect.BedeckType.LIFE_CYCLE)
								{
									activeSkillTrigger.lifecycle_bedeck_effects.Add(triggerBedeckEffect);
								}
							}
						}
						else if ("my:TransformConfig" == item5.Name)
						{
							foreach (XmlNode item11 in item5)
							{
								if ("my:MissileConfig" == item11.Name)
								{
									XmlElement xmlElement4 = (XmlElement)item11;
									activeSkillTrigger.trigger_missile = new TriggerMissile();
									activeSkillTrigger.trigger_missile.missile_name = xmlElement4.GetAttribute("my:MissileName").Trim();
									activeSkillTrigger.trigger_missile.use_fire_point = bool.Parse(xmlElement4.GetAttribute("my:FirePoint").Trim());
									activeSkillTrigger.trigger_missile.shoot_hit_point = bool.Parse(xmlElement4.GetAttribute("my:ShootHitPoint").Trim());
									empty = xmlElement4.GetAttribute("my:MissileOffset").Trim();
									string[] array2 = empty.Split(',');
									activeSkillTrigger.trigger_missile.fire_point_offset = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
									activeSkillTrigger.trigger_missile.shoot_caster = bool.Parse(xmlElement4.GetAttribute("my:BindCaster").Trim());
								}
							}
						}
						else if ("my:Range" == item5.Name)
						{
							activeSkillTrigger.area_of_effect = new AreaOfEffect();
							activeSkillTrigger.area_of_effect.filter_faction = (AreaOfEffect.FilterFaction)int.Parse(((XmlElement)item5).GetAttribute("my:RangeFaction").Trim());
							activeSkillTrigger.area_of_effect.include_default_target = bool.Parse(((XmlElement)item5).GetAttribute("my:IncludeDefaultTarget").Trim());
							activeSkillTrigger.area_of_effect.range_description = new D3DTextFloat();
							activeSkillTrigger.area_of_effect.trigger_areas = new List<AreaOfEffect.AreaConfig>();
							foreach (XmlNode item12 in item5)
							{
								if ("my:ValueStruct" == item12.Name)
								{
									activeSkillTrigger.area_of_effect.range_description.replace_tag = ((XmlElement)item12).GetAttribute("my:StrIndex").Trim();
									foreach (XmlNode item13 in item12)
									{
										if ("my:Value" == item13.Name)
										{
											activeSkillTrigger.area_of_effect.range_description.values.Add(float.Parse(((XmlElement)item13).GetAttribute("my:ValueData").Trim()));
										}
									}
								}
								else
								{
									if (!("my:ShapeConfig" == item12.Name))
									{
										continue;
									}
									foreach (XmlNode item14 in item12)
									{
										if ("my:Config" == item14.Name)
										{
											XmlElement xmlElement5 = (XmlElement)item14;
											AreaOfEffect.AreaConfig areaConfig = new AreaOfEffect.AreaConfig();
											areaConfig.area_shape = (AreaOfEffect.AreaConfig.AreaShape)int.Parse(xmlElement5.GetAttribute("my:RangeShape").Trim());
											empty = xmlElement5.GetAttribute("my:RangeSize").Trim();
											if (string.Empty != empty)
											{
												string[] array3 = empty.Split(',');
												areaConfig.area_size = new Vector2(float.Parse(array3[0].Trim()), float.Parse(array3[1].Trim()));
											}
											else
											{
												areaConfig.area_size = Vector2.zero;
											}
											areaConfig.area_origin = (AreaOfEffect.AreaConfig.AreaOrigin)int.Parse(xmlElement5.GetAttribute("my:ShapeOrigin").Trim());
											areaConfig.include_puppet_radius = bool.Parse(xmlElement5.GetAttribute("my:AddPuppetRadius").Trim());
											empty = xmlElement5.GetAttribute("my:V2Offset").Trim();
											if (string.Empty != empty)
											{
												string[] array4 = empty.Split(',');
												areaConfig.area_offset = new Vector2(float.Parse(array4[0].Trim()), float.Parse(array4[1].Trim()));
											}
											else
											{
												areaConfig.area_size = Vector2.zero;
											}
											activeSkillTrigger.area_of_effect.trigger_areas.Add(areaConfig);
										}
									}
								}
							}
						}
						else if ("my:Variable" == item5.Name)
						{
							foreach (XmlNode item15 in item5)
							{
								if (!("my:VariableConfig" == item15.Name))
								{
									continue;
								}
								XmlElement xmlElement6 = (XmlElement)item15;
								TriggerVariable triggerVariable = new TriggerVariable();
								triggerVariable.variable_values = new List<TriggerVariable.VariableValue>();
								triggerVariable.variable_type = (TriggerVariable.VariableType)int.Parse(xmlElement6.GetAttribute("my:VariableType").Trim());
								foreach (XmlNode item16 in item15)
								{
									XmlElement xmlElement7 = (XmlElement)item16;
									if ("my:VariableStruct" == item16.Name)
									{
										TriggerVariable.VariableValue variableValue = new TriggerVariable.VariableValue();
										variableValue.variable_source = (TriggerVariable.VariableValue.VariableSource)int.Parse(xmlElement7.GetAttribute("my:ValueType").Trim());
										variableValue.values = new D3DTextFloat();
										variableValue.values.replace_tag = xmlElement7.GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item17 in item16)
										{
											if ("my:Value" == item17.Name)
											{
												variableValue.values.values.Add(float.Parse(((XmlElement)item17).GetAttribute("my:ValueData").Trim()));
											}
										}
										triggerVariable.variable_values.Add(variableValue);
									}
									else if ("my:DotConfig" == item16.Name)
									{
										triggerVariable.dot_config = new TriggerVariable.DotConfig();
										foreach (XmlNode item18 in item16)
										{
											xmlElement7 = (XmlElement)item18;
											if ("my:DotTime" == item18.Name)
											{
												triggerVariable.dot_config.dot_time = new D3DTextFloat();
												triggerVariable.dot_config.dot_time.replace_tag = xmlElement7.GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item19 in item18)
												{
													if ("my:Value" == item19.Name)
													{
														triggerVariable.dot_config.dot_time.values.Add(float.Parse(((XmlElement)item19).GetAttribute("my:ValueData").Trim()));
													}
												}
											}
											else if ("my:DotInterval" == item18.Name)
											{
												triggerVariable.dot_config.dot_interval = new D3DTextFloat();
												triggerVariable.dot_config.dot_interval.replace_tag = xmlElement7.GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item20 in item18)
												{
													if ("my:Value" == item20.Name)
													{
														triggerVariable.dot_config.dot_interval.values.Add(float.Parse(((XmlElement)item20).GetAttribute("my:ValueData").Trim()));
													}
												}
											}
											else if ("my:DotEffect" == item18.Name)
											{
												triggerVariable.dot_config.dot_effect = xmlElement7.GetAttribute("my:EffectName").Trim();
												triggerVariable.dot_config.remain_effect = bool.Parse(xmlElement7.GetAttribute("my:EffectRemain").Trim());
												triggerVariable.dot_config.mount_point = xmlElement7.GetAttribute("my:MountPoint").Trim();
												triggerVariable.dot_config.dot_sfx = xmlElement7.GetAttribute("my:EffectSfx").Trim();
											}
											else
											{
												if (!("my:Extra" == item18.Name))
												{
													continue;
												}
												triggerVariable.dot_config.extra_variable = new TriggerVariable.DotConfig.ExtraVariable();
												foreach (XmlNode item21 in item18)
												{
													if ("my:DotEffect" == item21.Name)
													{
														triggerVariable.dot_config.extra_variable.extra_effect = ((XmlElement)item21).GetAttribute("my:EffectName").Trim();
														triggerVariable.dot_config.extra_variable.extra_sfx = ((XmlElement)item21).GetAttribute("my:EffectSfx").Trim();
														triggerVariable.dot_config.extra_variable.mount_point = ((XmlElement)item21).GetAttribute("my:MountPoint").Trim();
													}
													else
													{
														if (!("my:VariableStruct" == item21.Name))
														{
															continue;
														}
														TriggerVariable.VariableValue variableValue2 = new TriggerVariable.VariableValue();
														variableValue2.variable_source = (TriggerVariable.VariableValue.VariableSource)int.Parse(((XmlElement)item21).GetAttribute("my:ValueType").Trim());
														variableValue2.values = new D3DTextFloat();
														variableValue2.values.replace_tag = ((XmlElement)item21).GetAttribute("my:StrIndex").Trim();
														foreach (XmlNode item22 in item21)
														{
															if ("my:Value" == item22.Name)
															{
																variableValue2.values.values.Add(float.Parse(((XmlElement)item22).GetAttribute("my:ValueData").Trim()));
															}
														}
														if (triggerVariable.dot_config.extra_variable.extra_values == null)
														{
															triggerVariable.dot_config.extra_variable.extra_values = new List<TriggerVariable.VariableValue>();
														}
														triggerVariable.dot_config.extra_variable.extra_values.Add(variableValue2);
													}
												}
											}
										}
									}
									else if ("my:OutputConfig" == item16.Name)
									{
										triggerVariable.output_config = new VariableOutputConfig();
										triggerVariable.output_config.target_faction = (TargetFaction)int.Parse(xmlElement7.GetAttribute("my:Faction").Trim());
										triggerVariable.output_config.effect = xmlElement7.GetAttribute("my:EffectName").Trim();
										triggerVariable.output_config.mount_point = xmlElement7.GetAttribute("my:MountPoint").Trim();
										triggerVariable.output_config.sfx = xmlElement7.GetAttribute("my:EffectSfx").Trim();
										triggerVariable.output_config.outer_play = bool.Parse(xmlElement7.GetAttribute("my:HitOutside").Trim());
										triggerVariable.output_config.can_dodge = bool.Parse(xmlElement7.GetAttribute("my:CanDodge").Trim());
										foreach (XmlNode item23 in item16)
										{
											if (!("my:Imbibe" == item23.Name))
											{
												continue;
											}
											XmlElement xmlElement8 = (XmlElement)item23;
											triggerVariable.output_config.imbibe_config = new VariableOutputConfig.ImbibeConfig();
											triggerVariable.output_config.imbibe_config.imbibe_type = (VariableOutputConfig.ImbibeConfig.ImbibeType)int.Parse(xmlElement8.GetAttribute("my:ImbibeType").Trim());
											triggerVariable.output_config.imbibe_config.imbibe_to_hp = bool.Parse(xmlElement8.GetAttribute("my:ImbibeHp").Trim());
											triggerVariable.output_config.imbibe_config.imbibe_to_mp = bool.Parse(xmlElement8.GetAttribute("my:ImbibeMp").Trim());
											foreach (XmlNode item24 in item23)
											{
												if (!("my:ValueStruct" == item24.Name))
												{
													continue;
												}
												triggerVariable.output_config.imbibe_config.imbibe_percent = new D3DTextFloat();
												triggerVariable.output_config.imbibe_config.imbibe_percent.replace_tag = ((XmlElement)item24).GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item25 in item24)
												{
													if ("my:Value" == item25.Name)
													{
														triggerVariable.output_config.imbibe_config.imbibe_percent.values.Add(float.Parse(((XmlElement)item25).GetAttribute("my:ValueData").Trim()));
													}
												}
												if (triggerVariable.output_config.imbibe_config.imbibe_percent.values.Count == 0)
												{
													triggerVariable.output_config.imbibe_config.imbibe_percent = null;
												}
											}
										}
									}
									else
									{
										if (!("my:AureoleConfig" == item16.Name))
										{
											continue;
										}
										triggerVariable.aureole_config = new AureoleConfig();
										triggerVariable.aureole_config.aureole_faction = (AureoleConfig.AureoleFaction)int.Parse(xmlElement7.GetAttribute("my:AureoleFaction").Trim());
										triggerVariable.aureole_config.aureole_origin = (AureoleConfig.AureoleOrigin)int.Parse(xmlElement7.GetAttribute("my:AureoleOrigin").Trim());
										triggerVariable.aureole_config.include_puppet_radius = bool.Parse(xmlElement7.GetAttribute("my:AddPuppetRadius").Trim());
										triggerVariable.aureole_config.bind = bool.Parse(xmlElement7.GetAttribute("my:BindPosition").Trim());
										triggerVariable.aureole_config.aureole_effect = xmlElement7.GetAttribute("my:EffectName").Trim();
										triggerVariable.aureole_config.aureole_sfx = xmlElement7.GetAttribute("my:EffectSfx").Trim();
										triggerVariable.aureole_config.mount_point = xmlElement7.GetAttribute("my:MountPoint").Trim();
										foreach (XmlNode item26 in item16)
										{
											if ("my:AureolaRadius" == item26.Name)
											{
												triggerVariable.aureole_config.aureole_radius = new D3DTextFloat();
												triggerVariable.aureole_config.aureole_radius.replace_tag = ((XmlElement)item26).GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item27 in item26)
												{
													if ("my:Value" == item27.Name)
													{
														triggerVariable.aureole_config.aureole_radius.values.Add(float.Parse(((XmlElement)item27).GetAttribute("my:ValueData").Trim()));
													}
												}
											}
											else
											{
												if (!("my:DotTime" == item26.Name))
												{
													continue;
												}
												triggerVariable.aureole_config.aureole_time = new D3DTextFloat();
												triggerVariable.aureole_config.aureole_time.replace_tag = ((XmlElement)item26).GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item28 in item26)
												{
													if ("my:Value" == item28.Name)
													{
														triggerVariable.aureole_config.aureole_time.values.Add(float.Parse(((XmlElement)item28).GetAttribute("my:ValueData").Trim()));
													}
												}
											}
										}
									}
								}
								if (activeSkillTrigger.trigger_variable == null)
								{
									activeSkillTrigger.trigger_variable = new List<TriggerVariable>();
								}
								activeSkillTrigger.trigger_variable.Add(triggerVariable);
							}
						}
						else if ("my:Special" == item5.Name)
						{
							activeSkillTrigger.trigger_special = new TriggerSpecial();
							foreach (XmlNode item29 in item5)
							{
								if ("my:Taunt" == item29.Name)
								{
									activeSkillTrigger.trigger_special.taunt = new D3DTextInt();
									foreach (XmlNode item30 in item29)
									{
										if (!("my:ValueStruct" == item30.Name))
										{
											continue;
										}
										activeSkillTrigger.trigger_special.taunt.replace_tag = ((XmlElement)item30).GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item31 in item30)
										{
											if ("my:Value" == item31.Name)
											{
												activeSkillTrigger.trigger_special.taunt.values.Add(int.Parse(((XmlElement)item31).GetAttribute("my:ValueData").Trim()));
											}
										}
									}
								}
								else if ("my:Revive" == item29.Name)
								{
									activeSkillTrigger.trigger_special.revive = new TriggerSpecial.Revive();
									foreach (XmlNode item32 in item29)
									{
										if ("my:ReviveHP" == item32.Name)
										{
											activeSkillTrigger.trigger_special.revive.recover_hp = new D3DTextFloat();
											activeSkillTrigger.trigger_special.revive.recover_hp.replace_tag = ((XmlElement)item32).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item33 in item32)
											{
												if ("my:Value" == item33.Name)
												{
													activeSkillTrigger.trigger_special.revive.recover_hp.values.Add(float.Parse(((XmlElement)item33).GetAttribute("my:ValueData").Trim()));
												}
											}
										}
										else
										{
											if (!("my:ReviveMP" == item32.Name))
											{
												continue;
											}
											activeSkillTrigger.trigger_special.revive.recover_mp = new D3DTextFloat();
											activeSkillTrigger.trigger_special.revive.recover_mp.replace_tag = ((XmlElement)item32).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item34 in item32)
											{
												if ("my:Value" == item34.Name)
												{
													activeSkillTrigger.trigger_special.revive.recover_mp.values.Add(float.Parse(((XmlElement)item34).GetAttribute("my:ValueData").Trim()));
												}
											}
										}
									}
								}
								else if ("my:Dispel" == item29.Name)
								{
									activeSkillTrigger.trigger_special.dispel = new TriggerSpecial.Dispel();
									activeSkillTrigger.trigger_special.dispel.dispel_count = new D3DTextInt();
									foreach (XmlNode item35 in item29)
									{
										if (!("my:ValueStruct" == item35.Name))
										{
											continue;
										}
										activeSkillTrigger.trigger_special.dispel.dispel_count.replace_tag = ((XmlElement)item35).GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item36 in item35)
										{
											if ("my:Value" == item36.Name)
											{
												activeSkillTrigger.trigger_special.dispel.dispel_count.values.Add(int.Parse(((XmlElement)item36).GetAttribute("my:ValueData").Trim()));
											}
										}
									}
								}
								else
								{
									if (!("my:Summon" == item29.Name))
									{
										continue;
									}
									activeSkillTrigger.trigger_special.summon = new TriggerSpecial.Summon();
									activeSkillTrigger.trigger_special.summon.summon_id = new D3DTextString();
									activeSkillTrigger.trigger_special.summon.summon_level = new D3DTextInt();
									activeSkillTrigger.trigger_special.summon.summon_count = new D3DTextInt();
									activeSkillTrigger.trigger_special.summon.summon_life = new D3DTextFloat();
									activeSkillTrigger.trigger_special.summon.summon_effect = new List<string>();
									foreach (XmlNode item37 in item29)
									{
										if ("my:SummonList" == item37.Name)
										{
											activeSkillTrigger.trigger_special.summon.summon_id.replace_tag = ((XmlElement)item37).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item38 in item37)
											{
												if ("my:ListName" == item38.Name)
												{
													activeSkillTrigger.trigger_special.summon.summon_id.values.Add(((XmlElement)item38).GetAttribute("my:Name").Trim());
												}
											}
										}
										else if ("my:SummonLevel" == item37.Name)
										{
											activeSkillTrigger.trigger_special.summon.summon_level.replace_tag = ((XmlElement)item37).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item39 in item37)
											{
												if ("my:Value" == item39.Name)
												{
													activeSkillTrigger.trigger_special.summon.summon_level.values.Add(int.Parse(((XmlElement)item39).GetAttribute("my:ValueData").Trim()));
												}
											}
										}
										else if ("my:ValueStruct" == item37.Name)
										{
											activeSkillTrigger.trigger_special.summon.summon_count.replace_tag = ((XmlElement)item37).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item40 in item37)
											{
												if ("my:Value" == item40.Name)
												{
													activeSkillTrigger.trigger_special.summon.summon_count.values.Add(int.Parse(((XmlElement)item40).GetAttribute("my:ValueData").Trim()));
												}
											}
										}
										else if ("my:SummonLife" == item37.Name)
										{
											activeSkillTrigger.trigger_special.summon.summon_life.replace_tag = ((XmlElement)item37).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item41 in item37)
											{
												if ("my:Value" == item41.Name)
												{
													activeSkillTrigger.trigger_special.summon.summon_life.values.Add(float.Parse(((XmlElement)item41).GetAttribute("my:ValueData").Trim()));
												}
											}
										}
										else
										{
											if (!("my:SummonEffect" == item37.Name))
											{
												continue;
											}
											foreach (XmlNode item42 in item37)
											{
												if ("my:Value" == item42.Name)
												{
													activeSkillTrigger.trigger_special.summon.summon_effect.Add(((XmlElement)item42).GetAttribute("my:ValueData").Trim());
												}
											}
										}
									}
								}
							}
						}
						else if ("my:CrowdControl" == item5.Name)
						{
							foreach (XmlNode item43 in item5)
							{
								if (!("my:ControlConfig" == item43.Name))
								{
									continue;
								}
								XmlElement xmlElement9 = (XmlElement)item43;
								TriggerCrowdControl triggerCrowdControl = new TriggerCrowdControl();
								triggerCrowdControl.control_type = (TriggerCrowdControl.ControlType)int.Parse(xmlElement9.GetAttribute("my:ControlType").Trim());
								triggerCrowdControl.target_faction = (TargetFaction)int.Parse(xmlElement9.GetAttribute("my:Faction").Trim());
								triggerCrowdControl.awaken_effect = xmlElement9.GetAttribute("my:AwakenEffect").Trim();
								triggerCrowdControl.awaken_sfx = xmlElement9.GetAttribute("my:AwakenSfx").Trim();
								triggerCrowdControl.awaken_mount_point = xmlElement9.GetAttribute("my:AwakenMountPoint").Trim();
								triggerCrowdControl.effect = xmlElement9.GetAttribute("my:EffectName").Trim();
								triggerCrowdControl.sfx = xmlElement9.GetAttribute("my:EffectSfx").Trim();
								triggerCrowdControl.mount_point = xmlElement9.GetAttribute("my:MountPoint").Trim();
								foreach (XmlNode item44 in item43)
								{
									if ("my:Odds" == item44.Name)
									{
										triggerCrowdControl.odds = new D3DTextFloat();
										triggerCrowdControl.odds.replace_tag = ((XmlElement)item44).GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item45 in item44)
										{
											if ("my:Value" == item45.Name)
											{
												triggerCrowdControl.odds.values.Add(float.Parse(((XmlElement)item45).GetAttribute("my:ValueData").Trim()));
											}
										}
									}
									else
									{
										if (!("my:ControlTime" == item44.Name))
										{
											continue;
										}
										triggerCrowdControl.time = new D3DTextFloat();
										triggerCrowdControl.time.replace_tag = ((XmlElement)item44).GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item46 in item44)
										{
											if ("my:Value" == item46.Name)
											{
												triggerCrowdControl.time.values.Add(float.Parse(((XmlElement)item46).GetAttribute("my:ValueData").Trim()));
											}
										}
									}
								}
								if (activeSkillTrigger.trigger_crowd_control == null)
								{
									activeSkillTrigger.trigger_crowd_control = new List<TriggerCrowdControl>();
								}
								activeSkillTrigger.trigger_crowd_control.Add(triggerCrowdControl);
							}
						}
						else
						{
							if (!("my:Buff" == item5.Name))
							{
								continue;
							}
							foreach (XmlNode item47 in item5)
							{
								if (!("my:BuffConfig" == item47.Name))
								{
									continue;
								}
								TriggerBuff triggerBuff = new TriggerBuff();
								triggerBuff.buff_type = (TriggerBuff.BuffType)int.Parse(((XmlElement)item47).GetAttribute("my:StatusType").Trim());
								foreach (XmlNode item48 in item47)
								{
									if ("my:Odds" == item48.Name)
									{
										triggerBuff.odds = new D3DTextFloat();
										triggerBuff.odds.replace_tag = ((XmlElement)item48).GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item49 in item48)
										{
											if ("my:Value" == item49.Name)
											{
												triggerBuff.odds.values.Add(float.Parse(((XmlElement)item49).GetAttribute("my:ValueData").Trim()));
											}
										}
									}
									else if ("my:StatusTime" == item48.Name)
									{
										triggerBuff.time = new D3DTextFloat();
										triggerBuff.time.replace_tag = ((XmlElement)item48).GetAttribute("my:StrIndex").Trim();
										foreach (XmlNode item50 in item48)
										{
											if ("my:Value" == item50.Name)
											{
												triggerBuff.time.values.Add(float.Parse(((XmlElement)item50).GetAttribute("my:ValueData").Trim()));
											}
										}
									}
									else if ("my:BuffEffect" == item48.Name)
									{
										triggerBuff.awaken_effect = ((XmlElement)item48).GetAttribute("my:AwakenEffect").Trim();
										triggerBuff.awaken_sfx = ((XmlElement)item48).GetAttribute("my:AwakenSfx").Trim();
										triggerBuff.awaken_mount_point = ((XmlElement)item48).GetAttribute("my:AwakenMountPoint").Trim();
										triggerBuff.effect = ((XmlElement)item48).GetAttribute("my:EffectName").Trim();
										triggerBuff.sfx = ((XmlElement)item48).GetAttribute("my:EffectSfx").Trim();
										triggerBuff.mount_point = ((XmlElement)item48).GetAttribute("my:MountPoint").Trim();
									}
									else if ("my:BuffExtra" == item48.Name)
									{
										triggerBuff.buff_property = (TriggerBuff.Property)int.Parse(((XmlElement)item48).GetAttribute("my:ValueType").Trim());
										foreach (XmlNode item51 in item48)
										{
											if (!("my:ValueStruct" == item51.Name))
											{
												continue;
											}
											triggerBuff.buff_value = new D3DTextFloat();
											triggerBuff.buff_value.replace_tag = ((XmlElement)item51).GetAttribute("my:StrIndex").Trim();
											foreach (XmlNode item52 in item51)
											{
												if ("my:Value" == item52.Name)
												{
													triggerBuff.buff_value.values.Add(float.Parse(((XmlElement)item52).GetAttribute("my:ValueData").Trim()));
												}
											}
										}
									}
									else if ("my:OutputBuff" == item48.Name)
									{
										triggerBuff.target_faction = (TargetFaction)int.Parse(((XmlElement)item48).GetAttribute("my:Faction").Trim());
									}
									else
									{
										if (!("my:AureoleConfig" == item48.Name))
										{
											continue;
										}
										XmlElement xmlElement10 = (XmlElement)item48;
										triggerBuff.aureole_config = new AureoleConfig();
										triggerBuff.aureole_config.aureole_faction = (AureoleConfig.AureoleFaction)int.Parse(xmlElement10.GetAttribute("my:AureoleFaction").Trim());
										triggerBuff.aureole_config.aureole_origin = (AureoleConfig.AureoleOrigin)int.Parse(xmlElement10.GetAttribute("my:AureoleOrigin").Trim());
										triggerBuff.aureole_config.include_puppet_radius = bool.Parse(xmlElement10.GetAttribute("my:AddPuppetRadius").Trim());
										triggerBuff.aureole_config.bind = bool.Parse(xmlElement10.GetAttribute("my:BindPosition").Trim());
										triggerBuff.aureole_config.aureole_effect = xmlElement10.GetAttribute("my:EffectName").Trim();
										triggerBuff.aureole_config.aureole_sfx = xmlElement10.GetAttribute("my:EffectSfx").Trim();
										triggerBuff.aureole_config.mount_point = xmlElement10.GetAttribute("my:MountPoint").Trim();
										foreach (XmlNode item53 in item48)
										{
											if ("my:AureolaRadius" == item53.Name)
											{
												triggerBuff.aureole_config.aureole_radius = new D3DTextFloat();
												triggerBuff.aureole_config.aureole_radius.replace_tag = ((XmlElement)item53).GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item54 in item53)
												{
													if ("my:Value" == item54.Name)
													{
														triggerBuff.aureole_config.aureole_radius.values.Add(float.Parse(((XmlElement)item54).GetAttribute("my:ValueData").Trim()));
													}
												}
											}
											else
											{
												if (!("my:DotTime" == item53.Name))
												{
													continue;
												}
												triggerBuff.aureole_config.aureole_time = new D3DTextFloat();
												triggerBuff.aureole_config.aureole_time.replace_tag = ((XmlElement)item53).GetAttribute("my:StrIndex").Trim();
												foreach (XmlNode item55 in item53)
												{
													if ("my:Value" == item55.Name)
													{
														triggerBuff.aureole_config.aureole_time.values.Add(float.Parse(((XmlElement)item55).GetAttribute("my:ValueData").Trim()));
													}
												}
											}
										}
									}
								}
								if (activeSkillTrigger.trigger_buff == null)
								{
									activeSkillTrigger.trigger_buff = new List<TriggerBuff>();
								}
								activeSkillTrigger.trigger_buff.Add(triggerBuff);
							}
						}
					}
					if (d3DActiveSkill.skill_triggers == null)
					{
						d3DActiveSkill.skill_triggers = new List<ActiveSkillTrigger>();
					}
					d3DActiveSkill.skill_triggers.Add(activeSkillTrigger);
				}
				else
				{
					if (!("my:Description" == item3.Name))
					{
						continue;
					}
					foreach (XmlNode item56 in item3)
					{
						if ("my:Content" == item56.Name && item56.ChildNodes[0] != null)
						{
							d3DActiveSkill.description.Add(item56.ChildNodes[0].Value);
						}
					}
				}
			}
			d3DActiveSkill.CreateReplaceTags();
			D3DActiveSkillManager.Add(d3DActiveSkill.skill_id, d3DActiveSkill);
		}
		if (!diagnose)
		{
			return;
		}
		string text3 = text;
		text = text3 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item57 in list)
		{
			text = text + item57 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DActiveSkillsBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DActiveSkillFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public bool CheckActiveSkillID(string skill_id)
	{
		if (D3DActiveSkillManager.ContainsKey(skill_id))
		{
			return true;
		}
		return false;
	}

	public D3DActiveSkill GetActiveSkill(string skill_id)
	{
		if (!CheckActiveSkillID(skill_id))
		{
			return null;
		}
		return D3DActiveSkillManager[skill_id];
	}

	public void LoadD3DClassFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DClass File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "]$XUQ(OVTmM*gO,M");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DClass d3DClass = new D3DClass();
			foreach (XmlAttribute attribute13 in item.Attributes)
			{
				if ("my:ClassID" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_id = attribute13.Value;
					}
				}
				else if ("my:ClassName" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_name = attribute13.Value;
					}
				}
				else if ("my:ClassType" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_main_type = (D3DClass.ClassType)int.Parse(attribute13.Value);
					}
				}
				else if ("my:ClassSubType" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_sub_type = (D3DClass.ClassType)int.Parse(attribute13.Value);
					}
				}
				else if ("my:Editable" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.editable = bool.Parse(attribute13.Value);
					}
				}
				else if ("my:DefaultWeapon" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.default_weapon = attribute13.Value;
					}
				}
				else if ("my:DefaultArmor" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.default_armor = attribute13.Value;
					}
				}
				else if ("my:SingleAnimation" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_animations[0] = attribute13.Value;
					}
				}
				else if ("my:DoubleAnimation" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_animations[1] = attribute13.Value;
					}
				}
				else if ("my:DualAnimation" == attribute13.Name)
				{
					if (string.Empty != attribute13.Value)
					{
						d3DClass.class_animations[2] = attribute13.Value;
					}
				}
				else if ("my:MPSP" == attribute13.Name && string.Empty != attribute13.Value)
				{
					if (int.Parse(attribute13.Value) == 0)
					{
						d3DClass.sp_class = false;
					}
					else
					{
						d3DClass.sp_class = true;
					}
				}
			}
			foreach (XmlNode item2 in item)
			{
				if ("my:PlayerHatred" == item2.Name)
				{
					foreach (XmlAttribute attribute14 in item2.Attributes)
					{
						if ("my:HatredSend" == attribute14.Name)
						{
							if (string.Empty != attribute14.Value)
							{
								d3DClass.player_hatred_send = int.Parse(attribute14.Value);
							}
						}
						else if ("my:HatredResist" == attribute14.Name && string.Empty != attribute14.Value)
						{
							d3DClass.player_hatred_resist = int.Parse(attribute14.Value);
						}
					}
				}
				else if ("my:EnemyHatred" == item2.Name)
				{
					foreach (XmlAttribute attribute15 in item2.Attributes)
					{
						if ("my:HatredSend" == attribute15.Name)
						{
							if (string.Empty != attribute15.Value)
							{
								d3DClass.enemy_hatred_send = int.Parse(attribute15.Value);
							}
						}
						else if ("my:HatredResist" == attribute15.Name && string.Empty != attribute15.Value)
						{
							d3DClass.enemy_hatred_resist = int.Parse(attribute15.Value);
						}
					}
				}
				else if ("my:BasicSkill" == item2.Name)
				{
					foreach (XmlNode item3 in item2)
					{
						if ("my:DefaultBasicConfig" == item3.Name)
						{
							d3DClass.basic_skill_id[0] = ((XmlElement)item3).GetAttribute("my:DefaultBasic").Trim();
							d3DClass.basic_attack1_frames[0] = new List<List<int>>();
							d3DClass.basic_attack2_frames[0] = new List<List<int>>();
							foreach (XmlNode item4 in item3)
							{
								if ("my:Attack1FrameConfig" == item4.Name)
								{
									string text3 = ((XmlElement)item4).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text3)
									{
										string[] array = text3.Split(',');
										if (array.Length > 0)
										{
											List<int> list2 = new List<int>();
											string[] array2 = array;
											foreach (string s in array2)
											{
												list2.Add(int.Parse(s));
											}
											d3DClass.basic_attack1_frames[0].Add(list2);
										}
										else
										{
											d3DClass.basic_attack1_frames[0].Add(null);
										}
									}
									else
									{
										d3DClass.basic_attack1_frames[0].Add(null);
									}
								}
								else
								{
									if (!("my:Attack2FrameConfig" == item4.Name))
									{
										continue;
									}
									string text4 = ((XmlElement)item4).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text4)
									{
										string[] array3 = text4.Split(',');
										if (array3.Length > 0)
										{
											List<int> list3 = new List<int>();
											string[] array4 = array3;
											foreach (string s2 in array4)
											{
												list3.Add(int.Parse(s2));
											}
											d3DClass.basic_attack2_frames[0].Add(list3);
										}
										else
										{
											d3DClass.basic_attack2_frames[0].Add(null);
										}
									}
									else
									{
										d3DClass.basic_attack2_frames[0].Add(null);
									}
								}
							}
							if (d3DClass.basic_attack1_frames[0].Count == 0)
							{
								d3DClass.basic_attack1_frames[0] = null;
							}
							if (d3DClass.basic_attack2_frames[0].Count == 0)
							{
								d3DClass.basic_attack2_frames[0] = null;
							}
						}
						else if ("my:DoubleBasicConfig" == item3.Name)
						{
							d3DClass.basic_skill_id[1] = ((XmlElement)item3).GetAttribute("my:DoubleBasic").Trim();
							d3DClass.basic_attack1_frames[1] = new List<List<int>>();
							d3DClass.basic_attack2_frames[1] = new List<List<int>>();
							foreach (XmlNode item5 in item3)
							{
								if ("my:Attack1FrameConfig" == item5.Name)
								{
									string text5 = ((XmlElement)item5).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text5)
									{
										string[] array5 = text5.Split(',');
										if (array5.Length > 0)
										{
											List<int> list4 = new List<int>();
											string[] array6 = array5;
											foreach (string s3 in array6)
											{
												list4.Add(int.Parse(s3));
											}
											d3DClass.basic_attack1_frames[1].Add(list4);
										}
										else
										{
											d3DClass.basic_attack1_frames[1].Add(null);
										}
									}
									else
									{
										d3DClass.basic_attack1_frames[1].Add(null);
									}
								}
								else
								{
									if (!("my:Attack2FrameConfig" == item5.Name))
									{
										continue;
									}
									string text6 = ((XmlElement)item5).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text6)
									{
										string[] array7 = text6.Split(',');
										if (array7.Length > 0)
										{
											List<int> list5 = new List<int>();
											string[] array8 = array7;
											foreach (string s4 in array8)
											{
												list5.Add(int.Parse(s4));
											}
											d3DClass.basic_attack2_frames[1].Add(list5);
										}
										else
										{
											d3DClass.basic_attack2_frames[1].Add(null);
										}
									}
									else
									{
										d3DClass.basic_attack2_frames[1].Add(null);
									}
								}
							}
							if (d3DClass.basic_attack1_frames[1].Count == 0)
							{
								d3DClass.basic_attack1_frames[1] = null;
							}
							if (d3DClass.basic_attack2_frames[1].Count == 0)
							{
								d3DClass.basic_attack2_frames[1] = null;
							}
						}
						else
						{
							if (!("my:DualBasicConfig" == item3.Name))
							{
								continue;
							}
							d3DClass.basic_skill_id[2] = ((XmlElement)item3).GetAttribute("my:DualBasic").Trim();
							d3DClass.basic_attack1_frames[2] = new List<List<int>>();
							d3DClass.basic_attack2_frames[2] = new List<List<int>>();
							foreach (XmlNode item6 in item3)
							{
								if ("my:Attack1FrameConfig" == item6.Name)
								{
									string text7 = ((XmlElement)item6).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text7)
									{
										string[] array9 = text7.Split(',');
										if (array9.Length > 0)
										{
											List<int> list6 = new List<int>();
											string[] array10 = array9;
											foreach (string s5 in array10)
											{
												list6.Add(int.Parse(s5));
											}
											d3DClass.basic_attack1_frames[2].Add(list6);
										}
										else
										{
											d3DClass.basic_attack1_frames[2].Add(null);
										}
									}
									else
									{
										d3DClass.basic_attack1_frames[2].Add(null);
									}
								}
								else
								{
									if (!("my:Attack2FrameConfig" == item6.Name))
									{
										continue;
									}
									string text8 = ((XmlElement)item6).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text8)
									{
										string[] array11 = text8.Split(',');
										if (array11.Length > 0)
										{
											List<int> list7 = new List<int>();
											string[] array12 = array11;
											foreach (string s6 in array12)
											{
												list7.Add(int.Parse(s6));
											}
											d3DClass.basic_attack2_frames[2].Add(list7);
										}
										else
										{
											d3DClass.basic_attack2_frames[2].Add(null);
										}
									}
									else
									{
										d3DClass.basic_attack2_frames[2].Add(null);
									}
								}
							}
							if (d3DClass.basic_attack1_frames[2].Count == 0)
							{
								d3DClass.basic_attack1_frames[2] = null;
							}
							if (d3DClass.basic_attack2_frames[2].Count == 0)
							{
								d3DClass.basic_attack2_frames[2] = null;
							}
						}
					}
				}
				else if ("my:ActiveSkill" == item2.Name)
				{
					foreach (XmlNode item7 in item2)
					{
						if (!("my:ActiveConfig" == item7.Name))
						{
							continue;
						}
						D3DClassActiveSkillStatus d3DClassActiveSkillStatus = new D3DClassActiveSkillStatus();
						foreach (XmlAttribute attribute16 in item7.Attributes)
						{
							if ("my:SkillID" == attribute16.Name)
							{
								if (string.Empty != attribute16.Value)
								{
									d3DClassActiveSkillStatus.skill_id = attribute16.Value;
								}
							}
							else if ("my:CastTime" == attribute16.Name)
							{
								if (string.Empty != attribute16.Value)
								{
									d3DClassActiveSkillStatus.cast_time = float.Parse(attribute16.Value);
								}
							}
							else if ("my:MasterLevel" == attribute16.Name && string.Empty != attribute16.Value)
							{
								d3DClassActiveSkillStatus.unlock_level = int.Parse(attribute16.Value);
							}
						}
						foreach (XmlNode item8 in item7)
						{
							if ("my:DefaultAnimationConfig" == item8.Name)
							{
								int value = int.Parse(((XmlElement)item8).GetAttribute("my:ClipIndex").Trim());
								d3DClassActiveSkillStatus.animation_clip_index[0] = new D3DInt(value);
								d3DClassActiveSkillStatus.clip_frames[0] = new List<List<int>>();
								foreach (XmlNode item9 in item8)
								{
									if (!("my:FrameConfig" == item9.Name))
									{
										continue;
									}
									string text9 = ((XmlElement)item9).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text9)
									{
										string[] array13 = text9.Split(',');
										if (array13.Length > 0)
										{
											List<int> list8 = new List<int>();
											string[] array14 = array13;
											foreach (string s7 in array14)
											{
												list8.Add(int.Parse(s7));
											}
											d3DClassActiveSkillStatus.clip_frames[0].Add(list8);
										}
										else
										{
											d3DClassActiveSkillStatus.clip_frames[0].Add(null);
										}
									}
									else
									{
										d3DClassActiveSkillStatus.clip_frames[0].Add(null);
									}
								}
								if (d3DClassActiveSkillStatus.clip_frames[0].Count == 0)
								{
									d3DClassActiveSkillStatus.clip_frames[0] = null;
								}
							}
							else if ("my:DoubleAnimationConfig" == item8.Name)
							{
								int value2 = int.Parse(((XmlElement)item8).GetAttribute("my:ClipIndex").Trim());
								d3DClassActiveSkillStatus.animation_clip_index[1] = new D3DInt(value2);
								d3DClassActiveSkillStatus.clip_frames[1] = new List<List<int>>();
								foreach (XmlNode item10 in item8)
								{
									if (!("my:FrameConfig" == item10.Name))
									{
										continue;
									}
									string text10 = ((XmlElement)item10).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text10)
									{
										string[] array15 = text10.Split(',');
										if (array15.Length > 0)
										{
											List<int> list9 = new List<int>();
											string[] array16 = array15;
											foreach (string s8 in array16)
											{
												list9.Add(int.Parse(s8));
											}
											d3DClassActiveSkillStatus.clip_frames[1].Add(list9);
										}
										else
										{
											d3DClassActiveSkillStatus.clip_frames[1].Add(null);
										}
									}
									else
									{
										d3DClassActiveSkillStatus.clip_frames[1].Add(null);
									}
								}
								if (d3DClassActiveSkillStatus.clip_frames[1].Count == 0)
								{
									d3DClassActiveSkillStatus.clip_frames[1] = null;
								}
							}
							else if ("my:DualAnimationConfig" == item8.Name)
							{
								int value3 = int.Parse(((XmlElement)item8).GetAttribute("my:ClipIndex").Trim());
								d3DClassActiveSkillStatus.animation_clip_index[2] = new D3DInt(value3);
								d3DClassActiveSkillStatus.clip_frames[2] = new List<List<int>>();
								foreach (XmlNode item11 in item8)
								{
									if (!("my:FrameConfig" == item11.Name))
									{
										continue;
									}
									string text11 = ((XmlElement)item11).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text11)
									{
										string[] array17 = text11.Split(',');
										if (array17.Length > 0)
										{
											List<int> list10 = new List<int>();
											string[] array18 = array17;
											foreach (string s9 in array18)
											{
												list10.Add(int.Parse(s9));
											}
											d3DClassActiveSkillStatus.clip_frames[2].Add(list10);
										}
										else
										{
											d3DClassActiveSkillStatus.clip_frames[2].Add(null);
										}
									}
									else
									{
										d3DClassActiveSkillStatus.clip_frames[2].Add(null);
									}
								}
								if (d3DClassActiveSkillStatus.clip_frames[2].Count == 0)
								{
									d3DClassActiveSkillStatus.clip_frames[2] = null;
								}
							}
							else if ("my:MasterCost" == item8.Name)
							{
								if (item8.ChildNodes[0] != null)
								{
									d3DClassActiveSkillStatus.upgrade_cost.Add(int.Parse(item8.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterCrystal" == item8.Name)
							{
								if (item8.ChildNodes[0] != null)
								{
									d3DClassActiveSkillStatus.upgrade_crystal.Add(int.Parse(item8.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterSpace" == item8.Name && item8.ChildNodes[0] != null)
							{
								d3DClassActiveSkillStatus.upgrade_difference.Add(int.Parse(item8.ChildNodes[0].Value));
							}
						}
						if (string.Empty != d3DClassActiveSkillStatus.skill_id && !d3DClass.active_skill_id_list.ContainsKey(d3DClassActiveSkillStatus.skill_id))
						{
							d3DClass.active_skill_id_list.Add(d3DClassActiveSkillStatus.skill_id, d3DClassActiveSkillStatus);
						}
					}
				}
				else if ("my:PassiveSkill" == item2.Name)
				{
					foreach (XmlNode item12 in item2)
					{
						if (!("my:PassiveConfig" == item12.Name))
						{
							continue;
						}
						D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = new D3DClassPassiveSkillStatus();
						foreach (XmlAttribute attribute17 in item12.Attributes)
						{
							if ("my:Deprecate" == attribute17.Name)
							{
								d3DClassPassiveSkillStatus._bDeprecated = bool.Parse(attribute17.Value);
							}
							else if ("my:SkillID" == attribute17.Name)
							{
								if (string.Empty != attribute17.Value)
								{
									d3DClassPassiveSkillStatus.skill_id = attribute17.Value;
								}
							}
							else if ("my:MasterLevel" == attribute17.Name && string.Empty != attribute17.Value)
							{
								d3DClassPassiveSkillStatus.unlock_level = int.Parse(attribute17.Value);
							}
						}
						foreach (XmlNode item13 in item12)
						{
							if ("my:MasterCost" == item13.Name && item13.ChildNodes[0] != null)
							{
								d3DClassPassiveSkillStatus.upgrade_cost.Add(int.Parse(item13.ChildNodes[0].Value));
							}
							if ("my:MasterCrystal" == item13.Name)
							{
								if (item13.ChildNodes[0] != null)
								{
									d3DClassPassiveSkillStatus.upgrade_crystal.Add(int.Parse(item13.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterSpace" == item13.Name && item13.ChildNodes[0] != null)
							{
								d3DClassPassiveSkillStatus.upgrade_difference.Add(int.Parse(item13.ChildNodes[0].Value));
							}
						}
						if (string.Empty != d3DClassPassiveSkillStatus.skill_id && !d3DClass.passive_skill_id_list.ContainsKey(d3DClassPassiveSkillStatus.skill_id))
						{
							d3DClass.passive_skill_id_list.Add(d3DClassPassiveSkillStatus.skill_id, d3DClassPassiveSkillStatus);
						}
					}
				}
				else
				{
					if (!("my:ClassTalent" == item2.Name))
					{
						continue;
					}
					foreach (XmlNode item14 in item2)
					{
						if ("my:StrTalent" == item14.Name)
						{
							string attribute = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[0] = float.Parse(attribute);
							attribute = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array19 = attribute.Split(',');
							d3DClass.class_talent.ability_growth[0, 0] = float.Parse(array19[0].Trim());
							d3DClass.class_talent.ability_growth[0, 1] = float.Parse(array19[1].Trim());
							d3DClass.class_talent.ability_growth[0, 2] = float.Parse(array19[2].Trim());
						}
						else if ("my:AgiTalent" == item14.Name)
						{
							string attribute2 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[1] = float.Parse(attribute2);
							attribute2 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array20 = attribute2.Split(',');
							d3DClass.class_talent.ability_growth[1, 0] = float.Parse(array20[0].Trim());
							d3DClass.class_talent.ability_growth[1, 1] = float.Parse(array20[1].Trim());
							d3DClass.class_talent.ability_growth[1, 2] = float.Parse(array20[2].Trim());
						}
						else if ("my:SpiTalent" == item14.Name)
						{
							string attribute3 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[2] = float.Parse(attribute3);
							attribute3 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array21 = attribute3.Split(',');
							d3DClass.class_talent.ability_growth[2, 0] = float.Parse(array21[0].Trim());
							d3DClass.class_talent.ability_growth[2, 1] = float.Parse(array21[1].Trim());
							d3DClass.class_talent.ability_growth[2, 2] = float.Parse(array21[2].Trim());
						}
						else if ("my:StaTalent" == item14.Name)
						{
							string attribute4 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[3] = float.Parse(attribute4);
							attribute4 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array22 = attribute4.Split(',');
							d3DClass.class_talent.ability_growth[3, 0] = float.Parse(array22[0].Trim());
							d3DClass.class_talent.ability_growth[3, 1] = float.Parse(array22[1].Trim());
							d3DClass.class_talent.ability_growth[3, 2] = float.Parse(array22[2].Trim());
						}
						else if ("my:IntTalent" == item14.Name)
						{
							string attribute5 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[4] = float.Parse(attribute5);
							attribute5 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array23 = attribute5.Split(',');
							d3DClass.class_talent.ability_growth[4, 0] = float.Parse(array23[0].Trim());
							d3DClass.class_talent.ability_growth[4, 1] = float.Parse(array23[1].Trim());
							d3DClass.class_talent.ability_growth[4, 2] = float.Parse(array23[2].Trim());
						}
						else if ("my:HPTalent" == item14.Name)
						{
							string attribute6 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[5] = float.Parse(attribute6);
							attribute6 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array24 = attribute6.Split(',');
							d3DClass.class_talent.ability_growth[5, 0] = float.Parse(array24[0].Trim());
							d3DClass.class_talent.ability_growth[5, 1] = float.Parse(array24[1].Trim());
							d3DClass.class_talent.ability_growth[5, 2] = float.Parse(array24[2].Trim());
						}
						else if ("my:MPTalent" == item14.Name)
						{
							string attribute7 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[6] = float.Parse(attribute7);
							attribute7 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array25 = attribute7.Split(',');
							d3DClass.class_talent.ability_growth[6, 0] = float.Parse(array25[0].Trim());
							d3DClass.class_talent.ability_growth[6, 1] = float.Parse(array25[1].Trim());
							d3DClass.class_talent.ability_growth[6, 2] = float.Parse(array25[2].Trim());
						}
						else if ("my:Armor" == item14.Name)
						{
							string attribute8 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[7] = float.Parse(attribute8);
							attribute8 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array26 = attribute8.Split(',');
							d3DClass.class_talent.ability_growth[7, 0] = float.Parse(array26[0].Trim());
							d3DClass.class_talent.ability_growth[7, 1] = float.Parse(array26[1].Trim());
							d3DClass.class_talent.ability_growth[7, 2] = float.Parse(array26[2].Trim());
						}
						else if ("my:AtkSpdTalent" == item14.Name)
						{
							string attribute9 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[10] = float.Parse(attribute9);
						}
						else if ("my:MoveSpdTalent" == item14.Name)
						{
							string attribute10 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[11] = float.Parse(attribute10);
						}
						else if ("my:PhyDmgTalent" == item14.Name)
						{
							string attribute11 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[8] = float.Parse(attribute11);
							attribute11 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array27 = attribute11.Split(',');
							d3DClass.class_talent.ability_growth[8, 0] = float.Parse(array27[0].Trim());
							d3DClass.class_talent.ability_growth[8, 1] = float.Parse(array27[1].Trim());
							d3DClass.class_talent.ability_growth[8, 2] = float.Parse(array27[2].Trim());
						}
						else if ("my:MagDmgTalent" == item14.Name)
						{
							string attribute12 = ((XmlElement)item14).GetAttribute("my:Talent");
							d3DClass.class_talent.talent_ability[9] = float.Parse(attribute12);
							attribute12 = ((XmlElement)item14).GetAttribute("my:Growth").Trim();
							string[] array28 = attribute12.Split(',');
							d3DClass.class_talent.ability_growth[9, 0] = float.Parse(array28[0].Trim());
							d3DClass.class_talent.ability_growth[9, 1] = float.Parse(array28[1].Trim());
							d3DClass.class_talent.ability_growth[9, 2] = float.Parse(array28[2].Trim());
						}
					}
				}
			}
			if (string.Empty != d3DClass.class_id)
			{
				D3DClassManager.Add(d3DClass.class_id, d3DClass);
			}
			else if (diagnose)
			{
				num++;
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text12 = text;
		text = text12 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item15 in list)
		{
			text = text + item15 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DClassesBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DClassFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public void OutputD3DClasses(int count)
	{
		int num = 1;
		foreach (string key in D3DClassManager.Keys)
		{
			if (num > count && count > 0)
			{
				break;
			}
			string empty = string.Empty;
			D3DClass d3DClass = D3DClassManager[key];
			string text = empty;
			empty = text + "=================No." + num + "=================\n";
			empty = empty + "Class ID:" + d3DClass.class_id + "\n";
			empty = empty + "Class Name:" + d3DClass.class_name + "\n";
			text = empty;
			empty = string.Concat(text, "Class Main Type:", d3DClass.class_main_type, "\n");
			text = empty;
			empty = string.Concat(text, "Class Sub Type:", d3DClass.class_sub_type, "\n");
			text = empty;
			empty = text + "Editable:" + d3DClass.editable + "\n";
			empty = empty + "Default Weapon:" + d3DClass.default_weapon + "\n";
			empty = empty + "Default Armor:" + d3DClass.default_armor + "\n";
			empty = empty + "Baisc Animation:" + d3DClass.class_animations[0] + "\n";
			empty = empty + "Two-Hand Animation:" + d3DClass.class_animations[1] + "\n";
			empty = empty + "Dual Animation:" + d3DClass.class_animations[2] + "\n";
			text = empty;
			empty = text + "Player Hatred Send:" + d3DClass.player_hatred_send + "\n";
			text = empty;
			empty = text + "Player Hatred Resist:" + d3DClass.player_hatred_resist + "\n";
			text = empty;
			empty = text + "Enemy Hatred Send:" + d3DClass.enemy_hatred_send + "\n";
			text = empty;
			empty = text + "Enemy Hatred Resist:" + d3DClass.enemy_hatred_resist + "\n";
			empty = empty + "Default Basic Skill:" + d3DClass.basic_skill_id[0] + "\n";
			empty = empty + "Two Hand Basic Skill:" + d3DClass.basic_skill_id[1] + "\n";
			empty = empty + "Dual Basic Skill:" + d3DClass.basic_skill_id[2] + "\n";
			empty += "Talent Skills:\n";
			foreach (string key2 in d3DClass.active_skill_id_list.Keys)
			{
				D3DClassActiveSkillStatus d3DClassActiveSkillStatus = d3DClass.active_skill_id_list[key2];
				empty = empty + "-----Active Skill ID:" + d3DClassActiveSkillStatus.skill_id + "\n";
				text = empty;
				empty = text + "-----Cast Time:" + d3DClassActiveSkillStatus.cast_time + "\n";
				text = empty;
				empty = text + "-----Unlock Level:" + d3DClassActiveSkillStatus.unlock_level + "\n";
				empty += "-----Upgrade Diff:\n";
				foreach (int item in d3DClassActiveSkillStatus.upgrade_difference)
				{
					text = empty;
					empty = text + ">>>>>" + item + "<<<<<\n";
				}
				empty += "-----Costs:\n";
				foreach (int item2 in d3DClassActiveSkillStatus.upgrade_cost)
				{
					text = empty;
					empty = text + ">>>>>" + item2 + "<<<<<\n";
				}
			}
			foreach (string key3 in d3DClass.passive_skill_id_list.Keys)
			{
				D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = d3DClass.passive_skill_id_list[key3];
				empty = empty + "-----Passive Skill ID:" + d3DClassPassiveSkillStatus.skill_id + "\n";
				text = empty;
				empty = text + "-----Unlock Level:" + d3DClassPassiveSkillStatus.unlock_level + "\n";
				empty += "-----Upgrade Diff:\n";
				foreach (int item3 in d3DClassPassiveSkillStatus.upgrade_difference)
				{
					text = empty;
					empty = text + ">>>>>" + item3 + "<<<<<\n";
				}
				empty += "-----Costs:\n";
				foreach (int item4 in d3DClassPassiveSkillStatus.upgrade_cost)
				{
					text = empty;
					empty = text + ">>>>>" + item4 + "<<<<<\n";
				}
			}
			num++;
		}
	}

	public bool CheckClassID(string class_id)
	{
		if (D3DClassManager.ContainsKey(class_id))
		{
			return true;
		}
		return false;
	}

	public D3DClass GetClass(string class_id)
	{
		if (!CheckClassID(class_id))
		{
			return null;
		}
		return D3DClassManager[class_id];
	}

	public D3DClass GetClassClone(string class_id)
	{
		if (!CheckClassID(class_id))
		{
			return null;
		}
		return D3DClassManager[class_id].Clone();
	}

	public List<D3DEquipment> GetAllEquipmentInLevelRange(int nLowLevel, int nHighLevel)
	{
		List<D3DEquipment> list = new List<D3DEquipment>();
		foreach (string key in D3DEquipmentManager.Keys)
		{
			if (Instance.D3DEquipmentManager[key].require_level >= nLowLevel && Instance.D3DEquipmentManager[key].require_level <= nHighLevel)
			{
				list.Add(Instance.D3DEquipmentManager[key]);
			}
		}
		return list;
	}

	public void LoadD3DEquipmentFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DEquipment File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "s1f96TCw5+QfDpWs");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item2 in documentElement)
		{
			D3DEquipment d3DEquipment = new D3DEquipment();
			foreach (XmlAttribute attribute in item2.Attributes)
			{
				if ("my:EquipID" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.equipment_id = attribute.Value;
					}
				}
				else if ("my:EquipName" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.equipment_name = attribute.Value;
					}
				}
				else if ("my:Grade" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.equipment_grade = (D3DEquipment.EquipmentGrade)int.Parse(attribute.Value);
					}
				}
				else if ("my:Type" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.equipment_type = (D3DEquipment.EquipmentType)int.Parse(attribute.Value);
					}
				}
				else if ("my:Class" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.equipment_class = (D3DEquipment.EquipmentClass)int.Parse(attribute.Value);
					}
				}
				else if ("my:RequireLv" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.require_level = int.Parse(attribute.Value);
					}
				}
				else if ("my:SuitID" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.set_item_id = attribute.Value;
					}
				}
				else if ("my:BuyPrice" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DEquipment.buy_price = int.Parse(attribute.Value);
					}
				}
				else if ("my:CrystalPrice" == attribute.Name && string.Empty != attribute.Value)
				{
					d3DEquipment.buy_price_crystal = int.Parse(attribute.Value);
				}
			}
			foreach (XmlNode item3 in item2)
			{
				if ("my:Features" == item3.Name)
				{
					foreach (XmlAttribute attribute2 in item3.Attributes)
					{
						if ("my:UseIcon" == attribute2.Name)
						{
							if (string.Empty != attribute2.Value)
							{
								d3DEquipment.use_icon = attribute2.Value;
							}
						}
						else if ("my:UseModel" == attribute2.Name && string.Empty != attribute2.Value)
						{
							d3DEquipment.use_model = attribute2.Value;
						}
					}
					foreach (XmlNode item4 in item3)
					{
						if ("my:UseTexture" == item4.Name)
						{
							if (item4.ChildNodes[0] != null)
							{
								d3DEquipment.use_textures.Add(item4.ChildNodes[0].Value);
							}
						}
						else if ("my:EFX" == item4.Name)
						{
							string[] item = new string[2]
							{
								((XmlElement)item4).GetAttribute("my:LinkPt").Trim(),
								((XmlElement)item4).GetAttribute("my:Effect").Trim()
							};
							d3DEquipment.equipment_effects.Add(item);
						}
					}
				}
				else if ("my:Popedom" == item3.Name)
				{
					foreach (XmlNode item5 in item3)
					{
						if ("my:PopedomClass" == item5.Name && item5.ChildNodes[0] != null && string.Empty != item5.ChildNodes[0].Value)
						{
							d3DEquipment.explicit_popedom_classes.Add(item5.ChildNodes[0].Value);
						}
					}
				}
				else if ("my:BasicProperty" == item3.Name)
				{
					foreach (XmlNode item6 in item3)
					{
						if (!("my:DataDetial" == item6.Name))
						{
							continue;
						}
						bool flag = false;
						D3DEquipment.BasicAttribute key = D3DEquipment.BasicAttribute.PYH_DAMAGE_MIN;
						float value = 0f;
						foreach (XmlAttribute attribute3 in item6.Attributes)
						{
							if ("my:DataType" == attribute3.Name)
							{
								if (string.Empty != attribute3.Value)
								{
									key = (D3DEquipment.BasicAttribute)int.Parse(attribute3.Value);
									flag = true;
								}
							}
							else if ("my:DataValue" == attribute3.Name && string.Empty != attribute3.Value)
							{
								value = float.Parse(attribute3.Value);
							}
						}
						if (flag && !d3DEquipment.basic_attributes.ContainsKey(key))
						{
							d3DEquipment.basic_attributes.Add(key, value);
						}
					}
				}
				else if ("my:Extra" == item3.Name)
				{
					foreach (XmlNode item7 in item3)
					{
						if (!("my:EXData" == item7.Name))
						{
							continue;
						}
						D3DPassiveTriggerSimple d3DPassiveTriggerSimple = new D3DPassiveTriggerSimple();
						foreach (XmlAttribute attribute4 in item7.Attributes)
						{
							if ("my:EXType" == attribute4.Name && string.Empty != attribute4.Value)
							{
								d3DPassiveTriggerSimple.passive_type = (D3DPassiveTrigger.PassiveType)int.Parse(attribute4.Value);
							}
						}
						foreach (XmlNode item8 in item7)
						{
							if (!("my:DataDetial" == item8.Name))
							{
								continue;
							}
							bool flag2 = false;
							D3DPassiveTrigger.PassiveDataType data_type = D3DPassiveTrigger.PassiveDataType.PERCENT_VALUE;
							string value_data = string.Empty;
							foreach (XmlAttribute attribute5 in item8.Attributes)
							{
								if ("my:DataType" == attribute5.Name)
								{
									if (string.Empty != attribute5.Value)
									{
										data_type = (D3DPassiveTrigger.PassiveDataType)int.Parse(attribute5.Value);
										flag2 = true;
									}
								}
								else if ("my:DataValue" == attribute5.Name && string.Empty != attribute5.Value)
								{
									value_data = attribute5.Value;
								}
							}
							if (flag2)
							{
								d3DPassiveTriggerSimple.CreateTriggerData(data_type, value_data);
							}
						}
						if (!d3DEquipment.extra_attributes.ContainsKey(d3DPassiveTriggerSimple.passive_type))
						{
							d3DEquipment.extra_attributes.Add(d3DPassiveTriggerSimple.passive_type, d3DPassiveTriggerSimple);
						}
					}
				}
				else if ("my:TriggerState" == item3.Name)
				{
					foreach (XmlNode item9 in item3)
					{
						if (!("my:StateDetial" == item9.Name))
						{
							continue;
						}
						D3DStateTriggerSimple d3DStateTriggerSimple = new D3DStateTriggerSimple();
						foreach (XmlAttribute attribute6 in item9.Attributes)
						{
							if ("my:StateType" == attribute6.Name)
							{
								if (string.Empty != attribute6.Value)
								{
									d3DStateTriggerSimple.trigger_type = (D3DStateTrigger.TriggerType)int.Parse(attribute6.Value);
								}
							}
							else if ("my:Faction" == attribute6.Name && string.Empty != attribute6.Value)
							{
								d3DStateTriggerSimple.trigger_faction = (D3DStateTrigger.TriggerFaction)int.Parse(attribute6.Value);
							}
						}
						foreach (XmlNode item10 in item9)
						{
							if (!("my:DataDetial" == item10.Name))
							{
								continue;
							}
							bool flag3 = false;
							D3DStateTrigger.TriggerDataType data_type2 = D3DStateTrigger.TriggerDataType.ODDS;
							string value_data2 = string.Empty;
							foreach (XmlAttribute attribute7 in item10.Attributes)
							{
								if ("my:DataType" == attribute7.Name)
								{
									if (string.Empty != attribute7.Value)
									{
										data_type2 = (D3DStateTrigger.TriggerDataType)int.Parse(attribute7.Value);
										flag3 = true;
									}
								}
								else if ("my:DataValue" == attribute7.Name && string.Empty != attribute7.Value)
								{
									value_data2 = attribute7.Value;
								}
							}
							if (flag3)
							{
								d3DStateTriggerSimple.CreateTriggerData(data_type2, value_data2);
							}
						}
						if (!d3DEquipment.equipment_triggers.ContainsKey(d3DStateTriggerSimple.trigger_type))
						{
							d3DEquipment.equipment_triggers.Add(d3DStateTriggerSimple.trigger_type, d3DStateTriggerSimple);
						}
					}
				}
				else
				{
					if (!("my:Description" == item3.Name))
					{
						continue;
					}
					foreach (XmlNode item11 in item3)
					{
						if ("my:Content" == item11.Name && item11.ChildNodes[0] != null && string.Empty != item11.ChildNodes[0].Value)
						{
							d3DEquipment.description.Add(item11.ChildNodes[0].Value);
						}
					}
				}
			}
			if (string.Empty != d3DEquipment.equipment_id)
			{
				D3DEquipmentManager.Add(d3DEquipment.equipment_id, d3DEquipment);
			}
			else if (diagnose)
			{
				num++;
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text3 = text;
		text = text3 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item12 in list)
		{
			text = text + item12 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DEquipmentsBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DEquipmentFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public void OutputD3DEquipments(int filter)
	{
		int num = 1;
		foreach (string key in D3DEquipmentManager.Keys)
		{
			D3DEquipment d3DEquipment = D3DEquipmentManager[key];
			if (filter != (int)d3DEquipment.equipment_class && filter != -1)
			{
				continue;
			}
			string empty = string.Empty;
			string text = empty;
			empty = text + "=================No." + num + "=================\n";
			empty = empty + "Equipment ID:" + d3DEquipment.equipment_id + "\n";
			empty = empty + "Equipment Name:" + d3DEquipment.equipment_name + "\n";
			text = empty;
			empty = string.Concat(text, "Equipment Grade:", d3DEquipment.equipment_grade, "\n");
			text = empty;
			empty = string.Concat(text, "Equipment Type:", d3DEquipment.equipment_type, "\n");
			text = empty;
			empty = string.Concat(text, "Equipment Class:", d3DEquipment.equipment_class, "\n");
			text = empty;
			empty = text + "Require Level:" + d3DEquipment.require_level + "\n";
			empty = empty + "Suit ID:" + d3DEquipment.set_item_id + "\n";
			text = empty;
			empty = text + "Buy Price:" + d3DEquipment.buy_price + "\n";
			empty += "Features:\n";
			empty = empty + "-------Icon:" + d3DEquipment.use_icon + "\n";
			empty = empty + "-------Model:" + d3DEquipment.use_model + "\n";
			empty += "-------Textures:";
			foreach (string use_texture in d3DEquipment.use_textures)
			{
				empty = empty + use_texture + "/";
			}
			empty += "\nSpecial Popedom Classes:\n";
			if (d3DEquipment.explicit_popedom_classes.Count > 0)
			{
				foreach (string explicit_popedom_class in d3DEquipment.explicit_popedom_classes)
				{
					empty = empty + "-------" + explicit_popedom_class + "\n";
				}
			}
			else
			{
				empty += "--------ALL\n";
			}
			empty += "Basic Attributes:\n";
			foreach (D3DEquipment.BasicAttribute key2 in d3DEquipment.basic_attributes.Keys)
			{
				float num2 = d3DEquipment.basic_attributes[key2];
				text = empty;
				empty = string.Concat(text, "--------", key2, ":", num2, "\n");
			}
			empty += "Extra Attributes:\n";
			foreach (D3DPassiveTrigger.PassiveType key3 in d3DEquipment.extra_attributes.Keys)
			{
				D3DPassiveTriggerSimple d3DPassiveTriggerSimple = d3DEquipment.extra_attributes[key3];
				text = empty;
				empty = string.Concat(text, "--------", d3DPassiveTriggerSimple.passive_type, "\n");
				if (d3DPassiveTriggerSimple.fixed_value != null)
				{
					text = empty;
					empty = text + ">>>>>Static Value:" + d3DPassiveTriggerSimple.fixed_value.value + "<<<<<\n";
				}
				if (d3DPassiveTriggerSimple.percent_value != null)
				{
					text = empty;
					empty = text + ">>>>>Percent Value:" + d3DPassiveTriggerSimple.percent_value.value + "<<<<<\n";
				}
			}
			empty += "Triggers:\n";
			foreach (D3DStateTrigger.TriggerType key4 in d3DEquipment.equipment_triggers.Keys)
			{
				D3DStateTriggerSimple d3DStateTriggerSimple = d3DEquipment.equipment_triggers[key4];
				text = empty;
				empty = string.Concat(text, "--------", d3DStateTriggerSimple.trigger_type, "\n");
				if (d3DStateTriggerSimple.odds != null)
				{
					text = empty;
					empty = text + ">>>>>Odds:" + d3DStateTriggerSimple.odds.value + "<<<<<\n";
				}
				if (d3DStateTriggerSimple.radius != null)
				{
					text = empty;
					empty = text + ">>>>>Radius:" + d3DStateTriggerSimple.radius.value + "<<<<<\n";
				}
				if (d3DStateTriggerSimple.fixed_value != null)
				{
					text = empty;
					empty = text + ">>>>>Static Value:" + d3DStateTriggerSimple.fixed_value.value + "<<<<<\n";
				}
				if (d3DStateTriggerSimple.percent_value != null)
				{
					text = empty;
					empty = text + ">>>>>Percent Value:" + d3DStateTriggerSimple.percent_value.value + "<<<<<\n";
				}
				if (d3DStateTriggerSimple.vaild_count != null)
				{
					text = empty;
					empty = text + ">>>>>Count:" + d3DStateTriggerSimple.vaild_count.value + "<<<<<\n";
				}
				if (d3DStateTriggerSimple.vaild_time != null)
				{
					text = empty;
					empty = text + ">>>>>Time:" + d3DStateTriggerSimple.vaild_time.value + "<<<<<\n";
				}
				if (d3DStateTriggerSimple.vaild_interval != null)
				{
					text = empty;
					empty = text + ">>>>>Interval:" + d3DStateTriggerSimple.vaild_interval.value + "<<<<<\n";
				}
			}
			num++;
		}
	}

	public void LoadD3DImplicitPopedomFromFile(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			bool flag = false;
			D3DEquipment.EquipmentClass key = D3DEquipment.EquipmentClass.AXE;
			List<string> list = new List<string>();
			foreach (XmlAttribute attribute in item.Attributes)
			{
				if ("my:Equipment" == attribute.Name && string.Empty != attribute.Value)
				{
					flag = true;
					key = (D3DEquipment.EquipmentClass)int.Parse(attribute.Value);
				}
			}
			foreach (XmlNode item2 in item)
			{
				if ("my:Class" == item2.Name && item2.ChildNodes[0] != null && string.Empty != item2.ChildNodes[0].Value)
				{
					list.Add(item2.ChildNodes[0].Value);
				}
			}
			if (flag)
			{
				if (!D3DImplicitEquipPopedom.ContainsKey(key))
				{
					D3DImplicitEquipPopedom.Add(key, list);
				}
				else
				{
					D3DImplicitEquipPopedom[key].AddRange(list);
				}
			}
		}
	}

	public bool CheckEquipmentID(string equipment_id)
	{
		if (D3DEquipmentManager.ContainsKey(equipment_id))
		{
			return true;
		}
		return false;
	}

	public D3DEquipment GetEquipment(string equipment_id)
	{
		if (!CheckEquipmentID(equipment_id))
		{
			return null;
		}
		return D3DEquipmentManager[equipment_id];
	}

	public D3DEquipment GetEquipmentClone(string equipment_id)
	{
		if (!CheckEquipmentID(equipment_id))
		{
			return null;
		}
		return D3DEquipmentManager[equipment_id].Clone();
	}

	public void LoadD3DPuppetProfileFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DPuppetProfile File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "pMBAA#-HNb@#hKH(");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item2 in documentElement)
		{
			D3DPuppetProfile d3DPuppetProfile = new D3DPuppetProfile();
			foreach (XmlAttribute attribute in item2.Attributes)
			{
				if ("my:PuppetID" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DPuppetProfile.profile_id = attribute.Value;
					}
				}
				else if ("my:PuppetName" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DPuppetProfile.profile_name = attribute.Value;
					}
				}
				else if ("my:Type" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DPuppetProfile.profile_type = (D3DPuppetProfile.ProfileType)int.Parse(attribute.Value);
					}
				}
				else if ("my:D3DClass" == attribute.Name)
				{
					if (string.Empty != attribute.Value)
					{
						d3DPuppetProfile.profile_class = attribute.Value;
					}
				}
				else if ("my:Scale" == attribute.Name && string.Empty != attribute.Value)
				{
					d3DPuppetProfile.custom_scale = float.Parse(attribute.Value);
				}
			}
			foreach (XmlNode item3 in item2)
			{
				if ("my:Feature" == item3.Name)
				{
					foreach (XmlAttribute attribute2 in item3.Attributes)
					{
						if ("my:FeatureModel" == attribute2.Name)
						{
							if (string.Empty != attribute2.Value)
							{
								d3DPuppetProfile.feature_model = attribute2.Value;
							}
						}
						else if ("my:Skin" == attribute2.Name && string.Empty != attribute2.Value)
						{
							d3DPuppetProfile.feature_skin = attribute2.Value;
						}
					}
					foreach (XmlNode item4 in item3)
					{
						if ("my:FeatureTextures" == item4.Name)
						{
							if (item4.ChildNodes[0] != null)
							{
								d3DPuppetProfile.feature_textures.Add(item4.ChildNodes[0].Value);
							}
						}
						else if ("my:OtherTextures" == item4.Name)
						{
							if (item4.ChildNodes[0] != null)
							{
								d3DPuppetProfile.other_textures.Add(item4.ChildNodes[0].Value);
							}
						}
						else if ("my:PFX" == item4.Name)
						{
							string[] item = new string[2]
							{
								((XmlElement)item4).GetAttribute("my:LinkPt").Trim(),
								((XmlElement)item4).GetAttribute("my:Effect").Trim()
							};
							d3DPuppetProfile.puppet_effects.Add(item);
						}
					}
				}
				else if ("my:DefaultFeature" == item3.Name)
				{
					XmlElement xmlElement = (XmlElement)item3;
					string text3 = xmlElement.GetAttribute("my:DWeapon").Trim();
					if (string.Empty != text3)
					{
						d3DPuppetProfile.profile_default_weapon = text3;
					}
					text3 = xmlElement.GetAttribute("my:DBody").Trim();
					if (string.Empty != text3)
					{
						d3DPuppetProfile.profile_default_armor = text3;
					}
					text3 = xmlElement.GetAttribute("my:ClassName").Trim();
					if (string.Empty != text3)
					{
						d3DPuppetProfile.custom_class_name = text3;
					}
				}
				else if ("my:Animations" == item3.Name)
				{
					XmlElement xmlElement2 = (XmlElement)item3;
					string text4 = xmlElement2.GetAttribute("my:DefaultAnimation").Trim();
					if (string.Empty != text4)
					{
						d3DPuppetProfile.profile_animations[0] = text4;
					}
					text4 = xmlElement2.GetAttribute("my:TwoHandAnimation").Trim();
					if (string.Empty != text4)
					{
						d3DPuppetProfile.profile_animations[1] = text4;
					}
					text4 = xmlElement2.GetAttribute("my:DualAnimation").Trim();
					if (string.Empty != text4)
					{
						d3DPuppetProfile.profile_animations[2] = text4;
					}
				}
				else if ("my:PropertyLv1" == item3.Name || "my:PropertyLv2" == item3.Name || "my:PropertyLv3" == item3.Name || "my:PropertyLv4" == item3.Name || "my:PropertyLv5" == item3.Name || "my:PropertyLv6" == item3.Name || "my:PropertyLv7" == item3.Name || "my:PropertyLv8" == item3.Name || "my:PropertyLv9" == item3.Name)
				{
					int index = ((!("my:PropertyLv1" == item3.Name)) ? (("my:PropertyLv2" == item3.Name) ? 1 : (("my:PropertyLv3" == item3.Name) ? 2 : (("my:PropertyLv4" == item3.Name) ? 3 : (("my:PropertyLv5" == item3.Name) ? 4 : (("my:PropertyLv6" == item3.Name) ? 5 : (("my:PropertyLv7" == item3.Name) ? 6 : ((!("my:PropertyLv8" == item3.Name)) ? 8 : 7))))))) : 0);
					foreach (XmlNode item5 in item3)
					{
						XmlElement xmlElement3 = (XmlElement)item5;
						if ("my:Talent" == item5.Name)
						{
							d3DPuppetProfile.profile_talent[index].talent_ability[0] = float.Parse(xmlElement3.GetAttribute("my:Str").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[1] = float.Parse(xmlElement3.GetAttribute("my:Agi").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[2] = float.Parse(xmlElement3.GetAttribute("my:Spi").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[3] = float.Parse(xmlElement3.GetAttribute("my:Sta").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[4] = float.Parse(xmlElement3.GetAttribute("my:Int").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[5] = float.Parse(xmlElement3.GetAttribute("my:HP").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[6] = float.Parse(xmlElement3.GetAttribute("my:MP").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[7] = float.Parse(xmlElement3.GetAttribute("my:ArmorT").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[10] = float.Parse(xmlElement3.GetAttribute("my:AtkSpd").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[11] = float.Parse(xmlElement3.GetAttribute("my:MoveSpd").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[8] = float.Parse(xmlElement3.GetAttribute("my:PhyDmg").Trim());
							d3DPuppetProfile.profile_talent[index].talent_ability[9] = float.Parse(xmlElement3.GetAttribute("my:MagDmg").Trim());
						}
						else if ("my:Growth" == item5.Name)
						{
							foreach (XmlAttribute attribute3 in item5.Attributes)
							{
								if ("my:StrGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text5 = attribute3.Value.Trim();
										string[] array = text5.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[0, 0] = float.Parse(array[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[0, 1] = float.Parse(array[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[0, 2] = float.Parse(array[2].Trim());
									}
								}
								else if ("my:AgiGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text6 = attribute3.Value.Trim();
										string[] array2 = text6.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[1, 0] = float.Parse(array2[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[1, 1] = float.Parse(array2[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[1, 2] = float.Parse(array2[2].Trim());
									}
								}
								else if ("my:SpiGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text7 = attribute3.Value.Trim();
										string[] array3 = text7.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[2, 0] = float.Parse(array3[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[2, 1] = float.Parse(array3[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[2, 2] = float.Parse(array3[2].Trim());
									}
								}
								else if ("my:StaGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text8 = attribute3.Value.Trim();
										string[] array4 = text8.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[3, 0] = float.Parse(array4[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[3, 1] = float.Parse(array4[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[3, 2] = float.Parse(array4[2].Trim());
									}
								}
								else if ("my:IntGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text9 = attribute3.Value.Trim();
										string[] array5 = text9.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[4, 0] = float.Parse(array5[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[4, 1] = float.Parse(array5[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[4, 2] = float.Parse(array5[2].Trim());
									}
								}
								else if ("my:ArmorGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text10 = attribute3.Value.Trim();
										string[] array6 = text10.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[7, 0] = float.Parse(array6[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[7, 1] = float.Parse(array6[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[7, 2] = float.Parse(array6[2].Trim());
									}
								}
								else if ("my:HPGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text11 = attribute3.Value.Trim();
										string[] array7 = text11.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[5, 0] = float.Parse(array7[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[5, 1] = float.Parse(array7[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[5, 2] = float.Parse(array7[2].Trim());
									}
								}
								else if ("my:MPGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text12 = attribute3.Value.Trim();
										string[] array8 = text12.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[6, 0] = float.Parse(array8[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[6, 1] = float.Parse(array8[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[6, 2] = float.Parse(array8[2].Trim());
									}
								}
								else if ("my:PhyDmgGrowth" == attribute3.Name)
								{
									if (string.Empty != attribute3.Value)
									{
										string text13 = attribute3.Value.Trim();
										string[] array9 = text13.Split(',');
										d3DPuppetProfile.profile_talent[index].ability_growth[8, 0] = float.Parse(array9[0].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[8, 1] = float.Parse(array9[1].Trim());
										d3DPuppetProfile.profile_talent[index].ability_growth[8, 2] = float.Parse(array9[2].Trim());
									}
								}
								else if ("my:MagDmgGrowth" == attribute3.Name && string.Empty != attribute3.Value)
								{
									string text14 = attribute3.Value.Trim();
									string[] array10 = text14.Split(',');
									d3DPuppetProfile.profile_talent[index].ability_growth[9, 0] = float.Parse(array10[0].Trim());
									d3DPuppetProfile.profile_talent[index].ability_growth[9, 1] = float.Parse(array10[1].Trim());
									d3DPuppetProfile.profile_talent[index].ability_growth[9, 2] = float.Parse(array10[2].Trim());
								}
							}
						}
						else
						{
							if (!("my:Equipments" == item5.Name))
							{
								continue;
							}
							foreach (XmlAttribute attribute4 in item5.Attributes)
							{
								if ("my:RightHand" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][0] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][0].equipment_id = attribute4.Value;
									}
								}
								else if ("my:LeftHand" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][1] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][1].equipment_id = attribute4.Value;
									}
								}
								else if ("my:Armor" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][2] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][2].equipment_id = attribute4.Value;
									}
								}
								else if ("my:Helm" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][3] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][3].equipment_id = attribute4.Value;
									}
								}
								else if ("my:Boots" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][5] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][5].equipment_id = attribute4.Value;
									}
								}
								else if ("my:Necklance" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][7] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][7].equipment_id = attribute4.Value;
									}
								}
								else if ("my:Ring1" == attribute4.Name)
								{
									if (string.Empty != attribute4.Value)
									{
										d3DPuppetProfile.profile_arms[index][8] = new D3DGamer.D3DEquipmentSaveData();
										d3DPuppetProfile.profile_arms[index][8].equipment_id = attribute4.Value;
									}
								}
								else if ("my:Ring2" == attribute4.Name && string.Empty != attribute4.Value)
								{
									d3DPuppetProfile.profile_arms[index][9] = new D3DGamer.D3DEquipmentSaveData();
									d3DPuppetProfile.profile_arms[index][9].equipment_id = attribute4.Value;
								}
							}
						}
					}
				}
				else if ("my:BasicSkills" == item3.Name)
				{
					foreach (XmlNode item6 in item3)
					{
						if ("my:DefaultBasicConfig" == item6.Name)
						{
							d3DPuppetProfile.profile_basic_skill_id[0] = ((XmlElement)item6).GetAttribute("my:DefaultBasic").Trim();
							d3DPuppetProfile.basic_attack1_frames[0] = new List<List<int>>();
							d3DPuppetProfile.basic_attack2_frames[0] = new List<List<int>>();
							foreach (XmlNode item7 in item6)
							{
								if ("my:Attack1FrameConfig" == item7.Name)
								{
									string text15 = ((XmlElement)item7).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text15)
									{
										string[] array11 = text15.Split(',');
										if (array11.Length > 0)
										{
											List<int> list2 = new List<int>();
											string[] array12 = array11;
											foreach (string s in array12)
											{
												list2.Add(int.Parse(s));
											}
											d3DPuppetProfile.basic_attack1_frames[0].Add(list2);
										}
										else
										{
											d3DPuppetProfile.basic_attack1_frames[0].Add(null);
										}
									}
									else
									{
										d3DPuppetProfile.basic_attack1_frames[0].Add(null);
									}
								}
								else
								{
									if (!("my:Attack2FrameConfig" == item7.Name))
									{
										continue;
									}
									string text16 = ((XmlElement)item7).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text16)
									{
										string[] array13 = text16.Split(',');
										if (array13.Length > 0)
										{
											List<int> list3 = new List<int>();
											string[] array14 = array13;
											foreach (string s2 in array14)
											{
												list3.Add(int.Parse(s2));
											}
											d3DPuppetProfile.basic_attack2_frames[0].Add(list3);
										}
										else
										{
											d3DPuppetProfile.basic_attack2_frames[0].Add(null);
										}
									}
									else
									{
										d3DPuppetProfile.basic_attack2_frames[0].Add(null);
									}
								}
							}
							if (d3DPuppetProfile.basic_attack1_frames[0].Count == 0)
							{
								d3DPuppetProfile.basic_attack1_frames[0] = null;
							}
							if (d3DPuppetProfile.basic_attack2_frames[0].Count == 0)
							{
								d3DPuppetProfile.basic_attack2_frames[0] = null;
							}
						}
						else if ("my:DoubleBasicConfig" == item6.Name)
						{
							d3DPuppetProfile.profile_basic_skill_id[1] = ((XmlElement)item6).GetAttribute("my:DoubleBasic").Trim();
							d3DPuppetProfile.basic_attack1_frames[1] = new List<List<int>>();
							d3DPuppetProfile.basic_attack2_frames[1] = new List<List<int>>();
							foreach (XmlNode item8 in item6)
							{
								if ("my:Attack1FrameConfig" == item8.Name)
								{
									string text17 = ((XmlElement)item8).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text17)
									{
										string[] array15 = text17.Split(',');
										if (array15.Length > 0)
										{
											List<int> list4 = new List<int>();
											string[] array16 = array15;
											foreach (string s3 in array16)
											{
												list4.Add(int.Parse(s3));
											}
											d3DPuppetProfile.basic_attack1_frames[1].Add(list4);
										}
										else
										{
											d3DPuppetProfile.basic_attack1_frames[1].Add(null);
										}
									}
									else
									{
										d3DPuppetProfile.basic_attack1_frames[1].Add(null);
									}
								}
								else
								{
									if (!("my:Attack2FrameConfig" == item8.Name))
									{
										continue;
									}
									string text18 = ((XmlElement)item8).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text18)
									{
										string[] array17 = text18.Split(',');
										if (array17.Length > 0)
										{
											List<int> list5 = new List<int>();
											string[] array18 = array17;
											foreach (string s4 in array18)
											{
												list5.Add(int.Parse(s4));
											}
											d3DPuppetProfile.basic_attack2_frames[1].Add(list5);
										}
										else
										{
											d3DPuppetProfile.basic_attack2_frames[1].Add(null);
										}
									}
									else
									{
										d3DPuppetProfile.basic_attack2_frames[1].Add(null);
									}
								}
							}
							if (d3DPuppetProfile.basic_attack1_frames[1].Count == 0)
							{
								d3DPuppetProfile.basic_attack1_frames[1] = null;
							}
							if (d3DPuppetProfile.basic_attack2_frames[1].Count == 0)
							{
								d3DPuppetProfile.basic_attack2_frames[1] = null;
							}
						}
						else
						{
							if (!("my:DualBasicConfig" == item6.Name))
							{
								continue;
							}
							d3DPuppetProfile.profile_basic_skill_id[2] = ((XmlElement)item6).GetAttribute("my:DualBasic").Trim();
							d3DPuppetProfile.basic_attack1_frames[2] = new List<List<int>>();
							d3DPuppetProfile.basic_attack2_frames[2] = new List<List<int>>();
							foreach (XmlNode item9 in item6)
							{
								if ("my:Attack1FrameConfig" == item9.Name)
								{
									string text19 = ((XmlElement)item9).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text19)
									{
										string[] array19 = text19.Split(',');
										if (array19.Length > 0)
										{
											List<int> list6 = new List<int>();
											string[] array20 = array19;
											foreach (string s5 in array20)
											{
												list6.Add(int.Parse(s5));
											}
											d3DPuppetProfile.basic_attack1_frames[2].Add(list6);
										}
										else
										{
											d3DPuppetProfile.basic_attack1_frames[2].Add(null);
										}
									}
									else
									{
										d3DPuppetProfile.basic_attack1_frames[2].Add(null);
									}
								}
								else
								{
									if (!("my:Attack2FrameConfig" == item9.Name))
									{
										continue;
									}
									string text20 = ((XmlElement)item9).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text20)
									{
										string[] array21 = text20.Split(',');
										if (array21.Length > 0)
										{
											List<int> list7 = new List<int>();
											string[] array22 = array21;
											foreach (string s6 in array22)
											{
												list7.Add(int.Parse(s6));
											}
											d3DPuppetProfile.basic_attack2_frames[2].Add(list7);
										}
										else
										{
											d3DPuppetProfile.basic_attack2_frames[2].Add(null);
										}
									}
									else
									{
										d3DPuppetProfile.basic_attack2_frames[2].Add(null);
									}
								}
							}
							if (d3DPuppetProfile.basic_attack1_frames[2].Count == 0)
							{
								d3DPuppetProfile.basic_attack1_frames[2] = null;
							}
							if (d3DPuppetProfile.basic_attack2_frames[2].Count == 0)
							{
								d3DPuppetProfile.basic_attack2_frames[2] = null;
							}
						}
					}
				}
				else if ("my:ActiveSkill" == item3.Name)
				{
					foreach (XmlNode item10 in item3)
					{
						if (!("my:ActiveConfig" == item10.Name))
						{
							continue;
						}
						D3DClassActiveSkillStatus d3DClassActiveSkillStatus = new D3DClassActiveSkillStatus();
						foreach (XmlAttribute attribute5 in item10.Attributes)
						{
							if ("my:SkillID" == attribute5.Name)
							{
								if (string.Empty != attribute5.Value)
								{
									d3DClassActiveSkillStatus.skill_id = attribute5.Value;
								}
							}
							else if ("my:CastTime" == attribute5.Name)
							{
								if (string.Empty != attribute5.Value)
								{
									d3DClassActiveSkillStatus.cast_time = float.Parse(attribute5.Value);
								}
							}
							else if ("my:MasterLevel" == attribute5.Name && string.Empty != attribute5.Value)
							{
								d3DClassActiveSkillStatus.unlock_level = int.Parse(attribute5.Value);
							}
						}
						foreach (XmlNode item11 in item10)
						{
							if ("my:DefaultAnimationConfig" == item11.Name)
							{
								int value = int.Parse(((XmlElement)item11).GetAttribute("my:ClipIndex").Trim());
								d3DClassActiveSkillStatus.animation_clip_index[0] = new D3DInt(value);
								d3DClassActiveSkillStatus.clip_frames[0] = new List<List<int>>();
								foreach (XmlNode item12 in item11)
								{
									if (!("my:FrameConfig" == item12.Name))
									{
										continue;
									}
									string text21 = ((XmlElement)item12).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text21)
									{
										string[] array23 = text21.Split(',');
										if (array23.Length > 0)
										{
											List<int> list8 = new List<int>();
											string[] array24 = array23;
											foreach (string s7 in array24)
											{
												list8.Add(int.Parse(s7));
											}
											d3DClassActiveSkillStatus.clip_frames[0].Add(list8);
										}
										else
										{
											d3DClassActiveSkillStatus.clip_frames[0].Add(null);
										}
									}
									else
									{
										d3DClassActiveSkillStatus.clip_frames[0].Add(null);
									}
								}
								if (d3DClassActiveSkillStatus.clip_frames[0].Count == 0)
								{
									d3DClassActiveSkillStatus.clip_frames[0] = null;
								}
							}
							else if ("my:DoubleAnimationConfig" == item11.Name)
							{
								int value2 = int.Parse(((XmlElement)item11).GetAttribute("my:ClipIndex").Trim());
								d3DClassActiveSkillStatus.animation_clip_index[1] = new D3DInt(value2);
								d3DClassActiveSkillStatus.clip_frames[1] = new List<List<int>>();
								foreach (XmlNode item13 in item11)
								{
									if (!("my:FrameConfig" == item13.Name))
									{
										continue;
									}
									string text22 = ((XmlElement)item13).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text22)
									{
										string[] array25 = text22.Split(',');
										if (array25.Length > 0)
										{
											List<int> list9 = new List<int>();
											string[] array26 = array25;
											foreach (string s8 in array26)
											{
												list9.Add(int.Parse(s8));
											}
											d3DClassActiveSkillStatus.clip_frames[1].Add(list9);
										}
										else
										{
											d3DClassActiveSkillStatus.clip_frames[1].Add(null);
										}
									}
									else
									{
										d3DClassActiveSkillStatus.clip_frames[1].Add(null);
									}
								}
								if (d3DClassActiveSkillStatus.clip_frames[1].Count == 0)
								{
									d3DClassActiveSkillStatus.clip_frames[1] = null;
								}
							}
							else if ("my:DualAnimationConfig" == item11.Name)
							{
								int value3 = int.Parse(((XmlElement)item11).GetAttribute("my:ClipIndex").Trim());
								d3DClassActiveSkillStatus.animation_clip_index[2] = new D3DInt(value3);
								d3DClassActiveSkillStatus.clip_frames[2] = new List<List<int>>();
								foreach (XmlNode item14 in item11)
								{
									if (!("my:FrameConfig" == item14.Name))
									{
										continue;
									}
									string text23 = ((XmlElement)item14).GetAttribute("my:FrameIndexs").Trim();
									if (string.Empty != text23)
									{
										string[] array27 = text23.Split(',');
										if (array27.Length > 0)
										{
											List<int> list10 = new List<int>();
											string[] array28 = array27;
											foreach (string s9 in array28)
											{
												list10.Add(int.Parse(s9));
											}
											d3DClassActiveSkillStatus.clip_frames[2].Add(list10);
										}
										else
										{
											d3DClassActiveSkillStatus.clip_frames[2].Add(null);
										}
									}
									else
									{
										d3DClassActiveSkillStatus.clip_frames[2].Add(null);
									}
								}
								if (d3DClassActiveSkillStatus.clip_frames[2].Count == 0)
								{
									d3DClassActiveSkillStatus.clip_frames[2] = null;
								}
							}
							else if ("my:MasterCost" == item11.Name)
							{
								if (item11.ChildNodes[0] != null)
								{
									d3DClassActiveSkillStatus.upgrade_cost.Add(int.Parse(item11.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterCrystal" == item11.Name)
							{
								if (item11.ChildNodes[0] != null)
								{
									d3DClassActiveSkillStatus.upgrade_crystal.Add(int.Parse(item11.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterSpace" == item11.Name && item11.ChildNodes[0] != null)
							{
								d3DClassActiveSkillStatus.upgrade_difference.Add(int.Parse(item11.ChildNodes[0].Value));
							}
						}
						if (string.Empty != d3DClassActiveSkillStatus.skill_id && !d3DPuppetProfile.profile_active_skill_id_list.ContainsKey(d3DClassActiveSkillStatus.skill_id))
						{
							d3DPuppetProfile.profile_active_skill_id_list.Add(d3DClassActiveSkillStatus.skill_id, d3DClassActiveSkillStatus);
						}
					}
				}
				else if ("my:PassiveSkill" == item3.Name)
				{
					foreach (XmlNode item15 in item3)
					{
						if (!("my:PassiveConfig" == item15.Name))
						{
							continue;
						}
						D3DClassPassiveSkillStatus d3DClassPassiveSkillStatus = new D3DClassPassiveSkillStatus();
						foreach (XmlAttribute attribute6 in item15.Attributes)
						{
							if ("my:SkillID" == attribute6.Name)
							{
								if (string.Empty != attribute6.Value)
								{
									d3DClassPassiveSkillStatus.skill_id = attribute6.Value;
								}
							}
							else if ("my:MasterLevel" == attribute6.Name && string.Empty != attribute6.Value)
							{
								d3DClassPassiveSkillStatus.unlock_level = int.Parse(attribute6.Value);
							}
						}
						foreach (XmlNode item16 in item15)
						{
							if ("my:MasterCost" == item16.Name)
							{
								if (item16.ChildNodes[0] != null)
								{
									d3DClassPassiveSkillStatus.upgrade_cost.Add(int.Parse(item16.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterCrystal" == item16.Name)
							{
								if (item16.ChildNodes[0] != null)
								{
									d3DClassPassiveSkillStatus.upgrade_crystal.Add(int.Parse(item16.ChildNodes[0].Value));
								}
							}
							else if ("my:MasterSpace" == item16.Name && item16.ChildNodes[0] != null)
							{
								d3DClassPassiveSkillStatus.upgrade_difference.Add(int.Parse(item16.ChildNodes[0].Value));
							}
						}
						if (string.Empty != d3DClassPassiveSkillStatus.skill_id && !d3DPuppetProfile.profile_passive_skill_id_list.ContainsKey(d3DClassPassiveSkillStatus.skill_id))
						{
							d3DPuppetProfile.profile_passive_skill_id_list.Add(d3DClassPassiveSkillStatus.skill_id, d3DClassPassiveSkillStatus);
						}
					}
				}
				else if ("my:Bonus" == item3.Name)
				{
					XmlElement xmlElement4 = (XmlElement)item3;
					d3DPuppetProfile.percent_bonus[0] = float.Parse(xmlElement4.GetAttribute("my:ExpPercent").Trim());
					d3DPuppetProfile.percent_bonus[1] = float.Parse(xmlElement4.GetAttribute("my:GoldPercent").Trim());
					d3DPuppetProfile.fixed_bonus[0] = int.Parse(xmlElement4.GetAttribute("my:ExpFixed").Trim());
					d3DPuppetProfile.fixed_bonus[1] = int.Parse(xmlElement4.GetAttribute("my:GoldFixed").Trim());
				}
			}
			if (string.Empty != d3DPuppetProfile.profile_id)
			{
				D3DPuppetProfileManager.Add(d3DPuppetProfile.profile_id, d3DPuppetProfile);
			}
			else if (diagnose)
			{
				num++;
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text24 = text;
		text = text24 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item17 in list)
		{
			text = text + item17 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DPuppetProfilesBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DPuppetProfileFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public bool CheckProfileID(string profile_id)
	{
		if (D3DPuppetProfileManager.ContainsKey(profile_id))
		{
			return true;
		}
		return false;
	}

	public D3DPuppetProfile GetProfile(string profile_id)
	{
		if (!CheckProfileID(profile_id))
		{
			return null;
		}
		return D3DPuppetProfileManager[profile_id];
	}

	public D3DPuppetProfile GetProfileClone(string profile_id)
	{
		if (!CheckProfileID(profile_id))
		{
			return null;
		}
		return D3DPuppetProfileManager[profile_id].Clone();
	}

	public void LoadD3DEnemyGroupFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DEnemyGroup File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "]]7WMBmHxiATEEZ[");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DEnemyGroup d3DEnemyGroup = new D3DEnemyGroup();
			XmlElement xmlElement = (XmlElement)item;
			d3DEnemyGroup.group_id = xmlElement.GetAttribute("my:GroupID").Trim();
			for (int i = 1; i < 4; i++)
			{
				float num2 = float.Parse(xmlElement.GetAttribute("my:LootEnableOdds" + i).Trim());
				if (num2 == 0f)
				{
					continue;
				}
				D3DGearLoot d3DGearLoot = new D3DGearLoot();
				d3DGearLoot.loot_odds = num2;
				d3DGearLoot.custom_loot_id = xmlElement.GetAttribute("my:CustomLootID" + ((i != 1) ? i.ToString() : string.Empty)).Trim();
				string text3 = xmlElement.GetAttribute("my:CustomOdds" + ((i != 1) ? i.ToString() : string.Empty)).Trim();
				if (string.Empty != text3)
				{
					string[] array = text3.Split(',');
					for (int j = 0; j < array.Length; j++)
					{
						if (string.Empty != array[j])
						{
							d3DGearLoot.random_loot_odds[j] = new D3DFloat(float.Parse(array[j]));
						}
					}
				}
				d3DEnemyGroup.Loots.Add(d3DGearLoot);
			}
			foreach (XmlNode item2 in item)
			{
				xmlElement = (XmlElement)item2;
				if ("my:Patrol" == item2.Name)
				{
					d3DEnemyGroup.leader_id = xmlElement.GetAttribute("my:LeaderID").Trim();
					d3DEnemyGroup.map_spawn_interval = float.Parse(xmlElement.GetAttribute("my:MapSpawnInterval").Trim());
					d3DEnemyGroup.patrol_radius = float.Parse(xmlElement.GetAttribute("my:PatrolRadius").Trim());
					d3DEnemyGroup.sight_radius = float.Parse(xmlElement.GetAttribute("my:VisualField").Trim());
					d3DEnemyGroup.pursue_radius = float.Parse(xmlElement.GetAttribute("my:PursueRange").Trim());
					d3DEnemyGroup.patrol_speed = float.Parse(xmlElement.GetAttribute("my:PatrolSpeed").Trim());
					d3DEnemyGroup.pursue_speed = float.Parse(xmlElement.GetAttribute("my:PursueSpeed").Trim());
				}
				else if ("my:BattleData" == item2.Name)
				{
					d3DEnemyGroup.kill_require = int.Parse(xmlElement.GetAttribute("my:KillRequire").Trim());
					d3DEnemyGroup.battle_spawn_interval = float.Parse(xmlElement.GetAttribute("my:BattleSpawnInterval").Trim());
					d3DEnemyGroup.battle_spawn_limit = int.Parse(xmlElement.GetAttribute("my:BattleSpawnLimit").Trim());
				}
				else
				{
					if (!("my:GroupProperty" == item2.Name))
					{
						continue;
					}
					foreach (XmlNode item3 in item2)
					{
						if (!("my:SpawnPhase" == item3.Name))
						{
							continue;
						}
						D3DEnemyGroupSpawnPhase d3DEnemyGroupSpawnPhase = new D3DEnemyGroupSpawnPhase();
						xmlElement = (XmlElement)item3;
						d3DEnemyGroupSpawnPhase.random_spawn = bool.Parse(xmlElement.GetAttribute("my:IsRandomSpawn").Trim());
						d3DEnemyGroupSpawnPhase.once_spawn_count = int.Parse(xmlElement.GetAttribute("my:OnceSpawnCount").Trim());
						d3DEnemyGroupSpawnPhase.phase_spawn_count = int.Parse(xmlElement.GetAttribute("my:PhaseSpawnCount").Trim());
						d3DEnemyGroupSpawnPhase.is_wait = bool.Parse(xmlElement.GetAttribute("my:IsWait").Trim());
						d3DEnemyGroupSpawnPhase.battle_bgm = int.Parse(xmlElement.GetAttribute("my:BattleBGM").Trim());
						foreach (XmlNode item4 in item3)
						{
							if ("my:SpawnEnemy" == item4.Name)
							{
								xmlElement = (XmlElement)item4;
								D3DEnemyGroupSpawnPhase.EnemySpawner enemySpawner = new D3DEnemyGroupSpawnPhase.EnemySpawner();
								enemySpawner.enemy_id = xmlElement.GetAttribute("my:EnemyID").Trim();
								enemySpawner.odds = float.Parse(xmlElement.GetAttribute("my:SpawnOdds").Trim());
								enemySpawner.level_diff = int.Parse(xmlElement.GetAttribute("my:LevelDiff").Trim());
								d3DEnemyGroupSpawnPhase.phase_enemy_spawners.Add(enemySpawner);
							}
						}
						d3DEnemyGroup.spawn_phases.Add(d3DEnemyGroupSpawnPhase);
					}
				}
			}
			if (string.Empty != d3DEnemyGroup.group_id)
			{
				D3DEnemyGroupManager.Add(d3DEnemyGroup.group_id, d3DEnemyGroup);
			}
			else if (diagnose)
			{
				num++;
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text4 = text;
		text = text4 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item5 in list)
		{
			text = text + item5 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DEnemyGroupBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DEnemyGroupFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public D3DEnemyGroup GetEnemyGroup(string group_id)
	{
		if (!D3DEnemyGroupManager.ContainsKey(group_id))
		{
			return null;
		}
		return D3DEnemyGroupManager[group_id];
	}

	public void LoadD3DDungeonFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DDungeon File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "MjZQ*+GRJaGsk0]H");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		D3DDungeon d3DDungeon = new D3DDungeon();
		d3DDungeon.dungeon_id = ((XmlElement)documentElement).GetAttribute("my:DungeonID").Trim();
		d3DDungeon.dungeon_name = ((XmlElement)documentElement).GetAttribute("my:DungeonName").Trim();
		foreach (XmlNode item2 in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item2;
			if ("my:Map" == item2.Name)
			{
				foreach (XmlNode item3 in item2)
				{
					if ("my:MapJigsaw" == item3.Name)
					{
						d3DDungeon.dungeon_map_jigsaw.Add(item3.FirstChild.Value.Trim());
					}
				}
			}
			else if ("my:DungeonTown" == item2.Name)
			{
				xmlElement = (XmlElement)item2;
				string text3 = xmlElement.GetAttribute("my:TownSceneID").Trim();
				if (string.Empty != text3)
				{
					d3DDungeon.dungeon_town = new D3DDungeonTown();
					d3DDungeon.dungeon_town.town_model_preset = text3;
					string[] array = xmlElement.GetAttribute("my:MapTownPosition").Trim().Split(',');
					d3DDungeon.dungeon_town.town_map_position = new Vector2(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()));
				}
			}
			else
			{
				if (!("my:DungeonFloor" == item2.Name))
				{
					continue;
				}
				xmlElement = (XmlElement)item2;
				D3DDungeonFloor d3DDungeonFloor = new D3DDungeonFloor();
				d3DDungeonFloor.floor_index = int.Parse(xmlElement.GetAttribute("my:FloorIndex").Trim());
				string[] array2 = xmlElement.GetAttribute("my:MapPosition").Trim().Split(',');
				d3DDungeonFloor.floor_map_position = new Vector2(float.Parse(array2[0].Trim()), float.Parse(array2[1].Trim()));
				d3DDungeonFloor.boss_level = bool.Parse(xmlElement.GetAttribute("my:BossLevel").Trim());
				d3DDungeonFloor.floor_level_min = int.Parse(xmlElement.GetAttribute("my:FloorLevel").Trim());
				d3DDungeonFloor.floor_level_max = int.Parse(xmlElement.GetAttribute("my:FloorLevelMax").Trim());
				d3DDungeonFloor.floor_loot_min = int.Parse(xmlElement.GetAttribute("my:FloorLootLevel").Trim());
				d3DDungeonFloor.floor_loot_max = int.Parse(xmlElement.GetAttribute("my:FloorLootLevelMax").Trim());
				foreach (XmlNode item4 in item2)
				{
					xmlElement = (XmlElement)item4;
					if ("my:FloorInfo" == item4.Name)
					{
						d3DDungeonFloor.floor_model_preset = xmlElement.GetAttribute("my:FloorId").Trim();
						d3DDungeonFloor.open_room1 = bool.Parse(xmlElement.GetAttribute("my:FloorRoom1").Trim());
						d3DDungeonFloor.open_room2 = bool.Parse(xmlElement.GetAttribute("my:FloorRoom2").Trim());
						d3DDungeonFloor.spawn_points_prefab = xmlElement.GetAttribute("my:SpawnPointsPrefab").Trim();
						d3DDungeonFloor.enmey_power = int.Parse(xmlElement.GetAttribute("my:EnemyPower").Trim());
					}
					else if ("my:FloorBattleInfo" == item4.Name)
					{
						d3DDungeonFloor.floor_battle_kill_require = int.Parse(xmlElement.GetAttribute("my:KillRequire").Trim());
						d3DDungeonFloor.floor_battle_spawn_interval = float.Parse(xmlElement.GetAttribute("my:SpawnInterval").Trim());
						d3DDungeonFloor.floor_battle_spawn_limit = int.Parse(xmlElement.GetAttribute("my:BattleSpawnLimit").Trim());
					}
					else if ("my:FloorSpawnInfo" == item4.Name)
					{
						foreach (XmlNode item5 in item4)
						{
							if ("my:EnemyGroupList" == item5.Name)
							{
								foreach (XmlNode item6 in item5)
								{
									if ("my:GroupID" == item6.Name && item6.ChildNodes[0] != null && string.Empty != item6.ChildNodes[0].Value)
									{
										d3DDungeonFloor.floor_random_groups.Add(item6.ChildNodes[0].Value);
									}
								}
							}
							else if ("my:SpawnPoint" == item5.Name)
							{
								D3DDungeonFloorSpawner d3DDungeonFloorSpawner = new D3DDungeonFloorSpawner();
								xmlElement = (XmlElement)item5;
								d3DDungeonFloorSpawner.spawner_id = int.Parse(xmlElement.GetAttribute("my:SpawnerID").Trim());
								d3DDungeonFloorSpawner.group_id = xmlElement.GetAttribute("my:SpawnGroup").Trim();
								if (string.Empty == d3DDungeonFloorSpawner.group_id)
								{
									d3DDungeonFloorSpawner.random_spawn = true;
								}
								d3DDungeonFloorSpawner.spawn_interval = float.Parse(xmlElement.GetAttribute("my:PointSpawnInterval").Trim());
								if (d3DDungeonFloorSpawner.spawn_interval < 0f)
								{
									d3DDungeonFloorSpawner.use_group_interval = true;
								}
								d3DDungeonFloorSpawner.bShowCDTime = bool.Parse(xmlElement.GetAttribute("my:ShowCDTime").Trim());
								d3DDungeonFloorSpawner.fixed_group_level = int.Parse(xmlElement.GetAttribute("my:FixedLevel").Trim());
								d3DDungeonFloor.floor_spawners.Add(d3DDungeonFloorSpawner.spawner_id, d3DDungeonFloorSpawner);
							}
						}
					}
					else
					{
						if (!("my:Treasure" == item4.Name))
						{
							continue;
						}
						D3DDungeonFloorTreasureChest d3DDungeonFloorTreasureChest = new D3DDungeonFloorTreasureChest();
						d3DDungeonFloorTreasureChest.treasure_id = ((XmlElement)item4).GetAttribute("my:TreasureID").Trim();
						d3DDungeonFloorTreasureChest.spawn_odds = float.Parse(((XmlElement)item4).GetAttribute("my:Odds").Trim());
						d3DDungeonFloorTreasureChest.spawn_interval = float.Parse(((XmlElement)item4).GetAttribute("my:TreasureInterval").Trim());
						foreach (XmlNode item7 in item4)
						{
							if (!("my:SpwanerIDList" == item7.Name))
							{
								continue;
							}
							string[] array3 = item7.ChildNodes[0].Value.Split(',');
							string[] array4 = array3;
							foreach (string text4 in array4)
							{
								if (text4.Contains("-"))
								{
									string[] array5 = text4.Split('-');
									int num2 = int.Parse(array5[0].Trim());
									int num3 = int.Parse(array5[1].Trim());
									for (int j = num2; j <= num3; j++)
									{
										if (!d3DDungeonFloorTreasureChest.spawn_points.Contains(j))
										{
											d3DDungeonFloorTreasureChest.spawn_points.Add(j);
										}
									}
								}
								else
								{
									int item = int.Parse(text4.Trim());
									if (!d3DDungeonFloorTreasureChest.spawn_points.Contains(item))
									{
										d3DDungeonFloorTreasureChest.spawn_points.Add(item);
									}
								}
							}
						}
						d3DDungeonFloor.floor_treasures.Add(d3DDungeonFloorTreasureChest);
					}
				}
				d3DDungeon.dungeon_floors.Add(d3DDungeonFloor);
			}
		}
		if (string.Empty != d3DDungeon.dungeon_id)
		{
			D3DDungeonManager.Add(d3DDungeon.dungeon_id, d3DDungeon);
		}
		else if (diagnose)
		{
			num++;
		}
		if (!diagnose)
		{
			return;
		}
		string text5 = text;
		text = text5 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item8 in list)
		{
			text = text + item8 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DDungeonBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DDungeonFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public D3DDungeon GetDungeon(string dungeon_id)
	{
		if (!D3DDungeonManager.ContainsKey(dungeon_id))
		{
			return null;
		}
		return D3DDungeonManager[dungeon_id];
	}

	public Dictionary<string, D3DDungeon>.KeyCollection GetDungeonKeys()
	{
		return D3DDungeonManager.Keys;
	}

	public void InitDungeonsSpawnersOnNewGame()
	{
		foreach (string key in D3DDungeonManager.Keys)
		{
			foreach (D3DDungeonFloor dungeon_floor in D3DDungeonManager[key].dungeon_floors)
			{
				foreach (int key2 in dungeon_floor.floor_spawners.Keys)
				{
					dungeon_floor.UpdateSpawnerData(key2);
				}
			}
		}
	}

	public void D3DDungeonsSpawnInit()
	{
		foreach (string key in D3DDungeonManager.Keys)
		{
			D3DDungeonManager[key].DungeonFloorsTimeInit();
		}
	}

	public void LoadD3DDungeonProgressFromFile(string file_path, bool diagnose)
	{
		string empty = string.Empty;
		if (diagnose)
		{
			empty += ">===============================<\n";
			empty = empty + "Read D3DDungeonProgress File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, "QK9iz0Ue4A1C6z3t");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			string key = xmlElement.GetAttribute("my:DungeonID").Trim();
			Dictionary<int, D3DDungeonProgerssManager.LevelProgress> dictionary = new Dictionary<int, D3DDungeonProgerssManager.LevelProgress>();
			foreach (XmlNode item2 in item)
			{
				if (!("my:ProgressSet" == item2.Name))
				{
					continue;
				}
				D3DDungeonProgerssManager.LevelProgress levelProgress = new D3DDungeonProgerssManager.LevelProgress();
				int key2 = int.Parse(((XmlElement)item2).GetAttribute("my:Level").Trim());
				levelProgress.on_first_enter_story = ((XmlElement)item2).GetAttribute("my:OnFirstEnterStory").Trim();
				foreach (XmlNode item3 in item2)
				{
					if ("my:UnlockNextKey" == item3.Name)
					{
						xmlElement = (XmlElement)item3;
						int key3 = int.Parse(xmlElement.GetAttribute("my:SpawnPoint").Trim());
						D3DDungeonProgerssManager.LevelProgress.NextLevelBattleUnlock nextLevelBattleUnlock = new D3DDungeonProgerssManager.LevelProgress.NextLevelBattleUnlock();
						nextLevelBattleUnlock.on_battle_start_story = xmlElement.GetAttribute("my:OnBattleStory").Trim();
						nextLevelBattleUnlock.on_battle_win_story = xmlElement.GetAttribute("my:OnWinStory").Trim();
						nextLevelBattleUnlock.target_group = xmlElement.GetAttribute("my:TargetGroup").Trim();
						levelProgress.UnlockBattleList.Add(key3, nextLevelBattleUnlock);
					}
				}
				dictionary.Add(key2, levelProgress);
			}
			D3DDungeonProgerssManager.Instance.DungeonProgressManager.Add(key, dictionary);
		}
	}

	public void LoadD3DDungeonProgressBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DDungeonProgressFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public void LoadD3DImageCell(ref Dictionary<string, D3DImageCell> image_cell_indexer, string cell_config_file)
	{
		if (image_cell_indexer == null)
		{
			return;
		}
		TextAsset textAsset = Resources.Load(cell_config_file) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]);
		while (text != string.Empty)
		{
			int num = text.IndexOf('\n');
			string text2 = text.Substring(0, num);
			text = text.Remove(0, num + 1);
			num = text2.IndexOf('\t');
			string text3 = text2.Substring(0, num);
			text2 = text2.Remove(0, num + 1);
			num = text2.IndexOf('\t');
			string texture = text2.Substring(0, num);
			text2 = text2.Remove(0, num + 1);
			string text4 = text2;
			text4 = text4.Trim();
			string[] array = text4.Split(',');
			Rect rect = new Rect(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
			if (string.Empty != text3 && !image_cell_indexer.ContainsKey(text3))
			{
				image_cell_indexer.Add(text3, new D3DImageCell(text3, texture, rect));
			}
		}
	}

	public void LoadD3DFormula(string formula_path)
	{
		TextAsset textAsset = Resources.Load(formula_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item9 in documentElement)
		{
			if ("my:HPFormula" == item9.Name)
			{
				foreach (XmlNode item10 in item9)
				{
					if (!("my:HPCoe" == item10.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe = new D3DFormulaCoe();
					string empty = string.Empty;
					empty = ((XmlElement)item10).GetAttribute("my:coe1");
					if (string.Empty != empty)
					{
						d3DFormulaCoe.formula_coes.Add(float.Parse(empty));
					}
					foreach (XmlNode item11 in item10)
					{
						if ("my:ClassID" == item11.Name && item11.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe.formula_class.Add(item11.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe.formula_class.Count > 0)
					{
						hp_coe_list.Add(d3DFormulaCoe);
					}
					else if (hp_default_coe == null)
					{
						hp_default_coe = d3DFormulaCoe;
					}
				}
			}
			else if ("my:MPFormula" == item9.Name)
			{
				foreach (XmlNode item12 in item9)
				{
					if (!("my:MPCoe" == item12.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe2 = new D3DFormulaCoe();
					string empty2 = string.Empty;
					empty2 = ((XmlElement)item12).GetAttribute("my:coe1");
					if (string.Empty != empty2)
					{
						d3DFormulaCoe2.formula_coes.Add(float.Parse(empty2));
					}
					foreach (XmlNode item13 in item12)
					{
						if ("my:ClassID" == item13.Name && item13.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe2.formula_class.Add(item13.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe2.formula_class.Count > 0)
					{
						mp_coe_list.Add(d3DFormulaCoe2);
					}
					else if (mp_default_coe == null)
					{
						mp_default_coe = d3DFormulaCoe2;
					}
				}
			}
			else if ("my:DefFormula" == item9.Name)
			{
				foreach (XmlNode item14 in item9)
				{
					if (!("my:DefCoe" == item14.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe3 = new D3DFormulaCoe();
					string empty3 = string.Empty;
					empty3 = ((XmlElement)item14).GetAttribute("my:coe1");
					if (string.Empty != empty3)
					{
						d3DFormulaCoe3.formula_coes.Add(float.Parse(empty3));
					}
					foreach (XmlNode item15 in item14)
					{
						if ("my:ClassID" == item15.Name && item15.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe3.formula_class.Add(item15.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe3.formula_class.Count > 0)
					{
						def_coe_list.Add(d3DFormulaCoe3);
					}
					else if (def_default_coe == null)
					{
						def_default_coe = d3DFormulaCoe3;
					}
				}
			}
			else if ("my:APFormula" == item9.Name)
			{
				foreach (XmlNode item16 in item9)
				{
					if (!("my:APCoe" == item16.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe4 = new D3DFormulaCoe();
					string empty4 = string.Empty;
					empty4 = ((XmlElement)item16).GetAttribute("my:coe1");
					if (string.Empty != empty4)
					{
						d3DFormulaCoe4.formula_coes.Add(float.Parse(empty4));
					}
					empty4 = ((XmlElement)item16).GetAttribute("my:coe2");
					if (string.Empty != empty4)
					{
						d3DFormulaCoe4.formula_coes.Add(float.Parse(empty4));
					}
					empty4 = ((XmlElement)item16).GetAttribute("my:coe3");
					if (string.Empty != empty4)
					{
						d3DFormulaCoe4.formula_coes.Add(float.Parse(empty4));
					}
					foreach (XmlNode item17 in item16)
					{
						if ("my:ClassID" == item17.Name && item17.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe4.formula_class.Add(item17.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe4.formula_class.Count > 0)
					{
						main_power_coe_list.Add(d3DFormulaCoe4);
					}
					else if (main_power_default_coe == null)
					{
						main_power_default_coe = d3DFormulaCoe4;
					}
				}
			}
			else if ("my:SubAPFormula" == item9.Name)
			{
				foreach (XmlNode item18 in item9)
				{
					if (!("my:SubAPCoe" == item18.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe5 = new D3DFormulaCoe();
					string empty5 = string.Empty;
					empty5 = ((XmlElement)item18).GetAttribute("my:coe1");
					if (string.Empty != empty5)
					{
						d3DFormulaCoe5.formula_coes.Add(float.Parse(empty5));
					}
					empty5 = ((XmlElement)item18).GetAttribute("my:coe2");
					if (string.Empty != empty5)
					{
						d3DFormulaCoe5.formula_coes.Add(float.Parse(empty5));
					}
					empty5 = ((XmlElement)item18).GetAttribute("my:coe3");
					if (string.Empty != empty5)
					{
						d3DFormulaCoe5.formula_coes.Add(float.Parse(empty5));
					}
					foreach (XmlNode item19 in item18)
					{
						if ("my:ClassID" == item19.Name && item19.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe5.formula_class.Add(item19.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe5.formula_class.Count > 0)
					{
						sub_power_coe_list.Add(d3DFormulaCoe5);
					}
					else if (sub_power_default_coe == null)
					{
						sub_power_default_coe = d3DFormulaCoe5;
					}
				}
			}
			else if ("my:HPRecoverFormula" == item9.Name)
			{
				foreach (XmlNode item20 in item9)
				{
					if (!("my:HPRecoverCoe" == item20.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe6 = new D3DFormulaCoe();
					string empty6 = string.Empty;
					empty6 = ((XmlElement)item20).GetAttribute("my:coe1");
					if (string.Empty != empty6)
					{
						d3DFormulaCoe6.formula_coes.Add(float.Parse(empty6));
					}
					empty6 = ((XmlElement)item20).GetAttribute("my:coe2");
					if (string.Empty != empty6)
					{
						d3DFormulaCoe6.formula_coes.Add(float.Parse(empty6));
					}
					foreach (XmlNode item21 in item20)
					{
						if ("my:ClassID" == item21.Name && item21.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe6.formula_class.Add(item21.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe6.formula_class.Count > 0)
					{
						hp_recover_coe_list.Add(d3DFormulaCoe6);
					}
					else if (mp_recover_default_coe == null)
					{
						hp_recover_default_coe = d3DFormulaCoe6;
					}
				}
			}
			else if ("my:MPRecoverFormula" == item9.Name)
			{
				foreach (XmlNode item22 in item9)
				{
					if (!("my:MPRecoverCoe" == item22.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe7 = new D3DFormulaCoe();
					string empty7 = string.Empty;
					empty7 = ((XmlElement)item22).GetAttribute("my:coe1");
					if (string.Empty != empty7)
					{
						d3DFormulaCoe7.formula_coes.Add(float.Parse(empty7));
					}
					empty7 = ((XmlElement)item22).GetAttribute("my:coe2");
					if (string.Empty != empty7)
					{
						d3DFormulaCoe7.formula_coes.Add(float.Parse(empty7));
					}
					foreach (XmlNode item23 in item22)
					{
						if ("my:ClassID" == item23.Name && item23.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe7.formula_class.Add(item23.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe7.formula_class.Count > 0)
					{
						mp_recover_coe_list.Add(d3DFormulaCoe7);
					}
					else if (mp_recover_default_coe == null)
					{
						mp_recover_default_coe = d3DFormulaCoe7;
					}
				}
			}
			else if ("my:EXPGainFormula" == item9.Name)
			{
				D3DFormulas.exp_bonus_coe[0] = float.Parse(((XmlElement)item9).GetAttribute("my:coe1").Trim());
				D3DFormulas.exp_bonus_coe[1] = float.Parse(((XmlElement)item9).GetAttribute("my:coe2").Trim());
			}
			else if ("my:GoldGainFormula" == item9.Name)
			{
				D3DFormulas.gold_bonus_coe[0] = float.Parse(((XmlElement)item9).GetAttribute("my:coe1").Trim());
				D3DFormulas.gold_bonus_coe[1] = float.Parse(((XmlElement)item9).GetAttribute("my:coe2").Trim());
			}
			else if ("my:DmgRangeFormula" == item9.Name)
			{
				foreach (XmlNode item24 in item9)
				{
					if (!("my:DmgRange" == item24.Name))
					{
						continue;
					}
					D3DFormulaCoe d3DFormulaCoe8 = new D3DFormulaCoe();
					string empty8 = string.Empty;
					empty8 = ((XmlElement)item24).GetAttribute("my:coe1");
					if (string.Empty != empty8)
					{
						d3DFormulaCoe8.formula_coes.Add(float.Parse(empty8));
					}
					empty8 = ((XmlElement)item24).GetAttribute("my:coe2");
					if (string.Empty != empty8)
					{
						d3DFormulaCoe8.formula_coes.Add(float.Parse(empty8));
					}
					foreach (XmlNode item25 in item24)
					{
						if ("my:ClassID" == item25.Name && item25.ChildNodes[0].Value != null)
						{
							d3DFormulaCoe8.formula_class.Add(item25.ChildNodes[0].Value);
						}
					}
					if (d3DFormulaCoe8.formula_class.Count > 0)
					{
						damage_range_coe_list.Add(d3DFormulaCoe8);
					}
					else if (damage_range_default_coe == null)
					{
						damage_range_default_coe = d3DFormulaCoe8;
					}
				}
			}
			else if ("my:DmgReduceFormula" == item9.Name)
			{
				string empty9 = string.Empty;
				empty9 = ((XmlElement)item9).GetAttribute("my:coe1");
				if (string.Empty != empty9)
				{
					D3DFormulas.reduce_dmg_coe[0] = float.Parse(empty9);
				}
				empty9 = ((XmlElement)item9).GetAttribute("my:coe2");
				if (string.Empty != empty9)
				{
					D3DFormulas.reduce_dmg_coe[1] = float.Parse(empty9);
				}
				empty9 = ((XmlElement)item9).GetAttribute("my:coe3");
				if (string.Empty != empty9)
				{
					D3DFormulas.reduce_dmg_limit = float.Parse(empty9);
				}
			}
			else if ("my:ExpFormula" == item9.Name)
			{
				string empty10 = string.Empty;
				empty10 = ((XmlElement)item9).GetAttribute("my:coe1");
				if (string.Empty != empty10)
				{
					D3DFormulas.exp_coe[0] = float.Parse(empty10);
				}
				empty10 = ((XmlElement)item9).GetAttribute("my:coe2");
				if (string.Empty != empty10)
				{
					D3DFormulas.exp_coe[1] = float.Parse(empty10);
				}
				empty10 = ((XmlElement)item9).GetAttribute("my:coe3");
				if (string.Empty != empty10)
				{
					D3DFormulas.exp_coe[2] = float.Parse(empty10);
				}
				empty10 = ((XmlElement)item9).GetAttribute("my:coe4");
				if (string.Empty != empty10)
				{
					D3DFormulas.exp_coe[3] = float.Parse(empty10);
				}
				empty10 = ((XmlElement)item9).GetAttribute("my:coe5");
				if (string.Empty != empty10)
				{
					D3DFormulas.exp_coe[4] = float.Parse(empty10);
				}
			}
			else if ("my:PDPSFormula" == item9.Name)
			{
				string empty11 = string.Empty;
				empty11 = ((XmlElement)item9).GetAttribute("my:coe1");
				if (string.Empty != empty11)
				{
					D3DFormulas.dps_coe[0] = float.Parse(empty11);
				}
			}
			else if ("my:MDPSFormula" == item9.Name)
			{
				string empty12 = string.Empty;
				empty12 = ((XmlElement)item9).GetAttribute("my:coe1");
				if (string.Empty != empty12)
				{
					D3DFormulas.dps_coe[1] = float.Parse(empty12);
				}
			}
			else if ("my:LootFormula" == item9.Name)
			{
				D3DLootFromula.Instance.accessory_adjust = float.Parse(((XmlElement)item9).GetAttribute("my:AccessoryOdds").Trim());
				foreach (XmlNode item26 in item9)
				{
					if ("my:LegendOdds" == item26.Name)
					{
						float[] coe = new float[3]
						{
							float.Parse(((XmlElement)item26).GetAttribute("my:coe1")),
							float.Parse(((XmlElement)item26).GetAttribute("my:coe2")),
							float.Parse(((XmlElement)item26).GetAttribute("my:coe3"))
						};
						D3DLootFromula.Instance.SetFormulaCoe(2, coe);
					}
					else if ("my:RareOdds" == item26.Name)
					{
						float[] coe2 = new float[3]
						{
							float.Parse(((XmlElement)item26).GetAttribute("my:coe1")),
							float.Parse(((XmlElement)item26).GetAttribute("my:coe2")),
							float.Parse(((XmlElement)item26).GetAttribute("my:coe3"))
						};
						D3DLootFromula.Instance.SetFormulaCoe(1, coe2);
					}
					else if ("my:SuperiorOdds" == item26.Name)
					{
						float[] coe3 = new float[3]
						{
							float.Parse(((XmlElement)item26).GetAttribute("my:coe1")),
							float.Parse(((XmlElement)item26).GetAttribute("my:coe2")),
							float.Parse(((XmlElement)item26).GetAttribute("my:coe3"))
						};
						D3DLootFromula.Instance.SetFormulaCoe(0, coe3);
					}
				}
			}
			else if ("my:OtherFormula" == item9.Name)
			{
				D3DFormulas.dual_adjust = float.Parse(((XmlElement)item9).GetAttribute("my:DualAdjust").Trim());
				D3DFormulas.dual_dmg_adjust = float.Parse(((XmlElement)item9).GetAttribute("my:DualDmgAdjust").Trim());
				D3DFormulas.RotateCoe = float.Parse(((XmlElement)item9).GetAttribute("my:RotateCoe").Trim());
				D3DFormulas.Critical = float.Parse(((XmlElement)item9).GetAttribute("my:Critical").Trim());
			}
			else if ("my:DungeonLevelToEquipLevel" == item9.Name)
			{
				int nDungeonLow = int.Parse(((XmlElement)item9).GetAttribute("my:DungeonLow").Trim());
				int nDungeonHigh = int.Parse(((XmlElement)item9).GetAttribute("my:DungeonHigh").Trim());
				int nEquipLow = int.Parse(((XmlElement)item9).GetAttribute("my:EquipLow").Trim());
				int nEquipHigh = int.Parse(((XmlElement)item9).GetAttribute("my:EquipHigh").Trim());
				D3DShopRuleEx.Instance.dungenLevelToEquipLevel.AddRange(nDungeonLow, nDungeonHigh, nEquipLow, nEquipHigh);
			}
			else if ("my:ShopRule" == item9.Name)
			{
				XmlElement xmlElement = (XmlElement)item9;
				D3DShopRuleEx.Instance.ShopLevelMinus = int.Parse(xmlElement.GetAttribute("my:LevelMinus").Trim());
				D3DShopRuleEx.Instance.BattleRefreshCount = int.Parse(xmlElement.GetAttribute("my:BattleRefreshCount").Trim());
				D3DShopRuleEx.Instance.LevelPhase = int.Parse(xmlElement.GetAttribute("my:LevelPhase").Trim());
				string[] array = xmlElement.GetAttribute("my:ItemGradeOdds").Trim().Split(',');
				string[] array2 = array;
				foreach (string text2 in array2)
				{
				}
				array = xmlElement.GetAttribute("my:ItemGradeOddsAdjust").Trim().Split(',');
				string[] array3 = array;
				foreach (string text3 in array3)
				{
					D3DShopRuleEx.Instance.ItemGradeOddsAdjust.Add(float.Parse(text3.Trim()));
				}
				array = xmlElement.GetAttribute("my:ItemGradeOddsLimit").Trim().Split(',');
				string[] array4 = array;
				foreach (string text4 in array4)
				{
					D3DShopRuleEx.Instance.ItemGradeOddsLimit.Add(float.Parse(text4.Trim()));
				}
				foreach (XmlNode item27 in item9)
				{
					if ("my:RefreshCost" == item27.Name)
					{
						int[] array5 = new int[2];
						string[] array6 = item27.ChildNodes[0].Value.Trim().Split(',');
						array5[0] = int.Parse(array6[0]);
						array5[1] = int.Parse(array6[1]);
						D3DShopRuleEx.Instance.RefreshCost.Add(array5);
					}
				}
			}
			else if ("my:SlotRule" == item9.Name)
			{
				int num = int.Parse(((XmlElement)item9).GetAttribute("my:SlotIndex").Trim());
				int nLevelLow = int.Parse(((XmlElement)item9).GetAttribute("my:LevelLow").Trim());
				int nLevelHigh = int.Parse(((XmlElement)item9).GetAttribute("my:LevelHigh").Trim());
				D3DShopRuleEx.SlotRule rule = new D3DShopRuleEx.SlotRule();
				foreach (XmlNode item28 in item9)
				{
					if ("my:GradeRatio" == item28.Name)
					{
						float item = float.Parse(((XmlElement)item28).GetAttribute("my:InferiorRatio").Trim());
						float item2 = float.Parse(((XmlElement)item28).GetAttribute("my:NormalRatio").Trim());
						float item3 = float.Parse(((XmlElement)item28).GetAttribute("my:SuperiorRatio").Trim());
						float item4 = float.Parse(((XmlElement)item28).GetAttribute("my:MagicRatio").Trim());
						float item5 = float.Parse(((XmlElement)item28).GetAttribute("my:RareRatio").Trim());
						rule.GradeProbability.Add(item);
						rule.GradeProbability.Add(item2);
						rule.GradeProbability.Add(item3);
						rule.GradeProbability.Add(item4);
						rule.GradeProbability.Add(item5);
					}
					else
					{
						if (!("my:EquipRatio" == item28.Name))
						{
							continue;
						}
						D3DEquipment d3DEquipment = new D3DEquipment();
						int num2 = int.Parse(((XmlElement)item28).GetAttribute("my:EquipType").Trim());
						float item6 = float.Parse(((XmlElement)item28).GetAttribute("my:EquipProbability").Trim());
						if (num2 < 100)
						{
							d3DEquipment.equipment_type = (D3DEquipment.EquipmentType)num2;
							d3DEquipment.equipment_class = D3DEquipment.EquipmentClass.PLATE;
							rule.CadidatesTypes.Add(d3DEquipment);
						}
						else if (num2 < 200)
						{
							num2 -= 100;
							d3DEquipment.equipment_type = D3DEquipment.EquipmentType.None;
							d3DEquipment.equipment_class = (D3DEquipment.EquipmentClass)num2;
							rule.CadidatesTypes.Add(d3DEquipment);
						}
						else
						{
							switch (num2)
							{
							case 201:
								SlotRuleArmorRandom(ref rule, num, nLevelLow, nLevelHigh);
								break;
							case 202:
								SlotRuleWeaponRandom(ref rule, num, nLevelLow, nLevelHigh);
								break;
							case 203:
								SlotRuleAccessoryRandom(ref rule, num, nLevelLow, nLevelHigh);
								break;
							case 204:
								SlotRuleArmorRandom(ref rule, num, nLevelLow, nLevelHigh);
								SlotRuleWeaponRandom(ref rule, num, nLevelLow, nLevelHigh);
								SlotRuleAccessoryRandom(ref rule, num, nLevelLow, nLevelHigh);
								break;
							}
						}
						rule.EquipmentProbability.Add(item6);
					}
				}
				D3DShopRuleEx.Instance.AddSlotRules(rule, num, nLevelLow, nLevelHigh);
			}
			else if ("my:RetreatMulctFormula" == item9.Name)
			{
				D3DBattleRule.Instance.RetreatMulctCoe = float.Parse(((XmlElement)item9).GetAttribute("my:coe1").Trim());
			}
			else if ("my:GoldBagFormula" == item9.Name)
			{
				D3DBattleRule.Instance.GoldBagCoe[0] = float.Parse(((XmlElement)item9).GetAttribute("my:coe1").Trim());
				D3DBattleRule.Instance.GoldBagCoe[1] = float.Parse(((XmlElement)item9).GetAttribute("my:coe2").Trim());
				D3DBattleRule.Instance.GoldBagCoe[2] = float.Parse(((XmlElement)item9).GetAttribute("my:coe3").Trim());
				D3DBattleRule.Instance.GoldBagOdds[0] = float.Parse(((XmlElement)item9).GetAttribute("my:coe4").Trim());
				D3DBattleRule.Instance.GoldBagOdds[1] = float.Parse(((XmlElement)item9).GetAttribute("my:coe5").Trim());
				D3DBattleRule.Instance.GoldBagOdds[2] = float.Parse(((XmlElement)item9).GetAttribute("my:coe6").Trim());
			}
			else if ("my:RespawnCostFormula" == item9.Name)
			{
				D3DRespawnRule.Instance.GoldMultiFactor = int.Parse(((XmlElement)item9).GetAttribute("my:multiValue").Trim());
				D3DRespawnRule.Instance.MinGoldMultiFactor = int.Parse(((XmlElement)item9).GetAttribute("my:minGoldMulti").Trim());
				D3DRespawnRule.Instance.MinCrystalCost = int.Parse(((XmlElement)item9).GetAttribute("my:minCrystalCost").Trim());
				D3DRespawnRule.Instance.ExchangeRate = int.Parse(((XmlElement)item9).GetAttribute("my:gcExchangeRate").Trim());
				D3DRespawnRule.Instance.RefreshCount = int.Parse(((XmlElement)item9).GetAttribute("my:refreshCount").Trim());
			}
			else if ("my:IAPPriorityFormula" == item9.Name)
			{
				List<D3DEquipment.EquipmentType> list = new List<D3DEquipment.EquipmentType>();
				List<D3DEquipment.EquipmentClass> list2 = new List<D3DEquipment.EquipmentClass>();
				foreach (XmlNode item29 in item9)
				{
					string name = item29.Name;
					string text5 = ((XmlElement)item29).GetAttribute("my:type").Trim();
					int num3 = int.Parse(((XmlElement)item29).GetAttribute("my:type").Trim());
					int num4 = int.Parse(((XmlElement)item29).GetAttribute("my:class").Trim());
					D3DEquipment.EquipmentType item7 = (D3DEquipment.EquipmentType)num3;
					D3DEquipment.EquipmentClass item8 = (D3DEquipment.EquipmentClass)num4;
					list.Add(item7);
					list2.Add(item8);
				}
				D3DShopRuleEx.Instance.BuildPriority(list, list2);
			}
			else if ("my:ExpPunitive" == item9.Name)
			{
				D3DFormulas.ExpPunitive.Add(int.Parse(((XmlElement)item9).GetAttribute("my:LvDiff").Trim()), float.Parse(((XmlElement)item9).GetAttribute("my:ExpOff").Trim()));
			}
			else if ("my:EquipCompareFormula" == item9.Name)
			{
				D3DEquipment._EquipComparePlus.nWhitePlus = int.Parse(((XmlElement)item9).GetAttribute("my:WhitePlus").Trim());
				D3DEquipment._EquipComparePlus.nGreenPlus = int.Parse(((XmlElement)item9).GetAttribute("my:GreenPlus").Trim());
				D3DEquipment._EquipComparePlus.nBluePlus = int.Parse(((XmlElement)item9).GetAttribute("my:BluePlus").Trim());
				D3DEquipment._EquipComparePlus.nPurplePlus = int.Parse(((XmlElement)item9).GetAttribute("my:PurplePlus").Trim());
				D3DEquipment._EquipComparePlus.nMaxLevel = int.Parse(((XmlElement)item9).GetAttribute("my:MaxLevel").Trim());
			}
		}
	}

	private void AddSlotRuleUnit(ref D3DShopRuleEx.SlotRule rule, D3DEquipment.EquipmentType type, D3DEquipment.EquipmentClass className)
	{
		D3DEquipment d3DEquipment = new D3DEquipment();
		d3DEquipment.equipment_type = type;
		d3DEquipment.equipment_class = className;
		rule.CadidatesTypes.Add(d3DEquipment);
		rule.EquipmentProbability.Add(1f);
	}

	private void SlotRuleAccessoryRandom(ref D3DShopRuleEx.SlotRule rule, int nSlowIndex, int nLevelLow, int nLevelHigh)
	{
		AddSlotRuleUnit(ref rule, D3DEquipment.EquipmentType.ACCESSORY, D3DEquipment.EquipmentClass.NECKLACE);
		AddSlotRuleUnit(ref rule, D3DEquipment.EquipmentType.ACCESSORY, D3DEquipment.EquipmentClass.RING);
	}

	private void SlotRuleArmorRandom(ref D3DShopRuleEx.SlotRule rule, int nSlowIndex, int nLevelLow, int nLevelHigh)
	{
		AddSlotRuleUnit(ref rule, D3DEquipment.EquipmentType.HELM, D3DEquipment.EquipmentClass.PLATE);
		AddSlotRuleUnit(ref rule, D3DEquipment.EquipmentType.ARMOR, D3DEquipment.EquipmentClass.PLATE);
		AddSlotRuleUnit(ref rule, D3DEquipment.EquipmentType.BOOTS, D3DEquipment.EquipmentClass.PLATE);
	}

	private void SlotRuleWeaponRandom(ref D3DShopRuleEx.SlotRule rule, int nSlowIndex, int nLevelLow, int nLevelHigh)
	{
		for (int i = 0; i <= 7; i++)
		{
			AddSlotRuleUnit(ref rule, D3DEquipment.EquipmentType.None, (D3DEquipment.EquipmentClass)i);
		}
	}

	public void LoadD3DDrawRule(string rule_path)
	{
		TextAsset textAsset = Resources.Load(rule_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if ("my:DrawCost" == item.Name)
			{
				string text2 = ((XmlElement)item).GetAttribute("my:DungeonID").Trim();
				List<D3DBattleRule.DrawCost> list = null;
				foreach (XmlNode item2 in item)
				{
					if ("my:LevelDrawCost" == item2.Name)
					{
						if (list == null)
						{
							list = new List<D3DBattleRule.DrawCost>();
						}
						XmlElement xmlElement = (XmlElement)item2;
						D3DBattleRule.DrawCost drawCost = new D3DBattleRule.DrawCost();
						drawCost.cost = new int[2];
						string[] array = xmlElement.GetAttribute("my:Cost").Trim().Split(',');
						drawCost.cost[0] = int.Parse(array[0]);
						drawCost.cost[1] = int.Parse(array[1]);
						drawCost.dungeon_level = new int[2];
						drawCost.dungeon_level[0] = int.Parse(xmlElement.GetAttribute("my:MinLevel").Trim());
						drawCost.dungeon_level[1] = int.Parse(xmlElement.GetAttribute("my:MaxLevel").Trim());
						list.Add(drawCost);
					}
				}
				if (string.Empty != text2 && list != null && !D3DBattleRule.Instance.dungeon_draw_cost.ContainsKey(text2))
				{
					D3DBattleRule.Instance.dungeon_draw_cost.Add(text2, list);
				}
			}
			else if ("my:DrawOdds" == item.Name)
			{
				XmlElement xmlElement2 = (XmlElement)item;
				D3DBattleRule.Instance.first_draw_odds[0] = float.Parse(xmlElement2.GetAttribute("my:FirstDraw1").Trim());
				D3DBattleRule.Instance.first_draw_odds[1] = float.Parse(xmlElement2.GetAttribute("my:FirstDraw2").Trim());
				D3DBattleRule.Instance.second_draw_odd = float.Parse(xmlElement2.GetAttribute("my:SecondDraw").Trim());
			}
		}
	}

	public D3DPuppetAI GetPuppetAI(string puppet_id)
	{
		if (!D3DAIManager.ContainsKey(puppet_id))
		{
			return DefaultPuppetAI;
		}
		return D3DAIManager[puppet_id];
	}

	public void LoadD3DPuppetAIFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DPuppetAI File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "NX9HR~]AJtVh,nGl");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DPuppetAI d3DPuppetAI = new D3DPuppetAI();
			d3DPuppetAI.puppet_id = ((XmlElement)item).GetAttribute("my:PuppetID").Trim();
			foreach (XmlNode item2 in item)
			{
				XmlElement xmlElement = (XmlElement)item2;
				if ("my:BattleSkillPreset" == item2.Name)
				{
					d3DPuppetAI.entrance_skill_cd = float.Parse(xmlElement.GetAttribute("my:EntranceCD").Trim());
					d3DPuppetAI.chain_skill_cd_min = float.Parse(xmlElement.GetAttribute("my:ChainCDMin").Trim());
					d3DPuppetAI.chain_skill_cd_max = float.Parse(xmlElement.GetAttribute("my:ChainCDMax").Trim());
					d3DPuppetAI.add_all_skills = bool.Parse(xmlElement.GetAttribute("my:AddAllSkill").Trim());
					if (d3DPuppetAI.add_all_skills)
					{
						continue;
					}
					foreach (XmlNode item3 in item2)
					{
						if ("my:BattleSkill" == item3.Name)
						{
							XmlElement xmlElement2 = (XmlElement)item3;
							if (d3DPuppetAI.battle_skill_list == null)
							{
								d3DPuppetAI.battle_skill_list = new Dictionary<string, int>();
							}
							d3DPuppetAI.battle_skill_list.Add(xmlElement2.GetAttribute("my:SkillID").Trim(), int.Parse(xmlElement2.GetAttribute("my:UseCount").Trim()));
						}
					}
				}
				else if ("my:CallBack" == item2.Name)
				{
					d3DPuppetAI.on_entrance_skill = xmlElement.GetAttribute("my:OnEnterSkill").Trim();
					foreach (XmlNode item4 in item2)
					{
						XmlElement xmlElement3 = (XmlElement)item4;
						if ("my:OnHpDecreased" == item4.Name)
						{
							if (d3DPuppetAI.on_hp_decreased_skill == null)
							{
								d3DPuppetAI.on_hp_decreased_skill = new SortedDictionary<float, string>();
							}
							d3DPuppetAI.on_hp_decreased_skill.Add(float.Parse(xmlElement3.GetAttribute("my:HpScale").Trim()), xmlElement3.GetAttribute("my:SkillID").Trim());
						}
						else if ("my:OnDead" == item4.Name)
						{
							d3DPuppetAI.on_dead_skill = xmlElement3.GetAttribute("my:OnDeadSkill").Trim();
							foreach (XmlNode item5 in item4)
							{
								if (!("my:Revive" == item5.Name))
								{
									continue;
								}
								XmlElement xmlElement4 = (XmlElement)item5;
								d3DPuppetAI.auto_revive = new D3DPuppetAI.AutoRevive();
								d3DPuppetAI.auto_revive.revive_delay = float.Parse(xmlElement4.GetAttribute("my:TimeOut").Trim());
								d3DPuppetAI.auto_revive.revive_count = int.Parse(xmlElement4.GetAttribute("my:ReviveCount").Trim());
								foreach (XmlNode item6 in item5)
								{
									if (!("my:FriendCondition" == item6.Name))
									{
										continue;
									}
									foreach (XmlNode item7 in item6)
									{
										if ("my:FriendID" == item7.Name)
										{
											if (d3DPuppetAI.auto_revive.friend_conditions == null)
											{
												d3DPuppetAI.auto_revive.friend_conditions = new List<string>();
											}
											d3DPuppetAI.auto_revive.friend_conditions.Add(item7.ChildNodes[0].Value.Trim());
										}
									}
								}
							}
						}
						else if ("my:OnFriendNumChange" == item4.Name)
						{
							if (d3DPuppetAI.on_friend_count_changed == null)
							{
								d3DPuppetAI.on_friend_count_changed = new Dictionary<int, D3DPuppetAI.SkillBehaviour>();
							}
							int key = int.Parse(((XmlElement)item4).GetAttribute("my:CheckNum").Trim());
							D3DPuppetAI.SkillBehaviour skillBehaviour = new D3DPuppetAI.SkillBehaviour();
							skillBehaviour.trigger_skill = ((XmlElement)item4).GetAttribute("my:TriggerSkillID").Trim();
							foreach (XmlNode item8 in item4)
							{
								if ("my:RemoveBattleSkillID" == item8.Name)
								{
									if (skillBehaviour.remove_skills_from_list == null)
									{
										skillBehaviour.remove_skills_from_list = new List<string>();
									}
									skillBehaviour.remove_skills_from_list.Add(item8.ChildNodes[0].Value);
								}
								else if ("my:BattleSkill" == item8.Name)
								{
									if (skillBehaviour.add_skills_to_list == null)
									{
										skillBehaviour.add_skills_to_list = new Dictionary<string, int>();
									}
									skillBehaviour.add_skills_to_list.Add(((XmlElement)item8).GetAttribute("my:SkillID").Trim(), int.Parse(((XmlElement)item8).GetAttribute("my:UseCount").Trim()));
								}
							}
							d3DPuppetAI.on_friend_count_changed.Add(key, skillBehaviour);
						}
						else if ("my:OnEnemyNumChange" == item4.Name)
						{
							if (d3DPuppetAI.on_enemy_count_changed == null)
							{
								d3DPuppetAI.on_enemy_count_changed = new Dictionary<int, D3DPuppetAI.SkillBehaviour>();
							}
							int key2 = int.Parse(((XmlElement)item4).GetAttribute("my:CheckNum").Trim());
							D3DPuppetAI.SkillBehaviour skillBehaviour2 = new D3DPuppetAI.SkillBehaviour();
							skillBehaviour2.trigger_skill = ((XmlElement)item4).GetAttribute("my:TriggerSkillID").Trim();
							foreach (XmlNode item9 in item4)
							{
								if ("my:RemoveBattleSkillID" == item9.Name)
								{
									if (skillBehaviour2.remove_skills_from_list == null)
									{
										skillBehaviour2.remove_skills_from_list = new List<string>();
									}
									skillBehaviour2.remove_skills_from_list.Add(item9.ChildNodes[0].Value);
								}
								else if ("my:BattleSkill" == item9.Name)
								{
									if (skillBehaviour2.add_skills_to_list == null)
									{
										skillBehaviour2.add_skills_to_list = new Dictionary<string, int>();
									}
									skillBehaviour2.add_skills_to_list.Add(((XmlElement)item9).GetAttribute("my:SkillID").Trim(), int.Parse(((XmlElement)item9).GetAttribute("my:UseCount").Trim()));
								}
							}
							d3DPuppetAI.on_enemy_count_changed.Add(key2, skillBehaviour2);
						}
						else if ("my:OnSummonedNumChange" == item4.Name)
						{
							if (d3DPuppetAI.on_summoned_count_changed == null)
							{
								d3DPuppetAI.on_summoned_count_changed = new Dictionary<int, string>();
							}
							int key3 = int.Parse(((XmlElement)item4).GetAttribute("my:CheckNum").Trim());
							string value = ((XmlElement)item4).GetAttribute("my:TriggerSkillID").Trim();
							d3DPuppetAI.on_summoned_count_changed.Add(key3, value);
						}
					}
				}
				else
				{
					if (!("my:Clock" == item2.Name))
					{
						continue;
					}
					foreach (XmlNode item10 in item2)
					{
						XmlElement xmlElement5 = (XmlElement)item10;
						if ("my:CommonTimeOut" == item10.Name)
						{
							if (d3DPuppetAI.survival_time_out == null)
							{
								d3DPuppetAI.survival_time_out = new Dictionary<float, D3DPuppetAI.SkillBehaviour>();
							}
							float key4 = float.Parse(xmlElement5.GetAttribute("my:TimeOut").Trim());
							D3DPuppetAI.SkillBehaviour skillBehaviour3 = new D3DPuppetAI.SkillBehaviour();
							skillBehaviour3.trigger_skill = xmlElement5.GetAttribute("my:TriggerSkillID").Trim();
							foreach (XmlNode item11 in item10)
							{
								if ("my:RemoveBattleSkillID" == item11.Name)
								{
									if (skillBehaviour3.remove_skills_from_list == null)
									{
										skillBehaviour3.remove_skills_from_list = new List<string>();
									}
									skillBehaviour3.remove_skills_from_list.Add(item11.ChildNodes[0].Value);
								}
								else if ("my:BattleSkill" == item11.Name)
								{
									if (skillBehaviour3.add_skills_to_list == null)
									{
										skillBehaviour3.add_skills_to_list = new Dictionary<string, int>();
									}
									skillBehaviour3.add_skills_to_list.Add(((XmlElement)item11).GetAttribute("my:SkillID").Trim(), int.Parse(((XmlElement)item11).GetAttribute("my:UseCount").Trim()));
								}
							}
							d3DPuppetAI.survival_time_out.Add(key4, skillBehaviour3);
						}
						else if ("my:DisruptHatred" == item10.Name)
						{
							d3DPuppetAI.discrupt_hatred = new D3DPuppetAI.DisruptHatred();
							d3DPuppetAI.discrupt_hatred.cycle_time = float.Parse(xmlElement5.GetAttribute("my:Cycle").Trim());
							d3DPuppetAI.discrupt_hatred.keep_time = float.Parse(xmlElement5.GetAttribute("my:KeepTime").Trim());
							d3DPuppetAI.discrupt_hatred.tirgger_skill = xmlElement5.GetAttribute("my:TriggerSkillID").Trim();
						}
						else if ("my:LoopClock" == item10.Name)
						{
							if (d3DPuppetAI.loop_clock == null)
							{
								d3DPuppetAI.loop_clock = new Dictionary<float, string>();
							}
							float key5 = float.Parse(xmlElement5.GetAttribute("my:Cycle").Trim());
							string value2 = xmlElement5.GetAttribute("my:TriggerSkillID").Trim();
							d3DPuppetAI.loop_clock.Add(key5, value2);
						}
					}
				}
			}
			if (string.Empty == d3DPuppetAI.puppet_id)
			{
				if (DefaultPuppetAI == null)
				{
					DefaultPuppetAI = d3DPuppetAI;
				}
				else
				{
					text += "Default AI Overlaped!\n";
				}
			}
			else
			{
				D3DAIManager.Add(d3DPuppetAI.puppet_id, d3DPuppetAI);
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text3 = text;
		text = text3 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item12 in list)
		{
			text = text + item12 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DPuppetAIBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DPuppetAIFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public void LoadD3DPuppetTransforms(string pt_path)
	{
		TextAsset textAsset = Resources.Load(pt_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if (!("my:PuppetTransform" == item.Name))
			{
				continue;
			}
			string empty = string.Empty;
			D3DPuppetTransformCfg d3DPuppetTransformCfg = new D3DPuppetTransformCfg();
			XmlElement xmlElement = (XmlElement)item;
			empty = xmlElement.GetAttribute("my:ModelName").Trim();
			d3DPuppetTransformCfg.ring_size = float.Parse(xmlElement.GetAttribute("my:RingSize").Trim());
			string empty2 = string.Empty;
			foreach (XmlNode item2 in item)
			{
				xmlElement = (XmlElement)item2;
				if ("my:CharacterController" == item2.Name)
				{
					d3DPuppetTransformCfg.character_controller_cfg = new D3DPuppetTransformCfg.CharacterControllerCfg();
					empty2 = xmlElement.GetAttribute("my:Center").Trim();
					string[] array = empty2.Split(',');
					d3DPuppetTransformCfg.character_controller_cfg.center = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					d3DPuppetTransformCfg.character_controller_cfg.radius = float.Parse(xmlElement.GetAttribute("my:Radius").Trim());
					d3DPuppetTransformCfg.character_controller_cfg.height = float.Parse(xmlElement.GetAttribute("my:Height").Trim());
				}
				else if ("my:StashFeatureCameraCfg" == item2.Name)
				{
					d3DPuppetTransformCfg.stash_camera_cfg = new D3DPuppetTransformCfg.CameraCfg();
					empty2 = xmlElement.GetAttribute("my:CameraOffset").Trim();
					string[] array = empty2.Split(',');
					d3DPuppetTransformCfg.stash_camera_cfg.offset = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					empty2 = xmlElement.GetAttribute("my:CameraRotation").Trim();
					array = empty2.Split(',');
					d3DPuppetTransformCfg.stash_camera_cfg.rotation = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					d3DPuppetTransformCfg.stash_camera_cfg.size = float.Parse(xmlElement.GetAttribute("my:CameraSize").Trim());
				}
				else if ("my:FaceCameraCfg" == item2.Name)
				{
					d3DPuppetTransformCfg.face_camera_cfg = new D3DPuppetTransformCfg.CameraCfg();
					empty2 = xmlElement.GetAttribute("my:CameraOffset").Trim();
					string[] array = empty2.Split(',');
					d3DPuppetTransformCfg.face_camera_cfg.offset = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					empty2 = xmlElement.GetAttribute("my:CameraRotation").Trim();
					array = empty2.Split(',');
					d3DPuppetTransformCfg.face_camera_cfg.rotation = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					d3DPuppetTransformCfg.face_camera_cfg.size = float.Parse(xmlElement.GetAttribute("my:CameraSize").Trim());
				}
				else if ("my:TavernCameraCfg" == item2.Name)
				{
					d3DPuppetTransformCfg.tavern_hire_camera_cfg = new D3DPuppetTransformCfg.CameraCfg();
					empty2 = xmlElement.GetAttribute("my:CameraOffset").Trim();
					string[] array = empty2.Split(',');
					d3DPuppetTransformCfg.tavern_hire_camera_cfg.offset = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					empty2 = xmlElement.GetAttribute("my:CameraRotation").Trim();
					array = empty2.Split(',');
					d3DPuppetTransformCfg.tavern_hire_camera_cfg.rotation = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					d3DPuppetTransformCfg.tavern_hire_camera_cfg.size = float.Parse(xmlElement.GetAttribute("my:CameraSize").Trim());
				}
				else if ("my:TavernCampCameraCfg" == item2.Name)
				{
					d3DPuppetTransformCfg.tavern_camp_camera_cfg = new D3DPuppetTransformCfg.CameraCfg();
					empty2 = xmlElement.GetAttribute("my:CameraOffset").Trim();
					string[] array = empty2.Split(',');
					d3DPuppetTransformCfg.tavern_camp_camera_cfg.offset = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					empty2 = xmlElement.GetAttribute("my:CameraRotation").Trim();
					array = empty2.Split(',');
					d3DPuppetTransformCfg.tavern_camp_camera_cfg.rotation = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					d3DPuppetTransformCfg.tavern_camp_camera_cfg.size = float.Parse(xmlElement.GetAttribute("my:CameraSize").Trim());
				}
				else if ("my:TavernBattleTeamCameraCfg" == item2.Name)
				{
					d3DPuppetTransformCfg.tavern_battle_team_cfg = new D3DPuppetTransformCfg.CameraCfg();
					empty2 = xmlElement.GetAttribute("my:CameraOffset").Trim();
					string[] array = empty2.Split(',');
					d3DPuppetTransformCfg.tavern_battle_team_cfg.offset = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					empty2 = xmlElement.GetAttribute("my:CameraRotation").Trim();
					array = empty2.Split(',');
					d3DPuppetTransformCfg.tavern_battle_team_cfg.rotation = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					d3DPuppetTransformCfg.tavern_battle_team_cfg.size = float.Parse(xmlElement.GetAttribute("my:CameraSize").Trim());
				}
			}
			if (string.Empty == empty && DefaultPuppetTransform == null)
			{
				DefaultPuppetTransform = d3DPuppetTransformCfg;
			}
			else
			{
				PuppetTransformManager.Add(empty, d3DPuppetTransformCfg);
			}
		}
	}

	public void LoadD3DCustomLootFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DCustomLoot File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "E8pA*AO18Fu5.^DZ");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DCustomLoot d3DCustomLoot = new D3DCustomLoot();
			XmlElement xmlElement = (XmlElement)item;
			d3DCustomLoot.LootID = xmlElement.GetAttribute("my:LootID").Trim();
			string text3 = xmlElement.GetAttribute("my:CustomOdds").Trim();
			if (string.Empty != text3)
			{
				string[] array = text3.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					int num2 = 3 - i;
					if (num2 < 0)
					{
						break;
					}
					if (string.Empty != array[i])
					{
						d3DCustomLoot.CustomOdds[num2] = new D3DFloat(float.Parse(array[i]));
					}
				}
			}
			foreach (XmlNode item2 in item)
			{
				if ("my:LootItem" == item2.Name)
				{
					string value = item2.ChildNodes[0].Value;
					if (string.Empty != value)
					{
						d3DCustomLoot.LootEquipmentIDList.Add(item2.ChildNodes[0].Value);
					}
				}
			}
			if (string.Empty != d3DCustomLoot.LootID)
			{
				D3DCustomLootManager.Add(d3DCustomLoot.LootID, d3DCustomLoot);
			}
			else if (diagnose)
			{
				num++;
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text4 = text;
		text = text4 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item3 in list)
		{
			text = text + item3 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DCustomLootBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DCustomLootFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public List<D3DEquipment> LootRandomEquipments(string custom_loot_id, int custom_loot_count, D3DFloat[] random_loot_odds, int min_level, int max_level, int loot_level, int loot_count)
	{
		if (loot_count <= 0)
		{
			return null;
		}
		if (min_level > max_level)
		{
			int num = max_level;
			min_level = max_level;
			max_level = num;
		}
		List<D3DEquipment> list = new List<D3DEquipment>();
		List<D3DEquipment> list2 = new List<D3DEquipment>();
		List<D3DEquipment> list3 = new List<D3DEquipment>();
		List<D3DEquipment> list4 = new List<D3DEquipment>();
		foreach (string key in D3DEquipmentManager.Keys)
		{
			if (D3DEquipmentManager[key].require_level >= min_level && D3DEquipmentManager[key].require_level <= max_level)
			{
				if (D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.NORMAL)
				{
					list2.Add(D3DEquipmentManager[key]);
				}
				else if (D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.SUPERIOR)
				{
					list3.Add(D3DEquipmentManager[key]);
				}
				else if (D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.MAGIC)
				{
					list4.Add(D3DEquipmentManager[key]);
				}
			}
		}
		float num2 = ((random_loot_odds[0] != null && !(string.Empty == custom_loot_id) && D3DCustomLootManager.ContainsKey(custom_loot_id)) ? random_loot_odds[0].value : 0f);
		float num3 = ((random_loot_odds[1] != null) ? random_loot_odds[1].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.MAGIC, loot_level));
		float num4 = ((random_loot_odds[2] != null) ? random_loot_odds[2].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.SUPERIOR, loot_level));
		for (int i = 0; i < loot_count; i++)
		{
			D3DEquipment d3DEquipment = null;
			if (num2 > 0f && custom_loot_count > 0)
			{
				custom_loot_count--;
				if (Instance.Lottery(num2))
				{
					d3DEquipment = D3DCustomLootManager[custom_loot_id].LootCustomEquipment(loot_level);
					if (d3DEquipment != null)
					{
						d3DEquipment = d3DEquipment.Clone();
						d3DEquipment.EndowRandomMagicPower();
						list.Add(d3DEquipment);
						continue;
					}
				}
			}
			if (num3 > 0f && list4.Count > 0 && Instance.Lottery(num3))
			{
				do
				{
					d3DEquipment = list4[Random.Range(0, list4.Count)].Clone();
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				d3DEquipment.EndowRandomMagicPower();
				list.Add(d3DEquipment);
			}
			else if (num4 > 0f && list3.Count > 0 && Instance.Lottery(num4))
			{
				do
				{
					d3DEquipment = list3[Random.Range(0, list3.Count)].Clone();
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				d3DEquipment.EndowRandomMagicPower();
				list.Add(d3DEquipment);
			}
			else if (list2.Count > 0)
			{
				do
				{
					d3DEquipment = list2[Random.Range(0, list2.Count)].Clone();
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				d3DEquipment.EndowRandomMagicPower();
				list.Add(d3DEquipment);
			}
		}
		return list;
	}

	public List<D3DEquipment> LootRandomEquipments(string custom_loot_id, int custom_loot_count, D3DFloat[] random_loot_odds, int min_level, int max_level, int loot_level, int loot_count, List<D3DInt> GoldBag, float[] gold_bag_odds, int gold_basic)
	{
		if (loot_count <= 0)
		{
			return null;
		}
		if (min_level > max_level)
		{
			int num = max_level;
			min_level = max_level;
			max_level = num;
		}
		List<D3DEquipment> list = new List<D3DEquipment>();
		List<D3DEquipment> list2 = new List<D3DEquipment>();
		List<D3DEquipment> list3 = new List<D3DEquipment>();
		List<D3DEquipment> list4 = new List<D3DEquipment>();
		foreach (string key in D3DEquipmentManager.Keys)
		{
			if (D3DEquipmentManager[key].require_level >= min_level && D3DEquipmentManager[key].require_level <= max_level)
			{
				if (D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.NORMAL)
				{
					list2.Add(D3DEquipmentManager[key]);
				}
				else if (D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.SUPERIOR)
				{
					list3.Add(D3DEquipmentManager[key]);
				}
				else if (D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.MAGIC)
				{
					list4.Add(D3DEquipmentManager[key]);
				}
			}
		}
		float num2 = ((random_loot_odds[0] != null && !(string.Empty == custom_loot_id) && D3DCustomLootManager.ContainsKey(custom_loot_id)) ? random_loot_odds[0].value : 0f);
		float num3 = ((random_loot_odds[1] != null) ? random_loot_odds[1].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.MAGIC, loot_level));
		float num4 = ((random_loot_odds[2] != null) ? random_loot_odds[2].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.SUPERIOR, loot_level));
		for (int i = 0; i < loot_count; i++)
		{
			D3DEquipment d3DEquipment = null;
			if (num2 > 0f && custom_loot_count > 0)
			{
				custom_loot_count--;
				if (Instance.Lottery(num2))
				{
					d3DEquipment = D3DCustomLootManager[custom_loot_id].LootCustomEquipment(loot_level);
					if (d3DEquipment != null)
					{
						d3DEquipment = d3DEquipment.Clone();
						d3DEquipment.EndowRandomMagicPower();
						list.Add(d3DEquipment);
						GoldBag.Add(null);
						continue;
					}
				}
			}
			if (num3 > 0f && list4.Count > 0 && Instance.Lottery(num3))
			{
				float lottery_odds = ((gold_bag_odds[0] != -1f) ? gold_bag_odds[0] : D3DBattleRule.Instance.GoldBagOdds[0]);
				bool flag = Instance.Lottery(lottery_odds);
				do
				{
					d3DEquipment = list4[Random.Range(0, list4.Count)].Clone();
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				list.Add(d3DEquipment);
				if (flag)
				{
					GoldBag.Add(new D3DInt(Mathf.RoundToInt((float)gold_basic * D3DBattleRule.Instance.GoldBagCoe[0])));
					continue;
				}
				d3DEquipment.EndowRandomMagicPower();
				GoldBag.Add(null);
			}
			else if (num4 > 0f && list3.Count > 0 && Instance.Lottery(num4))
			{
				float lottery_odds2 = ((gold_bag_odds[1] != -1f) ? gold_bag_odds[1] : D3DBattleRule.Instance.GoldBagOdds[1]);
				bool flag2 = Instance.Lottery(lottery_odds2);
				do
				{
					d3DEquipment = list3[Random.Range(0, list3.Count)].Clone();
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				list.Add(d3DEquipment);
				if (flag2)
				{
					GoldBag.Add(new D3DInt(Mathf.RoundToInt((float)gold_basic * D3DBattleRule.Instance.GoldBagCoe[1])));
					continue;
				}
				d3DEquipment.EndowRandomMagicPower();
				GoldBag.Add(null);
			}
			else if (list2.Count > 0)
			{
				float lottery_odds3 = ((gold_bag_odds[2] != -1f) ? gold_bag_odds[2] : D3DBattleRule.Instance.GoldBagOdds[2]);
				bool flag3 = Instance.Lottery(lottery_odds3);
				do
				{
					d3DEquipment = list2[Random.Range(0, list2.Count)].Clone();
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				list.Add(d3DEquipment);
				if (flag3)
				{
					GoldBag.Add(new D3DInt(Mathf.RoundToInt((float)gold_basic * D3DBattleRule.Instance.GoldBagCoe[2])));
					continue;
				}
				d3DEquipment.EndowRandomMagicPower();
				GoldBag.Add(null);
			}
		}
		return list;
	}

	public List<D3DEquipment> LootRandomEquipmentsOnBattleWin(EnemyGroupBattleData group_data, D3DDungeonFloor dungeon_floor)
	{
		List<D3DEquipment> list = new List<D3DEquipment>();
		foreach (D3DGearLoot loot in group_data.temp_group.Loots)
		{
			if (Lottery(loot.loot_odds))
			{
				List<D3DEquipment> list2 = LootRandomEquipments(loot.custom_loot_id, 1, loot.random_loot_odds, dungeon_floor.LootLevelMin, dungeon_floor.LootLevelMax, group_data.group_level, 1);
				if (list2 != null && list2.Count > 0)
				{
					list.Add(list2[0]);
				}
			}
		}
		return list;
	}

	public List<D3DEquipment> LootRandomEquipmentsOnOpenTreasure(D3DTreasure treasure, D3DDungeonFloor dungeon_floor, int loot_count)
	{
		List<D3DEquipment> list = new List<D3DEquipment>();
		foreach (D3DGearLoot loot in treasure.Loots)
		{
			if (Lottery(loot.loot_odds))
			{
				List<D3DEquipment> list2 = LootRandomEquipments(loot.custom_loot_id, 1, loot.random_loot_odds, dungeon_floor.LootLevelMin, dungeon_floor.LootLevelMax, dungeon_floor.RandomFloorLevel, 1);
				if (list2 != null && list2.Count > 0)
				{
					list.Add(list2[0]);
				}
			}
		}
		return list;
	}

	public void LoadD3DTreasureFromFile(string file_path, bool diagnose)
	{
		int num = 0;
		List<string> list = new List<string>();
		string text = string.Empty;
		if (diagnose)
		{
			text += ">===============================<\n";
			text = text + "Read D3DTreasure File,Path is " + file_path + "\n";
		}
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "iFoGQ[DqHy0Cgz]b");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DTreasure d3DTreasure = new D3DTreasure();
			XmlElement xmlElement = (XmlElement)item;
			d3DTreasure.treasure_id = xmlElement.GetAttribute("my:TreasureID").Trim();
			d3DTreasure.big_chest = bool.Parse(xmlElement.GetAttribute("my:BigBox").Trim());
			for (int i = 1; i < 4; i++)
			{
				float num2 = float.Parse(xmlElement.GetAttribute("my:LootEnableOdds" + i).Trim());
				if (num2 == 0f)
				{
					continue;
				}
				D3DGearLoot d3DGearLoot = new D3DGearLoot();
				d3DGearLoot.loot_odds = num2;
				d3DGearLoot.custom_loot_id = xmlElement.GetAttribute("my:CustomLootID" + ((i != 1) ? i.ToString() : string.Empty)).Trim();
				string text3 = xmlElement.GetAttribute("my:CustomOdds" + ((i != 1) ? i.ToString() : string.Empty)).Trim();
				if (string.Empty != text3)
				{
					string[] array = text3.Split(',');
					for (int j = 0; j < array.Length; j++)
					{
						if (string.Empty != array[j])
						{
							d3DGearLoot.random_loot_odds[j] = new D3DFloat(float.Parse(array[j]));
						}
					}
				}
				d3DTreasure.Loots.Add(d3DGearLoot);
			}
			if (string.Empty != d3DTreasure.treasure_id)
			{
				D3DTreasureManager.Add(d3DTreasure.treasure_id, d3DTreasure);
			}
			else if (diagnose)
			{
				num++;
			}
		}
		if (!diagnose)
		{
			return;
		}
		string text4 = text;
		text = text4 + "Failed Count:" + num + "\n";
		text += "Overlaps:\n";
		foreach (string item2 in list)
		{
			text = text + item2 + "\n";
		}
		text += ">------------------------------<";
	}

	public void LoadD3DTreasureBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DTreasureFromFile(folder_path + "/" + @object.name, true);
		}
	}

	public void LoadD3DMagicPower(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			if ("my:PowerCoe" == item.Name)
			{
				D3DMagicPower.Instance.CommonCoe[0] = float.Parse(xmlElement.GetAttribute("my:CCoe").Trim());
				D3DMagicPower.Instance.CommonCoe[1] = float.Parse(xmlElement.GetAttribute("my:SCoe").Trim());
				D3DMagicPower.Instance.NecklaceCoe[0] = float.Parse(xmlElement.GetAttribute("my:NCoe").Trim());
				D3DMagicPower.Instance.NecklaceCoe[1] = float.Parse(xmlElement.GetAttribute("my:NSCoe").Trim());
				D3DMagicPower.Instance.RingCoe[0] = float.Parse(xmlElement.GetAttribute("my:RCoe").Trim());
				D3DMagicPower.Instance.RingCoe[1] = float.Parse(xmlElement.GetAttribute("my:RSCoe").Trim());
				D3DMagicPower.Instance.MPCoe = float.Parse(xmlElement.GetAttribute("my:MPCoe").Trim());
			}
			else if ("my:PowerRatio" == item.Name)
			{
				D3DMagicPower.Instance.PowerRatioCoe[0] = float.Parse(xmlElement.GetAttribute("my:ARatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[1] = float.Parse(xmlElement.GetAttribute("my:HRatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[2] = float.Parse(xmlElement.GetAttribute("my:SRatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[3] = float.Parse(xmlElement.GetAttribute("my:ORatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[4] = float.Parse(xmlElement.GetAttribute("my:DRatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[5] = float.Parse(xmlElement.GetAttribute("my:SHRatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[6] = float.Parse(xmlElement.GetAttribute("my:NecklaceRatio").Trim());
				D3DMagicPower.Instance.PowerRatioCoe[7] = float.Parse(xmlElement.GetAttribute("my:RingRatio").Trim());
			}
			else if ("my:Adjust" == item.Name)
			{
				D3DMagicPower.Instance.StaminaAdjustCoe[0] = float.Parse(xmlElement.GetAttribute("my:PAdjust").Trim());
				D3DMagicPower.Instance.StaminaAdjustCoe[1] = float.Parse(xmlElement.GetAttribute("my:LAdjust").Trim());
				D3DMagicPower.Instance.StaminaAdjustCoe[2] = float.Parse(xmlElement.GetAttribute("my:RAdjust").Trim());
				D3DMagicPower.Instance.StaminaAdjustCoe[3] = float.Parse(xmlElement.GetAttribute("my:OAdjust").Trim());
			}
			else if ("my:Redress" == item.Name)
			{
				D3DMagicPower.Instance.RedressCoe[0] = float.Parse(xmlElement.GetAttribute("my:ToStr").Trim());
				D3DMagicPower.Instance.RedressCoe[1] = float.Parse(xmlElement.GetAttribute("my:ToAgi").Trim());
				D3DMagicPower.Instance.RedressCoe[2] = float.Parse(xmlElement.GetAttribute("my:ToInt").Trim());
				D3DMagicPower.Instance.RedressCoe[3] = float.Parse(xmlElement.GetAttribute("my:ToSpi").Trim());
				D3DMagicPower.Instance.RedressCoe[4] = float.Parse(xmlElement.GetAttribute("my:CommonToSta").Trim());
			}
			else
			{
				if (!("my:Rule" == item.Name))
				{
					continue;
				}
				PowerRule powerRule = new PowerRule();
				powerRule.rule_id = xmlElement.GetAttribute("my:RuleID").Trim();
				if (string.Empty == powerRule.rule_id || D3DMagicPower.Instance.RuleManager.ContainsKey(powerRule.rule_id))
				{
					continue;
				}
				powerRule.affix = xmlElement.GetAttribute("my:Affix").Trim();
				foreach (XmlNode item2 in item)
				{
					if ("my:RulePower" == item2.Name)
					{
						PowerRule.PowerValue powerValue = new PowerRule.PowerValue();
						powerValue.power_type = int.Parse(((XmlElement)item2).GetAttribute("my:Power").Trim());
						powerValue.min = float.Parse(((XmlElement)item2).GetAttribute("my:Min").Trim());
						powerValue.max = float.Parse(((XmlElement)item2).GetAttribute("my:Max").Trim());
						powerRule.power_value.Add(powerValue);
					}
				}
				D3DMagicPower.Instance.RuleManager.Add(powerRule.rule_id, powerRule);
			}
		}
	}

	public void LoadD3DTavern(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			if ("my:Synopsis" == item.Name)
			{
				HeroSynopsis heroSynopsis = new HeroSynopsis();
				heroSynopsis.puppet_id = xmlElement.GetAttribute("my:PuppetID").Trim();
				if (!(string.Empty != heroSynopsis.puppet_id) || D3DTavern.Instance.HeroSynopsisManager.ContainsKey(heroSynopsis.puppet_id))
				{
					continue;
				}
				heroSynopsis.expert_skill_id = xmlElement.GetAttribute("my:Skill").Trim();
				foreach (XmlNode item2 in item)
				{
					if ("my:Intro" == item2.Name && string.Empty != item2.ChildNodes[0].Value)
					{
						heroSynopsis.puppet_intro.Add(item2.ChildNodes[0].Value);
					}
				}
				D3DTavern.Instance.HeroSynopsisManager.Add(heroSynopsis.puppet_id, heroSynopsis);
			}
			else if ("my:Hire" == item.Name)
			{
				HeroHire heroHire = new HeroHire();
				heroHire.puppet_id = xmlElement.GetAttribute("my:PuppetID").Trim();
				if (string.Empty != heroHire.puppet_id)
				{
					heroHire.hire_cost = int.Parse(xmlElement.GetAttribute("my:Cost").Trim());
					heroHire.hire_crystal = int.Parse(xmlElement.GetAttribute("my:CostCrystal").Trim());
					heroHire.default_level = int.Parse(xmlElement.GetAttribute("my:Level").Trim());
					heroHire.unlock_group = xmlElement.GetAttribute("my:UnlockEnemyGroup").Trim();
					D3DTavern.Instance.HeroHireManager.Add(heroHire);
				}
			}
			else if ("my:Default" == item.Name)
			{
				D3DTavern.Instance.DefaultHeros[0] = xmlElement.GetAttribute("my:Hero1").Trim();
				D3DTavern.Instance.DefaultHeros[1] = xmlElement.GetAttribute("my:Hero2").Trim();
				D3DTavern.Instance.DefaultHeros[2] = xmlElement.GetAttribute("my:Hero3").Trim();
			}
		}
	}

	public void LoadD3DHelmConfig(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			if (!("my:HideConfig" == item.Name))
			{
				continue;
			}
			D3DHelmConfig d3DHelmConfig = new D3DHelmConfig();
			d3DHelmConfig.helm_model_name = xmlElement.GetAttribute("my:HelmModelName").Trim();
			if (!(string.Empty != d3DHelmConfig.helm_model_name) || D3DHelmConfigManager.ContainsKey(d3DHelmConfig.helm_model_name))
			{
				continue;
			}
			foreach (XmlNode item2 in item)
			{
				if ("my:PartsConfig" == item2.Name)
				{
					XmlElement xmlElement2 = (XmlElement)item2;
					d3DHelmConfig.draw_ears = bool.Parse(xmlElement2.GetAttribute("my:ears").Trim());
					d3DHelmConfig.draw_hair = bool.Parse(xmlElement2.GetAttribute("my:hair").Trim());
					d3DHelmConfig.draw_nose = bool.Parse(xmlElement2.GetAttribute("my:nose").Trim());
					d3DHelmConfig.draw_beard = bool.Parse(xmlElement2.GetAttribute("my:beard").Trim());
				}
			}
			D3DHelmConfigManager.Add(d3DHelmConfig.helm_model_name, d3DHelmConfig);
		}
	}

	public void LoadD3DDungeonModelPresetFromFile(string file_path)
	{
		string text = ">===============================<\n";
		text = text + "Read D3DDungeonModelPreset File,Path is " + file_path + "\n";
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "U]xcQ0I^lqH]JDyh");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			D3DDungeonModelPreset.ModelPreset modelPreset = new D3DDungeonModelPreset.ModelPreset();
			XmlElement xmlElement = (XmlElement)item;
			modelPreset.PresetID = xmlElement.GetAttribute("my:PresetID").Trim();
			modelPreset.ModelPostfix = xmlElement.GetAttribute("my:ModelPostfix").Trim();
			foreach (XmlNode item2 in item)
			{
				if ("my:EnabledObstacle" == item2.Name)
				{
					if (modelPreset.EnabledObstacles == null)
					{
						modelPreset.EnabledObstacles = new List<string>();
					}
					modelPreset.EnabledObstacles.Add(item2.ChildNodes[0].Value);
				}
			}
			D3DDungeonModelPreset.Instance.AddModelPreset(modelPreset);
		}
	}

	public void LoadD3DDungeonModelPresetBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DDungeonModelPresetFromFile(folder_path + "/" + @object.name);
		}
	}

	public void LoadD3DBattlePosition(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			if ("my:PlayerPosition" == item.Name)
			{
				string[] array = xmlElement.GetAttribute("my:Hero1").Trim().Split(',');
				D3DBattlePreset.Instance.HeroPoints[0] = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
				array = xmlElement.GetAttribute("my:Hero2").Trim().Split(',');
				D3DBattlePreset.Instance.HeroPoints[1] = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
				array = xmlElement.GetAttribute("my:Hero3").Trim().Split(',');
				D3DBattlePreset.Instance.HeroPoints[2] = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
			}
			else if ("my:SpawnerConfig" == item.Name)
			{
				D3DBattlePreset.SpawnerConfig spawnerConfig = new D3DBattlePreset.SpawnerConfig();
				foreach (XmlNode item2 in item)
				{
					if ("my:ModelPostfix" == item2.Name)
					{
						spawnerConfig.ModlePostfix.Add(item2.FirstChild.Value);
					}
					else if ("my:SpawnerLine" == item2.Name)
					{
						Vector2[] array2 = new Vector2[2];
						string[] array3 = ((XmlElement)item2).GetAttribute("my:Vector1").Trim().Split(',');
						array2[0] = new Vector2(float.Parse(array3[0]), float.Parse(array3[1]));
						array3 = ((XmlElement)item2).GetAttribute("my:Vector2").Trim().Split(',');
						array2[1] = new Vector2(float.Parse(array3[0]), float.Parse(array3[1]));
						spawnerConfig.SpawnerLine.Add(array2);
					}
				}
				if (spawnerConfig.ModlePostfix.Count == 0)
				{
					D3DBattlePreset.Instance.DefaultSpawnerConfig = spawnerConfig;
				}
				else
				{
					D3DBattlePreset.Instance.SpawnerConfigList.Add(spawnerConfig);
				}
			}
			else if ("my:CustomEffect" == item.Name)
			{
				string key = xmlElement.GetAttribute("my:ID").Trim();
				string value = xmlElement.GetAttribute("my:Effect").Trim();
				D3DBattlePreset.Instance.CustomSpawnEffect.Add(key, value);
			}
		}
	}

	public void LoadD3DHowTo(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if ("my:Tutorial" == item.Name)
			{
				D3DHowTo.Instance.NewGameStory = ((XmlElement)item).GetAttribute("my:NewGameStory").Trim();
				int num = 0;
				foreach (XmlNode item2 in item)
				{
					XmlElement xmlElement = (XmlElement)item2;
					D3DHowTo.Tutorial tutorial = new D3DHowTo.Tutorial();
					tutorial.trigger_delay = float.Parse(xmlElement.GetAttribute("my:TriggerDelay").Trim());
					tutorial.ill_stay = float.Parse(xmlElement.GetAttribute("my:IllStay").Trim());
					foreach (XmlNode item3 in item2)
					{
						if ("my:Ill" == item3.Name)
						{
							tutorial.TutorialIll.Add(item3.FirstChild.Value.Trim());
						}
					}
					D3DHowTo.Instance.AddTutorial((D3DHowTo.TutorialType)num, tutorial);
					num++;
				}
			}
			else
			{
				if (!("my:HowTo" == item.Name))
				{
					continue;
				}
				foreach (XmlNode item4 in item)
				{
					if (!("my:HowToCfg" == item4.Name))
					{
						continue;
					}
					string title = ((XmlElement)item4).GetAttribute("my:Title").Trim();
					List<string> list = new List<string>();
					foreach (XmlNode item5 in item4)
					{
						if ("my:Ill" == item5.Name)
						{
							list.Add(item5.FirstChild.Value.Trim());
						}
					}
					D3DHowTo.Instance.AddHowToCfg(title, list);
				}
			}
		}
	}

	public void LoadD3DDungeonNpcConfigFromFile(string file_path)
	{
		string text = ">===============================<\n";
		text = text + "Read D3DDungeonNpcConfig File,Path is " + file_path + "\n";
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text2 = textAsset.text;
		text2 = XXTEAUtils.Decrypt(text2, "%dsXdyaNAU.IugTn");
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text2);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			string dungeon_id = xmlElement.GetAttribute("my:DungeonID").Trim();
			Dictionary<int, DungeonNpcConfig.LevelNpcConfig> dictionary = new Dictionary<int, DungeonNpcConfig.LevelNpcConfig>();
			foreach (XmlNode item2 in item)
			{
				if (!("my:LevelNpc" == item2.Name))
				{
					continue;
				}
				xmlElement = (XmlElement)item2;
				int key = int.Parse(xmlElement.GetAttribute("my:Level").Trim());
				DungeonNpcConfig.LevelNpcConfig levelNpcConfig = new DungeonNpcConfig.LevelNpcConfig();
				foreach (XmlNode item3 in item2)
				{
					if ("my:InteractiveNpc" == item3.Name)
					{
						if (levelNpcConfig.level_interactive_npc == null)
						{
							levelNpcConfig.level_interactive_npc = new List<D3DInteractiveNpc>();
						}
						XmlElement xmlElement2 = (XmlElement)item3;
						D3DInteractiveNpc d3DInteractiveNpc = new D3DInteractiveNpc();
						d3DInteractiveNpc.PuppetID = xmlElement2.GetAttribute("my:PuppetID").Trim();
						d3DInteractiveNpc.npcFunction = (D3DInteractiveNpc.NpcFunction)int.Parse(xmlElement2.GetAttribute("my:Function").Trim());
						string text3 = xmlElement2.GetAttribute("my:NpcPosition").Trim();
						string[] array = text3.Split(',');
						d3DInteractiveNpc.NpcPosition = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
						d3DInteractiveNpc.NpcRotationY = float.Parse(xmlElement2.GetAttribute("my:NpcRotation").Trim());
						levelNpcConfig.level_interactive_npc.Add(d3DInteractiveNpc);
					}
				}
				dictionary.Add(key, levelNpcConfig);
			}
			DungeonNpcConfig.Instance.AddNpcConfig(dungeon_id, dictionary);
		}
	}

	public void LoadD3DDungeonNpcConfigBatch(string folder_path)
	{
		Object[] array = Resources.LoadAll(folder_path, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			LoadD3DDungeonNpcConfigFromFile(folder_path + "/" + @object.name);
		}
	}

	public void LoadD3DTexts(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			if ("my:MessageBoxContents" == item.Name)
			{
				foreach (XmlNode item2 in item)
				{
					List<string> list = new List<string>();
					foreach (XmlNode item3 in item2)
					{
						if ("my:Content" == item3.Name)
						{
							if (item3.ChildNodes[0] == null)
							{
								list.Add(string.Empty);
							}
							else
							{
								list.Add(item3.ChildNodes[0].Value);
							}
						}
					}
					D3DTexts.Instance.AddMsgBoxContent(list);
				}
			}
			else
			{
				if (!("my:IapTexts" == item.Name))
				{
					continue;
				}
				foreach (XmlNode item4 in item)
				{
					string name = ((XmlElement)item4).GetAttribute("my:IapName").Trim();
					List<string> list2 = new List<string>();
					foreach (XmlNode item5 in item4)
					{
						if ("my:Content" == item5.Name)
						{
							list2.Add(item5.ChildNodes[0].Value);
						}
					}
					D3DTexts.Instance.AddTBankText(name, list2);
				}
			}
		}
	}

	public void LoadTutorialHins(string file_path)
	{
		D3DTutorialHintCfg.LoadConfig(file_path);
	}

	public void LoadD3DStory(string file_path)
	{
		TextAsset textAsset = Resources.Load(file_path) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			XmlElement xmlElement = (XmlElement)item;
			if (!("my:StorySet" == xmlElement.Name))
			{
				continue;
			}
			string id = xmlElement.GetAttribute("my:StoryID").Trim();
			D3DStoryManager.Story story = new D3DStoryManager.Story();
			story.story_bgm = (D3DStoryManager.Story.StoryBgm)int.Parse(xmlElement.GetAttribute("my:Bgm").Trim());
			story.story_phase = new List<D3DStoryManager.StoryPhase>();
			foreach (XmlNode item2 in item)
			{
				if (!("my:Phase" == item2.Name))
				{
					continue;
				}
				D3DStoryManager.StoryPhase storyPhase = new D3DStoryManager.StoryPhase();
				foreach (XmlNode item3 in item2)
				{
					if ("my:Illustration" == item3.Name)
					{
						storyPhase.Illustration = item3.FirstChild.Value;
					}
					else
					{
						if (!("my:Texts" == item3.Name))
						{
							continue;
						}
						List<string> list = new List<string>();
						foreach (XmlNode item4 in item3)
						{
							if ("my:Contents" == item4.Name)
							{
								list.Add(item4.FirstChild.Value);
							}
						}
						storyPhase.contents.Add(list);
					}
				}
				story.story_phase.Add(storyPhase);
			}
			D3DStoryManager.Instance.AddStoryPreset(id, story);
		}
	}

	public bool Lottery(float lottery_odds)
	{
		if (lottery_odds <= 0f)
		{
			return false;
		}
		float num = Random.Range(0f, 1f - lottery_odds);
		float num2 = Random.Range(0f, 1f);
		if (num2 >= num && num2 <= num + lottery_odds)
		{
			return true;
		}
		return false;
	}

	public void SetGameObjectMeshColor(GameObject obj, Color color)
	{
		if (null == obj)
		{
			return;
		}
		Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
		if (!(null == mesh))
		{
			Vector3[] vertices = mesh.vertices;
			Color[] array = new Color[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = color;
			}
			mesh.colors = array;
		}
	}

	public void SetGameObjectGeneralLayer(GameObject game_obj, int layer)
	{
		game_obj.layer = layer;
		int childCount = game_obj.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = game_obj.transform.GetChild(i).gameObject;
			SetGameObjectGeneralLayer(gameObject, layer);
		}
	}

	public GameObject FindGameObjectChild(GameObject game_obj, string find_name)
	{
		GameObject gameObject = null;
		int childCount = game_obj.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject2 = game_obj.transform.GetChild(i).gameObject;
			if (gameObject2.name == find_name)
			{
				gameObject = gameObject2;
				break;
			}
			gameObject = FindGameObjectChild(gameObject2, find_name);
			if (null != gameObject && gameObject.name == find_name)
			{
				break;
			}
		}
		return gameObject;
	}

	public Color GetEquipmentGradeColor(D3DEquipment.EquipmentGrade grade)
	{
		switch (grade)
		{
		case D3DEquipment.EquipmentGrade.INFERIOR:
			return new Color(0.6156863f, 0.6156863f, 0.6156863f);
		case D3DEquipment.EquipmentGrade.NORMAL:
			return Color.white;
		case D3DEquipment.EquipmentGrade.SUPERIOR:
			return new Color(0.11764706f, 50f / 51f, 0f);
		case D3DEquipment.EquipmentGrade.MAGIC:
			return new Color(0f, 0.4392157f, 13f / 15f);
		case D3DEquipment.EquipmentGrade.RARE:
			return new Color(0.6392157f, 0.20784314f, 14f / 15f);
		case D3DEquipment.EquipmentGrade.EX_RARE:
			return new Color(0.6392157f, 0.20784314f, 14f / 15f);
		case D3DEquipment.EquipmentGrade.IAP:
			return new Color(0.2901961f, 76f / 85f, 72f / 85f);
		default:
			return Color.red;
		}
	}

	public string GetEquipmentTypeDes(D3DEquipment.EquipmentType type)
	{
		switch (type)
		{
		case D3DEquipment.EquipmentType.ONE_HAND:
			return "1-Hand";
		case D3DEquipment.EquipmentType.OFF_HAND:
			return "Off-Hand";
		case D3DEquipment.EquipmentType.TWO_HAND:
			return "2-Hand";
		case D3DEquipment.EquipmentType.BOW_HAND:
			return "2-Hand";
		case D3DEquipment.EquipmentType.HELM:
			return "Head";
		case D3DEquipment.EquipmentType.ARMOR:
			return "Armor";
		case D3DEquipment.EquipmentType.BOOTS:
			return "Boots";
		case D3DEquipment.EquipmentType.ACCESSORY:
			return "Accessory";
		default:
			return "error";
		}
	}

	public string GetEquipmentClassDes(D3DEquipment.EquipmentClass item_class)
	{
		switch (item_class)
		{
		case D3DEquipment.EquipmentClass.AXE:
			return "Axe";
		case D3DEquipment.EquipmentClass.BOW:
			return "Bow";
		case D3DEquipment.EquipmentClass.SWORD:
			return "Sword";
		case D3DEquipment.EquipmentClass.CLAYMORE:
			return "Claymore";
		case D3DEquipment.EquipmentClass.DAGGER:
			return "Dragger";
		case D3DEquipment.EquipmentClass.HAMMER:
			return "Hammer";
		case D3DEquipment.EquipmentClass.SHIELD:
			return "Shield";
		case D3DEquipment.EquipmentClass.STAFF:
			return "Staff";
		case D3DEquipment.EquipmentClass.ROBE:
			return "Robe";
		case D3DEquipment.EquipmentClass.LEATHER:
			return "Leather";
		case D3DEquipment.EquipmentClass.PLATE:
			return "Plate";
		case D3DEquipment.EquipmentClass.NECKLACE:
			return "Necklace";
		case D3DEquipment.EquipmentClass.RING:
			return "Ring";
		default:
			return "error";
		}
	}

	public void AddCameraAdjust()
	{
		if (null != Camera.main && null == Camera.main.GetComponent<CameraAdjustForiPad>())
		{
			Camera.main.gameObject.AddComponent<CameraAdjustForiPad>();
		}
	}

	public Rect ConvertRectAutoHD(Rect rect)
	{
		return new Rect(rect.x * (float)Instance.HD_SIZE, rect.y * (float)Instance.HD_SIZE, rect.width * (float)Instance.HD_SIZE, rect.height * (float)Instance.HD_SIZE);
	}

	public Rect ConvertRectAutoHD(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Instance.HD_SIZE, y * (float)Instance.HD_SIZE, width * (float)Instance.HD_SIZE, height * (float)Instance.HD_SIZE);
	}

	public string RemoveIllegalCharacter(string str)
	{
		char[] anyOf = new char[10] { '\\', '/', ':', '*', '?', '"', '<', '>', '|', '=' };
		while (true)
		{
			int num = str.IndexOfAny(anyOf);
			if (num == -1)
			{
				break;
			}
			str = str.Remove(num, 1);
		}
		return str;
	}
}
