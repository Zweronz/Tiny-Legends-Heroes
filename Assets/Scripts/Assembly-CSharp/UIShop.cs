using System.Collections.Generic;
using UnityEngine;

public class UIShop : UIHelper
{
	private enum ShopUIManager
	{
		MAIN = 0,
		SHOP_STORE = 1,
		GEAR_STORE = 2,
		GEAR_DESCRIPTION = 3,
		TBANK = 4,
		MASK1 = 5,
		MASK2 = 6
	}

	private enum ShopTouchArea
	{
		GEAR_STORE = 0,
		SHOP_STORE = 1,
		GEAR_DESCRIPTION = 2
	}

	private List<PuppetBasic> PlayerTeamPuppetData;

	private SubUIPuppetFace _SubPuppetFaceUI = new SubUIPuppetFace();

	private D3DTextPushButton[] OptionBtns;

	private int CurrentOptionIndex;

	private D3DCurrencyText PlayerCurrencyText;

	private D3DGearSlotUI[] ShopSlots;

	private D3DGearSlotUI ActivingShopGear;

	private D3DCurrencyText RefreshCost;

	private D3DCurrencyText TradePrice;

	private UIText BuyerClass;

	private bool _bUseIAPShop;

	private UIImage _titleBg;

	private UIText _titleText;

	private UIClickButton RefreshButton;

	private UIText RefreshText;

	private UIClickButton BuyButton;

	private UIText BuyText;

	private readonly int _nTotCellCount = 16;

	private List<D3DEquipment> PlayerStore;

	private D3DTextPushButton[] GearPageBtns;

	private List<UIClickButton> UnlockPageBtns;

	private int CurrentGearPage;

	private UIImage PageHoverMask;

	private D3DGearSlotUI[] GearStoreSlots;

	private D3DGearSlotUI ActivingStoreGear;

	private NewHintBehaviour newHintBehaviour;

	private D3DUIGearDescription GearDescriptionUI;

	private SubUItBank _subUItBank = new SubUItBank();

	private UIImage DragIcon;

	private int StartTouchArea;

	private bool EnableDrag;

	private bool DoingDrag;

	private bool InstantClick;

	private D3DComplexSlotUI HoverGear;

	private Rect[] ShopTouchAreas;

	private void CreateShopPuppet()
	{
		if (PlayerTeamPuppetData != null)
		{
			return;
		}
		PlayerTeamPuppetData = new List<PuppetBasic>();
		int num = 1;
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			GameObject gameObject = new GameObject("ShopPuppet" + num);
			gameObject.transform.parent = base.transform;
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerBattleTeamDatum.pupet_profile_id), playerBattleTeamDatum))
			{
				Object.Destroy(gameObject);
				continue;
			}
			puppetBasic.model_builder.BuildPuppetModel();
			puppetBasic.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, Random.Range(0f, 2f));
			puppetBasic.CheckPuppetWeapons();
			gameObject.transform.localPosition = new Vector3(600 * D3DMain.Instance.HD_SIZE, 0f, num * 100);
			gameObject.transform.rotation = Quaternion.identity;
			puppetBasic.model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
			D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic.gameObject, 16);
			PlayerTeamPuppetData.Add(puppetBasic);
			num++;
		}
	}

	private void CreateMainUI()
	{
		InsertUIManager("Manager_Main", 0);
		CreateUIByCellXml("UIShopMainCfg", m_UIManagerRef[0]);
		string[] array = new string[2] { "STORE", "tBANK" };
		OptionBtns = new D3DTextPushButton[2];
		for (int i = 0; i < 2; i++)
		{
			OptionBtns[i] = new D3DTextPushButton(m_UIManagerRef[0], this);
			OptionBtns[i].CreateControl(new Vector2(82 * i, 283f), new Vector2(84f, 37f), "anniu1", "anniu2", string.Empty, D3DMain.Instance.GameFont2.FontName, 11, 22, array[i], (D3DMain.Instance.HD_SIZE != 2) ? new Vector2(0f, 1f) : new Vector2(0f, -3f), (float)D3DMain.Instance.HD_SIZE * 1.5f, D3DMain.Instance.CommonFontColor, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f));
		}
		OptionBtns[0].Set(true);
		OptionBtns[1].Enable(true);
		OptionBtns[1].Visible(true);
		PlayerCurrencyText = new D3DCurrencyText(m_UIManagerRef[0], this);
		UpdateCurrencyUI();
		CreateShopPuppet();
		_SubPuppetFaceUI.CreatePuppetFaceUI(this, 0, 6, OnSelectAnotherPuppetFace);
		DragIcon = new UIImage();
		DragIcon.SetAlpha(0.7f);
		DragIcon.Enable = false;
		DragIcon.Visible = false;
		m_UIManagerRef[6].Add(DragIcon);
		InsertUIManager("Manager_Mask1", 5);
		m_UIManagerRef[5].EnableUIHandler = false;
	}

	private void OnSelectAnotherPuppetFace(int nFaceIndex)
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
		D3DShopRuleEx.Instance.RefreshShop(_SubPuppetFaceUI.CurrentFaceIndex, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID, false);
		BuyerClass.SetText("CLASS: " + PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.class_name);
		UpdateCurrentGearPageStore();
		UpdateShopStore();
		if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
		else if (ActivingShopGear != null)
		{
			UpdateSelectStoreEquipInfo();
		}
	}

	private void LeaveShop()
	{
		D3DGamer.Instance.PlayerStore.Clear();
		foreach (D3DEquipment item in PlayerStore)
		{
			if (item == null)
			{
				D3DGamer.Instance.PlayerStore.Add(null);
				continue;
			}
			D3DGamer.D3DEquipmentSaveData d3DEquipmentSaveData = new D3DGamer.D3DEquipmentSaveData();
			d3DEquipmentSaveData.equipment_id = item.equipment_id;
			d3DEquipmentSaveData.magic_power_data = item.magic_power_data;
			D3DGamer.Instance.PlayerStore.Add(d3DEquipmentSaveData);
		}
		D3DGamer.Instance.SaveAllData();
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, CloseShop, false);
	}

	private void CloseShop()
	{
		if (D3DMain.Instance.CurrentScene == 1)
		{
			Object.Destroy(base.gameObject);
		}
		else if (D3DMain.Instance.CurrentScene == 3)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, uIHelper.FreezeTimeScale, true);
			uIHelper.GetManager(0).gameObject.SetActiveRecursively(true);
			GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().SetPortalVisible(D3DMain.Instance.exploring_dungeon.dungeon.explored_level > 0);
			GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().UpdateSubUINewHint();
			Object.Destroy(base.gameObject);
			if (null != D3DMain.Instance.HD_BOARD_OBJ)
			{
				D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
			}
		}
	}

	private bool ShopOptionsEvent(UIControl control)
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
					SwitchShopOption(num);
				}
				return true;
			}
			num++;
		}
		return false;
	}

	private void SwitchShopOption(int option_index)
	{
		if (option_index == CurrentOptionIndex)
		{
			return;
		}
		CurrentOptionIndex = option_index;
		switch (CurrentOptionIndex)
		{
		case 0:
			m_UIManagerRef[1].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[4].gameObject.SetActiveRecursively(false);
			UpdateShopStore();
			UpdateCurrentGearPageStore();
			if (ActivingStoreGear != null)
			{
				UpdateSelectedEquipInfo();
			}
			else if (ActivingShopGear != null)
			{
				DeSelectShopEquip();
				GearDescriptionUI.Visible(false);
				ActivingShopGear = null;
				GetControl("TradeTxt").Visible = false;
				TradePrice.Visible(false);
			}
			break;
		case 1:
			m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[4].gameObject.SetActiveRecursively(true);
			break;
		}
	}

	private void UpdateCurrencyUI()
	{
		PlayerCurrencyText.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		PlayerCurrencyText.SetPosition(new Vector2(475f - PlayerCurrencyText.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), 293f));
	}

	private void CreateShopStoreUI()
	{
		if (!(null != m_UIManagerRef[1]))
		{
			InsertUIManager("Manager_ShopStore", 1);
			m_UIManagerRef[1].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(208f, 8f, 206f, 271f));
			ShopSlots = new D3DGearSlotUI[_nTotCellCount];
			for (int i = 0; i < _nTotCellCount; i++)
			{
				ShopSlots[i] = new D3DGearSlotUI(m_UIManagerRef[1], this);
				ShopSlots[i].slot_index = i;
				ShopSlots[i].CreateControl(new Vector2(9 + i % 4 * 48, 170 - i / 4 * 43), "zhuangbeikuang");
			}
			_titleBg = new UIImage();
			D3DImageCell imageCell = GetImageCell("store-youlan");
			_titleBg.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			_titleBg.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 225f, 205f, 46f);
			m_UIManagerRef[1].Add(_titleBg);
			_titleText = new UIText();
			_titleText.Enable = false;
			_titleText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 13), "Shop", Color.white);
			_titleText.AlignStyle = UIText.enAlignStyle.center;
			_titleText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(13 * D3DMain.Instance.HD_SIZE);
			_titleText.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 215f, 205f, 46f);
			_titleText.AlignStyle = UIText.enAlignStyle.center;
			m_UIManagerRef[1].Add(_titleText);
			BuyButton = new UIClickButton();
			imageCell = GetImageCell("anniu1");
			BuyButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			imageCell = GetImageCell("anniu2");
			BuyButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			BuyButton.Rect = D3DMain.Instance.ConvertRectAutoHD(110f, 5f, 84f, 37f);
			m_UIManagerRef[1].Add(BuyButton);
			BuyButton.Id = cur_control_id++;
			m_control_table.Add("BuyBtn", BuyButton);
			BuyText = new UIText();
			BuyText.Enable = false;
			BuyText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), "Buy", D3DMain.Instance.CommonFontColor);
			BuyText.AlignStyle = UIText.enAlignStyle.center;
			BuyText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			BuyText.Rect = D3DMain.Instance.ConvertRectAutoHD(115f, -8f, 84f, 37f);
			m_UIManagerRef[1].Add(BuyText);
			BuyerClass = new UIText();
			BuyerClass.Enable = false;
			BuyerClass.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), "CLASS:", D3DMain.Instance.CommonFontColor);
			BuyerClass.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			BuyerClass.Rect = D3DMain.Instance.ConvertRectAutoHD(12f, 190f, 150f, 37f);
			m_UIManagerRef[1].Add(BuyerClass);
			UIText uIText = new UIText();
			uIText.Enable = false;
			uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), "PRICE :", D3DMain.Instance.CommonFontColor);
			uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(13f, 4f, 84f, 37f);
			uIText.Visible = false;
			m_UIManagerRef[1].Add(uIText);
			m_control_table.Add("TradeTxt", uIText);
			TradePrice = new D3DCurrencyText(m_UIManagerRef[1], this);
			TradePrice.SetPosition(new Vector2(10f, 4f));
			TradePrice.Visible(false);
		}
	}

	private D3DGearSlotUI PickShopSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[1].TouchPointOnManager(touch_point2);
		touch_point2 *= 1f / num;
		D3DGearSlotUI[] shopSlots = ShopSlots;
		foreach (D3DGearSlotUI d3DGearSlotUI in shopSlots)
		{
			if (d3DGearSlotUI != null && d3DGearSlotUI.PtInSlot(touch_point2))
			{
				return d3DGearSlotUI;
			}
		}
		return null;
	}

	private void UpdateShopStore()
	{
		if (_bUseIAPShop)
		{
			D3DShopRuleEx.Instance.RefreshIAPEquipByPuppet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance);
		}
		GearDescriptionUI.Visible(false);
		GetControl("TradeTxt").Visible = false;
		TradePrice.Visible(false);
		for (int i = 0; i < ShopSlots.Length; i++)
		{
			ShopSlots[i].HideSlot();
			if (i < D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex).Count && D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[i] != null)
			{
				ShopSlots[i].UpdateGearSlot(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[i], PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance);
			}
		}
		if (ActivingShopGear != null)
		{
			DeSelectShopEquip();
			GearDescriptionUI.Visible(false);
			GetControl("TradeTxt").Visible = false;
			TradePrice.Visible(false);
			ActivingShopGear = null;
		}
		else if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
	}

	private void BuyGear()
	{
		if (ActivingShopGear == null)
		{
			return;
		}
		if (int.Parse(D3DGamer.Instance.CurrencyText) < D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price || int.Parse(D3DGamer.Instance.CrystalText) < D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OpenTBank);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			return;
		}
		bool flag = true;
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage; i++)
		{
			int num = ThrowInPage(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index], i);
			if (num >= 0)
			{
				if (!D3DGamer.Instance.NewGearSlotHint.Contains(num))
				{
					D3DGamer.Instance.NewGearSlotHint.Add(num);
				}
				flag = false;
				break;
			}
		}
		if (flag)
		{
			if (D3DGamer.Instance.ValidStorePage >= 5)
			{
				List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_GET_ALL_BUT_STORE_FULL_MAX_PAGE);
				PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.OK, null);
				return;
			}
			List<string> msgBoxContent3 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_GET_ALL_BUT_STORE_FULL_CAN_UNLOCK);
			msgBoxContent3 = new List<string>(msgBoxContent3);
			int num2 = -1;
			for (int j = 0; j < msgBoxContent3.Count; j++)
			{
				if (msgBoxContent3[j].Contains("<GetPrice>"))
				{
					msgBoxContent3[j] = string.Empty;
					num2 = j;
				}
			}
			List<D3DMessageBoxButtonEvent.OnButtonClick> list2 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list2.Add(IapBuyGearSpace);
			UIManager uIManager = PushMessageBox(msgBoxContent3, D3DMessageBox.MgbButton.CANCEL_OK, list2);
			if (num2 >= 0)
			{
				D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
				d3DCurrencyText.EnableGold = false;
				d3DCurrencyText.SetCrystal(10);
				Rect cameraTransformRect = uIManager.GetCameraTransformRect();
				float num3 = 640f / (float)Screen.height;
				cameraTransformRect = new Rect(cameraTransformRect.x * num3, cameraTransformRect.y * num3, cameraTransformRect.width * num3, cameraTransformRect.height * num3);
				d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num2)));
			}
			return;
		}
		D3DEquipment d3DEquipment = D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index];
		string stringByGrade = D3DEquipment.GetStringByGrade(d3DEquipment.equipment_grade);
		string stringFromEquipmentType = D3DEquipment.GetStringFromEquipmentType(d3DEquipment.equipment_type);
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
		D3DGamer.Instance.UpdateCurrency(-D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price);
		D3DGamer.Instance.UpdateCrystal(-D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal);
		UpdateCurrencyUI();
		D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index] = null;
		GearDescriptionUI.Visible(false);
		DeSelectShopEquip();
		ActivingShopGear = null;
		UpdateCurrentGearPageStore();
		UpdateShopStore();
		GetControl("TradeTxt").Visible = false;
		TradePrice.Visible(false);
		D3DGamer.Instance.PlayerStore.Clear();
		foreach (D3DEquipment item in PlayerStore)
		{
			if (item == null)
			{
				D3DGamer.Instance.PlayerStore.Add(null);
				continue;
			}
			D3DGamer.D3DEquipmentSaveData d3DEquipmentSaveData = new D3DGamer.D3DEquipmentSaveData();
			d3DEquipmentSaveData.equipment_id = item.equipment_id;
			d3DEquipmentSaveData.magic_power_data = item.magic_power_data;
			D3DGamer.Instance.PlayerStore.Add(d3DEquipmentSaveData);
		}
		D3DGamer.Instance.SaveAllData();
	}

	private void DeSelectShopEquip()
	{
		ActivingShopGear.Select(false);
		_SubPuppetFaceUI.HideAll();
	}

	private void DeSelectStoreEquip()
	{
		ActivingStoreGear.Select(false);
		_SubPuppetFaceUI.HideAll();
	}

	private void SellGear()
	{
		if (ActivingStoreGear == null)
		{
			return;
		}
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
		D3DGamer.Instance.UpdateCurrency(PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].SellPrice);
		UpdateCurrencyUI();
		PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index] = null;
		DeSelectStoreEquip();
		ActivingStoreGear = null;
		GearDescriptionUI.Visible(false);
		GetControl("TradeTxt").Visible = false;
		TradePrice.Visible(false);
		UpdateCurrentGearPageStore();
		D3DGamer.Instance.PlayerStore.Clear();
		foreach (D3DEquipment item in PlayerStore)
		{
			if (item == null)
			{
				D3DGamer.Instance.PlayerStore.Add(null);
				continue;
			}
			D3DGamer.D3DEquipmentSaveData d3DEquipmentSaveData = new D3DGamer.D3DEquipmentSaveData();
			d3DEquipmentSaveData.equipment_id = item.equipment_id;
			d3DEquipmentSaveData.magic_power_data = item.magic_power_data;
			D3DGamer.Instance.PlayerStore.Add(d3DEquipmentSaveData);
		}
		D3DGamer.Instance.SaveAllData();
	}

	private bool ShopTypeEvent(UIControl control)
	{
		if (false)
		{
			UpdateShopStore();
		}
		return false;
	}

	private void CreateGearsStoreUI()
	{
		if (null != m_UIManagerRef[2])
		{
			return;
		}
		InsertUIManager("Manager_GearStore", 2);
		m_UIManagerRef[2].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(8f, 8f, 208.5f, 271f));
		CreateUIByCellXml("UIStashGearStoreCfg", m_UIManagerRef[2]);
		PlayerStore = new List<D3DEquipment>();
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage * 12; i++)
		{
			PlayerStore.Add(null);
		}
		int num = 0;
		foreach (D3DGamer.D3DEquipmentSaveData item in D3DGamer.Instance.PlayerStore)
		{
			if (item == null)
			{
				PlayerStore[num] = null;
				num++;
				continue;
			}
			D3DEquipment equipmentClone = D3DMain.Instance.GetEquipmentClone(item.equipment_id);
			equipmentClone.magic_power_data = item.magic_power_data;
			equipmentClone.EnableMagicPower();
			PlayerStore[num] = equipmentClone;
			num++;
		}
		GearPageBtns = new D3DTextPushButton[5];
		for (int j = 0; j < 5; j++)
		{
			GearPageBtns[j] = new D3DTextPushButton(m_UIManagerRef[2], this);
			GearPageBtns[j].CreateControl(new Vector2(-4f + 39f * (float)j, 92f), new Vector2(46f, 33f), "anniu3", "anniu4", "anniu5", D3DMain.Instance.GameFont1.FontName, 9, 18, (j + 1).ToString(), (D3DMain.Instance.HD_SIZE != 2) ? Vector2.zero : new Vector2(0f, -7f), 0f, D3DMain.Instance.CommonFontColor, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f));
			if (j > D3DGamer.Instance.ValidStorePage - 1)
			{
				GearPageBtns[j].Enable(false);
			}
		}
		CurrentGearPage = 0;
		GearPageBtns[CurrentGearPage].Set(true);
		UnlockPageBtns = new List<UIClickButton>();
		int num2 = 5 - D3DGamer.Instance.ValidStorePage;
		for (int k = 0; k < num2; k++)
		{
			UIClickButton uIClickButton = new UIClickButton();
			uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(-4f + 39f * (float)(D3DGamer.Instance.ValidStorePage + k), 92f, 46f, 33f);
			m_UIManagerRef[2].Add(uIClickButton);
			UnlockPageBtns.Add(uIClickButton);
		}
		PageHoverMask = new UIImage();
		D3DImageCell imageCell = GetImageCell("tuodongwupintingliuzhuangtai-1");
		PageHoverMask.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(33f, 19f) * D3DMain.Instance.HD_SIZE);
		PageHoverMask.Enable = false;
		PageHoverMask.Visible = false;
		m_UIManagerRef[2].Add(PageHoverMask);
		GearStoreSlots = new D3DGearSlotUI[12];
		for (int l = 0; l < 12; l++)
		{
			GearStoreSlots[l] = new D3DGearSlotUI(m_UIManagerRef[2], this);
			GearStoreSlots[l].slot_index = l;
			GearStoreSlots[l].CreateControl(new Vector2(4 + l % 4 * 48, 222 - l / 4 * 46), "beibaokuang");
		}
		UIImage uIImage = new UIImage();
		imageCell = UIImageCellIndexer["dakuang9"];
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(193f, 0f, 10f, 271f));
		uIImage.Enable = false;
		m_UIManagerRef[2].Add(uIImage);
		GameObject gameObject = new GameObject("NewHintObj");
		newHintBehaviour = gameObject.AddComponent<NewHintBehaviour>();
	}

	private bool GearStorePageEvent(UIControl control)
	{
		if (null == m_UIManagerRef[2])
		{
			return false;
		}
		int num = 0;
		D3DTextPushButton[] gearPageBtns = GearPageBtns;
		foreach (D3DTextPushButton d3DTextPushButton in gearPageBtns)
		{
			if (control == d3DTextPushButton.PushBtn)
			{
				if (num == CurrentGearPage)
				{
					d3DTextPushButton.Set(true);
					break;
				}
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				GearPageBtns[CurrentGearPage].Set(false);
				d3DTextPushButton.Set(true);
				CurrentGearPage = num;
				ActivingStoreGear = null;
				UpdateCurrentGearPageStore();
				if (ActivingShopGear != null)
				{
					UpdateSelectStoreEquipInfo();
				}
				return true;
			}
			num++;
		}
		return false;
	}

	public void UpdateStashGearStoreByIAP()
	{
		if (null == m_UIManagerRef[2])
		{
			return;
		}
		PlayerStore = new List<D3DEquipment>();
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage * 12; i++)
		{
			PlayerStore.Add(null);
		}
		int num = 0;
		foreach (D3DGamer.D3DEquipmentSaveData item in D3DGamer.Instance.PlayerStore)
		{
			if (item == null)
			{
				PlayerStore[num] = null;
				num++;
				continue;
			}
			D3DEquipment equipmentClone = D3DMain.Instance.GetEquipmentClone(item.equipment_id);
			equipmentClone.magic_power_data = item.magic_power_data;
			equipmentClone.EnableMagicPower();
			PlayerStore[num] = equipmentClone;
			num++;
		}
		UpdateCurrentGearPageStore();
	}

	private void UpdateCurrentGearPageStore()
	{
		GearDescriptionUI.Visible(false);
		GetControl("TradeTxt").Visible = false;
		TradePrice.Visible(false);
		int num = CurrentGearPage * 12;
		for (int i = 0; i < GearStoreSlots.Length; i++)
		{
			GearStoreSlots[i].HideSlot();
			int num2 = num + i;
			if (num2 > PlayerStore.Count - 1)
			{
				break;
			}
			if (PlayerStore[num2] != null)
			{
				GearStoreSlots[i].UpdateGearSlot(PlayerStore[num2], PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance);
				if (D3DGamer.Instance.NewGearSlotHint.Contains(num2))
				{
					GearStoreSlots[i].NewHint.Visible = true;
					newHintBehaviour.AddHintImage(GearStoreSlots[i].NewHint);
				}
			}
		}
		if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
	}

	private bool CheckPageFull(int page)
	{
		int num = 12 * page;
		for (int i = num; i < num + 12 && i <= PlayerStore.Count - 1; i++)
		{
			if (PlayerStore[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	private D3DGearSlotUI PickGearStoreSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[2].TouchPointOnManager(touch_point2);
		touch_point2 *= 1f / num;
		D3DGearSlotUI[] gearStoreSlots = GearStoreSlots;
		foreach (D3DGearSlotUI d3DGearSlotUI in gearStoreSlots)
		{
			if (d3DGearSlotUI != null && d3DGearSlotUI.PtInSlot(touch_point2))
			{
				return d3DGearSlotUI;
			}
		}
		return null;
	}

	private int ThrowInPage(D3DEquipment equipment, int page)
	{
		int num = 12 * page;
		for (int i = num; i < num + 12 && i <= PlayerStore.Count - 1; i++)
		{
			if (PlayerStore[i] == null)
			{
				PlayerStore[i] = equipment;
				return i;
			}
		}
		return -1;
	}

	private void IapBuyGearSpace()
	{
		if (int.Parse(D3DGamer.Instance.CrystalText) < 10)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OpenTBank);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			return;
		}
		D3DGamer.Instance.UpdateCrystal(-10);
		UpdateCurrencyUI();
		D3DGamer.Instance.ValidStorePage++;
		if (D3DGamer.Instance.ValidStorePage > 5)
		{
			D3DGamer.Instance.ValidStorePage = 5;
			return;
		}
		UnlockPageBtns[0].Enable = false;
		UnlockPageBtns.RemoveAt(0);
		GearPageBtns[D3DGamer.Instance.ValidStorePage - 1].Enable(true);
		for (int i = 0; i < 12; i++)
		{
			PlayerStore.Add(null);
		}
	}

	private void CreateGearDescriptionUI()
	{
		if (!(null != m_UIManagerRef[3]))
		{
			InsertUIManager("Manager_GearDescription", 3);
			GearDescriptionUI = new D3DUIGearDescription(m_UIManagerRef[3], this, new Rect(11f, 11f, 191f, 83f));
			GearDescriptionUI.CreateCompareBtn();
			GearDescriptionUI.CreateScrollBar(false, true);
			GearDescriptionUI.InitScrollBar();
			GearDescriptionUI.Visible(false);
		}
	}

	public void OpenTBank()
	{
		OptionBtns[CurrentOptionIndex].Set(false);
		OptionBtns[1].Visible(true);
		OptionBtns[1].Enable(true);
		OptionBtns[1].Set(true);
		CurrentOptionIndex = -1;
		SwitchShopOption(1);
	}

	private void SetDragIcon(UIImage source_img)
	{
		DragIcon.SetTexture(source_img.GetTexture(), source_img.GetTextureRect(), source_img.GetTextureSize());
	}

	private void SetDragIconPosition(Vector2 touch_point)
	{
		Vector2 vector = new Vector2(-18f, 18f) * D3DMain.Instance.HD_SIZE;
		DragIcon.SetPosition(touch_point + vector);
	}

	private void CreateShopTouchAreas()
	{
		ShopTouchAreas = new Rect[3]
		{
			new Rect(13 * D3DMain.Instance.HD_SIZE, 138 * D3DMain.Instance.HD_SIZE, 186 * D3DMain.Instance.HD_SIZE, 136 * D3DMain.Instance.HD_SIZE),
			new Rect(214 * D3DMain.Instance.HD_SIZE, 50 * D3DMain.Instance.HD_SIZE, 198 * D3DMain.Instance.HD_SIZE, 180 * D3DMain.Instance.HD_SIZE),
			new Rect(10 * D3DMain.Instance.HD_SIZE, 9 * D3DMain.Instance.HD_SIZE, 192 * D3DMain.Instance.HD_SIZE, 86 * D3DMain.Instance.HD_SIZE)
		};
	}

	private int GetShopTouchArea(Vector2 touch_position)
	{
		for (int i = 0; i <= 2; i++)
		{
			if (ShopTouchAreas[i].Contains(touch_position))
			{
				return i;
			}
		}
		return -1;
	}

	private int GearDragOnPage(Vector2 touch_point)
	{
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage; i++)
		{
			if (GearPageBtns[i].PushBtn.PtInRect(touch_point))
			{
				Rect rect = GearPageBtns[i].PushBtn.Rect;
				PageHoverMask.Rect = new Rect(rect.x + (float)(7 * D3DMain.Instance.HD_SIZE), rect.y + (float)(6 * D3DMain.Instance.HD_SIZE), 33 * D3DMain.Instance.HD_SIZE, 19 * D3DMain.Instance.HD_SIZE);
				PageHoverMask.Visible = true;
				if (CheckPageFull(i))
				{
					PageHoverMask.SetColor(Color.red);
				}
				else
				{
					PageHoverMask.SetColor(new Color(0.2901961f, 1f, 0.7647059f));
				}
				return i;
			}
		}
		PageHoverMask.Visible = false;
		return -1;
	}

	private void ShopTouchEvent(Vector2 touch_point, int touch_command, float x_delta, float y_delta)
	{
		switch (touch_command)
		{
		case 0:
			StartTouchArea = GetShopTouchArea(touch_point);
			switch (StartTouchArea)
			{
			case 2:
				GearDescriptionUI.StopInertia();
				break;
			case 0:
			{
				D3DGearSlotUI d3DGearSlotUI4 = PickGearStoreSlot(touch_point);
				if (d3DGearSlotUI4 == null)
				{
					break;
				}
				int index2 = d3DGearSlotUI4.slot_index + CurrentGearPage * 12;
				if (PlayerStore[index2] == null)
				{
					break;
				}
				if (d3DGearSlotUI4 != ActivingStoreGear)
				{
					if (ActivingShopGear != null)
					{
						DeSelectShopEquip();
						ActivingShopGear = null;
					}
					if (ActivingStoreGear != null)
					{
						DeSelectStoreEquip();
					}
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					ActivingStoreGear = d3DGearSlotUI4;
					ActivingStoreGear.Select(true);
					if (D3DGamer.Instance.NewGearSlotHint.Contains(CurrentGearPage * 12 + ActivingStoreGear.slot_index))
					{
						D3DGamer.Instance.NewGearSlotHint.Remove(CurrentGearPage * 12 + ActivingStoreGear.slot_index);
						ActivingStoreGear.NewHint.Visible = false;
					}
					UpdateSelectedEquipInfo();
				}
				else
				{
					InstantClick = true;
				}
				EnableDrag = true;
				break;
			}
			case 1:
			{
				D3DGearSlotUI d3DGearSlotUI3 = PickShopSlot(touch_point);
				if (d3DGearSlotUI3 == null)
				{
					break;
				}
				int slot_index = d3DGearSlotUI3.slot_index;
				if (slot_index >= D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex).Count || D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[slot_index] == null)
				{
					break;
				}
				if (d3DGearSlotUI3 != ActivingShopGear)
				{
					if (ActivingStoreGear != null)
					{
						DeSelectStoreEquip();
						ActivingStoreGear = null;
					}
					if (ActivingShopGear != null)
					{
						DeSelectShopEquip();
					}
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					ActivingShopGear = d3DGearSlotUI3;
					UpdateSelectStoreEquipInfo();
				}
				else
				{
					InstantClick = true;
				}
				EnableDrag = true;
				break;
			}
			}
			break;
		case 1:
			if ((StartTouchArea == 0 || StartTouchArea == 1) && EnableDrag)
			{
				DoingDrag = true;
				InstantClick = false;
				if (ActivingStoreGear != null)
				{
					SetDragIcon(ActivingStoreGear.SlotIcon);
					DragIcon.Visible = true;
					SetDragIconPosition(touch_point);
					int index3 = ActivingStoreGear.slot_index + CurrentGearPage * 12;
					D3DEquipment d3DEquipment2 = PlayerStore[index3];
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPickUpSfx((int)d3DEquipment2.equipment_class), null, false, false);
				}
				else if (ActivingShopGear != null)
				{
					SetDragIcon(ActivingShopGear.SlotIcon);
					DragIcon.Visible = true;
					SetDragIconPosition(touch_point);
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPickUpSfx((int)D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].equipment_class), null, false, false);
				}
			}
			break;
		case 2:
			switch (StartTouchArea)
			{
			case 2:
				GearDescriptionUI.Scroll(new Vector2(0f, y_delta));
				break;
			case 0:
			case 1:
				if (!DragIcon.Visible || !EnableDrag || (ActivingStoreGear == null && ActivingShopGear == null))
				{
					break;
				}
				SetDragIconPosition(touch_point);
				if (GetShopTouchArea(touch_point) == 0)
				{
					PageHoverMask.Visible = false;
					D3DGearSlotUI d3DGearSlotUI2 = PickGearStoreSlot(touch_point);
					if (d3DGearSlotUI2 != null)
					{
						if (HoverGear != null)
						{
							if (d3DGearSlotUI2 != HoverGear)
							{
								HoverGear.SetHover(false, true);
								HoverGear = d3DGearSlotUI2;
								HoverGear.SetHover(true, true);
							}
						}
						else
						{
							HoverGear = d3DGearSlotUI2;
							HoverGear.SetHover(true, true);
						}
					}
					else if (HoverGear != null)
					{
						HoverGear.SetHover(false, true);
						HoverGear = null;
					}
				}
				else
				{
					if (HoverGear != null)
					{
						HoverGear.SetHover(false, true);
						HoverGear = null;
					}
					GearDragOnPage(touch_point);
				}
				break;
			}
			break;
		case 4:
			switch (StartTouchArea)
			{
			case 2:
				GearDescriptionUI.ScrollInertia(new Vector2(0f, y_delta));
				break;
			case 0:
			case 1:
				if (!EnableDrag)
				{
					break;
				}
				if (ActivingStoreGear != null)
				{
					int shopTouchArea = GetShopTouchArea(touch_point);
					int index = ActivingStoreGear.slot_index + CurrentGearPage * 12;
					D3DEquipment d3DEquipment = PlayerStore[index];
					if (DoingDrag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPutDownSfx((int)d3DEquipment.equipment_class), null, false, false);
					}
					if (InstantClick)
					{
						List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_SELL_ITEM);
						msgBoxContent = new List<string>(msgBoxContent);
						Dictionary<int, Color> dictionary = null;
						int num = -1;
						for (int i = 0; i < msgBoxContent.Count; i++)
						{
							if (msgBoxContent[i].Contains("<GetGear>"))
							{
								if (dictionary == null)
								{
									dictionary = new Dictionary<int, Color>();
								}
								msgBoxContent[i] = msgBoxContent[i].Replace("<GetGear>", PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].equipment_name);
								dictionary.Add(i, D3DMain.Instance.GetEquipmentGradeColor(PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].equipment_grade));
							}
							if (msgBoxContent[i].Contains("<GetPrice>"))
							{
								msgBoxContent[i] = string.Empty;
								num = i;
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list.Add(SellGear);
						UIManager uIManager = PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list, false, dictionary);
						if (num >= 0)
						{
							D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
							d3DCurrencyText.SetGold(PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].SellPrice);
							d3DCurrencyText.EnableCrystal = false;
							Rect cameraTransformRect = uIManager.GetCameraTransformRect();
							float num2 = 640f / (float)Screen.height;
							cameraTransformRect = new Rect(cameraTransformRect.x * num2, cameraTransformRect.y * num2, cameraTransformRect.width * num2, cameraTransformRect.height * num2);
							d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num)));
						}
						InstantClick = false;
						break;
					}
					switch (shopTouchArea)
					{
					case 0:
					{
						D3DGearSlotUI d3DGearSlotUI = PickGearStoreSlot(touch_point);
						if (d3DGearSlotUI != null && d3DGearSlotUI != ActivingStoreGear)
						{
							int num4 = d3DGearSlotUI.slot_index + CurrentGearPage * 12;
							if (D3DGamer.Instance.NewGearSlotHint.Contains(num4))
							{
								D3DGamer.Instance.NewGearSlotHint.Remove(num4);
							}
							D3DEquipment value = PlayerStore[num4];
							PlayerStore[num4] = PlayerStore[index];
							PlayerStore[index] = value;
							GearDescriptionUI.Visible(false);
							DeSelectStoreEquip();
							ActivingStoreGear = null;
							UpdateCurrentGearPageStore();
							GetControl("TradeTxt").Visible = false;
							TradePrice.Visible(false);
						}
						break;
					}
					case 1:
					{
						List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_SELL_ITEM);
						msgBoxContent2 = new List<string>(msgBoxContent2);
						Dictionary<int, Color> dictionary2 = null;
						int num5 = -1;
						for (int j = 0; j < msgBoxContent2.Count; j++)
						{
							if (msgBoxContent2[j].Contains("<GetGear>"))
							{
								if (dictionary2 == null)
								{
									dictionary2 = new Dictionary<int, Color>();
								}
								msgBoxContent2[j] = msgBoxContent2[j].Replace("<GetGear>", PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].equipment_name);
								dictionary2.Add(j, D3DMain.Instance.GetEquipmentGradeColor(PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].equipment_grade));
							}
							if (msgBoxContent2[j].Contains("<GetPrice>"))
							{
								msgBoxContent2[j] = string.Empty;
								num5 = j;
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list2 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list2.Add(SellGear);
						UIManager uIManager2 = PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list2, false, dictionary2);
						if (num5 >= 0)
						{
							D3DCurrencyText d3DCurrencyText2 = new D3DCurrencyText(uIManager2, this);
							d3DCurrencyText2.SetGold(PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].SellPrice);
							d3DCurrencyText2.EnableCrystal = false;
							Rect cameraTransformRect2 = uIManager2.GetCameraTransformRect();
							float num6 = 640f / (float)Screen.height;
							cameraTransformRect2 = new Rect(cameraTransformRect2.x * num6, cameraTransformRect2.y * num6, cameraTransformRect2.width * num6, cameraTransformRect2.height * num6);
							d3DCurrencyText2.SetPosition(new Vector2((cameraTransformRect2.x + cameraTransformRect2.width * 0.5f - d3DCurrencyText2.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect2.y + 205f - (float)(30 * num5)));
						}
						break;
					}
					default:
					{
						int num3 = GearDragOnPage(touch_point);
						if (num3 != -1 && num3 != CurrentGearPage && ThrowInPage(PlayerStore[index], num3) >= 0)
						{
							PlayerStore[index] = null;
							GearDescriptionUI.Visible(false);
							DeSelectStoreEquip();
							ActivingStoreGear = null;
							UpdateCurrentGearPageStore();
							GetControl("TradeTxt").Visible = false;
							TradePrice.Visible(false);
						}
						break;
					}
					}
				}
				else
				{
					if (ActivingShopGear == null)
					{
						break;
					}
					if (DoingDrag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPutDownSfx((int)D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].equipment_class), null, false, false);
					}
					if (InstantClick)
					{
						List<string> msgBoxContent3 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_BUY_ITEM);
						msgBoxContent3 = new List<string>(msgBoxContent3);
						Dictionary<int, Color> dictionary3 = null;
						int num7 = -1;
						for (int k = 0; k < msgBoxContent3.Count; k++)
						{
							if (msgBoxContent3[k].Contains("<GetGear>"))
							{
								if (dictionary3 == null)
								{
									dictionary3 = new Dictionary<int, Color>();
								}
								msgBoxContent3[k] = msgBoxContent3[k].Replace("<GetGear>", D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].equipment_name);
								dictionary3.Add(k, D3DMain.Instance.GetEquipmentGradeColor(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].equipment_grade));
							}
							if (msgBoxContent3[k].Contains("<GetPrice>"))
							{
								msgBoxContent3[k] = string.Empty;
								num7 = k;
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list3 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list3.Add(BuyGear);
						UIManager uIManager3 = PushMessageBox(msgBoxContent3, D3DMessageBox.MgbButton.CANCEL_OK, list3, false, dictionary3);
						if (num7 >= 0)
						{
							D3DCurrencyText d3DCurrencyText3 = new D3DCurrencyText(uIManager3, this);
							if (D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price == 0)
							{
								d3DCurrencyText3.EnableGold = false;
							}
							else
							{
								d3DCurrencyText3.SetGold(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price);
							}
							if (D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal == 0)
							{
								d3DCurrencyText3.EnableCrystal = false;
							}
							else
							{
								d3DCurrencyText3.SetCrystal(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal);
							}
							Rect cameraTransformRect3 = uIManager3.GetCameraTransformRect();
							float num8 = 640f / (float)Screen.height;
							cameraTransformRect3 = new Rect(cameraTransformRect3.x * num8, cameraTransformRect3.y * num8, cameraTransformRect3.width * num8, cameraTransformRect3.height * num8);
							d3DCurrencyText3.SetPosition(new Vector2((cameraTransformRect3.x + cameraTransformRect3.width * 0.5f - d3DCurrencyText3.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect3.y + 205f - (float)(30 * num7)));
						}
						InstantClick = false;
					}
					else if (GetShopTouchArea(touch_point) == 0)
					{
						StartBuyGear();
					}
				}
				break;
			}
			StartTouchArea = -1;
			PageHoverMask.Visible = false;
			if (HoverGear != null)
			{
				HoverGear.SetHover(false, true);
				HoverGear = null;
			}
			DragIcon.Visible = false;
			EnableDrag = false;
			DoingDrag = false;
			break;
		}
	}

	private void StartBuyGear()
	{
		if (ActivingShopGear == null)
		{
			return;
		}
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_BUY_ITEM);
		msgBoxContent = new List<string>(msgBoxContent);
		Dictionary<int, Color> dictionary = null;
		int num = -1;
		for (int i = 0; i < msgBoxContent.Count; i++)
		{
			if (msgBoxContent[i].Contains("<GetGear>"))
			{
				if (dictionary == null)
				{
					dictionary = new Dictionary<int, Color>();
				}
				msgBoxContent[i] = msgBoxContent[i].Replace("<GetGear>", D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].equipment_name);
				dictionary.Add(i, D3DMain.Instance.GetEquipmentGradeColor(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].equipment_grade));
			}
			if (msgBoxContent[i].Contains("<GetPrice>"))
			{
				msgBoxContent[i] = string.Empty;
				num = i;
			}
		}
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		list.Add(BuyGear);
		UIManager uIManager = PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list, false, dictionary);
		if (num >= 0)
		{
			D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
			if (D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price == 0)
			{
				d3DCurrencyText.EnableGold = false;
			}
			else
			{
				d3DCurrencyText.SetGold(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price);
			}
			if (D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal == 0)
			{
				d3DCurrencyText.EnableCrystal = false;
			}
			else
			{
				d3DCurrencyText.SetCrystal(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal);
			}
			Rect cameraTransformRect = uIManager.GetCameraTransformRect();
			float num2 = 640f / (float)Screen.height;
			cameraTransformRect = new Rect(cameraTransformRect.x * num2, cameraTransformRect.y * num2, cameraTransformRect.width * num2, cameraTransformRect.height * num2);
			d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num)));
		}
	}

	private new void Awake()
	{
		base.name = "UIShop";
		base.Awake();
		AddImageCellIndexer(new string[7] { "UImg0_cell", "UImg1_cell", "UImg2_cell", "UImg5_cell", "UImg6_cell", "UI_Monolayer_cell", "UImg10_cell" });
		AddItemIcons();
		PlayerTeamPuppetData = null;
		StartTouchArea = -1;
		EnableDrag = false;
		DoingDrag = false;
	}

	private new void Start()
	{
		base.Start();
		D3DShopRuleEx.Instance.ResetOdds();
		for (ShopUIManager shopUIManager = ShopUIManager.MAIN; shopUIManager <= ShopUIManager.MASK2; shopUIManager++)
		{
			CreateUIManagerEmpty();
		}
		CreateMainUI();
		CreateShopStoreUI();
		CreateGearsStoreUI();
		CreateGearDescriptionUI();
		UpdateCurrentGearPageStore();
		if (D3DShopRuleEx.Instance.RestBattleCount <= 0)
		{
			D3DGamer.Instance.CleanShopRefreshStatus();
			D3DShopRuleEx.Instance.RestBattleCount = D3DShopRuleEx.Instance.BattleRefreshCount;
		}
		D3DShopRuleEx.Instance.RefreshShop(_SubPuppetFaceUI.CurrentFaceIndex, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID, false);
		UpdateShopStore();
		_subUItBank.CreateTBankUI(4, this, UpdateCurrencyUI, _SubPuppetFaceUI.UpdateFaceFrame);
		CreateShopTouchAreas();
		CurrentOptionIndex = -1;
		SwitchShopOption(0);
		BuyerClass.SetText("CLASS: " + PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.class_name);
		if (ui_index > 1)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			uIHelper.HideFade();
		}
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, CheckTutorial, true);
	}

	public new void Update()
	{
		base.Update();
		_SubPuppetFaceUI.Tick();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("shop_move") == control.Id)
		{
			Vector2 currentPosition = ((UIMove)control).GetCurrentPosition();
			ShopTouchEvent(currentPosition, command, wparam, lparam);
			return;
		}
		if (GetControlId("OKBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			LeaveShop();
			return;
		}
		if (GetControlId("BuyBtn") == control.Id && command == 0)
		{
			StartBuyGear();
		}
		else
		{
			if (GetControlId("RefreshBtn") == control.Id && command == 0)
			{
				if (!DoingDrag)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					int[] refreshCost = D3DShopRuleEx.Instance.GetRefreshCost(PlayerTeamPuppetData[0].profile_instance.puppet_level);
					if (int.Parse(D3DGamer.Instance.CurrencyText) < refreshCost[0] || int.Parse(D3DGamer.Instance.CrystalText) < refreshCost[1])
					{
						List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
						List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list.Add(OpenTBank);
						PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
						return;
					}
					D3DGamer.Instance.UpdateCurrency(-refreshCost[0]);
					D3DGamer.Instance.UpdateCrystal(-refreshCost[1]);
					UpdateCurrencyUI();
					D3DGamer.Instance.SaveAllData();
					D3DShopRuleEx.Instance.AdjustOdds();
					D3DShopRuleEx.Instance.RefreshShop(_SubPuppetFaceUI.CurrentFaceIndex, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID, true);
					D3DShopRuleEx.Instance.RestBattleCount = D3DShopRuleEx.Instance.BattleRefreshCount;
					UpdateShopStore();
				}
				return;
			}
			if (GearDescriptionUI.CompareButton == control && command == 0)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_COMPARE), null, false, false);
				GameObject original = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UICompare") as GameObject;
				original = (GameObject)Object.Instantiate(original);
				UICompare component = original.GetComponent<UICompare>();
				if (ActivingStoreGear != null)
				{
					D3DEquipment selected_gear = PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12];
					D3DEquipment compareGear = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetCompareGear(selected_gear);
					component.StartCoroutine(component.UpdateCompareGearsInfo(selected_gear, compareGear, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, _SubPuppetFaceUI.CurrentFaceIndex));
				}
				else if (ActivingShopGear != null)
				{
					D3DEquipment selected_gear2 = D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index];
					D3DEquipment compareGear2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetCompareGear(selected_gear2);
					component.StartCoroutine(component.UpdateCompareGearsInfo(selected_gear2, compareGear2, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, _SubPuppetFaceUI.CurrentFaceIndex));
				}
				else
				{
					component.StartCoroutine(component.UpdateCompareGearsInfo(null, null, null, _SubPuppetFaceUI.CurrentFaceIndex));
				}
			}
			else
			{
				if (GetControlId("IapBuyBtn") == control.Id && command == 0)
				{
					_subUItBank.BuyIap();
					return;
				}
				if (UnlockPageBtns != null)
				{
					for (int i = 0; i < UnlockPageBtns.Count; i++)
					{
						if (UnlockPageBtns[i] != control || command != 0)
						{
							continue;
						}
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
						List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_UNLOCK_STORE_PAGE);
						msgBoxContent2 = new List<string>(msgBoxContent2);
						int num = -1;
						for (int j = 0; j < msgBoxContent2.Count; j++)
						{
							if (msgBoxContent2[j].Contains("<GetPrice>"))
							{
								msgBoxContent2[j] = string.Empty;
								num = j;
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list2 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list2.Add(IapBuyGearSpace);
						UIManager uIManager = PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list2);
						if (num >= 0)
						{
							D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
							d3DCurrencyText.EnableGold = false;
							d3DCurrencyText.SetCrystal(10);
							Rect cameraTransformRect = uIManager.GetCameraTransformRect();
							float num2 = 640f / (float)Screen.height;
							cameraTransformRect = new Rect(cameraTransformRect.x * num2, cameraTransformRect.y * num2, cameraTransformRect.width * num2, cameraTransformRect.height * num2);
							d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num)));
						}
						return;
					}
				}
			}
		}
		if (!ShopTypeEvent(control) && !GearStorePageEvent(control) && !_SubPuppetFaceUI.PuppetFaceEvent(control) && !ShopOptionsEvent(control) && !_subUItBank.tBankEvent(control))
		{
		}
	}

	private void CheckTutorial()
	{
		if (!D3DGamer.Instance.TutorialState[5])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_ENTER_SHOP);
		}
	}

	private void UpdateSelectStoreEquipInfo()
	{
		ActivingShopGear.Select(true);
		GearDescriptionUI.UpdateDescriptionInfo(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index], false, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, true);
		GearDescriptionUI.Visible(true);
		GetControl("TradeTxt").Visible = true;
		if (D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price == 0)
		{
			TradePrice.EnableGold = false;
		}
		else
		{
			TradePrice.EnableGold = true;
			TradePrice.SetGold(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price);
		}
		if (D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal == 0)
		{
			TradePrice.EnableCrystal = false;
		}
		else
		{
			TradePrice.EnableCrystal = true;
			TradePrice.SetCrystal(D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index].buy_price_crystal);
		}
		TradePrice.Visible(true);
		OnSelectAnEquip(true);
	}

	private void UpdateSelectedEquipInfo()
	{
		ActivingStoreGear.Select(true);
		GearDescriptionUI.UpdateDescriptionInfo(PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12], false, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, true);
		GearDescriptionUI.Visible(true);
		GetControl("TradeTxt").Visible = true;
		TradePrice.EnableCrystal = false;
		TradePrice.EnableGold = true;
		TradePrice.SetGold(PlayerStore[CurrentGearPage * 12 + ActivingStoreGear.slot_index].SellPrice);
		TradePrice.Visible(true);
		OnSelectAnEquip(false);
	}

	private void OnSelectAnEquip(bool bInShop)
	{
		D3DEquipment d3DEquipment = ((!bInShop) ? PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12] : D3DShopRuleEx.Instance.GetCurrentStore(_bUseIAPShop, _SubPuppetFaceUI.CurrentFaceIndex)[ActivingShopGear.slot_index]);
		if (d3DEquipment == null)
		{
			return;
		}
		for (int i = 0; i < PlayerTeamPuppetData.Count; i++)
		{
			if (d3DEquipment.IsEquipmentUseable(PlayerTeamPuppetData[i].profile_instance))
			{
				D3DEquipment compareGear = PlayerTeamPuppetData[i].profile_instance.GetCompareGear(d3DEquipment);
				int num = 0;
				num = ((compareGear == null) ? 1 : d3DEquipment.CompareEquip(compareGear));
				_SubPuppetFaceUI.ShowCompareAnimImg((num != 0) ? true : false, num > 0, i);
			}
			else
			{
				_SubPuppetFaceUI.ShowCompareAnimImg(false, false, i);
			}
		}
	}
}
