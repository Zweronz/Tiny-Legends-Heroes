using System.Collections.Generic;
using UnityEngine;

public class SubUItBank : SubUIBase
{
	public delegate void OnUpdateCurrency();

	public delegate void OnUpdateFaceFrame();

	public OnUpdateCurrency _onUpdateCurrency;

	public OnUpdateFaceFrame _onUpdateFaceFrame;

	private D3DIapButton[] Iap_Btns;

	private int iap_index;

	private UIText iap_title;

	private UIText iap_detail;

	private void CallbackOnUpdateCurrency()
	{
		_onUpdateCurrency();
	}

	public void CreateTBankUI(int nTBankUIIndex, UIHelper owner, OnUpdateCurrency onUpdateCurrency, OnUpdateFaceFrame onUpdateFaceFrame)
	{
		_ownerUI = owner;
		_onUpdateCurrency = onUpdateCurrency;
		_onUpdateFaceFrame = onUpdateFaceFrame;
		_uiManager = _ownerUI.GetManager(nTBankUIIndex);
		if (null != _uiManager)
		{
			return;
		}
		_uiManager = _ownerUI.InsertUIManager("Manager_tBank", nTBankUIIndex);
		_uiManager.SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(8f, 8f, 410f, 275f));
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = _ownerUI.GetImageCell("dituzhuan");
		uIImage.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 410f, 275f);
		_uiManager.Add(uIImage);
		UIImage uIImage2 = new UIImage();
		imageCell = _ownerUI.GetImageCell("lvsexinxikuang");
		uIImage2.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(407f, 75f) * D3DMain.Instance.HD_SIZE);
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(-1f, 0f, 407f, 75f);
		_uiManager.Add(uIImage2);
		Vector2[] array = new Vector2[10]
		{
			new Vector2(3f, 175f),
			new Vector2(83f, 175f),
			new Vector2(163f, 175f),
			new Vector2(243f, 175f),
			new Vector2(323f, 175f),
			new Vector2(3f, 77f),
			new Vector2(83f, 77f),
			new Vector2(163f, 77f),
			new Vector2(243f, 77f),
			new Vector2(323f, 77f)
		};
		Iap_Btns = new D3DIapButton[10];
		for (int i = 0; i < 10; i++)
		{
			Iap_Btns[i] = new D3DIapButton(_uiManager, _ownerUI, array[i], (D3DGamer.IapMenu)i);
			if (i == 5 && ("696C48541164C101" == D3DGamer.Instance.Claim || "94266FCA48" == D3DGamer.Instance.Claim))
			{
				Iap_Btns[i].Disable();
			}
			else if (i == 9 && D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
			{
				Iap_Btns[i].Disable();
			}
		}
		iap_index = 0;
		Iap_Btns[iap_index].Select(true);
		UIText uIText = new UIText();
		uIText.Enable = false;
		uIText.Set(_ownerUI.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), "Product", D3DMain.Instance.CommonFontColor);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(10f, 49f, 120f, 20f);
		_uiManager.Add(uIText);
		iap_title = new UIText();
		iap_title.Enable = false;
		iap_title.Set(_ownerUI.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), string.Empty, D3DMain.Instance.CommonFontColor);
		iap_title.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
		iap_title.Rect = D3DMain.Instance.ConvertRectAutoHD(10f, 32f, 220f, 20f);
		_uiManager.Add(iap_title);
		iap_detail = new UIText();
		iap_detail.Enable = false;
		iap_detail.Set(_ownerUI.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), string.Empty, D3DMain.Instance.CommonFontColor);
		iap_detail.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
		iap_detail.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(8 * D3DMain.Instance.HD_SIZE);
		iap_detail.Rect = D3DMain.Instance.ConvertRectAutoHD(10f, -25f, 386f, 60f);
		_uiManager.Add(iap_detail);
		UpdateCurrentIapDetails();
		UIClickButton uIClickButton = new UIClickButton();
		imageCell = _ownerUI.GetImageCell("anniu1");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, _ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = _ownerUI.GetImageCell("anniu2");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, _ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(315f, 5f, 84f, 37f);
		_uiManager.Add(uIClickButton);
		uIClickButton.Id = _ownerUI.Cur_control_id;
		_ownerUI.Cur_control_id++;
		_ownerUI.AddControlToTable("IapBuyBtn", uIClickButton);
		uIText = new UIText();
		uIText.Enable = false;
		uIText.Set(_ownerUI.LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 13), "BUY", D3DMain.Instance.CommonFontColor);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(13 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(340f, -5f, 84f, 37f);
		_uiManager.Add(uIText);
	}

	public bool tBankEvent(UIControl control)
	{
		if (null == _uiManager)
		{
			return false;
		}
		for (int i = 0; i < Iap_Btns.Length; i++)
		{
			if (control == Iap_Btns[i].IapButton)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				Iap_Btns[iap_index].Select(false);
				Iap_Btns[i].Select(true);
				iap_index = i;
				UpdateCurrentIapDetails();
				return true;
			}
		}
		return false;
	}

	private void UpdateCurrentIapDetails()
	{
		iap_title.SetText(D3DTexts.Instance.GetTBankName((D3DGamer.IapMenu)iap_index));
		List<string> tBankContent = D3DTexts.Instance.GetTBankContent((D3DGamer.IapMenu)iap_index);
		string text = string.Empty;
		foreach (string item in tBankContent)
		{
			Debug.Log(item);
			if (item == "Get 100 tCrystals!")
			{
				Debug.Log("equals");
				text += "Get 1200 tCrystals!\n";
			}
			else
			{
				text = text + item + "\n";
			}
		}
		iap_detail.SetText(text);
	}

	private void ConfirmPurchaseIAP()
	{
		D3DGamer.IapMenu iapMenu = (D3DGamer.IapMenu)iap_index;
		if (iapMenu == D3DGamer.IapMenu.IAP_NEWBIE && ("696C48541164C101" == D3DGamer.Instance.Claim || "94266FCA48" == D3DGamer.Instance.Claim))
		{
			_ownerUI.PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CLICK_PURCHASED_IAP), D3DMessageBox.MgbButton.OK, null);
			return;
		}
		int[,] array = new int[6, 2]
		{
			{ 0, 10 },
			{ 0, 110 },
			{ 0, 225 },
			{ 0, 585 },
			{ 0, 1200 },
			{ 5000, 10 }
		};
		if (int.Parse(D3DGamer.Instance.CurrencyText) > 9999999 - array[iap_index, 0] || int.Parse(D3DGamer.Instance.CrystalText) > 9999 - array[iap_index, 1])
		{
			OverLimitWarning(iapMenu);
		}
		else
		{
			WaitPurchaseReturn();
		}
	}

	private void ConfirmTCrystalExchange()
	{
		int[] array = new int[4] { 5, 30, 100, 30 };
		D3DGamer.IapMenu iapMenu = (D3DGamer.IapMenu)iap_index;
		if (int.Parse(D3DGamer.Instance.CrystalText) < array[iap_index - 6])
		{
			_ownerUI.PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CRYSTAL_EXCHANGE_NOT_ENOUGH), D3DMessageBox.MgbButton.OK, null);
			return;
		}
		if (iapMenu == D3DGamer.IapMenu.IAP_VIP && D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
		{
			_ownerUI.PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CLICK_PURCHASED_IAP), D3DMessageBox.MgbButton.OK, null);
			return;
		}
		int[,] array2 = new int[3, 2]
		{
			{ 5000, 0 },
			{ 32000, 0 },
			{ 110000, 0 }
		};
		D3DGamer.IapMenu iapMenu2 = iapMenu;
		if (iapMenu2 != D3DGamer.IapMenu.IAP_VIP && (int.Parse(D3DGamer.Instance.CurrencyText) > 9999999 - array2[iap_index - 6, 0] || int.Parse(D3DGamer.Instance.CrystalText) > 9999 - array2[iap_index - 6, 1]))
		{
			OverLimitWarning(iapMenu);
		}
		else
		{
			TCrystalExchangeSuccess();
		}
	}

	private void WaitPurchaseReturn()
	{
		GameObject gameObject = UIWaitPurchase.CreateUIWaitpurchase().gameObject;
		if (iap_index == 5)
		{
			gameObject.GetComponent<UIWaitPurchase>().StartPurchase((D3DGamer.IapMenu)iap_index, OnBuyNewBieSuccess, PushIapLoot);
		}
		else
		{
			gameObject.GetComponent<UIWaitPurchase>().StartPurchase((D3DGamer.IapMenu)iap_index, CallbackOnUpdateCurrency, null);
		}
	}

	private void OnBuyNewBieSuccess()
	{
		CallbackOnUpdateCurrency();
		Iap_Btns[5].Disable();
	}

	private void TCrystalExchangeSuccess()
	{
		switch ((D3DGamer.IapMenu)iap_index)
		{
		case D3DGamer.IapMenu.IAP_5T:
			D3DGamer.Instance.UpdateCurrency(5000);
			D3DGamer.Instance.UpdateCrystal(-5);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_30T:
			D3DGamer.Instance.UpdateCurrency(32000);
			D3DGamer.Instance.UpdateCrystal(-30);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_100T:
			D3DGamer.Instance.UpdateCurrency(110000);
			D3DGamer.Instance.UpdateCrystal(-100);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_VIP:
			D3DGamer.Instance.ExpBonus = 0.2f;
			D3DGamer.Instance.GoldBonus = 0.1f;
			D3DGamer.Instance.UpdateCrystal(-30);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_LV_UP), null, false, false);
			Iap_Btns[9].Disable();
			if (_onUpdateFaceFrame != null)
			{
				_onUpdateFaceFrame();
			}
			break;
		}
		if (int.Parse(D3DGamer.Instance.CurrencyText) > 9999999)
		{
			D3DGamer.Instance.UpdateCurrency(9999999);
		}
		D3DGamer.Instance.SaveAllData();
		CallbackOnUpdateCurrency();
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_PURCHASE_SUCCESS);
		_ownerUI.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
	}

	public void BuyIap()
	{
		switch ((D3DGamer.IapMenu)iap_index)
		{
		case D3DGamer.IapMenu.IAP_499:
		case D3DGamer.IapMenu.IAP_999:
		case D3DGamer.IapMenu.IAP_1999:
		case D3DGamer.IapMenu.IAP_4999:
		case D3DGamer.IapMenu.IAP_9999:
		case D3DGamer.IapMenu.IAP_NEWBIE:
			ConfirmPurchaseIAP();
			break;
		case D3DGamer.IapMenu.IAP_5T:
		case D3DGamer.IapMenu.IAP_30T:
		case D3DGamer.IapMenu.IAP_100T:
		case D3DGamer.IapMenu.IAP_VIP:
		{
			if (iap_index == 9 && D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
			{
				_ownerUI.PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CLICK_PURCHASED_IAP), D3DMessageBox.MgbButton.OK, null);
				break;
			}
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_USE_CRYSTAL_EXCHANGE);
			msgBoxContent = new List<string>(msgBoxContent);
			int num = -1;
			for (int i = 0; i < msgBoxContent.Count; i++)
			{
				if (msgBoxContent[i].Contains("<GetIap>"))
				{
					msgBoxContent[i] = msgBoxContent[i].Replace("<GetIap>", D3DTexts.Instance.GetTBankName((D3DGamer.IapMenu)iap_index) + ((iap_index != 5 && iap_index != 9) ? " PACK" : string.Empty));
				}
				if (msgBoxContent[i].Contains("<GetPrice>"))
				{
					msgBoxContent[i] = string.Empty;
					num = i;
				}
			}
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(ConfirmTCrystalExchange);
			UIManager uIManager = _ownerUI.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			if (num >= 0)
			{
				D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, _ownerUI);
				d3DCurrencyText.EnableGold = false;
				int[] array = new int[4] { 5, 30, 100, 30 };
				d3DCurrencyText.SetCrystal(array[iap_index - 6]);
				Rect cameraTransformRect = uIManager.GetCameraTransformRect();
				float num2 = 640f / (float)Screen.height;
				cameraTransformRect = new Rect(cameraTransformRect.x * num2, cameraTransformRect.y * num2, cameraTransformRect.width * num2, cameraTransformRect.height * num2);
				d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num)));
			}
			break;
		}
		}
	}

	private void OverLimitWarning(D3DGamer.IapMenu iap)
	{
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		switch (iap)
		{
		case D3DGamer.IapMenu.IAP_499:
		case D3DGamer.IapMenu.IAP_999:
		case D3DGamer.IapMenu.IAP_1999:
		case D3DGamer.IapMenu.IAP_4999:
		case D3DGamer.IapMenu.IAP_9999:
		case D3DGamer.IapMenu.IAP_NEWBIE:
			list.Add(WaitPurchaseReturn);
			break;
		case D3DGamer.IapMenu.IAP_5T:
		case D3DGamer.IapMenu.IAP_30T:
		case D3DGamer.IapMenu.IAP_100T:
		case D3DGamer.IapMenu.IAP_VIP:
			list.Add(TCrystalExchangeSuccess);
			break;
		}
		_ownerUI.PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_IAP_GOLD_OVER_LIMIT), D3DMessageBox.MgbButton.CANCEL_OK, list);
	}

	private void PushIapLoot()
	{
		D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("xinshoulibaoxianglian001"));
		D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("xinshoulibaojiezhi001"));
		D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("xinshoulibaojiezhi001"));
		_ownerUI.SwithLevel(8);
	}
}
