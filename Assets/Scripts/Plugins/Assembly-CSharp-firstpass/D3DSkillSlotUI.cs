using UnityEngine;

public class D3DSkillSlotUI : D3DComplexSlotUI
{
	private string skill_id;

	public int slot_index;

	public string SkillId
	{
		get
		{
			return skill_id;
		}
	}

	public D3DSkillSlotUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
		skill_id = string.Empty;
		slot_index = -1;
	}

	public void CreateControl(Vector2 position)
	{
		ui_position = position;
		slot_hover_mask = new UIImage();
		D3DImageCell imageCell = ui_helper.GetImageCell("tuodongwupintingliuzhuangtai-1");
		slot_hover_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(41f, 41f) * D3DMain.Instance.HD_SIZE);
		slot_hover_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 5.5f, ui_position.y + 8.5f, 41f, 41f));
		slot_hover_mask.Enable = false;
		ui_manager.Add(slot_hover_mask);
		select_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("xuanzhongzhuangtai");
		select_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(47f, 46f) * D3DMain.Instance.HD_SIZE);
		select_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 2.5f, ui_position.y + 5.5f, 47f, 46f);
		select_frame.Enable = false;
		ui_manager.Add(select_frame);
		slot_icon = new UIImage();
		slot_icon.Enable = false;
		slot_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 4f, ui_position.y + 6.5f, 44f, 44f));
		ui_manager.Add(slot_icon);
		HideSlot();
	}

	public void UpdateSkillSlot(D3DSkillBasic skill)
	{
		skill_id = skill.skill_id;
		D3DImageCell iconCell = ui_helper.GetIconCell(skill.skill_icon);
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect), new Vector2(44f, 44f) * D3DMain.Instance.HD_SIZE);
		slot_icon.Visible = true;
		select_frame.Visible = false;
	}

	public void LockState()
	{
		skill_id = string.Empty;
		D3DImageCell imageCell = ui_helper.GetImageCell("suo88X88");
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(47f, 47f) * D3DMain.Instance.HD_SIZE);
		slot_icon.Visible = true;
	}

	public void HideSlot()
	{
		skill_id = string.Empty;
		select_frame.Visible = false;
		slot_icon.Visible = false;
		slot_hover_mask.Visible = false;
	}
}
