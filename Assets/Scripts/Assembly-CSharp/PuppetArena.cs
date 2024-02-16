using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetArena : PuppetBasic
{
	public enum ArenaPuppetState
	{
		Idle = 0,
		Move = 1,
		PrepareSkill = 2,
		CastSkill = 3,
		ExecutingSkill = 4,
		Dead = 5,
		StandStill = 6,
		Fear = 7,
		Grave = 8,
		WinnerIdle = 9
	}

	protected int current_hatred;

	protected readonly float target_correct_interval = 0.5f;

	protected PuppetArena target_puppet;

	protected PuppetArena hatred_puppet;

	protected PuppetArena friend_target;

	protected float prepare_skill_break;

	protected D3DFloat common_cd_delta;

	protected D3DFloat common_cd;

	protected D3DClassBattleSkillStatus common_skill;

	protected D3DClassBattleSkillStatus charged_skill;

	protected D3DClassBattleSkillStatus next_skill;

	protected D3DClassBattleSkillStatus excuting_skill;

	protected GameObject distance_line;

	protected PuppetComponents puppetComponents;

	protected ArenaPuppetState puppetState;

	public SceneArena scene_arena;

	public PuppetMonitorBasic puppet_monitor;

	public D3DFloat ImbibeBuff;

	public List<DotVariable> BadDotList;

	public Dictionary<TriggerCrowdControl.ControlType, CrowdControl> CrowdControlList;

	public List<Buff> DebuffList;

	protected bool Durance;

	protected int exp_bonus;

	protected float exp_add_delta;

	private static bool _bHealLogged;

	public PuppetArena TargetPuppet
	{
		get
		{
			return target_puppet;
		}
		set
		{
			target_puppet = value;
		}
	}

	public PuppetArena HatredPuppet
	{
		get
		{
			return hatred_puppet;
		}
	}

	public D3DClassBattleSkillStatus CommonSkill
	{
		get
		{
			return common_skill;
		}
	}

	public D3DClassBattleSkillStatus ChargedSkill
	{
		get
		{
			return charged_skill;
		}
	}

	public float ControllerRadius
	{
		get
		{
			return puppetController.radius * profile_instance.PuppetScale;
		}
	}

	protected void SetMoveTarget(Vector3 target_pt)
	{
		SetRotationTarget(target_pt);
		if (IsTargetInDistance())
		{
			OnPositionTarget();
			puppetState = ArenaPuppetState.PrepareSkill;
			prepare_skill_break = 0f;
			return;
		}
		puppetMovement.move_start = base.transform.position;
		puppetMovement.move_target = target_pt;
		puppetState = ArenaPuppetState.Move;
		if (!Durance)
		{
			model_builder.PlayPuppetAnimations(true, model_builder.ChangeableMoveClip, WrapMode.Loop);
		}
		else
		{
			model_builder.PlayPuppetAnimations(true, model_builder.ChangeableIdleClip, WrapMode.Loop);
		}
		if (null != TargetPuppet)
		{
			puppet_monitor.OnMove();
		}
	}

	public bool CheckMoveInWorldLimit()
	{
		RaycastHit hitInfo;
		if (D3DPlaneGeometry.PointInpolygon(scene_arena.WorldLimit, new Vector2(base.transform.position.x, base.transform.position.z)))
		{
			return Physics.Raycast(base.transform.position + Vector3.up, -Vector3.up, out hitInfo, float.PositiveInfinity, 256);
		}
		if (D3DPlaneGeometry.PointInpolygon(scene_arena.WorldLimit, new Vector2(puppetMovement.move_target.x, puppetMovement.move_target.z)))
		{
			return true;
		}
		return false;
	}

	private void MoveToTarget()
	{
		if (IsTargetInDistance())
		{
			ReverToIdle();
			puppetState = ArenaPuppetState.PrepareSkill;
			prepare_skill_break = 0f;
		}
		else if (!Durance)
		{
			puppetMovement.last_move_position = base.transform.position;
			puppetMovement.frame_move_delta = puppetMovement.move_velocity * Time.deltaTime;
			puppetController.Move(puppetMovement.rotate_target * Vector3.forward * Time.deltaTime * puppetMovement.move_velocity);
			base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			if (!CheckMoveInWorldLimit())
			{
				base.transform.position = puppetMovement.last_move_position;
			}
			if (Vector3.Distance(puppetMovement.move_target, base.transform.position) < 0.1f)
			{
				OnPositionTarget();
			}
			CorrectMove();
		}
	}

	public void SetPuppetMovement()
	{
		puppetMovement.move_velocity = profile_instance.puppet_property.move_speed;
		if (puppetMovement.move_velocity < 0f)
		{
			puppetMovement.move_velocity = 0f;
		}
		puppetMovement.rotate_speed = puppetMovement.move_velocity * D3DFormulas.RotateCoe;
		model_builder.SetClipSpeed(3, profile_instance.puppet_property.MoveSpdScale);
		model_builder.SetClipSpeed(4, profile_instance.puppet_property.MoveSpdScale);
	}

	protected bool IsTargetInDistance()
	{
		if (excuting_skill == null)
		{
			return false;
		}
		if (null != TargetPuppet)
		{
			if (TargetPuppet == this)
			{
				return true;
			}
			if (Vector2.Distance(new Vector2(TargetPuppet.transform.position.x, TargetPuppet.transform.position.z), new Vector2(base.transform.position.x, base.transform.position.z)) <= 0.1f)
			{
				return true;
			}
			Vector3 vector = puppetMovement.rotate_target * Vector3.forward;
			Vector3 vector2 = base.transform.position + vector * puppetController.radius * profile_instance.PuppetScale;
			float num = excuting_skill.Distance;
			if (num <= 0f)
			{
				num = 0.3f;
			}
			Vector3 vector3 = vector2 + vector * num;
			EnableDistanceLine(num, (vector2 + vector3) * 0.5f, puppetMovement.rotate_target);
			return D3DPlaneGeometry.SegmentIntersectCircle(new Vector2(vector2.x, vector2.z), new Vector2(vector3.x, vector3.z), new Vector2(TargetPuppet.transform.position.x, TargetPuppet.transform.position.z), TargetPuppet.ControllerRadius);
		}
		return false;
	}

	public void SetTarget(Vector3 target_pt)
	{
		if (!IsBadState() && ((puppetState != ArenaPuppetState.CastSkill && puppetState != ArenaPuppetState.ExecutingSkill) || excuting_skill.cancelable))
		{
			puppetMovement.last_move_position = base.transform.position;
			puppetMovement.frame_move_delta = 0f;
			puppetMovement.block_count = 0;
			if (null != TargetPuppet && TargetPuppet.tag == base.tag)
			{
				friend_target = TargetPuppet;
			}
			TargetPuppet = null;
			ClearHatredPuppet();
			SetMoveTarget(target_pt);
		}
	}

	public void SetTarget(PuppetArena target_obj)
	{
		if (null == target_obj || target_obj.IsDead() || IsBadState() || ((puppetState == ArenaPuppetState.CastSkill || puppetState == ArenaPuppetState.ExecutingSkill) && !excuting_skill.cancelable))
		{
			return;
		}
		TargetPuppet = target_obj;
		if (!CheckNextSkill())
		{
			TargetPuppet = null;
			if (CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
			else
			{
				ReverToIdle();
			}
			return;
		}
		excuting_skill = next_skill;
		if (TargetPuppet == this)
		{
			ReverToIdle();
			puppetState = ArenaPuppetState.PrepareSkill;
			prepare_skill_break = 0f;
		}
		else
		{
			puppetMovement.last_move_position = base.transform.position;
			puppetMovement.frame_move_delta = 0f;
			puppetMovement.block_count = 0;
			SetMoveTarget(target_obj.transform.position);
		}
	}

	protected bool CheckHatredTarget()
	{
		if (null == hatred_puppet || hatred_puppet.IsDead() || hatred_puppet.IsInGrave())
		{
			hatred_puppet = null;
			current_hatred = 0;
			return false;
		}
		if (!CheckVaildTarget(hatred_puppet))
		{
			hatred_puppet = null;
			current_hatred = 0;
			return false;
		}
		if (TargetPuppet == hatred_puppet)
		{
			return false;
		}
		return true;
	}

	public bool CheckVaildTarget(PuppetArena target_obj)
	{
		if (null == target_obj || target_obj.IsDead())
		{
			return false;
		}
		if (charged_skill != null)
		{
			switch (charged_skill.active_skill.active_type)
			{
			case D3DActiveSkill.ActiveType.TAP_ENEMY:
				if (base.tag != target_obj.tag)
				{
					return true;
				}
				break;
			case D3DActiveSkill.ActiveType.TAP_FRIEND:
				if (base.tag == target_obj.tag)
				{
					return true;
				}
				break;
			case D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME:
				if (base.tag == target_obj.tag && this != target_obj)
				{
					return true;
				}
				break;
			}
		}
		if (common_skill != null)
		{
			switch (common_skill.active_skill.active_type)
			{
			case D3DActiveSkill.ActiveType.TAP_ENEMY:
				if (base.tag != target_obj.tag)
				{
					return true;
				}
				break;
			case D3DActiveSkill.ActiveType.TAP_FRIEND:
				if (base.tag == target_obj.tag)
				{
					return true;
				}
				break;
			case D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME:
				if (base.tag == target_obj.tag && this != target_obj)
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	private void CorrectMove()
	{
		puppetMovement.correct_delta += Time.deltaTime;
		if (puppetMovement.correct_delta >= target_correct_interval)
		{
			puppetMovement.correct_delta = 0f;
			if (null != TargetPuppet)
			{
				puppetMovement.move_target = TargetPuppet.transform.position;
			}
			SetMoveTarget(puppetMovement.move_target);
		}
	}

	public void ReceiveHatredPuppet(PuppetArena puppet, int hatred, bool skill_hatred)
	{
		if (null == puppet || puppet.IsDead() || puppet.tag == base.tag || !CheckVaildTarget(puppet))
		{
			return;
		}
		if (null == hatred_puppet || hatred_puppet.IsDead() || hatred_puppet.IsInGrave())
		{
			hatred_puppet = null;
			current_hatred = 0;
		}
		if (hatred_puppet == puppet)
		{
			return;
		}
		bool flag = false;
		if (!skill_hatred)
		{
			if ("Player" == base.tag)
			{
				if (puppetState == ArenaPuppetState.Idle)
				{
					flag = true;
				}
			}
			else if (puppetState == ArenaPuppetState.Idle || puppetState == ArenaPuppetState.Move || puppetState == ArenaPuppetState.PrepareSkill)
			{
				flag = true;
			}
		}
		else if (puppetState == ArenaPuppetState.Idle || puppetState == ArenaPuppetState.Move || puppetState == ArenaPuppetState.PrepareSkill)
		{
			flag = true;
		}
		bool flag2 = false;
		if (hatred > profile_instance.puppet_class.apply_hatred_resist && hatred >= current_hatred)
		{
			if (hatred == current_hatred)
			{
				if (skill_hatred)
				{
					flag2 = true;
				}
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			hatred_puppet = puppet;
			current_hatred = hatred;
		}
		if (null == TargetPuppet)
		{
			if (flag)
			{
				SetTarget(puppet);
			}
		}
		else if (flag && flag2)
		{
			SetTarget(puppet);
		}
	}

	public void ClearHatredPuppet()
	{
		hatred_puppet = null;
		current_hatred = 0;
	}

	protected bool CheckSkillTarget(D3DClassBattleSkillStatus skill)
	{
		bool result = false;
		if (skill != null)
		{
			switch (skill.active_skill.active_type)
			{
			case D3DActiveSkill.ActiveType.TAP_ENEMY:
				if (base.tag != TargetPuppet.tag)
				{
					result = true;
				}
				break;
			case D3DActiveSkill.ActiveType.TAP_FRIEND:
				if (base.tag == TargetPuppet.tag)
				{
					result = true;
				}
				break;
			case D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME:
				if (base.tag == TargetPuppet.tag && this != TargetPuppet)
				{
					result = true;
				}
				break;
			}
		}
		return result;
	}

	protected bool CheckNextSkill()
	{
		if (null == TargetPuppet || TargetPuppet.IsDead() || TargetPuppet.IsInGrave())
		{
			next_skill = null;
			return false;
		}
		if (CheckSkillTarget(charged_skill))
		{
			next_skill = charged_skill;
			return true;
		}
		if (CheckSkillTarget(common_skill))
		{
			next_skill = common_skill;
			return true;
		}
		next_skill = null;
		return false;
	}

	public D3DClassBattleSkillStatus HaveAttackSkill()
	{
		if (charged_skill != null && charged_skill.active_skill.active_type == D3DActiveSkill.ActiveType.TAP_ENEMY)
		{
			return charged_skill;
		}
		if (common_skill != null && common_skill.active_skill.active_type == D3DActiveSkill.ActiveType.TAP_ENEMY)
		{
			return common_skill;
		}
		return null;
	}

	protected bool IsTargetInheritable(D3DClassBattleSkillStatus next_skill)
	{
		if (next_skill == null)
		{
			return false;
		}
		if (null == TargetPuppet || TargetPuppet.IsDead())
		{
			return false;
		}
		switch (next_skill.active_skill.active_type)
		{
		case D3DActiveSkill.ActiveType.TAP_ENEMY:
			if (base.tag != TargetPuppet.tag)
			{
				return true;
			}
			break;
		case D3DActiveSkill.ActiveType.TAP_FRIEND:
			if (base.tag == TargetPuppet.tag)
			{
				return true;
			}
			break;
		case D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME:
			if (base.tag == TargetPuppet.tag && this != TargetPuppet)
			{
				return true;
			}
			break;
		}
		return false;
	}

	protected void CastSkill()
	{
		if (excuting_skill == null)
		{
			ReverToIdle();
			FreezeSkills(false);
			return;
		}
		if (charged_skill == excuting_skill)
		{
			charged_skill = null;
		}
		if (!excuting_skill.cancelable)
		{
			FreezeSkills(true);
		}
		if ((excuting_skill.IsActivateSkill() || excuting_skill == profile_instance.basic_skill) && excuting_skill != profile_instance.basic_skill)
		{
			profile_instance.puppet_property.mp -= excuting_skill.MPConsume;
			if (profile_instance.puppet_property.mp < excuting_skill.MPConsume)
			{
				if (common_skill.battle_ui != null)
				{
					common_skill.battle_ui.press_mask.Set(false);
				}
				common_skill = profile_instance.basic_skill;
			}
		}
		if (excuting_skill.cast_time <= 0f)
		{
			ExecuteSkill();
			return;
		}
		excuting_skill.cast_delta = excuting_skill.cast_time;
		puppetState = ArenaPuppetState.CastSkill;
		model_builder.SetClipSpeed(11, 1f);
		model_builder.PlayPuppetAnimations(true, 11, WrapMode.Loop, true);
	}

	protected void ExecuteSkill()
	{
		if (excuting_skill == null || scene_arena.IsBattleWinBehaviour)
		{
			OnExecuteSkillOver();
			return;
		}
		if (charged_skill == excuting_skill)
		{
			charged_skill = null;
		}
		bool flag = excuting_skill == profile_instance.basic_skill;
		if (excuting_skill.animation_clip_index == null && !flag)
		{
			ActiveSkillTrigger frameTrigger = excuting_skill.GetFrameTrigger();
			if (frameTrigger != null)
			{
				GameObject gameObject = new GameObject("skilltriggerbehaviour");
				ActiveSkillTriggerBehaviour activeSkillTriggerBehaviour = gameObject.AddComponent<ActiveSkillTriggerBehaviour>();
				PuppetArena default_target = TargetPuppet;
				if (excuting_skill.active_skill.active_type == D3DActiveSkill.ActiveType.PROMPT)
				{
					default_target = this;
				}
				activeSkillTriggerBehaviour.InitSkillTriggerBehaviour(this, frameTrigger, excuting_skill.skill_level, default_target);
			}
			OnExecuteSkillOver();
		}
		else
		{
			int num;
			if (flag)
			{
				num = UnityEngine.Random.Range(6, 8);
				profile_instance.basic_skill.clip_frames = ((num != 6) ? profile_instance.puppet_class.basic_attack2_frames : profile_instance.puppet_class.basic_attack1_frames);
			}
			else
			{
				num = excuting_skill.animation_clip_index[current_animation_type].value;
			}
			model_builder.SetClipSpeed(num, 1f);
			if (model_builder.PlayPuppetAnimations(true, num, WrapMode.Once, true))
			{
				puppetState = ArenaPuppetState.ExecutingSkill;
			}
			else
			{
				OnExecuteSkillOver();
			}
		}
	}

	protected void FreezeSkills(bool freeze)
	{
		if (profile_instance.battle_active_skills == null)
		{
			return;
		}
		foreach (string key in profile_instance.battle_active_skills.Keys)
		{
			profile_instance.battle_active_skills[key].freeze = freeze;
		}
	}

	protected void OnExecuteSkillOver()
	{
		excuting_skill = null;
		FreezeSkills(false);
		ReverToIdle();
		if (scene_arena.IsBattleWinBehaviour)
		{
			TargetPuppet = null;
			ClearHatredPuppet();
			return;
		}
		puppet_monitor.OnExcuteSkillOver();
		puppet_monitor.OnIdle();
		if (puppetState == ArenaPuppetState.Idle)
		{
			if (CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
			else
			{
				SetTarget(TargetPuppet);
			}
		}
	}

	public void InitCommonCD()
	{
		common_cd_delta = new D3DFloat(0f);
		common_cd = new D3DFloat(profile_instance.puppet_property.attack_interval);
		if (profile_instance.basic_skill != null)
		{
			profile_instance.basic_skill.SetSkillCommonCD(common_cd_delta, common_cd);
			profile_instance.basic_skill.puppet_property = profile_instance.puppet_property;
		}
		if (profile_instance.battle_active_skills == null)
		{
			return;
		}
		foreach (string key in profile_instance.battle_active_skills.Keys)
		{
			profile_instance.battle_active_skills[key].SetSkillCommonCD(common_cd_delta, common_cd);
			profile_instance.battle_active_skills[key].puppet_property = profile_instance.puppet_property;
		}
	}

	public void SetCommonCD()
	{
		common_cd.value = profile_instance.puppet_property.attack_interval;
	}

	public void OnSkillKeyFrame(int frame_index)
	{
		if (puppetState != ArenaPuppetState.ExecutingSkill || scene_arena.IsBattleWinBehaviour)
		{
			return;
		}
		ActiveSkillTrigger frameTrigger = excuting_skill.GetFrameTrigger(frame_index, current_animation_type);
		if (frameTrigger != null)
		{
			if (excuting_skill.IsActivateSkill() || excuting_skill == profile_instance.basic_skill)
			{
				excuting_skill.FillCD();
			}
			GameObject gameObject = new GameObject("SkillTriggerBehaviour");
			ActiveSkillTriggerBehaviour activeSkillTriggerBehaviour = gameObject.AddComponent<ActiveSkillTriggerBehaviour>();
			PuppetArena default_target = TargetPuppet;
			if (excuting_skill.active_skill.active_type == D3DActiveSkill.ActiveType.PROMPT)
			{
				default_target = this;
			}
			activeSkillTriggerBehaviour.InitSkillTriggerBehaviour(this, frameTrigger, excuting_skill.skill_level, default_target);
		}
	}

	public void TriggerSkill(D3DClassBattleSkillStatus skill_status, bool ignore_cd)
	{
		switch (skill_status.active_skill.active_type)
		{
		case D3DActiveSkill.ActiveType.PROMPT:
			excuting_skill = skill_status;
			if (skill_status.battle_ui != null)
			{
				skill_status.battle_ui.press_mask.Set(false);
			}
			if (!ignore_cd)
			{
				excuting_skill.FillCD();
			}
			CastSkill();
			profile_instance.puppet_property.mp -= skill_status.MPConsume;
			TriggerSkillEffect();
			break;
		case D3DActiveSkill.ActiveType.TAP_ENEMY:
		case D3DActiveSkill.ActiveType.TAP_FRIEND:
		case D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME:
			if (skill_status.active_skill.activation)
			{
				if (common_skill != null && common_skill.battle_ui != null)
				{
					common_skill.battle_ui.press_mask.Set(false);
				}
				if (skill_status == common_skill)
				{
					common_skill = profile_instance.basic_skill;
				}
				else
				{
					common_skill = skill_status;
					if (common_skill.battle_ui != null)
					{
						common_skill.battle_ui.press_mask.Set(true);
					}
				}
				if (puppetState == ArenaPuppetState.Idle || puppetState == ArenaPuppetState.Move || puppetState == ArenaPuppetState.PrepareSkill)
				{
					SetTarget(TargetPuppet);
				}
			}
			else
			{
				charged_skill = skill_status;
				if (!ignore_cd)
				{
					charged_skill.FillCD();
				}
				profile_instance.puppet_property.mp -= skill_status.MPConsume;
				if (skill_status.battle_ui != null)
				{
					skill_status.battle_ui.press_mask.Set(false);
				}
				SetTarget(TargetPuppet);
				TriggerSkillEffect();
			}
			break;
		}
	}

	private void TriggerSkillEffect()
	{
		if (!("Player" != base.tag))
		{
			BasicEffectComponent.PlayEffect("glimpse", base.gameObject, string.Empty, true, Vector2.zero, Vector3.zero, true, 0f);
		}
	}

	public void RejectSkill(D3DClassBattleSkillStatus skill_status)
	{
		if (profile_instance.battle_active_skills == null || !profile_instance.battle_active_skills.ContainsValue(skill_status))
		{
			return;
		}
		switch (skill_status.active_skill.active_type)
		{
		case D3DActiveSkill.ActiveType.TAP_ENEMY:
		case D3DActiveSkill.ActiveType.TAP_FRIEND:
		case D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME:
			if (skill_status.active_skill.activation)
			{
				bool flag = false;
				if (puppetState == ArenaPuppetState.Idle || puppetState == ArenaPuppetState.Move || puppetState == ArenaPuppetState.PrepareSkill)
				{
					flag = true;
				}
				if (excuting_skill == skill_status && flag)
				{
					excuting_skill = null;
				}
				common_skill = profile_instance.basic_skill;
				if (!flag || excuting_skill != null)
				{
					break;
				}
				if (IsTargetInheritable(common_skill))
				{
					excuting_skill = common_skill;
					if (IsTargetInDistance())
					{
						CastSkill();
						break;
					}
					ReverToIdle();
					SetRotationTarget(TargetPuppet.transform.position);
					puppetState = ArenaPuppetState.Move;
				}
				else
				{
					TargetPuppet = null;
					ReverToIdle();
					ClearHatredPuppet();
					excuting_skill = common_skill;
				}
			}
			else
			{
				if (charged_skill == skill_status)
				{
					charged_skill.FillCD();
				}
				skill_status.battle_ui.press_mask.Set(true);
			}
			break;
		}
	}

	public void DeadTriggerSkill(D3DClassBattleSkillStatus skill_status)
	{
		if (scene_arena.IsBattleWinBehaviour)
		{
			return;
		}
		ActiveSkillTrigger frameTrigger = skill_status.GetFrameTrigger();
		if (frameTrigger != null)
		{
			GameObject gameObject = new GameObject("skilltriggerbehaviour");
			ActiveSkillTriggerBehaviour activeSkillTriggerBehaviour = gameObject.AddComponent<ActiveSkillTriggerBehaviour>();
			PuppetArena default_target = TargetPuppet;
			if (skill_status.active_skill.active_type == D3DActiveSkill.ActiveType.PROMPT)
			{
				default_target = this;
			}
			activeSkillTriggerBehaviour.InitSkillTriggerBehaviour(this, frameTrigger, skill_status.skill_level, default_target);
		}
	}

	public void UpdateBattleSkillUI()
	{
		if (profile_instance.battle_active_skills != null)
		{
			scene_arena.ui_arena.UpdateBattleSkillUI(profile_instance.battle_active_skills);
			if (charged_skill != null)
			{
				charged_skill.battle_ui.press_mask.Set(true);
			}
			if (common_skill != null && common_skill.battle_ui != null)
			{
				common_skill.battle_ui.press_mask.Set(true);
			}
		}
	}

	public void RecoverHatred()
	{
		if (null == hatred_puppet)
		{
			current_hatred = 0;
		}
		else
		{
			current_hatred = hatred_puppet.profile_instance.puppet_class.apply_hatred_send;
		}
	}

	private void ActiveSkillCDTimer()
	{
		if (common_cd_delta != null)
		{
			common_cd_delta.value -= Time.deltaTime;
		}
		if (profile_instance.basic_skill != null)
		{
			profile_instance.basic_skill.cd_delta -= Time.deltaTime;
		}
		if (profile_instance.battle_active_skills == null)
		{
			return;
		}
		foreach (string key in profile_instance.battle_active_skills.Keys)
		{
			profile_instance.battle_active_skills[key].cd_delta -= Time.deltaTime;
		}
	}

	protected void PuppetBehaviour()
	{
		RotateToTarget();
		switch (puppetState)
		{
		case ArenaPuppetState.Idle:
			ProcessStateIdle();
			break;
		case ArenaPuppetState.Move:
			ProcessStateMove();
			break;
		case ArenaPuppetState.PrepareSkill:
			ProcessStatePrepareSkill();
			break;
		case ArenaPuppetState.CastSkill:
			ProcessStateCastSkill();
			break;
		case ArenaPuppetState.ExecutingSkill:
			if (!model_builder.IsClipPlaying(model_builder.CurrentClip))
			{
				OnExecuteSkillOver();
			}
			break;
		case ArenaPuppetState.Dead:
			ProcessStateDead();
			break;
		case ArenaPuppetState.Fear:
			FearMove();
			break;
		case ArenaPuppetState.WinnerIdle:
			if (!model_builder.IsClipPlaying(10) && !model_builder.IsClipPlaying(model_builder.ChangeableIdleClip))
			{
				model_builder.PlayPuppetAnimations(true, model_builder.ChangeableIdleClip, WrapMode.Loop);
			}
			break;
		case ArenaPuppetState.StandStill:
		case ArenaPuppetState.Grave:
			break;
		}
	}

	private void ProcessStateCastSkill()
	{
		if (excuting_skill.active_skill.active_type != D3DActiveSkill.ActiveType.PROMPT && (null == TargetPuppet || TargetPuppet.IsDead() || TargetPuppet.IsInGrave()))
		{
			ReverToIdle();
			FreezeSkills(false);
			TargetPuppet = null;
			excuting_skill = null;
			puppet_monitor.OnIdle();
			if (puppetState == ArenaPuppetState.Idle && CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
			return;
		}
		excuting_skill.cast_delta -= Time.deltaTime;
		if (excuting_skill.cast_delta <= 0f)
		{
			if (base.gameObject.tag == "Player")
			{
			}
			ExecuteSkill();
		}
	}

	private void ProcessStatePrepareSkill()
	{
		if (puppetMovement.doing_rotation)
		{
			return;
		}
		if (null == TargetPuppet || TargetPuppet.IsDead() || TargetPuppet.IsInGrave())
		{
			ReverToIdle();
			TargetPuppet = null;
			excuting_skill = null;
			puppet_monitor.OnIdle();
			if (puppetState == ArenaPuppetState.Idle && CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
		}
		else if (IsTargetInDistance())
		{
			if (excuting_skill.CDPercent <= 0f || charged_skill == excuting_skill)
			{
				DisableDistanceLine();
				CastSkill();
			}
		}
		else
		{
			prepare_skill_break += Time.deltaTime;
			if (prepare_skill_break > 0.15f)
			{
				DisableDistanceLine();
				puppetMovement.correct_delta = 0f;
				SetMoveTarget(TargetPuppet.transform.position);
			}
		}
	}

	private void ProcessStateIdle()
	{
		if (null != friend_target)
		{
			if (null == TargetPuppet)
			{
				TargetPuppet = friend_target;
				if (!CheckNextSkill())
				{
					TargetPuppet = null;
				}
			}
			friend_target = null;
		}
		if (!(null != TargetPuppet))
		{
			return;
		}
		if (TargetPuppet.IsDead() || TargetPuppet.IsInGrave())
		{
			TargetPuppet = null;
			puppet_monitor.OnIdle();
			if (puppetState == ArenaPuppetState.Idle && CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
		}
		else
		{
			if (excuting_skill == null)
			{
				excuting_skill = next_skill;
			}
			SetMoveTarget(TargetPuppet.transform.position);
		}
	}

	private void ProcessStateMove()
	{
		if (null != TargetPuppet && (TargetPuppet.IsDead() || TargetPuppet.IsInGrave()))
		{
			ReverToIdle();
			TargetPuppet = null;
			excuting_skill = null;
			puppet_monitor.OnIdle();
			if (puppetState == ArenaPuppetState.Idle && CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
		}
		else if (!CheckBlock())
		{
			MoveToTarget();
		}
	}

	protected void ProcessStateDead()
	{
		if (!model_builder.IsPlayingOutOfLength(9, 2f))
		{
			return;
		}
		GameObject gameObject = null;
		if (scene_arena.playerGrave.Contains(this))
		{
			gameObject = scene_arena.playerGraveObj;
		}
		if (null != gameObject)
		{
			ImbibeBuff = null;
			BadDotList.Clear();
			CrowdControlList.Clear();
			DebuffList.Clear();
			Durance = false;
			if (null != puppetComponents)
			{
				puppetComponents.gameObject.SetActiveRecursively(false);
				puppetComponents.transform.parent = gameObject.transform;
			}
			base.gameObject.SetActiveRecursively(false);
			base.gameObject.transform.parent = gameObject.transform;
			puppetState = ArenaPuppetState.Grave;
			puppet_monitor.OnBodyRecycle();
		}
		else
		{
			if (null != puppetComponents)
			{
				UnityEngine.Object.Destroy(puppetComponents.gameObject);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected void FearMove()
	{
		if (!Durance)
		{
			puppetMovement.last_move_position = base.transform.position;
			puppetController.Move(puppetMovement.rotate_target * Vector3.forward * Time.deltaTime * puppetMovement.move_velocity);
			base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			if (!CheckMoveInWorldLimit())
			{
				base.transform.position = puppetMovement.last_move_position;
			}
		}
	}

	public void OnCrowdControlEnable(TriggerCrowdControl.ControlType control_type)
	{
		if (!IsDead())
		{
			switch (control_type)
			{
			case TriggerCrowdControl.ControlType.STANDSTILL:
				OnStandStill();
				break;
			case TriggerCrowdControl.ControlType.FEAR:
				OnFear();
				break;
			case TriggerCrowdControl.ControlType.DURANCE:
				OnDurance();
				break;
			}
		}
	}

	public void OnCrowdControlDisable(TriggerCrowdControl.ControlType control_type)
	{
		if (!IsDead())
		{
			switch (control_type)
			{
			case TriggerCrowdControl.ControlType.STANDSTILL:
				OnStandStillDisable();
				break;
			case TriggerCrowdControl.ControlType.FEAR:
				OnFearDisable();
				break;
			case TriggerCrowdControl.ControlType.DURANCE:
				OnDuranceDisable();
				break;
			}
		}
	}

	private void OnDurance()
	{
		if (!IsDead())
		{
			Durance = true;
			if (puppetState == ArenaPuppetState.Move || puppetState == ArenaPuppetState.Fear)
			{
				model_builder.PlayPuppetAnimations(true, model_builder.ChangeableIdleClip, WrapMode.Loop);
			}
		}
	}

	private void SetFearMove()
	{
		Quaternion quaternion = Quaternion.Euler(base.transform.rotation.eulerAngles + Vector3.right * 180f);
		Vector3 vector = quaternion * Vector3.forward * 99999f;
		SetRotationTarget(vector);
		puppetMovement.move_start = base.transform.position;
		puppetMovement.move_target = vector;
		puppetMovement.last_move_position = base.transform.position;
		if (!Durance)
		{
			model_builder.PlayPuppetAnimations(true, model_builder.ChangeableMoveClip, WrapMode.Loop);
		}
		else
		{
			model_builder.PlayPuppetAnimations(true, model_builder.ChangeableIdleClip, WrapMode.Loop);
		}
	}

	private void OnFear()
	{
		if (IsDead() || puppetState == ArenaPuppetState.StandStill)
		{
			return;
		}
		puppetState = ArenaPuppetState.Fear;
		if (profile_instance.battle_active_skills != null)
		{
			foreach (string key in profile_instance.battle_active_skills.Keys)
			{
				if (!profile_instance.battle_active_skills[key].IsActivateSkill())
				{
					profile_instance.battle_active_skills[key].forbidden = true;
				}
			}
		}
		excuting_skill = null;
		SetFearMove();
	}

	private void OnFearDisable()
	{
		if (IsDead() || puppetState == ArenaPuppetState.StandStill)
		{
			return;
		}
		if (IsInvoking("OnFear"))
		{
			CancelInvoke("OnFear");
		}
		FreezeSkills(false);
		ReverToIdle();
		if (profile_instance.battle_active_skills != null)
		{
			foreach (string key in profile_instance.battle_active_skills.Keys)
			{
				if (!profile_instance.battle_active_skills[key].IsActivateSkill())
				{
					profile_instance.battle_active_skills[key].forbidden = false;
				}
			}
		}
		puppet_monitor.OnIdle();
		if (puppetState == ArenaPuppetState.Idle)
		{
			if (CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
			else
			{
				SetTarget(TargetPuppet);
			}
		}
	}

	protected void OnDuranceDisable()
	{
		if (!IsDead())
		{
			Durance = false;
			if (puppetState == ArenaPuppetState.Move || puppetState == ArenaPuppetState.Fear)
			{
				model_builder.PlayPuppetAnimations(true, model_builder.ChangeableMoveClip, WrapMode.Loop);
			}
		}
	}

	protected void OnStandStill()
	{
		if (IsDead() || puppetState == ArenaPuppetState.StandStill)
		{
			return;
		}
		puppetState = ArenaPuppetState.StandStill;
		model_builder.PlayPuppetAnimations(false, model_builder.ChangeableIdleClip, WrapMode.Loop);
		model_builder.SetClipSpeed(0, 0f);
		model_builder.SetClipSpeed(1, 0f);
		if (puppetMovement.doing_rotation)
		{
			puppetMovement.doing_rotation = false;
		}
		if (profile_instance.battle_active_skills != null)
		{
			foreach (string key in profile_instance.battle_active_skills.Keys)
			{
				if (!profile_instance.battle_active_skills[key].IsActivateSkill())
				{
					profile_instance.battle_active_skills[key].forbidden = true;
				}
			}
		}
		excuting_skill = null;
	}

	protected void OnStandStillDisable()
	{
		if (IsDead())
		{
			return;
		}
		model_builder.SetClipSpeed(0, 1f);
		model_builder.SetClipSpeed(1, 1f);
		if (CrowdControlList.ContainsKey(TriggerCrowdControl.ControlType.FEAR))
		{
			puppetState = ArenaPuppetState.Fear;
			Invoke("OnFear", 0.1f);
			return;
		}
		FreezeSkills(false);
		ReverToIdle();
		if (profile_instance.battle_active_skills != null)
		{
			foreach (string key in profile_instance.battle_active_skills.Keys)
			{
				if (!profile_instance.battle_active_skills[key].IsActivateSkill())
				{
					profile_instance.battle_active_skills[key].forbidden = false;
				}
			}
		}
		puppet_monitor.OnIdle();
		if (puppetState == ArenaPuppetState.Idle)
		{
			if (CheckHatredTarget())
			{
				SetTarget(hatred_puppet);
			}
			else
			{
				SetTarget(TargetPuppet);
			}
		}
	}

	protected void EnableDistanceLine(float distance, Vector3 position, Quaternion rotation)
	{
	}

	protected void CreateDistanceLine()
	{
	}

	protected void DisableDistanceLine()
	{
	}

	private void Start()
	{
		puppet_monitor.OnEnter();
		CreateDistanceLine();
	}

	private void Awake()
	{
		puppetState = ArenaPuppetState.Idle;
		ImbibeBuff = null;
		BadDotList = new List<DotVariable>();
		CrowdControlList = new Dictionary<TriggerCrowdControl.ControlType, CrowdControl>();
		DebuffList = new List<Buff>();
		TargetPuppet = null;
		common_skill = null;
		charged_skill = null;
		next_skill = null;
		excuting_skill = null;
		friend_target = null;
		puppetMovement.rotate_speed = 4f * D3DFormulas.RotateCoe;
	}

	protected void Update()
	{
		if (base.tag == "Player")
		{
		}
		if (!scene_arena.IsBattleWinBehaviour)
		{
			profile_instance.puppet_property.hp += profile_instance.puppet_property.hp_recover * Time.deltaTime;
			if (profile_instance.puppet_property.hp > profile_instance.puppet_property.hp_max)
			{
				profile_instance.puppet_property.hp = profile_instance.puppet_property.hp_max;
			}
			profile_instance.puppet_property.mp += profile_instance.puppet_property.mp_recover * Time.deltaTime;
			if (profile_instance.puppet_property.mp > profile_instance.puppet_property.mp_max)
			{
				profile_instance.puppet_property.mp = profile_instance.puppet_property.mp_max;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(profile_instance.HPScale, profile_instance.MPScale);
			}
		}
		ActiveSkillCDTimer();
		PuppetBehaviour();
	}

	private void OnDestroy()
	{
		if (null != puppetComponents)
		{
			UnityEngine.Object.Destroy(puppetComponents.gameObject);
		}
	}

	public override void SetPuppetController()
	{
		base.SetPuppetController();
		puppetMovement.move_velocity = profile_instance.puppet_property.move_speed;
		puppetMovement.rotate_speed = puppetMovement.move_velocity * D3DFormulas.RotateCoe;
	}

	public void DebugSetSuperRotationSpeed()
	{
		puppetMovement.rotate_speed *= 5f;
	}

	public void InitPuppetComponents()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/PuppetComponentsPrefab"));
		puppetComponents = gameObject.GetComponent<PuppetComponents>();
		if ("Player" == base.gameObject.tag)
		{
			puppetComponents.Initialize(base.gameObject, true, true, true);
			PuppetComponents.MpType mp_type = PuppetComponents.MpType.MP;
			if (profile_instance.puppet_class.sp_class)
			{
				mp_type = PuppetComponents.MpType.SP;
			}
			puppetComponents.InitializeBars(PuppetComponents.BarType.PLAYER, mp_type);
			puppetComponents.InitializeRing(Vector3.one * model_builder.TransformCfg.ring_size * profile_instance.PuppetScale, (Material)Resources.Load("Dungeons3D/Images/ring_green_M"), (Material)Resources.Load("Dungeons3D/Images/ring_hero_M"));
			profile_instance.puppet_class.apply_hatred_send = profile_instance.puppet_class.player_hatred_send;
			profile_instance.puppet_class.apply_hatred_resist = profile_instance.puppet_class.player_hatred_resist;
		}
		else if ("Enemy" == base.gameObject.tag)
		{
			puppetComponents.Initialize(base.gameObject, true, false, true);
			puppetComponents.InitializeBars(PuppetComponents.BarType.ENEMY, PuppetComponents.MpType.HIDE);
			profile_instance.puppet_class.apply_hatred_send = profile_instance.puppet_class.enemy_hatred_send;
			profile_instance.puppet_class.apply_hatred_resist = profile_instance.puppet_class.enemy_hatred_resist;
		}
		puppetComponents.InitializeShadow(Vector3.one * model_builder.TransformCfg.ring_size * profile_instance.PuppetScale);
	}

	public void InitExpComponents(int exp_bonus)
	{
		this.exp_bonus = Mathf.RoundToInt((float)exp_bonus * profile_instance.puppet_property.exp_percent + profile_instance.puppet_property.exp_up);
		int num = 0;
		if (D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
		{
			num = Mathf.RoundToInt((float)this.exp_bonus * D3DGamer.Instance.ExpBonus);
			if (num < 1)
			{
				num = 1;
			}
		}
		puppetComponents.SetExpComponent();
		GameObject gameObject = new GameObject("ExpFont");
		gameObject.transform.position = puppetComponents.BarsObj.transform.position - new Vector3(0f, -0.2f, 0f);
		PopFont3D popFont3D = gameObject.AddComponent<PopFont3D>();
		popFont3D.SetFont("Adventure ", 29, D3DMain.Instance.CommonFontColor);
		popFont3D.SetString("EXP " + this.exp_bonus + ((num != 0) ? (" + " + num) : string.Empty), 0f, BoardType.HitNumberRaise, 0.7f);
		popFont3D.SetRaiseParameter(2f, 0.6f);
		gameObject = new GameObject("LevelFont");
		gameObject.transform.parent = puppetComponents.BarsObj.transform;
		gameObject.transform.localPosition = new Vector3(-1.9f, 0.2f, 0f);
		popFont3D = gameObject.AddComponent<PopFont3D>();
		popFont3D.SetFont("Adventure ", 29, D3DMain.Instance.CommonFontColor);
		popFont3D.SetString("Lv" + profile_instance.puppet_level, 0f, BoardType.Static, 0.6f);
		if (profile_instance.IsLevelMax())
		{
			puppetComponents.ClipBar(true, 1f);
		}
		else
		{
			puppetComponents.ClipBar(true, profile_instance.ExpScale);
		}
		this.exp_bonus += num;
		exp_add_delta = (float)this.exp_bonus / 1.5f;
	}

	public bool UpdateExpComponents()
	{
		if (exp_bonus <= 0)
		{
			return false;
		}
		int num = (int)(Time.deltaTime * exp_add_delta);
		if (num <= 0)
		{
			num = 1;
		}
		if (exp_bonus < num)
		{
			num = exp_bonus;
			exp_bonus = 0;
		}
		else
		{
			exp_bonus -= num;
		}
		if (profile_instance.IsLevelMax())
		{
			return true;
		}
		profile_instance.current_exp += num;
		int num2 = D3DFormulas.ConvertLevelUpExp(profile_instance.puppet_level);
		if (profile_instance.current_exp >= num2)
		{
			profile_instance.puppet_level++;
			profile_instance.current_exp -= num2;
			BasicEffectComponent.PlayEffect("level up", base.gameObject, string.Empty, true, Vector2.one, Vector3.zero, true, 0f);
			if (null == puppetComponents.BarsObj.transform.Find("NewSkillFont") && profile_instance.CheckNewSkill())
			{
				GameObject gameObject = new GameObject("NewSkillFont");
				gameObject.transform.parent = puppetComponents.BarsObj.transform;
				gameObject.transform.localPosition = new Vector3(0f, -0.3f, 0f);
				PopFont3D popFont3D = gameObject.AddComponent<PopFont3D>();
				popFont3D.SetFont("Adventure ", 29, new Color(0f, 0.8235294f, 1f));
				popFont3D.SetString("NEW SKILL!", 0f, BoardType.NewSkill, 0.7f);
			}
		}
		PopFont3D componentInChildren = puppetComponents.BarsObj.GetComponentInChildren<PopFont3D>();
		componentInChildren.SetString("Lv" + profile_instance.puppet_level, 0f, BoardType.Static, 0.6f);
		if (profile_instance.IsLevelMax())
		{
			puppetComponents.ClipBar(true, 1f);
		}
		else
		{
			puppetComponents.ClipBar(true, profile_instance.ExpScale);
		}
		return true;
	}

	public void SyncComponentsPosition()
	{
		if (null != puppetComponents)
		{
			puppetComponents.transform.position = base.transform.position;
		}
	}

	public void ReverToIdle()
	{
		puppetState = ArenaPuppetState.Idle;
		model_builder.PlayPuppetAnimations(true, model_builder.ChangeableIdleClip, WrapMode.Loop);
	}

	protected override void OnPositionTarget()
	{
		ReverToIdle();
	}

	public void WinnerBehaviour()
	{
		model_builder.PlayPuppetAnimations(true, 10);
		puppetState = ArenaPuppetState.WinnerIdle;
	}

	public bool IsIdle()
	{
		return ArenaPuppetState.Idle == puppetState;
	}

	public bool IsMove()
	{
		return ArenaPuppetState.Move == puppetState;
	}

	public bool IsExecutingSkill()
	{
		return ArenaPuppetState.ExecutingSkill == puppetState;
	}

	public bool IsDead()
	{
		return ArenaPuppetState.Dead == puppetState;
	}

	public bool IsInGrave()
	{
		return ArenaPuppetState.Grave == puppetState;
	}

	public bool IsBadState()
	{
		return puppetState == ArenaPuppetState.Dead || puppetState == ArenaPuppetState.StandStill || ArenaPuppetState.Fear == puppetState;
	}

	public void OnBeChoosed()
	{
		if (null != puppetComponents)
		{
			puppetComponents.SetRingState(PuppetComponents.RingAnimateState.ZOOM);
		}
		if (scene_arena.activing_puppet == this)
		{
			UpdateBattleSkillUI();
		}
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
	}

	public void OnCancelChoosed()
	{
		if (null != puppetComponents)
		{
			puppetComponents.SetRingState(PuppetComponents.RingAnimateState.STOP);
		}
		if (scene_arena.activing_puppet == this)
		{
			ClearBattleSkillUI();
		}
	}

	public void Dead()
	{
		puppetState = ArenaPuppetState.Dead;
		model_builder.PlayPuppetAnimations(false, 9, WrapMode.ClampForever);
		if (scene_arena.activing_puppet == this)
		{
			ClearBattleSkillUI();
			scene_arena.activing_puppet = null;
		}
		foreach (AureoleBehaviour item in scene_arena.AureoleManager)
		{
			item.RemoveAffectTarget(this);
		}
		if (null != puppetComponents)
		{
			puppetComponents.BarVisible(false);
			puppetComponents.SetRingState(PuppetComponents.RingAnimateState.STOP);
			puppetComponents.SetRingState(PuppetComponents.RingAnimateState.FLASH);
		}
		puppetController.enabled = false;
		if (base.gameObject.tag == "Player")
		{
			scene_arena.OnPlayerDead(this);
		}
		else if (base.gameObject.tag == "Enemy")
		{
			scene_arena.OnEnemyDead(this);
		}
		puppet_monitor.OnDead();
	}

	public bool Revive(float hp_recover_percent, float mp_recover_percent, ref ArenaPuppetState revive_state)
	{
		excuting_skill = null;
		charged_skill = null;
		if (!BaseRevive(hp_recover_percent, mp_recover_percent, ref revive_state))
		{
			return false;
		}
		TargetPuppet = null;
		common_cd_delta.value = 0f;
		if (profile_instance.battle_active_skills != null)
		{
			foreach (string key in profile_instance.battle_active_skills.Keys)
			{
				profile_instance.battle_active_skills[key].freeze = false;
				profile_instance.battle_active_skills[key].forbidden = false;
				profile_instance.battle_active_skills[key].RefreshCD();
			}
		}
		return true;
	}

	private bool BaseRevive(float hp_recover_percent, float mp_recover_percent, ref ArenaPuppetState revive_state)
	{
		if (puppetState != ArenaPuppetState.Dead && puppetState != ArenaPuppetState.Grave)
		{
			return false;
		}
		revive_state = puppetState;
		profile_instance.puppet_property.hp = Mathf.Round(profile_instance.puppet_property.hp_max * hp_recover_percent);
		if (profile_instance.puppet_property.hp > profile_instance.puppet_property.hp_max)
		{
			profile_instance.puppet_property.hp = profile_instance.puppet_property.hp_max;
		}
		profile_instance.puppet_property.mp = Mathf.Round(profile_instance.puppet_property.mp_max * mp_recover_percent);
		if (profile_instance.puppet_property.mp > profile_instance.puppet_property.mp_max)
		{
			profile_instance.puppet_property.mp = profile_instance.puppet_property.mp_max;
		}
		if (null != puppetComponents)
		{
			puppetComponents.ClipBar(true, profile_instance.HPScale);
			puppetComponents.ClipBar(false, profile_instance.MPScale);
			puppetComponents.transform.parent = null;
			puppetComponents.SetRingState(PuppetComponents.RingAnimateState.STOP);
			puppetComponents.gameObject.SetActiveRecursively(true);
		}
		base.gameObject.transform.parent = null;
		base.gameObject.SetActiveRecursively(true);
		model_builder.CheckHelmHide();
		ClearHatredPuppet();
		if (base.gameObject.tag == "Player")
		{
			if (scene_arena.playerTallyList.Contains(this))
			{
				scene_arena.player_rest++;
			}
			if (scene_arena.playerRecycleList.Contains(this))
			{
				scene_arena.playerRecycleList.Remove(this);
			}
			scene_arena.playerGrave.Remove(this);
			scene_arena.playerList.Add(this);
		}
		else if (base.gameObject.tag == "Enemy")
		{
			if (scene_arena.enemyRecycleList.Contains(this))
			{
				scene_arena.enemyRecycleList.Remove(this);
			}
			scene_arena.enemyGrave.Remove(this);
			scene_arena.enemyList.Add(this);
		}
		puppetController.enabled = true;
		base.transform.rotation = Quaternion.identity;
		model_builder.SetAllClipSpeed(1f);
		scene_arena.ui_arena.AddFaceButton(this);
		ReverToIdle();
		puppet_monitor.OnRevive();
		return true;
	}

	public float Variable(VariableOutputData variable_output_data)
	{
		if (puppetState == ArenaPuppetState.Dead || puppetState == ArenaPuppetState.Grave || scene_arena.IsBattleWinBehaviour)
		{
			return 0f;
		}
		bool flag = D3DMain.Instance.Lottery(variable_output_data.critical_chance);
		float num = ((!flag) ? 1f : D3DFormulas.Critical);
		float result = 0f;
		switch (variable_output_data.variable_type)
		{
		case TriggerVariable.VariableType.HP_DAMAGE:
		{
			float num4 = 1f - D3DFormulas.ConvertReduceDmgPercent(profile_instance.puppet_property, variable_output_data.attacker_level);
			float num3 = Mathf.Round((variable_output_data.phy_value * num4 + variable_output_data.mag_value + profile_instance.puppet_property.hp_max * variable_output_data.hpmax_percent + profile_instance.puppet_property.mp_max * variable_output_data.mpmax_percent) * num * profile_instance.puppet_property.DamageReduce - profile_instance.puppet_property.fixed_dmg_reduce);
			if (num3 < 1f)
			{
				num3 = 1f;
			}
			result = ((!(num3 > profile_instance.puppet_property.hp)) ? num3 : profile_instance.puppet_property.hp);
			if (flag)
			{
				PuppetPopFont(num3.ToString(), new Color(0.99607843f, 0.79607844f, 0f), BoardType.HitNumberCritical, 1.1f);
			}
			else
			{
				Color font_color = Color.white;
				if ("Player" == base.tag)
				{
					font_color = Color.red;
					OnAttackedChangeColor();
				}
				PuppetPopFont(num3.ToString(), font_color, BoardType.HitNumberJump, 0.8f);
			}
			profile_instance.puppet_property.hp -= num3;
			if (profile_instance.puppet_property.hp <= 0f)
			{
				if (!D3DGamer.Instance.TutorialState[0] && "Player" == base.tag)
				{
					if (profile_instance.puppet_property.hp_max > 100f || scene_arena.kill_require - scene_arena.kill_count > 2)
					{
						profile_instance.puppet_property.hp = 1f;
					}
					else
					{
						profile_instance.puppet_property.hp = 0f;
						Dead();
					}
				}
				else
				{
					profile_instance.puppet_property.hp = 0f;
					Dead();
				}
			}
			model_builder.BlendHurtAnimation();
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(true, profile_instance.HPScale);
			}
			puppet_monitor.OnHPDecrease();
			break;
		}
		case TriggerVariable.VariableType.HP_RECOVER:
		{
			if (!D3DGamer.Instance.TutorialState[0] && "Player" == base.tag && !_bHealLogged)
			{
				_bHealLogged = true;
			}
			float num2 = Mathf.Round((variable_output_data.phy_value + variable_output_data.mag_value + profile_instance.puppet_property.hp_max * variable_output_data.hpmax_percent + profile_instance.puppet_property.mp_max * variable_output_data.mpmax_percent) * num);
			result = ((!(num2 > profile_instance.puppet_property.hp)) ? num2 : profile_instance.puppet_property.hp);
			if (flag)
			{
				PuppetPopFont(num2.ToString(), new Color(0f, 1f, 14f / 85f), BoardType.HitNumberCritical, 1.1f);
			}
			else
			{
				PuppetPopFont(num2.ToString(), new Color(0f, 1f, 14f / 85f), BoardType.HitNumberRaise, 0.8f);
			}
			profile_instance.puppet_property.hp += num2;
			if (profile_instance.puppet_property.hp > profile_instance.puppet_property.hp_max)
			{
				profile_instance.puppet_property.hp = profile_instance.puppet_property.hp_max;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(true, profile_instance.HPScale);
			}
			break;
		}
		case TriggerVariable.VariableType.MP_DAMAGE:
		{
			if (profile_instance.puppet_class.sp_class)
			{
				PuppetPopFont("IMMUNITY", Color.white, BoardType.HitNumberRaise, 0.8f);
				break;
			}
			float num3 = Mathf.Round((variable_output_data.phy_value + variable_output_data.mag_value + profile_instance.puppet_property.hp_max * variable_output_data.hpmax_percent + profile_instance.puppet_property.mp_max * variable_output_data.mpmax_percent) * num);
			result = ((!(num3 > profile_instance.puppet_property.mp)) ? num3 : profile_instance.puppet_property.mp);
			if (flag)
			{
				PuppetPopFont(num3.ToString(), Color.blue, BoardType.HitNumberCritical, 1.1f);
			}
			else
			{
				PuppetPopFont(num3.ToString(), Color.blue, BoardType.HitNumberJump, 0.8f);
			}
			profile_instance.puppet_property.mp -= num3;
			if (profile_instance.puppet_property.mp < 0f)
			{
				profile_instance.puppet_property.mp = 0f;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(false, profile_instance.MPScale);
			}
			break;
		}
		case TriggerVariable.VariableType.MP_RECOVER:
		{
			if (profile_instance.puppet_class.sp_class)
			{
				PuppetPopFont("0", Color.blue, BoardType.HitNumberRaise, 0.8f);
				break;
			}
			float num2 = Mathf.Round((variable_output_data.phy_value + variable_output_data.mag_value + profile_instance.puppet_property.hp_max * variable_output_data.hpmax_percent + profile_instance.puppet_property.mp_max * variable_output_data.mpmax_percent) * num);
			result = ((!(num2 > profile_instance.puppet_property.mp)) ? num2 : profile_instance.puppet_property.mp);
			if (flag)
			{
				PuppetPopFont(num2.ToString(), Color.blue, BoardType.HitNumberCritical, 1.1f);
			}
			else
			{
				PuppetPopFont(num2.ToString(), Color.blue, BoardType.HitNumberRaise, 0.8f);
			}
			profile_instance.puppet_property.mp += num2;
			if (profile_instance.puppet_property.mp > profile_instance.puppet_property.mp_max)
			{
				profile_instance.puppet_property.mp = profile_instance.puppet_property.mp_max;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(false, profile_instance.MPScale);
			}
			break;
		}
		}
		return result;
	}

	public void Variable(TriggerVariable.VariableType variable_type, float value, bool popfont = true)
	{
		if (puppetState == ArenaPuppetState.Dead || puppetState == ArenaPuppetState.Grave || scene_arena.IsBattleWinBehaviour)
		{
			return;
		}
		value = Mathf.Round(value);
		switch (variable_type)
		{
		case TriggerVariable.VariableType.HP_DAMAGE:
			if (popfont)
			{
				Color font_color = Color.white;
				if ("Player" == base.tag)
				{
					font_color = Color.red;
				}
				PuppetPopFont(value.ToString(), font_color, BoardType.HitNumberJump, 0.8f);
			}
			profile_instance.puppet_property.hp -= value;
			if (profile_instance.puppet_property.hp <= 0f)
			{
				profile_instance.puppet_property.hp = 0f;
				Dead();
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(true, profile_instance.HPScale);
			}
			if (!IsDead())
			{
				puppet_monitor.OnHPDecrease();
			}
			break;
		case TriggerVariable.VariableType.HP_RECOVER:
			if (popfont)
			{
				PuppetPopFont(value.ToString(), Color.green, BoardType.HitNumberRaise, 0.8f);
			}
			profile_instance.puppet_property.hp += value;
			if (profile_instance.puppet_property.hp > profile_instance.puppet_property.hp_max)
			{
				profile_instance.puppet_property.hp = profile_instance.puppet_property.hp_max;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(true, profile_instance.HPScale);
			}
			break;
		case TriggerVariable.VariableType.MP_DAMAGE:
			if (profile_instance.puppet_class.sp_class)
			{
				if (popfont)
				{
					PuppetPopFont("IMMUNITY", Color.white, BoardType.HitNumberRaise, 0.8f);
				}
				break;
			}
			if (popfont)
			{
				PuppetPopFont(value.ToString(), Color.blue, BoardType.HitNumberJump, 0.8f);
			}
			profile_instance.puppet_property.mp -= value;
			if (profile_instance.puppet_property.mp < 0f)
			{
				profile_instance.puppet_property.mp = 0f;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(false, profile_instance.MPScale);
			}
			break;
		case TriggerVariable.VariableType.MP_RECOVER:
			if (profile_instance.puppet_class.sp_class)
			{
				if (popfont)
				{
					PuppetPopFont("0", Color.blue, BoardType.HitNumberRaise, 0.8f);
				}
				break;
			}
			PuppetPopFont(value.ToString(), Color.blue, BoardType.HitNumberRaise, 0.8f);
			profile_instance.puppet_property.mp += value;
			if (profile_instance.puppet_property.mp > profile_instance.puppet_property.mp_max)
			{
				profile_instance.puppet_property.mp = profile_instance.puppet_property.mp_max;
			}
			if (null != puppetComponents)
			{
				puppetComponents.ClipBar(false, profile_instance.MPScale);
			}
			break;
		}
	}

	public void KnockBack(Quaternion direction)
	{
	}

	public void PuppetPopFont(string text, Color font_color, BoardType board_type, float font_scale)
	{
		GameObject gameObject = new GameObject("Font");
		gameObject.transform.position = puppetComponents.BarsObj.transform.position + Vector3.up * 0.2f;
		PopFont3D popFont3D = gameObject.AddComponent<PopFont3D>();
		popFont3D.SetFont("Adventure ", 29, font_color);
		popFont3D.SetString(text, 0f, board_type, font_scale);
	}

	public void SetBattleSkill()
	{
		string text = profile_instance.puppet_class.basic_skill_id[current_animation_type];
		if (string.Empty != text)
		{
			profile_instance.basic_skill = new D3DClassBattleSkillStatus();
			profile_instance.basic_skill.skill_id = text;
			if (!profile_instance.basic_skill.Init())
			{
				profile_instance.basic_skill = null;
			}
			else if (profile_instance.basic_skill.active_skill.active_type == D3DActiveSkill.ActiveType.PROMPT)
			{
				profile_instance.basic_skill = null;
			}
			else
			{
				profile_instance.basic_skill.skill_level = 0;
				profile_instance.basic_skill.cancelable = true;
			}
			common_skill = profile_instance.basic_skill;
		}
		if (profile_instance.battle_active_slots == null)
		{
			return;
		}
		bool flag = false;
		profile_instance.battle_active_skills = new Dictionary<string, D3DClassBattleSkillStatus>();
		for (int i = 0; i < profile_instance.battle_active_slots.Length; i++)
		{
			D3DClassActiveSkillStatus statusProperties = profile_instance.puppet_class.active_skill_id_list[profile_instance.battle_active_slots[i]];
			D3DClassBattleSkillStatus d3DClassBattleSkillStatus = new D3DClassBattleSkillStatus();
			d3DClassBattleSkillStatus.SetStatusProperties(statusProperties);
			if (d3DClassBattleSkillStatus.Init())
			{
				profile_instance.battle_active_skills.Add(profile_instance.battle_active_slots[i], d3DClassBattleSkillStatus);
				if (d3DClassBattleSkillStatus.active_skill.activation && !flag)
				{
					flag = true;
					TriggerSkill(d3DClassBattleSkillStatus, false);
				}
			}
		}
	}

	public void ClearBattleSkillUI()
	{
		scene_arena.ui_arena.ClearBattleSkillUI();
	}

	public void Dispel(int dispel_count)
	{
		BasicEffectComponent.PlayEffect("disperse", base.gameObject, string.Empty, true, Vector2.zero, Vector3.zero, true, 0f);
		while (dispel_count > 0 && (BadDotList.Count != 0 || DebuffList.Count != 0 || CrowdControlList.Count != 0))
		{
			if (D3DMain.Instance.Lottery(0.5f) && BadDotList.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, BadDotList.Count);
				UnityEngine.Object.DestroyImmediate(BadDotList[index].gameObject);
				dispel_count--;
			}
			else if (D3DMain.Instance.Lottery(0.5f) && DebuffList.Count > 0)
			{
				int index2 = UnityEngine.Random.Range(0, DebuffList.Count);
				UnityEngine.Object.DestroyImmediate(DebuffList[index2].gameObject);
				dispel_count--;
			}
			else
			{
				if (!D3DMain.Instance.Lottery(0.5f) || CrowdControlList.Count <= 0)
				{
					continue;
				}
				int num = UnityEngine.Random.Range(0, CrowdControlList.Count);
				int num2 = 0;
				foreach (TriggerCrowdControl.ControlType key in CrowdControlList.Keys)
				{
					if (num2 == num)
					{
						UnityEngine.Object.DestroyImmediate(CrowdControlList[key].gameObject);
						dispel_count--;
						break;
					}
					num2++;
				}
			}
		}
	}

	private void SetAttackedShader(bool bSpecial, Color color)
	{
		Transform child = base.transform.GetChild(0);
		color /= 255f;
		if (!(child != null))
		{
			return;
		}
		foreach (Material item in ((PuppetAvatarBuilder)model_builder).MaterialWhileAttacked)
		{
			Shader shader = Shader.Find("Triniti/Character/" + ((!bSpecial) ? "COL_2S" : "COL_ACOL"));
			item.shader = shader;
		}
	}

	private void OnAttackedChangeColor()
	{
		SetAttackedShader(true, Color.black);
		float fTime = 0.3f;
		StopCoroutine("OnAttackedChangeColor");
		StartCoroutine(CoroutineChangeColor(fTime));
	}

	private IEnumerator CoroutineChangeColor(float fTime)
	{
		float fTimePassed = 0f;
		while (fTimePassed < fTime)
		{
			Color color = Color.black;
			float fTimeHalf = fTime / 2f;
			if (fTimePassed < fTimeHalf)
			{
				float fRatio2 = fTimePassed / fTimeHalf;
				color.r = 255f * Mathf.Sin(fRatio2 * (float)Math.PI / 2f);
			}
			else
			{
				float fRatio = fTimePassed / fTimeHalf - 1f;
				color.r = 255f * Mathf.Sin(fRatio * (float)Math.PI / 2f + (float)Math.PI / 2f);
			}
			color /= 255f;
			foreach (Material obj in ((PuppetAvatarBuilder)model_builder).MaterialWhileAttacked)
			{
				obj.SetColor("_AColor", color);
			}
			fTimePassed += Time.deltaTime;
			yield return 0;
		}
		SetAttackedShader(false, Color.black);
	}
}
