using System.Collections;
using UnityEngine;

public class DotVariable : MonoBehaviour
{
	private PuppetArena dot_host;

	private TriggerVariable.DotConfig dot_config;

	private int skill_level;

	private BasicEffectComponent dot_remain_effect;

	private D3DAudioBehaviour dot_sfx;

	private VariableOutputData dot_output_data;

	private VariableOutputData dot_extra_output_data;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (null == dot_host || dot_host.IsDead() || dot_host.scene_arena.IsBattleWinBehaviour)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (null != dot_remain_effect)
		{
			dot_remain_effect.Stop();
		}
		if (null != dot_sfx)
		{
			dot_sfx.Stop();
		}
		if (null != dot_host && dot_host.BadDotList.Contains(this))
		{
			dot_host.BadDotList.Remove(this);
		}
	}

	public void InitDotVariable(PuppetArena dot_host, TriggerVariable.DotConfig dot_config, TriggerVariable.VariableType dot_type, VariableOutputData dot_output_data, VariableOutputData dot_extra_output_data, int skill_level)
	{
		if (null == dot_host || dot_config == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.dot_host = dot_host;
		this.dot_config = dot_config;
		this.skill_level = skill_level;
		this.dot_output_data = dot_output_data;
		this.dot_output_data.variable_type = dot_type;
		if (dot_extra_output_data != null)
		{
			this.dot_extra_output_data = dot_extra_output_data;
			this.dot_extra_output_data.variable_type = dot_type;
		}
		float num = dot_config.DotTime(this.skill_level);
		if (num >= 0f)
		{
			StartCoroutine(ExpireDot(num));
		}
		if (dot_config.remain_effect)
		{
			dot_remain_effect = BasicEffectComponent.PlayEffect(dot_config.dot_effect, dot_host.gameObject, dot_config.mount_point, true, Vector2.one, Vector3.zero, false, 0f);
			D3DAudioManager.Instance.PlayAudio(dot_config.dot_sfx, ref dot_sfx, dot_host.gameObject, false, true);
		}
		if (dot_type == TriggerVariable.VariableType.HP_DAMAGE || dot_type == TriggerVariable.VariableType.MP_DAMAGE)
		{
			dot_host.BadDotList.Add(this);
		}
		StartCoroutine(DotAffect());
	}

	public void InitDotVariableByAureole(PuppetArena dot_host, TriggerVariable.DotConfig dot_config, TriggerVariable.VariableType dot_type, VariableOutputData dot_output_data, int skill_level)
	{
		if (null == dot_host || dot_config == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.dot_host = dot_host;
		this.dot_config = dot_config;
		this.skill_level = skill_level;
		this.dot_output_data = dot_output_data;
		this.dot_output_data.variable_type = dot_type;
		if (dot_config.remain_effect)
		{
			dot_remain_effect = BasicEffectComponent.PlayEffect(dot_config.dot_effect, dot_host.gameObject, dot_config.mount_point, true, Vector2.one, Vector3.zero, false, 0f);
			D3DAudioManager.Instance.PlayAudio(dot_config.dot_sfx, ref dot_sfx, dot_host.gameObject, false, true);
		}
		StartCoroutine(DotAffect());
	}

	private IEnumerator ExpireDot(float dot_time)
	{
		yield return new WaitForSeconds(dot_time);
		if (dot_config.extra_variable != null)
		{
			BasicEffectComponent.PlayEffect(dot_config.extra_variable.extra_effect, dot_host.gameObject, dot_config.extra_variable.mount_point, true, Vector2.one, Vector3.zero, true, 0f);
			D3DAudioManager.Instance.PlayAudio(dot_config.extra_variable.extra_sfx, dot_host.gameObject, false, true);
			dot_host.Variable(dot_extra_output_data);
		}
		Object.Destroy(base.gameObject);
	}

	private IEnumerator DotAffect()
	{
		float dot_interval = dot_config.DotInterval(skill_level);
		while (true)
		{
			if (!dot_config.remain_effect)
			{
				BasicEffectComponent.PlayEffect(dot_config.dot_effect, dot_host.gameObject, dot_config.mount_point, true, Vector2.one, Vector3.zero, true, 0f);
				D3DAudioManager.Instance.PlayAudio(dot_config.dot_sfx, dot_host.gameObject, false, true);
			}
			dot_host.Variable(dot_output_data);
			yield return new WaitForSeconds(dot_interval);
		}
	}
}
