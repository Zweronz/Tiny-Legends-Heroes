using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
	protected PuppetArena control_host;

	protected TriggerCrowdControl crowd_control_config;

	protected float control_lifecycle;

	protected float control_delta_time;

	private List<BasicEffectComponent> awake_effects;

	private List<BasicEffectComponent> loop_effects;

	private List<D3DAudioBehaviour> loop_sfx;

	private void Awake()
	{
		awake_effects = new List<BasicEffectComponent>();
		loop_effects = new List<BasicEffectComponent>();
		loop_sfx = new List<D3DAudioBehaviour>();
	}

	private void Start()
	{
	}

	protected void Update()
	{
		if (null == control_host || control_host.IsDead() || control_host.scene_arena.IsBattleWinBehaviour)
		{
			Object.Destroy(base.gameObject);
		}
		control_delta_time += Time.deltaTime;
		if (control_delta_time >= control_lifecycle)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		foreach (BasicEffectComponent awake_effect in awake_effects)
		{
			if (null != awake_effect)
			{
				awake_effect.Stop();
			}
		}
		foreach (BasicEffectComponent loop_effect in loop_effects)
		{
			if (null != loop_effect)
			{
				loop_effect.Stop();
			}
		}
		foreach (D3DAudioBehaviour item in loop_sfx)
		{
			if (null != item)
			{
				item.Stop();
			}
		}
		if (null != control_host && control_host.CrowdControlList.ContainsKey(crowd_control_config.control_type))
		{
			control_host.CrowdControlList.Remove(crowd_control_config.control_type);
			if (!control_host.IsDead())
			{
				control_host.OnCrowdControlDisable(crowd_control_config.control_type);
			}
		}
	}

	public virtual void InitControl(PuppetArena control_host, TriggerCrowdControl crowd_control_config, int skill_level)
	{
		if (null == control_host || crowd_control_config == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.control_host = control_host;
		this.crowd_control_config = crowd_control_config;
		control_lifecycle = this.crowd_control_config.GetTime(skill_level);
		switch (crowd_control_config.control_type)
		{
		case TriggerCrowdControl.ControlType.DURANCE:
			control_lifecycle -= control_lifecycle * (control_host.profile_instance.puppet_property.stakme_resist * 0.01f);
			break;
		case TriggerCrowdControl.ControlType.FEAR:
			control_lifecycle -= control_lifecycle * (control_host.profile_instance.puppet_property.fear_resist * 0.01f);
			break;
		case TriggerCrowdControl.ControlType.STANDSTILL:
			control_lifecycle -= control_lifecycle * (control_host.profile_instance.puppet_property.stun_resist * 0.01f);
			break;
		}
		if ((double)control_lifecycle <= 0.001)
		{
			control_host.PuppetPopFont("IMMUNITY", Color.white, BoardType.HitNumberRaise, 0.8f);
			Object.Destroy(base.gameObject);
			return;
		}
		control_delta_time = 0f;
		control_host.CrowdControlList.Add(this.crowd_control_config.control_type, this);
		control_host.OnCrowdControlEnable(crowd_control_config.control_type);
		BasicEffectComponent basicEffectComponent = BasicEffectComponent.PlayEffect(this.crowd_control_config.awaken_effect, this.control_host.gameObject, this.crowd_control_config.awaken_mount_point, true, Vector2.one, Vector3.zero, true, 0f);
		if (null != basicEffectComponent)
		{
			awake_effects.Add(basicEffectComponent);
		}
		BasicEffectComponent basicEffectComponent2 = BasicEffectComponent.PlayEffect(this.crowd_control_config.effect, this.control_host.gameObject, this.crowd_control_config.mount_point, true, Vector2.one, Vector3.zero, false, 0f);
		if (null != basicEffectComponent2)
		{
			basicEffectComponent2.name = this.crowd_control_config.effect;
			loop_effects.Add(basicEffectComponent2);
		}
		D3DAudioManager.Instance.PlayAudio(this.crowd_control_config.awaken_sfx, this.control_host.gameObject, false, true);
		D3DAudioBehaviour behaviour = null;
		D3DAudioManager.Instance.PlayAudio(this.crowd_control_config.sfx, ref behaviour, this.control_host.gameObject, false, true);
		if (null != behaviour)
		{
			loop_sfx.Add(behaviour);
		}
		if (null != basicEffectComponent2 || null != behaviour)
		{
			CrowdControlEffectDestoryer crowdControlEffectDestoryer = basicEffectComponent2.gameObject.AddComponent<CrowdControlEffectDestoryer>();
			crowdControlEffectDestoryer.StartCoroutine(crowdControlEffectDestoryer.StopControlEffect(basicEffectComponent2, behaviour, control_lifecycle));
		}
	}

	public virtual void ExtendControl(TriggerCrowdControl extend_control_config, int extend_skill_level)
	{
		if (extend_control_config == null)
		{
			return;
		}
		float num = control_lifecycle - control_delta_time;
		float time = extend_control_config.GetTime(extend_skill_level);
		if (time > num)
		{
			control_delta_time = 0f;
			control_lifecycle = time;
			switch (crowd_control_config.control_type)
			{
			case TriggerCrowdControl.ControlType.DURANCE:
				control_lifecycle -= control_lifecycle * (100f - control_host.profile_instance.puppet_property.stakme_resist) * 0.01f;
				break;
			case TriggerCrowdControl.ControlType.FEAR:
				control_lifecycle -= control_lifecycle * (100f - control_host.profile_instance.puppet_property.fear_resist) * 0.01f;
				break;
			case TriggerCrowdControl.ControlType.STANDSTILL:
				control_lifecycle -= control_lifecycle * (100f - control_host.profile_instance.puppet_property.stun_resist) * 0.01f;
				break;
			}
			if (control_lifecycle <= 0f)
			{
				control_host.PuppetPopFont("IMMUNITY", Color.white, BoardType.HitNumberRaise, 0.8f);
				Object.Destroy(base.gameObject);
				return;
			}
		}
		control_host.OnCrowdControlEnable(crowd_control_config.control_type);
		BasicEffectComponent basicEffectComponent = BasicEffectComponent.PlayEffect(extend_control_config.awaken_effect, control_host.gameObject, extend_control_config.awaken_mount_point, true, Vector2.one, Vector3.zero, true, 0f);
		if (null != basicEffectComponent)
		{
			awake_effects.Add(basicEffectComponent);
		}
		D3DAudioManager.Instance.PlayAudio(extend_control_config.awaken_sfx, control_host.gameObject, false, true);
		BasicEffectComponent basicEffectComponent2 = CheckLoopEffectExisted(extend_control_config.effect);
		D3DAudioBehaviour behaviour = null;
		if (null == basicEffectComponent2)
		{
			basicEffectComponent2 = BasicEffectComponent.PlayEffect(extend_control_config.effect, control_host.gameObject, extend_control_config.mount_point, true, Vector2.one, Vector3.zero, false, 0f);
			if (null != basicEffectComponent2)
			{
				basicEffectComponent2.name = crowd_control_config.effect;
				loop_effects.Add(basicEffectComponent2);
			}
			D3DAudioManager.Instance.PlayAudio(extend_control_config.sfx, ref behaviour, control_host.gameObject, false, true);
			if (null != behaviour)
			{
				loop_sfx.Add(behaviour);
			}
		}
		else
		{
			CrowdControlEffectDestoryer component = basicEffectComponent2.GetComponent<CrowdControlEffectDestoryer>();
			component.StopAllCoroutines();
			component.StartCoroutine(component.StopControlEffect(basicEffectComponent2, behaviour, control_lifecycle));
			basicEffectComponent2 = null;
		}
		if (null != basicEffectComponent2 || null != behaviour)
		{
			GameObject gameObject = new GameObject("CrowdControlEffectDestoryer");
			CrowdControlEffectDestoryer crowdControlEffectDestoryer = gameObject.AddComponent<CrowdControlEffectDestoryer>();
			crowdControlEffectDestoryer.StartCoroutine(crowdControlEffectDestoryer.StopControlEffect(basicEffectComponent2, behaviour, time));
		}
	}

	private BasicEffectComponent CheckLoopEffectExisted(string effect_name)
	{
		foreach (BasicEffectComponent loop_effect in loop_effects)
		{
			if (loop_effect.name == effect_name)
			{
				return loop_effect;
			}
		}
		return null;
	}
}
