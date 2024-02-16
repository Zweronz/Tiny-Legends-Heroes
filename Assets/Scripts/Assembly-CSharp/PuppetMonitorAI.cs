using System.Collections.Generic;
using UnityEngine;

public class PuppetMonitorAI : PuppetMonitorBasic
{
	private class LoopClock
	{
		public float delta_time;

		public float cycle_time;

		public string trigger_skill_id;

		public LoopClock(float cycle_time, string trigger_skill_id)
		{
			delta_time = 0f;
			this.cycle_time = cycle_time;
			this.trigger_skill_id = trigger_skill_id;
		}
	}

	private D3DPuppetAI puppet_ai;

	private Dictionary<string, int> battle_skills;

	private SortedDictionary<float, string> on_hp_decreased_skill;

	private List<float> skipped_hp_keys;

	private int auto_revived_time;

	private bool use_battle_skill;

	private bool lock_skill;

	private bool use_common_skill;

	private bool disrupt_hatred;

	private Dictionary<int, D3DPuppetAI.SkillBehaviour> on_friend_count_changed;

	private Dictionary<int, D3DPuppetAI.SkillBehaviour> on_enemy_count_changed;

	private Dictionary<int, string> on_summoned_count_changed;

	private List<string> trigger_skills;

	private int survival_index;

	private List<PuppetArena> friend_list;

	private List<PuppetArena> enemy_list;

	private D3DClassBattleSkillStatus check_target_skill;

	private bool check_target_ignore_cd;

	private bool check_move;

	private float check_move_delta_time;

	private List<LoopClock> loop_clocks;

	private new void Awake()
	{
		base.Awake();
		puppet_ai = D3DMain.Instance.GetPuppetAI(puppet_component.profile_instance.ProfileID);
		auto_revived_time = 0;
		skipped_hp_keys = new List<float>();
		trigger_skills = new List<string>();
		loop_clocks = new List<LoopClock>();
		if ("Player" == base.tag)
		{
			friend_list = puppet_component.scene_arena.playerList;
			enemy_list = puppet_component.scene_arena.enemyList;
		}
		else
		{
			enemy_list = puppet_component.scene_arena.playerList;
			friend_list = puppet_component.scene_arena.enemyList;
		}
		Reset();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (puppet_component.scene_arena.IsBattleWinBehaviour)
		{
			Object.Destroy(this);
			return;
		}
		foreach (LoopClock loop_clock in loop_clocks)
		{
			loop_clock.delta_time += Time.deltaTime;
			if (loop_clock.delta_time >= loop_clock.cycle_time)
			{
				if (string.Empty != loop_clock.trigger_skill_id)
				{
					trigger_skills.Add(loop_clock.trigger_skill_id);
					OnAddTriggerSkill();
				}
				loop_clock.delta_time = 0f;
			}
		}
		if (check_move)
		{
			check_move_delta_time += Time.deltaTime;
			if (check_move_delta_time >= 5f)
			{
				check_move = false;
				check_move_delta_time = 0f;
				OnIdle();
			}
		}
	}

	public override void OnEnter()
	{
		Invoke("EnableBattleSkill", puppet_ai.entrance_skill_cd);
		if (puppet_ai.discrupt_hatred != null)
		{
			Invoke("EnableDisruptHatred", puppet_ai.discrupt_hatred.cycle_time);
		}
		TrySkill(puppet_component.CommonSkill, false);
		if (string.Empty != puppet_ai.on_entrance_skill && puppet_component.profile_instance.CheckBattleActiveSkill(puppet_ai.on_entrance_skill))
		{
			TrySkill(puppet_component.profile_instance.battle_active_skills[puppet_ai.on_entrance_skill], true);
		}
		else
		{
			puppet_component.SetTarget(puppet_component.TargetPuppet);
		}
	}

	public override void OnDead()
	{
		CancelInvoke();
		if (string.Empty != puppet_ai.on_dead_skill && puppet_component.profile_instance.CheckBattleActiveSkill(puppet_ai.on_dead_skill))
		{
			puppet_component.DeadTriggerSkill(puppet_component.profile_instance.battle_active_skills[puppet_ai.on_dead_skill]);
		}
	}

	public override void OnBodyRecycle()
	{
		if (puppet_ai.auto_revive != null && (puppet_ai.auto_revive.revive_count == -1 || auto_revived_time < puppet_ai.auto_revive.revive_count))
		{
			Invoke("ReviveSchedule", puppet_ai.auto_revive.revive_delay);
			auto_revived_time++;
		}
	}

	public override void OnHPDecrease()
	{
		if (on_hp_decreased_skill == null || on_hp_decreased_skill.Count <= 0)
		{
			return;
		}
		bool flag = false;
		skipped_hp_keys.Clear();
		foreach (float key2 in on_hp_decreased_skill.Keys)
		{
			float num = key2;
			if (flag)
			{
				skipped_hp_keys.Add(num);
			}
			else if (puppet_component.profile_instance.HPScale <= num)
			{
				if (string.Empty != on_hp_decreased_skill[num])
				{
					trigger_skills.Add(on_hp_decreased_skill[num]);
					OnAddTriggerSkill();
				}
				skipped_hp_keys.Add(num);
				flag = true;
			}
		}
		foreach (float skipped_hp_key in skipped_hp_keys)
		{
			float key = skipped_hp_key;
			on_hp_decreased_skill.Remove(key);
		}
	}

	public override void OnFriendCountChange(int count)
	{
		if (on_friend_count_changed == null || !on_friend_count_changed.ContainsKey(count))
		{
			return;
		}
		D3DPuppetAI.SkillBehaviour skillBehaviour = on_friend_count_changed[count];
		if (skillBehaviour != null)
		{
			if (skillBehaviour.remove_skills_from_list != null)
			{
				foreach (string item in skillBehaviour.remove_skills_from_list)
				{
					if (battle_skills.ContainsKey(item))
					{
						battle_skills.Remove(item);
					}
				}
			}
			if (skillBehaviour.add_skills_to_list != null)
			{
				foreach (string key in skillBehaviour.add_skills_to_list.Keys)
				{
					int num = skillBehaviour.add_skills_to_list[key];
					if (num == -1)
					{
						num = 999999;
					}
					if (battle_skills.ContainsKey(key))
					{
						battle_skills[key] = num;
					}
					else
					{
						battle_skills.Add(key, num);
					}
				}
			}
			if (string.Empty != skillBehaviour.trigger_skill)
			{
				trigger_skills.Add(skillBehaviour.trigger_skill);
				OnAddTriggerSkill();
			}
		}
		on_friend_count_changed.Remove(count);
	}

	public override void OnEnemyCountChange(int count)
	{
		if (on_enemy_count_changed == null || !on_enemy_count_changed.ContainsKey(count))
		{
			return;
		}
		D3DPuppetAI.SkillBehaviour skillBehaviour = on_enemy_count_changed[count];
		if (skillBehaviour != null)
		{
			if (skillBehaviour.remove_skills_from_list != null)
			{
				foreach (string item in skillBehaviour.remove_skills_from_list)
				{
					if (battle_skills.ContainsKey(item))
					{
						battle_skills.Remove(item);
					}
				}
			}
			if (skillBehaviour.add_skills_to_list != null)
			{
				foreach (string key in skillBehaviour.add_skills_to_list.Keys)
				{
					int num = skillBehaviour.add_skills_to_list[key];
					if (num == -1)
					{
						num = 999999;
					}
					if (battle_skills.ContainsKey(key))
					{
						battle_skills[key] = num;
					}
					else
					{
						battle_skills.Add(key, num);
					}
				}
			}
			if (string.Empty != skillBehaviour.trigger_skill)
			{
				trigger_skills.Add(skillBehaviour.trigger_skill);
				OnAddTriggerSkill();
			}
		}
		on_enemy_count_changed.Remove(count);
	}

	public override void OnSummonedCountChange(int count)
	{
		if (on_summoned_count_changed != null && on_summoned_count_changed.ContainsKey(count))
		{
			if (string.Empty != on_summoned_count_changed[count])
			{
				trigger_skills.Add(on_summoned_count_changed[count]);
				OnAddTriggerSkill();
			}
			on_summoned_count_changed.Remove(count);
		}
	}

	private void OnSurvivalTimeOut()
	{
		D3DPuppetAI.SkillBehaviour skillBehaviour = null;
		int num = 0;
		foreach (float key2 in puppet_ai.survival_time_out.Keys)
		{
			float key = key2;
			if (num == survival_index)
			{
				skillBehaviour = puppet_ai.survival_time_out[key];
				break;
			}
			num++;
		}
		if (skillBehaviour != null)
		{
			if (skillBehaviour.remove_skills_from_list != null)
			{
				foreach (string item in skillBehaviour.remove_skills_from_list)
				{
					if (battle_skills.ContainsKey(item))
					{
						battle_skills.Remove(item);
					}
				}
			}
			if (skillBehaviour.add_skills_to_list != null)
			{
				foreach (string key3 in skillBehaviour.add_skills_to_list.Keys)
				{
					int num2 = skillBehaviour.add_skills_to_list[key3];
					if (num2 == -1)
					{
						num2 = 999999;
					}
					if (battle_skills.ContainsKey(key3))
					{
						battle_skills[key3] = num2;
					}
					else
					{
						battle_skills.Add(key3, num2);
					}
				}
			}
			if (string.Empty != skillBehaviour.trigger_skill)
			{
				trigger_skills.Add(skillBehaviour.trigger_skill);
				OnAddTriggerSkill();
			}
		}
		survival_index++;
	}

	public override void OnExcuteSkillOver()
	{
		if (!use_common_skill && !lock_skill)
		{
			use_common_skill = true;
			lock_skill = true;
			Invoke("UnlockSkill", Random.Range(puppet_ai.chain_skill_cd_min, puppet_ai.chain_skill_cd_max));
		}
	}

	public override void OnIdle()
	{
		check_move = false;
		D3DClassBattleSkillStatus d3DClassBattleSkillStatus = puppet_component.CommonSkill;
		bool ignore_cd = false;
		if (!lock_skill || use_common_skill)
		{
			string empty = string.Empty;
			if (trigger_skills.Count > 0)
			{
				empty = trigger_skills[0];
				trigger_skills.RemoveAt(0);
				if (string.Empty != empty && puppet_component.profile_instance.CheckBattleActiveSkill(empty))
				{
					d3DClassBattleSkillStatus = puppet_component.profile_instance.battle_active_skills[empty];
					ignore_cd = true;
				}
			}
			else if (use_battle_skill)
			{
				List<D3DClassBattleSkillStatus> list = new List<D3DClassBattleSkillStatus>();
				foreach (string key2 in battle_skills.Keys)
				{
					if (puppet_component.profile_instance.CheckBattleActiveSkill(key2))
					{
						D3DClassBattleSkillStatus d3DClassBattleSkillStatus2 = puppet_component.profile_instance.battle_active_skills[key2];
						if (d3DClassBattleSkillStatus2.Enable)
						{
							list.Add(d3DClassBattleSkillStatus2);
						}
					}
				}
				if (list.Count > 0)
				{
					int index = Random.Range(0, list.Count);
					d3DClassBattleSkillStatus = list[index];
					Dictionary<string, int> dictionary;
					Dictionary<string, int> dictionary2 = (dictionary = battle_skills);
					string skill_id;
					string key = (skill_id = d3DClassBattleSkillStatus.skill_id);
					int num = dictionary[skill_id];
					dictionary2[key] = num - 1;
					if (battle_skills[d3DClassBattleSkillStatus.skill_id] <= 0)
					{
						battle_skills.Remove(d3DClassBattleSkillStatus.skill_id);
					}
				}
			}
		}
		TrySkill(d3DClassBattleSkillStatus, ignore_cd);
	}

	public override void OnMove()
	{
		check_move = true;
	}

	public override void OnRevive()
	{
		CancelInvoke();
		Reset();
		Invoke("EnableBattleSkill", puppet_ai.entrance_skill_cd);
		if (puppet_ai.discrupt_hatred != null)
		{
			Invoke("EnableDisruptHatred", puppet_ai.discrupt_hatred.cycle_time);
		}
		TrySkill(puppet_component.CommonSkill, false);
		puppet_component.SetTarget(puppet_component.TargetPuppet);
	}

	protected override void Reset()
	{
		trigger_skills.Clear();
		loop_clocks.Clear();
		survival_index = 0;
		use_battle_skill = false;
		lock_skill = false;
		disrupt_hatred = false;
		check_move = false;
		check_move_delta_time = 0f;
		if (puppet_component.profile_instance.battle_active_skills != null)
		{
			battle_skills = new Dictionary<string, int>();
			if (puppet_ai.add_all_skills)
			{
				foreach (string key2 in puppet_component.profile_instance.battle_active_skills.Keys)
				{
					battle_skills.Add(key2, 999999);
				}
			}
			else if (puppet_ai.battle_skill_list != null)
			{
				foreach (string key3 in puppet_ai.battle_skill_list.Keys)
				{
					if (puppet_component.profile_instance.battle_active_skills.ContainsKey(key3))
					{
						if (puppet_ai.battle_skill_list[key3] == -1)
						{
							battle_skills.Add(key3, 999999);
						}
						else
						{
							battle_skills.Add(key3, puppet_ai.battle_skill_list[key3]);
						}
					}
				}
			}
		}
		if (puppet_ai.on_hp_decreased_skill != null)
		{
			on_hp_decreased_skill = new SortedDictionary<float, string>();
			foreach (float key4 in puppet_ai.on_hp_decreased_skill.Keys)
			{
				float key = key4;
				on_hp_decreased_skill.Add(key, puppet_ai.on_hp_decreased_skill[key]);
			}
		}
		if (puppet_ai.on_friend_count_changed != null)
		{
			on_friend_count_changed = new Dictionary<int, D3DPuppetAI.SkillBehaviour>();
			foreach (int key5 in puppet_ai.on_friend_count_changed.Keys)
			{
				on_friend_count_changed.Add(key5, puppet_ai.on_friend_count_changed[key5]);
			}
		}
		if (puppet_ai.on_enemy_count_changed != null)
		{
			on_enemy_count_changed = new Dictionary<int, D3DPuppetAI.SkillBehaviour>();
			foreach (int key6 in puppet_ai.on_enemy_count_changed.Keys)
			{
				on_enemy_count_changed.Add(key6, puppet_ai.on_enemy_count_changed[key6]);
			}
		}
		if (puppet_ai.on_summoned_count_changed != null)
		{
			on_summoned_count_changed = new Dictionary<int, string>();
			foreach (int key7 in puppet_ai.on_summoned_count_changed.Keys)
			{
				on_summoned_count_changed.Add(key7, puppet_ai.on_summoned_count_changed[key7]);
			}
		}
		if (puppet_ai.survival_time_out != null)
		{
			foreach (float key8 in puppet_ai.survival_time_out.Keys)
			{
				float time = key8;
				Invoke("OnSurvivalTimeOut", time);
			}
		}
		if (puppet_ai.loop_clock == null)
		{
			return;
		}
		foreach (float key9 in puppet_ai.loop_clock.Keys)
		{
			float num = key9;
			LoopClock item = new LoopClock(num, puppet_ai.loop_clock[num]);
			loop_clocks.Add(item);
		}
	}

	private void ReviveSchedule()
	{
		if (puppet_component.scene_arena.BattleResult != 0)
		{
			if (puppet_ai.auto_revive.revive_delay != 0f)
			{
				return;
			}
			if ("Player" == base.tag && puppet_component.scene_arena.BattleResult == -1)
			{
				puppet_component.scene_arena.BattleResult = 0;
				puppet_component.scene_arena.ui_arena.StopCoroutine("EndBattleDelay");
			}
			else
			{
				if (!("Enemy" == base.tag) || puppet_component.scene_arena.BattleResult != 1)
				{
					return;
				}
				puppet_component.scene_arena.BattleResult = 0;
				puppet_component.scene_arena.ui_arena.StopCoroutine("EndBattleDelay");
			}
		}
		bool flag = true;
		if (puppet_ai.auto_revive.friend_conditions != null)
		{
			flag = false;
			List<PuppetArena> list = ((!("Player" == base.tag)) ? puppet_component.scene_arena.enemyTallyList : puppet_component.scene_arena.playerTallyList);
			foreach (PuppetArena item in list)
			{
				if (puppet_ai.auto_revive.friend_conditions.Contains(item.profile_instance.ProfileID))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			PuppetArena.ArenaPuppetState revive_state = PuppetArena.ArenaPuppetState.Idle;
			puppet_component.Revive(1f, 1f, ref revive_state);
			if (revive_state == PuppetArena.ArenaPuppetState.Grave)
			{
				puppet_component.scene_arena.SetPuppetRandomPosition(puppet_component);
			}
			BasicEffectComponent.PlayEffect("resurrection", base.transform.position, Quaternion.identity, Vector2.zero, Vector3.zero, true, 0f);
		}
	}

	private void EnableBattleSkill()
	{
		use_battle_skill = true;
		UnlockSkill();
	}

	private void UnlockSkill()
	{
		lock_skill = false;
		if (!use_battle_skill || use_common_skill || (!puppet_component.IsIdle() && !puppet_component.IsMove()))
		{
			return;
		}
		string empty = string.Empty;
		D3DClassBattleSkillStatus d3DClassBattleSkillStatus = null;
		bool ignore_cd = false;
		if (trigger_skills.Count > 0)
		{
			empty = trigger_skills[0];
			trigger_skills.RemoveAt(0);
			if (string.Empty != empty && puppet_component.profile_instance.CheckBattleActiveSkill(empty))
			{
				d3DClassBattleSkillStatus = puppet_component.profile_instance.battle_active_skills[empty];
				ignore_cd = true;
			}
		}
		else
		{
			List<D3DClassBattleSkillStatus> list = new List<D3DClassBattleSkillStatus>();
			foreach (string key2 in battle_skills.Keys)
			{
				if (puppet_component.profile_instance.CheckBattleActiveSkill(key2))
				{
					D3DClassBattleSkillStatus d3DClassBattleSkillStatus2 = puppet_component.profile_instance.battle_active_skills[key2];
					if (d3DClassBattleSkillStatus2.Enable)
					{
						list.Add(d3DClassBattleSkillStatus2);
					}
				}
			}
			if (list.Count > 0)
			{
				int index = Random.Range(0, list.Count);
				d3DClassBattleSkillStatus = list[index];
				Dictionary<string, int> dictionary;
				Dictionary<string, int> dictionary2 = (dictionary = battle_skills);
				string skill_id;
				string key = (skill_id = d3DClassBattleSkillStatus.skill_id);
				int num = dictionary[skill_id];
				dictionary2[key] = num - 1;
				if (battle_skills[d3DClassBattleSkillStatus.skill_id] <= 0)
				{
					battle_skills.Remove(d3DClassBattleSkillStatus.skill_id);
				}
			}
		}
		TrySkill(d3DClassBattleSkillStatus, ignore_cd);
	}

	private void EnableDisruptHatred()
	{
		disrupt_hatred = true;
		if (puppet_component.IsIdle() || puppet_component.IsMove())
		{
			D3DClassBattleSkillStatus d3DClassBattleSkillStatus = puppet_component.ChargedSkill;
			if (d3DClassBattleSkillStatus == null)
			{
				d3DClassBattleSkillStatus = puppet_component.CommonSkill;
			}
			if (d3DClassBattleSkillStatus != null)
			{
				TrySkill(d3DClassBattleSkillStatus, false);
			}
		}
	}

	private void RestartDisruptHatred()
	{
		puppet_component.RecoverHatred();
		if (puppet_ai.discrupt_hatred != null)
		{
			Invoke("EnableDisruptHatred", puppet_ai.discrupt_hatred.cycle_time);
		}
	}

	private void TrySkill(D3DClassBattleSkillStatus skill, bool ignore_cd)
	{
		if (puppet_component.IsInGrave() || puppet_component.IsBadState() || skill == null)
		{
			return;
		}
		if (skill.active_skill.active_type == D3DActiveSkill.ActiveType.TAP_ENEMY)
		{
			if (null != puppet_component.TargetPuppet && puppet_component.TargetPuppet.tag == base.tag)
			{
				puppet_component.TargetPuppet = null;
			}
			if (disrupt_hatred)
			{
				disrupt_hatred = false;
				if (puppet_ai.discrupt_hatred != null)
				{
					Invoke("RestartDisruptHatred", puppet_ai.discrupt_hatred.keep_time);
				}
				if (enemy_list.Count > 1)
				{
					PuppetArena puppetArena;
					do
					{
						int index = Random.Range(0, enemy_list.Count);
						puppetArena = enemy_list[index];
					}
					while (puppetArena == puppet_component.TargetPuppet);
					puppet_component.TargetPuppet = puppetArena;
					puppet_component.ReceiveHatredPuppet(puppetArena, 100, false);
					if (string.Empty != puppet_ai.discrupt_hatred.tirgger_skill && puppet_component.profile_instance.CheckBattleActiveSkill(puppet_ai.discrupt_hatred.tirgger_skill))
					{
						skill = puppet_component.profile_instance.battle_active_skills[puppet_ai.discrupt_hatred.tirgger_skill];
						TrySkill(skill, true);
						return;
					}
				}
			}
			if (null == puppet_component.TargetPuppet || puppet_component.TargetPuppet.IsDead() || puppet_component.IsInGrave())
			{
				puppet_component.TargetPuppet = null;
				if (IsInvoking("RestartDisruptHatred"))
				{
					CancelInvoke("RestartDisruptHatred");
					if (puppet_ai.discrupt_hatred != null)
					{
						Invoke("EnableDisruptHatred", puppet_ai.discrupt_hatred.cycle_time);
					}
				}
				if (null == puppet_component.HatredPuppet || puppet_component.HatredPuppet.IsDead() || puppet_component.HatredPuppet.IsInGrave())
				{
					if (enemy_list.Count > 0)
					{
						int index2 = Random.Range(0, enemy_list.Count);
						PuppetArena puppetArena2 = enemy_list[index2];
						puppet_component.TargetPuppet = puppetArena2;
						puppet_component.ReceiveHatredPuppet(puppetArena2, puppetArena2.profile_instance.puppet_class.apply_hatred_send, false);
					}
				}
				else
				{
					puppet_component.TargetPuppet = puppet_component.HatredPuppet;
				}
			}
			else if (puppet_component.HatredPuppet != puppet_component.TargetPuppet)
			{
				puppet_component.TargetPuppet = puppet_component.HatredPuppet;
			}
		}
		else if (skill.active_skill.active_type == D3DActiveSkill.ActiveType.TAP_FRIEND || skill.active_skill.active_type == D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME)
		{
			if (null != puppet_component.TargetPuppet)
			{
				if (puppet_component.TargetPuppet.tag == base.tag)
				{
					if (puppet_component.TargetPuppet.profile_instance.HPScale >= 1f)
					{
						puppet_component.TargetPuppet = null;
					}
					else if (skill.active_skill.active_type == D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME && puppet_component.TargetPuppet == puppet_component)
					{
						puppet_component.TargetPuppet = null;
					}
				}
				else
				{
					puppet_component.TargetPuppet = null;
				}
			}
			if (null == puppet_component.TargetPuppet || puppet_component.TargetPuppet.IsDead() || puppet_component.IsInGrave())
			{
				puppet_component.TargetPuppet = null;
				float num = 999f;
				foreach (PuppetArena item in friend_list)
				{
					if ((skill.active_skill.active_type != D3DActiveSkill.ActiveType.TAP_FRIEND_EXCLUDE_ME || !(item == puppet_component)) && item.profile_instance.HPScale < num)
					{
						puppet_component.TargetPuppet = item;
						num = item.profile_instance.HPScale;
					}
				}
			}
		}
		if (IsInvoking("CheckMyTarget"))
		{
			CancelInvoke("CheckMyTarget");
		}
		if (null == puppet_component.TargetPuppet)
		{
			check_target_skill = skill;
			check_target_ignore_cd = ignore_cd;
			Invoke("CheckMyTarget", 0.5f);
			return;
		}
		use_common_skill = true;
		if (skill != puppet_component.CommonSkill)
		{
			use_common_skill = false;
			puppet_component.TriggerSkill(skill, ignore_cd);
		}
		else
		{
			puppet_component.SetTarget(puppet_component.TargetPuppet);
		}
	}

	private void OnAddTriggerSkill()
	{
		if (lock_skill || !use_common_skill || (!puppet_component.IsIdle() && !puppet_component.IsMove()))
		{
			return;
		}
		string empty = string.Empty;
		D3DClassBattleSkillStatus skill = null;
		bool ignore_cd = false;
		if (trigger_skills.Count > 0)
		{
			empty = trigger_skills[0];
			trigger_skills.RemoveAt(0);
			if (string.Empty != empty && puppet_component.profile_instance.CheckBattleActiveSkill(empty))
			{
				skill = puppet_component.profile_instance.battle_active_skills[empty];
				ignore_cd = true;
			}
		}
		TrySkill(skill, ignore_cd);
	}

	private void CheckMyTarget()
	{
		TrySkill(check_target_skill, check_target_ignore_cd);
	}
}
