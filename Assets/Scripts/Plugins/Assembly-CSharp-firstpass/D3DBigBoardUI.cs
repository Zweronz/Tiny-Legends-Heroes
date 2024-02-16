using UnityEngine;

public class D3DBigBoardUI : D3DCustomUI
{
	private UIImage[] board_corner;

	private UIImage[] board_frame;

	public D3DBigBoardUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateBigBoard(Vector2 position)
	{
		board_corner = new UIImage[4];
		board_frame = new UIImage[4];
		string[] array = new string[4] { "dakuang2", "dakuang1", "dakuang3", "dakuang4" };
		string[] array2 = new string[4] { "dakuang6", "dakuang5", "dakuang7", "dakuang8" };
		for (int i = 0; i < 4; i++)
		{
			board_corner[i] = new UIImage();
			D3DImageCell imageCell = ui_helper.GetImageCell(array[i]);
			board_corner[i].SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			board_corner[i].Enable = false;
			ui_manager.Add(board_corner[i]);
			board_frame[i] = new UIImage();
			imageCell = ui_helper.GetImageCell(array2[i]);
			board_frame[i].SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			board_frame[i].Enable = false;
			ui_manager.Add(board_frame[i]);
		}
		ui_position = position;
		board_corner[0].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x, ui_position.y + 263f, 32f, 32f));
		board_corner[1].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 399.5f, ui_position.y + 263f, 32f, 32f));
		board_corner[2].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x, ui_position.y, 32f, 32f));
		board_corner[3].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 399f, ui_position.y, 32f, 32f));
		board_frame[0].SetTextureSize(new Vector2(16f, 232f) * D3DMain.Instance.HD_SIZE);
		board_frame[0].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 2.5f, ui_position.y + 32f, 16f, 232f));
		board_frame[1].SetTextureSize(new Vector2(16f, 232f) * D3DMain.Instance.HD_SIZE);
		board_frame[1].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 415f, ui_position.y + 32f, 16f, 232f));
		board_frame[2].SetTextureSize(new Vector2(368f, 16f) * D3DMain.Instance.HD_SIZE);
		board_frame[2].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 32f, ui_position.y + 279f, 368f, 16f));
		board_frame[3].SetTextureSize(new Vector2(368f, 16f) * D3DMain.Instance.HD_SIZE);
		board_frame[3].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 32f, ui_position.y + 1f, 368f, 16f));
	}

	public void CreateBigBoard(Rect board_rect)
	{
		board_corner = new UIImage[4];
		board_frame = new UIImage[4];
		string[] array = new string[4] { "dakuang2", "dakuang1", "dakuang3", "dakuang4" };
		string[] array2 = new string[4] { "dakuang6", "dakuang5", "dakuang7", "dakuang8" };
		for (int i = 0; i < 4; i++)
		{
			board_corner[i] = new UIImage();
			D3DImageCell imageCell = ui_helper.GetImageCell(array[i]);
			board_corner[i].SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			board_corner[i].Enable = false;
			ui_manager.Add(board_corner[i]);
			board_frame[i] = new UIImage();
			imageCell = ui_helper.GetImageCell(array2[i]);
			board_frame[i].SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			board_frame[i].Enable = false;
			ui_manager.Add(board_frame[i]);
		}
		ui_position = new Vector2(board_rect.x, board_rect.y);
		float num = board_rect.width - 64f;
		if (num < 0f)
		{
			num = 0f;
		}
		float num2 = board_rect.height - 64f;
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		board_corner[0].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x, ui_position.y + num2 + 32f, 32f, 32f));
		board_corner[1].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + num + 32.5f, ui_position.y + num2 + 32f, 32f, 32f));
		board_corner[2].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x, ui_position.y, 32f, 32f));
		board_corner[3].Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + num + 32f, ui_position.y, 32f, 32f));
		board_frame[0].SetTextureSize(new Vector2(16f, num2 + 1f) * D3DMain.Instance.HD_SIZE);
		board_frame[0].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 2.5f, ui_position.y + 32f, 16f, num2 + 1f);
		board_frame[1].SetTextureSize(new Vector2(16f, num2 + 1f) * D3DMain.Instance.HD_SIZE);
		board_frame[1].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + num + 48f, ui_position.y + 32f, 16f, num2 + 1f);
		board_frame[2].SetTextureSize(new Vector2(num + 1f, 16f) * D3DMain.Instance.HD_SIZE);
		board_frame[2].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 32f, ui_position.y + num2 + 48f, num + 1f, 16f);
		board_frame[3].SetTextureSize(new Vector2(num + 1f, 16f) * D3DMain.Instance.HD_SIZE);
		board_frame[3].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 32f, ui_position.y + 1f, num + 1f, 16f);
	}
}
