using System.Collections.Generic;
using UnityEngine;

public class D3DUIImageAnimation : D3DCustomUI
{
	public delegate void OnAnimationEnd();

	private List<D3DImageCell> animation_cells;

	private int animation_frame_index;

	private float frame_time;

	private bool loop;

	private UIImage animation_control;

	private float animation_rate;

	private UIAnimationBehaviour animation_behaviour;

	private OnAnimationEnd onAnimationEnd;

	public UIImage AnimationControl
	{
		get
		{
			return animation_control;
		}
	}

	public List<D3DImageCell> Cells
	{
		set
		{
			animation_cells = value;
		}
	}

	public float Rate
	{
		set
		{
			animation_rate = value;
		}
	}

	public bool Loop
	{
		set
		{
			loop = value;
		}
	}

	public OnAnimationEnd AnimationEndCallBack
	{
		set
		{
			onAnimationEnd = value;
		}
	}

	public D3DUIImageAnimation(UIManager manager, UIHelper helper, Rect ui_rect)
		: base(manager, helper)
	{
		GameObject gameObject = new GameObject("UIAnimationBehaviour");
		animation_behaviour = gameObject.AddComponent<UIAnimationBehaviour>();
		animation_behaviour.UIImageAnimation = this;
		gameObject.transform.parent = ui_manager.transform;
		animation_behaviour.enabled = false;
		animation_control = new UIImage();
		animation_control.Enable = false;
		animation_control.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_rect);
		ui_manager.Add(animation_control);
	}

	public void Show(bool bShow)
	{
		animation_control.Visible = bShow;
		animation_control.Enable = bShow;
	}

	public void SetRect(Rect ui_rect)
	{
		animation_control.Rect = ui_rect;
	}

	public void InitAnimation(List<D3DImageCell> cells, float rate, bool loop, OnAnimationEnd onAnimationEndDelegate)
	{
		animation_cells = cells;
		animation_rate = rate;
		this.loop = loop;
		onAnimationEnd = onAnimationEndDelegate;
		animation_frame_index = 0;
		animation_control.SetTexture(ui_helper.LoadUIMaterialAutoHD(animation_cells[animation_frame_index].cell_texture), D3DMain.Instance.ConvertRectAutoHD(animation_cells[animation_frame_index].cell_rect));
	}

	public void SetAnimation()
	{
		animation_frame_index = 0;
		frame_time = 0f;
		animation_control.SetTexture(ui_helper.LoadUIMaterialAutoHD(animation_cells[animation_frame_index].cell_texture), D3DMain.Instance.ConvertRectAutoHD(animation_cells[animation_frame_index].cell_rect));
	}

	public void SetAnimationReverse()
	{
		animation_cells.Reverse();
		SetAnimation();
	}

	public void Play()
	{
		animation_behaviour.enabled = true;
		animation_behaviour.StartRealTime();
	}

	public void Stop()
	{
		animation_behaviour.enabled = false;
	}

	public void AnimationPlaying(float delta_time)
	{
		frame_time += delta_time;
		if (!(frame_time > 1f / animation_rate))
		{
			return;
		}
		frame_time = 0f;
		animation_frame_index++;
		if (animation_frame_index >= animation_cells.Count)
		{
			if (!loop)
			{
				animation_behaviour.enabled = false;
				if (onAnimationEnd != null)
				{
					onAnimationEnd();
				}
				return;
			}
			animation_frame_index = 0;
		}
		animation_control.SetTexture(ui_helper.LoadUIMaterialAutoHD(animation_cells[animation_frame_index].cell_texture), D3DMain.Instance.ConvertRectAutoHD(animation_cells[animation_frame_index].cell_rect));
	}

	public bool IsPlaying()
	{
		return animation_behaviour.enabled;
	}
}
