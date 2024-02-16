using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWaitPurchase : UIHelper
{
	public delegate void UpdateCurrencyUI();

	public delegate void UpdateFaceFrame();

	private D3DGamer.IapMenu PurchaseIap;

	private UpdateCurrencyUI updateCurrencyUI;

	private D3DMessageBoxButtonEvent.OnButtonClick mgbOkEvent;

	private D3DUIImageAnimation Indicator;

	private string strLastGoogleplayPurchaseId = string.Empty;

	public static GameObject CreateUIWaitpurchase()
	{
		return UnityEngine.Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIWaitPurchase")) as GameObject;
	}

	private new void Awake()
	{
		base.name = "UIWaitPurchase";
		base.Awake();
		AddImageCellIndexer(new string[3] { "UImg1_cell", "UI_Monolayer_cell", "Indicator_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = GetImageCell("ui_monolayer");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(GameScreen.width, GameScreen.height));
		uIImage.SetColor(new Color(0f, 0f, 0f, 0.85f));
		uIImage.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		m_UIManagerRef[0].Add(uIImage);
		CreateUIManager("Manager_WaitPurchase");
		CreateUIByCellXml("UIWaitPurchaseCfg", m_UIManagerRef[1]);
		string font = LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9);
		float charSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		Color commonFontColor = D3DMain.Instance.CommonFontColor;
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_WAITING_PURCHASE);
		int num = 0;
		foreach (string item in msgBoxContent)
		{
			UIText uIText = new UIText();
			uIText.Set(font, item, commonFontColor);
			uIText.AlignStyle = UIText.enAlignStyle.center;
			uIText.CharacterSpacing = charSpacing;
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(92f, 205 - 30 * num, 295f, 20f);
			m_UIManagerRef[1].Add(uIText);
			num++;
		}
		Indicator = new D3DUIImageAnimation(m_UIManagerRef[1], this, new Rect(205f, 90f, 64f, 64f));
		List<D3DImageCell> list = new List<D3DImageCell>();
		for (int i = 1; i < 12; i++)
		{
			list.Add(GetImageCell(i.ToString()));
		}
		Indicator.InitAnimation(list, 12f, true, null);
		Indicator.Play();
	}

	private void OnApplicationQuit()
	{
		if (D3DMain.Instance.AndroidPlatform != D3DMain.ANDROID_PLATFORM.GOOGLE_PLAY)
		{
		}
	}

	public void StartPurchase(D3DGamer.IapMenu iap, UpdateCurrencyUI updateCurrencyUI, D3DMessageBoxButtonEvent.OnButtonClick mgbOkEvent)
	{
		PurchaseIap = iap;
		Debug.Log(string.Concat("StartPurchase iapMenu:", iap, "platform:", D3DMain.Instance.AndroidPlatform));
		if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
		{
			AmazonIAPManager.purchaseSuccessfulEvent += OnIapSuccessAmazon;
			AmazonIAPManager.purchaseFailedEvent += OnIapFailedAmazon;
		}
		else
		{
			Debug.Log("google add event");
			GoogleIABManager.purchaseSucceededEvent += OnIapSuccessGooglePlay;
			GoogleIABManager.purchaseFailedEvent += OnIapFailedGooglePlay;
		}
		if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.GOOGLE_PLAY)
		{
		}
		Debug.Log("switch iap");
		switch (iap)
		{
		case D3DGamer.IapMenu.IAP_499:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.499centsv121");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.499centsv121");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.499centsv121";
			break;
		case D3DGamer.IapMenu.IAP_999:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.999cents2");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.999cents2");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.999cents2";
			break;
		case D3DGamer.IapMenu.IAP_1999:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.1999cents2");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.1999cents2");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.1999cents2";
			break;
		case D3DGamer.IapMenu.IAP_4999:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.4999cents2");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.4999cents2");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.4999cents2";
			break;
		case D3DGamer.IapMenu.IAP_9999:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.9999centsv135");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.9999centsv135");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.9999centsv135";
			break;
		case D3DGamer.IapMenu.IAP_NEWBIE:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.299centsv135new");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.299centsv135new");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.299centsv135new";
			break;
		case D3DGamer.IapMenu.IAP_DISCOUNT_ACTIVE:
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				AmazonIAP.initiatePurchaseRequest("com.trinitigame.tinylegendsheroes.1199centssale");
				break;
			}
			GoogleIAB.purchaseProduct("com.trinitigame.tinylegendsheroes.1199centssale");
			strLastGoogleplayPurchaseId = "com.trinitigame.tinylegendsheroes.1199centssale";
			break;
		}
		this.updateCurrencyUI = updateCurrencyUI;
		this.mgbOkEvent = mgbOkEvent;
	}

	private new void Update()
	{
	}

	private void OnPurchaseSuccess()
	{
		switch (PurchaseIap)
		{
		case D3DGamer.IapMenu.IAP_499:
			D3DGamer.Instance.UpdateCrystal(54);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_999:
		case D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE7:
		case D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE14:
		case D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE21:
			D3DGamer.Instance.UpdateCrystal(110);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			if (PurchaseIap == D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE7 || PurchaseIap == D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE14 || PurchaseIap == D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE21)
			{
				D3DIapDiscount.Instance.DiscountCountDownSeconds = 0f;
			}
			break;
		case D3DGamer.IapMenu.IAP_DISCOUNT_ACTIVE:
			D3DGamer.Instance.UpdateCrystal(225);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			D3DIapDiscount.Instance.DiscountCountDownSeconds = 0f;
			break;
		case D3DGamer.IapMenu.IAP_1999:
			D3DGamer.Instance.UpdateCrystal(225);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_4999:
			D3DGamer.Instance.UpdateCrystal(585);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_9999:
			D3DGamer.Instance.UpdateCrystal(1200);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			break;
		case D3DGamer.IapMenu.IAP_NEWBIE:
			D3DGamer.Instance.UpdateCurrency(15000);
			D3DGamer.Instance.UpdateCrystal(30);
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
			D3DGamer.Instance.Claim = 9426648.ToString().Insert(5, Convert.ToChar(70).ToString() + Convert.ToChar(67) + Convert.ToChar(65));
			break;
		}
		if (int.Parse(D3DGamer.Instance.CurrencyText) > 9999999)
		{
			D3DGamer.Instance.UpdateCurrency(9999999);
		}
		if (int.Parse(D3DGamer.Instance.CrystalText) > 9999)
		{
			D3DGamer.Instance.UpdateCrystal(9999);
		}
		D3DGamer.Instance.SaveAllData();
		if (updateCurrencyUI != null)
		{
			updateCurrencyUI();
		}
		Hashtable hashtable = new Hashtable();
		switch (PurchaseIap)
		{
		case D3DGamer.IapMenu.IAP_499:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.499centsv121");
			break;
		case D3DGamer.IapMenu.IAP_999:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.999cents2");
			break;
		case D3DGamer.IapMenu.IAP_1999:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.1999cents2");
			break;
		case D3DGamer.IapMenu.IAP_4999:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.4999cents2");
			break;
		case D3DGamer.IapMenu.IAP_9999:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.9999centsv135");
			break;
		case D3DGamer.IapMenu.IAP_NEWBIE:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.299centsv135new");
			break;
		case D3DGamer.IapMenu.IAP_DISCOUNT_ACTIVE:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.1199centssale");
			break;
		case D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE7:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.599centssale");
			break;
		case D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE14:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.399centssale");
			break;
		case D3DGamer.IapMenu.IAP_DISCOUNT_INACTIVE21:
			hashtable.Add("IAP_ID", "com.trinitigame.tinylegendsheroes.199centssale");
			break;
		}
		hashtable.Add("Leader_Level", (D3DGamer.Instance.PlayerBattleTeamData.Count <= 0) ? D3DGamer.Instance.PlayerTeamData[0].puppet_level : D3DGamer.Instance.PlayerBattleTeamData[0].puppet_level);
		D3DGamer.Instance.LogIAP(hashtable["IAP_ID"].ToString());
	}

	private void OnIapSuccessAmazon(AmazonReceipt receipt)
	{
		AmazonIAPManager.purchaseSuccessfulEvent -= OnIapSuccessAmazon;
		AmazonIAPManager.purchaseFailedEvent -= OnIapFailedAmazon;
		UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = null;
		D3DTexts.MsgBoxState box_state = D3DTexts.MsgBoxState.ON_PURCHASE_SUCCESS;
		OnPurchaseSuccess();
		if (mgbOkEvent != null)
		{
			list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(mgbOkEvent);
		}
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(box_state);
		uIHelper.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, list, true);
		base.enabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnIapFailedAmazon(string reason)
	{
		AmazonIAPManager.purchaseSuccessfulEvent -= OnIapSuccessAmazon;
		AmazonIAPManager.purchaseFailedEvent -= OnIapFailedAmazon;
		UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
		List<D3DMessageBoxButtonEvent.OnButtonClick> events = null;
		D3DTexts.MsgBoxState box_state = D3DTexts.MsgBoxState.ON_PURCHASE_FAILED;
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(box_state);
		uIHelper.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, events, true);
		base.enabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnIapSuccessGooglePlay(GooglePurchase product)
	{
		GoogleIAB.consumeProduct(strLastGoogleplayPurchaseId);
		GoogleIABManager.purchaseSucceededEvent -= OnIapSuccessGooglePlay;
		GoogleIABManager.purchaseFailedEvent -= OnIapFailedGooglePlay;
		UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = null;
		D3DTexts.MsgBoxState box_state = D3DTexts.MsgBoxState.ON_PURCHASE_SUCCESS;
		OnPurchaseSuccess();
		if (mgbOkEvent != null)
		{
			list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(mgbOkEvent);
		}
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(box_state);
		uIHelper.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, list, true);
		base.enabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnIapFailedGooglePlay(string reason)
	{
		GoogleIAB.consumeProduct(strLastGoogleplayPurchaseId);
		GoogleIABManager.purchaseSucceededEvent -= OnIapSuccessGooglePlay;
		GoogleIABManager.purchaseFailedEvent -= OnIapFailedGooglePlay;
		UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
		List<D3DMessageBoxButtonEvent.OnButtonClick> events = null;
		D3DTexts.MsgBoxState box_state = D3DTexts.MsgBoxState.ON_PURCHASE_FAILED;
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(box_state);
		uIHelper.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, events, true);
		base.enabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
