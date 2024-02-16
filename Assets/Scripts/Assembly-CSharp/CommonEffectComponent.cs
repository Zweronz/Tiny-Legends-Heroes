using System.Collections.Generic;
using UnityEngine;

public class CommonEffectComponent : LinkableEffectComponent
{
	public List<EffectPlayerComponent> EffectPlayerList;

	private EffectPlayerComponent CurrentEffectPlayer;

	private bool started;

	private bool stopped;

	private void Start()
	{
	}

	private void Update()
	{
		if (null == CurrentEffectPlayer && started)
		{
			if (EffectPlayerList.Count <= 0 || stopped)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PlayListEffect();
		}
		if (null != effect_host)
		{
			base.transform.rotation = rotation_host.transform.rotation;
			if (!CurrentEffectPlayer.Direction)
			{
				CurrentEffectPlayer.transform.rotation = Quaternion.identity;
			}
			base.transform.position = effect_host.transform.position + base.transform.rotation * effect_offset;
		}
	}

	public override void Play(bool one_shot, GameObject effect_host, GameObject rotation_host, Vector3 effect_offset, Vector2 range, float delay)
	{
		base.one_shot = one_shot;
		base.effect_host = effect_host;
		base.rotation_host = rotation_host;
		base.effect_offset = effect_offset;
		Invoke("PlayListEffect", delay);
	}

	private void PlayListEffect()
	{
		started = true;
		CurrentEffectPlayer = EffectPlayerList[0];
		EffectPlayerList.RemoveAt(0);
		Quaternion rotation = Quaternion.identity;
		if (CurrentEffectPlayer.Direction)
		{
			rotation = base.transform.rotation;
		}
		CurrentEffectPlayer = (EffectPlayerComponent)Object.Instantiate(CurrentEffectPlayer, base.transform.position, rotation);
		CurrentEffectPlayer.transform.parent = base.transform;
		CurrentEffectPlayer.transform.localPosition = Vector3.zero;
		if (!CurrentEffectPlayer.Direction)
		{
			CurrentEffectPlayer.transform.rotation = Quaternion.identity;
		}
		if (!one_shot && EffectPlayerList.Count == 0)
		{
			CurrentEffectPlayer.Play(false);
		}
		else
		{
			CurrentEffectPlayer.Play(true);
		}
	}

	public override void Stop()
	{
		if (IsInvoking("PlayListEffect"))
		{
			CancelInvoke("PlayListEffect");
			started = true;
		}
		stopped = true;
		if (null != CurrentEffectPlayer)
		{
			CurrentEffectPlayer.Stop();
		}
	}
}
