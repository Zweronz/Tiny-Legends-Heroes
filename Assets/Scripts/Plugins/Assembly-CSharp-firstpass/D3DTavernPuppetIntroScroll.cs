using System.Collections.Generic;
using UnityEngine;

public class D3DTavernPuppetIntroScroll : D3DScrollManager
{
	private List<UIText> intro_text_inuse;

	private List<UIText> intro_text_cache;

	private UIImage intro_skill_board;

	private UIImage intro_skill_icon;

	public D3DTavernPuppetIntroScroll(UIManager manager, UIHelper helper, Rect camera_view_port)
		: base(manager, helper, camera_view_port)
	{
		intro_text_inuse = new List<UIText>();
		intro_text_cache = new List<UIText>();
		intro_skill_board = new UIImage();
		D3DImageCell imageCell = ui_helper.GetImageCell("beibaokuang");
		intro_skill_board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(32f, 32f) * D3DMain.Instance.HD_SIZE);
		intro_skill_board.Enable = false;
		intro_skill_board.Visible = false;
		ui_manager.Add(intro_skill_board);
		intro_skill_icon = new UIImage();
		intro_skill_icon.Enable = false;
		intro_skill_icon.Visible = false;
		ui_manager.Add(intro_skill_icon);
	}

	private UIText GetIntroText()
	{
		UIText uIText;
		if (intro_text_cache.Count > 0)
		{
			uIText = intro_text_cache[0];
			intro_text_cache.RemoveAt(0);
		}
		else
		{
			uIText = new UIText();
			uIText.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 7), string.Empty, D3DMain.Instance.CommonFontColor);
			uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(7 * D3DMain.Instance.HD_SIZE);
			uIText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(7 * D3DMain.Instance.HD_SIZE);
			uIText.Enable = false;
			ui_manager.Add(uIText);
		}
		uIText.Visible = true;
		intro_text_inuse.Add(uIText);
		return uIText;
	}

	public void UpdatePuppetIntro(HeroSynopsis hero_synopsis)
	{
		foreach (UIText item in intro_text_inuse)
		{
			item.Visible = false;
		}
		intro_text_cache.AddRange(intro_text_inuse);
		intro_text_inuse.Clear();
		intro_skill_board.Visible = false;
		intro_skill_icon.Visible = false;
		scroll_limit.yMin = scroll_limit.yMax;
		if (hero_synopsis != null)
		{
			foreach (string item2 in hero_synopsis.puppet_intro)
			{
				UIText introText = GetIntroText();
				introText.Rect = new Rect(0f, 0f, scroll_limit.width - (float)(15 * D3DMain.Instance.HD_SIZE), 999f);
				introText.SetText(item2);
				float linesTotalHeight = introText.GetLinesTotalHeight();
				scroll_limit.yMin -= linesTotalHeight;
				introText.Rect = new Rect(0f, scroll_limit.yMin, scroll_limit.width - (float)(15 * D3DMain.Instance.HD_SIZE), linesTotalHeight);
				scroll_limit.yMin -= introText.LineSpacing;
			}
			scroll_limit.yMin -= 15 * D3DMain.Instance.HD_SIZE;
			if (string.Empty != hero_synopsis.expert_skill_id)
			{
				D3DActiveSkill activeSkill = D3DMain.Instance.GetActiveSkill(hero_synopsis.expert_skill_id);
				UIText introText2 = GetIntroText();
				introText2.Rect = new Rect(0f, 0f, scroll_limit.width - (float)(15 * D3DMain.Instance.HD_SIZE), 999f);
				introText2.SetText("SKILL:");
				float linesTotalHeight2 = introText2.GetLinesTotalHeight();
				scroll_limit.yMin -= linesTotalHeight2;
				introText2.Rect = new Rect(0f, scroll_limit.yMin, scroll_limit.width - (float)(15 * D3DMain.Instance.HD_SIZE), linesTotalHeight2);
				scroll_limit.yMin -= introText2.LineSpacing;
				intro_skill_board.Rect = new Rect(0f, scroll_limit.yMin - (float)(32 * D3DMain.Instance.HD_SIZE), 32 * D3DMain.Instance.HD_SIZE, 32 * D3DMain.Instance.HD_SIZE);
				intro_skill_board.Visible = true;
				scroll_limit.yMin -= introText2.LineSpacing;
				Rect rect = intro_skill_board.Rect;
				D3DImageCell iconCell = ui_helper.GetIconCell(activeSkill.skill_icon);
				intro_skill_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect), new Vector2(26f, 26f) * D3DMain.Instance.HD_SIZE);
				intro_skill_icon.Rect = new Rect(rect.x + (float)(2 * D3DMain.Instance.HD_SIZE), rect.y + (float)(3 * D3DMain.Instance.HD_SIZE), 26 * D3DMain.Instance.HD_SIZE, 26 * D3DMain.Instance.HD_SIZE);
				intro_skill_icon.Visible = true;
				introText2 = GetIntroText();
				introText2.SetText(activeSkill.skill_name);
				introText2.Rect = new Rect(rect.xMax + (float)(5 * D3DMain.Instance.HD_SIZE), rect.yMin + (float)(9 * D3DMain.Instance.HD_SIZE), scroll_limit.width - (float)(32 * D3DMain.Instance.HD_SIZE), 15 * D3DMain.Instance.HD_SIZE);
			}
		}
		InitScrollBar();
		ResetScroll();
	}
}
