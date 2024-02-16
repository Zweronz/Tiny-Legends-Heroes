using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillTriggerExecuter : MonoBehaviour
{
	private PuppetArena skill_caster;

	private SceneArena scene_arena;

	private int caster_level;

	private int skill_level;

	private float camera_shake_time;

	private bool shake_target;

	private bool special_hatred_used;

	private PuppetArena default_target;

	private Vector3 caster_position;

	private Quaternion caster_rotation;

	private Vector3 default_target_position;

	private Quaternion default_target_rotation;

	private Vector3 target_position;

	private Quaternion target_rotation;

	private ActiveSkillTrigger skill_trigger;

	private D3DPuppetVariableData caster_variabledata;

	private List<PuppetArena> friend_targets = new List<PuppetArena>();

	private List<PuppetArena> enemy_targets = new List<PuppetArena>();

	private void Start()
	{
	}

	private void Update()
	{
		if (null != skill_caster && skill_caster.IsInGrave())
		{
			skill_caster = null;
		}
	}

	public void Init(PuppetArena skill_caster, Vector3 caster_position, Quaternion caster_rotation, int caster_level, int skill_level, PuppetArena default_target, Vector3 default_target_position, Quaternion default_target_rotation, Vector3 target_position, Quaternion target_rotation, ActiveSkillTrigger skill_trigger, SceneArena scene_arena, D3DPuppetVariableData caster_variabledata, float camera_shake_time, bool shake_target)
	{
		this.caster_position = caster_position;
		this.caster_rotation = caster_rotation;
		this.default_target_position = default_target_position;
		this.default_target_rotation = default_target_rotation;
		this.skill_caster = skill_caster;
		this.caster_level = caster_level;
		this.skill_level = skill_level;
		this.default_target = default_target;
		this.target_position = target_position;
		this.target_rotation = target_rotation;
		this.skill_trigger = skill_trigger;
		this.scene_arena = scene_arena;
		this.caster_variabledata = caster_variabledata;
		this.camera_shake_time = camera_shake_time;
		this.shake_target = shake_target;
	}

	public void Execute()
	{
		if (scene_arena.IsBattleWinBehaviour)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (camera_shake_time > 0f)
		{
			CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
			if (null == cameraShake)
			{
				cameraShake = Camera.main.gameObject.AddComponent<CameraShake>();
			}
			cameraShake.Reset(camera_shake_time);
		}
		FilterSkillTargets();
		ExecuteVariableConfig();
		ExecuteSpecial();
		ExecuteCrowdControl();
		ExecuteBuff();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void FilterSkillTargets()
	{
		if (skill_trigger.area_of_effect != null)
		{
			List<PuppetArena> list = null;
			List<PuppetArena> list2 = null;
			AreaOfEffect.AreaConfig areaConfigs = skill_trigger.area_of_effect.GetAreaConfigs(skill_level);
			if (skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.ALL || skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.ALL_EXCLUDE_ME)
			{
				list = new List<PuppetArena>();
				list2 = new List<PuppetArena>();
				if ("TriggerPlayer" == base.tag)
				{
					list.AddRange(scene_arena.playerList);
					list2.AddRange(scene_arena.enemyList);
				}
				else if ("TriggerEnemy" == base.tag)
				{
					list.AddRange(scene_arena.enemyList);
					list2.AddRange(scene_arena.playerList);
				}
			}
			else if (skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.FRIEND || skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.FRIEND_EXCLUDE_ME)
			{
				list = new List<PuppetArena>();
				if ("TriggerPlayer" == base.tag)
				{
					list.AddRange(scene_arena.playerList);
				}
				else if ("TriggerEnemy" == base.tag)
				{
					list.AddRange(scene_arena.enemyList);
				}
			}
			else if (skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.ENEMY)
			{
				list2 = new List<PuppetArena>();
				if ("TriggerPlayer" == base.tag)
				{
					list2.AddRange(scene_arena.enemyList);
				}
				else if ("TriggerEnemy" == base.tag)
				{
					list2.AddRange(scene_arena.playerList);
				}
			}
			if ((skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.ALL_EXCLUDE_ME || skill_trigger.area_of_effect.filter_faction == AreaOfEffect.FilterFaction.FRIEND_EXCLUDE_ME) && null != skill_caster && list != null && list.Contains(skill_caster))
			{
				list.Remove(skill_caster);
			}
			Vector3 vector = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			if (areaConfigs.area_origin == AreaOfEffect.AreaConfig.AreaOrigin.DEFAULT_TARGET)
			{
				if (null != default_target)
				{
					float num = 0f;
					if (areaConfigs.include_puppet_radius)
					{
						num = default_target.ControllerRadius;
					}
					quaternion = default_target.transform.rotation;
					vector = new Vector3(default_target.transform.position.x, 0f, default_target.transform.position.z) + quaternion * Vector3.forward * num + quaternion * new Vector3(areaConfigs.area_offset.x, 0f, areaConfigs.area_offset.y);
				}
				else
				{
					quaternion = default_target_rotation;
					vector = new Vector3(default_target_position.x, 0f, default_target_position.z) + quaternion * new Vector3(areaConfigs.area_offset.x, 0f, areaConfigs.area_offset.y);
				}
			}
			else if (areaConfigs.area_origin == AreaOfEffect.AreaConfig.AreaOrigin.TRIGGER_POINT)
			{
				quaternion = target_rotation;
				vector = new Vector3(target_position.x, 0f, target_position.z) + target_rotation * new Vector3(areaConfigs.area_offset.x, 0f, areaConfigs.area_offset.y);
			}
			else if (areaConfigs.area_origin == AreaOfEffect.AreaConfig.AreaOrigin.CASTER)
			{
				if (null != skill_caster)
				{
					float num2 = 0f;
					if (areaConfigs.include_puppet_radius)
					{
						num2 = skill_caster.ControllerRadius;
					}
					quaternion = skill_caster.transform.rotation;
					vector = new Vector3(skill_caster.transform.position.x, 0f, skill_caster.transform.position.z) + quaternion * Vector3.forward * num2 + quaternion * new Vector3(areaConfigs.area_offset.x, 0f, areaConfigs.area_offset.y);
				}
				else
				{
					quaternion = caster_rotation;
					vector = new Vector3(caster_position.x, 0f, caster_position.z) + quaternion * new Vector3(areaConfigs.area_offset.x, 0f, areaConfigs.area_offset.y);
				}
			}
			if (list != null)
			{
				foreach (PuppetArena item in list)
				{
					if (item.IsDead())
					{
						continue;
					}
					Vector3 vector2 = new Vector3(item.transform.position.x, 0f, item.transform.position.z);
					if (areaConfigs.area_shape == AreaOfEffect.AreaConfig.AreaShape.CIRCLE)
					{
						if (D3DPlaneGeometry.CircleIntersectCircle(new Vector2(vector2.x, vector2.z), item.ControllerRadius, new Vector2(vector.x, vector.z), areaConfigs.area_size.x))
						{
							friend_targets.Add(item);
						}
					}
					else if (areaConfigs.area_shape == AreaOfEffect.AreaConfig.AreaShape.RECT)
					{
						if (D3DPlaneGeometry.CircleIntersectQuads(new Vector2(vector.x, vector.z), quaternion.eulerAngles.y * ((float)Math.PI / 180f), areaConfigs.area_size, new Vector2(vector2.x, vector2.z), item.ControllerRadius))
						{
							friend_targets.Add(item);
						}
					}
					else if (areaConfigs.area_shape == AreaOfEffect.AreaConfig.AreaShape.ARENA)
					{
						friend_targets.Add(item);
					}
				}
			}
			if (list2 != null)
			{
				foreach (PuppetArena item2 in list2)
				{
					if (item2.IsDead())
					{
						continue;
					}
					Vector3 vector3 = new Vector3(item2.transform.position.x, 0f, item2.transform.position.z);
					if (areaConfigs.area_shape == AreaOfEffect.AreaConfig.AreaShape.CIRCLE)
					{
						if (D3DPlaneGeometry.CircleIntersectCircle(new Vector2(vector3.x, vector3.z), item2.ControllerRadius, new Vector2(vector.x, vector.z), areaConfigs.area_size.x))
						{
							enemy_targets.Add(item2);
						}
					}
					else if (areaConfigs.area_shape == AreaOfEffect.AreaConfig.AreaShape.RECT)
					{
						if (D3DPlaneGeometry.CircleIntersectQuads(new Vector2(vector.x, vector.z), quaternion.eulerAngles.y * ((float)Math.PI / 180f), areaConfigs.area_size, new Vector2(vector3.x, vector3.z), item2.ControllerRadius))
						{
							enemy_targets.Add(item2);
						}
					}
					else if (areaConfigs.area_shape == AreaOfEffect.AreaConfig.AreaShape.ARENA)
					{
						enemy_targets.Add(item2);
					}
				}
			}
			if (skill_trigger.area_of_effect.include_default_target && null != skill_caster && !friend_targets.Contains(skill_caster))
			{
				friend_targets.Add(skill_caster);
			}
		}
		else
		{
			if (!(null != default_target))
			{
				return;
			}
			if ("Player" == default_target.tag)
			{
				if ("TriggerPlayer" == base.tag)
				{
					friend_targets.Add(default_target);
				}
				else
				{
					enemy_targets.Add(default_target);
				}
			}
			else if ("Enemy" == default_target.tag)
			{
				if ("TriggerPlayer" == base.tag)
				{
					enemy_targets.Add(default_target);
				}
				else
				{
					friend_targets.Add(default_target);
				}
			}
		}
	}

	private void ExecuteVariableConfig()
	{
		if (skill_trigger.trigger_variable == null)
		{
			return;
		}
		List<float[]> random_phy_values = new List<float[]>();
		List<float[]> random_mag_values = new List<float[]>();
		bool skill_hatred = false;
		int num;
		if (skill_trigger.trigger_special == null)
		{
			num = caster_variabledata.hatred_send;
		}
		else
		{
			num = skill_trigger.trigger_special.GetTaunt(skill_level);
			if (num < 0)
			{
				num = caster_variabledata.hatred_send;
			}
			else
			{
				skill_hatred = true;
			}
		}
		foreach (TriggerVariable item in skill_trigger.trigger_variable)
		{
			float phy_value = 0f;
			float mag_value = 0f;
			float target_hpmax_percent = 0f;
			float target_mpmax_percent = 0f;
			random_phy_values.Clear();
			random_mag_values.Clear();
			item.StatVariable(caster_variabledata, skill_level, ref phy_value, ref mag_value, ref target_hpmax_percent, ref target_mpmax_percent, ref random_phy_values, ref random_mag_values);
			if (item.dot_config != null)
			{
				float num2 = phy_value;
				foreach (float[] item2 in random_phy_values)
				{
					num2 += UnityEngine.Random.Range(item2[0], item2[1]);
				}
				float num3 = mag_value;
				foreach (float[] item3 in random_mag_values)
				{
					num3 += UnityEngine.Random.Range(item3[0], item3[1]);
				}
				float num4 = target_hpmax_percent;
				float num5 = target_mpmax_percent;
				if (item.variable_type == TriggerVariable.VariableType.HP_DAMAGE)
				{
					num2 *= caster_variabledata.dmg_extra;
					num3 *= caster_variabledata.dmg_extra;
					num4 *= caster_variabledata.dmg_extra;
					num5 *= caster_variabledata.dmg_extra;
					num3 += caster_variabledata.fixed_dmg_extra;
				}
				VariableOutputData dot_output_data = new VariableOutputData(item.variable_type, num2, num3, num4, num5, caster_variabledata.critical_chance, caster_level);
				if (item.output_config != null)
				{
					VariableOutputData dot_extra_output_data = null;
					if (item.dot_config.extra_variable != null)
					{
						phy_value = 0f;
						mag_value = 0f;
						target_hpmax_percent = 0f;
						target_mpmax_percent = 0f;
						random_phy_values.Clear();
						random_mag_values.Clear();
						item.dot_config.extra_variable.StatVariable(caster_variabledata, skill_level, ref phy_value, ref mag_value, ref target_hpmax_percent, ref target_mpmax_percent, ref random_phy_values, ref random_mag_values);
						num2 = phy_value;
						foreach (float[] item4 in random_phy_values)
						{
							num2 += UnityEngine.Random.Range(item4[0], item4[1]);
						}
						num3 = mag_value;
						foreach (float[] item5 in random_mag_values)
						{
							num3 += UnityEngine.Random.Range(item5[0], item5[1]);
						}
						num4 = target_hpmax_percent;
						num5 = target_mpmax_percent;
						if (item.variable_type == TriggerVariable.VariableType.HP_DAMAGE)
						{
							num2 *= caster_variabledata.dmg_extra;
							num3 *= caster_variabledata.dmg_extra;
							num4 *= caster_variabledata.dmg_extra;
							num5 *= caster_variabledata.dmg_extra;
							num3 += caster_variabledata.fixed_dmg_extra;
						}
						dot_extra_output_data = new VariableOutputData(item.variable_type, num2, num3, num4, num5, caster_variabledata.critical_chance, caster_level);
					}
					if (item.output_config.target_faction == TargetFaction.CASTER)
					{
						if (!(null != skill_caster) || skill_caster.IsDead())
						{
							continue;
						}
						if (!item.output_config.can_dodge || (item.output_config.can_dodge && !skill_caster.profile_instance.puppet_property.Dodge))
						{
							Vector3 offset = Vector3.zero;
							if (item.output_config.outer_play)
							{
								offset = Vector3.forward * skill_caster.ControllerRadius;
							}
							BasicEffectComponent.PlayEffect(item.output_config.effect, skill_caster.gameObject, item.output_config.mount_point, true, Vector2.one, offset, true, 0f);
							D3DAudioManager.Instance.PlayAudio(item.output_config.sfx, skill_caster.gameObject, false, true);
							GameObject gameObject = new GameObject("DotVariable");
							DotVariable dotVariable = gameObject.AddComponent<DotVariable>();
							dotVariable.InitDotVariable(skill_caster, item.dot_config, item.variable_type, dot_output_data, dot_extra_output_data, skill_level);
							skill_caster.ReceiveHatredPuppet(skill_caster, num, skill_hatred);
						}
						else
						{
							skill_caster.PuppetPopFont("DODGE", Color.white, BoardType.HitNumberRaise, 0.8f);
						}
						continue;
					}
					if (item.output_config.target_faction == TargetFaction.TARGET_ENEMY || item.output_config.target_faction == TargetFaction.TARGET_ALL)
					{
						foreach (PuppetArena enemy_target in enemy_targets)
						{
							if (!(null != enemy_target) || enemy_target.IsDead())
							{
								continue;
							}
							if (!item.output_config.can_dodge || (item.output_config.can_dodge && !enemy_target.profile_instance.puppet_property.Dodge))
							{
								Vector3 offset2 = Vector3.zero;
								if (item.output_config.outer_play)
								{
									offset2 = Vector3.forward * enemy_target.ControllerRadius;
								}
								BasicEffectComponent.PlayEffect(item.output_config.effect, enemy_target.gameObject, item.output_config.mount_point, true, Vector2.one, offset2, true, 0f);
								D3DAudioManager.Instance.PlayAudio(item.output_config.sfx, enemy_target.gameObject, false, true);
								GameObject gameObject2 = new GameObject("DotVariable");
								DotVariable dotVariable2 = gameObject2.AddComponent<DotVariable>();
								dotVariable2.InitDotVariable(enemy_target, item.dot_config, item.variable_type, dot_output_data, dot_extra_output_data, skill_level);
								enemy_target.ReceiveHatredPuppet(skill_caster, num, skill_hatred);
								special_hatred_used = true;
							}
							else
							{
								enemy_target.PuppetPopFont("DODGE", Color.white, BoardType.HitNumberRaise, 0.8f);
							}
						}
					}
					if (item.output_config.target_faction != 0 && item.output_config.target_faction != TargetFaction.TARGET_ALL)
					{
						continue;
					}
					foreach (PuppetArena friend_target in friend_targets)
					{
						if (!(null != friend_target) || friend_target.IsDead())
						{
							continue;
						}
						if (!item.output_config.can_dodge || (item.output_config.can_dodge && !friend_target.profile_instance.puppet_property.Dodge))
						{
							Vector3 offset3 = Vector3.zero;
							if (item.output_config.outer_play)
							{
								offset3 = Vector3.forward * friend_target.ControllerRadius;
							}
							BasicEffectComponent.PlayEffect(item.output_config.effect, friend_target.gameObject, item.output_config.mount_point, true, Vector2.one, offset3, true, 0f);
							D3DAudioManager.Instance.PlayAudio(item.output_config.sfx, friend_target.gameObject, false, true);
							GameObject gameObject3 = new GameObject("DotVariable");
							DotVariable dotVariable3 = gameObject3.AddComponent<DotVariable>();
							dotVariable3.InitDotVariable(friend_target, item.dot_config, item.variable_type, dot_output_data, dot_extra_output_data, skill_level);
							friend_target.ReceiveHatredPuppet(skill_caster, num, skill_hatred);
						}
						else
						{
							friend_target.PuppetPopFont("DODGE", Color.white, BoardType.HitNumberRaise, 0.8f);
						}
					}
				}
				else if (item.aureole_config != null)
				{
					AureoleBehaviour aureoleBehaviour = scene_arena.CheckAureoleExisting(skill_caster, item.aureole_config);
					if (null != aureoleBehaviour)
					{
						UnityEngine.Object.Destroy(aureoleBehaviour.gameObject);
					}
					GameObject gameObject4 = new GameObject("AureoleVariable");
					aureoleBehaviour = gameObject4.AddComponent<AureoleBehaviour>();
					aureoleBehaviour.InitAureole(skill_caster, default_target, target_position, scene_arena, item.aureole_config, skill_level, item.dot_config, item.variable_type, dot_output_data, null);
					scene_arena.AureoleManager.Add(aureoleBehaviour);
				}
			}
			else
			{
				if (item.output_config == null)
				{
					continue;
				}
				float num6 = 0f;
				if (item.output_config.target_faction == TargetFaction.CASTER)
				{
					if (null != skill_caster && !skill_caster.IsDead())
					{
						if (!item.output_config.can_dodge || (item.output_config.can_dodge && !skill_caster.profile_instance.puppet_property.Dodge))
						{
							Vector3 offset4 = Vector3.zero;
							if (item.output_config.outer_play)
							{
								offset4 = Vector3.forward * skill_caster.ControllerRadius;
							}
							BasicEffectComponent.PlayEffect(item.output_config.effect, skill_caster.gameObject, item.output_config.mount_point, true, Vector2.one, offset4, true, 0f);
							D3DAudioManager.Instance.PlayAudio(item.output_config.sfx, skill_caster.gameObject, false, true);
							float num7 = phy_value;
							foreach (float[] item6 in random_phy_values)
							{
								num7 += UnityEngine.Random.Range(item6[0], item6[1]);
							}
							float num8 = mag_value;
							foreach (float[] item7 in random_mag_values)
							{
								num8 += UnityEngine.Random.Range(item7[0], item7[1]);
							}
							float num9 = target_hpmax_percent;
							float num10 = target_mpmax_percent;
							if (item.variable_type == TriggerVariable.VariableType.HP_DAMAGE)
							{
								num7 *= caster_variabledata.dmg_extra;
								num8 *= caster_variabledata.dmg_extra;
								num9 *= caster_variabledata.dmg_extra;
								num10 *= caster_variabledata.dmg_extra;
								num8 += caster_variabledata.fixed_dmg_extra;
							}
							VariableOutputData variable_output_data = new VariableOutputData(item.variable_type, num7, num8, num9, num10, caster_variabledata.critical_chance, caster_level);
							if (shake_target)
							{
								skill_caster.KnockBack(target_rotation);
							}
							num6 += skill_caster.Variable(variable_output_data);
							skill_caster.ReceiveHatredPuppet(skill_caster, num, skill_hatred);
						}
						else
						{
							skill_caster.PuppetPopFont("DODGE", Color.white, BoardType.HitNumberRaise, 0.8f);
						}
					}
				}
				else
				{
					if (item.output_config.target_faction == TargetFaction.TARGET_ENEMY || item.output_config.target_faction == TargetFaction.TARGET_ALL)
					{
						foreach (PuppetArena enemy_target2 in enemy_targets)
						{
							if (!(null != enemy_target2) || enemy_target2.IsDead())
							{
								continue;
							}
							if (!item.output_config.can_dodge || (item.output_config.can_dodge && !enemy_target2.profile_instance.puppet_property.Dodge))
							{
								Vector3 offset5 = Vector3.zero;
								if (item.output_config.outer_play)
								{
									offset5 = Vector3.forward * enemy_target2.ControllerRadius;
								}
								BasicEffectComponent.PlayEffect(item.output_config.effect, enemy_target2.gameObject, item.output_config.mount_point, true, Vector2.one, offset5, true, 0f);
								D3DAudioManager.Instance.PlayAudio(item.output_config.sfx, enemy_target2.gameObject, false, true);
								float num11 = phy_value;
								foreach (float[] item8 in random_phy_values)
								{
									num11 += UnityEngine.Random.Range(item8[0], item8[1]);
								}
								float num12 = mag_value;
								foreach (float[] item9 in random_mag_values)
								{
									num12 += UnityEngine.Random.Range(item9[0], item9[1]);
								}
								float num13 = target_hpmax_percent;
								float num14 = target_mpmax_percent;
								if (item.variable_type == TriggerVariable.VariableType.HP_DAMAGE)
								{
									num11 *= caster_variabledata.dmg_extra;
									num12 *= caster_variabledata.dmg_extra;
									num13 *= caster_variabledata.dmg_extra;
									num14 *= caster_variabledata.dmg_extra;
									num12 += caster_variabledata.fixed_dmg_extra;
								}
								VariableOutputData variable_output_data2 = new VariableOutputData(item.variable_type, num11, num12, num13, num14, caster_variabledata.critical_chance, caster_level);
								if (shake_target)
								{
									enemy_target2.KnockBack(target_rotation);
								}
								num6 += enemy_target2.Variable(variable_output_data2);
								enemy_target2.ReceiveHatredPuppet(skill_caster, num, skill_hatred);
								special_hatred_used = true;
							}
							else
							{
								enemy_target2.PuppetPopFont("DODGE", Color.white, BoardType.HitNumberRaise, 0.8f);
							}
						}
					}
					if (item.output_config.target_faction == TargetFaction.TARGET_FRIEND || item.output_config.target_faction == TargetFaction.TARGET_ALL)
					{
						foreach (PuppetArena friend_target2 in friend_targets)
						{
							if (!(null != friend_target2) || friend_target2.IsDead())
							{
								continue;
							}
							if (!item.output_config.can_dodge || (item.output_config.can_dodge && !friend_target2.profile_instance.puppet_property.Dodge))
							{
								Vector3 offset6 = Vector3.zero;
								if (item.output_config.outer_play)
								{
									offset6 = Vector3.forward * friend_target2.ControllerRadius;
								}
								BasicEffectComponent.PlayEffect(item.output_config.effect, friend_target2.gameObject, item.output_config.mount_point, true, Vector2.one, offset6, true, 0f);
								D3DAudioManager.Instance.PlayAudio(item.output_config.sfx, friend_target2.gameObject, false, true);
								float num15 = phy_value;
								foreach (float[] item10 in random_phy_values)
								{
									num15 += UnityEngine.Random.Range(item10[0], item10[1]);
								}
								float num16 = mag_value;
								foreach (float[] item11 in random_mag_values)
								{
									num16 += UnityEngine.Random.Range(item11[0], item11[1]);
								}
								float num17 = target_hpmax_percent;
								float num18 = target_mpmax_percent;
								if (item.variable_type == TriggerVariable.VariableType.HP_DAMAGE)
								{
									num15 *= caster_variabledata.dmg_extra;
									num16 *= caster_variabledata.dmg_extra;
									num17 *= caster_variabledata.dmg_extra;
									num18 *= caster_variabledata.dmg_extra;
									num16 += caster_variabledata.fixed_dmg_extra;
								}
								VariableOutputData variable_output_data3 = new VariableOutputData(item.variable_type, num15, num16, num17, num18, caster_variabledata.critical_chance, caster_level);
								if (shake_target)
								{
									friend_target2.KnockBack(target_rotation);
								}
								num6 += friend_target2.Variable(variable_output_data3);
								friend_target2.ReceiveHatredPuppet(skill_caster, num, skill_hatred);
							}
							else
							{
								friend_target2.PuppetPopFont("DODGE", Color.white, BoardType.HitNumberRaise, 0.8f);
							}
						}
					}
				}
				if (item.output_config.imbibe_config == null || !(null != skill_caster) || skill_caster.IsDead())
				{
					continue;
				}
				float num19 = item.output_config.imbibe_config.ImbibePercent(skill_level);
				if (item.output_config.imbibe_config.imbibe_type == VariableOutputConfig.ImbibeConfig.ImbibeType.BUFF)
				{
					if (skill_caster.ImbibeBuff == null)
					{
						continue;
					}
					if (num19 < 0f)
					{
						num19 = skill_caster.ImbibeBuff.value;
					}
					if (num19 > 0f)
					{
						if (item.output_config.imbibe_config.imbibe_to_hp)
						{
							skill_caster.Variable(TriggerVariable.VariableType.HP_RECOVER, num6 * num19);
						}
						if (item.output_config.imbibe_config.imbibe_to_mp)
						{
							skill_caster.Variable(TriggerVariable.VariableType.MP_RECOVER, num6 * num19);
						}
					}
				}
				else if (item.output_config.imbibe_config.imbibe_type == VariableOutputConfig.ImbibeConfig.ImbibeType.PASSIVE && num19 > 0f)
				{
					if (item.output_config.imbibe_config.imbibe_to_hp)
					{
						skill_caster.Variable(TriggerVariable.VariableType.HP_RECOVER, num6 * num19);
					}
					if (item.output_config.imbibe_config.imbibe_to_mp)
					{
						skill_caster.Variable(TriggerVariable.VariableType.MP_RECOVER, num6 * num19);
					}
				}
			}
		}
	}

	private void ExecuteSpecial()
	{
		if (skill_trigger.trigger_special == null)
		{
			return;
		}
		if (skill_trigger.trigger_special != null && !special_hatred_used)
		{
			int taunt = skill_trigger.trigger_special.GetTaunt(skill_level);
			foreach (PuppetArena enemy_target in enemy_targets)
			{
				enemy_target.ReceiveHatredPuppet(skill_caster, taunt, true);
			}
		}
		if (skill_trigger.trigger_special.revive != null)
		{
			scene_arena.ReviveGravePuppet(base.tag, skill_trigger.trigger_special.revive.RecoverHP(skill_level), skill_trigger.trigger_special.revive.RecoverMP(skill_level));
		}
		if (skill_trigger.trigger_special.dispel != null && friend_targets.Count > 0 && skill_trigger.trigger_special.dispel.dispel_count != null)
		{
			foreach (PuppetArena friend_target in friend_targets)
			{
				friend_target.Dispel(skill_trigger.trigger_special.dispel.DispelCount(skill_level));
			}
		}
		if (skill_trigger.trigger_special.summon != null && null != skill_caster && !skill_caster.IsDead())
		{
			List<PuppetArena> summomed_puppets = new List<PuppetArena>();
			scene_arena.SummonPuppet(skill_caster, skill_trigger.trigger_special.summon.SummonID(skill_level), skill_trigger.trigger_special.summon.SummonLevel(skill_level), skill_trigger.trigger_special.summon.SummonCount(skill_level), skill_trigger.trigger_special.summon.SummonLife(skill_level), skill_trigger.trigger_special.summon.SummonEffect(skill_level), ref summomed_puppets);
			if ("Player" == skill_caster.tag)
			{
				scene_arena.OnPlayerSummonedCountChange(summomed_puppets);
			}
			else
			{
				scene_arena.OnEnemySummonedCountChange(summomed_puppets);
			}
		}
	}

	private void ExecuteCrowdControl()
	{
		if (skill_trigger.trigger_crowd_control == null)
		{
			return;
		}
		foreach (TriggerCrowdControl item in skill_trigger.trigger_crowd_control)
		{
			float odds = item.GetOdds(skill_level);
			if (item.target_faction == TargetFaction.CASTER)
			{
				if (null != skill_caster && !skill_caster.IsDead() && D3DMain.Instance.Lottery(odds))
				{
					GameObject gameObject = new GameObject("CrowdControl");
					CrowdControl crowdControl = gameObject.AddComponent<CrowdControl>();
					if (skill_caster.CrowdControlList.ContainsKey(item.control_type))
					{
						skill_caster.CrowdControlList[item.control_type].ExtendControl(item, skill_level);
					}
					else
					{
						crowdControl.InitControl(skill_caster, item, skill_level);
					}
				}
				continue;
			}
			if (item.target_faction == TargetFaction.TARGET_ENEMY || item.target_faction == TargetFaction.TARGET_ALL)
			{
				foreach (PuppetArena enemy_target in enemy_targets)
				{
					if (null != enemy_target && !enemy_target.IsDead() && D3DMain.Instance.Lottery(odds))
					{
						GameObject gameObject2 = new GameObject("CrowdControl");
						CrowdControl crowdControl2 = gameObject2.AddComponent<CrowdControl>();
						if (enemy_target.CrowdControlList.ContainsKey(item.control_type))
						{
							enemy_target.CrowdControlList[item.control_type].ExtendControl(item, skill_level);
						}
						else
						{
							crowdControl2.InitControl(enemy_target, item, skill_level);
						}
					}
				}
			}
			if (item.target_faction != 0 && item.target_faction != TargetFaction.TARGET_ALL)
			{
				continue;
			}
			foreach (PuppetArena friend_target in friend_targets)
			{
				if (null != friend_target && !friend_target.IsDead() && D3DMain.Instance.Lottery(odds))
				{
					GameObject gameObject3 = new GameObject("CrowdControl");
					CrowdControl crowdControl3 = gameObject3.AddComponent<CrowdControl>();
					if (friend_target.CrowdControlList.ContainsKey(item.control_type))
					{
						friend_target.CrowdControlList[item.control_type].ExtendControl(item, skill_level);
					}
					else
					{
						crowdControl3.InitControl(friend_target, item, skill_level);
					}
				}
			}
		}
	}

	private void ExecuteBuff()
	{
		if (skill_trigger.trigger_buff == null)
		{
			return;
		}
		foreach (TriggerBuff item in skill_trigger.trigger_buff)
		{
			if (item.aureole_config != null)
			{
				AureoleBehaviour aureoleBehaviour = scene_arena.CheckAureoleExisting(skill_caster, item.aureole_config);
				if (null != aureoleBehaviour)
				{
					UnityEngine.Object.Destroy(aureoleBehaviour.gameObject);
				}
				GameObject gameObject = new GameObject("AureoleBuff");
				aureoleBehaviour = gameObject.AddComponent<AureoleBehaviour>();
				aureoleBehaviour.InitAureole(skill_caster, default_target, target_position, scene_arena, item.aureole_config, skill_level, null, TriggerVariable.VariableType.HP_DAMAGE, null, item);
				scene_arena.AureoleManager.Add(aureoleBehaviour);
				continue;
			}
			float odds = item.GetOdds(skill_level);
			if (item.target_faction == TargetFaction.CASTER)
			{
				if (null != skill_caster && !skill_caster.IsDead() && D3DMain.Instance.Lottery(odds))
				{
					GameObject gameObject2 = new GameObject("Buff");
					Buff buff = gameObject2.AddComponent<Buff>();
					buff.InitBuff(skill_caster, item, skill_level);
				}
				continue;
			}
			if (item.target_faction == TargetFaction.TARGET_ENEMY || item.target_faction == TargetFaction.TARGET_ALL)
			{
				foreach (PuppetArena enemy_target in enemy_targets)
				{
					if (null != enemy_target && !enemy_target.IsDead() && D3DMain.Instance.Lottery(odds))
					{
						GameObject gameObject3 = new GameObject("Buff");
						Buff buff2 = gameObject3.AddComponent<Buff>();
						buff2.InitBuff(enemy_target, item, skill_level);
					}
				}
			}
			if (item.target_faction != 0 && item.target_faction != TargetFaction.TARGET_ALL)
			{
				continue;
			}
			foreach (PuppetArena friend_target in friend_targets)
			{
				if (null != friend_target && !friend_target.IsDead() && D3DMain.Instance.Lottery(odds))
				{
					GameObject gameObject4 = new GameObject("Buff");
					Buff buff3 = gameObject4.AddComponent<Buff>();
					buff3.InitBuff(friend_target, item, skill_level);
				}
			}
		}
	}
}
