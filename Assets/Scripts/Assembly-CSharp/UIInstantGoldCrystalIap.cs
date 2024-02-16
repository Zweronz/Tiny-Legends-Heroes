using System.Collections.Generic;
using UnityEngine;

public class UIInstantGoldCrystalIap : UIHelper
{
	private D3DIapButton[] Iap_Btns;

	private int buy_index;

	private int iap_index;

	private D3DCurrencyText PlayerCurrency;

	private new void Awake()
	{
		base.name = "UIInstantIap";
		base.Awake();
		AddImageCellIndexer(new string[3] { "UImg1_cell", "UImg5_cell", "UI_Monolayer_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			CreateUIByCellXml("UIInstantIapNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			CreateUIByCellXml("UIInstantIapIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UIInstantIapCfg", m_UIManagerRef[0]);
		}
		Vector2[] array = new Vector2[3]
		{
			new Vector2(100f, 115f),
			new Vector2(200f, 115f),
			new Vector2(300f, 115f)
		};
		D3DGamer.IapMenu[] array2 = new D3DGamer.IapMenu[3]
		{
			D3DGamer.IapMenu.IAP_499,
			D3DGamer.IapMenu.IAP_999,
			D3DGamer.IapMenu.IAP_5T
		};
		Iap_Btns = new D3DIapButton[3];
		for (int i = 0; i < 3; i++)
		{
			Iap_Btns[i] = new D3DIapButton(m_UIManagerRef[0], this, array[i] + m_UIManagerRef[0].ScreenOffset * (1f / (float)D3DMain.Instance.HD_SIZE), array2[i]);
		}
		buy_index = 0;
		iap_index = 0;
		Iap_Btns[iap_index].Select(true);
		PlayerCurrency = new D3DCurrencyText(m_UIManagerRef[0], this);
		UpdateCurrencyUI();
		UpdateCurrentIapDetails();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("IIapCloseBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			Object.Destroy(base.gameObject);
		}
		else if (GetControlId("BuyBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			BuyIap();
		}
		else
		{
			tBankEvent(control);
		}
	}

	private void UpdateCurrentIapDetails()
	{
		UIText uIText = (UIText)GetControl("IapTxt");
		string text = D3DTexts.Instance.GetTBankName((D3DGamer.IapMenu)iap_index) + " PACK\n";
		List<string> tBankContent = D3DTexts.Instance.GetTBankContent((D3DGamer.IapMenu)iap_index);
		foreach (string item in tBankContent)
		{
			text = text + item + "\n";
		}
		uIText.SetText(text);
	}

	private void UpdateCurrencyUI()
	{
		PlayerCurrency.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		PlayerCurrency.SetPosition(new Vector2((float)GameScreen.width * (1f / (float)D3DMain.Instance.HD_SIZE) - PlayerCurrency.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), (float)GameScreen.height * (1f / (float)D3DMain.Instance.HD_SIZE) - 27f));
	}

	private void PurchasedUpdate()
	{
		UpdateCurrencyUI();
	}

	private void tBankEvent(UIControl control)
	{
		for (int i = 0; i < Iap_Btns.Length; i++)
		{
			if (control == Iap_Btns[i].IapButton)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				Iap_Btns[buy_index].Select(false);
				Iap_Btns[i].Select(true);
				buy_index = i;
				if (buy_index == 0)
				{
					iap_index = 0;
				}
				else if (buy_index == 1)
				{
					iap_index = 1;
				}
				else if (buy_index == 2)
				{
					iap_index = 6;
				}
				UpdateCurrentIapDetails();
				break;
			}
		}
	}

	private void BuyIap()
	{
		switch ((D3DGamer.IapMenu)iap_index)
		{
		case D3DGamer.IapMenu.IAP_499:
		case D3DGamer.IapMenu.IAP_999:
			ConfirmPurchaseIAP();
			break;
		case D3DGamer.IapMenu.IAP_5T:
		{
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
			UIManager uIManager = PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			if (num >= 0)
			{
				D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
				d3DCurrencyText.EnableGold = false;
				int[] array = new int[4] { 5, 30, 100, 30 };
				d3DCurrencyText.SetCrystal(array[iap_index - 6]);
				Rect cameraTransformRect = uIManager.GetCameraTransformRect();
				d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num)));
			}
			break;
		}
		}
	}

	private void ConfirmPurchaseIAP()
	{
		D3DGamer.IapMenu iap = (D3DGamer.IapMenu)iap_index;
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
			OverLimitWarning(iap);
		}
		else
		{
			WaitPurchaseReturn();
		}
	}

	private void WaitPurchaseReturn()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIWaitPurchase"));
		gameObject.GetComponent<UIWaitPurchase>().StartPurchase((D3DGamer.IapMenu)iap_index, PurchasedUpdate, null);
	}

	private void OverLimitWarning(D3DGamer.IapMenu iap)
	{
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		if (iap != 0 && iap != D3DGamer.IapMenu.IAP_999 && iap == D3DGamer.IapMenu.IAP_5T)
		{
			list.Add(TCrystalExchangeSuccess);
		}
		PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_IAP_GOLD_OVER_LIMIT), D3DMessageBox.MgbButton.CANCEL_OK, list);
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
		}
		if (int.Parse(D3DGamer.Instance.CurrencyText) > 9999999)
		{
			D3DGamer.Instance.UpdateCurrency(9999999);
		}
		D3DGamer.Instance.SaveAllData();
		UpdateCurrencyUI();
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_PURCHASE_SUCCESS);
		PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
	}

	private void ConfirmTCrystalExchange()
	{
		int[] array = new int[4] { 5, 30, 100, 30 };
		D3DGamer.IapMenu iapMenu = (D3DGamer.IapMenu)iap_index;
		if (int.Parse(D3DGamer.Instance.CrystalText) < array[iap_index - 6])
		{
			PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CRYSTAL_EXCHANGE_NOT_ENOUGH), D3DMessageBox.MgbButton.OK, null);
			return;
		}
		if (iapMenu == D3DGamer.IapMenu.IAP_VIP && D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
		{
			PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CLICK_PURCHASED_IAP), D3DMessageBox.MgbButton.OK, null);
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
}
