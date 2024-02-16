using System.Collections.Generic;
using UnityEngine;

public class PuppetModelBuilder : MonoBehaviour
{
	public enum ClipIndex
	{
		RANDOM_ATK = -2,
		NONE = -1,
		IDLE1 = 0,
		IDLE2 = 1,
		MAP_IDLE = 2,
		MOVE1 = 3,
		MOVE2 = 4,
		MAP_MOVE = 5,
		ATTACK1 = 6,
		ATTACK2 = 7,
		HURT = 8,
		DEAD = 9,
		UPGRADE = 10,
		CAST = 11,
		SKILL0 = 12,
		SKILL1 = 13,
		SKILL2 = 14,
		SKILL3 = 15,
		SKILL4 = 16,
		SKILL5 = 17,
		SKILL6 = 18,
		SKILL7 = 19,
		SKILL8 = 20,
		SKILL9 = 21,
		SKILL10 = 22,
		SKILL11 = 23,
		SKILL12 = 24,
		SKILL13 = 25,
		SKILL14 = 26,
		SKILL15 = 27
	}

	public enum ModelParts
	{
		BODY = 0,
		HEAD = 1,
		HELM = 2,
		LEFT_HAND = 3,
		RIGHT_HAND = 4
	}

	protected PuppetBasic puppet_component;

	protected Animation animation_component;

	protected List<string> animation_clip_names;

	protected GameObject[] model_parts;

	protected D3DPuppetTransformCfg puppet_transform_cfg;

	protected int changeable_idle_clip;

	protected int changeable_move_clip;

	protected int changeable_attack_clip;

	protected int current_clip;

	protected string current_animation;

	public PuppetAnimationEvent puppetAnimationEvent;

	public PuppetBasic PuppetComponent
	{
		set
		{
			puppet_component = value;
		}
	}

	public Animation AnimationComponent
	{
		set
		{
			animation_component = value;
		}
	}

	public D3DPuppetTransformCfg TransformCfg
	{
		get
		{
			return puppet_transform_cfg;
		}
	}

	public int ChangeableIdleClip
	{
		get
		{
			return changeable_idle_clip;
		}
		set
		{
			changeable_idle_clip = value;
		}
	}

	public int ChangeableMoveClip
	{
		get
		{
			return changeable_move_clip;
		}
		set
		{
			changeable_move_clip = value;
		}
	}

	public int ChangeableAttackClip
	{
		get
		{
			return changeable_attack_clip;
		}
		set
		{
			changeable_attack_clip = value;
		}
	}

	public int CurrentClip
	{
		get
		{
			return current_clip;
		}
	}

	protected void Awake()
	{
		puppet_component = null;
		animation_component = null;
		animation_clip_names = new List<string>();
		model_parts = new GameObject[5];
		current_clip = 2;
		current_animation = string.Empty;
		changeable_idle_clip = 0;
		changeable_move_clip = 3;
		changeable_attack_clip = 6;
	}

	protected void Start()
	{
	}

	protected void Update()
	{
	}

	public virtual void BuildPuppetModel()
	{
	}

	public virtual void BuildPuppetFaceFeatureModel()
	{
	}

	public virtual void CheckHelmHide()
	{
	}

	public void AddAnimationEvent()
	{
		puppetAnimationEvent = model_parts[0].AddComponent<PuppetAnimationEvent>();
		puppetAnimationEvent.SetParentPuppetScript(GetComponent<PuppetArena>());
	}

	public void AddAudioController(bool use_event)
	{
		TAudioController tAudioController = model_parts[0].AddComponent<TAudioController>();
		tAudioController.useAuidoEvent = use_event;
	}

	public virtual void ChangeModelParts(D3DPuppetProfile.PuppetArms arm_part)
	{
	}

	public virtual void ChangeFaceFeatureModel(D3DPuppetProfile.PuppetArms arm_part)
	{
	}

	public virtual void RemoveModelParts(D3DPuppetProfile.PuppetArms arm_part, bool bUseDefaultPart = true)
	{
	}

	public virtual void RemoveFaceFeatureModelParts(D3DPuppetProfile.PuppetArms arm_part)
	{
	}

	public virtual void ChangeAnimation(string animation)
	{
	}

	public GameObject GetModelParts(int parts_slot)
	{
		if (parts_slot > 4)
		{
			return null;
		}
		return model_parts[parts_slot];
	}

	public void SetPartsVisible(int parts_slot, bool visible)
	{
		if (parts_slot <= 4)
		{
			SkinnedMeshRenderer componentInChildren = model_parts[parts_slot].GetComponentInChildren<SkinnedMeshRenderer>();
			if (!(null == componentInChildren))
			{
				componentInChildren.gameObject.SetActiveRecursively(visible);
			}
		}
	}

	protected void GetAnimationComponent()
	{
		if (null != model_parts[0])
		{
			animation_component = model_parts[0].GetComponent<Animation>();
			if (null == animation_component)
			{
				animation_component = model_parts[0].AddComponent<Animation>();
			}
			animation_component.playAutomatically = false;
		}
	}

	public bool PlayPuppetAnimations(bool cross_fade, int clip_index, WrapMode wrapMode = WrapMode.Once, bool rewind = false, float fade_length = 0.1f, float start_time = 0f)
	{
		if (null == animation_component)
		{
			return false;
		}
		if (clip_index < 0)
		{
			return false;
		}
		string text = animation_clip_names[clip_index];
		if (string.Empty == text)
		{
			return false;
		}
		if (null == animation_component.GetClip(text))
		{
			return false;
		}
		if (clip_index == current_clip && !rewind)
		{
			return true;
		}
		current_clip = clip_index;
		if (rewind)
		{
			RewindClip(current_clip);
		}
		animation_component[text].wrapMode = wrapMode;
		animation_component[text].time = start_time * animation_component[text].length;
		if (cross_fade)
		{
			animation_component.CrossFade(text, fade_length);
		}
		else
		{
			animation_component.Play(text);
		}
		return true;
	}

	public void PlayFeatureAnimation()
	{
		if (puppet_component.profile_instance.PuppetType == D3DPuppetProfile.ProfileType.AVATAR)
		{
			animation_component.wrapMode = WrapMode.Loop;
			animation_component["idle_icon"].time = Random.Range(0f, 2f) * animation_component["idle_icon"].length;
			animation_component.Play("idle_icon");
		}
		else
		{
			PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, Random.Range(0f, 2f));
		}
	}

	protected void InitClipsLayer()
	{
		for (ClipIndex clipIndex = ClipIndex.IDLE1; clipIndex <= ClipIndex.SKILL9; clipIndex++)
		{
			if (!(string.Empty == animation_clip_names[(int)clipIndex]))
			{
				int layer = 0;
				if (clipIndex != 0 && clipIndex != ClipIndex.IDLE2 && clipIndex != ClipIndex.MAP_IDLE)
				{
					layer = 1;
				}
				animation_component[animation_clip_names[(int)clipIndex]].layer = layer;
			}
		}
	}

	public void StopCurrentClip()
	{
		if (!(null == animation_component))
		{
			animation_component.Stop();
		}
	}

	public void SetAllClipSpeed(float speed_scale)
	{
		if (null == animation_component)
		{
			return;
		}
		foreach (string animation_clip_name in animation_clip_names)
		{
			if (!(string.Empty == animation_clip_name))
			{
				animation_component[animation_clip_name].speed = speed_scale;
			}
		}
	}

	public void SetClipSpeed(int clip_index, float speed_scale)
	{
		if (!(null == animation_component) && clip_index >= 0)
		{
			string text = animation_clip_names[clip_index];
			if (!(string.Empty == text))
			{
				animation_component[text].speed = speed_scale;
			}
		}
	}

	public void SetCurrentClipSpeed(float speed_scale)
	{
		if (!(null == animation_component))
		{
			string text = animation_clip_names[current_clip];
			if (!(string.Empty == text))
			{
				animation_component[text].speed = speed_scale;
			}
		}
	}

	public float GetCurrentClipPlayedTime()
	{
		return animation_component[animation_clip_names[current_clip]].time;
	}

	public WrapMode GetCurrentClipWarpMode()
	{
		return animation_component[animation_clip_names[current_clip]].wrapMode;
	}

	public float GetClipLength(int clip_index)
	{
		return animation_component[animation_clip_names[clip_index]].length;
	}

	public float GetCurrentClipSpeed()
	{
		return animation_component[animation_clip_names[current_clip]].speed;
	}

	public bool IsPlayingOutOfLength(int clip_index, float scale)
	{
		if (null == animation_component)
		{
			return false;
		}
		if (clip_index < 0)
		{
			return false;
		}
		string text = animation_clip_names[clip_index];
		if (string.Empty == text)
		{
			return false;
		}
		if (animation_component[text].time >= animation_component[text].length * scale)
		{
			return true;
		}
		return false;
	}

	public void RewindClip(int clip_index)
	{
		if (!(null == animation_component) && clip_index >= 0)
		{
			string text = animation_clip_names[clip_index];
			if (!(string.Empty == text))
			{
				animation_component.Rewind(text);
			}
		}
	}

	public void RewindClipIfIsPlaying(int clip_index)
	{
		if (!(null == animation_component) && clip_index >= 0)
		{
			string text = animation_clip_names[clip_index];
			if (!(string.Empty == text) && animation_component.IsPlaying(text))
			{
				animation_component.Rewind(text);
			}
		}
	}

	public bool IsClipPlaying(int clip_index)
	{
		if (null == animation_component)
		{
			return false;
		}
		if (clip_index < 0)
		{
			return false;
		}
		string text = animation_clip_names[clip_index];
		if (string.Empty == text)
		{
			return false;
		}
		return animation_component.IsPlaying(text);
	}

	public void BlendHurtAnimation()
	{
		if (current_clip != 9 && (current_clip < 12 || current_clip > 27) && GetCurrentClipSpeed() != 0f && !(null == animation_component))
		{
			string text = animation_clip_names[8];
			if (!(string.Empty == text) && !(null == animation_component.GetClip(text)))
			{
				animation_component.Blend(text, 3f, 0.1f);
			}
		}
	}
}
