using System;
using UnityEngine;

public class UIHowTo : UIHelper
{
	private enum HowToUIManager
	{
		MAIN = 0
	}

	private string[] HowToTitles;

	private D3DTextPushButton[] OptionBtns;

	private int CurrentOptionIndex;

	private D3DHowToSlipUI HowToSlipUI;

	private UIImage[] IllArrow;

	private UIMove HowToIllMove;

	private UIText PageText;

	private void CreateMainUI()
	{
		CreateUIManager("Manager_Main");
		CreateUIManager("Manager_Slip");
		m_UIManagerRef[1].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(50f, 25f, 377f, 251f));
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = GetImageCell("ui_monolayer");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		uIImage.SetColor(new Color(0f, 0f, 0f, 0.7f));
		m_UIManagerRef[0].Add(uIImage);
		HowToTitles = D3DHowTo.Instance.GetHowToTitles();
		OptionBtns = new D3DTextPushButton[HowToTitles.Length];
		for (int i = 0; i < OptionBtns.Length; i++)
		{
			OptionBtns[i] = new D3DTextPushButton(m_UIManagerRef[0], this);
			OptionBtns[i].CreateControl(new Vector2(90 * i, 286f), new Vector2(92f, 33f), "anniu-2", "anniu-1", string.Empty, D3DMain.Instance.GameFont2.FontName, 9, 18, HowToTitles[i], (D3DMain.Instance.HD_SIZE != 2) ? new Vector2(0f, 1f) : new Vector2(0f, -6f), (float)D3DMain.Instance.HD_SIZE * 1.5f, D3DMain.Instance.CommonFontColor, new Color(0f, 0f, 0f, 1f), new Color(0f, 0f, 0f, 0f));
		}
		OptionBtns[0].Set(true);
		CurrentOptionIndex = 0;
		uIImage = new UIImage();
		imageCell = GetImageCell("ditu");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(480f, 290f) * D3DMain.Instance.HD_SIZE);
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 290f);
		m_UIManagerRef[0].Add(uIImage);
		IllArrow = new UIImage[2];
		IllArrow[0] = new UIImage();
		imageCell = GetImageCell("tishijiantou");
		IllArrow[0].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(40f, 30f) * D3DMain.Instance.HD_SIZE);
		IllArrow[0].Rect = D3DMain.Instance.ConvertRectAutoHD(5f, 140f, 40f, 30f);
		IllArrow[0].Enable = false;
		IllArrow[0].SetRotation(-(float)Math.PI / 2f);
		m_UIManagerRef[0].Add(IllArrow[0]);
		IllArrow[1] = new UIImage();
		imageCell = GetImageCell("tishijiantou");
		IllArrow[1].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(40f, 30f) * D3DMain.Instance.HD_SIZE);
		IllArrow[1].Rect = D3DMain.Instance.ConvertRectAutoHD(435f, 140f, 40f, 30f);
		IllArrow[1].Enable = false;
		IllArrow[1].SetRotation(-(float)Math.PI / 2f);
		IllArrow[1].FlipY(true);
		m_UIManagerRef[0].Add(IllArrow[1]);
		PageText = new UIText();
		PageText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, Color.black);
		PageText.Enable = false;
		PageText.Rect = D3DMain.Instance.ConvertRectAutoHD(215f, 3f, 50f, 20f);
		PageText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		PageText.AlignStyle = UIText.enAlignStyle.center;
		m_UIManagerRef[0].Add(PageText);
		HowToSlipUI = new D3DHowToSlipUI(m_UIManagerRef[1], this, 0, new Rect(0f, 0f, 377f, 251f), -50f, IllArrow, PageText);
		HowToSlipUI.InitHowToUI(D3DHowTo.Instance.GetHowToIlls(HowToTitles[CurrentOptionIndex]));
		HowToIllMove = new UIMove();
		HowToIllMove.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 25f, 480f, 260f);
		m_UIManagerRef[0].Add(HowToIllMove);
		UIClickButton uIClickButton = new UIClickButton();
		imageCell = GetImageCell("fanhui-1");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = GetImageCell("fanhui-2");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		cur_control_id++;
		uIClickButton.Id = cur_control_id;
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(428f, 5f, 48f, 35f);
		m_UIManagerRef[0].Add(uIClickButton);
		m_control_table.Add("HowToBackBtn", uIClickButton);
	}

	private bool HowToOptionsEvent(UIControl control)
	{
		int num = 0;
		D3DTextPushButton[] optionBtns = OptionBtns;
		foreach (D3DTextPushButton d3DTextPushButton in optionBtns)
		{
			if (control == d3DTextPushButton.PushBtn)
			{
				if (num == CurrentOptionIndex)
				{
					d3DTextPushButton.Set(true);
				}
				else
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					OptionBtns[CurrentOptionIndex].Set(false);
					d3DTextPushButton.Set(true);
					HowToSlipUI.InitHowToUI(D3DHowTo.Instance.GetHowToIlls(HowToTitles[num]));
					CurrentOptionIndex = num;
				}
				return true;
			}
			num++;
		}
		return false;
	}

	private new void Awake()
	{
		base.name = "UIHowTo";
		base.Awake();
		AddImageCellIndexer(new string[16]
		{
			"UImg1_cell", "UImg4_cell", "howto_ui_cell", "howto0_cell", "howto1_cell", "howto2_cell", "howto3_cell", "howto4_cell", "howto5_cell", "howto6_cell",
			"howto7_cell", "howto8_cell", "howto9_cell", "howto10_cell", "howto11_cell", "UI_Monolayer_cell"
		});
		AddItemIcons();
	}

	private new void Start()
	{
		base.Start();
		CreateMainUI();
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = m_UIManagerRef[0].DEPTH;
		}
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, null, true);
	}

	public new void Update()
	{
		base.Update();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("HowToBackBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, QuitHowTo, false);
		}
		else if (HowToIllMove == control)
		{
			HowToSlipUI.PageSlip((UIMove.Command)command, HowToIllMove.GetCurrentPosition(), wparam);
		}
		else if (!HowToOptionsEvent(control))
		{
		}
	}

	private void QuitHowTo()
	{
		UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
		uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, null, true);
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
