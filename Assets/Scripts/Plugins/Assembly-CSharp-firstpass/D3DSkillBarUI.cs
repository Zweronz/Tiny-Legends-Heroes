using System.Collections.Generic;
using UnityEngine;

public class D3DSkillBarUI : D3DComplexSlotUI
{
	public static float bar_height = 62f;

	private string skill_id;

	private bool hit_icon;

	public int bar_index;

	private UIImage bar_img;

	private UIText skill_name_text;

	private UIText skill_upgrade_level;

	private D3DCurrencyText skill_upgrade_cost;

	private UIText skill_level_text;

	private List<UIText> skill_description;

	private UIImage new_hint;

	private Color font_color;

	public string SkillId
	{
		get
		{
			return skill_id;
		}
	}

	public bool HitIcon
	{
		get
		{
			return hit_icon;
		}
	}

	public UIImage NewHint
	{
		get
		{
			return new_hint;
		}
	}

	public D3DSkillBarUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
		skill_id = string.Empty;
		skill_description = new List<UIText>();
	}

	private UIText GetDescriptionUIText(int index)
	{
		if (index > skill_description.Count - 1)
		{
			UIText uIText = new UIText();
			uIText.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 7), string.Empty, D3DMain.Instance.CommonFontColor);
			uIText.Visible = false;
			uIText.Enable = false;
			uIText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(7 * D3DMain.Instance.HD_SIZE);
			uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(7 * D3DMain.Instance.HD_SIZE);
			ui_manager.Add(uIText);
			skill_description.Add(uIText);
			return uIText;
		}
		return skill_description[index];
	}

	public void CreateControl()
	{
		bar_img = new UIImage();
		D3DImageCell imageCell = ui_helper.GetImageCell("jinengshuominglan");
		bar_img.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		bar_img.Enable = false;
		ui_manager.Add(bar_img);
		slot_icon = new UIImage();
		slot_icon.Enable = false;
		ui_manager.Add(slot_icon);
		new_hint = new UIImage();
		imageCell = ui_helper.GetImageCell("zhuangbeikuangshandong");
		new_hint.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(56f, 56f) * D3DMain.Instance.HD_SIZE);
		new_hint.Enable = false;
		ui_manager.Add(new_hint);
		select_frame = new UIImage();
		imageCell = ui_helper.GetImageCell("xuanzhongzhuangtai");
		select_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(49f, 49f) * D3DMain.Instance.HD_SIZE);
		select_frame.Enable = false;
		ui_manager.Add(select_frame);
		font_color = D3DMain.Instance.CommonFontColor;
		skill_name_text = new UIText();
		skill_name_text.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		skill_name_text.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, font_color);
		skill_name_text.Enable = false;
		ui_manager.Add(skill_name_text);
		skill_upgrade_level = new UIText();
		skill_upgrade_level.AlignStyle = UIText.enAlignStyle.right;
		skill_upgrade_level.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		skill_upgrade_level.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, font_color);
		skill_upgrade_level.Enable = false;
		ui_manager.Add(skill_upgrade_level);
		skill_upgrade_cost = new D3DCurrencyText(ui_manager, ui_helper);
		skill_level_text = new UIText();
		skill_level_text.AlignStyle = UIText.enAlignStyle.right;
		skill_level_text.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		skill_level_text.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, Color.yellow);
		skill_level_text.Enable = false;
		ui_manager.Add(skill_level_text);
	}

	public void UpdateSkillBarInfo(Vector2 position, D3DClassSkillStatus skill_status, D3DSkillBasic skill, bool skill_school)
	{
		foreach (UIText item in skill_description)
		{
			item.Visible = false;
		}
		ui_position = position;
		skill_id = skill_status.skill_id;
		bar_img.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 410f, 62f);
		bar_img.Visible = true;
		D3DImageCell iconCell = ui_helper.GetIconCell(skill.skill_icon);
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect), new Vector2(47f, 47f) * D3DMain.Instance.HD_SIZE);
		slot_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 11.5f, ui_position.y + 8f, 47f, 47f));
		slot_icon.Visible = true;
		select_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 10.5f, ui_position.y + 7f, 49f, 49f);
		select_frame.Visible = false;
		new_hint.Visible = false;
		new_hint.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 7f, ui_position.y + 3.5f, 56f, 56f);
		skill_name_text.SetText(skill.skill_name);
		skill_name_text.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 63f, ui_position.y + 36f, 300f, 20f);
		skill_name_text.Visible = true;
		if (skill_school)
		{
			if (skill_status.UpgradeCost == 0)
			{
				skill_upgrade_cost.EnableGold = false;
			}
			else
			{
				skill_upgrade_cost.EnableGold = true;
				skill_upgrade_cost.SetGold(skill_status.UpgradeCost);
			}
			if (skill_status.UpgradeCrystal == 0)
			{
				skill_upgrade_cost.EnableCrystal = false;
			}
			else
			{
				skill_upgrade_cost.EnableCrystal = true;
				skill_upgrade_cost.SetCrystal(skill_status.UpgradeCrystal);
			}
			skill_upgrade_cost.SetPosition(new Vector2(ui_position.x + 405f - skill_upgrade_cost.GetUIWidth() / (float)D3DMain.Instance.HD_SIZE, ui_position.y + 36f));
			skill_upgrade_cost.Visible(true);
			skill_upgrade_level.SetText("Require:Lv " + skill_status.UpgradeRequireLevel);
			skill_upgrade_level.Rect = new Rect((ui_position.x + 272f) * (float)D3DMain.Instance.HD_SIZE - skill_upgrade_cost.GetUIWidth(), (ui_position.y + 35f) * (float)D3DMain.Instance.HD_SIZE, 130 * D3DMain.Instance.HD_SIZE, 20 * D3DMain.Instance.HD_SIZE);
			skill_upgrade_level.Visible = true;
		}
		else
		{
			skill_upgrade_level.Visible = false;
			skill_upgrade_cost.Visible(false);
		}
		skill_level_text.SetText(skill_status.skill_level + 1 + "/" + skill_status.MaxLevel);
		skill_level_text.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 355f, ui_position.y, 50f, 20f);
		skill_level_text.Visible = true;
		int num = skill_status.skill_level;
		if (skill_school)
		{
			num++;
		}
		List<string> skillDescription = skill.GetSkillDescription(num);
		for (int i = 0; i < skillDescription.Count; i++)
		{
			UIText descriptionUIText = GetDescriptionUIText(i);
			descriptionUIText.SetText(skillDescription[i]);
			descriptionUIText.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 63f, ui_position.y + 15f - (float)(i * 15), 300f, 20f);
			descriptionUIText.Visible = true;
		}
	}

	public void CheckSkillUpgradeValid(bool level_valid, bool cost_valid, bool crystal_valid)
	{
		if (level_valid)
		{
			skill_upgrade_level.SetColor(font_color);
		}
		else
		{
			skill_upgrade_level.SetColor(Color.red);
		}
		if (cost_valid)
		{
			skill_upgrade_cost.SetGoldColor(font_color);
		}
		else
		{
			skill_upgrade_cost.SetGoldColor(Color.red);
		}
		if (crystal_valid)
		{
			skill_upgrade_cost.SetCrystalColor(font_color);
		}
		else
		{
			skill_upgrade_cost.SetCrystalColor(Color.red);
		}
	}

	public void HideBar()
	{
		skill_id = string.Empty;
		bar_img.Visible = false;
		skill_name_text.Visible = false;
		select_frame.Visible = false;
		slot_icon.Visible = false;
		skill_upgrade_level.Visible = false;
		skill_upgrade_cost.Visible(false);
		skill_level_text.Visible = false;
		foreach (UIText item in skill_description)
		{
			item.Visible = false;
		}
		new_hint.Visible = false;
	}

	public override bool PtInSlot(Vector2 point)
	{
		if (select_frame.PtInRect(point))
		{
			hit_icon = true;
			return true;
		}
		if (bar_img.PtInRect(point))
		{
			hit_icon = false;
			return true;
		}
		return false;
	}
}
