using System.Collections.Generic;
using UnityEngine;

public class EffectTester : MonoBehaviour
{
	public BasicEffectComponent Effect;

	public string MountPoint;

	public Vector3 Offset;

	public bool Bind;

	public Vector2 Range = Vector2.one;

	private List<BasicEffectComponent> LoopEffects;

	private void Start()
	{
		LoopEffects = new List<BasicEffectComponent>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Work(true);
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			Work(false);
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			foreach (BasicEffectComponent loopEffect in LoopEffects)
			{
				loopEffect.Stop();
			}
			LoopEffects.Clear();
		}
		else if (Input.GetKeyDown(KeyCode.T))
		{
			Vector2 randomPointOnLine = D3DPlaneGeometry.GetRandomPointOnLine(new Vector2(-8f, 3f), new Vector2(1f, 7f));
			base.gameObject.transform.position = new Vector3(randomPointOnLine.x, 0f, randomPointOnLine.y);
		}
	}

	private void Work(bool one_shot)
	{
		GameObject gameObject = base.gameObject;
		if (string.Empty != MountPoint)
		{
			GameObject gameObject2 = D3DMain.Instance.FindGameObjectChild(base.gameObject, MountPoint);
			if (null != gameObject2)
			{
				gameObject = gameObject2;
			}
		}
		BasicEffectComponent basicEffectComponent = (BasicEffectComponent)Object.Instantiate(Effect, gameObject.transform.position + gameObject.transform.rotation * Offset, gameObject.transform.rotation);
		if (!Bind)
		{
			gameObject = null;
		}
		basicEffectComponent.Play(one_shot, gameObject, base.gameObject, Offset, Range, 0f);
		if (!one_shot)
		{
			LoopEffects.Add(basicEffectComponent);
		}
	}
}
