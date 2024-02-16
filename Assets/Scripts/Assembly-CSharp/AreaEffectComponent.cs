using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectComponent : LinkableEffectComponent
{
	public float Rate;

	public List<CommonEffectComponent> Effects;

	public float OneShotRemainsTime;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
	}

	private void Start()
	{
	}

	private void Update()
	{
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
		base.transform.localScale = new Vector3(range.x, 0f, range.y);
		Invoke("StartPlay", delay);
	}

	private void StartPlay()
	{
		if (one_shot)
		{
			Invoke("Stop", OneShotRemainsTime);
		}
		if (Effects.Count > 0)
		{
			StartCoroutine("RandomAreaEffect");
		}
	}

	private IEnumerator RandomAreaEffect()
	{
		while (true)
		{
			Vector3 scale = base.transform.localScale * 0.5f;
			Vector3 offset = new Vector3(Random.Range(0f - scale.x, scale.x), 0f, Random.Range(0f - scale.z, scale.z));
			Vector3 random_position = base.transform.position + base.transform.rotation * offset;
			CommonEffectComponent effect2 = Effects[Random.Range(0, Effects.Count)];
			effect2 = (CommonEffectComponent)Object.Instantiate(effect2, random_position, base.transform.rotation);
			effect2.Play(true, null, null, Vector3.zero, Vector2.one, 0f);
			yield return new WaitForSeconds(Rate);
		}
	}

	public override void Stop()
	{
		if (IsInvoking("Stop"))
		{
			CancelInvoke("Stop");
		}
		StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}
}
