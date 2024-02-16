using UnityEngine;

public class D3DGearSlotUI : D3DComplexSlotUI
{
	protected UIImage board;

	protected UIImage grade_frame;

	private UIImage new_hint;

	public int slot_index;

	public UIImage NewHint
	{
		get
		{
			return new_hint;
		}
	}

	public D3DGearSlotUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateControl(Vector2 position, string board_cell, float fScale = 1f)
	{
		ui_position = position;
		D3DImageCell imageCell;
		if (string.Empty != board_cell)
		{
			board = new UIImage();
			imageCell = ui_helper.GetImageCell(board_cell);
			board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			board.SetTextureSize(board.GetTextureSize());
			board.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 44f * fScale, 44f * fScale);
			board.Enable = false;
			ui_manager.Add(board);
		}
		grade_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("wupindengji-1");
		grade_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		grade_frame.SetTextureSize(grade_frame.GetTextureSize() * fScale);
		grade_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 5f * fScale, ui_position.y + 6.5f * fScale, 32f * fScale, 32f * fScale));
		grade_frame.Enable = false;
		ui_manager.Add(grade_frame);
		slot_hover_mask = new UIImage();
		imageCell = ui_helper.GetImageCell("tuodongwupintingliuzhuangtai-1");
		slot_hover_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		slot_hover_mask.SetTextureSize(slot_hover_mask.GetTextureSize() * fScale);
		slot_hover_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 4f * fScale, ui_position.y + 6f * fScale, 33f * fScale, 33f * fScale));
		slot_hover_mask.Enable = false;
		ui_manager.Add(slot_hover_mask);
		new_hint = new UIImage();
		imageCell = ui_helper.GetImageCell("zhuangbeikuangshandong");
		new_hint.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		new_hint.SetTextureSize(new_hint.GetTextureSize() * fScale);
		new_hint.Enable = false;
		new_hint.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x - 1f * fScale, ui_position.y + 1f * fScale, 44f * fScale, 44f * fScale);
		ui_manager.Add(new_hint);
		select_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("xuanzhongzhuangtai");
		select_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		select_frame.SetTextureSize(select_frame.GetTextureSize() * fScale);
		select_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 2.5f * fScale, ui_position.y + 5f * fScale, 36f * fScale, 36f * fScale);
		select_frame.Enable = false;
		ui_manager.Add(select_frame);
		slot_icon = new UIImage();
		slot_icon.Enable = false;
		slot_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 1.5f * fScale, ui_position.y + 2f * fScale, 41f * fScale, 41f * fScale));
		ui_manager.Add(slot_icon);
		HideSlot();
	}

	public void UpdateGearSlot(D3DEquipment gear, D3DProfileInstance puppet_profile_instance, float fScale = 1f)
	{
		D3DImageCell iconCell = ui_helper.GetIconCell(gear.use_icon);
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(slot_icon.GetTextureSize() * fScale);
		slot_icon.SetTextureSize(new Vector2(slot_icon.Rect.width, slot_icon.Rect.height));
		slot_icon.Visible = true;
		select_frame.Visible = false;
		slot_hover_mask.Visible = false;
		if (gear.equipment_grade >= D3DEquipment.EquipmentGrade.SUPERIOR)
		{
			grade_frame.SetColor(D3DMain.Instance.GetEquipmentGradeColor(gear.equipment_grade));
			grade_frame.Visible = true;
		}
		if (puppet_profile_instance.puppet_class.editable)
		{
			if (!gear.IsEquipmentUseable(puppet_profile_instance))
			{
				slot_icon.SetColor(Color.red);
			}
			else
			{
				slot_icon.SetColor(Color.white);
			}
		}
		else
		{
			slot_icon.SetColor(Color.red);
		}
	}

	public void SetVisible(bool visible)
	{
		slot_icon.Visible = visible;
		grade_frame.Visible = visible;
		if (new_hint != null)
		{
			new_hint.Visible = visible;
		}
	}

	public void HideSlot()
	{
		slot_icon.Visible = false;
		grade_frame.Visible = false;
		select_frame.Visible = false;
		slot_hover_mask.Visible = false;
		if (new_hint != null)
		{
			new_hint.Visible = false;
		}
	}
}
