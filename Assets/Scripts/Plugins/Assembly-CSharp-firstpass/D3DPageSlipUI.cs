using System.Collections.Generic;
using UnityEngine;

public class D3DPageSlipUI : D3DCustomUI
{
	protected Vector2 page_size;

	protected float page_center_x;

	protected float page_slip_x;

	protected int current_page_index;

	protected int next_page_index;

	protected int page_moved_delta;

	protected bool page_move_left;

	protected bool page_quick_slip;

	protected List<UIImage> page_dots;

	protected List<UIImage> page_dots_cache;

	protected D3DImageCell[] page_dot_image_cell;

	protected Vector2 dot_center;

	protected PageSlipBehaviour page_slip_behaviour;

	protected bool click_event;

	protected bool enable_page_auto_slip;

	protected float page_auto_slip_delta;

	protected float page_auto_slip_target;

	protected int page_auto_slip_target_index;

	public int CurrentPageIndex
	{
		get
		{
			return current_page_index;
		}
	}

	public int NextPageIndex
	{
		get
		{
			return next_page_index;
		}
	}

	public int PageCount
	{
		get
		{
			return page_dots.Count;
		}
	}

	public float NextPageX
	{
		get
		{
			if (next_page_index < current_page_index)
			{
				return 0f - page_size.x;
			}
			return page_size.x;
		}
	}

	public D3DPageSlipUI(UIManager manager, UIHelper helper, int page_count, Rect page_rect, float page_dot_y)
		: base(manager, helper)
	{
		ui_position = new Vector2(page_rect.x, page_rect.y) * D3DMain.Instance.HD_SIZE;
		page_size = new Vector2(page_rect.width, page_rect.height) * D3DMain.Instance.HD_SIZE;
		page_dot_image_cell = new D3DImageCell[2];
		page_dot_image_cell[0] = ui_helper.GetImageCell("team-fanye1");
		page_dot_image_cell[1] = ui_helper.GetImageCell("team-fanye2");
		page_center_x = ui_position.x + page_size.x * 0.5f;
		page_slip_x = page_center_x;
		dot_center = new Vector2(page_center_x, page_dot_y * (float)D3DMain.Instance.HD_SIZE);
		page_dots = new List<UIImage>();
		page_dots_cache = new List<UIImage>();
		current_page_index = 0;
		RegroupPageDot(page_count);
		GameObject gameObject = new GameObject("PageSlipBehaviour");
		gameObject.transform.parent = ui_manager.transform;
		page_slip_behaviour = gameObject.AddComponent<PageSlipBehaviour>();
		page_slip_behaviour.PageSlipUI = this;
		click_event = false;
		enable_page_auto_slip = false;
	}

	protected UIImage GetPageDot()
	{
		UIImage uIImage;
		if (page_dots_cache.Count > 0)
		{
			uIImage = page_dots_cache[0];
			page_dots_cache.RemoveAt(0);
			uIImage.Visible = true;
		}
		else
		{
			uIImage = new UIImage();
			uIImage.Enable = false;
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(page_dot_image_cell[0].cell_texture), D3DMain.Instance.ConvertRectAutoHD(page_dot_image_cell[0].cell_rect));
			ui_manager.Add(uIImage);
		}
		return uIImage;
	}

	public void RegroupPageDot(int page_count)
	{
		if (page_count > page_dots.Count)
		{
			int num = page_count - page_dots.Count;
			for (int i = 0; i < num; i++)
			{
				UIImage pageDot = GetPageDot();
				page_dots.Add(pageDot);
			}
		}
		else if (page_count < page_dots.Count)
		{
			int num2 = page_dots.Count - page_count;
			for (int j = 0; j < num2; j++)
			{
				UIImage uIImage = page_dots[0];
				page_dots.RemoveAt(0);
				page_dots_cache.Add(uIImage);
				uIImage.Visible = false;
			}
		}
		float num3 = (12 * page_count + 20 * (page_count - 1)) * D3DMain.Instance.HD_SIZE;
		float num4 = dot_center.x - num3 * 0.5f;
		for (int k = 0; k < page_dots.Count; k++)
		{
			page_dots[k].Rect = new Rect(num4 + (float)(32 * k * D3DMain.Instance.HD_SIZE), dot_center.y, 12 * D3DMain.Instance.HD_SIZE, 12 * D3DMain.Instance.HD_SIZE);
		}
		UpdatePageDot();
	}

	protected void UpdatePageDot()
	{
		if (page_dots.Count == 0)
		{
			return;
		}
		for (int i = 0; i < page_dots.Count; i++)
		{
			if (current_page_index == i)
			{
				page_dots[i].SetTexture(ui_helper.LoadUIMaterialAutoHD(page_dot_image_cell[1].cell_texture), D3DMain.Instance.ConvertRectAutoHD(page_dot_image_cell[1].cell_rect));
			}
			else
			{
				page_dots[i].SetTexture(ui_helper.LoadUIMaterialAutoHD(page_dot_image_cell[0].cell_texture), D3DMain.Instance.ConvertRectAutoHD(page_dot_image_cell[0].cell_rect));
			}
		}
	}

	public void PageSlip(UIMove.Command move_command, Vector2 touch_position, float slip_delta)
	{
		if (page_dots.Count == 0)
		{
			return;
		}
		switch (move_command)
		{
		case UIMove.Command.Down:
			if (enable_page_auto_slip)
			{
				enable_page_auto_slip = false;
				click_event = false;
			}
			else
			{
				click_event = true;
			}
			page_moved_delta = 0;
			break;
		case UIMove.Command.Hold:
			page_moved_delta = 0;
			break;
		case UIMove.Command.Begin:
			click_event = false;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SCROLL_SLIP), null, false, false);
			OnBeginMove();
			break;
		case UIMove.Command.Move:
			page_moved_delta++;
			page_slip_x += slip_delta;
			if (slip_delta < 0f)
			{
				page_move_left = true;
				if (page_slip_x < page_center_x)
				{
					next_page_index = current_page_index + 1;
					if (next_page_index >= page_dots.Count)
					{
						page_slip_x -= slip_delta * 0.5f;
					}
				}
				else
				{
					next_page_index = current_page_index - 1;
				}
				break;
			}
			page_move_left = false;
			if (page_slip_x > page_center_x)
			{
				next_page_index = current_page_index - 1;
				if (next_page_index < 0)
				{
					page_slip_x -= slip_delta * 0.5f;
				}
			}
			else
			{
				next_page_index = current_page_index + 1;
			}
			break;
		case UIMove.Command.End:
			if (page_moved_delta > 0 && page_moved_delta < 10)
			{
				page_quick_slip = true;
				if (page_move_left)
				{
					next_page_index = current_page_index + 1;
				}
				else
				{
					next_page_index = current_page_index - 1;
				}
			}
			else
			{
				page_quick_slip = false;
			}
			if (click_event)
			{
				OnClickEvent(touch_position);
				click_event = false;
			}
			else
			{
				EnablePageAutoSlip();
			}
			break;
		}
		PageUIUpdate();
	}

	protected void EnablePageAutoSlip()
	{
		if (page_dots.Count == 0)
		{
			return;
		}
		float num = page_slip_x - page_center_x;
		if (page_quick_slip)
		{
			if (page_move_left)
			{
				if (next_page_index >= page_dots.Count)
				{
					num = 0f - num;
					page_auto_slip_target = page_center_x;
					page_auto_slip_target_index = current_page_index;
				}
				else
				{
					page_auto_slip_target = page_center_x - page_size.x;
					num = page_auto_slip_target - page_slip_x;
					page_auto_slip_target_index = next_page_index;
				}
			}
			else if (next_page_index < 0)
			{
				num = 0f - num;
				page_auto_slip_target = page_center_x;
				page_auto_slip_target_index = current_page_index;
			}
			else
			{
				page_auto_slip_target = page_center_x + page_size.x;
				num = page_auto_slip_target - page_slip_x;
				page_auto_slip_target_index = next_page_index;
			}
		}
		else if (Mathf.Abs(num) < page_size.x * 0.25f)
		{
			num = 0f - num;
			page_auto_slip_target = page_center_x;
			page_auto_slip_target_index = current_page_index;
		}
		else if (num > 0f)
		{
			if (next_page_index < 0)
			{
				num = 0f - num;
				page_auto_slip_target = page_center_x;
				page_auto_slip_target_index = current_page_index;
			}
			else
			{
				page_auto_slip_target = page_center_x + page_size.x;
				num = page_auto_slip_target - page_slip_x;
				page_auto_slip_target_index = next_page_index;
			}
		}
		else if (next_page_index >= page_dots.Count)
		{
			num = 0f - num;
			page_auto_slip_target = page_center_x;
			page_auto_slip_target_index = current_page_index;
		}
		else
		{
			page_auto_slip_target = page_center_x - page_size.x;
			num = page_auto_slip_target - page_slip_x;
			page_auto_slip_target_index = next_page_index;
		}
		page_auto_slip_delta = num / 0.3f;
		enable_page_auto_slip = true;
		page_slip_behaviour.StartRealTime();
	}

	public void PageAutoSlip(float delta_time)
	{
		if (page_dots.Count == 0 || !enable_page_auto_slip)
		{
			return;
		}
		bool flag = false;
		page_slip_x += page_auto_slip_delta * delta_time;
		if (page_auto_slip_delta > 0f)
		{
			if (page_slip_x >= page_auto_slip_target)
			{
				flag = true;
			}
		}
		else if (page_slip_x <= page_auto_slip_target)
		{
			flag = true;
		}
		PageUIUpdate();
		if (flag)
		{
			OnPageAutoSlipOver();
		}
	}

	protected virtual void OnBeginMove()
	{
	}

	protected virtual void OnClickEvent(Vector2 click_point)
	{
	}

	protected virtual void PageUIUpdate()
	{
	}

	protected virtual void OnPageAutoSlipOver()
	{
		enable_page_auto_slip = false;
		current_page_index = page_auto_slip_target_index;
		page_slip_x = page_center_x;
		next_page_index = current_page_index;
		UpdatePageDot();
	}
}
