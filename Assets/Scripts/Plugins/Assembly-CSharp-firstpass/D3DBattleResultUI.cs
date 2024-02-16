using UnityEngine;

public class D3DBattleResultUI : D3DCustomUI
{
	private D3DCurrencyText PlayerCurrency;

	private D3DCurrencyText GoldBonus;

	public D3DBattleResultUI(UIManager manager, UIHelper helper, Vector2 position)
		: base(manager, helper)
	{
		ui_position = position;
		Vector2 vector = new Vector2(960f, 640f);
		D3DImageCell imageCell = ui_helper.GetImageCell("ui_monolayer");
		UIImage uIImage = new UIImage();
		uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(vector.x, vector.y) * D3DMain.Instance.HD_SIZE);
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, vector.x, vector.y);
		uIImage.SetColor(new Color(0f, 0f, 0f, 0.8f));
		ui_manager.Add(uIImage);
		PlayerCurrency = new D3DCurrencyText(ui_manager, ui_helper);
		UIImage uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang5");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(4f, 165f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 70f, ui_position.y + 76f, 4f, 165f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(4f, 165f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 405.5f, ui_position.y + 76f, 4f, 165f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang7");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(83f, 4f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 78f, ui_position.y + 65.5f, 83f, 4f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(83f, 4f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 314.5f, ui_position.y + 65.5f, 83f, 4f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(320f, 4f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 78f, ui_position.y + 246f, 320f, 4f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang9");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 159.5f, ui_position.y + 45.5f, 157f, 25f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang3");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 70f, ui_position.y + 65f, 17f, 18f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang4");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 70f, ui_position.y + 232f, 17f, 18f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang1");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 393f, ui_position.y + 65f, 17f, 18f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		imageCell = ui_helper.GetImageCell("tankuang2");
		uIImage2.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 393f, ui_position.y + 232f, 17f, 18f);
		ui_manager.Add(uIImage2);
		UIImage uIImage3 = new UIImage();
		ui_helper.AddControlToTable("ResultTitle", uIImage3);
		uIImage3.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 77.5f, ui_position.y + 230.5f, 325f, 50f);
		ui_manager.Add(uIImage3);
		UIClickButton uIClickButton = new UIClickButton();
		ui_helper.AddControlToTable("ResultButton", uIClickButton);
		imageCell = ui_helper.GetImageCell("anniu1");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(92f, 37f) * D3DMain.Instance.HD_SIZE);
		imageCell = ui_helper.GetImageCell("anniu2");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(92f, 37f) * D3DMain.Instance.HD_SIZE);
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 193f, ui_position.y + 56f, 92f, 37f);
		ui_manager.Add(uIClickButton);
		UIText uIText = new UIText();
		ui_helper.AddControlToTable("ResultButtonText", uIText);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
		uIText.AlignStyle = UIText.enAlignStyle.center;
		uIText.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), "OK", D3DMain.Instance.CommonFontColor);
		uIText.Enable = false;
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 195f, ui_position.y + 62f, 92f, 20f);
		ui_manager.Add(uIText);
	}

	public void SetResultUI(bool win, int gold_bonus = 0)
	{
		UIImage uIImage = (UIImage)ui_helper.GetControl("ResultTitle");
		if (win)
		{
			Vector2 vector = new Vector2(ui_manager.GetManagerCamera().pixelWidth * (1f / (float)D3DMain.Instance.HD_SIZE), ui_manager.GetManagerCamera().pixelHeight * (1f / (float)D3DMain.Instance.HD_SIZE));
			if (D3DGamer.Instance.TutorialState[0])
			{
				int num = int.Parse(D3DGamer.Instance.CurrencyText) - gold_bonus;
				GoldBonus = new D3DCurrencyText(ui_manager, ui_helper);
				GoldBonus.EnableCrystal = false;
				string text = gold_bonus.ToString();
				if (D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
				{
					int num2 = Mathf.RoundToInt((float)gold_bonus * D3DGamer.Instance.GoldBonus);
					if (num2 < 1)
					{
						num2 = 1;
					}
					num -= num2;
					string text2 = text;
					text = text2 + " + " + num2 + " VIP BONUS";
				}
				GoldBonus.SetGold(text);
				GoldBonus.SetPosition(new Vector2(ui_position.x + 240f - GoldBonus.GetUIWidth() * 0.5f * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y + 180f));
				GoldBonus.Visible(false);
				PlayerCurrency.SetGold(num);
				PlayerCurrency.SetCrystal(D3DGamer.Instance.CrystalText);
				PlayerCurrency.SetPosition(new Vector2((float)GameScreen.width - PlayerCurrency.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), GameScreen.height - 27));
			}
			else
			{
				PlayerCurrency.SetGold(D3DGamer.Instance.CurrencyText);
				PlayerCurrency.SetCrystal(D3DGamer.Instance.CrystalText);
				PlayerCurrency.SetPosition(new Vector2((float)GameScreen.width - PlayerCurrency.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), GameScreen.height - 27));
			}
			D3DImageCell imageCell = ui_helper.GetImageCell("biaoti-win");
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			((UIText)ui_helper.GetControl("ResultButtonText")).SetText("SKIP");
		}
		else
		{
			PlayerCurrency.Visible(false);
			UIText uIText = new UIText();
			ui_helper.AddControlToTable("LootText", uIText);
			uIText.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 9), "You wake up back home with a pounding headache. Ouch.", new Color(9f / 85f, 29f / 51f, 0.3254902f));
			uIText.AlignStyle = UIText.enAlignStyle.center;
			uIText.Enable = false;
			uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 77.5f, ui_position.y + 135f, 325f, 50f);
			ui_manager.Add(uIText);
			D3DImageCell imageCell = ui_helper.GetImageCell("biaoti-lose");
			uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		}
	}

	public void UpdateCurrency()
	{
		Vector2 vector = new Vector2(ui_manager.GetManagerCamera().pixelWidth * (1f / (float)D3DMain.Instance.HD_SIZE), ui_manager.GetManagerCamera().pixelHeight * (1f / (float)D3DMain.Instance.HD_SIZE));
		if (int.Parse(D3DGamer.Instance.CurrencyText) > 9999999)
		{
			D3DGamer.Instance.UpdateCurrency(9999999);
		}
		PlayerCurrency.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		PlayerCurrency.SetPosition(new Vector2((float)GameScreen.width - PlayerCurrency.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), GameScreen.height - 27));
	}

	public void EnableLootUI()
	{
		UIImage uIImage = new UIImage();
		uIImage.Enable = false;
		D3DImageCell imageCell = ui_helper.GetImageCell("LOOT");
		uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 107f, ui_position.y + 198f, 260f, 35f);
		ui_manager.Add(uIImage);
		UpdateCurrency();
		if (GoldBonus != null)
		{
			GoldBonus.Visible(true);
			if (D3DMain.Instance.LootEquipments.Count == 0)
			{
				GoldBonus.SetPosition(new Vector2(ui_position.x + 240f - GoldBonus.GetUIWidth() * 0.5f * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y + 140f));
			}
		}
		((UIText)ui_helper.GetControl("ResultButtonText")).SetText("OK");
		D3DGamer.Instance.SaveAllData();
	}
}
