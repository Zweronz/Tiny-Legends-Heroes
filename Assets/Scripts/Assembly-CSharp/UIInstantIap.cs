using System.Collections.Generic;
using UnityEngine;

public class UIInstantIap : UIHelper
{
	private D3DIapButton[] Iap_Btns;

	private int buy_index;

	private int iap_index;

	private D3DCurrencyText PlayerCurrency;

	public UIArena ui_arena;

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
			D3DGamer.IapMenu.IAP_4999
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

	private new void Update()
	{
		base.Update();
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
		if (ui_arena != null)
		{
			ui_arena.InstantIapUpdate();
		}
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
					iap_index = buy_index;
				}
				else if (buy_index == 1)
				{
					iap_index = 1;
				}
				else if (buy_index == 2)
				{
					iap_index = 3;
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
		case D3DGamer.IapMenu.IAP_4999:
			ConfirmPurchaseIAP();
			break;
		case D3DGamer.IapMenu.IAP_1999:
			break;
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
		switch (iap)
		{
		case D3DGamer.IapMenu.IAP_499:
		case D3DGamer.IapMenu.IAP_999:
		case D3DGamer.IapMenu.IAP_4999:
			list.Add(WaitPurchaseReturn);
			break;
		}
		PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_IAP_GOLD_OVER_LIMIT), D3DMessageBox.MgbButton.CANCEL_OK, list);
	}
}
