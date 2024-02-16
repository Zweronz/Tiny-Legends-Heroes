using System.Collections.Generic;
using UnityEngine;

public class PuppetAutoSpot : MonoBehaviour
{
	private PuppetArena puppet_component;

	private float heart_beat;

	private float spot_radius;

	private List<PuppetArena> spot_enemy_list;

	private void Awake()
	{
		puppet_component = GetComponent<PuppetArena>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		heart_beat += Time.deltaTime;
		if (!(heart_beat > 1f))
		{
			return;
		}
		heart_beat = 0f;
		if ((puppet_component.TargetPuppet != null && !puppet_component.TargetPuppet.IsDead()) || puppet_component.IsInGrave() || !puppet_component.IsIdle())
		{
			return;
		}
		D3DClassBattleSkillStatus d3DClassBattleSkillStatus = puppet_component.HaveAttackSkill();
		if (d3DClassBattleSkillStatus == null)
		{
			return;
		}
		spot_radius = d3DClassBattleSkillStatus.Distance;
		if (spot_radius < 5f)
		{
			spot_radius = 5f;
		}
		else if (spot_radius > 15f)
		{
			spot_radius = 15f;
		}
		float num = 99999f;
		PuppetArena puppetArena = null;
		foreach (PuppetArena item in spot_enemy_list)
		{
			float num2 = Vector3.Distance(base.transform.position, item.transform.position);
			if (num2 < spot_radius && num2 < num)
			{
				num = num2;
				puppetArena = item;
			}
		}
		if (null != puppetArena)
		{
			puppet_component.ReceiveHatredPuppet(puppetArena, puppetArena.profile_instance.puppet_class.apply_hatred_send, false);
			puppet_component.SetTarget(puppetArena);
		}
	}

	public void Init(List<PuppetArena> spot_list, float radius)
	{
		spot_enemy_list = spot_list;
		spot_radius = radius;
	}
}
