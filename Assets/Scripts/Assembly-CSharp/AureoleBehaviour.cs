using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AureoleBehaviour : MonoBehaviour
{
	private PuppetArena aureole_caster;

	private PuppetArena aureole_host;

	private SceneArena scene_arena;

	private AureoleConfig aureole_config;

	private TriggerVariable.VariableType dot_type;

	private VariableOutputData dot_output_data;

	private TriggerVariable.DotConfig dot_config;

	private TriggerBuff buff_config;

	private int skill_level;

	private float aureole_radius;

	private BasicEffectComponent aureole_effect;

	private D3DAudioBehaviour aureole_sfx;

	private List<PuppetArena> current_aureole_targets = new List<PuppetArena>();

	private Dictionary<PuppetArena, DotVariable> aureole_dots = new Dictionary<PuppetArena, DotVariable>();

	private Dictionary<PuppetArena, Buff> aureole_buffs = new Dictionary<PuppetArena, Buff>();

	private void Start()
	{
	}

	private void Update()
	{
		if (aureole_config.aureole_origin != AureoleConfig.AureoleOrigin.TRIGGER_POINT && aureole_config.bind)
		{
			if (null == aureole_host || aureole_host.IsDead())
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				base.transform.position = aureole_host.transform.position;
			}
		}
	}

	private void OnDestroy()
	{
		if (null != aureole_effect)
		{
			aureole_effect.Stop();
		}
		if (null != aureole_sfx)
		{
			aureole_sfx.Stop();
		}
		foreach (DotVariable value in aureole_dots.Values)
		{
			if (null != value)
			{
				Object.Destroy(value.gameObject);
			}
		}
		foreach (Buff value2 in aureole_buffs.Values)
		{
			if (null != value2)
			{
				Object.Destroy(value2.gameObject);
			}
		}
		scene_arena.AureoleManager.Remove(this);
	}

	public void InitAureole(PuppetArena aureole_caster, PuppetArena aureole_host, Vector3 target_point, SceneArena scene_arena, AureoleConfig aureole_config, int skill_level, TriggerVariable.DotConfig dot_config, TriggerVariable.VariableType dot_type, VariableOutputData dot_output_data, TriggerBuff buff_config)
	{
		if (null == scene_arena || aureole_config == null || (dot_config == null && buff_config == null))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.aureole_caster = aureole_caster;
		this.aureole_host = null;
		this.scene_arena = scene_arena;
		this.aureole_config = aureole_config;
		this.dot_type = dot_type;
		this.dot_output_data = dot_output_data;
		this.dot_config = dot_config;
		this.buff_config = buff_config;
		this.skill_level = skill_level;
		if ("Player" == this.aureole_caster.tag)
		{
			base.tag = "TriggerPlayer";
		}
		else if ("Enemy" == this.aureole_caster.tag)
		{
			base.tag = "TriggerEnemy";
		}
		if (this.aureole_config.aureole_origin == AureoleConfig.AureoleOrigin.TRIGGER_POINT)
		{
			base.transform.position = target_point;
			aureole_effect = BasicEffectComponent.PlayEffect(this.aureole_config.aureole_effect, base.transform.position, Quaternion.identity, Vector2.one, Vector3.zero, false, 0f);
		}
		else if (this.aureole_config.aureole_origin == AureoleConfig.AureoleOrigin.CASTER)
		{
			base.transform.position = this.aureole_caster.transform.position;
			this.aureole_host = this.aureole_caster;
			aureole_effect = BasicEffectComponent.PlayEffect(this.aureole_config.aureole_effect, this.aureole_host.gameObject, this.aureole_config.mount_point, this.aureole_config.bind, Vector2.one, Vector3.zero, false, 0f);
		}
		else if (this.aureole_config.aureole_origin == AureoleConfig.AureoleOrigin.DEFAULT_TARGET)
		{
			this.aureole_host = aureole_host;
			base.transform.position = this.aureole_host.transform.position;
			aureole_effect = BasicEffectComponent.PlayEffect(this.aureole_config.aureole_effect, this.aureole_host.gameObject, this.aureole_config.mount_point, this.aureole_config.bind, Vector2.one, Vector3.zero, false, 0f);
		}
		GameObject audio_player = ((!(null == aureole_effect)) ? aureole_effect.gameObject : null);
		D3DAudioManager.Instance.PlayAudio(this.aureole_config.aureole_sfx, ref aureole_sfx, audio_player, false, true);
		aureole_radius = this.aureole_config.AureoleRadius(this.skill_level);
		if (null != this.aureole_host && this.aureole_config.include_puppet_radius)
		{
			aureole_radius += this.aureole_host.ControllerRadius;
		}
		float num = this.aureole_config.AureoleTime(this.skill_level);
		if (num >= 0f)
		{
			StartCoroutine(ExpireAureole(num));
		}
		StartCoroutine(AureoleAffect());
	}

	private IEnumerator AureoleAffect()
	{
		while (true)
		{
			FilterAureoleTargets();
			yield return new WaitForSeconds(0.15f);
		}
	}

	private IEnumerator ExpireAureole(float aureole_time)
	{
		yield return new WaitForSeconds(aureole_time);
		Object.Destroy(base.gameObject);
	}

	private void FilterAureoleTargets()
	{
		int num = 0;
		while (num < current_aureole_targets.Count)
		{
			if (null == current_aureole_targets[num] || current_aureole_targets[num].IsDead())
			{
				current_aureole_targets.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		List<PuppetArena> list = new List<PuppetArena>();
		if (aureole_config.aureole_faction == AureoleConfig.AureoleFaction.ALL || aureole_config.aureole_faction == AureoleConfig.AureoleFaction.ALL_EXCLUDE_ME)
		{
			list.AddRange(scene_arena.playerList);
			list.AddRange(scene_arena.enemyList);
			if (aureole_config.aureole_faction == AureoleConfig.AureoleFaction.ALL_EXCLUDE_ME && null != aureole_caster && list.Contains(aureole_caster))
			{
				list.Remove(aureole_caster);
			}
		}
		else if (aureole_config.aureole_faction == AureoleConfig.AureoleFaction.ENEMY)
		{
			if ("TriggerPlayer" == base.tag)
			{
				list.AddRange(scene_arena.enemyList);
			}
			else if ("TriggerEnemy" == base.tag)
			{
				list.AddRange(scene_arena.playerList);
			}
		}
		else if (aureole_config.aureole_faction == AureoleConfig.AureoleFaction.FRIEND || aureole_config.aureole_faction == AureoleConfig.AureoleFaction.FRIEND_EXCLUDE_ME)
		{
			if ("TriggerPlayer" == base.tag)
			{
				list.AddRange(scene_arena.playerList);
			}
			else if ("TriggerEnemy" == base.tag)
			{
				list.AddRange(scene_arena.enemyList);
			}
			if (aureole_config.aureole_faction == AureoleConfig.AureoleFaction.FRIEND_EXCLUDE_ME && null != aureole_caster && list.Contains(aureole_caster))
			{
				list.Remove(aureole_caster);
			}
		}
		foreach (PuppetArena item in list)
		{
			if (null == item || item.IsDead() || item.IsInGrave())
			{
				continue;
			}
			if (D3DPlaneGeometry.CircleIntersectCircle(new Vector2(item.transform.position.x, item.transform.position.z), item.ControllerRadius, new Vector2(base.transform.position.x, base.transform.position.z), aureole_radius))
			{
				if (!current_aureole_targets.Contains(item))
				{
					if (dot_config != null)
					{
						GameObject gameObject = new GameObject("DotVariable");
						DotVariable dotVariable = gameObject.AddComponent<DotVariable>();
						dotVariable.InitDotVariableByAureole(item, dot_config, dot_type, dot_output_data, skill_level);
						aureole_dots.Add(item, dotVariable);
					}
					if (buff_config != null)
					{
						GameObject gameObject2 = new GameObject("Buff");
						Buff buff = gameObject2.AddComponent<Buff>();
						buff.InitBuffByAureloe(item, buff_config, skill_level);
						aureole_buffs.Add(item, buff);
					}
					current_aureole_targets.Add(item);
				}
			}
			else if (current_aureole_targets.Contains(item))
			{
				if (aureole_dots.ContainsKey(item))
				{
					Object.Destroy(aureole_dots[item].gameObject);
					aureole_dots.Remove(item);
				}
				if (aureole_buffs.ContainsKey(item))
				{
					Object.Destroy(aureole_buffs[item].gameObject);
					aureole_buffs.Remove(item);
				}
				current_aureole_targets.Remove(item);
			}
		}
	}

	public void RemoveAffectTarget(PuppetArena puppet)
	{
		if (aureole_dots.ContainsKey(puppet))
		{
			Object.Destroy(aureole_dots[puppet].gameObject);
			aureole_dots.Remove(puppet);
		}
		if (aureole_buffs.ContainsKey(puppet))
		{
			Object.Destroy(aureole_buffs[puppet].gameObject);
			aureole_buffs.Remove(puppet);
		}
	}

	public bool AureoleExisting(PuppetArena check_caster, AureoleConfig check_config)
	{
		if (check_caster == aureole_caster && check_config == aureole_config)
		{
			return true;
		}
		return false;
	}
}
