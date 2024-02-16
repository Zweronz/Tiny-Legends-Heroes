using UnityEngine;

public class D3DDungeonPortalSlipUI : D3DPageSlipUI
{
	public delegate void DungeonPortal(int portal_level);

	private D3DDungeonPortalSelectUI[] current_page_select_ui;

	private D3DDungeonPortalSelectUI[] next_page_select_ui;

	private bool left_updated;

	private bool right_updated;

	private int last_level = -1;

	private DungeonPortal dungeonPortal;

	public D3DDungeonPortalSlipUI(UIManager manager, UIHelper helper, int page_count, Rect page_rect, float page_dot_y)
		: base(manager, helper, page_count, page_rect, page_dot_y)
	{
		current_page_select_ui = new D3DDungeonPortalSelectUI[15];
		next_page_select_ui = new D3DDungeonPortalSelectUI[15];
		for (int i = 0; i < 15; i++)
		{
			current_page_select_ui[i] = new D3DDungeonPortalSelectUI(ui_manager, ui_helper);
			next_page_select_ui[i] = new D3DDungeonPortalSelectUI(ui_manager, ui_helper);
			current_page_select_ui[i].Visible(false);
			next_page_select_ui[i].Visible(false);
		}
	}

	public void OpenPortalUI(int page_index, int last_level)
	{
		current_page_index = page_index;
		this.last_level = last_level;
		UpdatePortalUI();
	}

	private void UpdatePortalUI()
	{
		if (current_page_index < 0)
		{
			current_page_index = 0;
		}
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0)
		{
			ui_helper.GetControl("ProtalCampBtn").Enable = false;
			ui_helper.GetControl("ProtalCampBtn").Visible = false;
			ui_helper.GetControl("ProtalCampTxt").Enable = false;
			ui_helper.GetControl("ProtalCampTxt").Visible = false;
		}
		else
		{
			ui_helper.GetControl("ProtalCampBtn").Enable = true;
			ui_helper.GetControl("ProtalCampBtn").Visible = true;
			ui_helper.GetControl("ProtalCampTxt").Enable = true;
			ui_helper.GetControl("ProtalCampTxt").Visible = true;
		}
		for (int i = 0; i < current_page_select_ui.Length; i++)
		{
			int num = current_page_index * 15 + i + 1;
			D3DDungeonPortalSelectUI.LevelSelectState state = D3DDungeonPortalSelectUI.LevelSelectState.NORMAL;
			if (num > D3DMain.Instance.exploring_dungeon.dungeon.dungeon_floors.Count)
			{
				current_page_select_ui[i].Visible(false);
				continue;
			}
			if (num > D3DMain.Instance.exploring_dungeon.dungeon.explored_level)
			{
				state = D3DDungeonPortalSelectUI.LevelSelectState.DISABLE;
			}
			else if (num == D3DMain.Instance.exploring_dungeon.current_floor)
			{
				state = D3DDungeonPortalSelectUI.LevelSelectState.CURRENT;
			}
			else if (num == last_level)
			{
				state = D3DDungeonPortalSelectUI.LevelSelectState.LAST;
			}
			current_page_select_ui[i].Visible(true);
			current_page_select_ui[i].Init(new Vector2(46 + i % 5 * 80, 215 - i / 5 * 75), num, state);
		}
		UpdatePageDot();
	}

	private void UpdateNextPortalUI()
	{
		if (next_page_index < 0 || next_page_index >= page_dots.Count)
		{
			for (int i = 0; i < next_page_select_ui.Length; i++)
			{
				next_page_select_ui[i].Visible(false);
			}
			return;
		}
		float num = ((next_page_index >= current_page_index) ? page_size.x : (0f - page_size.x));
		for (int j = 0; j < next_page_select_ui.Length; j++)
		{
			int num2 = next_page_index * 15 + j + 1;
			D3DDungeonPortalSelectUI.LevelSelectState state = D3DDungeonPortalSelectUI.LevelSelectState.NORMAL;
			if (num2 > D3DMain.Instance.exploring_dungeon.dungeon.dungeon_floors.Count)
			{
				next_page_select_ui[j].Visible(false);
				continue;
			}
			if (num2 > D3DMain.Instance.exploring_dungeon.dungeon.explored_level)
			{
				state = D3DDungeonPortalSelectUI.LevelSelectState.DISABLE;
			}
			else if (num2 == D3DMain.Instance.exploring_dungeon.current_floor)
			{
				state = D3DDungeonPortalSelectUI.LevelSelectState.CURRENT;
			}
			else if (num2 == last_level)
			{
				state = D3DDungeonPortalSelectUI.LevelSelectState.LAST;
			}
			next_page_select_ui[j].Visible(true);
			next_page_select_ui[j].Init(new Vector2(num * (1f / (float)D3DMain.Instance.HD_SIZE) + 46f + (float)(j % 5 * 80), 215 - j / 5 * 75), num2, state);
		}
		UpdatePageDot();
	}

	protected override void PageUIUpdate()
	{
		if (current_page_index != next_page_index)
		{
			float delta_slip = page_slip_x - page_center_x;
			for (int i = 0; i < current_page_select_ui.Length; i++)
			{
				current_page_select_ui[i].UISlip(delta_slip);
			}
			if (next_page_index < current_page_index && !left_updated)
			{
				UpdateNextPortalUI();
				left_updated = true;
				right_updated = false;
			}
			else if (next_page_index > current_page_index && !right_updated)
			{
				UpdateNextPortalUI();
				right_updated = true;
				left_updated = false;
			}
			for (int j = 0; j < next_page_select_ui.Length; j++)
			{
				next_page_select_ui[j].UISlip(delta_slip);
			}
		}
	}

	protected override void OnPageAutoSlipOver()
	{
		base.OnPageAutoSlipOver();
		UpdatePortalUI();
		right_updated = false;
		left_updated = false;
		for (int i = 0; i < next_page_select_ui.Length; i++)
		{
			next_page_select_ui[i].Visible(false);
		}
	}

	protected override void OnClickEvent(Vector2 click_position)
	{
		D3DDungeonPortalSelectUI[] array = current_page_select_ui;
		foreach (D3DDungeonPortalSelectUI d3DDungeonPortalSelectUI in array)
		{
			if (d3DDungeonPortalSelectUI.Click(click_position))
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				int level = d3DDungeonPortalSelectUI.GetLevel();
				if (level != D3DMain.Instance.exploring_dungeon.current_floor)
				{
					dungeonPortal(level);
				}
				break;
			}
		}
	}

	public void SetDungeonPortalDelegate(DungeonPortal dungeonPortal)
	{
		this.dungeonPortal = dungeonPortal;
	}
}
