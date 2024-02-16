using UnityEngine;

public class D3DCurrencyText : D3DCustomUI
{
	private UIImage[] currency_icon;

	private UIText[] currency_text;

	private Color font_color;

	private Vector2[] icon_offset;

	private bool[] enable;

	private bool ui_visible = true;

	public bool EnableGold
	{
		get
		{
			return enable[0];
		}
		set
		{
			enable[0] = value;
			Update();
		}
	}

	public bool EnableCrystal
	{
		get
		{
			return enable[1];
		}
		set
		{
			enable[1] = value;
			Update();
		}
	}

	public D3DCurrencyText(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
		ui_position = Vector2.zero;
		currency_icon = new UIImage[2];
		icon_offset = new Vector2[2]
		{
			new Vector2(22f, 1f),
			new Vector2(17f, 0f)
		};
		enable = new bool[2] { true, true };
		D3DImageCell imageCell = helper.GetImageCell("jinbi");
		currency_icon[0] = new UIImage();
		currency_icon[0].SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		currency_icon[0].Enable = false;
		currency_icon[0].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 20f, 20f);
		manager.Add(currency_icon[0]);
		imageCell = helper.GetImageCell("shuijing");
		currency_icon[1] = new UIImage();
		currency_icon[1].SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		currency_icon[1].Enable = false;
		currency_icon[1].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 15f, 24f);
		manager.Add(currency_icon[1]);
		font_color = D3DMain.Instance.CommonFontColor;
		currency_text = new UIText[2];
		currency_text[0] = new UIText();
		currency_text[0].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		currency_text[0].Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, font_color);
		currency_text[0].Enable = false;
		currency_text[0].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 200f, 18f);
		ui_manager.Add(currency_text[0]);
		currency_text[1] = new UIText();
		currency_text[1].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		currency_text[1].Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, font_color);
		currency_text[1].Enable = false;
		currency_text[1].Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, 200f, 18f);
		ui_manager.Add(currency_text[1]);
		GotTapPointsMono.CurrencyTextList.Add(this);
	}

	public void Update()
	{
		float num = 0f;
		if (enable[0])
		{
			currency_icon[0].Rect = new Rect(ui_position.x * (float)D3DMain.Instance.HD_SIZE, (ui_position.y + icon_offset[0].y) * (float)D3DMain.Instance.HD_SIZE, currency_icon[0].Rect.width, currency_icon[0].Rect.height);
			currency_text[0].Rect = new Rect((ui_position.x + icon_offset[0].x) * (float)D3DMain.Instance.HD_SIZE, ui_position.y * (float)D3DMain.Instance.HD_SIZE, currency_text[0].Rect.width, currency_text[0].Rect.height);
			num = (icon_offset[0].x + 2f) * (float)D3DMain.Instance.HD_SIZE + currency_text[0].GetLinesMaxWidth();
		}
		bool visible = (ui_visible || enable[0]) && ui_visible && enable[0];
		currency_icon[0].Visible = visible;
		currency_text[0].Visible = visible;
		if (enable[1])
		{
			currency_icon[1].Rect = new Rect(num + ui_position.x * (float)D3DMain.Instance.HD_SIZE, (ui_position.y + icon_offset[1].y) * (float)D3DMain.Instance.HD_SIZE, currency_icon[1].Rect.width, currency_icon[1].Rect.height);
			currency_text[1].Rect = new Rect(num + (ui_position.x + icon_offset[1].x) * (float)D3DMain.Instance.HD_SIZE, ui_position.y * (float)D3DMain.Instance.HD_SIZE, currency_text[1].Rect.width, currency_text[1].Rect.height);
		}
		visible = (ui_visible || enable[1]) && ui_visible && enable[1];
		currency_icon[1].Visible = visible;
		currency_text[1].Visible = visible;
	}

	public void Visible(bool visible)
	{
		ui_visible = visible;
		visible = (ui_visible || enable[0]) && ui_visible && enable[0];
		currency_icon[0].Visible = visible;
		currency_text[0].Visible = visible;
		visible = (ui_visible || enable[1]) && ui_visible && enable[1];
		currency_icon[1].Visible = visible;
		currency_text[1].Visible = visible;
	}

	public void DefaultColor()
	{
		currency_text[0].SetColor(font_color);
		currency_text[1].SetColor(font_color);
	}

	public void SetColor(Color color)
	{
		currency_text[0].SetColor(color);
		currency_text[1].SetColor(color);
	}

	public void SetGoldColor(Color color)
	{
		currency_text[0].SetColor(color);
	}

	public void SetCrystalColor(Color color)
	{
		currency_text[1].SetColor(color);
	}

	public void SetPosition(Vector2 position)
	{
		ui_position = position;
		Update();
	}

	public float GetUIWidth()
	{
		float num = 0f;
		if (enable[0])
		{
			num = icon_offset[0].x * (float)D3DMain.Instance.HD_SIZE + currency_text[0].GetLinesMaxWidth();
		}
		if (enable[1])
		{
			num += (2f + icon_offset[1].x) * (float)D3DMain.Instance.HD_SIZE + currency_text[1].GetLinesMaxWidth();
		}
		return num;
	}

	public void SetCurrency(int gold, int crystal)
	{
		currency_text[0].SetText(gold.ToString());
		currency_text[1].SetText(crystal.ToString());
	}

	public void SetCurrency(string gold, string crystal)
	{
		currency_text[0].SetText(gold);
		currency_text[1].SetText(crystal);
	}

	public void SetGold(int count)
	{
		currency_text[0].SetText(count.ToString());
	}

	public void SetGold(string count)
	{
		currency_text[0].SetText(count);
	}

	public void SetCrystal(int count)
	{
		currency_text[1].SetText(count.ToString());
	}

	public void SetCrystal(string count)
	{
		currency_text[1].SetText(count);
	}
}
