using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillTriggerBehaviour : MonoBehaviour
{
	private enum TimerState
	{
		SLEEP = 0,
		AWAKE = 1,
		TIME_OUT = 2
	}

	private ActiveSkillTrigger skill_trigger;

	private int skill_level;

	private PuppetArena skill_caster;

	private int caster_level;

	private int caster_clip_index;

	private PuppetArena default_target;

	private Vector3 caster_position;

	private Quaternion caster_rotation;

	private Vector3 last_default_target_position;

	private Quaternion last_default_target_rotation;

	private Vector3 target_position;

	private Quaternion target_rotation;

	private int execute_index;

	private int execute_count = 1;

	private float execute_interval;

	private SceneArena scene_arena;

	private D3DPuppetVariableData caster_variabledata;

	private float timer_lifecycle;

	private TimerState timer_state;

	private bool trigger_paused;

	private List<BasicEffectComponent> lifecycle_effects = new List<BasicEffectComponent>();

	private void Start()
	{
	}

	private void Update()
	{
		if (!skill_trigger.independence && (null == skill_caster || skill_caster.IsBadState()))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (null != skill_caster)
		{
			if (skill_caster.IsInGrave())
			{
				skill_caster = null;
			}
			else
			{
				caster_position = skill_caster.transform.position;
				caster_rotation = skill_caster.transform.rotation;
			}
		}
		if (null != default_target)
		{
			last_default_target_position = default_target.transform.position;
			last_default_target_rotation = default_target.transform.rotation;
		}
		if (trigger_paused || timer_state != TimerState.AWAKE)
		{
			return;
		}
		timer_lifecycle -= Time.deltaTime;
		if (timer_lifecycle <= 0f)
		{
			timer_state = TimerState.TIME_OUT;
			if (execute_index >= execute_count)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		foreach (BasicEffectComponent lifecycle_effect in lifecycle_effects)
		{
			if (null != lifecycle_effect)
			{
				lifecycle_effect.Stop();
			}
		}
		if (!(null == skill_caster) && !skill_caster.IsDead() && skill_trigger.lock_frame)
		{
			skill_caster.model_builder.SetClipSpeed(caster_clip_index, 1f);
		}
	}

	public void InitSkillTriggerBehaviour(PuppetArena caster, ActiveSkillTrigger trigger, int skill_level, PuppetArena default_target)
	{
		skill_caster = caster;
		if ("Player" == skill_caster.tag)
		{
			base.tag = "TriggerPlayer";
		}
		else if ("Enemy" == skill_caster.tag)
		{
			base.tag = "TriggerEnemy";
		}
		caster_level = skill_caster.profile_instance.puppet_level;
		scene_arena = skill_caster.scene_arena;
		caster_variabledata = skill_caster.profile_instance.GetVariableData();
		skill_trigger = trigger;
		this.skill_level = skill_level;
		this.default_target = default_target;
		if (null != default_target)
		{
			last_default_target_position = default_target.transform.position;
			last_default_target_rotation = default_target.transform.rotation;
		}
		target_position = default_target.transform.position;
		target_rotation = caster.transform.rotation;
		execute_count = skill_trigger.TriggerCount(this.skill_level);
		execute_interval = skill_trigger.TriggerInterval(this.skill_level);
		if (skill_trigger.lock_frame)
		{
			caster_clip_index = skill_caster.model_builder.CurrentClip;
			skill_caster.model_builder.SetCurrentClipSpeed(0f);
		}
		StartCoroutine("ExecuteTrigger", skill_trigger.trigger_delay);
	}

	private IEnumerator ExecuteTrigger(float delay)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		if (execute_index == 0)
		{
			timer_lifecycle = skill_trigger.TriggerLifeCycle(skill_level);
			if (timer_lifecycle > 0f)
			{
				timer_state = TimerState.AWAKE;
			}
			else
			{
				timer_state = TimerState.TIME_OUT;
			}
			if (skill_trigger.lifecycle_bedeck_effects != null)
			{
				foreach (TriggerBedeckEffect bedeck_effect2 in skill_trigger.lifecycle_bedeck_effects)
				{
					PlayEffect(bedeck_effect2, false);
				}
			}
		}
		while (true)
		{
			if (skill_trigger.common_bedeck_effects != null)
			{
				foreach (TriggerBedeckEffect bedeck_effect in skill_trigger.common_bedeck_effects)
				{
					PlayEffect(bedeck_effect, true);
				}
			}
			GameObject executer_obj = new GameObject("Executer");
			executer_obj.tag = base.tag;
			executer_obj.transform.position = last_default_target_position;
			executer_obj.transform.rotation = last_default_target_rotation;
			ActiveSkillTriggerExecuter executer = executer_obj.AddComponent<ActiveSkillTriggerExecuter>();
			executer.Init(skill_caster, caster_position, caster_rotation, caster_level, skill_level, default_target, last_default_target_position, last_default_target_rotation, target_position, target_rotation, skill_trigger, scene_arena, caster_variabledata, skill_trigger.camera_shake_time, skill_trigger.puppet_shake);
			if (skill_trigger.trigger_missile != null)
			{
				Vector3 fire_point = skill_caster.transform.position;
				if (skill_trigger.trigger_missile.use_fire_point)
				{
					GameObject fire_point_obj = D3DMain.Instance.FindGameObjectChild(skill_caster.gameObject, "mount_attack01");
					if (null != fire_point_obj)
					{
						fire_point = fire_point_obj.transform.position;
					}
				}
				else
				{
					fire_point += skill_caster.transform.rotation * skill_trigger.trigger_missile.fire_point_offset;
				}
				Vector3 target_firepoint = last_default_target_position;
				if (skill_trigger.trigger_missile.shoot_hit_point && null != default_target)
				{
					GameObject target_firepoint_obj = D3DMain.Instance.FindGameObjectChild(default_target.gameObject, "mount_hurt");
					if (null != target_firepoint_obj)
					{
						target_firepoint = target_firepoint_obj.transform.position;
					}
				}
				PuppetArena shooter = null;
				ActiveSkillTriggerBehaviour trigger = null;
				if (skill_trigger.trigger_missile.shoot_caster)
				{
					shooter = skill_caster;
					trigger_paused = true;
					trigger = this;
					if (null != default_target)
					{
						Quaternion direction = Quaternion.LookRotation(target_firepoint - fire_point);
						target_firepoint -= direction * Vector3.forward * default_target.ControllerRadius + direction * Vector3.forward * skill_caster.ControllerRadius;
					}
				}
				GameObject missile_obj = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Missile/" + skill_trigger.trigger_missile.missile_name), fire_point, skill_caster.transform.rotation);
				missile_obj.GetComponent<MissileComponent>().InitMissile(fire_point, target_firepoint, shooter, trigger, executer);
			}
			else
			{
				executer.Execute();
			}
			execute_index++;
			if (trigger_paused)
			{
				StopCoroutine("ExecuteTrigger");
				yield break;
			}
			if (execute_index >= execute_count)
			{
				break;
			}
			yield return new WaitForSeconds(execute_interval);
		}
		if (timer_state == TimerState.TIME_OUT)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void PlayEffect(TriggerBedeckEffect bedeck_effect, bool one_shot)
	{
		Vector2 range = Vector2.one;
		if (skill_trigger.area_of_effect != null)
		{
			AreaOfEffect.AreaConfig areaConfigs = skill_trigger.area_of_effect.GetAreaConfigs(skill_level);
			range = areaConfigs.AreaSize;
		}
		BasicEffectComponent basicEffectComponent = null;
		if (bedeck_effect.bedeck_player == TriggerBedeckEffect.BedeckPlayer.CASTER)
		{
			Vector3 offset = bedeck_effect.effect_offset;
			if (null != skill_caster)
			{
				if (string.Empty == bedeck_effect.mount_point && bedeck_effect.include_puppet_radius)
				{
					offset = skill_caster.transform.rotation * Vector3.forward * skill_caster.ControllerRadius;
				}
				basicEffectComponent = BasicEffectComponent.PlayEffect(bedeck_effect.effect_name, skill_caster.gameObject, bedeck_effect.mount_point, bedeck_effect.bind, range, offset, one_shot, bedeck_effect.delay_time);
			}
			else
			{
				basicEffectComponent = BasicEffectComponent.PlayEffect(bedeck_effect.effect_name, caster_position, caster_rotation, range, bedeck_effect.effect_offset, one_shot, bedeck_effect.delay_time);
			}
		}
		else if (bedeck_effect.bedeck_player == TriggerBedeckEffect.BedeckPlayer.DEFAULT_TARGET)
		{
			Vector3 offset2 = bedeck_effect.effect_offset;
			if (null != default_target)
			{
				if (string.Empty == bedeck_effect.mount_point && bedeck_effect.include_puppet_radius)
				{
					offset2 = default_target.transform.rotation * Vector3.forward * default_target.ControllerRadius;
				}
				basicEffectComponent = BasicEffectComponent.PlayEffect(bedeck_effect.effect_name, default_target.gameObject, bedeck_effect.mount_point, bedeck_effect.bind, range, offset2, one_shot, bedeck_effect.delay_time);
			}
			else
			{
				basicEffectComponent = BasicEffectComponent.PlayEffect(bedeck_effect.effect_name, last_default_target_position, last_default_target_rotation, range, bedeck_effect.effect_offset, one_shot, bedeck_effect.delay_time);
			}
		}
		else if (bedeck_effect.bedeck_player == TriggerBedeckEffect.BedeckPlayer.TRIGGER_POINT)
		{
			basicEffectComponent = BasicEffectComponent.PlayEffect(bedeck_effect.effect_name, target_position, target_rotation, range, bedeck_effect.effect_offset, one_shot, bedeck_effect.delay_time);
		}
		if (bedeck_effect.sfx_list != null)
		{
			foreach (string item in bedeck_effect.sfx_list)
			{
				D3DAudioManager.Instance.PlayAudio(item, basicEffectComponent.gameObject, false, true);
			}
		}
		if (!one_shot && null != basicEffectComponent)
		{
			lifecycle_effects.Add(basicEffectComponent);
		}
	}

	public void ResumeTrigger()
	{
		if (execute_index >= execute_count && timer_state == TimerState.TIME_OUT)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		trigger_paused = false;
		StartCoroutine("ExecuteTrigger", execute_interval);
	}
}
