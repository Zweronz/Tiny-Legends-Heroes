using UnityEngine;

public class D3DPuppetGearSlotUI : D3DGearSlotUI
{
	private UIImage arm_hint_frame;

	public D3DPuppetGearSlotUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateControl(Vector2 position, string board_cell)
	{
		ui_position = position;
		D3DImageCell imageCell;
		if (string.Empty != board_cell)
		{
			board = new UIImage();
			imageCell = ui_helper.GetImageCell(board_cell);
			board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			board.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 44f, 44f);
			board.Enable = false;
			ui_manager.Add(board);
		}
		grade_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("wupindengji-1");
		grade_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		grade_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 5f, ui_position.y + 6.5f, 32f, 32f));
		grade_frame.Enable = false;
		ui_manager.Add(grade_frame);
		slot_hover_mask = new UIImage();
		imageCell = ui_helper.GetImageCell("tuodongwupintingliuzhuangtai-1");
		slot_hover_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		slot_hover_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 4f, ui_position.y + 6f, 33f, 33f));
		slot_hover_mask.Enable = false;
		ui_manager.Add(slot_hover_mask);
		arm_hint_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("renwuzhuangbeiduiyingzhuangtai");
		arm_hint_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		arm_hint_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 2.5f, ui_position.y + 5f, 36f, 36f);
		arm_hint_frame.Enable = false;
		ui_manager.Add(arm_hint_frame);
		select_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("xuanzhongzhuangtai");
		select_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		select_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 2.5f, ui_position.y + 5f, 36f, 36f);
		select_frame.Enable = false;
		ui_manager.Add(select_frame);
		slot_icon = new UIImage();
		slot_icon.Enable = false;
		slot_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 1.5f, ui_position.y + 2f, 41f, 41f));
		ui_manager.Add(slot_icon);
		HideSlot();
	}

	public new void HideSlot()
	{
		slot_icon.Visible = false;
		arm_hint_frame.Visible = false;
		grade_frame.Visible = false;
		select_frame.Visible = false;
		slot_hover_mask.Visible = false;
	}

	public void ArmHint(bool hint)
	{
		arm_hint_frame.Visible = hint;
	}
}
