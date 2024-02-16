using System.Collections.Generic;
using UnityEngine;

public class D3DSkillScroll : D3DScrollManager
{
	private List<D3DSkillBarUI> skill_bar_list;

	private List<D3DSkillBarUI> skill_bar_cache;

	public D3DSkillScroll(UIManager manager, UIHelper helper, Rect camera_view_port)
		: base(manager, helper, camera_view_port)
	{
		skill_bar_list = new List<D3DSkillBarUI>();
		skill_bar_cache = new List<D3DSkillBarUI>();
	}

	public void InitActiveSkillForSkillSet(Dictionary<string, D3DClassActiveSkillStatus> puppet_skill_list, UIText info_text)
	{
		RecycleBarList();
		int num = 1;
		foreach (string key in puppet_skill_list.Keys)
		{
			if (puppet_skill_list[key].skill_level >= 0)
			{
				D3DSkillBarUI d3DSkillBarUI;
				if (skill_bar_cache.Count > 0)
				{
					d3DSkillBarUI = skill_bar_cache[0];
					skill_bar_cache.RemoveAt(0);
				}
				else
				{
					d3DSkillBarUI = new D3DSkillBarUI(ui_manager, ui_helper);
					d3DSkillBarUI.CreateControl();
				}
				d3DSkillBarUI.bar_index = num - 1;
				d3DSkillBarUI.UpdateSkillBarInfo(new Vector2(-1f, camera_default_rect.yMax / (float)D3DMain.Instance.HD_SIZE - (float)(62 * num)), puppet_skill_list[key], puppet_skill_list[key].active_skill, false);
				skill_bar_list.Add(d3DSkillBarUI);
				num++;
			}
		}
		scroll_limit.yMin = camera_default_rect.yMax - D3DSkillBarUI.bar_height * (float)(num - 1) * (float)D3DMain.Instance.HD_SIZE;
		InitScrollBar();
		ResetScroll();
		if (num > 1)
		{
			info_text.Visible = false;
			return;
		}
		info_text.SetText("This skill hasn't been learned yet.");
		info_text.Visible = true;
		info_text.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 70f, 410f, 30f);
	}

	public void InitPassiveSkillForSkillSet(Dictionary<string, D3DClassPassiveSkillStatus> puppet_skill_list, UIText info_text)
	{
		RecycleBarList();
		int num = 1;
		foreach (string key in puppet_skill_list.Keys)
		{
			if (puppet_skill_list[key].skill_level >= 0)
			{
				D3DSkillBarUI d3DSkillBarUI;
				if (skill_bar_cache.Count > 0)
				{
					d3DSkillBarUI = skill_bar_cache[0];
					skill_bar_cache.RemoveAt(0);
				}
				else
				{
					d3DSkillBarUI = new D3DSkillBarUI(ui_manager, ui_helper);
					d3DSkillBarUI.CreateControl();
				}
				d3DSkillBarUI.bar_index = num - 1;
				d3DSkillBarUI.UpdateSkillBarInfo(new Vector2(-1f, camera_default_rect.yMax / (float)D3DMain.Instance.HD_SIZE - (float)(62 * num)), puppet_skill_list[key], puppet_skill_list[key].passive_skill, false);
				skill_bar_list.Add(d3DSkillBarUI);
				num++;
			}
		}
		scroll_limit.yMin = camera_default_rect.yMax - D3DSkillBarUI.bar_height * (float)(num - 1) * (float)D3DMain.Instance.HD_SIZE;
		InitScrollBar();
		ResetScroll();
		if (num > 1)
		{
			info_text.Visible = false;
			return;
		}
		info_text.SetText("This skill hasn't been learned yet.");
		info_text.Visible = true;
		info_text.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 70f, 410f, 30f);
	}

	public void InitActiveSkillForSkillSchool(Dictionary<string, D3DClassActiveSkillStatus> puppet_skill_list, int puppet_level, UIText info_text)
	{
		RecycleBarList();
		int num = 1;
		foreach (string key in puppet_skill_list.Keys)
		{
			if (!puppet_skill_list[key].SkillMax)
			{
				D3DSkillBarUI d3DSkillBarUI;
				if (skill_bar_cache.Count > 0)
				{
					d3DSkillBarUI = skill_bar_cache[0];
					skill_bar_cache.RemoveAt(0);
				}
				else
				{
					d3DSkillBarUI = new D3DSkillBarUI(ui_manager, ui_helper);
					d3DSkillBarUI.CreateControl();
				}
				d3DSkillBarUI.bar_index = num - 1;
				d3DSkillBarUI.UpdateSkillBarInfo(new Vector2(-1f, camera_default_rect.yMax / (float)D3DMain.Instance.HD_SIZE - (float)(62 * num)), puppet_skill_list[key], puppet_skill_list[key].active_skill, true);
				d3DSkillBarUI.CheckSkillUpgradeValid(puppet_skill_list[key].UpgradeRequireLevel <= puppet_level, puppet_skill_list[key].UpgradeCost <= int.Parse(D3DGamer.Instance.CurrencyText), puppet_skill_list[key].UpgradeCrystal <= int.Parse(D3DGamer.Instance.CrystalText));
				skill_bar_list.Add(d3DSkillBarUI);
				num++;
			}
		}
		scroll_limit.yMin = camera_default_rect.yMax - D3DSkillBarUI.bar_height * (float)(num - 1) * (float)D3DMain.Instance.HD_SIZE;
		InitScrollBar();
		ResetScroll();
		if (num > 1)
		{
			info_text.Visible = false;
			return;
		}
		info_text.SetText("All skills learned!");
		info_text.Visible = true;
		info_text.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 70f, 410f, 78f);
	}

	public void InitPassiveSkillForSkillSchool(Dictionary<string, D3DClassPassiveSkillStatus> puppet_skill_list, int puppet_level, UIText info_text)
	{
		RecycleBarList();
		int num = 1;
		foreach (string key in puppet_skill_list.Keys)
		{
			if (!puppet_skill_list[key].SkillMax && !puppet_skill_list[key]._bDeprecated)
			{
				D3DSkillBarUI d3DSkillBarUI;
				if (skill_bar_cache.Count > 0)
				{
					d3DSkillBarUI = skill_bar_cache[0];
					skill_bar_cache.RemoveAt(0);
				}
				else
				{
					d3DSkillBarUI = new D3DSkillBarUI(ui_manager, ui_helper);
					d3DSkillBarUI.CreateControl();
				}
				d3DSkillBarUI.bar_index = num - 1;
				d3DSkillBarUI.UpdateSkillBarInfo(new Vector2(-1f, camera_default_rect.yMax / (float)D3DMain.Instance.HD_SIZE - (float)(62 * num)), puppet_skill_list[key], puppet_skill_list[key].passive_skill, true);
				d3DSkillBarUI.CheckSkillUpgradeValid(puppet_skill_list[key].UpgradeRequireLevel <= puppet_level, puppet_skill_list[key].UpgradeCost <= int.Parse(D3DGamer.Instance.CurrencyText), puppet_skill_list[key].UpgradeCrystal <= int.Parse(D3DGamer.Instance.CrystalText));
				skill_bar_list.Add(d3DSkillBarUI);
				num++;
			}
		}
		scroll_limit.yMin = camera_default_rect.yMax - D3DSkillBarUI.bar_height * (float)(num - 1) * (float)D3DMain.Instance.HD_SIZE;
		InitScrollBar();
		ResetScroll();
		if (num > 1)
		{
			info_text.Visible = false;
			return;
		}
		info_text.SetText("All skills learned!");
		info_text.Visible = true;
		info_text.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 70f, 410f, 78f);
	}

	public List<UIImage> CheckNewSkill(string puppet_id)
	{
		if (!D3DGamer.Instance.NewSkillHint.ContainsKey(puppet_id))
		{
			return null;
		}
		List<UIImage> list = new List<UIImage>();
		foreach (D3DSkillBarUI item in skill_bar_list)
		{
			if (D3DGamer.Instance.NewSkillHint[puppet_id].Contains(item.SkillId))
			{
				item.NewHint.Visible = true;
				list.Add(item.NewHint);
			}
		}
		return list;
	}

	private void RecycleBarList()
	{
		foreach (D3DSkillBarUI item in skill_bar_list)
		{
			item.HideBar();
		}
		skill_bar_cache.AddRange(skill_bar_list);
		skill_bar_list.Clear();
	}

	public D3DSkillBarUI GetSkillBar(Vector2 touch_point)
	{
		Vector3 localPosition = ui_manager.GetManagerCamera().transform.localPosition;
		Vector2 point = ui_manager.TouchPointOnManager(touch_point) + new Vector2(localPosition.x - camera_reset_position.x, localPosition.y - camera_reset_position.y);
		float num = (float)Screen.height / 640f;
		point += Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		foreach (D3DSkillBarUI item in skill_bar_list)
		{
			if (item.PtInSlot(point))
			{
				return item;
			}
		}
		return null;
	}

	public D3DSkillBarUI GetSkillBar(string skill_id)
	{
		if (string.Empty == skill_id)
		{
			return null;
		}
		foreach (D3DSkillBarUI item in skill_bar_list)
		{
			if (item.SkillId == skill_id)
			{
				return item;
			}
		}
		return null;
	}

	public void JumpScroll(int bar_index)
	{
		ResetScroll();
		Scroll(Vector2.up * bar_index * D3DSkillBarUI.bar_height * 2f);
	}
}
