using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiniStore : UIHelper
{
	private UIManagerOpenClose hint_open_close;

	private D3DGearSlotUI[] ShopSlots = new D3DGearSlotUI[3];

	private List<PuppetBasic> PlayerTeamPuppetData = new List<PuppetBasic>();

	private UIImage _movingEquipIcon = new UIImage();

	private float fScaleIcon = 1.25f;

	private List<D3DEquipment> _equips = new List<D3DEquipment>();

	private SubUItBank _subUItBank = new SubUItBank();

	private new void Awake()
	{
		base.name = "UIMiniStore";
		base.Awake();
		AddImageCellIndexer(new string[5] { "UImg0_cell", "UImg1_cell", "UImg2_cell", "UImg9_cell", "UI_Monolayer_cell" });
		AddItemIcons();
		int num = 1;
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			GameObject gameObject = new GameObject("MiniShopPuppet" + num);
			gameObject.transform.parent = base.transform;
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerBattleTeamDatum.pupet_profile_id), playerBattleTeamDatum))
			{
				Object.Destroy(gameObject);
			}
		}
	}

	private new void Start()
	{
		Time.timeScale = 0.001f;
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		Vector2 vector = default(Vector2);
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			vector = new Vector2(16f, 32f);
			CreateUIByCellXml("UIMiniStoreNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			vector = new Vector2(44f, 0f);
			CreateUIByCellXml("UIMiniStoreIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UIMiniStoreCfg", m_UIManagerRef[0]);
		}
		hint_open_close = m_UIManagerRef[0].transform.Find("UIMesh").gameObject.AddComponent<UIManagerOpenClose>();
		hint_open_close.Init(m_UIManagerRef[0].GetCameraTransformRect(), new Rect(0f, 0f, 98f, 140f), OnBoardClose);
		hint_open_close.enabled = false;
		hint_open_close.Open();
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.MiniShop_title);
		for (int i = 0; i < msgBoxContent.Count; i++)
		{
			UIText uIText = new UIText();
			uIText.Enable = false;
			uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, Color.black);
			uIText.AlignStyle = UIText.enAlignStyle.center;
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(50f + vector.x, (float)(190 - 17 * i) + vector.y, 380f, 80f);
			string text = msgBoxContent[i];
			uIText.SetText(text);
			m_UIManagerRef[0].Add(uIText);
		}
		msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.MiniShop_Go);
		for (int j = 0; j < msgBoxContent.Count; j++)
		{
			UIText uIText2 = new UIText();
			uIText2.Enable = false;
			uIText2.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, Color.black);
			uIText2.AlignStyle = UIText.enAlignStyle.left;
			uIText2.Rect = D3DMain.Instance.ConvertRectAutoHD(78f + vector.x, (float)(24 - 15 * j) + vector.y, 250f, 80f);
			string text2 = msgBoxContent[j];
			uIText2.SetText(text2);
			m_UIManagerRef[0].Add(uIText2);
		}
		D3DImageCell imageCell = GetImageCell("duibi");
		for (int k = 0; k < 3; k++)
		{
			ShopSlots[k] = new D3DGearSlotUI(m_UIManagerRef[0], this);
			ShopSlots[k].slot_index = k;
			ShopSlots[k].CreateControl(new Vector2((float)(112 + k * 100) + vector.x, 170f + vector.y), "zhuangbeikuang01", fScaleIcon);
			UIImage uIImage = new UIImage();
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.SetPosition(new Vector2((float)(160 + k * 100) + vector.x, 180f + vector.y) * D3DMain.Instance.HD_SIZE);
			uIImage.SetTextureSize(new Vector2(31f, 21f) * 0.7f * D3DMain.Instance.HD_SIZE);
			m_UIManagerRef[0].Add(uIImage);
		}
		for (int l = 0; l < D3DGamer.Instance.PlayerBattleTeamData.Count; l++)
		{
			D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = D3DGamer.Instance.PlayerBattleTeamData[l];
			GameObject gameObject = new GameObject();
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(d3DPuppetSaveData.pupet_profile_id), d3DPuppetSaveData))
			{
				Object.Destroy(gameObject);
				continue;
			}
			puppetBasic.CheckPuppetWeapons();
			D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic.gameObject, 16);
			PlayerTeamPuppetData.Add(puppetBasic);
		}
		UpdateEquipment();
		m_UIManagerRef[0].Add(_movingEquipIcon);
		for (int m = 0; m < _equips.Count; m++)
		{
			UIText uIText3 = new UIText();
			uIText3.Enable = false;
			uIText3.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, Color.black);
			uIText3.AlignStyle = UIText.enAlignStyle.center;
			uIText3.Rect = D3DMain.Instance.ConvertRectAutoHD((float)(95 + m * 100) + vector.x, 153f + vector.y, 100f, 12f);
			string text3 = _equips[m].buy_price_crystal.ToString();
			uIText3.SetText(text3);
			float num = uIText3.GetTextWidth() / (float)D3DMain.Instance.HD_SIZE;
			float num2 = uIText3.Rect.width / (float)D3DMain.Instance.HD_SIZE;
			float num3 = uIText3.Rect.x / (float)D3DMain.Instance.HD_SIZE + (num2 - num) / 2f;
			m_UIManagerRef[0].Add(uIText3);
			UIImage uIImage2 = new UIImage();
			uIImage2.SetTexture(LoadUIMaterialAutoHD(GetImageCell("shuijing").cell_texture), D3DMain.Instance.ConvertRectAutoHD(GetImageCell("shuijing").cell_rect));
			uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(num3 - 19f, 153f + vector.y - 5f, 15f, 24f);
			uIImage2.Enable = false;
			m_UIManagerRef[0].Add(uIImage2);
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (hint_open_close.enabled)
		{
			return;
		}
		if (GetControlId("OpenStore") == control.Id && command == 0)
		{
			GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().OpenShop();
			Object.Destroy(base.gameObject);
		}
		else if (GetControlId("CloseBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			hint_open_close.Close();
		}
		else if (GetControlId("minishop_move") == control.Id && command == 0)
		{
			Vector2 currentPosition = ((UIMove)control).GetCurrentPosition();
			D3DGearSlotUI d3DGearSlotUI = PickGearStoreSlot(currentPosition);
			int num = -1;
			if (d3DGearSlotUI == null)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				if (d3DGearSlotUI == ShopSlots[i])
				{
					num = i;
					break;
				}
			}
			if (num < _equips.Count && num != -1 && _equips[num] != null)
			{
				GameObject original = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UICompare") as GameObject;
				original = (GameObject)Object.Instantiate(original);
				UICompare component = original.GetComponent<UICompare>();
				D3DEquipment selected_gear = _equips[num];
				D3DEquipment compareGear = PlayerTeamPuppetData[num].profile_instance.GetCompareGear(selected_gear);
				component.StartCoroutine(component.UpdateCompareGearsInfo(selected_gear, compareGear, PlayerTeamPuppetData[num].profile_instance, num));
			}
		}
		else if (GetControlId("BuyBtn1") == control.Id && command == 0)
		{
			Buy(0);
		}
		else if (GetControlId("BuyBtn2") == control.Id && command == 0)
		{
			Buy(1);
		}
		else if (GetControlId("BuyBtn3") == control.Id && command == 0)
		{
			Buy(2);
		}
	}

	private void Buy(int nIndex)
	{
		if (nIndex >= _equips.Count || _equips[nIndex] == null)
		{
			return;
		}
		if (int.Parse(D3DGamer.Instance.CurrencyText) < _equips[nIndex].buy_price || int.Parse(D3DGamer.Instance.CrystalText) < _equips[nIndex].buy_price_crystal)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OpenTBank);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			return;
		}
		bool flag = true;
		if (D3DGamer.Instance.PlayerStore.Count < D3DGamer.Instance.ValidStorePage * 12)
		{
			flag = false;
		}
		else
		{
			for (int i = 0; i < D3DGamer.Instance.ValidStorePage * 12; i++)
			{
				if (D3DGamer.Instance.PlayerStore[i] == null)
				{
					flag = false;
					break;
				}
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
			int num = -1;
			for (int j = 0; j < msgBoxContent3.Count; j++)
			{
				if (msgBoxContent3[j].Contains("<GetPrice>"))
				{
					msgBoxContent3[j] = string.Empty;
					num = j;
				}
			}
			List<D3DMessageBoxButtonEvent.OnButtonClick> list2 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list2.Add(IapBuyGearSpace);
			UIManager uIManager = PushMessageBox(msgBoxContent3, D3DMessageBox.MgbButton.CANCEL_OK, list2);
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
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
		D3DGamer.Instance.UpdateCurrency(-_equips[nIndex].buy_price);
		D3DGamer.Instance.UpdateCrystal(-_equips[nIndex].buy_price_crystal);
		D3DGamer.D3DEquipmentSaveData d3DEquipmentSaveData = new D3DGamer.D3DEquipmentSaveData();
		d3DEquipmentSaveData.equipment_id = _equips[nIndex].equipment_id;
		d3DEquipmentSaveData.magic_power_data = _equips[nIndex].magic_power_data;
		int num3 = -1;
		for (int k = 0; k < D3DGamer.Instance.PlayerStore.Count; k++)
		{
			if (D3DGamer.Instance.PlayerStore[k] == null)
			{
				num3 = k;
				break;
			}
		}
		if (num3 == -1)
		{
			num3 = D3DGamer.Instance.PlayerStore.Count;
			D3DGamer.Instance.PlayerStore.Add(d3DEquipmentSaveData);
		}
		else
		{
			D3DGamer.Instance.PlayerStore[num3] = d3DEquipmentSaveData;
		}
		if (!D3DGamer.Instance.NewGearSlotHint.Contains(num3))
		{
			D3DGamer.Instance.NewGearSlotHint.Add(num3);
		}
		GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().UpdateSubUINewHint();
		ShopSlots[nIndex].HideSlot();
		D3DImageCell iconCell = GetIconCell(_equips[nIndex].use_icon);
		_movingEquipIcon.SetTexture(LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		_movingEquipIcon.SetTextureSize(new Vector2(ShopSlots[nIndex].SlotIcon.Rect.width, ShopSlots[nIndex].SlotIcon.Rect.height));
		_movingEquipIcon.SetPosition(new Vector2(117 + nIndex * 100, 140f) * D3DMain.Instance.HD_SIZE);
		_movingEquipIcon.Visible = true;
		_movingEquipIcon.SetScale(1f);
		StartCoroutine("moveEquip");
		_equips[nIndex] = null;
	}

	private IEnumerator moveEquip()
	{
		float fDuration = 0.5f;
		float fTimeLeft = fDuration;
		while (fTimeLeft > 0f)
		{
			fTimeLeft -= Time.deltaTime / Time.timeScale;
			if (fDuration > 0f)
			{
				_movingEquipIcon.SetPosition(_movingEquipIcon.GetPosition() * (fTimeLeft / fDuration) + new Vector2(10f, 20f) * D3DMain.Instance.HD_SIZE);
				_movingEquipIcon.SetScale(fTimeLeft / fDuration);
				yield return 0;
				continue;
			}
			break;
		}
	}

	private void OnBoardClose()
	{
		Time.timeScale = 1f;
		Object.Destroy(base.gameObject);
	}

	private void UpdateEquipment()
	{
		_equips.Clear();
		for (int i = 0; i < PlayerTeamPuppetData.Count; i++)
		{
			D3DEquipment bestWeapon = D3DShopRuleEx.Instance.GetBestWeapon(D3DGamer.Instance.PlayerBattleTeamData[i]);
			ShopSlots[i].UpdateGearSlot(bestWeapon, PlayerTeamPuppetData[i].profile_instance, fScaleIcon);
			_equips.Add(bestWeapon);
		}
	}

	protected new void AddItemIcons()
	{
		int num = 0;
		while (true)
		{
			TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UIImgCell", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UI_Icons" + num + "_cell", D3DGamer.Instance.Sk[0]))) as TextAsset;
			if (null == textAsset)
			{
				break;
			}
			string text = XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]);
			while (text != string.Empty)
			{
				int num2 = text.IndexOf('\n');
				string text2 = text.Substring(0, num2);
				text = text.Remove(0, num2 + 1);
				num2 = text2.IndexOf('\t');
				string text3 = text2.Substring(0, num2);
				text2 = text2.Remove(0, num2 + 1);
				num2 = text2.IndexOf('\t');
				string texture = text2.Substring(0, num2);
				text2 = text2.Remove(0, num2 + 1);
				string text4 = text2;
				text4 = text4.Trim();
				string[] array = text4.Split(',');
				Rect rect = new Rect(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
				if (string.Empty != text3 && !UIImageCellIndexer.ContainsKey(text3))
				{
					UIImageCellIndexer.Add(text3, new D3DImageCell(text3, texture, rect));
				}
			}
			num++;
		}
	}

	private D3DGearSlotUI PickGearStoreSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[0].TouchPointOnManager(touch_point2);
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
		D3DGamer.Instance.ValidStorePage++;
		if (D3DGamer.Instance.ValidStorePage > 5)
		{
			D3DGamer.Instance.ValidStorePage = 5;
		}
	}

	public void OpenTBank()
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
		D3DMain.Instance.LoadingScene = 4;
		UIStash.EnabledOptions = new UIStash.StashOption[1] { UIStash.StashOption.tBANK };
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}
}
