using System.Collections;
using UnityEngine;

public class UIDungeon : UIHelper
{
	private enum DungeonUIManager
	{
		MAIN = 0,
		PORTAL = 1,
		PORTAL_MOVE = 2,
		APPLICATION_PAUSE = 3,
		BATTLE_FADE = 4
	}

	protected class CurrencyTopRight
	{
		private UIImage _leftImg = new UIImage();

		private UIImage _rightImg = new UIImage();

		private UIText _num = new UIText();

		private UIImage _icon = new UIImage();

		private UIImage[] _midImg;

		private UIClickButton _btnAdd = new UIClickButton();

		private string _strBtnName;

		private float _fOffsetY;

		private bool _bIsGold = true;

		private int _nCurMidCount;

		private readonly float _fMidWidth = 10f;

		private UIManager _uiManager;

		private UIHelper _uiHelper;

		public void Create(UIHelper uihelper, UIManager manager, bool bIsGold)
		{
			_uiHelper = uihelper;
			_uiManager = manager;
			Vector2 vector = new Vector2(0f, 0f);
			_bIsGold = bIsGold;
			if (!bIsGold)
			{
				_fOffsetY = -27f;
			}
			vector = ((UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12) ? new Vector2(482f, 353f) : ((UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.IPHONE5) ? new Vector2(450f, 291f) : new Vector2(538f, 291f)));
			CreateText(uihelper, manager, new Vector4(vector.x - 118f, vector.y + 1f, 100f, 20f), D3DMain.Instance.CommonFontColor, ref _num);
			_num.SetText((!bIsGold) ? D3DGamer.Instance.CrystalText : D3DGamer.Instance.CurrencyText);
			_nCurMidCount = CalcMidCount(_num);
			CreateImage(uihelper, manager, new Vector4(vector.x, vector.y, 26f, 27f), "iapdi-you", ref _rightImg);
			_midImg = new UIImage[_nCurMidCount];
			for (int i = 0; i < _nCurMidCount; i++)
			{
				_midImg[i] = new UIImage();
				CreateImage(uihelper, manager, new Vector4(vector.x - (float)(i + 1) * _fMidWidth, vector.y, _fMidWidth, 27f), "iapdi-zhong", ref _midImg[i]);
			}
			CreateImage(uihelper, manager, new Vector4(vector.x - (float)_nCurMidCount * _fMidWidth - 13f, vector.y, 13f, 27f), "iapdi-zuo", ref _leftImg);
			CreateImage(uihelper, manager, new Vector4(vector.x - 24f, vector.y + 4f, 20f, 20f), (!bIsGold) ? "shuijing" : "jinbi", ref _icon);
			manager.Add(_num);
			D3DImageCell imageCell = uihelper.GetImageCell("anniuPlus");
			_btnAdd.SetTexture(UIButtonBase.State.Normal, uihelper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			imageCell = uihelper.GetImageCell("anniuPlus2");
			_btnAdd.SetTexture(UIButtonBase.State.Pressed, uihelper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			_btnAdd.Rect = D3DMain.Instance.ConvertRectAutoHD(vector.x - 8f, _fOffsetY + vector.y - 3f, 35f, 35f);
			_btnAdd.Id = uihelper.Cur_control_id++;
			_strBtnName = ((!bIsGold) ? "AddCrystal" : "AddGold");
			uihelper.AddControlToTable(_strBtnName, _btnAdd);
			manager.Add(_btnAdd);
		}

		private int CalcMidCount(UIText text)
		{
			float num = _num.GetTextWidth() / (float)D3DMain.Instance.HD_SIZE;
			return (int)Mathf.Ceil(num / _fMidWidth) + 2;
		}

		private void RecreateAll()
		{
			_uiManager.Remove(_rightImg);
			_uiManager.Remove(_leftImg);
			UIImage[] midImg = _midImg;
			foreach (UIImage control in midImg)
			{
				_uiManager.Remove(control);
			}
			_uiManager.Remove(_num);
			_uiManager.Remove(_btnAdd);
			_uiHelper.RemoveControlFromTable(_strBtnName);
			Create(_uiHelper, _uiManager, _bIsGold);
		}

		public void AddToChangedEventList()
		{
			D3DGamer.Instance.AddCurrencyChangedEvent(OnCurrencyChangedEventHandler);
		}

		public void RemoveFromChangedEventList()
		{
			D3DGamer.Instance.RemoveCurrencyChangedEvent(OnCurrencyChangedEventHandler);
		}

		private void CreateText(UIHelper uihelper, UIManager manager, Vector4 rect, Color textColor, ref UIText createText)
		{
			createText = new UIText();
			createText.Enable = false;
			createText.Set(uihelper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, textColor);
			createText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			createText.Rect = D3DMain.Instance.ConvertRectAutoHD(rect.x, rect.y + _fOffsetY, rect.z, rect.w);
			createText.AlignStyle = UIText.enAlignStyle.right;
		}

		private void CreateImage(UIHelper uihelper, UIManager manager, Vector4 rect, string strTexture, ref UIImage createImg)
		{
			createImg = new UIImage();
			createImg.SetTexture(uihelper.LoadUIMaterialAutoHD(uihelper.GetImageCell(strTexture).cell_texture), D3DMain.Instance.ConvertRectAutoHD(uihelper.GetImageCell(strTexture).cell_rect));
			createImg.Rect = D3DMain.Instance.ConvertRectAutoHD(rect.x, rect.y + _fOffsetY, rect.z, rect.w);
			createImg.Enable = false;
			manager.Add(createImg);
		}

		public void OnCurrencyChangedEventHandler(string strCurrency, string strCrystal)
		{
			_num.SetText((!_bIsGold) ? strCrystal : strCurrency);
			if (CalcMidCount(_num) != _nCurMidCount)
			{
				RecreateAll();
			}
		}
	}

	private bool ui_activing;

	public SceneDungeon scene_dungeon;

	private UIText dungeon_level_text;

	private D3DDungeonPortalRemakeUI portal_ui;

	private RenderTexture enter_battle_texture;

	private Camera portal_effect_camera;

	public SubUIDugeonStash _subUIDungeonStash = new SubUIDugeonStash();

	private CurrencyTopRight[] _currency = new CurrencyTopRight[2];

	public bool UIActiving
	{
		get
		{
			return ui_activing;
		}
		set
		{
			ui_activing = value;
		}
	}

	public void UpdateSubUINewHint()
	{
		_subUIDungeonStash.ShowImgBgNewHint(SubUIDugeonStash.EIconType.Gear, D3DGamer.Instance.NewGearSlotHint.Count > 0);
		bool bShow = false;
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			if (D3DGamer.Instance.NewSkillHint.ContainsKey(playerBattleTeamDatum.pupet_profile_id))
			{
				bShow = true;
				break;
			}
		}
		_subUIDungeonStash.ShowImgBgNewHint(SubUIDugeonStash.EIconType.Skill, bShow);
		_subUIDungeonStash.ShowImgBgNewHint(SubUIDugeonStash.EIconType.Team, D3DGamer.Instance.NewHeroHint.Count > 0);
	}

	private new void Awake()
	{
		base.name = "UIDungeon";
		base.Awake();
		AddImageCellIndexer(new string[7] { "UImg1_cell", "UImg2_cell", "UImg3_cell", "UImg4_cell", "UImg6_cell", "UImg10_cell", "UI_Monolayer_cell" });
	}

	public new void Start()
	{
		if (D3DMain.Instance.bDefeatedByBoss)
		{
			D3DMain.Instance.bDefeatedByBoss = false;
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UIMiniStore>();
		}
		base.Start();
		CreateUIManager("Manager_Main");
		CreateUIManager("Manager_Portal");
		CreateUIManager("Manager_PortalMove");
		CreateUIManager("AP_Manager");
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
		{
			foreach (UIManager item in m_UIManagerRef)
			{
				item.SetSpriteCameraViewPort(new Rect(0f - item.ScreenOffset.x, 0f - item.ScreenOffset.y, GameScreen.width, GameScreen.height));
			}
			if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
			{
				CreateUIByCellXml("UIDungeonNewPadCfg", m_UIManagerRef[0]);
				portal_ui = new D3DDungeonPortalRemakeUI(m_UIManagerRef[1], this, new Rect((0f - m_UIManagerRef[1].ScreenOffset.x) * 0.5f, (0f - m_UIManagerRef[1].ScreenOffset.y) * 0.5f, (float)GameScreen.width * 0.5f, (float)GameScreen.height * 0.5f), UIPortalLevel);
			}
			else
			{
				CreateUIByCellXml("UIDungeonIphone5Cfg", m_UIManagerRef[0]);
				portal_ui = new D3DDungeonPortalRemakeUI(m_UIManagerRef[1], this, new Rect((0f - m_UIManagerRef[1].ScreenOffset.x) * 0.5f, (0f - m_UIManagerRef[1].ScreenOffset.y) * 0.5f, (float)GameScreen.width * 0.5f, (float)GameScreen.height * 0.5f), UIPortalLevel);
			}
			dungeon_level_text = new UIText();
			dungeon_level_text.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 9), string.Empty, D3DMain.Instance.CommonFontColor);
			dungeon_level_text.Enable = false;
			dungeon_level_text.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			dungeon_level_text.AlignStyle = UIText.enAlignStyle.center;
			dungeon_level_text.Rect = new Rect(m_UIManagerRef[0].ScreenOffset.x + (float)(142 * D3DMain.Instance.HD_SIZE), m_UIManagerRef[0].ScreenOffset.y * 2f + (float)(279 * D3DMain.Instance.HD_SIZE), 200 * D3DMain.Instance.HD_SIZE, 30 * D3DMain.Instance.HD_SIZE);
			m_UIManagerRef[0].Add(dungeon_level_text);
		}
		else
		{
			CreateUIByCellXml("UIDungeonCfg", m_UIManagerRef[0]);
			portal_ui = new D3DDungeonPortalRemakeUI(m_UIManagerRef[1], this, new Rect(0f, 0f, 480f, 320f), UIPortalLevel);
			dungeon_level_text = new UIText();
			dungeon_level_text.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), string.Empty, D3DMain.Instance.CommonFontColor);
			dungeon_level_text.Enable = false;
			dungeon_level_text.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			dungeon_level_text.AlignStyle = UIText.enAlignStyle.center;
			dungeon_level_text.Rect = D3DMain.Instance.ConvertRectAutoHD(143f, 279f, 200f, 30f);
			m_UIManagerRef[0].Add(dungeon_level_text);
		}
		UIMove uIMove = new UIMove();
		uIMove.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		m_UIManagerRef[2].Add(uIMove);
		uIMove.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("PortalMove", uIMove);
		UIClickButton uIClickButton = new UIClickButton();
		D3DImageCell imageCell = GetImageCell("fanhui-weixuanzhong");
		uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = GetImageCell("fanhui-xuanzhong");
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		uIClickButton.Rect = new Rect(GameScreen.width - 98, 5f, 93f, 93f);
		m_UIManagerRef[2].Add(uIClickButton);
		uIClickButton.Id = cur_control_id;
		cur_control_id++;
		m_control_table.Add("PortalBackBtn", uIClickButton);
		GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/ui_door"));
		gameObject2.transform.parent = m_UIManagerRef[0].transform;
		gameObject2.transform.localPosition = m_UIManagerRef[0].transform.position + new Vector3(0f, 90000f, 90000f);
		GameObject gameObject3 = new GameObject("EffectCamera");
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = new Vector3(-520f, 0f, 590f);
		portal_effect_camera = gameObject3.AddComponent<Camera>();
		portal_effect_camera.clearFlags = CameraClearFlags.Depth;
		portal_effect_camera.cullingMask = 65536;
		portal_effect_camera.orthographic = true;
		portal_effect_camera.orthographicSize = 1000f;
		portal_effect_camera.near = -100f;
		portal_effect_camera.far = 100f;
		portal_effect_camera.depth = m_UIManagerRef[0].GetManagerCamera().depth;
		float num = (float)Screen.height / 640f;
		portal_effect_camera.pixelRect = new Rect(((float)Screen.width - 960f * num) * 0.5f + ((float)(345 * D3DMain.Instance.HD_SIZE) + m_UIManagerRef[0].ScreenOffset.x * 2f) * num, 0f, 270f * num, 286f * num);
		portal_effect_camera.aspect = portal_effect_camera.pixelRect.width / portal_effect_camera.pixelRect.height;
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
		m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
		CreateDiscountUI();
		CreateApplicationPauseUI();
		EnableUIFadeHold(Color.black, false);
		scene_dungeon.ReloadScene();
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0)
		{
			dungeon_level_text.SetText("HOME");
		}
		else
		{
			dungeon_level_text.SetText("LEVEL " + D3DMain.Instance.exploring_dungeon.current_floor);
		}
		_currency[0] = new CurrencyTopRight();
		_currency[0].Create(this, m_UIManagerRef[0], true);
		_currency[1] = new CurrencyTopRight();
		_currency[1].Create(this, m_UIManagerRef[0], false);
		CurrencyTopRight[] currency = _currency;
		foreach (CurrencyTopRight currencyTopRight in currency)
		{
			currencyTopRight.AddToChangedEventList();
		}
		_subUIDungeonStash.Initialize(this);
	}

	public new void OnDestroy()
	{
		CurrencyTopRight[] currency = _currency;
		foreach (CurrencyTopRight currencyTopRight in currency)
		{
			currencyTopRight.RemoveFromChangedEventList();
		}
	}

	private void OnEnterDungeon()
	{
		if (D3DIapDiscount.Instance.UnlockDiscountInDungeon())
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UIDiscount>().Init(false);
		}
		else
		{
			CreateNewHintNotice();
		}
		D3DMain.Instance.TriggerApplicationPause = true;
		UpdateSubUINewHint();
		portal_ui.UpdateMap(SceneDungeon.CampBackLastLevel);
		scene_dungeon.map_player.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true);
		PuppetDungeonPlayerBehaviour puppetDungeonPlayerBehaviour = null;
		puppetDungeonPlayerBehaviour = scene_dungeon.map_player.GetComponent<PuppetDungeonPlayerBehaviour>();
		if (scene_dungeon.DoingPortal)
		{
			Time.timeScale = 1f;
			if (null != puppetDungeonPlayerBehaviour)
			{
				puppetDungeonPlayerBehaviour.SetPlayerInvincibilityOnPortal();
			}
			foreach (PuppetDungeon dungeon_enemy_group in scene_dungeon.dungeon_enemy_groups)
			{
				PuppetDungeonEnmeyBehaviour component = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
				component.SetBreakStateOnPlayerPortal();
			}
			BasicEffectComponent.PlayEffect("home_02", scene_dungeon.map_player.gameObject, string.Empty, false, Vector2.one, Vector3.zero, true, 0f);
			StartCoroutine(scene_dungeon.map_player.Hide(true, 0.8f));
			StartCoroutine(ArrivePortalTargetLevel());
			return;
		}
		if (null != puppetDungeonPlayerBehaviour)
		{
			puppetDungeonPlayerBehaviour.StartCoroutine(puppetDungeonPlayerBehaviour.EnableInvincibility(SceneDungeon.player_protect_time));
		}
		foreach (PuppetDungeon dungeon_enemy_group2 in scene_dungeon.dungeon_enemy_groups)
		{
			PuppetDungeonEnmeyBehaviour component2 = dungeon_enemy_group2.GetComponent<PuppetDungeonEnmeyBehaviour>();
			component2.StartCoroutine(component2.BreakBehaviour(SceneDungeon.player_protect_time));
		}
		SceneDungeon.player_protect_time = 0f;
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0 && !D3DGamer.Instance.TutorialState[3])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_IN_CAMP);
		}
		else if (D3DMain.Instance.exploring_dungeon.current_floor != 0 && !D3DGamer.Instance.TutorialState[8])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_ENTER_DUNGEON);
		}
		else if (scene_dungeon.SpawnedGrave && !D3DGamer.Instance.TutorialState[10])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_BOSS_GRAVE);
		}
		else
		{
			Time.timeScale = 1f;
		}
		scene_dungeon.StartCoroutine("DisableFirstEnterByTime");
	}

	public void EnterDungeon()
	{
		ui_fade.StartFade(UIFade.FadeState.FADE_IN, OnEnterDungeon, true);
	}

	public void QuitDungeon()
	{
		SceneDungeon.floor_enemy_records.Clear();
		SceneDungeon.player_protect_time = 0f;
		SceneDungeon.CampBackLastLevel = -1;
		Time.timeScale = 1f;
		Application.LoadLevel(5);
	}

	public void SwitchFloor()
	{
		SceneDungeon.floor_enemy_records.Clear();
		SceneDungeon.player_protect_time = 0f;
		scene_dungeon.ReloadScene();
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0)
		{
			dungeon_level_text.SetText("HOME");
		}
		else
		{
			dungeon_level_text.SetText("LEVEL " + D3DMain.Instance.exploring_dungeon.current_floor);
		}
	}

	public void StopDungeonMusic(bool keep_amb)
	{
		if (null != D3DAudioManager.Instance.DungeonAmbAudio)
		{
			D3DDungeonModelPreset.ModelPreset modelPreset = ((D3DMain.Instance.exploring_dungeon.current_floor != 0) ? D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_model_preset) : D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_town.town_model_preset));
			if (!keep_amb || modelPreset.ModelPostfix != scene_dungeon.CurrentModelPostfix)
			{
				D3DAudioManager.Instance.DungeonAmbAudio.transform.parent = null;
				D3DAudioManager.Instance.DungeonAmbAudio.Stop();
				D3DAudioManager.Instance.DungeonAmbAudio = null;
			}
			else
			{
				D3DAudioManager.Instance.DungeonAmbAudio.transform.parent = TAudioManager.instance.AudioListener.transform;
			}
		}
		if (null != D3DAudioManager.Instance.DungeonTownAudio)
		{
			D3DAudioManager.Instance.DungeonTownAudio.Stop();
			D3DAudioManager.Instance.DungeonTownAudio = null;
		}
	}

	public void EnterArena()
	{
		m_UIManagerRef[4].gameObject.SetActiveRecursively(false);
		if (D3DDungeonProgerssManager.Instance.CurrentDungeonProgress != null && D3DDungeonProgerssManager.Instance.CurrentDungeonProgress.ContainsKey(D3DMain.Instance.exploring_dungeon.current_floor))
		{
			D3DDungeonProgerssManager.LevelProgress levelProgress = D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor];
			if (levelProgress.UnlockBattleList.ContainsKey(D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id))
			{
				D3DDungeonProgerssManager.LevelProgress.NextLevelBattleUnlock nextLevelBattleUnlock = levelProgress.UnlockBattleList[D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id];
				if (string.Empty != nextLevelBattleUnlock.on_battle_start_story && !nextLevelBattleUnlock.start_read && (string.Empty == nextLevelBattleUnlock.target_group || nextLevelBattleUnlock.target_group == D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id))
				{
					GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIStory"));
					gameObject.GetComponent<UIStory>().Init(nextLevelBattleUnlock.on_battle_start_story, StoryEnterArena, false);
					return;
				}
			}
		}
		Time.timeScale = 1f;
		SwitchLevelImmediately();
	}

	public void StoryEnterArena()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor].UnlockBattleList[D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id].start_read = true;
		D3DGamer.Instance.SaveDungeonProgress();
		Time.timeScale = 1f;
		SwitchLevelImmediately();
	}

	public void ChestLootEnd()
	{
		PuppetDungeonPlayerBehaviour component = scene_dungeon.map_player.GetComponent<PuppetDungeonPlayerBehaviour>();
		component.StartCoroutine(component.EnableInvincibility(SceneDungeon.player_protect_time));
		foreach (PuppetDungeon dungeon_enemy_group in scene_dungeon.dungeon_enemy_groups)
		{
			PuppetDungeonEnmeyBehaviour component2 = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
			component2.StartCoroutine(component2.BreakBehaviour(SceneDungeon.player_protect_time));
		}
		D3DMain.Instance.exploring_dungeon.player_trigger_chest.Respawn();
		D3DMain.Instance.exploring_dungeon.player_trigger_chest = null;
		UpdateSubUINewHint();
		SceneDungeon.player_protect_time = 0f;
		Time.timeScale = 1f;
	}

	public new void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.V))
		{
			StartCoroutine(BattleFade());
		}
		_subUIDungeonStash.Tick();
	}

	private void OpenOption()
	{
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
		PushLevel();
	}

	public void OpenStash()
	{
		UIActiving = true;
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
		Time.timeScale = 0.0001f;
		D3DMain.Instance.LoadingScene = 4;
		UIStash.EnabledOptions = new UIStash.StashOption[2]
		{
			UIStash.StashOption.GEARS,
			UIStash.StashOption.tBANK
		};
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	public void OpentBank()
	{
		UIActiving = true;
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
		Time.timeScale = 0.0001f;
		D3DMain.Instance.LoadingScene = 4;
		UIStash.EnabledOptions = new UIStash.StashOption[1] { UIStash.StashOption.tBANK };
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("MenuBtn") == control.Id && command == 0)
		{
			Time.timeScale = 0.0001f;
			D3DMain.Instance.LoadingScene = 6;
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, OpenOption, false);
		}
		else if (GetControlId("StashBtn") == control.Id && command == 0)
		{
			OpenStash();
		}
		else if (GetControlId("StoreBtn") == control.Id && command == 0)
		{
			UIActiving = true;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			Time.timeScale = 0.0001f;
			D3DMain.Instance.LoadingScene = 10;
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
		}
		else if (GetControlId("ProtalBtn") == control.Id && command == 0)
		{
			UIActiving = true;
			Time.timeScale = 0.0001f;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			int num = ((D3DMain.Instance.exploring_dungeon.current_floor != 0 || SceneDungeon.CampBackLastLevel <= 0) ? ((D3DMain.Instance.exploring_dungeon.current_floor - 1) / 15) : ((SceneDungeon.CampBackLastLevel - 1) / 15));
			m_UIManagerRef[1].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[0].gameObject.SetActiveRecursively(false);
			portal_ui.JumpScrollToCurrentFloor();
			D3DMain.Instance.TriggerApplicationPause = false;
			if (!D3DGamer.Instance.TutorialState[9])
			{
				((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_TELEPORT);
			}
		}
		else if (GetControlId("PortalBackBtn") == control.Id && command == 0)
		{
			UIActiving = false;
			Time.timeScale = 1f;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[0].gameObject.SetActiveRecursively(true);
			D3DMain.Instance.TriggerApplicationPause = true;
		}
		else if (GetControlId("PortalCampBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			UIPortalLevel(0);
		}
		else if (GetControl("PortalMove") == control)
		{
			portal_ui.UIEvent(command, ((UIMove)control).GetCurrentPosition(), new Vector2(wparam, lparam));
		}
		else if (GetControlId("ap_resume_btn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(false);
			Time.timeScale = 1f;
			if (D3DIapDiscount.Instance.UnlockDiscountInDungeon())
			{
				CreateDiscountUI();
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<UIDiscount>().Init(false);
			}
		}
		else if (GetControlId("OpenDiscountBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			GameObject gameObject2 = new GameObject();
			gameObject2.AddComponent<UIDiscount>().Init(false);
		}
		else if ((GetControlId("AddGold") == control.Id && command == 0) || (GetControlId("AddCrystal") == control.Id && command == 0))
		{
			OpentBank();
		}
		else if (!_subUIDungeonStash.OnHandleEventFromParent(control, command, wparam, lparam))
		{
		}
	}

	public override void FreezeTimeScale()
	{
		Time.timeScale = 1f;
		UIActiving = false;
		D3DMain.Instance.TriggerApplicationPause = true;
	}

	protected override void PushLevel()
	{
		base.PushLevel();
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
		}
		m_UIManagerRef[0].gameObject.SetActiveRecursively(false);
	}

	private void UIPortalLevel(int portal_level)
	{
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
		m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
		Time.timeScale = 1f;
		scene_dungeon.StartDungeonPortal();
		StartCoroutine(PortalToLevel(portal_level));
	}

	private IEnumerator PortalToLevel(int portal_level)
	{
		yield return new WaitForSeconds(1.5f);
		if (portal_level == 0)
		{
			SceneDungeon.CampBackLastLevel = D3DMain.Instance.exploring_dungeon.current_floor;
			SceneDungeon.CampBackLastPosition = scene_dungeon.map_player.transform.position;
		}
		else if (portal_level != SceneDungeon.CampBackLastLevel)
		{
			SceneDungeon.CampBackLastLevel = -1;
		}
		Time.timeScale = 0.0001f;
		UIActiving = false;
		D3DMain.Instance.exploring_dungeon.player_last_transform = null;
		scene_dungeon.ClearOldFloor();
		D3DMain.Instance.exploring_dungeon.current_floor = portal_level;
		D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.PREVIOUS;
		StopDungeonMusic(false);
		_subUIDungeonStash.ShowIcons(false, 0.01f);
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, SwitchFloor, false);
	}

	private IEnumerator ArrivePortalTargetLevel()
	{
		scene_dungeon.first_enter = true;
		yield return new WaitForSeconds(1.5f);
		scene_dungeon.DoingPortal = false;
		m_UIManagerRef[0].gameObject.SetActiveRecursively(true);
		PuppetDungeonPlayerBehaviour player_behaviour = scene_dungeon.map_player.GetComponent<PuppetDungeonPlayerBehaviour>();
		if (null != player_behaviour)
		{
			player_behaviour.StartCoroutine(player_behaviour.EnableInvincibility(SceneDungeon.player_protect_time));
		}
		foreach (PuppetDungeon enemy in scene_dungeon.dungeon_enemy_groups)
		{
			PuppetDungeonEnmeyBehaviour enemy_behaviour = enemy.GetComponent<PuppetDungeonEnmeyBehaviour>();
			enemy_behaviour.StartCoroutine(enemy_behaviour.BreakBehaviour(SceneDungeon.player_protect_time));
		}
		SceneDungeon.player_protect_time = 0f;
		Time.timeScale = 1f;
	}

	public void OpenShop()
	{
		UIActiving = true;
		Time.timeScale = 0.0001f;
		D3DMain.Instance.LoadingScene = 10;
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	public void OpenSkillSchool()
	{
		UIActiving = true;
		Time.timeScale = 0.0001f;
		D3DMain.Instance.LoadingScene = 4;
		UIStash.EnabledOptions = new UIStash.StashOption[3]
		{
			UIStash.StashOption.ACTIVE,
			UIStash.StashOption.PASSIVE,
			UIStash.StashOption.SKILLS
		};
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	public void OpenTavern()
	{
		UIActiving = true;
		Time.timeScale = 0.0001f;
		D3DMain.Instance.LoadingScene = 9;
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	private void CreateApplicationPauseUI()
	{
		UIClickButton uIClickButton = new UIClickButton();
		D3DImageCell imageCell = GetImageCell("ui_monolayer");
		Vector2 vector = new Vector2(480f, 320f);
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
		{
			vector = new Vector2(GameScreen.width, GameScreen.height);
		}
		uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), vector * D3DMain.Instance.HD_SIZE);
		uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), vector * D3DMain.Instance.HD_SIZE);
		uIClickButton.SetColor(UIButtonBase.State.Normal, new Color(0f, 0f, 0f, 0.8f));
		uIClickButton.SetColor(UIButtonBase.State.Pressed, new Color(0f, 0f, 0f, 0.8f));
		uIClickButton.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, vector.x, vector.y);
		m_UIManagerRef[3].Add(uIClickButton);
		UIText uIText = new UIText();
		uIText.AlignStyle = UIText.enAlignStyle.center;
		uIText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 13), "Pause", D3DMain.Instance.CommonFontColor);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(13 * D3DMain.Instance.HD_SIZE);
		uIText.LineSpacing = D3DMain.Instance.GameFont2.GetLineSpacing(13 * D3DMain.Instance.HD_SIZE);
		uIText.Enable = false;
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
		{
			uIText.Rect = new Rect(m_UIManagerRef[3].ScreenOffset.x + (float)(148 * D3DMain.Instance.HD_SIZE), m_UIManagerRef[3].ScreenOffset.y + (float)(140 * D3DMain.Instance.HD_SIZE), 200 * D3DMain.Instance.HD_SIZE, 50 * D3DMain.Instance.HD_SIZE);
		}
		else
		{
			uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(148f, 140f, 200f, 50f);
		}
		m_UIManagerRef[3].Add(uIText);
		m_control_table.Add("ap_resume_btn", uIClickButton);
		m_UIManagerRef[3].gameObject.SetActiveRecursively(false);
	}

	public void ApplicationPause()
	{
		if (Time.timeScale == 1f && !m_UIManagerRef[3].gameObject.active)
		{
			Time.timeScale = 0.0001f;
			m_UIManagerRef[3].gameObject.SetActiveRecursively(true);
		}
	}

	public IEnumerator BattleFade()
	{
		CreateUIManager("Manager_BattleFade");
		m_UIManagerRef[4].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[4].ScreenOffset.x, 0f - m_UIManagerRef[4].ScreenOffset.y, GameScreen.width, GameScreen.height));
		UIImage image = new UIImage();
		D3DImageCell image_cell = GetImageCell("ui_monolayer");
		Material mat = LoadUIMaterialAutoHD(image_cell.cell_texture);
		image.SetTexture(mat, new Rect(0f, 0f, GameScreen.width, GameScreen.height), new Vector2(GameScreen.width, GameScreen.height));
		image.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		image.SetColor(Color.black);
		m_UIManagerRef[4].Add(image);
		image = new UIImage();
		enter_battle_texture = new RenderTexture(GameScreen.width, GameScreen.height, 16);
		Camera.main.targetTexture = enter_battle_texture;
		image.SetTexture(new Material(mat.shader)
		{
			mainTexture = enter_battle_texture
		}, new Rect(0f, 0f, GameScreen.width, GameScreen.height));
		image.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		m_UIManagerRef[4].Add(image);
		D3DAudioManager.Instance.PlayAudio("fx_StartBattle", TAudioManager.instance.AudioListener.gameObject, true, false);
		yield return 0;
		Camera.main.targetTexture = null;
		Camera.main.gameObject.active = false;
		BattleEnterBehaviour battle_enter_behaviour = m_UIManagerRef[4].gameObject.AddComponent<BattleEnterBehaviour>();
		battle_enter_behaviour.Init(image, this);
	}

	private void QuitToTitle()
	{
		StopDungeonMusic(false);
		scene_dungeon.ClearOldFloor();
		D3DMain.Instance.exploring_dungeon.Reset();
		Time.timeScale = 0.0001f;
		D3DMain.Instance.LoadingScene = 1;
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, QuitDungeon, false);
	}

	public void SetPortalVisible(bool visible)
	{
		portal_effect_camera.gameObject.active = visible;
		UIClickButton uIClickButton = (UIClickButton)GetControl("ProtalBtn");
		uIClickButton.Visible = visible;
		uIClickButton.Enable = visible;
	}

	private void CreateDiscountUI()
	{
		if (D3DIapDiscount.Instance.CurrentDiscount >= 0 && (D3DIapDiscount.Instance.IapDiscountStates[D3DIapDiscount.Instance.CurrentDiscount] == D3DIapDiscount.DiscountState.READY || D3DIapDiscount.Instance.IapDiscountStates[D3DIapDiscount.Instance.CurrentDiscount] == D3DIapDiscount.DiscountState.UNLOCKED) && GetControl("OpenDiscountBtn") == null)
		{
			AddImageCellIndexer(new string[1] { "UImg9_cell" });
			if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
			{
				CreateUIByCellXml("UIDiscountOpenNewPadCfg", m_UIManagerRef[0]);
			}
			else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
			{
				CreateUIByCellXml("UIDiscountOpenIphone5Cfg", m_UIManagerRef[0]);
			}
			else
			{
				CreateUIByCellXml("UIDiscountOpenCfg", m_UIManagerRef[0]);
			}
			NeverDestroyedScript.DiscountOpenBtn = (UIClickButton)GetControl("OpenDiscountBtn");
			NeverDestroyedScript.DiscountText1 = (UIText)GetControl("OpenDiscountTxt");
			NeverDestroyedScript.DiscountOpenBtn.Visible = false;
			NeverDestroyedScript.DiscountOpenBtn.Enable = false;
			NeverDestroyedScript.DiscountText1.Visible = false;
		}
	}

	public void CreateNewHintNotice()
	{
		if (D3DGamer.Instance.CurrentUnlockedSkills.Count > 0)
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UINewHintNotice>().Init(true);
		}
	}

	public void UITavernBack()
	{
		if (D3DGamer.Instance.CurrentUnlockedSkills.Count > 0)
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UINewHintNotice>().Init(true);
			UIActiving = false;
		}
		else
		{
			FreezeTimeScale();
		}
	}
}
