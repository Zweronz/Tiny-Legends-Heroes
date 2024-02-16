using System.Collections.Generic;
using UnityEngine;

public class CombinedEffectComponent : BasicEffectComponent
{
	public List<LinkableEffectComponent> LinkedEffects;

	private LinkableEffectComponent CurrentLinkEffect;

	private bool started;

	private bool stopped;

	private Vector2 range;

	private void Start()
	{
	}

	private void Update()
	{
		if (null == CurrentLinkEffect && started)
		{
			if (LinkedEffects.Count <= 0 || stopped)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PlayLinkEffect();
		}
		if (null != effect_host)
		{
			base.transform.rotation = rotation_host.transform.rotation;
			base.transform.position = effect_host.transform.position + base.transform.rotation * effect_offset;
		}
	}

	public override void Play(bool one_shot, GameObject effect_host, GameObject rotation_host, Vector3 effect_offset, Vector2 range, float delay)
	{
		base.one_shot = one_shot;
		base.effect_host = effect_host;
		base.rotation_host = rotation_host;
		base.effect_offset = effect_offset;
		this.range = range;
		Invoke("PlayLinkEffect", delay);
	}

	private void PlayLinkEffect()
	{
		started = true;
		CurrentLinkEffect = LinkedEffects[0];
		LinkedEffects.RemoveAt(0);
		CurrentLinkEffect = (LinkableEffectComponent)Object.Instantiate(CurrentLinkEffect, base.transform.position, base.transform.rotation);
		if (!one_shot && LinkedEffects.Count == 0)
		{
			CurrentLinkEffect.Play(false, effect_host, rotation_host, effect_offset, range, 0f);
		}
		else
		{
			CurrentLinkEffect.Play(true, effect_host, rotation_host, effect_offset, range, 0f);
		}
	}

	public override void Stop()
	{
		if (IsInvoking("PlayLinkEffect"))
		{
			CancelInvoke("PlayLinkEffect");
			started = true;
		}
		stopped = true;
		if (null != CurrentLinkEffect)
		{
			CurrentLinkEffect.Stop();
		}
	}
}
