using System.Collections.Generic;
using UnityEngine;

public class UIStash : UIHelper
{
	private enum StashUIManager
	{
		MAIN = 0,
		GEAR_PUPPET = 1,
		GEAR_PUPPET_PROPERTY = 2,
		GEAR_STORE = 3,
		GEAR_DESCRIPTION = 4,
		SKILL_SET = 5,
		SKILL_SCROLL = 6,
		TBANK = 7,
		MASK1 = 8,
		MASK2 = 9
	}

	public enum StashOption
	{
		GEARS = 0,
		SKILLS = 1,
		ITEMS = 2,
		tBANK = 3,
		ACTIVE = 4,
		PASSIVE = 5
	}

	private enum GearsTouchArea
	{
		GEAR_STORE = 0,
		PUPPET_GEAR_LEFT = 1,
		PUPPET_GEAR_RIGHT = 2,
		PUPPET_GEAR_BOTTOM = 3,
		PUPPET_FEATURE = 4,
		GEAR_DESCRIPTION = 5,
		PUPPET_PROPERTY = 6,
		LITTER_BIN = 7
	}

	private enum SkillSetTouchArea
	{
		ACTIVE_TAG = 0,
		PASSIVE_TAG = 1,
		ACTIVE_SLOTS = 2,
		PASSIVE_SLOTS = 3,
		SKILL_SCROLL = 4
	}

	public static StashOption[] EnabledOptions;

	private int CurrentOptionIndex;

	private List<PuppetBasic> PlayerTeamPuppetData;

	private D3DTextPushButton[] OptionBtns;

	private D3DCurrencyText PlayerCurrencyText;

	private D3DLitterBin StashLitterBin;

	private UIClickButton TapJoyBtn;

	private SubUIPuppetFace _SubPuppetFaceUI = new SubUIPuppetFace();

	private NewHintBehaviour newHintBehaviour;

	private D3DFeatureCameraUI PuppetFeatureUI;

	private D3DPuppetGearSlotUI[] PuppetGearSlots;

	private D3DPuppetGearSlotUI ActivingPuppetGear;

	private D3DUIPuppetProperty PuppetPropertyUI;

	private List<D3DEquipment> GearStore;

	private D3DTextPushButton[] GearPageBtns;

	private List<UIClickButton> UnlockPageBtns;

	private int CurrentGearPage;

	private UIImage PageHoverMask;

	private D3DGearSlotUI[] GearStoreSlots;

	private D3DGearSlotUI ActivingStoreGear;

	private D3DUIGearDescription GearDescriptionUI;

	private D3DSkillSlotUI[] ActiveSkillSlots;

	private D3DSkillSlotUI[] PassiveSkillSlots;

	private D3DSkillSlotUI ActivingSkillSlot;

	private bool ActiveTagActiving = true;

	private D3DSkillScroll SkillScroll;

	private D3DSkillBarUI ActivingSkillBar;

	private D3DClassSkillStatus UpgradeSkill;

	private UIText SkillInfoText;

	private bool DoSkillScroll;

	private UIImage[] OptionHintImg;

	private SubUItBank _subUItBank = new SubUItBank();

	private UIImage DoubleHandMask;

	private UIImage DragIcon;

	private int StartTouchArea;

	private bool EnableDrag;

	private bool DoingDrag;

	private bool InstantClick;

	private D3DComplexSlotUI HoverGear;

	private Rect[] GearsTouchAreas;

	private int ArmHintIndex;

	private Rect[] SkillSetTouchAreas;

	private Rect SkillSchoolTouchArea;

	private void CreateStashManagerByOption(StashOption stash_option)
	{
		switch (stash_option)
		{
		case StashOption.GEARS:
			CreateGearPuppetUI();
			CreateGearPuppetPropertyUI();
			CreateGearsStoreUI();
			CreateGearDescriptionUI();
			UpdateCurrentGearPageStore();
			UpdateCurrentPuppetInfo(true);
			CreateGearsTouchAreas();
			break;
		case StashOption.SKILLS:
			CreateSkillUI();
			CreateSkillScrollUI();
			CreateSkillSetTouchAreas();
			break;
		case StashOption.ACTIVE:
		case StashOption.PASSIVE:
			CreateSkillScrollUI();
			CreateSkillSchoolTouchArea();
			break;
		case StashOption.tBANK:
			_subUItBank.CreateTBankUI(7, this, UpdateCurrencyUI, _SubPuppetFaceUI.UpdateFaceFrame);
			break;
		case StashOption.ITEMS:
			break;
		}
	}

	private void CreateStashPuppet()
	{
		if (PlayerTeamPuppetData != null)
		{
			return;
		}
		PlayerTeamPuppetData = new List<PuppetBasic>();
		int num = 1;
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			GameObject gameObject = new GameObject("StashPuppet" + num);
			gameObject.transform.parent = base.transform;
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerBattleTeamDatum.pupet_profile_id), playerBattleTeamDatum))
			{
				Object.Destroy(gameObject);
				continue;
			}
			puppetBasic.profile_instance.InitSkillLevel(playerBattleTeamDatum);
			puppetBasic.profile_instance.InitSkillSlots(playerBattleTeamDatum);
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
		CreateUIByCellXml("UIStashMainCfg", m_UIManagerRef[0]);
		OptionBtns = new D3DTextPushButton[EnabledOptions.Length];
		for (int i = 0; i < EnabledOptions.Length; i++)
		{
			OptionBtns[i] = new D3DTextPushButton(m_UIManagerRef[0], this);
			OptionBtns[i].CreateControl(new Vector2(82 * i, 283f), new Vector2(84f, 37f), "anniu1", "anniu2", string.Empty, D3DMain.Instance.GameFont2.FontName, 11, 22, EnabledOptions[i].ToString(), (D3DMain.Instance.HD_SIZE != 2) ? new Vector2(0f, 1f) : new Vector2(0f, -3f), (float)D3DMain.Instance.HD_SIZE * 1.5f, D3DMain.Instance.CommonFontColor, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f));
		}
		CurrentOptionIndex = -1;
		OptionBtns[0].Set(true);
		PlayerCurrencyText = new D3DCurrencyText(m_UIManagerRef[0], this);
		UpdateCurrencyUI();
		StashLitterBin = new D3DLitterBin(m_UIManagerRef[0], this);
		StashLitterBin.CreateLitterBin(new Vector2(425f, 58f));
		TapJoyBtn = new UIClickButton();
		D3DImageCell imageCell = GetImageCell("shuijing-free");
		TapJoyBtn.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = GetImageCell("shuijing-free1");
		TapJoyBtn.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		TapJoyBtn.Rect = D3DMain.Instance.ConvertRectAutoHD(422f, 55f, 55f, 55f);
		m_UIManagerRef[0].Add(TapJoyBtn);
		CreateStashPuppet();
		_SubPuppetFaceUI.CreatePuppetFaceUI(this, 0, 9, OnSelectAnotherPuppetFace);
		DragIcon = new UIImage();
		DragIcon.SetAlpha(0.7f);
		DragIcon.Enable = false;
		DragIcon.Visible = false;
		m_UIManagerRef[9].Add(DragIcon);
		InsertUIManager("Manager_Mask1", 8);
		m_UIManagerRef[8].EnableUIHandler = false;
	}

	private bool StashOptionsEvent(UIControl control)
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
					SwitchStashOption(num);
				}
				return true;
			}
			num++;
		}
		return false;
	}

	private void SwitchStashOption(int option_index)
	{
		if (option_index == CurrentOptionIndex)
		{
			return;
		}
		for (int i = 1; i < 9; i++)
		{
			if (!(null == m_UIManagerRef[i]))
			{
				m_UIManagerRef[i].gameObject.SetActiveRecursively(false);
			}
		}
		if (ActivingStoreGear != null)
		{
			DeSelectStoreEquip();
			ActivingStoreGear = null;
		}
		if (ActivingPuppetGear != null)
		{
			ActivingPuppetGear.Select(false);
			ActivingPuppetGear = null;
		}
		ActivingSkillSlot = null;
		ActivingSkillBar = null;
		if (PuppetFeatureUI != null)
		{
			PuppetFeatureUI.Visible(false);
		}
		StashLitterBin.Visible(false);
		TapJoyBtn.Enable = false;
		TapJoyBtn.Visible = false;
		CurrentOptionIndex = option_index;
		switch (EnabledOptions[CurrentOptionIndex])
		{
		case StashOption.GEARS:
			PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].CheckPuppetWeapons();
			UpdateCurrentPuppetInfo(true);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[1].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[8].gameObject.SetActiveRecursively(true);
			PuppetFeatureUI.Visible(true);
			StashLitterBin.Visible(true);
			break;
		case StashOption.SKILLS:
			UpdateSkillSlots();
			SwitchSkillTag(true);
			m_UIManagerRef[5].gameObject.SetActiveRecursively(true);
			SkillScroll.SetScrollViewPort(new Rect(7f, 8f, 406.5f, 158f));
			SkillScroll.InitActiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, SkillInfoText);
			m_UIManagerRef[6].gameObject.SetActiveRecursively(true);
			break;
		case StashOption.ACTIVE:
		{
			SkillScroll.SetScrollViewPort(new Rect(7f, 8f, 406.5f, 271f));
			SkillScroll.InitActiveSkillForSkillSchool(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, SkillInfoText);
			List<UIImage> list = SkillScroll.CheckNewSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
			if (list != null && list.Count > 0)
			{
				newHintBehaviour.AddHintImage(list);
			}
			CheckSkillOptionNewHint();
			m_UIManagerRef[6].gameObject.SetActiveRecursively(true);
			break;
		}
		case StashOption.PASSIVE:
		{
			SkillScroll.SetScrollViewPort(new Rect(7f, 8f, 406.5f, 271f));
			SkillScroll.InitPassiveSkillForSkillSchool(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, SkillInfoText);
			List<UIImage> list = SkillScroll.CheckNewSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
			if (list != null && list.Count > 0)
			{
				newHintBehaviour.AddHintImage(list);
			}
			CheckSkillOptionNewHint();
			m_UIManagerRef[6].gameObject.SetActiveRecursively(true);
			break;
		}
		case StashOption.tBANK:
			m_UIManagerRef[7].gameObject.SetActiveRecursively(true);
			TapJoyBtn.Visible = true;
			TapJoyBtn.Enable = true;
			break;
		case StashOption.ITEMS:
			break;
		}
	}

	private void OnSelectAnotherPuppetFace(int nFaceIndex)
	{
		for (int i = 0; i < EnabledOptions.Length; i++)
		{
			switch (EnabledOptions[i])
			{
			case StashOption.GEARS:
				UpdateCurrentGearPageStore();
				UpdateCurrentPuppetInfo(true);
				break;
			case StashOption.SKILLS:
				if (i == CurrentOptionIndex)
				{
					ActivingSkillSlot = null;
					UpdateSkillSlots();
					if (ActiveTagActiving)
					{
						SkillScroll.InitActiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, SkillInfoText);
					}
					else
					{
						SkillScroll.InitPassiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list, SkillInfoText);
					}
				}
				break;
			case StashOption.ACTIVE:
				if (i == CurrentOptionIndex)
				{
					SkillScroll.InitActiveSkillForSkillSchool(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, SkillInfoText);
					List<UIImage> list2 = SkillScroll.CheckNewSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
					if (list2 != null && list2.Count > 0)
					{
						newHintBehaviour.AddHintImage(list2);
					}
					CheckSkillOptionNewHint();
				}
				break;
			case StashOption.PASSIVE:
				if (i == CurrentOptionIndex)
				{
					SkillScroll.InitPassiveSkillForSkillSchool(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, SkillInfoText);
					List<UIImage> list = SkillScroll.CheckNewSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
					if (list != null && list.Count > 0)
					{
						newHintBehaviour.AddHintImage(list);
					}
					CheckSkillOptionNewHint();
				}
				break;
			}
		}
	}

	private void CloseStash()
	{
		if (D3DMain.Instance.CurrentScene != 4)
		{
			if (D3DMain.Instance.CurrentScene == 3)
			{
				GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().scene_dungeon.UpdatePlayerAvatar();
				if (null != D3DMain.Instance.HD_BOARD_OBJ)
				{
					D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
				}
				UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
				uIHelper.GetManager(0).gameObject.SetActiveRecursively(true);
				GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().SetPortalVisible(D3DMain.Instance.exploring_dungeon.dungeon.explored_level > 0);
				GameObject.FindGameObjectWithTag("UIDungeon").GetComponent<UIDungeon>().UpdateSubUINewHint();
			}
			if (ui_index > 1)
			{
				UIHelper uIHelper2 = D3DMain.Instance.D3DUIList[ui_index - 2];
				uIHelper2.ui_fade.StartFade(UIFade.FadeState.FADE_IN, uIHelper2.FreezeTimeScale, true);
			}
			Object.Destroy(base.gameObject);
		}
		else
		{
			D3DMain.Instance.CurrentScene = 1;
			Application.LoadLevel(1);
		}
	}

	private void UpdateCurrencyUI()
	{
		PlayerCurrencyText.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		PlayerCurrencyText.SetPosition(new Vector2(475f - PlayerCurrencyText.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), 293f));
	}

	private void CreateGearPuppetUI()
	{
		if (null != m_UIManagerRef[1])
		{
			return;
		}
		InsertUIManager("Manager_GearPuppet", 1);
		m_UIManagerRef[1].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(208f, 8f, 206f, 271f));
		CreateUIByCellXml("UIStashGearPuppetCfg", m_UIManagerRef[1]);
		m_UIManagerRef[1].EnableUIHandler = false;
		Vector2[] array = new Vector2[10]
		{
			new Vector2(53f, 93f),
			new Vector2(110f, 93f),
			new Vector2(2f, 178.5f),
			new Vector2(2f, 224.5f),
			new Vector2(2f, 266f),
			new Vector2(2f, 132.5f),
			new Vector2(161.5f, 275f),
			new Vector2(161.5f, 224.5f),
			new Vector2(161.5f, 178.5f),
			new Vector2(161.5f, 132.5f)
		};
		PuppetGearSlots = new D3DPuppetGearSlotUI[10];
		for (int i = 0; i <= 9; i++)
		{
			if (i != 4 && i != 6)
			{
				PuppetGearSlots[i] = new D3DPuppetGearSlotUI(m_UIManagerRef[1], this);
				PuppetGearSlots[i].slot_index = i;
				PuppetGearSlots[i].CreateControl(array[i], string.Empty);
			}
		}
		DoubleHandMask = new UIImage();
		DoubleHandMask.SetAlpha(0.6f);
		DoubleHandMask.Enable = false;
		DoubleHandMask.Visible = false;
		m_UIManagerRef[8].Add(DoubleHandMask);
		PuppetFeatureUI = new D3DFeatureCameraUI(m_UIManagerRef[8], this);
		PuppetFeatureUI.CreateControl(new Vector2(210f, 120f), string.Empty, Vector2.zero, null, Vector2.zero, new Vector2(205f, 160f));
	}

	private D3DPuppetGearSlotUI PickPuppetGearSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[1].TouchPointOnManager(touch_point2);
		touch_point2 *= 1f / num;
		D3DPuppetGearSlotUI[] puppetGearSlots = PuppetGearSlots;
		foreach (D3DPuppetGearSlotUI d3DPuppetGearSlotUI in puppetGearSlots)
		{
			if (d3DPuppetGearSlotUI != null && d3DPuppetGearSlotUI.PtInSlot(touch_point2))
			{
				return d3DPuppetGearSlotUI;
			}
		}
		return null;
	}

	private void RemovePuppetGearToBin()
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_DESTORY), null, false, false);
		PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
		_SubPuppetFaceUI.CurrentPuppet.RemoveFaceFeatureArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
		ActivingPuppetGear.Select(false);
		ActivingPuppetGear = null;
		UpdateCurrentPuppetInfo(false);
	}

	private void CreateGearPuppetPropertyUI()
	{
		if (!(null != m_UIManagerRef[2]))
		{
			InsertUIManager("Manager_GearPuppetProperty", 2);
			m_UIManagerRef[2].EnableUIHandler = false;
			PuppetPropertyUI = new D3DUIPuppetProperty(m_UIManagerRef[2], this, new Rect(216f, 8f, 195f, 71f));
			PuppetPropertyUI.CreateScrollBar(false, true);
			PuppetPropertyUI.InitScrollBar();
			PuppetPropertyUI.Visible(true);
		}
	}

	private void UpdateCurrentPuppetInfo(bool camera_reset)
	{
		PuppetBasic puppetBasic = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex];
		if (ActivingPuppetGear != null)
		{
			GearDescriptionUI.Visible(false);
			ActivingPuppetGear = null;
		}
		if (ActivingStoreGear != null)
		{
			UpdateSelectedEquipInfo();
		}
		for (int i = 0; i <= 9; i++)
		{
			if (i != 4 && i != 6)
			{
				PuppetGearSlots[i].HideSlot();
			}
		}
		for (int j = 0; j <= 9; j++)
		{
			if (j != 4 && j != 6)
			{
				D3DEquipment d3DEquipment = puppetBasic.profile_instance.puppet_arms[j];
				if (d3DEquipment != null)
				{
					PuppetGearSlots[j].UpdateGearSlot(d3DEquipment, puppetBasic.profile_instance);
				}
			}
		}
		if (camera_reset)
		{
			PuppetFeatureUI.SetCameraFeatureObject(puppetBasic.gameObject);
			D3DPuppetTransformCfg transformCfg = puppetBasic.model_builder.TransformCfg;
			PuppetFeatureUI.SetCameraFeatureTransform(transformCfg.stash_camera_cfg.offset, transformCfg.stash_camera_cfg.rotation, transformCfg.stash_camera_cfg.size);
		}
		switch (puppetBasic.cover_weapon_mask)
		{
		case 0:
			DoubleHandMask.Visible = false;
			break;
		case 1:
		{
			D3DEquipment d3DEquipment3 = puppetBasic.profile_instance.puppet_arms[0];
			if (d3DEquipment3 == null)
			{
				DoubleHandMask.Visible = false;
				break;
			}
			UIImage slotIcon2 = PuppetGearSlots[0].SlotIcon;
			Rect rect3 = slotIcon2.Rect;
			DoubleHandMask.SetTexture(slotIcon2.GetTexture(), slotIcon2.GetTextureRect(), new Vector2(rect3.width, rect3.height));
			Rect rect4 = PuppetGearSlots[1].SlotIcon.Rect;
			Vector2 vector2 = new Vector2(rect4.x, rect4.y) + new Vector2(m_UIManagerRef[1].ManagerRect.x, m_UIManagerRef[1].ManagerRect.y);
			DoubleHandMask.Rect = new Rect(vector2.x, vector2.y, rect4.width, rect4.height);
			DoubleHandMask.Visible = true;
			break;
		}
		case -1:
		{
			D3DEquipment d3DEquipment2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1];
			if (d3DEquipment2 == null)
			{
				DoubleHandMask.Visible = false;
				break;
			}
			UIImage slotIcon = PuppetGearSlots[1].SlotIcon;
			Rect rect = slotIcon.Rect;
			DoubleHandMask.SetTexture(slotIcon.GetTexture(), slotIcon.GetTextureRect(), new Vector2(rect.width, rect.height));
			Rect rect2 = PuppetGearSlots[0].SlotIcon.Rect;
			Vector2 vector = new Vector2(rect2.x, rect2.y) + new Vector2(m_UIManagerRef[1].ManagerRect.x, m_UIManagerRef[1].ManagerRect.y);
			DoubleHandMask.Rect = new Rect(vector.x, vector.y, rect2.width, rect2.height);
			DoubleHandMask.Visible = true;
			break;
		}
		}
		PuppetPropertyUI.UpdatePropertyInfo(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, camera_reset);
		PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
	}

	private void CreateGearsStoreUI()
	{
		if (null != m_UIManagerRef[3])
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
		InsertUIManager("Manager_GearStore", 3);
		m_UIManagerRef[3].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(8f, 8f, 208.5f, 271f));
		CreateUIByCellXml("UIStashGearStoreCfg", m_UIManagerRef[3]);
		GearPageBtns = new D3DTextPushButton[5];
		for (int j = 0; j < 5; j++)
		{
			GearPageBtns[j] = new D3DTextPushButton(m_UIManagerRef[3], this);
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
			m_UIManagerRef[3].Add(uIClickButton);
			UnlockPageBtns.Add(uIClickButton);
		}
		PageHoverMask = new UIImage();
		D3DImageCell imageCell = GetImageCell("tuodongwupintingliuzhuangtai-1");
		PageHoverMask.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(33f, 19f) * D3DMain.Instance.HD_SIZE);
		PageHoverMask.Enable = false;
		PageHoverMask.Visible = false;
		m_UIManagerRef[3].Add(PageHoverMask);
		GearStoreSlots = new D3DGearSlotUI[12];
		for (int l = 0; l < 12; l++)
		{
			GearStoreSlots[l] = new D3DGearSlotUI(m_UIManagerRef[3], this);
			GearStoreSlots[l].slot_index = l;
			GearStoreSlots[l].CreateControl(new Vector2(4 + l % 4 * 48, 222 - l / 4 * 46), "beibaokuang");
		}
		UIImage uIImage = new UIImage();
		imageCell = UIImageCellIndexer["dakuang9"];
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(193f, 0f, 10f, 271f));
		uIImage.Enable = false;
		m_UIManagerRef[3].Add(uIImage);
	}

	public void UpdateStashGearStoreByIAP()
	{
		if (null == m_UIManagerRef[3])
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
		if (null == m_UIManagerRef[3])
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
		GearDescriptionUI.Visible(false);
		int num = CurrentGearPage * 12;
		for (int i = 0; i < GearStoreSlots.Length; i++)
		{
			GearStoreSlots[i].HideSlot();
			int num2 = num + i;
			if (num2 > GearStore.Count - 1)
			{
				break;
			}
			if (GearStore[num2] != null)
			{
				GearStoreSlots[i].UpdateGearSlot(GearStore[num2], PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance);
				if (D3DGamer.Instance.NewGearSlotHint.Contains(num2))
				{
					GearStoreSlots[i].NewHint.Visible = true;
					newHintBehaviour.AddHintImage(GearStoreSlots[i].NewHint);
				}
			}
		}
		if (ActivingStoreGear != null)
		{
			ActivingStoreGear.Select(true);
			UpdateSelectedEquipInfo();
			GearDescriptionUI.Visible(true);
		}
		if (ActivingPuppetGear != null)
		{
			GearDescriptionUI.Visible(true);
		}
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
		touch_point2 = m_UIManagerRef[3].TouchPointOnManager(touch_point2);
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
			GearStore.Add(null);
		}
	}

	private void CreateGearDescriptionUI()
	{
		if (!(null != m_UIManagerRef[4]))
		{
			InsertUIManager("Manager_GearDescription", 4);
			GearDescriptionUI = new D3DUIGearDescription(m_UIManagerRef[4], this, new Rect(11f, 9.5f, 191f, 87f));
			GearDescriptionUI.CreateCompareBtn();
			GearDescriptionUI.CreateScrollBar(false, true);
			GearDescriptionUI.InitScrollBar();
			GearDescriptionUI.Visible(false);
		}
	}

	private void CreateSkillUI()
	{
		if (null != m_UIManagerRef[5])
		{
			return;
		}
		InsertUIManager("Manager_SkillSet", 5);
		m_UIManagerRef[5].EnableUIHandler = false;
		D3DImageCell imageCell = GetImageCell("jinengditu");
		UIImage[] array = new UIImage[2]
		{
			new UIImage(),
			null
		};
		array[0].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		if (D3DMain.Instance.HD_SIZE == 1)
		{
			array[0].Rect = new Rect(4f, 144f, 208f, 141f);
		}
		else
		{
			array[0].Rect = new Rect(6f, 288f, 416f, 282f);
		}
		array[0].FlipX(true);
		m_UIManagerRef[5].Add(array[0]);
		array[1] = new UIImage();
		array[1].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		if (D3DMain.Instance.HD_SIZE == 1)
		{
			array[1].Rect = new Rect(210f, 144f, 208f, 141f);
		}
		else
		{
			array[1].Rect = new Rect(420f, 288f, 416f, 282f);
		}
		m_UIManagerRef[5].Add(array[1]);
		CreateUIByCellXml("UIStashSkillSetCfg", m_UIManagerRef[5]);
		ActiveSkillSlots = new D3DSkillSlotUI[4];
		PassiveSkillSlots = new D3DSkillSlotUI[4];
		imageCell = GetImageCell("zhuangbeikuang");
		for (int i = 0; i < 4; i++)
		{
			UIImage uIImage = new UIImage();
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(54f, 54f) * D3DMain.Instance.HD_SIZE);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(95 + 78 * i, 223f, 54f, 54f);
			uIImage.Enable = false;
			m_UIManagerRef[5].Add(uIImage);
			ActiveSkillSlots[i] = new D3DSkillSlotUI(m_UIManagerRef[5], this);
			ActiveSkillSlots[i].slot_index = i;
			ActiveSkillSlots[i].CreateControl(new Vector2(95 + 78 * i, 223f));
			if (i > D3DGamer.Instance.VaildSkillSlot - 1)
			{
				ActiveSkillSlots[i].LockState();
			}
			uIImage = new UIImage();
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(54f, 54f) * D3DMain.Instance.HD_SIZE);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(95 + 78 * i, 168f, 54f, 54f);
			uIImage.Enable = false;
			m_UIManagerRef[5].Add(uIImage);
			PassiveSkillSlots[i] = new D3DSkillSlotUI(m_UIManagerRef[5], this);
			PassiveSkillSlots[i].slot_index = i;
			PassiveSkillSlots[i].CreateControl(new Vector2(95 + 78 * i, 168f));
			if (i > D3DGamer.Instance.VaildSkillSlot - 1)
			{
				PassiveSkillSlots[i].LockState();
			}
		}
		D3DImageCell imageCell2 = GetImageCell("hongbiaoqian1-1");
		((UIImage)GetControl("StashActiveTagImg")).SetTexture(LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect));
	}

	private void SwitchSkillTag(bool active_tag)
	{
		if (active_tag != ActiveTagActiving)
		{
			ActiveTagActiving = active_tag;
			D3DImageCell imageCell = GetImageCell("hongbiaoqian1-1");
			D3DImageCell imageCell2 = GetImageCell("hongbiaoqian1");
			if (ActiveTagActiving)
			{
				((UIImage)GetControl("StashActiveTagImg")).SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
				((UIImage)GetControl("StashPassiveTagImg")).SetTexture(LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect));
			}
			else
			{
				((UIImage)GetControl("StashPassiveTagImg")).SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
				((UIImage)GetControl("StashActiveTagImg")).SetTexture(LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect));
			}
		}
	}

	private void UpdateSkillSlots()
	{
		SwitchSkillTag(ActiveTagActiving);
		for (int i = 0; i < 4; i++)
		{
			ActiveSkillSlots[i].HideSlot();
			if (i > D3DGamer.Instance.VaildSkillSlot - 1)
			{
				ActiveSkillSlots[i].LockState();
			}
			else if (i <= PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.battle_active_slots.Length - 1)
			{
				D3DActiveSkill activeSkill = D3DMain.Instance.GetActiveSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.battle_active_slots[i]);
				if (activeSkill != null)
				{
					ActiveSkillSlots[i].UpdateSkillSlot(activeSkill);
				}
			}
			PassiveSkillSlots[i].HideSlot();
			if (i > D3DGamer.Instance.VaildSkillSlot - 1)
			{
				PassiveSkillSlots[i].LockState();
			}
			else if (i <= PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.battle_passive_slots.Length - 1)
			{
				D3DPassiveSkill passiveSkill = D3DMain.Instance.GetPassiveSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.battle_passive_slots[i]);
				if (passiveSkill != null)
				{
					PassiveSkillSlots[i].UpdateSkillSlot(passiveSkill);
				}
			}
		}
	}

	private D3DSkillSlotUI PickActiveSkillSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[5].TouchPointOnManager(touch_point2);
		touch_point2 *= 1f / num;
		D3DSkillSlotUI[] activeSkillSlots = ActiveSkillSlots;
		foreach (D3DSkillSlotUI d3DSkillSlotUI in activeSkillSlots)
		{
			if (d3DSkillSlotUI.PtInSlot(touch_point2))
			{
				return d3DSkillSlotUI;
			}
		}
		return null;
	}

	private D3DSkillSlotUI PickPassiveSkillSlot(Vector2 touch_point)
	{
		float num = (float)Screen.height / 640f;
		Vector2 touch_point2 = touch_point * num + Vector2.right * ((float)Screen.width - 960f * num) * 0.5f;
		touch_point2 = m_UIManagerRef[5].TouchPointOnManager(touch_point2);
		touch_point2 *= 1f / num;
		D3DSkillSlotUI[] passiveSkillSlots = PassiveSkillSlots;
		foreach (D3DSkillSlotUI d3DSkillSlotUI in passiveSkillSlots)
		{
			if (d3DSkillSlotUI.PtInSlot(touch_point2))
			{
				return d3DSkillSlotUI;
			}
		}
		return null;
	}

	private void IapBuySkillSlots()
	{
		if (int.Parse(D3DGamer.Instance.CrystalText) < 20)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OpenTBank);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
		}
		else
		{
			D3DGamer.Instance.UpdateCrystal(-20);
			UpdateCurrencyUI();
			D3DGamer.Instance.VaildSkillSlot = 4;
			UpdateSkillSlots();
		}
	}

	private void CreateSkillScrollUI()
	{
		if (!(null != m_UIManagerRef[6]))
		{
			InsertUIManager("Manager_SkillScroll", 6);
			m_UIManagerRef[6].EnableUIHandler = false;
			D3DImageCell imageCell = GetImageCell("jinengditu");
			UIImage[] array = new UIImage[4]
			{
				new UIImage(),
				null,
				null,
				null
			};
			array[0].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			if (D3DMain.Instance.HD_SIZE == 1)
			{
				array[0].Rect = new Rect(-3f, 136f, 208f, 141f);
			}
			else
			{
				array[0].Rect = new Rect(-8f, 272f, 416f, 282f);
			}
			array[0].FlipX(true);
			m_UIManagerRef[6].Add(array[0]);
			array[1] = new UIImage();
			array[1].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			if (D3DMain.Instance.HD_SIZE == 1)
			{
				array[1].Rect = new Rect(203f, 136f, 208f, 141f);
			}
			else
			{
				array[1].Rect = new Rect(406f, 272f, 416f, 282f);
			}
			m_UIManagerRef[6].Add(array[1]);
			array[2] = new UIImage();
			array[2].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			if (D3DMain.Instance.HD_SIZE == 1)
			{
				array[2].Rect = new Rect(-3f, -3f, 208f, 141f);
			}
			else
			{
				array[2].Rect = new Rect(-8f, -6f, 416f, 282f);
			}
			array[2].FlipX(true);
			array[2].FlipY(true);
			m_UIManagerRef[6].Add(array[2]);
			array[3] = new UIImage();
			array[3].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			if (D3DMain.Instance.HD_SIZE == 1)
			{
				array[3].Rect = new Rect(203f, -3f, 208f, 141f);
			}
			else
			{
				array[3].Rect = new Rect(406f, -6f, 416f, 282f);
			}
			array[3].FlipY(true);
			m_UIManagerRef[6].Add(array[3]);
			SkillInfoText = new UIText();
			SkillInfoText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), string.Empty, Color.black);
			SkillInfoText.Enable = false;
			SkillInfoText.Visible = false;
			SkillInfoText.AlignStyle = UIText.enAlignStyle.center;
			SkillInfoText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
			SkillInfoText.Rect = D3DMain.Instance.ConvertRectAutoHD(8f, 125f, 410f, 30f);
			m_UIManagerRef[6].Add(SkillInfoText);
			SkillScroll = new D3DSkillScroll(m_UIManagerRef[6], this, new Rect(7f, 8f, 407f, 158f));
			SkillScroll.CreateScrollBar(false, true);
			m_UIManagerRef[6].gameObject.SetActiveRecursively(false);
			imageCell = GetImageCell("optionhighlight");
			OptionHintImg = new UIImage[2];
			for (int i = 0; i < 2; i++)
			{
				OptionHintImg[i] = new UIImage();
				OptionHintImg[i].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
				OptionHintImg[i].Enable = false;
				OptionHintImg[i].Visible = false;
				OptionHintImg[i].Rect = OptionBtns[i].PushBtn.Rect;
				m_UIManagerRef[0].Add(OptionHintImg[i]);
				newHintBehaviour.AddHintImage(OptionHintImg[i]);
			}
		}
	}

	private void ConfirmUpgradeSkill()
	{
		D3DGamer.Instance.UpdateCurrency(-UpgradeSkill.UpgradeCost);
		D3DGamer.Instance.UpdateCrystal(-UpgradeSkill.UpgradeCrystal);
		UpdateCurrencyUI();
		UpgradeSkill.skill_level++;
		if (UpgradeSkill.SkillMax)
		{
			UpgradeSkill.skill_level = UpgradeSkill.MaxLevel - 1;
		}
		float y = m_UIManagerRef[6].GetManagerCamera().transform.localPosition.y;
		if (EnabledOptions[CurrentOptionIndex] == StashOption.ACTIVE)
		{
			SkillScroll.InitActiveSkillForSkillSchool(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, SkillInfoText);
		}
		else
		{
			SkillScroll.InitPassiveSkillForSkillSchool(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level, SkillInfoText);
		}
		List<UIImage> list = SkillScroll.CheckNewSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
		if (list != null && list.Count > 0)
		{
			newHintBehaviour.AddHintImage(list);
		}
		CheckSkillOptionNewHint();
		float y2 = m_UIManagerRef[6].GetManagerCamera().transform.localPosition.y;
		SkillScroll.Scroll(Vector2.up * (y2 - y));
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_LV_UP), null, false, false);
	}

	private void CheckSkillOptionNewHint()
	{
		OptionHintImg[0].Visible = false;
		OptionHintImg[1].Visible = false;
		if (!D3DGamer.Instance.NewSkillHint.ContainsKey(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID))
		{
			return;
		}
		foreach (string item in D3DGamer.Instance.NewSkillHint[PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID])
		{
			if (OptionHintImg[0].Visible && OptionHintImg[1].Visible)
			{
				break;
			}
			if (D3DMain.Instance.CheckActiveSkillID(item))
			{
				OptionHintImg[0].Visible = true;
			}
			if (D3DMain.Instance.CheckPassiveSkillID(item))
			{
				OptionHintImg[1].Visible = true;
			}
		}
	}

	private void OpenTBank()
	{
		int num = -1;
		for (int i = 0; i < EnabledOptions.Length; i++)
		{
			if (EnabledOptions[i] == StashOption.tBANK)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			StashOption[] array = new StashOption[EnabledOptions.Length + 1];
			for (int j = 0; j < array.Length; j++)
			{
				if (j < EnabledOptions.Length)
				{
					array[j] = EnabledOptions[j];
				}
				else
				{
					array[j] = StashOption.tBANK;
				}
			}
			EnabledOptions = array;
			D3DTextPushButton[] array2 = new D3DTextPushButton[OptionBtns.Length + 1];
			for (int k = 0; k < array2.Length; k++)
			{
				if (k < OptionBtns.Length)
				{
					array2[k] = OptionBtns[k];
					continue;
				}
				array2[k] = new D3DTextPushButton(m_UIManagerRef[0], this);
				array2[k].CreateControl(new Vector2(82 * k, 283f), new Vector2(84f, 37f), "anniu1", "anniu2", string.Empty, D3DMain.Instance.GameFont2.FontName, 11, 22, StashOption.tBANK.ToString(), (D3DMain.Instance.HD_SIZE != 2) ? new Vector2(0f, 1f) : new Vector2(0f, -3f), (float)D3DMain.Instance.HD_SIZE * 1.5f, D3DMain.Instance.CommonFontColor, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f));
			}
			OptionBtns = array2;
			num = EnabledOptions.Length - 1;
		}
		CreateStashManagerByOption(StashOption.tBANK);
		_subUItBank.CreateTBankUI(7, this, UpdateCurrencyUI, _SubPuppetFaceUI.UpdateFaceFrame);
		OptionBtns[CurrentOptionIndex].Set(false);
		CurrentOptionIndex = -1;
		OptionBtns[num].Set(true);
		SwitchStashOption(num);
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

	private void CreateGearsTouchAreas()
	{
		GearsTouchAreas = new Rect[8]
		{
			new Rect(13 * D3DMain.Instance.HD_SIZE, 138 * D3DMain.Instance.HD_SIZE, 186 * D3DMain.Instance.HD_SIZE, 136 * D3DMain.Instance.HD_SIZE),
			new Rect(209 * D3DMain.Instance.HD_SIZE, 140 * D3DMain.Instance.HD_SIZE, 44 * D3DMain.Instance.HD_SIZE, 136 * D3DMain.Instance.HD_SIZE),
			new Rect(368 * D3DMain.Instance.HD_SIZE, 140 * D3DMain.Instance.HD_SIZE, 44 * D3DMain.Instance.HD_SIZE, 136 * D3DMain.Instance.HD_SIZE),
			new Rect(260 * D3DMain.Instance.HD_SIZE, 102 * D3DMain.Instance.HD_SIZE, 102 * D3DMain.Instance.HD_SIZE, 42 * D3DMain.Instance.HD_SIZE),
			new Rect(262 * D3DMain.Instance.HD_SIZE, 147 * D3DMain.Instance.HD_SIZE, 99 * D3DMain.Instance.HD_SIZE, 126 * D3DMain.Instance.HD_SIZE),
			new Rect(10 * D3DMain.Instance.HD_SIZE, 9 * D3DMain.Instance.HD_SIZE, 192 * D3DMain.Instance.HD_SIZE, 86 * D3DMain.Instance.HD_SIZE),
			new Rect(216 * D3DMain.Instance.HD_SIZE, 8 * D3DMain.Instance.HD_SIZE, 192 * D3DMain.Instance.HD_SIZE, 70 * D3DMain.Instance.HD_SIZE),
			new Rect(430 * D3DMain.Instance.HD_SIZE, 61 * D3DMain.Instance.HD_SIZE, 42 * D3DMain.Instance.HD_SIZE, 42 * D3DMain.Instance.HD_SIZE)
		};
	}

	private int GetGearsTouchArea(Vector2 touch_position)
	{
		for (int i = 0; i <= 7; i++)
		{
			if (GearsTouchAreas[i].Contains(touch_position))
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

	private void GearsTouchEvent(Vector2 touch_point, int touch_command, float x_delta, float y_delta)
	{
		switch (touch_command)
		{
		case 0:
			StartTouchArea = GetGearsTouchArea(touch_point);
			switch (StartTouchArea)
			{
			case 5:
				GearDescriptionUI.StopInertia();
				break;
			case 6:
				PuppetPropertyUI.StopInertia();
				break;
			case 0:
			{
				D3DGearSlotUI d3DGearSlotUI4 = PickGearStoreSlot(touch_point);
				if (d3DGearSlotUI4 == null)
				{
					break;
				}
				int index2 = d3DGearSlotUI4.slot_index + CurrentGearPage * 12;
				if (GearStore[index2] == null)
				{
					break;
				}
				if (d3DGearSlotUI4 != ActivingStoreGear)
				{
					if (ActivingPuppetGear != null)
					{
						ActivingPuppetGear.Select(false);
						ActivingPuppetGear = null;
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
					GearDescriptionUI.Visible(true);
				}
				else
				{
					InstantClick = true;
				}
				EnableDrag = true;
				break;
			}
			case 1:
			case 2:
			case 3:
			{
				D3DPuppetGearSlotUI d3DPuppetGearSlotUI2 = PickPuppetGearSlot(touch_point);
				if (d3DPuppetGearSlotUI2 == null)
				{
					break;
				}
				int slot_index = d3DPuppetGearSlotUI2.slot_index;
				D3DEquipment d3DEquipment4 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[slot_index];
				if (d3DEquipment4 == null)
				{
					break;
				}
				if (d3DPuppetGearSlotUI2 != ActivingPuppetGear)
				{
					if (ActivingStoreGear != null)
					{
						DeSelectStoreEquip();
						ActivingStoreGear = null;
					}
					if (ActivingPuppetGear != null)
					{
						ActivingPuppetGear.Select(false);
					}
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					ActivingPuppetGear = d3DPuppetGearSlotUI2;
					ActivingPuppetGear.Select(true);
					UpdateSelectEquipOnBody(slot_index);
					GearDescriptionUI.Visible(true);
				}
				else
				{
					InstantClick = true;
				}
				if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.editable)
				{
					EnableDrag = true;
					break;
				}
				EnableDrag = false;
				InstantClick = false;
				break;
			}
			case 4:
				break;
			}
			break;
		case 1:
			if ((StartTouchArea != 0 && StartTouchArea != 1 && StartTouchArea != 2 && StartTouchArea != 3) || !EnableDrag)
			{
				break;
			}
			DoingDrag = true;
			InstantClick = false;
			if (ActivingStoreGear != null)
			{
				SetDragIcon(ActivingStoreGear.SlotIcon);
				DragIcon.Visible = true;
				SetDragIconPosition(touch_point);
				int index3 = ActivingStoreGear.slot_index + CurrentGearPage * 12;
				D3DEquipment d3DEquipment6 = GearStore[index3];
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPickUpSfx((int)d3DEquipment6.equipment_class), null, false, false);
				ArmHintIndex = -1;
				switch (d3DEquipment6.equipment_type)
				{
				case D3DEquipment.EquipmentType.ONE_HAND:
				case D3DEquipment.EquipmentType.TWO_HAND:
				case D3DEquipment.EquipmentType.BOW_HAND:
					ArmHintIndex = 0;
					break;
				case D3DEquipment.EquipmentType.OFF_HAND:
					ArmHintIndex = 1;
					break;
				case D3DEquipment.EquipmentType.ARMOR:
					ArmHintIndex = 2;
					break;
				case D3DEquipment.EquipmentType.HELM:
					ArmHintIndex = 3;
					break;
				case D3DEquipment.EquipmentType.BOOTS:
					ArmHintIndex = 5;
					break;
				case D3DEquipment.EquipmentType.ACCESSORY:
					if (d3DEquipment6.equipment_class == D3DEquipment.EquipmentClass.NECKLACE)
					{
						ArmHintIndex = 7;
					}
					else if (d3DEquipment6.equipment_class == D3DEquipment.EquipmentClass.RING)
					{
						ArmHintIndex = 8;
					}
					break;
				}
				if (ArmHintIndex == -1)
				{
					break;
				}
				PuppetGearSlots[ArmHintIndex].ArmHint(true);
				if (ArmHintIndex == 8)
				{
					PuppetGearSlots[9].ArmHint(true);
				}
				else
				{
					if (ArmHintIndex != 0)
					{
						break;
					}
					if (d3DEquipment6.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
					{
						if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetTitanPower() || PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetDualWield())
						{
							PuppetGearSlots[1].ArmHint(true);
						}
					}
					else if (d3DEquipment6.equipment_type == D3DEquipment.EquipmentType.TWO_HAND && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetTitanPower())
					{
						PuppetGearSlots[1].ArmHint(true);
					}
				}
			}
			else if (ActivingPuppetGear != null)
			{
				D3DClass puppet_class = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class;
				if (puppet_class != null && puppet_class.editable)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPickUpSfx((int)PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index].equipment_class), null, false, false);
					SetDragIcon(ActivingPuppetGear.SlotIcon);
					DragIcon.Visible = true;
					SetDragIconPosition(touch_point);
				}
				else
				{
					DragIcon.Visible = false;
					StartTouchArea = -1;
				}
			}
			break;
		case 2:
			switch (StartTouchArea)
			{
			case 4:
				PuppetFeatureUI.ViewFeatureObj(Vector3.up, x_delta);
				break;
			case 5:
				GearDescriptionUI.Scroll(new Vector2(0f, y_delta));
				break;
			case 6:
				PuppetPropertyUI.Scroll(new Vector2(0f, y_delta));
				break;
			case 0:
			case 1:
			case 2:
			case 3:
				if (!DragIcon.Visible || !EnableDrag || (ActivingStoreGear == null && ActivingPuppetGear == null))
				{
					break;
				}
				SetDragIconPosition(touch_point);
				switch (GetGearsTouchArea(touch_point))
				{
				case 0:
				{
					StashLitterBin.Hover(false);
					PageHoverMask.Visible = false;
					D3DGearSlotUI d3DGearSlotUI5 = PickGearStoreSlot(touch_point);
					if (d3DGearSlotUI5 != null)
					{
						if (HoverGear != null)
						{
							if (d3DGearSlotUI5 != HoverGear)
							{
								HoverGear.SetHover(false, true);
								HoverGear = d3DGearSlotUI5;
								HoverGear.SetHover(true, true);
							}
						}
						else
						{
							HoverGear = d3DGearSlotUI5;
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
				case 1:
				case 2:
				case 3:
				{
					StashLitterBin.Hover(false);
					PageHoverMask.Visible = false;
					D3DPuppetGearSlotUI d3DPuppetGearSlotUI3 = PickPuppetGearSlot(touch_point);
					if (d3DPuppetGearSlotUI3 != null)
					{
						D3DEquipment d3DEquipment5 = null;
						if (ActivingStoreGear != null)
						{
							d3DEquipment5 = GearStore[ActivingStoreGear.slot_index + 12 * CurrentGearPage];
						}
						else if (ActivingPuppetGear != null)
						{
							d3DEquipment5 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index];
						}
						if (d3DEquipment5 == null)
						{
							break;
						}
						if (!d3DEquipment5.CheckEquipmentEquipLegal(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, (D3DPuppetProfile.PuppetArms)d3DPuppetGearSlotUI3.slot_index))
						{
							if (d3DPuppetGearSlotUI3.slot_index == 0 || d3DPuppetGearSlotUI3.slot_index == 1)
							{
								if (d3DEquipment5.IsEquipmentUseable(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance))
								{
									if ((d3DPuppetGearSlotUI3.slot_index == 0 && (d3DEquipment5.equipment_type == D3DEquipment.EquipmentType.ONE_HAND || d3DEquipment5.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)) || (d3DPuppetGearSlotUI3.slot_index == 1 && d3DEquipment5.equipment_type == D3DEquipment.EquipmentType.OFF_HAND))
									{
										if (HoverGear != null)
										{
											if (d3DPuppetGearSlotUI3 != HoverGear)
											{
												HoverGear.SetHover(false, true);
												HoverGear = d3DPuppetGearSlotUI3;
												HoverGear.SetHover(true, true);
											}
										}
										else
										{
											HoverGear = d3DPuppetGearSlotUI3;
											HoverGear.SetHover(true, true);
										}
									}
									else if (HoverGear != null)
									{
										if (d3DPuppetGearSlotUI3 != HoverGear)
										{
											HoverGear.SetHover(false, true);
											HoverGear = d3DPuppetGearSlotUI3;
											HoverGear.SetHover(true, false);
										}
									}
									else
									{
										HoverGear = d3DPuppetGearSlotUI3;
										HoverGear.SetHover(true, false);
									}
								}
								else if (HoverGear != null)
								{
									if (d3DPuppetGearSlotUI3 != HoverGear)
									{
										HoverGear.SetHover(false, true);
										HoverGear = d3DPuppetGearSlotUI3;
										HoverGear.SetHover(true, false);
									}
								}
								else
								{
									HoverGear = d3DPuppetGearSlotUI3;
									HoverGear.SetHover(true, false);
								}
							}
							else if (HoverGear != null)
							{
								if (d3DPuppetGearSlotUI3 != HoverGear)
								{
									HoverGear.SetHover(false, true);
									HoverGear = d3DPuppetGearSlotUI3;
									HoverGear.SetHover(true, false);
								}
							}
							else
							{
								HoverGear = d3DPuppetGearSlotUI3;
								HoverGear.SetHover(true, false);
							}
						}
						else if (HoverGear != null)
						{
							if (d3DPuppetGearSlotUI3 != HoverGear)
							{
								HoverGear.SetHover(false, true);
								HoverGear = d3DPuppetGearSlotUI3;
								HoverGear.SetHover(true, true);
							}
						}
						else
						{
							HoverGear = d3DPuppetGearSlotUI3;
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
				case 7:
					StashLitterBin.Hover(true);
					break;
				default:
					if (HoverGear != null)
					{
						HoverGear.SetHover(false, true);
						HoverGear = null;
					}
					StashLitterBin.Hover(false);
					GearDragOnPage(touch_point);
					break;
				}
				break;
			}
			break;
		case 4:
			switch (StartTouchArea)
			{
			case 5:
				GearDescriptionUI.ScrollInertia(new Vector2(0f, y_delta));
				break;
			case 6:
				PuppetPropertyUI.ScrollInertia(new Vector2(0f, y_delta));
				break;
			case 0:
			case 1:
			case 2:
			case 3:
				if (!EnableDrag)
				{
					break;
				}
				if (ActivingStoreGear != null)
				{
					int gearsTouchArea = GetGearsTouchArea(touch_point);
					int num = ActivingStoreGear.slot_index + CurrentGearPage * 12;
					D3DEquipment d3DEquipment = GearStore[num];
					if (DoingDrag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPutDownSfx((int)d3DEquipment.equipment_class), null, false, false);
					}
					if (InstantClick)
					{
						EquipGear(d3DEquipment, num, d3DEquipment.GetDefaultArm());
						InstantClick = false;
						break;
					}
					switch (gearsTouchArea)
					{
					case 0:
					{
						D3DGearSlotUI d3DGearSlotUI = PickGearStoreSlot(touch_point);
						if (d3DGearSlotUI != null && d3DGearSlotUI != ActivingStoreGear)
						{
							int num3 = d3DGearSlotUI.slot_index + CurrentGearPage * 12;
							if (D3DGamer.Instance.NewGearSlotHint.Contains(num3))
							{
								D3DGamer.Instance.NewGearSlotHint.Remove(num3);
							}
							D3DEquipment value = GearStore[num3];
							GearStore[num3] = GearStore[num];
							GearStore[num] = value;
							GearDescriptionUI.Visible(false);
							DeSelectStoreEquip();
							ActivingStoreGear = null;
							UpdateCurrentGearPageStore();
						}
						break;
					}
					case 1:
					case 2:
					case 3:
					{
						D3DPuppetGearSlotUI d3DPuppetGearSlotUI = PickPuppetGearSlot(touch_point);
						if (d3DPuppetGearSlotUI != null)
						{
							EquipGear(d3DEquipment, num, (D3DPuppetProfile.PuppetArms)d3DPuppetGearSlotUI.slot_index);
						}
						break;
					}
					case 7:
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
								msgBoxContent[i] = msgBoxContent[i].Replace("<GetGear>", GearStore[ActivingStoreGear.slot_index + CurrentGearPage * 12].equipment_name);
								dictionary.Add(i, D3DMain.Instance.GetEquipmentGradeColor(GearStore[ActivingStoreGear.slot_index + CurrentGearPage * 12].equipment_grade));
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list.Add(RemoveGearToBin);
						PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list, false, dictionary);
						break;
					}
					default:
					{
						int num2 = GearDragOnPage(touch_point);
						if (num2 != -1 && num2 != CurrentGearPage && ThrowInPage(GearStore[num], num2))
						{
							GearStore[num] = null;
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
					if (ActivingPuppetGear == null)
					{
						break;
					}
					if (DoingDrag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetEquipmentPutDownSfx((int)PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index].equipment_class), null, false, false);
					}
					if (InstantClick)
					{
						InstantRemoveEquipGear();
						InstantClick = false;
						break;
					}
					switch (GetGearsTouchArea(touch_point))
					{
					case 0:
					{
						D3DGearSlotUI d3DGearSlotUI3 = PickGearStoreSlot(touch_point);
						if (d3DGearSlotUI3 == null)
						{
							break;
						}
						int index = d3DGearSlotUI3.slot_index + CurrentGearPage * 12;
						if (GearStore[index] == null)
						{
							GearStore[index] = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index];
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
							_SubPuppetFaceUI.CurrentPuppet.RemoveFaceFeatureArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
						}
						else
						{
							if (!ThrowInPage(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index], CurrentGearPage))
							{
								break;
							}
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
							_SubPuppetFaceUI.CurrentPuppet.RemoveFaceFeatureArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
						}
						GearDescriptionUI.Visible(false);
						ActivingPuppetGear.Select(false);
						ActivingPuppetGear = null;
						UpdateCurrentPuppetInfo(false);
						UpdateCurrentGearPageStore();
						break;
					}
					case 2:
					case 3:
					{
						D3DGearSlotUI d3DGearSlotUI2 = PickPuppetGearSlot(touch_point);
						if (ActivingPuppetGear.slot_index == 8 && d3DGearSlotUI2.slot_index == 9)
						{
							D3DEquipment equipment = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[9];
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RING2, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[8]);
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RING1, equipment);
							UpdateCurrentPuppetInfo(false);
						}
						else if (ActivingPuppetGear.slot_index == 9 && d3DGearSlotUI2.slot_index == 8)
						{
							D3DEquipment equipment2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[8];
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RING1, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[9]);
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RING2, equipment2);
							UpdateCurrentPuppetInfo(false);
						}
						else if (ActivingPuppetGear.slot_index == 0 && d3DGearSlotUI2.slot_index == 1)
						{
							D3DEquipment d3DEquipment2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1];
							if (d3DEquipment2 == null)
							{
								break;
							}
							if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetTitanPower() && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0].equipment_type != D3DEquipment.EquipmentType.BOW_HAND && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0].equipment_type != D3DEquipment.EquipmentType.OFF_HAND)
							{
								if (d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.ONE_HAND || d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
								{
									PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0]);
									PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, d3DEquipment2);
									UpdateCurrentPuppetInfo(false);
								}
							}
							else if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetDualWield() && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0].equipment_type == D3DEquipment.EquipmentType.ONE_HAND && d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
							{
								PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0]);
								PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, d3DEquipment2);
								UpdateCurrentPuppetInfo(false);
							}
						}
						else
						{
							if (ActivingPuppetGear.slot_index != 1 || d3DGearSlotUI2.slot_index != 0)
							{
								break;
							}
							D3DEquipment d3DEquipment3 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0];
							if (d3DEquipment3 == null)
							{
								break;
							}
							if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetTitanPower() && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1].equipment_type != D3DEquipment.EquipmentType.BOW_HAND && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1].equipment_type != D3DEquipment.EquipmentType.OFF_HAND)
							{
								if (d3DEquipment3.equipment_type == D3DEquipment.EquipmentType.ONE_HAND || d3DEquipment3.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
								{
									PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1]);
									PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, d3DEquipment3);
									UpdateCurrentPuppetInfo(false);
								}
							}
							else if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetDualWield() && PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1].equipment_type == D3DEquipment.EquipmentType.ONE_HAND && d3DEquipment3.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
							{
								PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1]);
								PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, d3DEquipment3);
								UpdateCurrentPuppetInfo(false);
							}
						}
						break;
					}
					case 7:
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
								msgBoxContent2[j] = msgBoxContent2[j].Replace("<GetGear>", PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index].equipment_name);
								dictionary2.Add(j, D3DMain.Instance.GetEquipmentGradeColor(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index].equipment_grade));
							}
						}
						list2.Add(RemovePuppetGearToBin);
						PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list2, false, dictionary2);
						break;
					}
					default:
					{
						int num4 = GearDragOnPage(touch_point);
						if (num4 != -1 && ThrowInPage(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index], num4))
						{
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
							_SubPuppetFaceUI.CurrentPuppet.RemoveFaceFeatureArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
							GearDescriptionUI.Visible(false);
							ActivingPuppetGear.Select(false);
							ActivingPuppetGear = null;
							UpdateCurrentPuppetInfo(false);
							UpdateCurrentGearPageStore();
						}
						break;
					}
					}
				}
				break;
			}
			StartTouchArea = -1;
			StashLitterBin.Hover(false);
			if (ArmHintIndex != -1)
			{
				PuppetGearSlots[ArmHintIndex].ArmHint(false);
				PuppetGearSlots[9].ArmHint(false);
				PuppetGearSlots[1].ArmHint(false);
				ArmHintIndex = -1;
			}
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

	private void EquipGear(D3DEquipment picking_gear, int current_store_index, D3DPuppetProfile.PuppetArms arm_type)
	{
		if (picking_gear.CheckEquipmentEquipLegal(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, arm_type))
		{
			D3DEquipment value = null;
			if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[(int)arm_type] != null)
			{
				value = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[(int)arm_type];
			}
			PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(arm_type, picking_gear);
			_SubPuppetFaceUI.CurrentPuppet.ChangeFaceFeatureArms(arm_type, picking_gear);
			GearStore[current_store_index] = value;
			GearDescriptionUI.Visible(false);
			DeSelectStoreEquip();
			ActivingStoreGear = null;
			UpdateCurrentPuppetInfo(false);
			UpdateCurrentGearPageStore();
		}
		else
		{
			if (!picking_gear.IsEquipmentUseable(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance))
			{
				return;
			}
			switch (arm_type)
			{
			case D3DPuppetProfile.PuppetArms.RIGHT_HAND:
			{
				if (picking_gear.equipment_type != D3DEquipment.EquipmentType.BOW_HAND && picking_gear.equipment_type != D3DEquipment.EquipmentType.TWO_HAND)
				{
					break;
				}
				bool flag2 = true;
				for (int j = 0; j < D3DGamer.Instance.ValidStorePage; j++)
				{
					if (!CheckPageFull(j))
					{
						ThrowInPage(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1], j);
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms(D3DPuppetProfile.PuppetArms.LEFT_HAND);
						D3DEquipment value3 = null;
						if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[(int)arm_type] != null)
						{
							value3 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[(int)arm_type];
						}
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(arm_type, picking_gear);
						GearStore[current_store_index] = value3;
						GearDescriptionUI.Visible(false);
						DeSelectStoreEquip();
						ActivingStoreGear = null;
						UpdateCurrentPuppetInfo(false);
						UpdateCurrentGearPageStore();
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CHANGE_GEAR_IF_STORE_FULL);
					PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.OK, null);
				}
				break;
			}
			case D3DPuppetProfile.PuppetArms.LEFT_HAND:
			{
				if (picking_gear.equipment_type != D3DEquipment.EquipmentType.OFF_HAND)
				{
					break;
				}
				bool flag = true;
				for (int i = 0; i < D3DGamer.Instance.ValidStorePage; i++)
				{
					if (!CheckPageFull(i))
					{
						ThrowInPage(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0], i);
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND);
						D3DEquipment value2 = null;
						if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[(int)arm_type] != null)
						{
							value2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[(int)arm_type];
						}
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].ChangeArms(arm_type, picking_gear);
						GearStore[current_store_index] = value2;
						GearDescriptionUI.Visible(false);
						DeSelectStoreEquip();
						ActivingStoreGear = null;
						UpdateCurrentPuppetInfo(false);
						UpdateCurrentGearPageStore();
						flag = false;
						break;
					}
				}
				if (flag)
				{
					List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CHANGE_GEAR_IF_STORE_FULL);
					PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
				}
				break;
			}
			}
		}
	}

	private void InstantRemoveEquipGear()
	{
		bool flag = true;
		for (int i = 0; i < D3DGamer.Instance.ValidStorePage; i++)
		{
			if (ThrowInPage(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index], i))
			{
				PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].RemoveArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
				_SubPuppetFaceUI.CurrentPuppet.RemoveFaceFeatureArms((D3DPuppetProfile.PuppetArms)ActivingPuppetGear.slot_index);
				GearDescriptionUI.Visible(false);
				ActivingPuppetGear.Select(false);
				ActivingPuppetGear = null;
				UpdateCurrentPuppetInfo(false);
				UpdateCurrentGearPageStore();
				flag = false;
				break;
			}
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
		int num = -1;
		for (int j = 0; j < msgBoxContent2.Count; j++)
		{
			if (msgBoxContent2[j].Contains("<GetPrice>"))
			{
				msgBoxContent2[j] = string.Empty;
				num = j;
			}
		}
		List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
		list.Add(IapBuyGearSpace);
		UIManager uIManager = PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list);
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
	}

	private void CreateSkillSetTouchAreas()
	{
		SkillSetTouchAreas = new Rect[5]
		{
			new Rect(8 * D3DMain.Instance.HD_SIZE, 230 * D3DMain.Instance.HD_SIZE, 78 * D3DMain.Instance.HD_SIZE, 33 * D3DMain.Instance.HD_SIZE),
			new Rect(8 * D3DMain.Instance.HD_SIZE, 180 * D3DMain.Instance.HD_SIZE, 78 * D3DMain.Instance.HD_SIZE, 33 * D3DMain.Instance.HD_SIZE),
			new Rect(93 * D3DMain.Instance.HD_SIZE, 224 * D3DMain.Instance.HD_SIZE, 293 * D3DMain.Instance.HD_SIZE, 53 * D3DMain.Instance.HD_SIZE),
			new Rect(93 * D3DMain.Instance.HD_SIZE, 170 * D3DMain.Instance.HD_SIZE, 293 * D3DMain.Instance.HD_SIZE, 53 * D3DMain.Instance.HD_SIZE),
			new Rect(8 * D3DMain.Instance.HD_SIZE, 8 * D3DMain.Instance.HD_SIZE, 406 * D3DMain.Instance.HD_SIZE, 158 * D3DMain.Instance.HD_SIZE)
		};
	}

	private int GetSkillSetTouchArea(Vector2 touch_position)
	{
		for (int i = 0; i <= 4; i++)
		{
			if (SkillSetTouchAreas[i].Contains(touch_position))
			{
				return i;
			}
		}
		return -1;
	}

	private void SkillSetTouchEvent(Vector2 touch_point, int touch_command, float x_delta, float y_delta)
	{
		switch (touch_command)
		{
		case 0:
			StartTouchArea = GetSkillSetTouchArea(touch_point);
			switch (StartTouchArea)
			{
			case 0:
				if (!ActiveTagActiving)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					SwitchSkillTag(true);
					SkillScroll.InitActiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, SkillInfoText);
					if (ActivingSkillSlot != null)
					{
						ActivingSkillSlot.Select(false);
						ActivingSkillSlot = null;
						ActivingSkillBar = null;
					}
				}
				break;
			case 1:
				if (ActiveTagActiving)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					SwitchSkillTag(false);
					SkillScroll.InitPassiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list, SkillInfoText);
					if (ActivingSkillSlot != null)
					{
						ActivingSkillSlot.Select(false);
						ActivingSkillSlot = null;
						ActivingSkillBar = null;
					}
				}
				break;
			case 2:
			{
				D3DSkillSlotUI d3DSkillSlotUI5 = PickActiveSkillSlot(touch_point);
				if (d3DSkillSlotUI5 != null)
				{
					if (d3DSkillSlotUI5.slot_index >= D3DGamer.Instance.VaildSkillSlot)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
						List<string> msgBoxContent7 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_UNLOCK_SKILL_SLOTS);
						msgBoxContent7 = new List<string>(msgBoxContent7);
						int num3 = -1;
						for (int k = 0; k < msgBoxContent7.Count; k++)
						{
							if (msgBoxContent7[k].Contains("<GetPrice>"))
							{
								msgBoxContent7[k] = string.Empty;
								num3 = k;
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list.Add(IapBuySkillSlots);
						UIManager uIManager = PushMessageBox(msgBoxContent7, D3DMessageBox.MgbButton.CANCEL_OK, list);
						if (num3 >= 0)
						{
							D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
							d3DCurrencyText.EnableGold = false;
							d3DCurrencyText.SetCrystal(20);
							Rect cameraTransformRect = uIManager.GetCameraTransformRect();
							float num4 = 640f / (float)Screen.height;
							cameraTransformRect = new Rect(cameraTransformRect.x * num4, cameraTransformRect.y * num4, cameraTransformRect.width * num4, cameraTransformRect.height * num4);
							d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num3)));
						}
						break;
					}
					bool flag = false;
					if (string.Empty != d3DSkillSlotUI5.SkillId)
					{
						if (d3DSkillSlotUI5 != ActivingSkillSlot)
						{
							D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
							flag = true;
							if (ActivingSkillSlot != null)
							{
								ActivingSkillSlot.Select(false);
							}
							if (ActivingSkillBar != null)
							{
								ActivingSkillBar.Select(false);
								ActivingSkillBar = null;
							}
							ActivingSkillSlot = d3DSkillSlotUI5;
							ActivingSkillSlot.Select(true);
							ActivingSkillBar = SkillScroll.GetSkillBar(ActivingSkillSlot.SkillId);
							if (ActivingSkillBar != null)
							{
								ActivingSkillBar.Select(true);
								SkillScroll.JumpScroll(ActivingSkillBar.bar_index);
							}
						}
						EnableDrag = true;
					}
					if (ActiveTagActiving)
					{
						break;
					}
					if (!flag)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					}
					SwitchSkillTag(true);
					SkillScroll.InitActiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list, SkillInfoText);
					if (ActivingSkillSlot == null)
					{
						break;
					}
					if (string.Empty == d3DSkillSlotUI5.SkillId)
					{
						ActivingSkillSlot.Select(false);
						ActivingSkillSlot = null;
						ActivingSkillBar = null;
						break;
					}
					ActivingSkillBar = SkillScroll.GetSkillBar(ActivingSkillSlot.SkillId);
					if (ActivingSkillBar != null)
					{
						ActivingSkillBar.Select(true);
						SkillScroll.JumpScroll(ActivingSkillBar.bar_index);
					}
				}
				else
				{
					StartTouchArea = -1;
				}
				break;
			}
			case 3:
			{
				D3DSkillSlotUI d3DSkillSlotUI5 = PickPassiveSkillSlot(touch_point);
				if (d3DSkillSlotUI5 != null)
				{
					if (d3DSkillSlotUI5.slot_index >= D3DGamer.Instance.VaildSkillSlot)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
						List<string> msgBoxContent8 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_UNLOCK_SKILL_SLOTS);
						msgBoxContent8 = new List<string>(msgBoxContent8);
						int num5 = -1;
						for (int l = 0; l < msgBoxContent8.Count; l++)
						{
							if (msgBoxContent8[l].Contains("<GetPrice>"))
							{
								msgBoxContent8[l] = string.Empty;
								num5 = l;
							}
						}
						List<D3DMessageBoxButtonEvent.OnButtonClick> list2 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
						list2.Add(IapBuySkillSlots);
						UIManager uIManager2 = PushMessageBox(msgBoxContent8, D3DMessageBox.MgbButton.CANCEL_OK, list2);
						if (num5 >= 0)
						{
							D3DCurrencyText d3DCurrencyText2 = new D3DCurrencyText(uIManager2, this);
							d3DCurrencyText2.EnableGold = false;
							d3DCurrencyText2.SetCrystal(20);
							Rect cameraTransformRect2 = uIManager2.GetCameraTransformRect();
							float num6 = 640f / (float)Screen.height;
							cameraTransformRect2 = new Rect(cameraTransformRect2.x * num6, cameraTransformRect2.y * num6, cameraTransformRect2.width * num6, cameraTransformRect2.height * num6);
							d3DCurrencyText2.SetPosition(new Vector2((cameraTransformRect2.x + cameraTransformRect2.width * 0.5f - d3DCurrencyText2.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect2.y + 205f - (float)(30 * num5)));
						}
						break;
					}
					bool flag2 = false;
					if (string.Empty != d3DSkillSlotUI5.SkillId)
					{
						if (d3DSkillSlotUI5 != ActivingSkillSlot)
						{
							D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
							flag2 = true;
							if (ActivingSkillSlot != null)
							{
								ActivingSkillSlot.Select(false);
							}
							if (ActivingSkillBar != null)
							{
								ActivingSkillBar.Select(false);
								ActivingSkillBar = null;
							}
							ActivingSkillSlot = d3DSkillSlotUI5;
							ActivingSkillSlot.Select(true);
							ActivingSkillBar = SkillScroll.GetSkillBar(ActivingSkillSlot.SkillId);
							if (ActivingSkillBar != null)
							{
								ActivingSkillBar.Select(true);
								SkillScroll.JumpScroll(ActivingSkillBar.bar_index);
							}
						}
						EnableDrag = true;
					}
					if (!ActiveTagActiving)
					{
						break;
					}
					if (!flag2)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					}
					SwitchSkillTag(false);
					SkillScroll.InitPassiveSkillForSkillSet(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list, SkillInfoText);
					if (ActivingSkillSlot == null)
					{
						break;
					}
					if (string.Empty == d3DSkillSlotUI5.SkillId)
					{
						ActivingSkillSlot.Select(false);
						ActivingSkillSlot = null;
						ActivingSkillBar = null;
						break;
					}
					ActivingSkillBar = SkillScroll.GetSkillBar(ActivingSkillSlot.SkillId);
					if (ActivingSkillBar != null)
					{
						ActivingSkillBar.Select(true);
						SkillScroll.JumpScroll(ActivingSkillBar.bar_index);
					}
				}
				else
				{
					StartTouchArea = -1;
				}
				break;
			}
			case 4:
			{
				SkillScroll.StopInertia();
				D3DSkillBarUI skillBar = SkillScroll.GetSkillBar(touch_point);
				if (skillBar != null)
				{
					if (skillBar.HitIcon)
					{
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
						if (ActivingSkillBar != null)
						{
							ActivingSkillBar.Select(false);
						}
						skillBar.Select(true);
						EnableDrag = true;
						ActivingSkillBar = skillBar;
						if (ActiveTagActiving)
						{
							if (ActivingSkillSlot != null)
							{
								ActivingSkillSlot.Select(false);
								ActivingSkillSlot = null;
							}
							D3DSkillSlotUI[] activeSkillSlots = ActiveSkillSlots;
							foreach (D3DSkillSlotUI d3DSkillSlotUI3 in activeSkillSlots)
							{
								if (d3DSkillSlotUI3.SkillId == ActivingSkillBar.SkillId)
								{
									ActivingSkillSlot = d3DSkillSlotUI3;
									ActivingSkillSlot.Select(true);
									break;
								}
							}
							break;
						}
						if (ActivingSkillSlot != null)
						{
							ActivingSkillSlot.Select(false);
							ActivingSkillSlot = null;
						}
						D3DSkillSlotUI[] passiveSkillSlots = PassiveSkillSlots;
						foreach (D3DSkillSlotUI d3DSkillSlotUI4 in passiveSkillSlots)
						{
							if (d3DSkillSlotUI4.SkillId == ActivingSkillBar.SkillId)
							{
								ActivingSkillSlot = d3DSkillSlotUI4;
								ActivingSkillSlot.Select(true);
								break;
							}
						}
					}
					else
					{
						DoSkillScroll = true;
					}
				}
				else
				{
					StartTouchArea = -1;
				}
				break;
			}
			}
			break;
		case 1:
			switch (StartTouchArea)
			{
			case 2:
			case 3:
				if (EnableDrag)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_UP), null, false, false);
					SetDragIcon(ActivingSkillSlot.SlotIcon);
					DragIcon.Visible = true;
					SetDragIconPosition(touch_point);
					DoingDrag = true;
				}
				break;
			case 4:
				if (DoSkillScroll)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SCROLL_SLIP), null, false, false);
					if (ActivingSkillSlot != null)
					{
						ActivingSkillSlot.Select(false);
						ActivingSkillSlot = null;
					}
					if (ActivingSkillBar != null)
					{
						ActivingSkillBar.Select(false);
						ActivingSkillBar = null;
					}
				}
				else if (EnableDrag)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_UP), null, false, false);
					SetDragIcon(ActivingSkillBar.SlotIcon);
					DragIcon.Visible = true;
					SetDragIconPosition(touch_point);
					DoingDrag = true;
				}
				break;
			}
			break;
		case 2:
			if (StartTouchArea == 4 && DoSkillScroll)
			{
				SkillScroll.Scroll(new Vector2(0f, y_delta));
			}
			else
			{
				if (!DragIcon.Visible || !EnableDrag)
				{
					break;
				}
				SetDragIconPosition(touch_point);
				int skillSetTouchArea2 = GetSkillSetTouchArea(touch_point);
				D3DSkillSlotUI d3DSkillSlotUI2 = null;
				if (skillSetTouchArea2 == 2 && ActiveTagActiving)
				{
					d3DSkillSlotUI2 = PickActiveSkillSlot(touch_point);
				}
				else if (skillSetTouchArea2 == 3 && !ActiveTagActiving)
				{
					d3DSkillSlotUI2 = PickPassiveSkillSlot(touch_point);
				}
				if (d3DSkillSlotUI2 != null)
				{
					if (d3DSkillSlotUI2.slot_index >= D3DGamer.Instance.VaildSkillSlot)
					{
						if (HoverGear != null)
						{
							HoverGear.SetHover(false, true);
							HoverGear = null;
						}
					}
					else if (HoverGear != null)
					{
						if (d3DSkillSlotUI2 != HoverGear)
						{
							HoverGear.SetHover(false, true);
							HoverGear = d3DSkillSlotUI2;
							HoverGear.SetHover(true, true);
						}
					}
					else
					{
						HoverGear = d3DSkillSlotUI2;
						HoverGear.SetHover(true, true);
					}
				}
				else if (HoverGear != null)
				{
					HoverGear.SetHover(false, true);
					HoverGear = null;
				}
			}
			break;
		case 4:
		{
			int skillSetTouchArea = GetSkillSetTouchArea(touch_point);
			D3DSkillSlotUI d3DSkillSlotUI = null;
			switch (StartTouchArea)
			{
			case 2:
			case 3:
				if (ActivingSkillSlot == null || !EnableDrag || !DoingDrag)
				{
					break;
				}
				if (skillSetTouchArea == 2 && ActiveTagActiving)
				{
					d3DSkillSlotUI = PickActiveSkillSlot(touch_point);
					if (d3DSkillSlotUI != null)
					{
						if (d3DSkillSlotUI.slot_index >= D3DGamer.Instance.VaildSkillSlot || d3DSkillSlotUI == ActivingSkillSlot)
						{
							break;
						}
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_DOWN), null, false, false);
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ExchangeActiveSkillInSlots(ActivingSkillSlot.slot_index, d3DSkillSlotUI.slot_index);
					}
					else
					{
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.RemoveActiveSkillInSlots(ActivingSkillSlot.slot_index);
					}
				}
				else if (skillSetTouchArea == 3 && !ActiveTagActiving)
				{
					d3DSkillSlotUI = PickPassiveSkillSlot(touch_point);
					if (d3DSkillSlotUI != null)
					{
						if (d3DSkillSlotUI.slot_index >= D3DGamer.Instance.VaildSkillSlot || d3DSkillSlotUI == ActivingSkillSlot)
						{
							break;
						}
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_DOWN), null, false, false);
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ExchangePassiveSkillInSlots(ActivingSkillSlot.slot_index, d3DSkillSlotUI.slot_index);
					}
					else
					{
						if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.CheckCurrentPassiveIsTitanPower(ActivingSkillSlot.slot_index))
						{
							D3DEquipment d3DEquipment = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1];
							if (d3DEquipment != null)
							{
								if (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ONE_HAND || d3DEquipment.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
								{
									List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CANNOT_REMOVE_DUAL);
									PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
									break;
								}
								if (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.OFF_HAND)
								{
									D3DEquipment d3DEquipment2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0];
									if (d3DEquipment2 != null && d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
									{
										List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CANNOT_REMOVE_DUAL);
										PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.OK, null);
										break;
									}
								}
							}
						}
						else if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.CheckCurrentPassiveIsDualWield(ActivingSkillSlot.slot_index))
						{
							D3DEquipment d3DEquipment3 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1];
							if (d3DEquipment3 != null && d3DEquipment3.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
							{
								List<string> msgBoxContent3 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CANNOT_REMOVE_DUAL);
								PushMessageBox(msgBoxContent3, D3DMessageBox.MgbButton.OK, null);
								break;
							}
						}
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.RemovePassiveSkillInSlots(ActivingSkillSlot.slot_index);
					}
				}
				else if (ActiveTagActiving)
				{
					PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.RemoveActiveSkillInSlots(ActivingSkillSlot.slot_index);
				}
				else
				{
					if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.CheckCurrentPassiveIsTitanPower(ActivingSkillSlot.slot_index))
					{
						D3DEquipment d3DEquipment4 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1];
						if (d3DEquipment4 != null)
						{
							if (d3DEquipment4.equipment_type == D3DEquipment.EquipmentType.ONE_HAND || d3DEquipment4.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
							{
								List<string> msgBoxContent4 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CANNOT_REMOVE_DUAL);
								PushMessageBox(msgBoxContent4, D3DMessageBox.MgbButton.OK, null);
								break;
							}
							if (d3DEquipment4.equipment_type == D3DEquipment.EquipmentType.OFF_HAND)
							{
								D3DEquipment d3DEquipment5 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[0];
								if (d3DEquipment5 != null && d3DEquipment5.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
								{
									List<string> msgBoxContent5 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CANNOT_REMOVE_DUAL);
									PushMessageBox(msgBoxContent5, D3DMessageBox.MgbButton.OK, null);
									break;
								}
							}
						}
					}
					else if (PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.CheckCurrentPassiveIsDualWield(ActivingSkillSlot.slot_index))
					{
						D3DEquipment d3DEquipment6 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[1];
						if (d3DEquipment6 != null && d3DEquipment6.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
						{
							List<string> msgBoxContent6 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CANNOT_REMOVE_DUAL);
							PushMessageBox(msgBoxContent6, D3DMessageBox.MgbButton.OK, null);
							break;
						}
					}
					PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.RemovePassiveSkillInSlots(ActivingSkillSlot.slot_index);
				}
				if (ActivingSkillBar != null)
				{
					ActivingSkillBar.Select(false);
					ActivingSkillBar = null;
				}
				ActivingSkillSlot = null;
				UpdateSkillSlots();
				break;
			case 4:
				if (!DoSkillScroll)
				{
					if (skillSetTouchArea == 2 && ActiveTagActiving)
					{
						d3DSkillSlotUI = PickActiveSkillSlot(touch_point);
						if (d3DSkillSlotUI == null)
						{
							break;
						}
						int num = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.CheckActiveSkillExistsInSlot(ActivingSkillBar.SkillId);
						if (num != -1)
						{
							if (num == d3DSkillSlotUI.slot_index)
							{
								break;
							}
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.RemoveActiveSkillInSlots(num);
						}
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_DOWN), null, false, false);
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.AddActiveSkillInSlots(ActivingSkillBar.SkillId, d3DSkillSlotUI.slot_index);
						if (ActivingSkillBar != null)
						{
							ActivingSkillBar.Select(false);
							ActivingSkillBar = null;
						}
						ActivingSkillSlot = null;
						UpdateSkillSlots();
					}
					else
					{
						if (skillSetTouchArea != 3 || ActiveTagActiving)
						{
							break;
						}
						d3DSkillSlotUI = PickPassiveSkillSlot(touch_point);
						if (d3DSkillSlotUI == null)
						{
							break;
						}
						int num2 = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.CheckPassiveSkillExistsInSlot(ActivingSkillBar.SkillId);
						if (num2 != -1)
						{
							if (num2 == d3DSkillSlotUI.slot_index)
							{
								break;
							}
							PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.RemovePassiveSkillInSlots(num2);
						}
						D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SKILL_DOWN), null, false, false);
						PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.AddPassiveSkillInSlots(ActivingSkillBar.SkillId, d3DSkillSlotUI.slot_index);
						if (ActivingSkillBar != null)
						{
							ActivingSkillBar.Select(false);
							ActivingSkillBar = null;
						}
						ActivingSkillSlot = null;
						UpdateSkillSlots();
					}
				}
				else
				{
					SkillScroll.ScrollInertia(new Vector2(0f, y_delta));
				}
				break;
			}
			StartTouchArea = -1;
			DoSkillScroll = false;
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
	}

	private void CreateSkillSchoolTouchArea()
	{
		SkillSchoolTouchArea = D3DMain.Instance.ConvertRectAutoHD(7f, 8f, 406.5f, 271f);
	}

	private void SkillSchoolTouchEvent(Vector2 touch_point, int touch_command, float x_delta, float y_delta)
	{
		switch (touch_command)
		{
		case 0:
		{
			StartTouchArea = ((!SkillSchoolTouchArea.Contains(touch_point)) ? (-1) : 0);
			if (StartTouchArea == -1)
			{
				break;
			}
			SkillScroll.StopInertia();
			D3DSkillBarUI skillBar = SkillScroll.GetSkillBar(touch_point);
			if (skillBar != null)
			{
				if (ActivingSkillBar != null)
				{
					ActivingSkillBar.Select(false);
				}
				ActivingSkillBar = skillBar;
				ActivingSkillBar.Select(true);
			}
			else
			{
				StartTouchArea = -1;
			}
			break;
		}
		case 1:
			if (StartTouchArea != -1)
			{
				DoSkillScroll = true;
				ActivingSkillBar.Select(false);
			}
			break;
		case 2:
			if (StartTouchArea != -1 && DoSkillScroll)
			{
				SkillScroll.Scroll(new Vector2(0f, y_delta));
			}
			break;
		case 4:
			if (StartTouchArea != -1)
			{
				if (!DoSkillScroll)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					ActivingSkillBar.NewHint.Visible = false;
					if (D3DGamer.Instance.NewSkillHint.ContainsKey(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID) && D3DGamer.Instance.NewSkillHint[PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID].Contains(ActivingSkillBar.SkillId))
					{
						D3DGamer.Instance.NewSkillHint[PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID].Remove(ActivingSkillBar.SkillId);
						if (D3DGamer.Instance.NewSkillHint[PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID].Count == 0)
						{
							D3DGamer.Instance.NewSkillHint.Remove(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
						}
					}
					List<UIImage> list = SkillScroll.CheckNewSkill(PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.ProfileID);
					if (list == null || list.Count == 0)
					{
						OptionHintImg[CurrentOptionIndex].Visible = false;
					}
					UpgradeSkill = null;
					if (EnabledOptions[CurrentOptionIndex] == StashOption.ACTIVE)
					{
						UpgradeSkill = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.active_skill_id_list[ActivingSkillBar.SkillId];
					}
					else if (EnabledOptions[CurrentOptionIndex] == StashOption.PASSIVE)
					{
						UpgradeSkill = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_class.passive_skill_id_list[ActivingSkillBar.SkillId];
					}
					if (UpgradeSkill != null)
					{
						List<string> list2 = new List<string>();
						if (UpgradeSkill.UpgradeRequireLevel > PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_level)
						{
							list2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_UPGRADE_SKILL_IF_LEVEL_NOT_ENOUGH);
							PushMessageBox(list2, D3DMessageBox.MgbButton.OK, null);
						}
						else if (UpgradeSkill.UpgradeCost > int.Parse(D3DGamer.Instance.CurrencyText) || UpgradeSkill.UpgradeCrystal > int.Parse(D3DGamer.Instance.CrystalText))
						{
							list2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
							List<D3DMessageBoxButtonEvent.OnButtonClick> list3 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
							list3.Add(OpenTBank);
							PushMessageBox(list2, D3DMessageBox.MgbButton.CANCEL_OK, list3);
						}
						else
						{
							D3DSkillBasic d3DSkillBasic = null;
							if (EnabledOptions[CurrentOptionIndex] == StashOption.ACTIVE)
							{
								d3DSkillBasic = D3DMain.Instance.GetActiveSkill(UpgradeSkill.skill_id);
							}
							else if (EnabledOptions[CurrentOptionIndex] == StashOption.PASSIVE)
							{
								d3DSkillBasic = D3DMain.Instance.GetPassiveSkill(UpgradeSkill.skill_id);
							}
							if (d3DSkillBasic != null)
							{
								List<D3DMessageBoxButtonEvent.OnButtonClick> list4 = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
								list4.Add(ConfirmUpgradeSkill);
								list2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CONFIRM_UPGRADE_SKILL);
								list2 = new List<string>(list2);
								int num = -1;
								for (int i = 0; i < list2.Count; i++)
								{
									if (list2[i].Contains("<GetSkill>"))
									{
										list2[i] = list2[i].Replace("<GetSkill>", d3DSkillBasic.skill_name);
									}
									if (list2[i].Contains("<GetPrice>"))
									{
										list2[i] = string.Empty;
										num = i;
									}
								}
								UIManager uIManager = PushMessageBox(list2, D3DMessageBox.MgbButton.CANCEL_OK, list4);
								if (num >= 0)
								{
									D3DCurrencyText d3DCurrencyText = new D3DCurrencyText(uIManager, this);
									if (UpgradeSkill.UpgradeCost == 0)
									{
										d3DCurrencyText.EnableGold = false;
									}
									else
									{
										d3DCurrencyText.SetGold(UpgradeSkill.UpgradeCost);
									}
									if (UpgradeSkill.UpgradeCrystal == 0)
									{
										d3DCurrencyText.EnableCrystal = false;
									}
									else
									{
										d3DCurrencyText.SetCrystal(UpgradeSkill.UpgradeCrystal);
									}
									Rect cameraTransformRect = uIManager.GetCameraTransformRect();
									float num2 = 640f / (float)Screen.height;
									cameraTransformRect = new Rect(cameraTransformRect.x * num2, cameraTransformRect.y * num2, cameraTransformRect.width * num2, cameraTransformRect.height * num2);
									d3DCurrencyText.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - d3DCurrencyText.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 205f - (float)(30 * num)));
								}
							}
						}
					}
				}
				else
				{
					SkillScroll.ScrollInertia(new Vector2(0f, y_delta));
				}
			}
			StartTouchArea = -1;
			DoSkillScroll = false;
			break;
		}
	}

	private new void Awake()
	{
		base.name = "UIStash";
		base.Awake();
		AddImageCellIndexer(new string[8] { "UImg0_cell", "UImg1_cell", "UImg2_cell", "UImg5_cell", "UImg6_cell", "UI_Monolayer_cell", "UImg7_cell", "UImg10_cell" });
		AddItemIcons();
		AddSkillIcons();
		PlayerTeamPuppetData = null;
		CurrentOptionIndex = -1;
		StartTouchArea = -1;
		UpgradeSkill = null;
		DoSkillScroll = false;
		EnableDrag = false;
		DoingDrag = false;
	}

	private new void Start()
	{
		base.Start();
		GameObject gameObject = new GameObject("NewHintObj");
		newHintBehaviour = gameObject.AddComponent<NewHintBehaviour>();
		gameObject.transform.parent = base.transform;
		for (StashUIManager stashUIManager = StashUIManager.MAIN; stashUIManager <= StashUIManager.MASK2; stashUIManager++)
		{
			CreateUIManagerEmpty();
		}
		CreateMainUI();
		StashOption[] enabledOptions = EnabledOptions;
		foreach (StashOption stash_option in enabledOptions)
		{
			CreateStashManagerByOption(stash_option);
		}
		SwitchStashOption(0);
		if (D3DMain.Instance.CurrentScene != 4 && ui_index > 1)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			uIHelper.HideFade();
		}
		if (EnabledOptions[0] == StashOption.ACTIVE)
		{
			EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, CheckSkillTutorial, true);
		}
		else if (EnabledOptions[0] == StashOption.GEARS)
		{
			EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, CheckStashTutorial, true);
		}
		else
		{
			EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, null, true);
		}
	}

	public new void Update()
	{
		base.Update();
		_SubPuppetFaceUI.Tick();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("stash_move") == control.Id)
		{
			Vector2 currentPosition = ((UIMove)control).GetCurrentPosition();
			if (EnabledOptions[CurrentOptionIndex] == StashOption.GEARS)
			{
				GearsTouchEvent(currentPosition, command, wparam, lparam);
			}
			else if (EnabledOptions[CurrentOptionIndex] == StashOption.SKILLS)
			{
				SkillSetTouchEvent(currentPosition, command, wparam, lparam);
			}
			else if (EnabledOptions[CurrentOptionIndex] == StashOption.ACTIVE || EnabledOptions[CurrentOptionIndex] == StashOption.PASSIVE)
			{
				SkillSchoolTouchEvent(currentPosition, command, wparam, lparam);
			}
			return;
		}
		if (GetControlId("BackBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
			{
				D3DGamer.Instance.PlayerTeamData.Remove(playerBattleTeamDatum);
			}
			D3DGamer.Instance.PlayerBattleTeamData.Clear();
			foreach (PuppetBasic playerTeamPuppetDatum in PlayerTeamPuppetData)
			{
				D3DGamer.D3DPuppetSaveData item = playerTeamPuppetDatum.profile_instance.ExtractPuppetSaveData();
				D3DGamer.Instance.PlayerBattleTeamData.Add(item);
				D3DGamer.Instance.PlayerTeamData.Add(item);
			}
			if (GearStore != null)
			{
				D3DGamer.Instance.PlayerStore.Clear();
				foreach (D3DEquipment item2 in GearStore)
				{
					if (item2 == null)
					{
						D3DGamer.Instance.PlayerStore.Add(null);
						continue;
					}
					D3DGamer.D3DEquipmentSaveData d3DEquipmentSaveData = new D3DGamer.D3DEquipmentSaveData();
					d3DEquipmentSaveData.equipment_id = item2.equipment_id;
					d3DEquipmentSaveData.magic_power_data = item2.magic_power_data;
					D3DGamer.Instance.PlayerStore.Add(d3DEquipmentSaveData);
				}
			}
			D3DGamer.Instance.SaveAllData();
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, CloseStash, false);
			return;
		}
		if (GearDescriptionUI != null && GearDescriptionUI.CompareButton == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_COMPARE), null, false, false);
			GameObject original = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UICompare") as GameObject;
			original = (GameObject)Object.Instantiate(original);
			UICompare component = original.GetComponent<UICompare>();
			if (ActivingStoreGear != null)
			{
				D3DEquipment selected_gear = GearStore[ActivingStoreGear.slot_index + CurrentGearPage * 12];
				D3DEquipment compareGear = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.GetCompareGear(selected_gear);
				component.StartCoroutine(component.UpdateCompareGearsInfo(selected_gear, compareGear, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, _SubPuppetFaceUI.CurrentFaceIndex));
			}
			else if (ActivingPuppetGear != null)
			{
				component.StartCoroutine(component.UpdateCompareGearsInfo(null, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[ActivingPuppetGear.slot_index], PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, _SubPuppetFaceUI.CurrentFaceIndex));
			}
			else
			{
				component.StartCoroutine(component.UpdateCompareGearsInfo(null, null, null, _SubPuppetFaceUI.CurrentFaceIndex));
			}
			return;
		}
		if (GetControlId("IapBuyBtn") == control.Id && command == 0)
		{
			_subUItBank.BuyIap();
			return;
		}
		if (TapJoyBtn == control && command == 0)
		{
			MyTapjoy.Show();
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
				List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_UNLOCK_STORE_PAGE);
				msgBoxContent = new List<string>(msgBoxContent);
				int num = -1;
				for (int j = 0; j < msgBoxContent.Count; j++)
				{
					if (msgBoxContent[j].Contains("<GetPrice>"))
					{
						msgBoxContent[j] = string.Empty;
						num = j;
					}
				}
				List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
				list.Add(IapBuyGearSpace);
				UIManager uIManager = PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
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
		if (!StashOptionsEvent(control) && !GearStorePageEvent(control) && !_SubPuppetFaceUI.PuppetFaceEvent(control) && !_subUItBank.tBankEvent(control))
		{
		}
	}

	private void CheckSkillTutorial()
	{
		if (!D3DGamer.Instance.TutorialState[4])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_ENTER_SKILL);
		}
	}

	private void CheckStashTutorial()
	{
		if (!D3DGamer.Instance.TutorialState[7])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_ENTER_STASH);
		}
	}

	private void UpdateSelectedEquipInfo()
	{
		GearDescriptionUI.UpdateDescriptionInfo(GearStore[ActivingStoreGear.slot_index + CurrentGearPage * 12], false, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, true);
		OnSelectAnEquip(false);
	}

	private void UpdateSelectEquipOnBody(int nGearIndex)
	{
		D3DEquipment gear = PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[nGearIndex];
		GearDescriptionUI.UpdateDescriptionInfo(gear, true, PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance, true);
		OnSelectAnEquip(true, nGearIndex);
	}

	private void OnSelectAnEquip(bool bIsInBody, int nGearIndex = 0)
	{
		D3DEquipment d3DEquipment = ((!bIsInBody) ? GearStore[ActivingStoreGear.slot_index + CurrentGearPage * 12] : PlayerTeamPuppetData[_SubPuppetFaceUI.CurrentFaceIndex].profile_instance.puppet_arms[nGearIndex]);
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

	private void DeSelectStoreEquip()
	{
		ActivingStoreGear.Select(false);
		_SubPuppetFaceUI.HideAll();
	}
}
