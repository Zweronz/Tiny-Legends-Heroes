using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
	private PuppetArena buff_host;

	private TriggerBuff buff_config;

	private int skill_level;

	private BasicEffectComponent awake_effect;

	private BasicEffectComponent loop_effect;

	private D3DAudioBehaviour loop_sfx;

	private float buff_affect_value;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (null == buff_host || buff_host.IsDead() || buff_host.scene_arena.IsBattleWinBehaviour)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		RemoveBuffValue();
		if (null != awake_effect)
		{
			awake_effect.Stop();
		}
		if (null != loop_effect)
		{
			loop_effect.Stop();
		}
		if (null != loop_sfx)
		{
			loop_sfx.Stop();
		}
		if (null != buff_host && buff_host.DebuffList.Contains(this))
		{
			buff_host.DebuffList.Remove(this);
		}
	}

	public void InitBuff(PuppetArena buff_host, TriggerBuff buff_config, int skill_level)
	{
		if (null == buff_host || buff_config == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.buff_host = buff_host;
		this.buff_config = buff_config;
		this.skill_level = skill_level;
		float time = buff_config.GetTime(skill_level);
		if (time >= 0f)
		{
			StartCoroutine(ExpireBuff(time));
		}
		AddBuffValue(false);
		awake_effect = BasicEffectComponent.PlayEffect(this.buff_config.awaken_effect, this.buff_host.gameObject, this.buff_config.awaken_mount_point, true, Vector2.one, Vector3.zero, true, 0f);
		loop_effect = BasicEffectComponent.PlayEffect(this.buff_config.effect, this.buff_host.gameObject, this.buff_config.mount_point, true, Vector2.one, Vector3.zero, false, 0f);
		D3DAudioManager.Instance.PlayAudio(this.buff_config.awaken_sfx, this.buff_host.gameObject, false, true);
		D3DAudioManager.Instance.PlayAudio(this.buff_config.sfx, ref loop_sfx, this.buff_host.gameObject, false, true);
	}

	public void InitBuffByAureloe(PuppetArena buff_host, TriggerBuff buff_config, int skill_level)
	{
		if (null == buff_host || buff_config == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.buff_host = buff_host;
		this.buff_config = buff_config;
		this.skill_level = skill_level;
		AddBuffValue(true);
		awake_effect = BasicEffectComponent.PlayEffect(this.buff_config.awaken_effect, this.buff_host.gameObject, this.buff_config.awaken_mount_point, true, Vector2.one, Vector3.zero, true, 0f);
		loop_effect = BasicEffectComponent.PlayEffect(this.buff_config.effect, this.buff_host.gameObject, this.buff_config.mount_point, true, Vector2.one, Vector3.zero, false, 0f);
		D3DAudioManager.Instance.PlayAudio(this.buff_config.awaken_sfx, this.buff_host.gameObject, false, true);
		D3DAudioManager.Instance.PlayAudio(this.buff_config.sfx, ref loop_sfx, this.buff_host.gameObject, false, true);
	}

	private IEnumerator ExpireBuff(float buff_time)
	{
		yield return new WaitForSeconds(buff_time);
		Object.Destroy(base.gameObject);
	}

	private void AddBuffValue(bool aureole_buff)
	{
		float value = buff_config.GetValue(skill_level);
		switch (buff_config.buff_type)
		{
		case TriggerBuff.BuffType.RECEIVE_DAMAGE:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				buff_host.profile_instance.puppet_property.fixed_dmg_reduce += buff_affect_value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = 0f - (1f + buff_host.profile_instance.puppet_property.dmg_reduce) * value;
				buff_host.profile_instance.puppet_property.dmg_reduce += buff_affect_value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = value;
				buff_host.profile_instance.puppet_property.fixed_dmg_reduce += buff_affect_value;
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (1f + buff_host.profile_instance.puppet_property.dmg_reduce) * value;
				buff_host.profile_instance.puppet_property.dmg_reduce += buff_affect_value;
				break;
			}
			break;
		case TriggerBuff.BuffType.OUTPUT_DAMAGE:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = value;
				buff_host.profile_instance.puppet_property.fixed_dmg_extra += buff_affect_value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = buff_host.profile_instance.puppet_property.dmg_extra * value;
				buff_host.profile_instance.puppet_property.dmg_extra += buff_affect_value;
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				buff_host.profile_instance.puppet_property.fixed_dmg_extra += buff_affect_value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (0f - buff_host.profile_instance.puppet_property.dmg_extra) * value;
				buff_host.profile_instance.puppet_property.dmg_extra += buff_affect_value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			}
			break;
		case TriggerBuff.BuffType.ATTACK_INTERVAL:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = buff_host.profile_instance.puppet_property.attack_interval * value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (0f - buff_host.profile_instance.puppet_property.attack_interval) * value;
				break;
			}
			buff_host.profile_instance.puppet_property.attack_interval += buff_affect_value;
			buff_host.SetCommonCD();
			break;
		case TriggerBuff.BuffType.MOVE_SPEED:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = buff_host.profile_instance.puppet_property.move_speed * value;
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (0f - buff_host.profile_instance.puppet_property.move_speed) * value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			}
			buff_host.profile_instance.puppet_property.move_speed += buff_affect_value;
			buff_host.SetPuppetMovement();
			break;
		case TriggerBuff.BuffType.ARMOR:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = buff_host.profile_instance.puppet_property.armor * value;
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (0f - buff_host.profile_instance.puppet_property.armor) * value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			}
			buff_host.profile_instance.puppet_property.armor += buff_affect_value;
			break;
		case TriggerBuff.BuffType.ATTACK_POWER:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = buff_host.profile_instance.puppet_property.attack_power * value;
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (0f - buff_host.profile_instance.puppet_property.attack_power) * value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			}
			buff_host.profile_instance.puppet_property.attack_power += buff_affect_value;
			break;
		case TriggerBuff.BuffType.MAGICAL_POWER:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
				buff_affect_value = value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
				buff_affect_value = buff_host.profile_instance.puppet_property.magic_power * value;
				break;
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_affect_value = 0f - value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_affect_value = (0f - buff_host.profile_instance.puppet_property.magic_power) * value;
				if (!aureole_buff)
				{
					buff_host.DebuffList.Add(this);
				}
				break;
			}
			buff_host.profile_instance.puppet_property.magic_power += buff_affect_value;
			break;
		case TriggerBuff.BuffType.IMBIBE:
		{
			TriggerBuff.Property buff_property = buff_config.buff_property;
			if (buff_property == TriggerBuff.Property.PLUS_PERCENT_VALUE)
			{
				buff_host.ImbibeBuff = new D3DFloat(value);
			}
			break;
		}
		case TriggerBuff.BuffType.CLOAK:
			break;
		}
	}

	private void RemoveBuffValue()
	{
		switch (buff_config.buff_type)
		{
		case TriggerBuff.BuffType.RECEIVE_DAMAGE:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_host.profile_instance.puppet_property.fixed_dmg_reduce -= buff_affect_value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_host.profile_instance.puppet_property.dmg_reduce -= buff_affect_value;
				break;
			}
			break;
		case TriggerBuff.BuffType.OUTPUT_DAMAGE:
			switch (buff_config.buff_property)
			{
			case TriggerBuff.Property.PLUS_FIXED_VALUE:
			case TriggerBuff.Property.MINUS_FIXED_VALUE:
				buff_host.profile_instance.puppet_property.fixed_dmg_extra -= buff_affect_value;
				break;
			case TriggerBuff.Property.PLUS_PERCENT_VALUE:
			case TriggerBuff.Property.MINUS_PERCENT_VALUE:
				buff_host.profile_instance.puppet_property.dmg_extra -= buff_affect_value;
				break;
			}
			break;
		case TriggerBuff.BuffType.ATTACK_INTERVAL:
			buff_host.profile_instance.puppet_property.attack_interval -= buff_affect_value;
			buff_host.SetCommonCD();
			break;
		case TriggerBuff.BuffType.MOVE_SPEED:
			buff_host.profile_instance.puppet_property.move_speed -= buff_affect_value;
			buff_host.SetPuppetMovement();
			break;
		case TriggerBuff.BuffType.ARMOR:
			buff_host.profile_instance.puppet_property.armor -= buff_affect_value;
			break;
		case TriggerBuff.BuffType.ATTACK_POWER:
			buff_host.profile_instance.puppet_property.attack_power -= buff_affect_value;
			break;
		case TriggerBuff.BuffType.MAGICAL_POWER:
			buff_host.profile_instance.puppet_property.magic_power -= buff_affect_value;
			break;
		case TriggerBuff.BuffType.IMBIBE:
			buff_host.ImbibeBuff = null;
			break;
		case TriggerBuff.BuffType.CLOAK:
			break;
		}
	}
}
