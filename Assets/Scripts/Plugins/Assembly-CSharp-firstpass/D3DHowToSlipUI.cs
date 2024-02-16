using System.Collections.Generic;
using UnityEngine;

public class D3DHowToSlipUI : D3DPageSlipUI
{
	private UIImage current_howto_ill;

	private UIImage next_howto_ill;

	private bool left_updated;

	private bool right_updated;

	private List<string> howto_ills;

	private UIImage[] IllArrows;

	private UIText PageText;

	public D3DHowToSlipUI(UIManager manager, UIHelper helper, int page_count, Rect page_rect, float page_dot_y, UIImage[] arrows, UIText page_text)
		: base(manager, helper, page_count, page_rect, page_dot_y)
	{
		current_howto_ill = new UIImage();
		current_howto_ill.Enable = false;
		ui_manager.Add(current_howto_ill);
		next_howto_ill = new UIImage();
		next_howto_ill.Enable = false;
		ui_manager.Add(next_howto_ill);
		IllArrows = arrows;
		PageText = page_text;
	}

	public void InitHowToUI(List<string> ills)
	{
		base.OnPageAutoSlipOver();
		right_updated = false;
		left_updated = false;
		next_howto_ill.Visible = false;
		current_page_index = 0;
		next_page_index = 1;
		howto_ills = ills;
		RegroupPageDot(howto_ills.Count);
		UpdateHowToUI();
		UpdateNextHowToUI();
		PageText.SetText(current_page_index + 1 + "/" + howto_ills.Count);
	}

	private void UpdateHowToUI()
	{
		if (current_page_index < 0)
		{
			current_page_index = 0;
		}
		D3DImageCell imageCell = ui_helper.GetImageCell(howto_ills[current_page_index]);
		current_howto_ill.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		current_howto_ill.SetPosition(new Vector2(188.5f, 125.5f) * D3DMain.Instance.HD_SIZE);
		current_howto_ill.Visible = true;
		IllArrows[0].Visible = 0 != current_page_index;
		IllArrows[1].Visible = current_page_index < page_dots.Count - 1;
		UpdatePageDot();
	}

	private void UpdateNextHowToUI()
	{
		if (next_page_index < 0 || next_page_index >= page_dots.Count)
		{
			next_howto_ill.Visible = false;
			return;
		}
		float num = ((next_page_index >= current_page_index) ? page_size.x : (0f - page_size.x));
		D3DImageCell imageCell = ui_helper.GetImageCell(howto_ills[next_page_index]);
		next_howto_ill.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		next_howto_ill.SetPosition(new Vector2(num * (1f / (float)D3DMain.Instance.HD_SIZE) + 188.5f, 125.5f) * D3DMain.Instance.HD_SIZE);
		next_howto_ill.Visible = true;
		UpdatePageDot();
	}

	protected override void PageUIUpdate()
	{
		if (current_page_index != next_page_index)
		{
			float num = page_slip_x - page_center_x;
			current_howto_ill.SetPosition(new Vector2(188.5f, 125.5f) * D3DMain.Instance.HD_SIZE + Vector2.right * num);
			if (next_page_index < current_page_index && !left_updated)
			{
				UpdateNextHowToUI();
				left_updated = true;
				right_updated = false;
			}
			else if (next_page_index > current_page_index && !right_updated)
			{
				UpdateNextHowToUI();
				right_updated = true;
				left_updated = false;
			}
			float num2 = ((next_page_index >= current_page_index) ? page_size.x : (0f - page_size.x));
			next_howto_ill.SetPosition(new Vector2(188.5f + num2 * (1f / (float)D3DMain.Instance.HD_SIZE), 125.5f) * D3DMain.Instance.HD_SIZE + Vector2.right * num);
		}
	}

	protected override void OnPageAutoSlipOver()
	{
		base.OnPageAutoSlipOver();
		UpdateHowToUI();
		right_updated = false;
		left_updated = false;
		next_howto_ill.Visible = false;
		IllArrows[0].Visible = 0 != current_page_index;
		IllArrows[1].Visible = current_page_index < page_dots.Count - 1;
		PageText.SetText(current_page_index + 1 + "/" + howto_ills.Count);
	}
}
