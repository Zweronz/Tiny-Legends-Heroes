using UnityEngine;

public class EffectAnimationComponent : MonoBehaviour
{
	public string StartAnimation;

	public string LoopAnimation;

	public string EndAnimation;

	private bool one_shot;

	private Animation animation_component;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Init()
	{
		animation_component = GetComponent<Animation>();
		if (null == animation_component)
		{
			Object.Destroy(this);
		}
		else
		{
			animation_component.Stop();
		}
	}

	public void StartPlayAnimation(bool one_shot)
	{
		this.one_shot = one_shot;
		animation_component.wrapMode = WrapMode.Once;
		if (string.Empty != StartAnimation && null != animation_component.GetClip(StartAnimation))
		{
			animation_component.Play(StartAnimation);
			if (!one_shot)
			{
				Invoke("PlayLoopAnimation", animation_component[StartAnimation].length);
			}
			else
			{
				Invoke("StopAnimation", animation_component[StartAnimation].length);
			}
		}
		else if (!one_shot)
		{
			PlayLoopAnimation();
		}
		else
		{
			StopAnimation();
		}
	}

	private void PlayLoopAnimation()
	{
		if (string.Empty != LoopAnimation && null != animation_component.GetClip(LoopAnimation))
		{
			animation_component.wrapMode = WrapMode.Loop;
			animation_component.Play(LoopAnimation);
		}
	}

	public void StopAnimation()
	{
		if (IsInvoking("PlayLoopAnimation"))
		{
			CancelInvoke("PlayLoopAnimation");
		}
		if (one_shot)
		{
			if (!IsInvoking("StopAnimation"))
			{
				return;
			}
			CancelInvoke("StopAnimation");
		}
		animation_component.Stop();
		if (string.Empty != EndAnimation && null != animation_component.GetClip(EndAnimation))
		{
			animation_component.wrapMode = WrapMode.Once;
			animation_component.CrossFade(EndAnimation);
		}
	}

	public bool IsAnimationPlaying()
	{
		return animation_component.isPlaying;
	}
}
