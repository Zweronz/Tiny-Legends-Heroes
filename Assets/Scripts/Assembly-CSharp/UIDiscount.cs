using System.Collections.Generic;
using UnityEngine;

public class UIDiscount : UIHelper
{
	private UIManagerOpenClose discount_open_close;

	private bool opened = true;

	private new void Awake()
	{
		base.name = "UIDiscount";
		base.Awake();
		AddImageCellIndexer(new string[2] { "UImg9_cell", "UI_Monolayer_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			CreateUIByCellXml("UIDiscountNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			CreateUIByCellXml("UIDiscountIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UIDiscountCfg", m_UIManagerRef[0]);
		}
		switch (D3DIapDiscount.Instance.CurrentDiscount)
		{
		case 0:
			((UIText)GetControl("DiscountCrystalTxt")).SetText("225");
			((UIText)GetControl("OriginalPriceTxt")).SetText("$19.99");
			((UIText)GetControl("DiscountPriceTxt")).SetText("$11.99");
			((UIText)GetControl("OffTxt")).SetText("-40%");
			break;
		case 1:
			((UIText)GetControl("DiscountCrystalTxt")).SetText("110");
			((UIText)GetControl("OriginalPriceTxt")).SetText("$9.99");
			((UIText)GetControl("DiscountPriceTxt")).SetText("$5.99");
			((UIText)GetControl("OffTxt")).SetText("-40%");
			break;
		case 2:
			((UIText)GetControl("DiscountCrystalTxt")).SetText("110");
			((UIText)GetControl("OriginalPriceTxt")).SetText("$9.99");
			((UIText)GetControl("DiscountPriceTxt")).SetText("$3.99");
			((UIText)GetControl("OffTxt")).SetText("-60%");
			break;
		case 3:
			((UIText)GetControl("DiscountCrystalTxt")).SetText("110");
			((UIText)GetControl("OriginalPriceTxt")).SetText("$9.99");
			((UIText)GetControl("DiscountPriceTxt")).SetText("$1.99");
			((UIText)GetControl("OffTxt")).SetText("-80%");
			break;
		}
		discount_open_close = m_UIManagerRef[0].transform.Find("UIMesh").gameObject.AddComponent<UIManagerOpenClose>();
		discount_open_close.Init(m_UIManagerRef[0].GetCameraTransformRect(), new Rect(GameScreen.width - 79 * D3DMain.Instance.HD_SIZE, GameScreen.height - 75 * D3DMain.Instance.HD_SIZE, 79 * D3DMain.Instance.HD_SIZE, 33 * D3DMain.Instance.HD_SIZE), OnBoardClose);
		discount_open_close.enabled = false;
		if (!opened)
		{
			discount_open_close.Open();
		}
		NeverDestroyedScript.DiscountText2 = (UIText)GetControl("DiscountTimeTxt");
		Time.timeScale = 0.0001f;
	}

	private new void Update()
	{
		base.Update();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (!discount_open_close.enabled)
		{
			if (GetControlId("BoardCloseBtn") == control.Id && command == 0)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
				discount_open_close.Close();
			}
			else if (GetControlId("BuyBtn") == control.Id && command == 0)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
				ConfirmPurchaseIAP();
			}
		}
	}

	private void UpdateCurrencyUI()
	{
	}

	private void PurchasedUpdate()
	{
		UpdateCurrencyUI();
	}

	private void ConfirmPurchaseIAP()
	{
		int[] array = new int[4] { 225, 110, 110, 110 };
		if (int.Parse(D3DGamer.Instance.CrystalText) > 9999 - array[D3DIapDiscount.Instance.CurrentDiscount])
		{
			OverLimitWarning();
		}
		else
		{
			WaitPurchaseReturn();
		}
	}

	private void WaitPurchaseReturn()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIWaitPurchase"));
		gameObject.GetComponent<UIWaitPurchase>().StartPurchase((D3DGamer.IapMenu)(100 + D3DIapDiscount.Instance.CurrentDiscount), null, OnBoughtDiscountIap);
	}

	private void OverLimitWarning()
	{
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		list.Add(WaitPurchaseReturn);
		PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_IAP_GOLD_OVER_LIMIT), D3DMessageBox.MgbButton.CANCEL_OK, list);
	}

	public void CloseBoard()
	{
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
	}

	private void OnBoardClose()
	{
		Time.timeScale = 1f;
		Object.Destroy(base.gameObject);
	}

	private void OnBoughtDiscountIap()
	{
		discount_open_close.Close();
	}

	public void Init(bool opened)
	{
		this.opened = opened;
	}
}
