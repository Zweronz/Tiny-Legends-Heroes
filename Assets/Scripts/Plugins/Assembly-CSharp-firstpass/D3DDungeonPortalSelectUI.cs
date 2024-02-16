using UnityEngine;

public class D3DDungeonPortalSelectUI : D3DCustomUI
{
	public enum LevelSelectState
	{
		NORMAL = 0,
		CURRENT = 1,
		DISABLE = 2,
		LAST = 3
	}

	private UIImage select_board;

	private D3DImageCell[] board_state_img;

	private UIText level_text;

	private float board_default_x;

	private float text_default_x;

	private LevelSelectState select_state;

	public LevelSelectState SelectState
	{
		get
		{
			return select_state;
		}
	}

	public D3DDungeonPortalSelectUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
		board_state_img = new D3DImageCell[8];
		board_state_img[0] = ui_helper.GetImageCell("cengjianniu-1");
		board_state_img[1] = ui_helper.GetImageCell("cengjianniu-2");
		board_state_img[2] = ui_helper.GetImageCell("cengjianniu-3");
		board_state_img[3] = ui_helper.GetImageCell("zhiqiancengshu-1");
		board_state_img[4] = ui_helper.GetImageCell("cengjianniu-6");
		board_state_img[5] = ui_helper.GetImageCell("cengjianniu-5");
		board_state_img[6] = ui_helper.GetImageCell("cengjianniu-4");
		board_state_img[7] = ui_helper.GetImageCell("BOSSdangqianceng");
		select_board = new UIImage();
		select_board.Enable = false;
		ui_manager.Add(select_board);
		level_text = new UIText();
		level_text.Enable = false;
		level_text.AlignStyle = UIText.enAlignStyle.center;
		level_text.SetColor(D3DMain.Instance.CommonFontColor);
		ui_manager.Add(level_text);
	}

	public void Visible(bool visible)
	{
		select_board.Visible = visible;
		level_text.Visible = visible;
	}

	public void Init(Vector2 position, int level, LevelSelectState state)
	{
		ui_position = position;
		select_state = state;
		int num = (D3DMain.Instance.exploring_dungeon.dungeon.dungeon_floors[level - 1].boss_level ? 4 : 0);
		float num2 = 0f;
		if (ui_manager.ScreenOffset.y == 64f)
		{
			num2 = -40f;
		}
		select_board.SetTexture(ui_helper.LoadUIMaterialAutoHD(board_state_img[(int)(num + select_state)].cell_texture), D3DMain.Instance.ConvertRectAutoHD(board_state_img[(int)(num + select_state)].cell_rect));
		select_board.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_manager.ScreenOffset.x * 0.5f + ui_position.x, ui_manager.ScreenOffset.y + ui_position.y + num2, 65f, 65f);
		switch (select_state)
		{
		case LevelSelectState.NORMAL:
		case LevelSelectState.LAST:
			level_text.SetFont(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9));
			level_text.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			level_text.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_manager.ScreenOffset.x * 0.5f + ui_position.x + 2.5f, ui_manager.ScreenOffset.y + ui_position.y + 10f + num2, 65f, 30f);
			break;
		case LevelSelectState.CURRENT:
			level_text.SetFont(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 13));
			level_text.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(13 * D3DMain.Instance.HD_SIZE);
			level_text.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_manager.ScreenOffset.x * 0.5f + ui_position.x + 3f, ui_manager.ScreenOffset.y + ui_position.y + num2, 65f, 43f);
			break;
		case LevelSelectState.DISABLE:
			level_text.Visible = false;
			break;
		}
		level_text.SetText(level.ToString());
		board_default_x = select_board.Rect.x;
		text_default_x = level_text.Rect.x;
	}

	public void UISlip(float delta_slip)
	{
		Rect rect = select_board.Rect;
		select_board.Rect = new Rect(board_default_x + delta_slip, rect.y, rect.width, rect.height);
		rect = level_text.Rect;
		level_text.Rect = new Rect(text_default_x + delta_slip, rect.y, rect.width, rect.height);
	}

	public bool Click(Vector2 position)
	{
		if (!level_text.Visible)
		{
			return false;
		}
		if (select_board.PtInRect(position))
		{
			return true;
		}
		return false;
	}

	public int GetLevel()
	{
		return int.Parse(level_text.GetText());
	}
}
