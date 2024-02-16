using UnityEngine;

public class D3DIapButton : D3DCustomUI
{
	private UIPushButton iap_button;

	private UIImage iap_icon;

	private UIText iap_title;

	private UIText iap_price;

	private D3DImageCell[] Icon_cell;

	public UIPushButton IapButton
	{
		get
		{
			return iap_button;
		}
	}

	public D3DIapButton(UIManager manager, UIHelper helper, Vector2 position, D3DGamer.IapMenu iap)
		: base(manager, helper)
	{
		ui_position = position;
		Vector2[] array = new Vector2[10]
		{
			new Vector2(19f, 18f),
			new Vector2(16f, 16f),
			new Vector2(10f, 16f),
			new Vector2(8f, 16f),
			new Vector2(5f, 15f),
			new Vector2(7f, 13f),
			new Vector2(18f, 13f),
			new Vector2(11f, 13f),
			new Vector2(0f, 13f),
			new Vector2(3f, 9f)
		};
		string[,] array2 = new string[10, 2]
		{
			{ "yihaoshuijing-1", "yihaoshuijing-2" },
			{ "erhaoshuijing-1", "erhaoshuijing-2" },
			{ "sanhaoshuijing-1", "sanhaoshuijing-2" },
			{ "sihaoshuijing-1", "sihaoshuijing-2" },
			{ "wuhaoshuijing-1", "wuhaoshuijing-2" },
			{ "xinshoubao-1", "xinshoubao-2" },
			{ "gold-1", "gold-1-2" },
			{ "gold-3", "gold-3-2" },
			{ "gold-5", "gold-5-2" },
			{ "VIP-1", "VIP-2" }
		};
		iap_button = new UIPushButton();
		D3DImageCell imageCell = ui_helper.GetImageCell("danyuankuang-1");
		iap_button.SetTexture(UIButtonBase.State.Normal, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		iap_button.SetTexture(UIButtonBase.State.Disabled, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = ui_helper.GetImageCell("danyuankuang-2");
		iap_button.SetTexture(UIButtonBase.State.Pressed, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		iap_button.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, imageCell.cell_rect.width, imageCell.cell_rect.height);
		manager.Add(iap_button);
		Icon_cell = new D3DImageCell[2]
		{
			ui_helper.GetImageCell(array2[(int)iap, 0]),
			ui_helper.GetImageCell(array2[(int)iap, 1])
		};
		iap_icon = new UIImage();
		iap_icon.Enable = false;
		iap_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(Icon_cell[0].cell_texture), D3DMain.Instance.ConvertRectAutoHD(Icon_cell[0].cell_rect));
		iap_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + array[(int)iap].x, ui_position.y + array[(int)iap].y, Icon_cell[0].cell_rect.width, Icon_cell[0].cell_rect.height);
		manager.Add(iap_icon);
		iap_title = new UIText();
		iap_title.Enable = false;
		iap_title.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 6), "\n" + D3DTexts.Instance.GetTBankName(iap), D3DMain.Instance.CommonFontColor);
		iap_title.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(3 * D3DMain.Instance.HD_SIZE);
		iap_title.LineSpacing = -6.5f * (float)D3DMain.Instance.HD_SIZE;
		iap_title.AlignStyle = UIText.enAlignStyle.center;
		iap_title.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 3f, ui_position.y + 64f, 80f, 30f);
		manager.Add(iap_title);
		UIImage uIImage = null;
		string text = string.Empty;
		float num = 2f;
		switch (iap)
		{
		case D3DGamer.IapMenu.IAP_499:
			text = "$4.99";
			break;
		case D3DGamer.IapMenu.IAP_999:
			text = "$9.99";
			break;
		case D3DGamer.IapMenu.IAP_1999:
			text = "$19.99";
			break;
		case D3DGamer.IapMenu.IAP_4999:
			text = "$49.99";
			break;
		case D3DGamer.IapMenu.IAP_9999:
			text = "$99.99";
			break;
		case D3DGamer.IapMenu.IAP_NEWBIE:
			text = "$2.99";
			break;
		case D3DGamer.IapMenu.IAP_5T:
			uIImage = new UIImage();
			imageCell = ui_helper.GetImageCell("shuijing");
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(10f, 16f) * D3DMain.Instance.HD_SIZE);
			uIImage.Enable = false;
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 30f, ui_position.y + 5f, 10f, 16f);
			manager.Add(uIImage);
			num = 8f;
			text = "5";
			break;
		case D3DGamer.IapMenu.IAP_30T:
			uIImage = new UIImage();
			imageCell = ui_helper.GetImageCell("shuijing");
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(10f, 16f) * D3DMain.Instance.HD_SIZE);
			uIImage.Enable = false;
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 25f, ui_position.y + 5f, 10f, 16f);
			manager.Add(uIImage);
			num = 9f;
			text = "30";
			break;
		case D3DGamer.IapMenu.IAP_100T:
			uIImage = new UIImage();
			imageCell = ui_helper.GetImageCell("shuijing");
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(10f, 16f) * D3DMain.Instance.HD_SIZE);
			uIImage.Enable = false;
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 23f, ui_position.y + 5f, 10f, 16f);
			manager.Add(uIImage);
			num = 9f;
			text = "100";
			break;
		case D3DGamer.IapMenu.IAP_VIP:
			uIImage = new UIImage();
			imageCell = ui_helper.GetImageCell("shuijing");
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(10f, 16f) * D3DMain.Instance.HD_SIZE);
			uIImage.Enable = false;
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 25f, ui_position.y + 5f, 10f, 16f);
			manager.Add(uIImage);
			num = 9f;
			text = "30";
			break;
		}
		iap_price = new UIText();
		iap_price.Enable = false;
		iap_price.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 7), text, D3DMain.Instance.CommonFontColor);
		iap_price.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(7 * D3DMain.Instance.HD_SIZE);
		iap_price.AlignStyle = UIText.enAlignStyle.center;
		iap_price.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + num, ui_position.y - 3f, 80f, 20f);
		manager.Add(iap_price);
	}

	public void Select(bool select)
	{
		int num;
		Color color;
		if (select)
		{
			num = 1;
			color = Color.white;
		}
		else
		{
			num = 0;
			color = D3DMain.Instance.CommonFontColor;
		}
		iap_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(Icon_cell[num].cell_texture), D3DMain.Instance.ConvertRectAutoHD(Icon_cell[num].cell_rect));
		iap_title.SetColor(color);
		iap_price.SetColor(color);
		iap_button.Set(select);
	}

	public void Disable()
	{
		iap_button.Enable = false;
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = ui_helper.GetImageCell("sold-out");
		uIImage.Enable = false;
		uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 1f, ui_position.y + 30f, imageCell.cell_rect.width, imageCell.cell_rect.height);
		ui_manager.Add(uIImage);
	}
}
