using UnityEngine;

public class D3DTextPushButton : D3DCustomUI
{
	private UIPushButton push_btn;

	private UIText btn_text;

	private Color normal_text_color;

	private Color press_text_color;

	private Color disable_text_color;

	public UIPushButton PushBtn
	{
		get
		{
			return push_btn;
		}
	}

	public D3DTextPushButton(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateControl(Vector2 position, Vector2 size, string normal_cell, string press_cell, string disable_cell, string font_name, int normal_size, int hd_size, string text_content, Vector2 text_offset, float char_spacing, Color normal_text_color, Color press_text_color, Color disable_text_color)
	{
		ui_position = position;
		push_btn = new UIPushButton();
		if (string.Empty != normal_cell)
		{
			D3DImageCell imageCell = ui_helper.GetImageCell(normal_cell);
			push_btn.SetTexture(UIButtonBase.State.Normal, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), size * D3DMain.Instance.HD_SIZE);
		}
		if (string.Empty != press_cell)
		{
			D3DImageCell imageCell2 = ui_helper.GetImageCell(press_cell);
			push_btn.SetTexture(UIButtonBase.State.Pressed, ui_helper.LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect), size * D3DMain.Instance.HD_SIZE);
		}
		if (string.Empty != disable_cell)
		{
			D3DImageCell imageCell3 = ui_helper.GetImageCell(disable_cell);
			push_btn.SetTexture(UIButtonBase.State.Disabled, ui_helper.LoadUIMaterialAutoHD(imageCell3.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell3.cell_rect), size * D3DMain.Instance.HD_SIZE);
		}
		push_btn.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x, ui_position.y, size.x, size.y));
		ui_manager.Add(push_btn);
		this.normal_text_color = normal_text_color;
		this.press_text_color = press_text_color;
		this.disable_text_color = disable_text_color;
		btn_text = new UIText();
		btn_text.Set(ui_helper.LoadFont(font_name, normal_size, hd_size), text_content, this.normal_text_color);
		btn_text.CharacterSpacing = char_spacing;
		btn_text.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x, ui_position.y, 999f, 999f));
		Vector2 vector = new Vector2(btn_text.GetTextWidth(), btn_text.GetLinesTotalHeight());
		btn_text.Rect = new Rect((ui_position.x + size.x * 0.5f) * (float)D3DMain.Instance.HD_SIZE - vector.x * 0.5f + text_offset.x, (ui_position.y + size.y * 0.5f) * (float)D3DMain.Instance.HD_SIZE - vector.y * 0.5f + text_offset.y, vector.x, vector.y);
		btn_text.Enable = false;
		ui_manager.Add(btn_text);
	}

	public void Enable(bool enable)
	{
		push_btn.Enable = enable;
		btn_text.SetColor((!enable) ? disable_text_color : normal_text_color);
	}

	public void Set(bool set)
	{
		push_btn.Set(set);
		btn_text.SetColor((!set) ? normal_text_color : press_text_color);
	}

	public void Visible(bool visible)
	{
		push_btn.Visible = visible;
		btn_text.Visible = visible;
	}
}
