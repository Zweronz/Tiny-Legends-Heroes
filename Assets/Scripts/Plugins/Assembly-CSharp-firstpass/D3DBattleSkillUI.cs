using System;
using UnityEngine;

public class D3DBattleSkillUI : D3DCustomUI
{
	public D3DClassBattleSkillStatus skill_status;

	public UIImage skill_board;

	public UIImage skill_icon;

	public UIPushButton press_mask;

	public UIImage disable_mask;

	public UIImage cd_mask;

	public D3DBattleSkillUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
		ui_position = Vector2.zero;
		skill_status = null;
		skill_board = new UIImage();
		skill_board.Enable = false;
		D3DImageCell imageCell = ui_helper.GetImageCell("jinengkuang");
		skill_board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		ui_manager.Add(skill_board);
		skill_icon = new UIImage();
		skill_icon.Enable = false;
		ui_manager.Add(skill_icon);
		press_mask = new UIPushButton();
		press_mask.Enable = false;
		ui_manager.Add(press_mask);
		disable_mask = new UIImage();
		disable_mask.Enable = false;
		imageCell = ui_helper.GetImageCell("zhezhao-2");
		disable_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		ui_manager.Add(disable_mask);
		cd_mask = new UIImage();
		cd_mask.Enable = false;
		imageCell = ui_helper.GetImageCell("zhezhao-1");
		cd_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		cd_mask.SetRotation((float)Math.PI);
		ui_manager.Add(cd_mask);
	}

	public void InitUI(D3DClassBattleSkillStatus skill_status, Vector2 position)
	{
		this.skill_status = skill_status;
		skill_status.battle_ui = this;
		ui_position = position;
		skill_board.Rect = D3DMain.Instance.ConvertRectAutoHD(position.x, position.y, 47f, 47f);
		skill_board.Visible = true;
		D3DImageCell iconCell = ui_helper.GetIconCell(skill_status.active_skill.skill_icon);
		skill_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect), new Vector2(36f, 36f) * D3DMain.Instance.HD_SIZE);
		skill_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(position.x + 5f, position.y + 5f, 36f, 36f);
		skill_icon.Visible = true;
		if (skill_status.IsActivateSkill())
		{
			iconCell = ui_helper.GetImageCell("jinengxuanzhong");
			press_mask.SetTexture(UIButtonBase.State.Pressed, ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		}
		else
		{
			press_mask.SetTexture(UIButtonBase.State.Pressed, null, new Rect(0f, 0f, 0f, 0f));
		}
		press_mask.Rect = new Rect(skill_icon.Rect.x - (float)(10 * D3DMain.Instance.HD_SIZE), skill_icon.Rect.y - (float)(10 * D3DMain.Instance.HD_SIZE), skill_icon.Rect.width + (float)(20 * D3DMain.Instance.HD_SIZE), skill_icon.Rect.height + (float)(20 * D3DMain.Instance.HD_SIZE));
		press_mask.Set(false);
		press_mask.Visible = true;
		press_mask.Enable = true;
		disable_mask.Rect = skill_icon.Rect;
		disable_mask.Visible = skill_status.Enable;
		cd_mask.Rect = skill_icon.Rect;
		cd_mask.Visible = true;
	}

	public void DisableUI()
	{
		skill_board.Visible = false;
		skill_icon.Visible = false;
		press_mask.Set(false);
		press_mask.Visible = false;
		press_mask.Enable = false;
		disable_mask.Visible = false;
		cd_mask.Visible = false;
		skill_status.battle_ui = null;
		skill_status = null;
	}
}
