using System.Collections.Generic;
using UnityEngine;

public class SubUIGearStore : SubUIBase
{
	public delegate void OnUpdateCurrency();

	public delegate void OnOpenTBank();

	public OnUpdateCurrency _onUpdateCurrency;

	public OnOpenTBank _onOpenTBank;

	private List<D3DEquipment> GearStore;

	private D3DTextPushButton[] GearPageBtns;

	private List<UIClickButton> UnlockPageBtns;

	private int CurrentGearPage;

	private UIImage PageHoverMask;

	private D3DGearSlotUI[] GearStoreSlots;

	private D3DGearSlotUI ActivingStoreGear;

	private void OpenTBank()
	{
		_onOpenTBank();
	}

	private void CreateGearsStoreUI(int nGearsStoreIndex, UIHelper owner, OnUpdateCurrency onUpdateCurrency, OnOpenTBank onOpenTBank)
	{
		_ownerUI = owner;
		_onOpenTBank = onOpenTBank;
		_onUpdateCurrency = onUpdateCurrency;
		_uiManager = _ownerUI.GetManager(nGearsStoreIndex);
		if (null != _uiManager)
		{
			return;
		}
		GearStore = new List<D3DEquipment>();
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage * 12; i++)
		{
			GearStore.Add(null);
		}
		int num = 0;
		foreach (D3DGamer.D3DEquipmentSaveData item in D3DGamer.Instance.PlayerStore)
		{
			if (item == null)
			{
				GearStore[num] = null;
				num++;
				continue;
			}
			D3DEquipment equipmentClone = D3DMain.Instance.GetEquipmentClone(item.equipment_id);
			equipmentClone.magic_power_data = item.magic_power_data;
			equipmentClone.EnableMagicPower();
			GearStore[num] = equipmentClone;
			num++;
		}
		_ownerUI.InsertUIManager("Manager_GearStore", nGearsStoreIndex);
		_uiManager.SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(8f, 8f, 208.5f, 271f));
		_ownerUI.CreateUIByCellXml("UIStashGearStoreCfg", _uiManager);
		GearPageBtns = new D3DTextPushButton[5];
		for (int j = 0; j < 5; j++)
		{
			GearPageBtns[j] = new D3DTextPushButton(_uiManager, _ownerUI);
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
			_uiManager.Add(uIClickButton);
			UnlockPageBtns.Add(uIClickButton);
		}
		PageHoverMask = new UIImage();
		D3DImageCell imageCell = _ownerUI.GetImageCell("tuodongwupintingliuzhuangtai-1");
		PageHoverMask.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(33f, 19f) * D3DMain.Instance.HD_SIZE);
		PageHoverMask.Enable = false;
		PageHoverMask.Visible = false;
		_uiManager.Add(PageHoverMask);
		GearStoreSlots = new D3DGearSlotUI[12];
		for (int l = 0; l < 12; l++)
		{
			GearStoreSlots[l] = new D3DGearSlotUI(_uiManager, _ownerUI);
			GearStoreSlots[l].slot_index = l;
			GearStoreSlots[l].CreateControl(new Vector2(4 + l % 4 * 48, 222 - l / 4 * 46), "beibaokuang");
		}
		UIImage uIImage = new UIImage();
		imageCell = _ownerUI.GetImageCell("dakuang9");
		uIImage.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(193f, 0f, 10f, 271f));
		uIImage.Enable = false;
		_uiManager.Add(uIImage);
	}

	public void UpdateStashGearStoreByIAP()
	{
		if (null == _uiManager)
		{
			return;
		}
		GearStore = new List<D3DEquipment>();
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage * 12; i++)
		{
			GearStore.Add(null);
		}
		int num = 0;
		foreach (D3DGamer.D3DEquipmentSaveData item in D3DGamer.Instance.PlayerStore)
		{
			if (item == null)
			{
				GearStore[num] = null;
				num++;
				continue;
			}
			D3DEquipment equipmentClone = D3DMain.Instance.GetEquipmentClone(item.equipment_id);
			equipmentClone.magic_power_data = item.magic_power_data;
			equipmentClone.EnableMagicPower();
			GearStore[num] = equipmentClone;
			num++;
		}
		UpdateCurrentGearPageStore();
	}

	private bool GearStorePageEvent(UIControl control)
	{
		if (null == _uiManager)
		{
			return false;
		}
		int num = 0;
		D3DTextPushButton[] gearPageBtns = GearPageBtns;
		foreach (D3DTextPushButton d3DTextPushButton in gearPageBtns)
		{
			if (control == d3DTextPushButton.PushBtn)
			{
				if (num != CurrentGearPage)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					GearPageBtns[CurrentGearPage].Set(false);
					d3DTextPushButton.Set(true);
					CurrentGearPage = num;
					ActivingStoreGear = null;
					UpdateCurrentGearPageStore();
					return true;
				}
				d3DTextPushButton.Set(true);
			}
			num++;
		}
		return false;
	}

	private void UpdateCurrentGearPageStore()
	{
	}

	private bool CheckPageFull(int page)
	{
		int num = 12 * page;
		for (int i = num; i < num + 12 && i <= GearStore.Count - 1; i++)
		{
			if (GearStore[i] == null)
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
		touch_point2 = _uiManager.TouchPointOnManager(touch_point2);
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

	private bool ThrowInPage(D3DEquipment equipment, int page)
	{
		int num = 12 * page;
		for (int i = num; i < num + 12 && i <= GearStore.Count - 1; i++)
		{
			if (GearStore[i] == null)
			{
				GearStore[i] = equipment;
				return true;
			}
		}
		return false;
	}

	private void RemoveGearToBin()
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_DESTORY), null, false, false);
		GearStore[ActivingStoreGear.slot_index + CurrentGearPage * 12] = null;
		ActivingStoreGear.Select(false);
		ActivingStoreGear = null;
		UpdateCurrentGearPageStore();
	}

	private void IapBuyGearSpace()
	{
		if (int.Parse(D3DGamer.Instance.CrystalText) < 10)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OpenTBank);
			_ownerUI.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			return;
		}
		D3DGamer.Instance.UpdateCrystal(-10);
		_onUpdateCurrency();
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
			GearStore.Add(null);
		}
	}
}
