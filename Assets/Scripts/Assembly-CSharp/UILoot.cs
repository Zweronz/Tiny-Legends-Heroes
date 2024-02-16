using System;
using System.Collections.Generic;
using UnityEngine;

public class UILoot : UIHelper
{
	private enum LootUIManager
	{
		MAIN = 0,
		LOOT_STORE = 1,
		GEAR_STORE = 2,
		GEAR_DESCRIPTION = 3,
		TBANK = 4,
		MASK1 = 5,
		MASK2 = 6
	}

	private enum LootTouchArea
	{
		GEAR_STORE = 0,
		LOOT_STORE = 1,
		GEAR_DESCRIPTION = 2,
		LITTER_BIN = 3
	}

	private List<PuppetBasic> PlayerTeamPuppetData;

	private D3DTextPushButton[] OptionBtns;

	private int CurrentOptionIndex;

	private D3DCurrencyText PlayerCurrencyText;

	private D3DLitterBin LootLitterBin;

	private SubUIPuppetFace _SubPuppetFaceUI = new SubUIPuppetFace();

	private D3DGearSlotUI[] LootSlots;

	private D3DGearSlotUI ActivingLootGear;

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

	private Rect[] LootTouchAreas;

	private void CreateLootPuppet()
	{
		if (PlayerTeamPuppetData != null)
		{
			return;
		}
		PlayerTeamPuppetData = new List<PuppetBasic>();
		int num = 1;
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			GameObject gameObject = new GameObject("LootPuppet" + num);
			gameObject.transform.parent = base.transform;
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerBattleTeamDatum.pupet_profile_id), playerBattleTeamDatum))
			{
				UnityEngine.Object.Destroy(gameObject);
				continue;
			}
			puppetBasic.model_builder.BuildPuppetModel();
			puppetBasic.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, UnityEngine.Random.Range(0f, 2f));
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
		CreateUIByCellXml("UILootMainCfg", m_UIManagerRef[0]);
		string[] array = new string[2] { "STASH", "tBANK" };
		OptionBtns = new D3DTextPushButton[2];
		for (int i = 0; i < 2; i++)
		{
			OptionBtns[i] = new D3DTextPushButton(m_UIManagerRef[0], this);
			OptionBtns[i].CreateControl(new Vector2(82 * i, 283f), new Vector2(84f, 37f), "anniu1", "anniu2", string.Empty, D3DMain.Instance.GameFont2.FontName, 11, 22, array[i], (D3DMain.Instance.HD_SIZE != 2) ? new Vector2(0f, 1f) : new Vector2(0f, -3f), (float)D3DMain.Instance.HD_SIZE * 1.5f, D3DMain.Instance.CommonFontColor, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f));
		}
		OptionBtns[0].Set(true);
		OptionBtns[1].Enable(false);
		OptionBtns[1].Visible(false);
		PlayerCurrencyText = new D3DCurrencyText(m_UIManagerRef[0], this);
		UpdateCurrencyUI();
		LootLitterBin = new D3DLitterBin(m_UIManagerRef[0], this);
		LootLitterBin.CreateLitterBin(new Vector2(425f, 58f));
		CreateLootPuppet();
		_SubPuppetFaceUI.CreatePuppetFaceUI(this, 0, 6, OnSelectAnotherPuppetFace);
		DragIcon = new UIImage();
		DragIcon.SetAlpha(0.7f);
		DragIcon.Enable = false;
		DragIcon.Visible = false;
		m_UIManagerRef[6].Add(DragIcon);
		InsertUIManager("Manager_Mask1", 5);
		m_UIManagerRef[5].EnableUIHandler = false;
		GetControl("LootTxt").Visible = false;
		GetControl("LootBtn").Enable = false;
	}

	private void OnSelectAnotherPuppetFace(int nFaceIndex)
	{
		UpdateCurrentGearPageStore();
		UpdateLootStore();
		if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
		else if (ActivingLootGear != null)
		{
			UpdateSelectEquipInfoInLoot();
		}
	}

	private void LeaveLoot()
	{
		D3DMain.Instance.LootEquipments.Clear();
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
		string text = 9426648.ToString().Insert(5, Convert.ToChar(70).ToString() + Convert.ToChar(67) + Convert.ToChar(65));
		if (text == D3DGamer.Instance.Claim)
		{
			text = 6964854.ToString();
			text += 1164101;
			D3DGamer.Instance.Claim = text.Insert(3, Convert.ToChar(67).ToString());
			D3DGamer.Instance.Claim = D3DGamer.Instance.Claim.Insert(12, Convert.ToChar(67).ToString());
		}
		D3DGamer.Instance.SaveAllData();
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, CloseLoot, false);
	}

	private void CloseLoot()
	{
		if (D3DMain.Instance.CurrentScene == 8)
		{
			if (D3DDungeonProgerssManager.Instance.CurrentDungeonProgress != null && D3DDungeonProgerssManager.Instance.CurrentDungeonProgress.ContainsKey(D3DMain.Instance.exploring_dungeon.current_floor))
			{
				D3DDungeonProgerssManager.LevelProgress levelProgress = D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor];
				if (levelProgress.UnlockBattleList.ContainsKey(D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id))
				{
					D3DDungeonProgerssManager.LevelProgress.NextLevelBattleUnlock nextLevelBattleUnlock = levelProgress.UnlockBattleList[D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id];
					if (string.Empty != nextLevelBattleUnlock.on_battle_win_story && !nextLevelBattleUnlock.win_read && (string.Empty == nextLevelBattleUnlock.target_group || nextLevelBattleUnlock.target_group == D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id))
					{
						GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIStory"));
						gameObject.GetComponent<UIStory>().Init(nextLevelBattleUnlock.on_battle_win_story, StoryBackToDungeon);
						return;
					}
				}
			}
			D3DMain.Instance.LoadingScene = 3;
			SwitchLevelImmediately();
			D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
		}
		else if (D3DMain.Instance.CurrentScene == 3)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			if (uIHelper is UIDungeon)
			{
				uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, ((UIDungeon)uIHelper).ChestLootEnd, true);
				uIHelper.GetManager(0).gameObject.SetActiveRecursively(true);
				if (null != D3DMain.Instance.HD_BOARD_OBJ)
				{
					D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
				}
			}
			else if (uIHelper is UIStash)
			{
				uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, ((UIStash)uIHelper).UpdateStashGearStoreByIAP, true);
			}
			else if (uIHelper is UIShop)
			{
				uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, ((UIShop)uIHelper).UpdateStashGearStoreByIAP, true);
			}
			else
			{
				uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, null, true);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (D3DMain.Instance.CurrentScene == -1)
		{
			D3DMain.Instance.LoadingScene = 3;
			D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
			D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.PREVIOUS;
			D3DMain.Instance.exploring_dungeon.current_floor = 0;
			if (D3DDungeonProgerssManager.Instance.DungeonProgressManager.ContainsKey(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id))
			{
				D3DDungeonProgerssManager.Instance.CurrentDungeonProgress = D3DDungeonProgerssManager.Instance.DungeonProgressManager[D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id];
			}
			else
			{
				D3DDungeonProgerssManager.Instance.CurrentDungeonProgress = null;
			}
			if (null != D3DAudioManager.Instance.DungeonAmbAudio)
			{
				D3DAudioManager.Instance.DungeonAmbAudio.Stop();
				D3DAudioManager.Instance.DungeonAmbAudio = null;
			}
			if (null != D3DAudioManager.Instance.DungeonTownAudio)
			{
				D3DAudioManager.Instance.DungeonTownAudio.Stop();
				D3DAudioManager.Instance.DungeonTownAudio = null;
			}
			Time.timeScale = 1f;
			SwitchLevelImmediately();
		}
		else
		{
			UIHelper uIHelper2 = D3DMain.Instance.D3DUIList[ui_index - 2];
			if (uIHelper2 is UIStash)
			{
				uIHelper2.ui_fade.StartFade(UIFade.FadeState.FADE_IN, ((UIStash)uIHelper2).UpdateStashGearStoreByIAP, true);
			}
			else if (uIHelper2 is UIShop)
			{
				uIHelper2.ui_fade.StartFade(UIFade.FadeState.FADE_IN, ((UIShop)uIHelper2).UpdateStashGearStoreByIAP, true);
			}
			else
			{
				uIHelper2.ui_fade.StartFade(UIFade.FadeState.FADE_IN, null, true);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void StoryBackToDungeon()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor].UnlockBattleList[D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id].win_read = true;
		D3DGamer.Instance.SaveDungeonProgress();
		D3DMain.Instance.LoadingScene = 3;
		SwitchLevelImmediately();
		D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
	}

	private bool LootOptionsEvent(UIControl control)
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
					SwitchLootOption(num);
				}
				return true;
			}
			num++;
		}
		return false;
	}

	private void SwitchLootOption(int option_index)
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
			UpdateLootStore();
			UpdateCurrentGearPageStore();
			if (ActivingStoreGear != null)
			{
				UpdateSelectedEquipInfo();
			}
			else if (ActivingLootGear != null)
			{
				DeSelectLootEquip();
				GearDescriptionUI.Visible(false);
				ActivingLootGear = null;
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

	private void CreateLootStoreUI()
	{
		if (!(null != m_UIManagerRef[1]))
		{
			InsertUIManager("Manager_LootStore", 1);
			m_UIManagerRef[1].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(208f, 8f, 206f, 271f));
			UIImage uIImage = new UIImage();
			uIImage.Enable = false;
			D3DImageCell imageCell = GetImageCell("store-youlan");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 224f, 205f, 46f);
			m_UIManagerRef[1].Add(uIImage);
			uIImage = new UIImage();
			uIImage.Enable = false;
			imageCell = GetImageCell("loot");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(77f, 241f, 53f, 15f);
			m_UIManagerRef[1].Add(uIImage);
			LootSlots = new D3DGearSlotUI[12];
			for (int i = 0; i < 12; i++)
			{
				LootSlots[i] = new D3DGearSlotUI(m_UIManagerRef[1], this);
				LootSlots[i].slot_index = i;
				LootSlots[i].CreateControl(new Vector2(9 + i % 4 * 48, 170 - i / 4 * 46), "zhuangbeikuang");
			}
			UIClickButton uIClickButton = new UIClickButton();
			imageCell = GetImageCell("anniu1");
			uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(90f, 37f) * D3DMain.Instance.HD_SIZE);
			imageCell = GetImageCell("anniu2");
			uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(90f, 37f) * D3DMain.Instance.HD_SIZE);
			uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(58f, 5f, 90f, 37f);
			m_UIManagerRef[1].Add(uIClickButton);
			m_control_table.Add("GetallBtn", uIClickButton);
			UIText uIText = new UIText();
			uIText.Enable = false;
			uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), "GET ALL", D3DMain.Instance.CommonFontColor);
			uIText.AlignStyle = UIText.enAlignStyle.center;
			uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(64f, -7f, 84f, 37f);
			m_UIManagerRef[1].Add(uIText);
		}
	}

	private D3DGearSlotUI PickLootSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[1].TouchPointOnManager(touch_point2);
		touch_point2 *= 1f / num;
		D3DGearSlotUI[] lootSlots = LootSlots;
		foreach (D3DGearSlotUI d3DGearSlotUI in lootSlots)
		{
			if (d3DGearSlotUI != null && d3DGearSlotUI.PtInSlot(touch_point2))
			{
				return d3DGearSlotUI;
			}
		}
		return null;
	}

	private void UpdateLootStore()
	{
		GearDescriptionUI.Visible(false);
		for (int i = 0; i < LootSlots.Length; i++)
		{
			LootSlots[i].HideSlot();
			if (i < D3DMain.Instance.LootEquipments.Count && D3DMain.Instance.LootEquipments[i] != null)
			{
				LootSlots[i].UpdateGearSlot(D3DMain.Instance.LootEquipments[i], PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance);
			}
		}
		if (ActivingLootGear != null)
		{
			UpdateSelectEquipInfoInLoot();
		}
	}

	private void InstantGetLoot(int index)
	{
		bool flag = true;
		if (index >= D3DMain.Instance.LootEquipments.Count)
		{
			return;
		}
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage; i++)
		{
			int num = ThrowInPage(D3DMain.Instance.LootEquipments[index], i);
			if (num >= 0)
			{
				D3DMain.Instance.LootEquipments[index] = null;
				if (ActivingLootGear != null && index == ActivingLootGear.slot_index)
				{
					ActivingStoreGear = null;
				}
				flag = false;
				D3DGamer.Instance.NewGearSlotHint.Add(num);
				break;
			}
		}
		UpdateLootStore();
		UpdateCurrentGearPageStore();
		if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
		else if (ActivingLootGear != null)
		{
			DeSelectLootEquip();
			GearDescriptionUI.Visible(false);
			ActivingLootGear = null;
		}
		if (!flag)
		{
			return;
		}
		if (D3DGamer.Instance.ValidStorePage >= 5)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_GET_ALL_BUT_STORE_FULL_MAX_PAGE);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
			return;
		}
		List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_GET_ALL_BUT_STORE_FULL_CAN_UNLOCK);
		msgBoxContent2 = new List<string>(msgBoxContent2);
		int num2 = -1;
		for (int j = 0; j < msgBoxContent2.Count; j++)
		{
			if (msgBoxContent2[j].Contains("<GetPrice>"))
			{
				msgBoxContent2[j] = string.Empty;
				num2 = j;
			}
		}
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		list.Add(IapBuyGearSpace);
		UIManager uIManager = PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list);
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
	}

	private void GetAllLoots()
	{
		bool flag = true;
		if (D3DMain.Instance.LootEquipments.Count == 0)
		{
			return;
		}
		for (int i = 0; i < D3DMain.Instance.LootEquipments.Count; i++)
		{
			flag = true;
			if (D3DMain.Instance.LootEquipments[i] == null)
			{
				flag = false;
				continue;
			}
			for (int j = 0; j < D3DGamer.Instance.ValidStorePage; j++)
			{
				int num = ThrowInPage(D3DMain.Instance.LootEquipments[i], j);
				if (num >= 0)
				{
					D3DMain.Instance.LootEquipments[i] = null;
					if (ActivingLootGear != null && i == ActivingLootGear.slot_index)
					{
						ActivingStoreGear = null;
					}
					flag = false;
					D3DGamer.Instance.NewGearSlotHint.Add(num);
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		UpdateLootStore();
		UpdateCurrentGearPageStore();
		if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
		else if (ActivingLootGear != null)
		{
			DeSelectLootEquip();
			GearDescriptionUI.Visible(false);
			ActivingLootGear = null;
		}
		if (!flag)
		{
			return;
		}
		if (D3DGamer.Instance.ValidStorePage >= 5)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_GET_ALL_BUT_STORE_FULL_MAX_PAGE);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
			return;
		}
		List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_GET_ALL_BUT_STORE_FULL_CAN_UNLOCK);
		msgBoxContent2 = new List<string>(msgBoxContent2);
		int num2 = -1;
		for (int k = 0; k < msgBoxContent2.Count; k++)
		{
			if (msgBoxContent2[k].Contains("<GetPrice>"))
			{
				msgBoxContent2[k] = string.Empty;
				num2 = k;
			}
		}
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		list.Add(IapBuyGearSpace);
		UIManager uIManager = PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list);
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
	}

	private void RemoveLootGearToBin()
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_DESTORY), null, false, false);
		D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index] = null;
		DeSelectLootEquip();
		ActivingLootGear = null;
		UpdateLootStore();
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
				if (ActivingLootGear != null)
				{
					UpdateSelectEquipInfoInLoot();
				}
				return true;
			}
			num++;
		}
		return false;
	}

	private void UpdateCurrentGearPageStore()
	{
		GearDescriptionUI.Visible(false);
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

	private void RemoveGearToBin()
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_DESTORY), null, false, false);
		PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12] = null;
		DeSelectStoreEquip();
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

	private void OpenTBank()
	{
		OptionBtns[CurrentOptionIndex].Set(false);
		OptionBtns[1].Visible(true);
		OptionBtns[1].Enable(true);
		OptionBtns[1].Set(true);
		CurrentOptionIndex = -1;
		SwitchLootOption(1);
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

	private void CreateLootTouchAreas()
	{
		LootTouchAreas = new Rect[4]
		{
			new Rect(13 * D3DMain.Instance.HD_SIZE, 138 * D3DMain.Instance.HD_SIZE, 186 * D3DMain.Instance.HD_SIZE, 136 * D3DMain.Instance.HD_SIZE),
			new Rect(214 * D3DMain.Instance.HD_SIZE, 80 * D3DMain.Instance.HD_SIZE, 198 * D3DMain.Instance.HD_SIZE, 150 * D3DMain.Instance.HD_SIZE),
			new Rect(10 * D3DMain.Instance.HD_SIZE, 9 * D3DMain.Instance.HD_SIZE, 192 * D3DMain.Instance.HD_SIZE, 86 * D3DMain.Instance.HD_SIZE),
			new Rect(430 * D3DMain.Instance.HD_SIZE, 61 * D3DMain.Instance.HD_SIZE, 42 * D3DMain.Instance.HD_SIZE, 42 * D3DMain.Instance.HD_SIZE)
		};
	}

	private int GetLootTouchArea(Vector2 touch_position)
	{
		for (int i = 0; i <= 3; i++)
		{
			if (LootTouchAreas[i].Contains(touch_position))
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

	private void LootTouchEvent(Vector2 touch_point, int touch_command, float x_delta, float y_delta)
	{
		switch (touch_command)
		{
		case 0:
			StartTouchArea = GetLootTouchArea(touch_point);
			switch (StartTouchArea)
			{
			case 2:
				GearDescriptionUI.StopInertia();
				break;
			case 0:
			{
				D3DGearSlotUI d3DGearSlotUI5 = PickGearStoreSlot(touch_point);
				if (d3DGearSlotUI5 == null)
				{
					break;
				}
				int index3 = d3DGearSlotUI5.slot_index + CurrentGearPage * 12;
				if (PlayerStore[index3] == null)
				{
					break;
				}
				if (d3DGearSlotUI5 != ActivingStoreGear)
				{
					if (ActivingLootGear != null)
					{
						DeSelectLootEquip();
						ActivingLootGear = null;
					}
					if (ActivingStoreGear != null)
					{
						DeSelectStoreEquip();
					}
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					ActivingStoreGear = d3DGearSlotUI5;
					ActivingStoreGear.Select(true);
					if (D3DGamer.Instance.NewGearSlotHint.Contains(CurrentGearPage * 12 + ActivingStoreGear.slot_index))
					{
						D3DGamer.Instance.NewGearSlotHint.Remove(CurrentGearPage * 12 + ActivingStoreGear.slot_index);
						ActivingStoreGear.NewHint.Visible = false;
					}
					UpdateSelectedEquipInfo();
				}
				EnableDrag = true;
				break;
			}
			case 1:
			{
				D3DGearSlotUI d3DGearSlotUI4 = PickLootSlot(touch_point);
				if (d3DGearSlotUI4 == null)
				{
					break;
				}
				int slot_index = d3DGearSlotUI4.slot_index;
				if (slot_index >= D3DMain.Instance.LootEquipments.Count || D3DMain.Instance.LootEquipments[slot_index] == null)
				{
					break;
				}
				if (d3DGearSlotUI4 != ActivingLootGear)
				{
					if (ActivingStoreGear != null)
					{
						DeSelectStoreEquip();
						ActivingStoreGear = null;
					}
					if (ActivingLootGear != null)
					{
						DeSelectLootEquip();
					}
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					ActivingLootGear = d3DGearSlotUI4;
					ActivingLootGear.Select(true);
					UpdateSelectEquipInfoInLoot();
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
					int index4 = ActivingStoreGear.slot_index + CurrentGearPage * 12;
					D3DEquipment d3DEquipment2 = PlayerStore[index4];
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPickUpSfx((int)d3DEquipment2.equipment_class), null, false, false);
				}
				else if (ActivingLootGear != null)
				{
					SetDragIcon(ActivingLootGear.SlotIcon);
					DragIcon.Visible = true;
					SetDragIconPosition(touch_point);
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPickUpSfx((int)D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index].equipment_class), null, false, false);
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
				if (!DragIcon.Visible || !EnableDrag || (ActivingStoreGear == null && ActivingLootGear == null))
				{
					break;
				}
				SetDragIconPosition(touch_point);
				switch (GetLootTouchArea(touch_point))
				{
				case 0:
				{
					LootLitterBin.Hover(false);
					PageHoverMask.Visible = false;
					D3DGearSlotUI d3DGearSlotUI3 = PickGearStoreSlot(touch_point);
					if (d3DGearSlotUI3 != null)
					{
						if (HoverGear != null)
						{
							if (d3DGearSlotUI3 != HoverGear)
							{
								HoverGear.SetHover(false, true);
								HoverGear = d3DGearSlotUI3;
								HoverGear.SetHover(true, true);
							}
						}
						else
						{
							HoverGear = d3DGearSlotUI3;
							HoverGear.SetHover(true, true);
						}
					}
					else if (HoverGear != null)
					{
						HoverGear.SetHover(false, true);
						HoverGear = null;
					}
					break;
				}
				case 3:
					LootLitterBin.Hover(true);
					break;
				default:
					if (HoverGear != null)
					{
						HoverGear.SetHover(false, true);
						HoverGear = null;
					}
					LootLitterBin.Hover(false);
					GearDragOnPage(touch_point);
					break;
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
					int lootTouchArea = GetLootTouchArea(touch_point);
					int index = ActivingStoreGear.slot_index + CurrentGearPage * 12;
					D3DEquipment d3DEquipment = PlayerStore[index];
					if (DoingDrag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPutDownSfx((int)d3DEquipment.equipment_class), null, false, false);
					}
					switch (lootTouchArea)
					{
					case 0:
					{
						D3DGearSlotUI d3DGearSlotUI = PickGearStoreSlot(touch_point);
						if (d3DGearSlotUI != null && d3DGearSlotUI != ActivingStoreGear)
						{
							int num2 = d3DGearSlotUI.slot_index + CurrentGearPage * 12;
							if (D3DGamer.Instance.NewGearSlotHint.Contains(num2))
							{
								D3DGamer.Instance.NewGearSlotHint.Remove(num2);
							}
							D3DEquipment value = PlayerStore[num2];
							PlayerStore[num2] = PlayerStore[index];
							PlayerStore[index] = value;
							GearDescriptionUI.Visible(false);
							DeSelectStoreEquip();
							ActivingStoreGear = null;
							UpdateCurrentGearPageStore();
						}
						break;
					}
					case 3:
					{
						List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_DESTROY_GEAR);
						msgBoxContent = new List<string>(msgBoxContent);
						Dictionary<int, Color> dictionary = null;
						for (int i = 0; i < msgBoxContent.Count; i++)
						{
							if (msgBoxContent[i].Contains("<GetGear>"))
							{
								if (dictionary == null)
								{
									dictionary = new Dictionary<int, Color>();
								}
								msgBoxContent[i] = msgBoxContent[i].Replace("<GetGear>", PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12].equipment_name);
								dictionary.Add(i, D3DMain.Instance.GetEquipmentGradeColor(PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12].equipment_grade));
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list.Add(RemoveGearToBin);
						PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list, false, dictionary);
						break;
					}
					default:
					{
						int num = GearDragOnPage(touch_point);
						if (num != -1 && num != CurrentGearPage && ThrowInPage(PlayerStore[index], num) >= 0)
						{
							PlayerStore[index] = null;
							GearDescriptionUI.Visible(false);
							DeSelectStoreEquip();
							ActivingStoreGear = null;
							UpdateCurrentGearPageStore();
						}
						break;
					}
					}
				}
				else
				{
					if (ActivingLootGear == null)
					{
						break;
					}
					if (DoingDrag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPutDownSfx((int)D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index].equipment_class), null, false, false);
					}
					if (InstantClick)
					{
						InstantGetLoot(ActivingLootGear.slot_index);
						InstantClick = false;
						break;
					}
					switch (GetLootTouchArea(touch_point))
					{
					case 0:
					{
						D3DGearSlotUI d3DGearSlotUI2 = PickGearStoreSlot(touch_point);
						if (d3DGearSlotUI2 == null)
						{
							break;
						}
						int index2 = d3DGearSlotUI2.slot_index + CurrentGearPage * 12;
						if (PlayerStore[index2] == null)
						{
							PlayerStore[index2] = D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index];
							D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index] = null;
						}
						else
						{
							int num5 = ThrowInPage(D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index], CurrentGearPage);
							if (num5 < 0)
							{
								break;
							}
							D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index] = null;
							D3DGamer.Instance.NewGearSlotHint.Add(num5);
						}
						GearDescriptionUI.Visible(false);
						DeSelectLootEquip();
						ActivingLootGear = null;
						UpdateCurrentGearPageStore();
						UpdateLootStore();
						break;
					}
					case 3:
					{
						List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_DESTROY_GEAR);
						List<D3DMessageBoxButtonEvent.OnButtonClick> list2 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						msgBoxContent2 = new List<string>(msgBoxContent2);
						Dictionary<int, Color> dictionary2 = null;
						for (int j = 0; j < msgBoxContent2.Count; j++)
						{
							if (msgBoxContent2[j].Contains("<GetGear>"))
							{
								if (dictionary2 == null)
								{
									dictionary2 = new Dictionary<int, Color>();
								}
								msgBoxContent2[j] = msgBoxContent2[j].Replace("<GetGear>", D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index].equipment_name);
								dictionary2.Add(j, D3DMain.Instance.GetEquipmentGradeColor(D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index].equipment_grade));
							}
						}
						list2.Add(RemoveLootGearToBin);
						PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list2, false, dictionary2);
						break;
					}
					default:
					{
						int num3 = GearDragOnPage(touch_point);
						if (num3 != -1)
						{
							int num4 = ThrowInPage(D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index], num3);
							if (num4 >= 0)
							{
								D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index] = null;
								DeSelectLootEquip();
								ActivingLootGear = null;
								UpdateCurrentGearPageStore();
								UpdateLootStore();
								D3DGamer.Instance.NewGearSlotHint.Add(num4);
							}
						}
						break;
					}
					}
				}
				break;
			}
			StartTouchArea = -1;
			LootLitterBin.Hover(false);
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

	private new void Awake()
	{
		base.name = "UILoot";
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
		for (LootUIManager lootUIManager = LootUIManager.MAIN; lootUIManager <= LootUIManager.MASK2; lootUIManager++)
		{
			CreateUIManagerEmpty();
		}
		CreateMainUI();
		CreateLootStoreUI();
		CreateGearsStoreUI();
		CreateGearDescriptionUI();
		UpdateCurrentGearPageStore();
		UpdateLootStore();
		_subUItBank.CreateTBankUI(4, this, UpdateCurrencyUI, _SubPuppetFaceUI.UpdateFaceFrame);
		CreateLootTouchAreas();
		CurrentOptionIndex = -1;
		SwitchLootOption(0);
		if (D3DMain.Instance.CurrentScene != 8 && ui_index > 1)
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
		if (GetControlId("loot_move") == control.Id)
		{
			Vector2 currentPosition = ((UIMove)control).GetCurrentPosition();
			LootTouchEvent(currentPosition, command, wparam, lparam);
			return;
		}
		if (GetControlId("OKBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			if (D3DMain.Instance.LootEquipments.Count > 0)
			{
				foreach (D3DEquipment lootEquipment in D3DMain.Instance.LootEquipments)
				{
					if (lootEquipment != null)
					{
						List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_LOOT_NOT_GET_ALL_QUIT);
						List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list.Add(LeaveLoot);
						PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
						return;
					}
				}
				LeaveLoot();
			}
			else
			{
				LeaveLoot();
			}
			return;
		}
		if (GetControl("GetallBtn") == control && command == 0)
		{
			if (!DoingDrag)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				GetAllLoots();
			}
			return;
		}
		if (GearDescriptionUI.CompareButton == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_COMPARE), null, false, false);
			GameObject original = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UICompare") as GameObject;
			original = (GameObject)UnityEngine.Object.Instantiate(original);
			UICompare component = original.GetComponent<UICompare>();
			if (ActivingStoreGear != null)
			{
				D3DEquipment selected_gear = PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12];
				D3DEquipment compareGear = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetCompareGear(selected_gear);
				component.StartCoroutine(component.UpdateCompareGearsInfo(selected_gear, compareGear, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, _SubPuppetFaceUI.CurrentFaceIndex));
			}
			else if (ActivingLootGear != null)
			{
				D3DEquipment selected_gear2 = D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index];
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
		if (!GearStorePageEvent(control) && !_SubPuppetFaceUI.PuppetFaceEvent(control) && !LootOptionsEvent(control) && !_subUItBank.tBankEvent(control))
		{
		}
	}

	private void CheckTutorial()
	{
		if (!D3DGamer.Instance.TutorialState[2])
		{
			((GameObject)UnityEngine.Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_GET_LOOT);
		}
	}

	private void UpdateSelectEquipInfoInLoot()
	{
		ActivingLootGear.Select(true);
		GearDescriptionUI.UpdateDescriptionInfo(D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index], false, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, true);
		GearDescriptionUI.Visible(true);
		OnSelectAnEquip(true);
	}

	private void UpdateSelectedEquipInfo()
	{
		ActivingStoreGear.Select(true);
		GearDescriptionUI.UpdateDescriptionInfo(PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12], false, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, true);
		GearDescriptionUI.Visible(true);
		OnSelectAnEquip(false);
	}

	private void OnSelectAnEquip(bool bIsLoot)
	{
		D3DEquipment d3DEquipment = ((!bIsLoot) ? PlayerStore[ActivingStoreGear.slot_index + CurrentGearPage * 12] : D3DMain.Instance.LootEquipments[ActivingLootGear.slot_index]);
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
		}
	}

	private void DeSelectLootEquip()
	{
		ActivingLootGear.Select(false);
		_SubPuppetFaceUI.HideAll();
	}

	private void DeSelectStoreEquip()
	{
		ActivingStoreGear.Select(false);
		_SubPuppetFaceUI.HideAll();
	}
}
