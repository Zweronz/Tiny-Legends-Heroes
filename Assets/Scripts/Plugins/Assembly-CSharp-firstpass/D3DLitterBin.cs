using UnityEngine;

public class D3DLitterBin : D3DCustomUI
{
	private UIImage bin_board;

	private UIImage bin_image;

	private Material normal_bin_mat;

	private Material hover_bin_mat;

	private Rect normal_bin_texture_rect;

	private Rect hover_bin_texture_rect;

	private bool hover_state;

	public D3DLitterBin(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateLitterBin(Vector2 position)
	{
		ui_position = position;
		bin_board = new UIImage();
		D3DImageCell imageCell = ui_helper.GetImageCell("touxiangkuang");
		bin_board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(50f, 50f) * D3DMain.Instance.HD_SIZE);
		bin_board.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 50f, 50f);
		bin_board.Enable = false;
		ui_manager.Add(bin_board);
		bin_image = new UIImage();
		imageCell = ui_helper.GetImageCell("lajitong");
		normal_bin_mat = ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture);
		normal_bin_texture_rect = D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect);
		imageCell = ui_helper.GetImageCell("lajitong-tuozhuaixiaoguo");
		hover_bin_mat = ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture);
		hover_bin_texture_rect = D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect);
		bin_image.SetTexture(normal_bin_mat, normal_bin_texture_rect);
		bin_image.Rect = bin_board.Rect;
		bin_image.Enable = false;
		ui_manager.Add(bin_image);
	}

	public void Hover(bool hover)
	{
		if (!hover_state && hover)
		{
			hover_state = true;
			bin_image.SetTexture(hover_bin_mat, hover_bin_texture_rect);
		}
		else if (hover_state && !hover)
		{
			hover_state = false;
			bin_image.SetTexture(normal_bin_mat, normal_bin_texture_rect);
		}
	}

	public void Visible(bool visible)
	{
		bin_board.Visible = visible;
		bin_image.Visible = visible;
	}
}
