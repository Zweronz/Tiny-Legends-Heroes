using System.Collections;
using UnityEngine;

public class EffectPlayerComponent : MonoBehaviour
{
	public bool Direction;

	public string AwakenSfx;

	public float SfxDelay;

	public bool StopSfxOnEnd;

	private bool one_shot;

	private ParticleSystem[] particle_systems;

	private ParticleEmitter[] particle_emitters;

	private ParticleAnimator[] particle_animators;

	private EffectAnimationComponent[] particle_animations;

	private D3DAudioBehaviour effect_audio;

	private void Awake()
	{
		particle_systems = GetComponentsInChildren<ParticleSystem>();
		particle_emitters = GetComponentsInChildren<ParticleEmitter>();
		particle_animators = GetComponentsInChildren<ParticleAnimator>();
		particle_animations = GetComponentsInChildren<EffectAnimationComponent>();
		ParticleEmitter[] array = particle_emitters;
		foreach (ParticleEmitter particleEmitter in array)
		{
			particleEmitter.enabled = false;
		}
		EffectAnimationComponent[] array2 = particle_animations;
		foreach (EffectAnimationComponent effectAnimationComponent in array2)
		{
			effectAnimationComponent.Init();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!one_shot)
		{
			return;
		}
		EffectAnimationComponent[] array = particle_animations;
		foreach (EffectAnimationComponent effectAnimationComponent in array)
		{
			if (effectAnimationComponent.IsAnimationPlaying())
			{
				return;
			}
		}
		ParticleEmitter[] array2 = particle_emitters;
		foreach (ParticleEmitter particleEmitter in array2)
		{
			if (null != particleEmitter)
			{
				return;
			}
		}
		ParticleSystem[] array3 = particle_systems;
		foreach (ParticleSystem particleSystem in array3)
		{
			if (particleSystem.IsAlive())
			{
				return;
			}
		}
		ParticleAnimator[] array4 = particle_animators;
		foreach (ParticleAnimator particleAnimator in array4)
		{
			if (null != particleAnimator)
			{
				return;
			}
		}
		Object.Destroy(base.gameObject);
		if (null != effect_audio && StopSfxOnEnd)
		{
			effect_audio.Stop();
		}
	}

	public void Play(bool one_shot)
	{
		this.one_shot = one_shot;
		ParticleAnimator[] array = particle_animators;
		foreach (ParticleAnimator particleAnimator in array)
		{
			particleAnimator.autodestruct = one_shot;
		}
		EffectAnimationComponent[] array2 = particle_animations;
		foreach (EffectAnimationComponent effectAnimationComponent in array2)
		{
			effectAnimationComponent.StartPlayAnimation(one_shot);
		}
		ParticleEmitter[] array3 = particle_emitters;
		foreach (ParticleEmitter particleEmitter in array3)
		{
			particleEmitter.enabled = true;
		}
		ParticleSystem[] array4 = particle_systems;
		foreach (ParticleSystem particleSystem in array4)
		{
			particleSystem.Play(false);
			particleSystem.loop = !one_shot;
		}
		if (SfxDelay > 0f)
		{
			StartCoroutine(PlaySfxDelay());
		}
		else
		{
			D3DAudioManager.Instance.PlayAudio(AwakenSfx, ref effect_audio, base.gameObject, false, true);
		}
	}

	public void Stop()
	{
		if (one_shot)
		{
			return;
		}
		ParticleAnimator[] array = particle_animators;
		foreach (ParticleAnimator particleAnimator in array)
		{
			if (null != particleAnimator)
			{
				particleAnimator.autodestruct = true;
			}
		}
		EffectAnimationComponent[] array2 = particle_animations;
		foreach (EffectAnimationComponent effectAnimationComponent in array2)
		{
			if (null != effectAnimationComponent)
			{
				effectAnimationComponent.StopAnimation();
			}
		}
		ParticleEmitter[] array3 = particle_emitters;
		foreach (ParticleEmitter particleEmitter in array3)
		{
			if (null != particleEmitter)
			{
				particleEmitter.emit = false;
			}
		}
		ParticleSystem[] array4 = particle_systems;
		foreach (ParticleSystem particleSystem in array4)
		{
			if (null != particleSystem)
			{
				particleSystem.enableEmission = false;
				particleSystem.Stop(false);
			}
		}
		one_shot = true;
	}

	private IEnumerator PlaySfxDelay()
	{
		yield return new WaitForSeconds(SfxDelay);
		D3DAudioManager.Instance.PlayAudio(AwakenSfx, ref effect_audio, base.gameObject, false, true);
	}
}
