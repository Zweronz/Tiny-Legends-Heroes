using System.Collections.Generic;
using UnityEngine;

public class UITavern : UIHelper
{
	private enum TavernUIManager
	{
		MAIN = 0,
		TAVERN = 1,
		TAVERN_PUPPET_INTRO = 2,
		TEAM = 3,
		TBANK = 4,
		MASK1 = 5,
		LV_UP = 6,
		MASK2 = 7
	}

	private enum TavernOption
	{
		TAVERN = 0,
		TEAM = 1,
		tBank = 2
	}

	private class LevelUpConfirmUI
	{
		private UIManager ui_manager;

		private UIHelper ui_helper;

		private UIText[] messages;

		private UIText[] level_texts;

		private D3DCurrencyText levelup_cost;

		private D3DProfileInstance levelup_puppet;

		public LevelUpConfirmUI(UIManager manager, UIHelper helper)
		{
			ui_manager = manager;
			ui_helper = helper;
			messages = new UIText[5];
			level_texts = new UIText[3];
			for (int i = 0; i < 5; i++)
			{
				messages[i] = new UIText();
				messages[i].Enable = false;
				messages[i].Set(helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), "aaaaa", Color.black);
				messages[i].AlignStyle = UIText.enAlignStyle.center;
				messages[i].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
				messages[i].Rect = D3DMain.Instance.ConvertRectAutoHD(100f, 205 - 22 * i, 290f, 20f);
				manager.Add(messages[i]);
				if (i <= 2)
				{
					level_texts[i] = new UIText();
					level_texts[i].Enable = false;
					level_texts[i].Set(helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), "aaaaa", new Color(0f, 46f / 85f, 4f / 51f));
					level_texts[i].AlignStyle = UIText.enAlignStyle.center;
					level_texts[i].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
					manager.Add(level_texts[i]);
				}
			}
			messages[0].SetText("You can level up this hero all the way");
			messages[1].SetText("up to    (the highest level in your team).");
			level_texts[0].SetText(D3DGamer.Instance.TeamMaxLevel.ToString());
			level_texts[0].Rect = D3DMain.Instance.ConvertRectAutoHD(144f, 183f, 30f, 20f);
			level_texts[1].Rect = D3DMain.Instance.ConvertRectAutoHD(228f, 117f, 30f, 20f);
			level_texts[2].Rect = D3DMain.Instance.ConvertRectAutoHD(265f, 117f, 30f, 20f);
			levelup_cost = new D3DCurrencyText(manager, helper);
			levelup_cost.EnableGold = false;
			levelup_cost.SetColor(Color.black);
		}

		public void Enable(bool enable)
		{
			ui_manager.gameObject.SetActiveRecursively(enable);
		}

		public void OpenTBank()
		{
			ui_manager.gameObject.SetActiveRecursively(false);
			((UITavern)ui_helper).OpenTBank();
		}

		public void UpdateLvUpInfo(D3DProfileInstance puppet_profile_instance)
		{
			levelup_puppet = puppet_profile_instance;
			if (levelup_puppet.puppet_level >= D3DGamer.Instance.TeamMaxLevel)
			{
				messages[0].Visible = false;
				messages[1].Visible = false;
				messages[2].SetText("You can't level up this hero for now!");
				messages[3].Visible = false;
				messages[4].Visible = false;
				level_texts[0].Visible = false;
				level_texts[1].Visible = false;
				level_texts[2].Visible = false;
				levelup_cost.Visible(false);
				return;
			}
			messages[0].Visible = true;
			messages[1].Visible = true;
			messages[3].Visible = true;
			messages[4].Visible = true;
			messages[2].SetText("HERO: " + puppet_profile_instance.ProfileName);
			messages[3].SetText("CLASS: " + puppet_profile_instance.puppet_class.class_name);
			messages[4].SetText("Level:   to   ");
			level_texts[0].Visible = true;
			level_texts[1].Visible = true;
			level_texts[2].Visible = true;
			level_texts[1].SetText(puppet_profile_instance.puppet_level.ToString());
			level_texts[2].SetText((puppet_profile_instance.puppet_level + 1).ToString());
			Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
			float num = 640f / (float)Screen.height;
			cameraTransformRect = new Rect(cameraTransformRect.x * num, cameraTransformRect.y * num, cameraTransformRect.width * num, cameraTransformRect.height * num);
			levelup_cost.Visible(true);
			levelup_cost.SetPosition(new Vector2((cameraTransformRect.x + cameraTransformRect.width * 0.5f - levelup_cost.GetUIWidth() * 0.5f - 8f) * (1f / (float)D3DMain.Instance.HD_SIZE), cameraTransformRect.y + 90f));
			levelup_cost.SetCrystal(LevelUpCost(levelup_puppet.puppet_level));
			if (int.Parse(D3DGamer.Instance.CrystalText) < LevelUpCost(levelup_puppet.puppet_level))
			{
				levelup_cost.SetColor(Color.red);
			}
			else
			{
				levelup_cost.SetColor(Color.black);
			}
		}

		public void PuppetLevelUp()
		{
			if (levelup_puppet.puppet_level >= D3DGamer.Instance.TeamMaxLevel)
			{
				Enable(false);
				return;
			}
			if (int.Parse(D3DGamer.Instance.CrystalText) < LevelUpCost(levelup_puppet.puppet_level))
			{
				List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
				List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
				list.Add(OpenTBank);
				ui_helper.PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
				return;
			}
			D3DAudioManager.Instance.PlayAudio("fx_LevelUp", TAudioManager.instance.AudioListener.gameObject, false, true);
			D3DGamer.Instance.UpdateCrystal(-LevelUpCost(levelup_puppet.puppet_level));
			levelup_puppet.puppet_level++;
			if (levelup_puppet.puppet_level > 40)
			{
				levelup_puppet.puppet_level = 40;
				levelup_puppet.current_exp = 0;
				return;
			}
			levelup_puppet.CheckNewSkill();
			((UITavern)ui_helper).UpdateTeamData();
			D3DGamer.Instance.SaveAllData();
			if (levelup_puppet.puppet_level >= D3DGamer.Instance.TeamMaxLevel)
			{
				Enable(false);
			}
			else
			{
				UpdateLvUpInfo(levelup_puppet);
			}
		}

		private int LevelUpCost(int level)
		{
			return Mathf.Max(1, Mathf.RoundToInt((0.09f * (Mathf.Pow(level, 1.8f) - 1f) + 2f) * (float)(29 * (level - 1) + 300) / 3f / 1000f));
		}
	}

	private List<HeroHire> NotHiredHeros;

	private List<PuppetBasic> TavernPuppet;

	private List<PuppetBasic> CampPuppet;

	private List<PuppetBasic> BattlePuppet;

	private D3DTextPushButton[] OptionBtns;

	private int CurrentOptionIndex;

	private D3DCurrencyText PlayerCurrencyText;

	private D3DCurrencyText HireCost;

	private D3DFeatureCameraUI TavernPuppetCamera;

	private D3DTavernPuppetIntroScroll PuppetIntroScroll;

	private bool DoIntroScroll;

	private int CurrentTavernPuppetIndex;

	private UIMove TeamUIMove;

	private D3DTavernTeamSlipUI TeamSlipUI;

	private UIClickButton[] LevelUpBtn;

	private LevelUpConfirmUI lvUpConfirmUI;

	private SubUItBank _subUItBank = new SubUItBank();

	private void CreateTavernPuppet()
	{
		TavernPuppet = new List<PuppetBasic>();
		CampPuppet = new List<PuppetBasic>();
		BattlePuppet = new List<PuppetBasic>();
		NotHiredHeros = new List<HeroHire>();
		NotHiredHeros.AddRange(D3DTavern.Instance.HeroHireManager);
		int num = 0;
		while (num < NotHiredHeros.Count)
		{
			if (string.Empty == NotHiredHeros[num].unlock_group)
			{
				num++;
			}
			else if (!D3DGamer.Instance.TavernPuppet.Contains(NotHiredHeros[num].puppet_id))
			{
				NotHiredHeros.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		num = 1;
		foreach (D3DGamer.D3DPuppetSaveData playerTeamDatum in D3DGamer.Instance.PlayerTeamData)
		{
			foreach (HeroHire notHiredHero in NotHiredHeros)
			{
				if (notHiredHero.puppet_id == playerTeamDatum.pupet_profile_id)
				{
					NotHiredHeros.Remove(notHiredHero);
					break;
				}
			}
			if (!D3DGamer.Instance.PlayerBattleTeamData.Contains(playerTeamDatum))
			{
				GameObject gameObject = new GameObject("CampPuppet");
				gameObject.transform.parent = base.transform;
				PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
				if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerTeamDatum.pupet_profile_id), playerTeamDatum))
				{
					Object.Destroy(gameObject);
					continue;
				}
				puppetBasic.profile_instance.InitSkillLevel(playerTeamDatum);
				puppetBasic.profile_instance.InitSkillSlots(playerTeamDatum);
				puppetBasic.model_builder.BuildPuppetModel();
				puppetBasic.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, Random.Range(0f, 2f));
				puppetBasic.CheckPuppetWeapons();
				gameObject.transform.localPosition = new Vector3(600 * D3DMain.Instance.HD_SIZE, 0f, num * 100);
				gameObject.transform.rotation = Quaternion.identity;
				puppetBasic.model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
				D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic.gameObject, 16);
				CampPuppet.Add(puppetBasic);
				num++;
			}
		}
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			GameObject gameObject2 = new GameObject("BattlePuppet");
			gameObject2.transform.parent = base.transform;
			PuppetBasic puppetBasic2 = gameObject2.AddComponent<PuppetBasic>();
			if (!puppetBasic2.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerBattleTeamDatum.pupet_profile_id), playerBattleTeamDatum))
			{
				Object.Destroy(gameObject2);
				continue;
			}
			puppetBasic2.profile_instance.InitSkillLevel(playerBattleTeamDatum);
			puppetBasic2.profile_instance.InitSkillSlots(playerBattleTeamDatum);
			puppetBasic2.model_builder.BuildPuppetModel();
			puppetBasic2.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, Random.Range(0f, 2f));
			puppetBasic2.CheckPuppetWeapons();
			gameObject2.transform.localPosition = new Vector3(600 * D3DMain.Instance.HD_SIZE, 0f, num * 100);
			gameObject2.transform.rotation = Quaternion.identity;
			puppetBasic2.model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
			D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic2.gameObject, 16);
			BattlePuppet.Add(puppetBasic2);
			num++;
		}
		foreach (HeroHire notHiredHero2 in NotHiredHeros)
		{
			D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = notHiredHero2.ConvertToPuppetSaveData();
			GameObject gameObject3 = new GameObject("HirePuppet");
			gameObject3.transform.parent = base.transform;
			PuppetBasic puppetBasic3 = gameObject3.AddComponent<PuppetBasic>();
			if (!puppetBasic3.InitProfileInstance(D3DMain.Instance.GetProfileClone(d3DPuppetSaveData.pupet_profile_id), d3DPuppetSaveData))
			{
				Object.Destroy(gameObject3);
				continue;
			}
			puppetBasic3.profile_instance.InitSkillLevel(d3DPuppetSaveData);
			puppetBasic3.profile_instance.InitSkillSlots(d3DPuppetSaveData);
			puppetBasic3.model_builder.BuildPuppetModel();
			puppetBasic3.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, Random.Range(0f, 2f));
			puppetBasic3.CheckPuppetWeapons();
			gameObject3.transform.localPosition = new Vector3(600 * D3DMain.Instance.HD_SIZE, 0f, num * 100);
			gameObject3.transform.rotation = Quaternion.identity;
			puppetBasic3.model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
			D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic3.gameObject, 16);
			TavernPuppet.Add(puppetBasic3);
			num++;
		}
	}

	private void CloseTavern()
	{
		if (D3DMain.Instance.CurrentScene != 9)
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
				foreach (PuppetBasic item in CampPuppet)
				{
					if (!(null == item) && D3DGamer.Instance.CurrentUnlockedSkills.ContainsKey(item.profile_instance.ProfileID))
					{
						D3DGamer.Instance.CurrentUnlockedSkills.Remove(item.profile_instance.ProfileID);
					}
				}
				uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, ((UIDungeon)uIHelper).UITavernBack, true);
				Object.Destroy(base.gameObject);
			}
			else
			{
				if (ui_index > 1)
				{
					UIHelper uIHelper2 = D3DMain.Instance.D3DUIList[ui_index - 2];
					uIHelper2.ui_fade.StartFade(UIFade.FadeState.FADE_IN, uIHelper2.FreezeTimeScale, true);
				}
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			D3DMain.Instance.CurrentScene = 1;
			Application.LoadLevel(1);
		}
	}

	public void UpdateTeamData()
	{
		D3DGamer.Instance.PlayerTeamData.Clear();
		D3DGamer.Instance.PlayerBattleTeamData.Clear();
		foreach (PuppetBasic item3 in BattlePuppet)
		{
			if (!(null == item3))
			{
				D3DGamer.D3DPuppetSaveData item = item3.profile_instance.ExtractPuppetSaveData();
				D3DGamer.Instance.PlayerBattleTeamData.Add(item);
				D3DGamer.Instance.PlayerTeamData.Add(item);
			}
		}
		foreach (PuppetBasic item4 in CampPuppet)
		{
			if (!(null == item4))
			{
				D3DGamer.D3DPuppetSaveData item2 = item4.profile_instance.ExtractPuppetSaveData();
				D3DGamer.Instance.PlayerTeamData.Add(item2);
			}
		}
	}

	private void CreateMainUI()
	{
		InsertUIManager("Manager_Main", 0);
		CreateUIByCellXml("UITavernMainCfg", m_UIManagerRef[0]);
		string[] array = new string[3] { "HIRE", "EDIT", "tBank" };
		OptionBtns = new D3DTextPushButton[3];
		for (int i = 0; i < 3; i++)
		{
			OptionBtns[i] = new D3DTextPushButton(m_UIManagerRef[0], this);
			OptionBtns[i].CreateControl(new Vector2(82 * i, 283f), new Vector2(84f, 37f), "anniu1", "anniu2", string.Empty, D3DMain.Instance.GameFont2.FontName, 11, 22, array[i], (D3DMain.Instance.HD_SIZE != 2) ? new Vector2(0f, 1f) : new Vector2(0f, -3f), (float)D3DMain.Instance.HD_SIZE * 1.5f, D3DMain.Instance.CommonFontColor, new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f));
		}
		OptionBtns[0].Set(true);
		OptionBtns[2].Enable(false);
		OptionBtns[2].Visible(false);
		InsertUIManager("Manager_mask2", 7);
		PlayerCurrencyText = new D3DCurrencyText(m_UIManagerRef[7], this);
		UpdateCurrencyUI();
		D3DImageCell imageCell = GetImageCell("jinengditu");
		UIImage[] array2 = new UIImage[4]
		{
			new UIImage(),
			null,
			null,
			null
		};
		array2[0].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		if (D3DMain.Instance.HD_SIZE == 1)
		{
			array2[0].Rect = new Rect(4f, 143f, 208f, 141f);
		}
		else
		{
			array2[0].Rect = new Rect(6f, 286f, 416f, 282f);
		}
		array2[0].FlipX(true);
		m_UIManagerRef[0].Add(array2[0]);
		array2[1] = new UIImage();
		array2[1].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		if (D3DMain.Instance.HD_SIZE == 1)
		{
			array2[1].Rect = new Rect(210f, 143f, 208f, 141f);
		}
		else
		{
			array2[1].Rect = new Rect(420f, 286f, 416f, 282f);
		}
		m_UIManagerRef[0].Add(array2[1]);
		array2[2] = new UIImage();
		array2[2].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		if (D3DMain.Instance.HD_SIZE == 1)
		{
			array2[2].Rect = new Rect(4f, 4f, 208f, 141f);
		}
		else
		{
			array2[2].Rect = new Rect(6f, 8f, 416f, 282f);
		}
		array2[2].FlipX(true);
		array2[2].FlipY(true);
		m_UIManagerRef[0].Add(array2[2]);
		array2[3] = new UIImage();
		array2[3].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		if (D3DMain.Instance.HD_SIZE == 1)
		{
			array2[3].Rect = new Rect(210f, 4f, 208f, 141f);
		}
		else
		{
			array2[3].Rect = new Rect(420f, 8f, 416f, 282f);
		}
		array2[3].FlipY(true);
		m_UIManagerRef[0].Add(array2[3]);
		UIText uIText = new UIText();
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), "All heroes recruited!", Color.black);
		uIText.Enable = false;
		uIText.AlignStyle = UIText.enAlignStyle.center;
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(8f, 125f, 410f, 30f);
		m_UIManagerRef[0].Add(uIText);
		CreateTavernPuppet();
		InsertUIManager("Manager_Mask1", 5);
		m_UIManagerRef[5].EnableUIHandler = false;
		D3DBigBoardUI d3DBigBoardUI = new D3DBigBoardUI(m_UIManagerRef[5], this);
		d3DBigBoardUI.CreateBigBoard(new Vector2(-5f, -5f));
	}

	private bool TavernOptionsEvent(UIControl control)
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
					SwitchTavernOption(num);
				}
				return true;
			}
			num++;
		}
		return false;
	}

	private void SwitchTavernOption(int option_index)
	{
		if (option_index == CurrentOptionIndex)
		{
			return;
		}
		CurrentOptionIndex = option_index;
		switch (CurrentOptionIndex)
		{
		case 0:
		{
			D3DFeatureCameraUI[] battle_puppet3 = TeamSlipUI.battle_puppet;
			foreach (D3DFeatureCameraUI d3DFeatureCameraUI5 in battle_puppet3)
			{
				d3DFeatureCameraUI5.Visible(false);
			}
			D3DFeatureCameraSlipUI[] current_page_camp_puppet3 = TeamSlipUI.current_page_camp_puppet;
			foreach (D3DFeatureCameraUI d3DFeatureCameraUI6 in current_page_camp_puppet3)
			{
				d3DFeatureCameraUI6.Visible(false);
			}
			m_UIManagerRef[3].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[4].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[1].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(true);
			UpdateCurrentTavernPuppet();
			break;
		}
		case 1:
		{
			m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			TavernPuppetCamera.Visible(false);
			m_UIManagerRef[4].gameObject.SetActiveRecursively(false);
			D3DFeatureCameraUI[] battle_puppet2 = TeamSlipUI.battle_puppet;
			foreach (D3DFeatureCameraUI d3DFeatureCameraUI3 in battle_puppet2)
			{
				d3DFeatureCameraUI3.Visible(true);
			}
			D3DFeatureCameraSlipUI[] current_page_camp_puppet2 = TeamSlipUI.current_page_camp_puppet;
			foreach (D3DFeatureCameraUI d3DFeatureCameraUI4 in current_page_camp_puppet2)
			{
				d3DFeatureCameraUI4.Visible(true);
			}
			m_UIManagerRef[3].gameObject.SetActiveRecursively(true);
			UpdateTeamUI();
			break;
		}
		case 2:
		{
			m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			TavernPuppetCamera.Visible(false);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(true);
			D3DFeatureCameraUI[] battle_puppet = TeamSlipUI.battle_puppet;
			foreach (D3DFeatureCameraUI d3DFeatureCameraUI in battle_puppet)
			{
				d3DFeatureCameraUI.Visible(false);
			}
			D3DFeatureCameraSlipUI[] current_page_camp_puppet = TeamSlipUI.current_page_camp_puppet;
			foreach (D3DFeatureCameraUI d3DFeatureCameraUI2 in current_page_camp_puppet)
			{
				d3DFeatureCameraUI2.Visible(false);
			}
			m_UIManagerRef[4].gameObject.SetActiveRecursively(true);
			break;
		}
		}
	}

	private void UpdateCurrencyUI()
	{
		PlayerCurrencyText.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		PlayerCurrencyText.SetPosition(new Vector2(475f - PlayerCurrencyText.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), 293f));
	}

	private void UpdateFaceFrame()
	{
	}

	private void CreateTavernUI()
	{
		InsertUIManager("Manager_Tavern", 1);
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = GetImageCell("zuorenwulan");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(8f, 8f, 201f, 271f);
		m_UIManagerRef[1].Add(uIImage);
		uIImage = new UIImage();
		imageCell = GetImageCell("youshuxinglan");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(209f, 8f, 206f, 271f);
		m_UIManagerRef[1].Add(uIImage);
		UIClickButton uIClickButton = new UIClickButton();
		imageCell = GetImageCell("zuofangxiangjian1");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = GetImageCell("zuofangxiangjian1-1");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(15f, 120f, 38f, 50f);
		uIClickButton.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernPuppetLeftBtn", uIClickButton);
		m_UIManagerRef[1].Add(uIClickButton);
		uIClickButton = new UIClickButton();
		imageCell = GetImageCell("youfangxiangjian1");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = GetImageCell("youfangxiangjian1-1");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(165f, 120f, 38f, 50f);
		uIClickButton.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernPuppetRightBtn", uIClickButton);
		m_UIManagerRef[1].Add(uIClickButton);
		uIClickButton = new UIClickButton();
		imageCell = GetImageCell("anniu1");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = GetImageCell("anniu2");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(65f, 10f, 84f, 37f);
		uIClickButton.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernHireBtn", uIClickButton);
		m_UIManagerRef[1].Add(uIClickButton);
		UIText uIText = new UIText();
		uIText.AlignStyle = UIText.enAlignStyle.center;
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), "HIRE", D3DMain.Instance.CommonFontColor);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(67f, -1f, 84f, 37f);
		uIText.Enable = false;
		m_UIManagerRef[1].Add(uIText);
		uIText = new UIText();
		uIText.AlignStyle = UIText.enAlignStyle.center;
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), "HERO", D3DMain.Instance.CommonFontColor);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(65f, 225f, 84f, 37f);
		uIText.Enable = false;
		uIText.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernHeroTypeText", uIText);
		m_UIManagerRef[1].Add(uIText);
		uIText = new UIText();
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), "AAAAAA", Color.white);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(220f, 239f, 200f, 30f);
		uIText.Enable = false;
		uIText.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernHeroNameText", uIText);
		m_UIManagerRef[1].Add(uIText);
		uIText = new UIText();
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), "CLASS:WARRIOR", new Color(0.11764706f, 1f / 15f, 0.003921569f));
		uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(220f, 216f, 200f, 30f);
		uIText.Enable = false;
		uIText.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernHeroClassText", uIText);
		m_UIManagerRef[1].Add(uIText);
		uIText = new UIText();
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), "LEVEL:1", new Color(0.11764706f, 1f / 15f, 0.003921569f));
		uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(220f, 196f, 200f, 30f);
		uIText.Enable = false;
		uIText.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernHeroLevelText", uIText);
		m_UIManagerRef[1].Add(uIText);
		UIImage uIImage2 = new UIImage();
		imageCell = GetImageCell("newhero");
		uIImage2.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage2.Rect = D3DMain.Instance.ConvertRectAutoHD(145f, 190f, 51f, 52.5f);
		uIImage2.Visible = false;
		uIImage2.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("NewHeroTag", uIImage2);
		m_UIManagerRef[1].Add(uIImage2);
		HireCost = new D3DCurrencyText(m_UIManagerRef[1], this);
		UIMove uIMove = new UIMove();
		uIMove.Rect = D3DMain.Instance.ConvertRectAutoHD(220f, 0f, 189f, 320f);
		uIMove.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("TavernMove", uIMove);
		m_UIManagerRef[1].Add(uIMove);
		TavernPuppetCamera = new D3DFeatureCameraUI(m_UIManagerRef[1], this);
		TavernPuppetCamera.CreateControl(new Vector2(11f, 75f), string.Empty, Vector2.one, null, Vector2.zero, new Vector2(201f, 200f));
	}

	private void CreateTavernPuppetIntroUI()
	{
		InsertUIManager("Manager_TavernPuppetIntro", 2);
		PuppetIntroScroll = new D3DTavernPuppetIntroScroll(m_UIManagerRef[2], this, new Rect(220f, 15f, 189f, 180f));
		PuppetIntroScroll.CreateScrollBar(false, true);
		PuppetIntroScroll.InitScrollBar();
	}

	private void UpdateCurrentTavernPuppet()
	{
		if (TavernPuppet.Count == 0)
		{
			m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			TavernPuppetCamera.Visible(false);
			return;
		}
		PuppetBasic puppetBasic = TavernPuppet[CurrentTavernPuppetIndex];
		HeroHire heroHire = NotHiredHeros[CurrentTavernPuppetIndex];
		TavernPuppetCamera.Visible(true);
		TavernPuppetCamera.SetCameraFeatureObject(puppetBasic.gameObject);
		D3DPuppetTransformCfg transformCfg = puppetBasic.model_builder.TransformCfg;
		TavernPuppetCamera.SetCameraFeatureTransform(transformCfg.tavern_hire_camera_cfg.offset, transformCfg.tavern_hire_camera_cfg.rotation, transformCfg.tavern_hire_camera_cfg.size);
		if (heroHire.hire_cost > 0)
		{
			HireCost.EnableGold = true;
			HireCost.SetGold(heroHire.hire_cost);
		}
		else
		{
			HireCost.EnableGold = false;
		}
		if (heroHire.hire_crystal > 0)
		{
			HireCost.EnableCrystal = true;
			HireCost.SetCrystal(heroHire.hire_crystal);
		}
		else
		{
			HireCost.EnableCrystal = false;
		}
		HireCost.SetPosition(new Vector2(((float)(106 * D3DMain.Instance.HD_SIZE) - HireCost.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), 60f));
		((UIText)GetControl("TavernHeroNameText")).SetText(puppetBasic.profile_instance.ProfileName);
		((UIText)GetControl("TavernHeroClassText")).SetText("CLASS:" + puppetBasic.profile_instance.puppet_class.class_name);
		((UIText)GetControl("TavernHeroLevelText")).SetText("LEVEL:" + heroHire.default_level);
		HeroSynopsis hero_synopsis = null;
		if (D3DTavern.Instance.HeroSynopsisManager.ContainsKey(puppetBasic.profile_instance.ProfileID))
		{
			hero_synopsis = D3DTavern.Instance.HeroSynopsisManager[puppetBasic.profile_instance.ProfileID];
		}
		PuppetIntroScroll.UpdatePuppetIntro(hero_synopsis);
		if (D3DGamer.Instance.NewHeroHint.Contains(puppetBasic.profile_instance.ProfileID))
		{
			D3DGamer.Instance.NewHeroHint.Remove(puppetBasic.profile_instance.ProfileID);
			GetControl("NewHeroTag").Visible = true;
		}
		else
		{
			GetControl("NewHeroTag").Visible = false;
		}
	}

	private void HireCurrentTavernPuppet()
	{
		if (int.Parse(D3DGamer.Instance.CurrencyText) < NotHiredHeros[CurrentTavernPuppetIndex].hire_cost || int.Parse(D3DGamer.Instance.CrystalText) < NotHiredHeros[CurrentTavernPuppetIndex].hire_crystal)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CASH_NOT_ENOUGH_OPEN_IAP);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OpenTBank);
			PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.CANCEL_OK, list);
			return;
		}
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.MONEY_GET), null, false, false);
		D3DGamer.Instance.UpdateCurrency(-NotHiredHeros[CurrentTavernPuppetIndex].hire_cost);
		D3DGamer.Instance.UpdateCrystal(-NotHiredHeros[CurrentTavernPuppetIndex].hire_crystal);
		UpdateCurrencyUI();
		D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = TavernPuppet[CurrentTavernPuppetIndex].profile_instance.ExtractPuppetSaveData();
		foreach (string key in TavernPuppet[CurrentTavernPuppetIndex].profile_instance.puppet_class.active_skill_id_list.Keys)
		{
			if (TavernPuppet[CurrentTavernPuppetIndex].profile_instance.puppet_class.active_skill_id_list[key].skill_level >= 0)
			{
				TavernPuppet[CurrentTavernPuppetIndex].profile_instance.battle_active_slots = new string[1];
				TavernPuppet[CurrentTavernPuppetIndex].profile_instance.battle_active_slots[0] = TavernPuppet[CurrentTavernPuppetIndex].profile_instance.puppet_class.active_skill_id_list[key].active_skill.skill_id;
				break;
			}
		}
		TavernPuppet[CurrentTavernPuppetIndex].profile_instance.CheckNewSkill(true);
		CampPuppet.Add(TavernPuppet[CurrentTavernPuppetIndex]);
		TavernPuppet.RemoveAt(CurrentTavernPuppetIndex);
		NotHiredHeros.RemoveAt(CurrentTavernPuppetIndex);
		CurrentTavernPuppetIndex--;
		if (CurrentTavernPuppetIndex < 0)
		{
			CurrentTavernPuppetIndex = 0;
		}
		UpdateCurrentTavernPuppet();
		UpdateTeamData();
		D3DGamer.Instance.SaveAllData();
	}

	private void CreateTeamUI()
	{
		InsertUIManager("Manager_Team", 3);
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = GetImageCell("team-beijing-5");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Enable = false;
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(5f, 108f, 412f, 155f);
		m_UIManagerRef[3].Add(uIImage);
		uIImage = new UIImage();
		imageCell = GetImageCell("team-beijing-4");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIImage.Enable = false;
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(5f, 7f, 412f, 86f);
		m_UIManagerRef[3].Add(uIImage);
		UIText uIText = new UIText();
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 9), "Tap to add to team", new Color(0.9372549f, 73f / 85f, 0.76862746f));
		uIText.AlignStyle = UIText.enAlignStyle.center;
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		uIText.Enable = false;
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(123f, 250f, 179f, 20f);
		m_UIManagerRef[3].Add(uIText);
		TeamSlipUI = new D3DTavernTeamSlipUI(m_UIManagerRef[3], this, CampPuppet.Count, new Rect(8f, 8f, 407f, 157f), 100f);
		TeamSlipUI.SetUpdateNextPageCampPuppetDelegate(UpdateNextPageCampPuppet);
		TeamSlipUI.SetUpdateCurrentPageCampPuppetDelegate(UpdateCurrentPageCampPuppet);
		TeamSlipUI.SetEditCampPuppetToTeam(EditCampPuppetToBattle);
		TeamSlipUI.SetEditBattlePuppetToCamp(EditBattlePuppetToCamp);
		LevelUpBtn = new UIClickButton[3];
		imageCell = GetImageCell("ui_monolayer");
		for (int i = 0; i < 3; i++)
		{
			TeamSlipUI.battle_puppet[i].CreateControl(new Vector2(88 + 75 * i, 3f), string.Empty, Vector2.zero, null, Vector2.zero, new Vector2(100f, 112f));
			TeamSlipUI.battle_puppet[i].Visible(false);
			TeamSlipUI.current_page_camp_puppet[i].CreateControl(new Vector2(20 + 130 * i, 115f), string.Empty, Vector2.zero, null, Vector2.zero, new Vector2(180f, 200f));
			TeamSlipUI.current_page_camp_puppet[i].Visible(false);
			TeamSlipUI.next_page_camp_puppet[i].CreateControl(new Vector2(20 + 130 * i, 115f), string.Empty, Vector2.zero, null, Vector2.zero, new Vector2(180f, 200f));
			TeamSlipUI.next_page_camp_puppet[i].Visible(false);
			TeamSlipUI.current_page_camp_puppet_tag[i].Rect = D3DMain.Instance.ConvertRectAutoHD(5 + 131 * i, 105f, 150f, 30f);
			TeamSlipUI.current_page_camp_puppet_tag[i].Visible = false;
		}
		TeamUIMove = new UIMove();
		TeamUIMove.Rect = D3DMain.Instance.ConvertRectAutoHD(8f, 8f, 407f, 242f);
		m_UIManagerRef[3].Add(TeamUIMove);
		UIControlSpringBehaviour uIControlSpringBehaviour = base.gameObject.AddComponent<UIControlSpringBehaviour>();
		LevelUpBtn = new UIClickButton[3];
		imageCell = GetImageCell("level-up");
		for (int j = 0; j < 3; j++)
		{
			LevelUpBtn[j] = new UIClickButton();
			LevelUpBtn[j].SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			LevelUpBtn[j].SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			LevelUpBtn[j].Rect = D3DMain.Instance.ConvertRectAutoHD(110 + 130 * j, 210f, 45f, 42f);
			m_UIManagerRef[3].Add(LevelUpBtn[j]);
			uIControlSpringBehaviour.AddControl(LevelUpBtn[j]);
		}
		InsertUIManager("Manager_LvUp", 6);
		CreateUIByCellXml("UITeamLevelUp", m_UIManagerRef[6]);
		lvUpConfirmUI = new LevelUpConfirmUI(m_UIManagerRef[6], this);
		lvUpConfirmUI.Enable(false);
	}

	private void UpdateTeamUI()
	{
		for (int i = 0; i < 3; i++)
		{
			if (i > BattlePuppet.Count - 1)
			{
				TeamSlipUI.battle_puppet[i].Visible(false);
				continue;
			}
			if (null == BattlePuppet[i])
			{
				TeamSlipUI.battle_puppet[i].Visible(false);
				continue;
			}
			TeamSlipUI.battle_puppet[i].SetCameraFeatureObject(BattlePuppet[i].gameObject);
			D3DPuppetTransformCfg transformCfg = BattlePuppet[i].model_builder.TransformCfg;
			TeamSlipUI.battle_puppet[i].SetCameraFeatureTransform(transformCfg.tavern_battle_team_cfg.offset, transformCfg.tavern_battle_team_cfg.rotation, transformCfg.tavern_battle_team_cfg.size);
			TeamSlipUI.battle_puppet[i].Visible(true);
		}
		int page_count = ((CampPuppet.Count != 0) ? ((CampPuppet.Count - 1) / 3 + 1) : 0);
		TeamSlipUI.RegroupPageDot(page_count);
		UpdateCurrentPageCampPuppet();
	}

	private void UpdateCurrentPageCampPuppet()
	{
		for (int i = 0; i < 3; i++)
		{
			LevelUpBtn[i].Visible = false;
			LevelUpBtn[i].Enable = false;
			int num = TeamSlipUI.CurrentPageIndex * 3 + i;
			if (num > CampPuppet.Count - 1)
			{
				TeamSlipUI.current_page_camp_puppet[i].SetCameraFeatureObject(null);
				TeamSlipUI.current_page_camp_puppet[i].Visible(false);
				TeamSlipUI.current_page_camp_puppet_tag[i].Visible = false;
				continue;
			}
			if (null == CampPuppet[num])
			{
				TeamSlipUI.current_page_camp_puppet[i].Visible(false);
				TeamSlipUI.current_page_camp_puppet_tag[i].Visible = false;
				continue;
			}
			TeamSlipUI.current_page_camp_puppet[i].SetX(20 + 130 * i);
			TeamSlipUI.current_page_camp_puppet[i].SetCameraFeatureObject(CampPuppet[num].gameObject);
			D3DPuppetTransformCfg transformCfg = CampPuppet[num].model_builder.TransformCfg;
			TeamSlipUI.current_page_camp_puppet[i].SetCameraFeatureTransform(transformCfg.tavern_camp_camera_cfg.offset, transformCfg.tavern_camp_camera_cfg.rotation, transformCfg.tavern_camp_camera_cfg.size);
			TeamSlipUI.current_page_camp_puppet[i].Visible(true);
			TeamSlipUI.current_page_camp_puppet_tag[i].SetText("Lv." + CampPuppet[num].profile_instance.puppet_level + "  " + CampPuppet[num].profile_instance.puppet_class.class_name);
			TeamSlipUI.current_page_camp_puppet_tag[i].Visible = true;
			if (CampPuppet[num].profile_instance.puppet_level >= 40)
			{
				LevelUpBtn[i].Visible = false;
				LevelUpBtn[i].Enable = false;
			}
			else
			{
				LevelUpBtn[i].Visible = true;
				LevelUpBtn[i].Enable = true;
			}
		}
	}

	private void UpdateNextPageCampPuppet()
	{
		if (TeamSlipUI.NextPageIndex < 0 || TeamSlipUI.NextPageIndex >= TeamSlipUI.PageCount)
		{
			for (int i = 0; i < TeamSlipUI.next_page_camp_puppet.Length; i++)
			{
				TeamSlipUI.next_page_camp_puppet[i].SetCameraFeatureObject(null);
				TeamSlipUI.next_page_camp_puppet[i].Visible(false);
			}
			return;
		}
		for (int j = 0; j < 3; j++)
		{
			int num = TeamSlipUI.NextPageIndex * 3 + j;
			if (num > CampPuppet.Count - 1)
			{
				TeamSlipUI.next_page_camp_puppet[j].SetCameraFeatureObject(null);
				TeamSlipUI.next_page_camp_puppet[j].Visible(false);
				continue;
			}
			if (null == CampPuppet[num])
			{
				TeamSlipUI.next_page_camp_puppet[j].SetCameraFeatureObject(null);
				TeamSlipUI.next_page_camp_puppet[j].Visible(false);
				continue;
			}
			TeamSlipUI.next_page_camp_puppet[j].SetX(TeamSlipUI.NextPageX * 1f / (float)D3DMain.Instance.HD_SIZE + 20f + (float)(130 * j));
			TeamSlipUI.next_page_camp_puppet[j].SetCameraFeatureObject(CampPuppet[num].gameObject);
			D3DPuppetTransformCfg transformCfg = CampPuppet[num].model_builder.TransformCfg;
			TeamSlipUI.next_page_camp_puppet[j].SetCameraFeatureTransform(transformCfg.tavern_camp_camera_cfg.offset, transformCfg.tavern_camp_camera_cfg.rotation, transformCfg.tavern_camp_camera_cfg.size);
			TeamSlipUI.next_page_camp_puppet[j].Visible(true);
		}
	}

	private void EditCampPuppetToBattle(int index)
	{
		int num = TeamSlipUI.CurrentPageIndex * 3;
		int index2 = num + index;
		PuppetBasic puppetBasic = CampPuppet[index2];
		int num2 = BattlePuppet.IndexOf(null);
		if (num2 < 0)
		{
			if (BattlePuppet.Count >= 3)
			{
				return;
			}
			BattlePuppet.Add(null);
			num2 = BattlePuppet.Count - 1;
		}
		BattlePuppet[num2] = puppetBasic;
		puppetBasic.profile_instance.battle_puppet = true;
		TeamSlipUI.battle_puppet[num2].SetCameraFeatureObject(puppetBasic.gameObject);
		D3DPuppetTransformCfg transformCfg = puppetBasic.model_builder.TransformCfg;
		TeamSlipUI.battle_puppet[num2].SetCameraFeatureTransform(transformCfg.tavern_battle_team_cfg.offset, transformCfg.tavern_battle_team_cfg.rotation, transformCfg.tavern_battle_team_cfg.size);
		TeamSlipUI.battle_puppet[num2].Visible(true);
		CampPuppet[index2] = null;
		TeamSlipUI.current_page_camp_puppet[index].SetCameraFeatureObject(null);
		TeamSlipUI.current_page_camp_puppet[index].Visible(false);
		UpdateCurrentPageCampPuppet();
		bool flag = true;
		for (int i = 0; i < 3 && num + i < CampPuppet.Count; i++)
		{
			if (null != CampPuppet[num + i])
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		for (int j = 0; j < 3; j++)
		{
			if (num >= CampPuppet.Count)
			{
				break;
			}
			CampPuppet.RemoveAt(num);
		}
		int page_count = ((CampPuppet.Count != 0) ? ((CampPuppet.Count - 1) / 3 + 1) : 0);
		TeamSlipUI.RegroupPageDot(page_count);
		TeamSlipUI.ClearCurrentEmptyPage();
	}

	private void EditBattlePuppetToCamp(int index)
	{
		PuppetBasic puppetBasic = BattlePuppet[index];
		puppetBasic.profile_instance.battle_puppet = false;
		int num = CampPuppet.IndexOf(null);
		if (num < 0)
		{
			CampPuppet.Add(puppetBasic);
		}
		else
		{
			CampPuppet[num] = puppetBasic;
		}
		BattlePuppet[index] = null;
		TeamSlipUI.battle_puppet[index].SetCameraFeatureObject(null);
		TeamSlipUI.battle_puppet[index].Visible(false);
		int page_count = ((CampPuppet.Count != 0) ? ((CampPuppet.Count - 1) / 3 + 1) : 0);
		TeamSlipUI.RegroupPageDot(page_count);
		UpdateCurrentPageCampPuppet();
	}

	public void OpenTBank()
	{
		OptionBtns[CurrentOptionIndex].Set(false);
		OptionBtns[2].Visible(true);
		OptionBtns[2].Enable(true);
		OptionBtns[2].Set(true);
		CurrentOptionIndex = -1;
		SwitchTavernOption(2);
	}

	private new void Awake()
	{
		base.name = "UITavern";
		base.Awake();
		AddImageCellIndexer(new string[9] { "UImg0_cell", "UImg1_cell", "UImg2_cell", "UImg3_cell", "UImg4_cell", "UImg5_cell", "UI_Monolayer_cell", "UImg7_cell", "UImg9_cell" });
		AddSkillIcons();
		TavernPuppet = null;
		CampPuppet = null;
		BattlePuppet = null;
	}

	private new void Start()
	{
		foreach (string item in D3DGamer.Instance.NewHeroHint)
		{
		}
		base.Start();
		for (TavernUIManager tavernUIManager = TavernUIManager.MAIN; tavernUIManager <= TavernUIManager.MASK2; tavernUIManager++)
		{
			CreateUIManagerEmpty();
		}
		CreateMainUI();
		CreateTavernUI();
		CreateTavernPuppetIntroUI();
		CreateTeamUI();
		_subUItBank.CreateTBankUI(4, this, UpdateCurrencyUI, UpdateFaceFrame);
		CurrentOptionIndex = -1;
		SwitchTavernOption(0);
		if (D3DMain.Instance.CurrentScene != 9 && ui_index > 1)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			uIHelper.HideFade();
		}
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, CheckTutorial, true);
	}

	public new void Update()
	{
		base.Update();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControl("TavernMove") == control)
		{
			switch (command)
			{
			case 0:
				if (D3DMain.Instance.ConvertRectAutoHD(220f, 15f, 189f, 180f).Contains(((UIMove)control).GetCurrentPosition()))
				{
					DoIntroScroll = true;
					PuppetIntroScroll.StopInertia();
				}
				break;
			case 2:
				if (DoIntroScroll)
				{
					PuppetIntroScroll.Scroll(Vector2.up * lparam);
				}
				break;
			case 4:
				if (DoIntroScroll)
				{
					PuppetIntroScroll.ScrollInertia(Vector2.up * lparam);
				}
				DoIntroScroll = false;
				break;
			case 1:
			case 3:
				break;
			}
			return;
		}
		if (GetControlId("TavernBackBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			bool flag = true;
			foreach (PuppetBasic item in BattlePuppet)
			{
				if (null != item)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_TEAM_EMPTY_TRY_QUIT);
				PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, null);
			}
			else
			{
				UpdateTeamData();
				D3DGamer.Instance.SaveAllData();
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, CloseTavern, false);
			}
			return;
		}
		if (GetControlId("TavernPuppetLeftBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			CurrentTavernPuppetIndex--;
			if (CurrentTavernPuppetIndex < 0)
			{
				CurrentTavernPuppetIndex = TavernPuppet.Count - 1;
			}
			UpdateCurrentTavernPuppet();
			return;
		}
		if (GetControlId("TavernPuppetRightBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			CurrentTavernPuppetIndex++;
			if (CurrentTavernPuppetIndex >= TavernPuppet.Count)
			{
				CurrentTavernPuppetIndex = 0;
			}
			UpdateCurrentTavernPuppet();
			return;
		}
		if (GetControlId("TavernHireBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			List<string> msgBoxContent2 = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_CONFIRM_HIRE_HERO);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(HireCurrentTavernPuppet);
			PushMessageBox(msgBoxContent2, D3DMessageBox.MgbButton.CANCEL_OK, list);
			return;
		}
		if (GetControlId("IapBuyBtn") == control.Id && command == 0)
		{
			_subUItBank.BuyIap();
			return;
		}
		if (TeamUIMove == control)
		{
			if (D3DPlaneGeometry.PtInRect(D3DMain.Instance.ConvertRectAutoHD(8f, 96f, 407f, 146f), TeamUIMove.GetCurrentPosition()))
			{
				if (command == 1)
				{
					UIClickButton[] levelUpBtn = LevelUpBtn;
					foreach (UIClickButton uIClickButton in levelUpBtn)
					{
						uIClickButton.Visible = false;
						uIClickButton.Enable = false;
					}
				}
				TeamSlipUI.PageSlip((UIMove.Command)command, TeamUIMove.GetCurrentPosition(), wparam);
			}
			else if (D3DPlaneGeometry.PtInRect(D3DMain.Instance.ConvertRectAutoHD(85f, 8f, 232f, 80f), TeamUIMove.GetCurrentPosition()))
			{
				TeamSlipUI.BattlePuppetCameraClick((UIMove.Command)command, TeamUIMove.GetCurrentPosition());
			}
			else
			{
				TeamSlipUI.PageSlip(UIMove.Command.End, TeamUIMove.GetCurrentPosition(), wparam);
			}
			return;
		}
		if (GetControl("LvUpOkBtn") == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			lvUpConfirmUI.PuppetLevelUp();
			UpdateCurrencyUI();
			UpdateCurrentPageCampPuppet();
			return;
		}
		if (GetControl("LvUpCloseBtn") == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			lvUpConfirmUI.Enable(false);
			return;
		}
		for (int j = 0; j < LevelUpBtn.Length; j++)
		{
			if (LevelUpBtn[j] == control && command == 0)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				lvUpConfirmUI.Enable(true);
				lvUpConfirmUI.UpdateLvUpInfo(CampPuppet[TeamSlipUI.CurrentPageIndex * 3 + j].profile_instance);
				return;
			}
		}
		if (!TavernOptionsEvent(control) && !_subUItBank.tBankEvent(control))
		{
		}
	}

	private void CheckTutorial()
	{
		if (!D3DGamer.Instance.TutorialState[6])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_ENTER_TAVERN);
		}
	}
}
