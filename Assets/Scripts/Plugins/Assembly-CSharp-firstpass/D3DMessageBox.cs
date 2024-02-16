using System.Collections.Generic;
using UnityEngine;

public class D3DMessageBox : D3DCustomUI
{
	public enum MgbButton
	{
		OK = 0,
		OK_CANCEL = 1,
		CANCEL_OK = 2
	}

	private static string mgb_image_path;

	private static string mgb_font_path;

	private static Dictionary<string, D3DImageCell> mgb_image_cells;

	private Vector2 box_size;

	private Vector2 box_offset;

	public D3DMessageBox(UIManager manager, UIHelper helper, bool full_screen = false)
		: base(manager, helper)
	{
		if (full_screen)
		{
			manager.SetSpriteCameraViewPort(new Rect(0f - manager.ScreenOffset.x, 0f - manager.ScreenOffset.y, GameScreen.width, GameScreen.height));
			box_size = new Vector2(GameScreen.width, GameScreen.height) * (1f / (float)D3DMain.Instance.HD_SIZE);
			box_offset = manager.ScreenOffset * (1f / (float)D3DMain.Instance.HD_SIZE);
		}
		else
		{
			box_size = new Vector2(480f, 320f);
			box_offset = Vector2.zero;
		}
		ui_position = Vector2.zero;
		string text = ((D3DMain.Instance.HD_SIZE != 2) ? "_M" : "_hd_M");
		D3DImageCell d3DImageCell = mgb_image_cells["ui_monolayer"];
		UIImage uIImage = new UIImage();
		Material material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect), box_size * D3DMain.Instance.HD_SIZE);
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, box_size.x, box_size.y);
		uIImage.SetColor(new Color(0f, 0f, 0f, 0.85f));
		ui_manager.Add(uIImage);
		UIImage uIImage2 = new UIImage();
		d3DImageCell = mgb_image_cells["tankuang5"];
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect), new Vector2(4f, 160f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 80f + box_offset.x, ui_position.y + 81f + box_offset.y, 4f, 160f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect), new Vector2(4f, 160f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 395.5f + box_offset.x, ui_position.y + 81f + box_offset.y, 4f, 160f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		d3DImageCell = mgb_image_cells["tankuang7"];
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect), new Vector2(300f, 4f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 88f + box_offset.x, ui_position.y + 70.5f + box_offset.y, 300f, 4f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect), new Vector2(300f, 4f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 88f + box_offset.x, ui_position.y + 246f + box_offset.y, 300f, 4f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		d3DImageCell = mgb_image_cells["tankuang3"];
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 80f + box_offset.x, ui_position.y + 70f + box_offset.y, 17f, 18f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		d3DImageCell = mgb_image_cells["tankuang4"];
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 80f + box_offset.x, ui_position.y + 232f + box_offset.y, 17f, 18f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		d3DImageCell = mgb_image_cells["tankuang1"];
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 383f + box_offset.x, ui_position.y + 70f + box_offset.y, 17f, 18f);
		ui_manager.Add(uIImage2);
		uIImage2 = new UIImage();
		d3DImageCell = mgb_image_cells["tankuang2"];
		material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
		uIImage2.SetTexture(material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 383f + box_offset.x, ui_position.y + 232f + box_offset.y, 17f, 18f);
		ui_manager.Add(uIImage2);
	}

	public static void InitMgbResources()
	{
		mgb_image_path = "Dungeons3D/Images/UIImages/";
		mgb_font_path = "Dungeons3D/Fonts/";
		mgb_image_cells = new Dictionary<string, D3DImageCell>();
		string[] array = new string[2] { "UImg1_cell", "UI_Monolayer_cell" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			D3DMain.Instance.LoadD3DImageCell(ref mgb_image_cells, "Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UIImgCell", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt(text, D3DGamer.Instance.Sk[0])));
		}
	}

	public void ShowMessageBox(List<string> mgb_content, MgbButton button_type, List<D3DMessageBoxButtonEvent.OnButtonClick> events, Dictionary<int, Color> content_color = null)
	{
		int num = 0;
		string font = mgb_font_path + D3DMain.Instance.GameFont1.FontName + 8 * D3DMain.Instance.HD_SIZE;
		float charSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
		Color commonFontColor = D3DMain.Instance.CommonFontColor;
		foreach (string item in mgb_content)
		{
			UIText uIText = new UIText();
			uIText.Set(font, item, commonFontColor);
			uIText.AlignStyle = UIText.enAlignStyle.center;
			uIText.CharacterSpacing = charSpacing;
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 92f + box_offset.x, ui_position.y + 205f - (float)(30 * num) + box_offset.y, 295f, 20f);
			if (content_color != null && content_color.ContainsKey(num))
			{
				uIText.SetColor(content_color[num]);
			}
			ui_manager.Add(uIText);
			num++;
		}
		string text = ((D3DMain.Instance.HD_SIZE != 2) ? "_M" : "_hd_M");
		font = mgb_font_path + D3DMain.Instance.GameFont1.FontName + 9 * D3DMain.Instance.HD_SIZE;
		charSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		switch (button_type)
		{
		case MgbButton.OK:
		{
			UIClickButton uIClickButton = new UIClickButton();
			D3DImageCell d3DImageCell = mgb_image_cells["anniu1"];
			Material material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
			uIClickButton.SetTexture(UIButtonBase.State.Normal, material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
			d3DImageCell = mgb_image_cells["anniu2"];
			material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
			uIClickButton.SetTexture(UIButtonBase.State.Pressed, material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
			uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 198f + box_offset.x, ui_position.y + 80f + box_offset.y, 84f, 37f);
			ui_manager.Add(uIClickButton);
			Dictionary<UIControl, D3DMessageBoxButtonEvent.OnButtonClick> dictionary = new Dictionary<UIControl, D3DMessageBoxButtonEvent.OnButtonClick>();
			if (events == null)
			{
				dictionary.Add(uIClickButton, null);
			}
			else
			{
				dictionary.Add(uIClickButton, events[0]);
			}
			new D3DMessageBoxButtonEvent(ui_helper, ui_manager, dictionary);
			UIText uIText2 = new UIText();
			uIText2.Set(font, "OK", commonFontColor);
			uIText2.AlignStyle = UIText.enAlignStyle.center;
			uIText2.CharacterSpacing = charSpacing;
			uIText2.Enable = false;
			uIText2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 198f + box_offset.x, ui_position.y + 68f + box_offset.y, 84f, 37f);
			ui_manager.Add(uIText2);
			break;
		}
		case MgbButton.OK_CANCEL:
		case MgbButton.CANCEL_OK:
		{
			UIClickButton uIClickButton = new UIClickButton();
			D3DImageCell d3DImageCell = mgb_image_cells["anniu1"];
			Material material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
			uIClickButton.SetTexture(UIButtonBase.State.Normal, material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
			d3DImageCell = mgb_image_cells["anniu2"];
			material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
			uIClickButton.SetTexture(UIButtonBase.State.Pressed, material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
			if (button_type == MgbButton.OK_CANCEL)
			{
				uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 100f + box_offset.x, ui_position.y + 80f + box_offset.y, 84f, 37f);
			}
			else
			{
				uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 295f + box_offset.x, ui_position.y + 80f + box_offset.y, 84f, 37f);
			}
			ui_manager.Add(uIClickButton);
			Dictionary<UIControl, D3DMessageBoxButtonEvent.OnButtonClick> dictionary = new Dictionary<UIControl, D3DMessageBoxButtonEvent.OnButtonClick>();
			if (events == null)
			{
				dictionary.Add(uIClickButton, null);
			}
			else
			{
				dictionary.Add(uIClickButton, events[0]);
			}
			uIClickButton = new UIClickButton();
			d3DImageCell = mgb_image_cells["anniu1"];
			material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
			uIClickButton.SetTexture(UIButtonBase.State.Normal, material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
			d3DImageCell = mgb_image_cells["anniu2"];
			material = Resources.Load(mgb_image_path + d3DImageCell.cell_texture + text) as Material;
			uIClickButton.SetTexture(UIButtonBase.State.Pressed, material, D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
			if (button_type == MgbButton.OK_CANCEL)
			{
				uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 295f + box_offset.x, ui_position.y + 80f + box_offset.y, 84f, 37f);
			}
			else
			{
				uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 100f + box_offset.x, ui_position.y + 80f + box_offset.y, 84f, 37f);
			}
			ui_manager.Add(uIClickButton);
			if (events == null || events.Count < 2)
			{
				dictionary.Add(uIClickButton, null);
			}
			else
			{
				dictionary.Add(uIClickButton, events[1]);
			}
			new D3DMessageBoxButtonEvent(ui_helper, ui_manager, dictionary);
			UIText uIText2 = new UIText();
			uIText2.AlignStyle = UIText.enAlignStyle.center;
			uIText2.CharacterSpacing = charSpacing;
			uIText2.Set(font, "OK", commonFontColor);
			uIText2.Enable = false;
			if (button_type == MgbButton.OK_CANCEL)
			{
				uIText2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 100f + box_offset.x, ui_position.y + 68f + box_offset.y, 84f, 37f);
			}
			else
			{
				uIText2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 297f + box_offset.x, ui_position.y + 68f + box_offset.y, 84f, 37f);
			}
			ui_manager.Add(uIText2);
			uIText2 = new UIText();
			uIText2.AlignStyle = UIText.enAlignStyle.center;
			uIText2.CharacterSpacing = charSpacing;
			uIText2.Set(font, "CANCEL", commonFontColor);
			uIText2.Enable = false;
			if (button_type == MgbButton.OK_CANCEL)
			{
				uIText2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 302f + box_offset.x, ui_position.y + 68f + box_offset.y, 84f, 37f);
			}
			else
			{
				uIText2.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + 104f + box_offset.x, ui_position.y + 68f + box_offset.y, 84f, 37f);
			}
			ui_manager.Add(uIText2);
			break;
		}
		}
	}
}
