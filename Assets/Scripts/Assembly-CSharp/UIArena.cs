using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArena : UIHelper
{
	private enum ArenaUIManager
	{
		BATTLE = 0,
		FACE_MASK = 1,
		PAUSE = 2,
		BATTLE_RESULT = 3
	}

	public class ArenaFaceButton
	{
		private UIHelper ui_helper;

		private PuppetArena link_puppet;

		private PuppetBasic face_puppet;

		private D3DFeatureCameraButton face_button;

		private UIImage button_mask;

		public PuppetArena LinkPuppet
		{
			get
			{
				return link_puppet;
			}
		}

		public UIPushButton PushButton
		{
			get
			{
				return face_button.PushButton;
			}
		}

		public ArenaFaceButton(UIManager button_manager, UIManager mask_manager, UIHelper helper, Vector2 button_position, Vector2 mask_position, PuppetArena puppet, int index)
		{
			ui_helper = helper;
			link_puppet = puppet;
			D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = link_puppet.profile_instance.ExtractPuppetSaveData();
			GameObject gameObject = new GameObject("FacePuppet" + index);
			gameObject.transform.parent = button_manager.transform;
			face_puppet = gameObject.AddComponent<PuppetBasic>();
			face_puppet.InitProfileInstance(D3DMain.Instance.GetProfileClone(d3DPuppetSaveData.pupet_profile_id), d3DPuppetSaveData);
			face_puppet.model_builder.BuildPuppetFaceFeatureModel();
			face_puppet.model_builder.PlayFeatureAnimation();
			gameObject.transform.localPosition = new Vector3(1000f, 0f, index * 100);
			gameObject.transform.rotation = Quaternion.identity;
			face_puppet.model_builder.SetAllClipSpeed(1f);
			D3DMain.Instance.SetGameObjectGeneralLayer(face_puppet.gameObject, 16);
			bool flag = D3DGamer.Instance.ExpBonus == 0.2f && 0.1f == D3DGamer.Instance.GoldBonus;
			face_button = new D3DFeatureCameraButton(button_manager, helper);
			face_button.CreateControl(button_position, string.Empty, Vector2.zero, gameObject, new Vector2(1f, 8.3f), new Vector2(50f, 40f), new string[3]
			{
				"touxiangkuang" + ((!flag) ? string.Empty : "2"),
				(!flag) ? "touxiangkuang-zhanchangxuanzhong-1" : "touxiangkuang2-zhanchangxuanzhong-1",
				string.Empty
			}, new Rect(0f, 0f, 58f, 58f));
			D3DPuppetTransformCfg transformCfg = face_puppet.model_builder.TransformCfg;
			face_button.SetCameraFeatureTransform(transformCfg.face_camera_cfg.offset + new Vector3(0f, -0.2f, 0f), transformCfg.face_camera_cfg.rotation, transformCfg.face_camera_cfg.size * 0.8f);
			button_mask = new UIImage();
			D3DImageCell imageCell = helper.GetImageCell((!flag) ? "touxiangkuang-1" : "touxiangkuang2-1");
			button_mask.SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			button_mask.Enable = false;
			button_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(mask_position.x, mask_position.y, 58f, 14f);
			mask_manager.Add(button_mask);
		}

		public void DestroyPuppet()
		{
			Object.Destroy(face_puppet.gameObject);
		}

		public void RefreshFacePuppet()
		{
			face_puppet.gameObject.SetActiveRecursively(true);
		}

		public void Set(bool down)
		{
			face_button.Set(down);
			D3DImageCell d3DImageCell = ((D3DGamer.Instance.ExpBonus != 0.2f || 0.1f != D3DGamer.Instance.GoldBonus) ? ui_helper.GetImageCell((!down) ? "touxiangkuang-1" : "touxiangkuang-zhanchangxuanzhong-2") : ui_helper.GetImageCell((!down) ? "touxiangkuang2-1" : "touxiangkuang2-zhanchangxuanzhong-2"));
			button_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(d3DImageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect));
		}

		public void Visible(bool visible)
		{
			face_button.Visible(visible);
			button_mask.Visible = visible;
		}
	}

	public bool battle_ui_event = true;

	public SceneArena scene_arena;

	private bool new_hero_hint;

	private List<D3DLootTreasure> LootTreasures;

	private List<D3DEquipment> LootDraw;

	private D3DCurrencyText PlayerCurrency;

	private D3DCurrencyText RetreatLostGold;

	private ArenaFaceButton current_face_button;

	private List<ArenaFaceButton> arena_face_buttons;

	private List<ArenaFaceButton> face_button_cache;

	public int face_index;

	private List<D3DBattleSkillUI> skill_ui_list;

	private List<D3DBattleSkillUI> skill_ui_cache;

	public UIText kill_text;

	private D3DBattleResultUI BattleResultUI;

	private BattleLootShuffle LootShuffle;

	private TutorialDamaoHints _tutorialDamaoHints;

	public void FadeInArena()
	{
		ui_fade.StartFade(UIFade.FadeState.FADE_IN, OnBattleStart, true);
	}

	private void OnBattleStart()
	{
		Time.timeScale = 1f;
		D3DMain.Instance.TriggerApplicationPause = true;
	}

	private void OnRetreat()
	{
		D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
		Time.timeScale = 1f;
		SwitchLevelImmediately();
	}

	private void OnBattleEnd()
	{
		if (new_hero_hint)
		{
			List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_UNLOCK_NEW_HERO);
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(OnBattleEnd);
			UIManager uIManager = PushMessageBox(msgBoxContent, D3DMessageBox.MgbButton.OK, list);
			uIManager.GetManagerCamera().depth = ui_index + 1;
			isFadeing = false;
			new_hero_hint = false;
			return;
		}
		D3DGamer.Instance.UpdateLastBattleInfo(D3DMain.Instance.exploring_dungeon.current_floor, D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id, 1 == scene_arena.BattleResult);
		if (scene_arena.BattleResult == 1)
		{
			if (!D3DGamer.Instance.TutorialState[0])
			{
				D3DGamer.Instance.TutorialState[0] = true;
				D3DGamer.Instance.TutorialState[1] = true;
				D3DGamer.Instance.SaveTutorialState();
				D3DGamer.Instance.DefaultTeamData();
				D3DGamer.Instance.DefaultProgress();
				if (D3DMain.Instance.LootEquipments.Count > 0)
				{
					D3DMain.Instance.LoadingScene = 8;
					Time.timeScale = 1f;
					SwitchLevelImmediately();
					D3DMain.Instance.CurrentScene = -1;
					return;
				}
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
				return;
			}
			D3DMain.Instance.exploring_dungeon.CurrentFloor.StartSpawnerRespawn(D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id, scene_arena.battle_gained_gold);
			D3DGamer.Instance.SaveAllData();
			if (D3DMain.Instance.LootEquipments.Count > 0)
			{
				D3DMain.Instance.LoadingScene = 8;
			}
			else
			{
				if (D3DDungeonProgerssManager.Instance.CurrentDungeonProgress != null && D3DDungeonProgerssManager.Instance.CurrentDungeonProgress.ContainsKey(D3DMain.Instance.exploring_dungeon.current_floor))
				{
					D3DDungeonProgerssManager.LevelProgress levelProgress = D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor];
					if (levelProgress.UnlockBattleList.ContainsKey(D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id))
					{
						D3DDungeonProgerssManager.LevelProgress.NextLevelBattleUnlock nextLevelBattleUnlock = levelProgress.UnlockBattleList[D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id];
						if (string.Empty != nextLevelBattleUnlock.on_battle_win_story && !nextLevelBattleUnlock.win_read && (string.Empty == nextLevelBattleUnlock.target_group || nextLevelBattleUnlock.target_group == D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id))
						{
							GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIStory"));
							gameObject.GetComponent<UIStory>().Init(nextLevelBattleUnlock.on_battle_win_story, StoryBackToDungeon);
							return;
						}
					}
				}
				D3DMain.Instance.LoadingScene = 3;
				D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
			}
		}
		else
		{
			if (scene_arena.IsBoss)
			{
				D3DMain.Instance.bDefeatedByBoss = true;
			}
			D3DMain.Instance.exploring_dungeon.current_floor = 0;
			D3DMain.Instance.exploring_dungeon.player_last_transform = null;
			D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.PREVIOUS;
			D3DMain.Instance.LoadingScene = 3;
			D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
		}
		Time.timeScale = 1f;
		SwitchLevelImmediately();
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
		D3DMain.Instance.exploring_dungeon.player_battle_group_data = null;
		Time.timeScale = 1f;
		SwitchLevelImmediately();
	}

	public IEnumerator EndBattleDelay()
	{
		yield return new WaitForSeconds(2f);
		if (!D3DGamer.Instance.TutorialState[0])
		{
			ShowTutorialNewHeroComin(D3DTutorialHintCfg.DamaoHintData.HintCondition.End, 0, 0f);
		}
		else
		{
			//ChartBoostAndroid.showInterstitial(null);
		}
		yield return new WaitForSeconds(0.5f);
		m_UIManagerRef[0].gameObject.SetActiveRecursively(false);
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
		D3DMain.Instance.TriggerApplicationPause = false;
		battle_ui_event = false;
		if (scene_arena.BattleResult == 1)
		{
			if (D3DDungeonProgerssManager.Instance.CurrentDungeonProgress != null && D3DDungeonProgerssManager.Instance.CurrentDungeonProgress.ContainsKey(D3DMain.Instance.exploring_dungeon.current_floor))
			{
				D3DDungeonProgerssManager.LevelProgress level_progress = D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor];
				if (level_progress.UnlockBattleList.ContainsKey(D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id))
				{
					D3DDungeonProgerssManager.LevelProgress.NextLevelBattleUnlock battle_unlock = level_progress.UnlockBattleList[D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id];
					if (string.Empty == battle_unlock.target_group || battle_unlock.target_group == D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id)
					{
						battle_unlock.win_target = true;
						D3DGamer.Instance.SaveDungeonProgress();
					}
				}
			}
			scene_arena.StartCoroutine(scene_arena.EnableBattleWinBehaviour());
		}
		else
		{
			if (null != D3DAudioManager.Instance.ArenaAudio)
			{
				D3DAudioManager.Instance.ArenaAudio.Stop();
				D3DAudioManager.Instance.ArenaAudio = null;
			}
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BATTLE_LOSE), null, false, false);
			PopBattleResultUI(false);
		}
	}

	public void PopBattleResultUI(bool win)
	{
		int gold_bonus = scene_arena.battle_gained_gold;
		if (win)
		{
			if (!D3DGamer.Instance.TutorialState[0])
			{
				D3DMain.Instance.LootEquipments.Clear();
				D3DMain.Instance.LootGoldBag.Clear();
				LootDraw.Clear();
				D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("weapon_bow_g_001"));
				D3DMain.Instance.LootGoldBag.Add(null);
				D3DMain.Instance.LootGoldBag.Add(null);
				D3DMain.Instance.LootGoldBag.Add(null);
				LootTreasures = new List<D3DLootTreasure>();
				for (int i = 0; i < 1; i++)
				{
					D3DLootTreasure d3DLootTreasure = new D3DLootTreasure(m_UIManagerRef[3], this);
					d3DLootTreasure.CreateControl(new Vector2(m_UIManagerRef[3].ScreenOffset.x * 0.5f + 200f, m_UIManagerRef[3].ScreenOffset.y * 0.5f + 100f));
					LootTreasures.Add(d3DLootTreasure);
				}
				LootShuffle = m_UIManagerRef[3].gameObject.AddComponent<BattleLootShuffle>();
				LootShuffle.Init(LootTreasures, BattleResultUI);
				gold_bonus = 0;
			}
			else
			{
				BattleLoot();
				LootShuffle = m_UIManagerRef[3].gameObject.AddComponent<BattleLootShuffle>();
				LootShuffle.Init(LootTreasures, BattleResultUI);
				foreach (HeroHire item in D3DTavern.Instance.HeroHireManager)
				{
					if (item.unlock_group == D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id && !D3DGamer.Instance.TavernPuppet.Contains(item.puppet_id))
					{
						D3DGamer.Instance.NewHeroHint.Add(item.puppet_id);
						D3DGamer.Instance.TavernPuppet.Add(item.puppet_id);
						new_hero_hint = true;
					}
				}
			}
		}
		BattleResultUI.SetResultUI(win, gold_bonus);
		m_UIManagerRef[3].gameObject.SetActiveRecursively(true);
		Time.timeScale = 0.0001f;
	}

	private void BattleLoot()
	{
		EnemyGroupBattleData player_battle_group_data = D3DMain.Instance.exploring_dungeon.player_battle_group_data;
		D3DMain.Instance.LootEquipments.Clear();
		D3DMain.Instance.LootGoldBag.Clear();
		LootDraw.Clear();
		D3DMain.Instance.MutationSlot.Clear();
		D3DMain.Instance.LootEquipments = D3DMain.Instance.LootRandomEquipmentsOnBattleWin(player_battle_group_data, D3DMain.Instance.exploring_dungeon.CurrentFloor);
		LootTreasures = new List<D3DLootTreasure>();
		Vector2 vector = new Vector2(95f, 0f);
		if (D3DMain.Instance.LootEquipments.Count == 1)
		{
			vector = new Vector2(95f, 95f);
		}
		else if (D3DMain.Instance.LootEquipments.Count == 2)
		{
			vector = new Vector2(130f, 30f);
		}
		for (int i = 0; i < D3DMain.Instance.LootEquipments.Count; i++)
		{
			D3DLootTreasure d3DLootTreasure = new D3DLootTreasure(m_UIManagerRef[3], this);
			d3DLootTreasure.CreateControl(new Vector2(m_UIManagerRef[3].ScreenOffset.x * 0.5f + 105f + vector.x * (float)i + vector.y, m_UIManagerRef[3].ScreenOffset.y * 0.5f + 100f));
			LootTreasures.Add(d3DLootTreasure);
		}
	}

	public void CreateFaceButtonForPuppet(PuppetArena puppet)
	{
		int count = arena_face_buttons.Count;
		ArenaFaceButton arenaFaceButton;
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
		{
			arenaFaceButton = new ArenaFaceButton(m_UIManagerRef[0], m_UIManagerRef[1], this, new Vector2((0f - m_UIManagerRef[0].ScreenOffset.x) * 0.5f + 5f + (float)(count * 70), m_UIManagerRef[0].ScreenOffset.y * 0.5f + 265f), new Vector2(count * 70, 0f), puppet, count);
			arenaFaceButton.PushButton.Rect = new Rect((5 + count * 70) * D3DMain.Instance.HD_SIZE, m_UIManagerRef[0].ScreenOffset.y * 2f + (float)(265 * D3DMain.Instance.HD_SIZE), arenaFaceButton.PushButton.Rect.width, arenaFaceButton.PushButton.Rect.height);
		}
		else
		{
			arenaFaceButton = new ArenaFaceButton(m_UIManagerRef[0], m_UIManagerRef[1], this, new Vector2(5 + count * 70, 265f), new Vector2(count * 70, 0f), puppet, count);
		}
		arena_face_buttons.Add(arenaFaceButton);
	}

	private ArenaFaceButton GetPuppetFaceButton(PuppetArena puppet)
	{
		foreach (ArenaFaceButton arena_face_button in arena_face_buttons)
		{
			if (arena_face_button.LinkPuppet == puppet)
			{
				return arena_face_button;
			}
		}
		return null;
	}

	private ArenaFaceButton GetPuppetFaceButtonInCache(PuppetArena puppet)
	{
		foreach (ArenaFaceButton item in face_button_cache)
		{
			if (item.LinkPuppet == puppet)
			{
				return item;
			}
		}
		return null;
	}

	public void SetFaceButtonDown(PuppetArena puppet)
	{
		ArenaFaceButton puppetFaceButton = GetPuppetFaceButton(puppet);
		if (puppetFaceButton == null)
		{
			if (current_face_button != null)
			{
				current_face_button.Set(false);
				current_face_button = null;
			}
		}
		else if (puppetFaceButton != current_face_button)
		{
			if (current_face_button != null)
			{
				current_face_button.Set(false);
			}
			current_face_button = puppetFaceButton;
			current_face_button.Set(true);
		}
	}

	public void AddFaceButton(PuppetArena puppet)
	{
		ArenaFaceButton puppetFaceButtonInCache = GetPuppetFaceButtonInCache(puppet);
		if (puppetFaceButtonInCache != null)
		{
			puppetFaceButtonInCache.Visible(true);
			face_button_cache.Remove(puppetFaceButtonInCache);
			arena_face_buttons.Add(puppetFaceButtonInCache);
		}
	}

	public void ClearFaceButtons()
	{
		foreach (ArenaFaceButton arena_face_button in arena_face_buttons)
		{
			arena_face_button.DestroyPuppet();
		}
		arena_face_buttons.Clear();
		foreach (ArenaFaceButton item in face_button_cache)
		{
			item.DestroyPuppet();
		}
		face_button_cache.Clear();
	}

	public void RemoveFaceButton(PuppetArena puppet)
	{
		ArenaFaceButton puppetFaceButton = GetPuppetFaceButton(puppet);
		if (puppetFaceButton != null)
		{
			if (puppetFaceButton == current_face_button)
			{
				current_face_button.Set(false);
				current_face_button = null;
			}
			puppetFaceButton.Visible(false);
			arena_face_buttons.Remove(puppetFaceButton);
			face_button_cache.Add(puppetFaceButton);
		}
	}

	public bool FaceButtonEvent(UIControl control)
	{
		foreach (ArenaFaceButton arena_face_button in arena_face_buttons)
		{
			if (control != arena_face_button.PushButton)
			{
				continue;
			}
			if (arena_face_button == current_face_button)
			{
				arena_face_button.Set(true);
			}
			else
			{
				if (current_face_button != null)
				{
					current_face_button.Set(false);
				}
				arena_face_button.Set(true);
				current_face_button = arena_face_button;
				scene_arena.UIPickPuppet(current_face_button.LinkPuppet);
			}
			return true;
		}
		return false;
	}

	public void UpdateBattleSkillUI(Dictionary<string, D3DClassBattleSkillStatus> active_skill_list)
	{
		if (active_skill_list == null)
		{
			return;
		}
		int num = 0;
		foreach (string key in active_skill_list.Keys)
		{
			D3DClassBattleSkillStatus skill_status = active_skill_list[key];
			D3DBattleSkillUI d3DBattleSkillUI;
			if (skill_ui_cache.Count > 0)
			{
				d3DBattleSkillUI = skill_ui_cache[0];
				skill_ui_cache.RemoveAt(0);
			}
			else
			{
				d3DBattleSkillUI = new D3DBattleSkillUI(m_UIManagerRef[0], this);
			}
			if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
			{
				d3DBattleSkillUI.InitUI(skill_status, new Vector2(m_UIManagerRef[0].ScreenOffset.x + 265f + (float)(num % 9 * 55), m_UIManagerRef[0].ScreenOffset.y + 275f - (float)(num / 9 * 55)));
			}
			else
			{
				d3DBattleSkillUI.InitUI(skill_status, new Vector2(265 + num % 9 * 55, 275 - num / 9 * 55));
			}
			skill_ui_list.Add(d3DBattleSkillUI);
			num++;
		}
		UpdateSkillCDMask();
	}

	public void ClearBattleSkillUI()
	{
		foreach (D3DBattleSkillUI item in skill_ui_list)
		{
			item.DisableUI();
		}
		skill_ui_cache.AddRange(skill_ui_list);
		skill_ui_list.Clear();
	}

	private bool SkillUIEvent(UIControl control)
	{
		foreach (D3DBattleSkillUI item in skill_ui_list)
		{
			if (control != item.press_mask)
			{
				continue;
			}
			if (item.skill_status.Enable)
			{
				if (scene_arena.activing_puppet.profile_instance.CheckBattleActiveSkill(item.skill_status))
				{
					scene_arena.activing_puppet.TriggerSkill(item.skill_status, false);
				}
			}
			else
			{
				item.press_mask.Set(false);
			}
			return true;
		}
		return false;
	}

	private void UpdateSkillCDMask()
	{
		foreach (D3DBattleSkillUI item in skill_ui_list)
		{
			Rect rect = item.cd_mask.Rect;
			float num = ((!(item.skill_status.CDPercent < 0f)) ? item.skill_status.CDPercent : 0f);
			item.cd_mask.SetClip(new Rect(rect.x, rect.y, rect.width, rect.height * num));
			item.disable_mask.Visible = !item.skill_status.Enable;
		}
	}

	private new void Awake()
	{
		base.name = "UIArena";
		base.Awake();
		AddImageCellIndexer(new string[6] { "UImg0_cell", "UImg1_cell", "UImg2_cell", "UImg3_cell", "UImg6_cell", "UI_Monolayer_cell" });
		AddItemIcons();
		AddSkillIcons();
		current_face_button = null;
		arena_face_buttons = new List<ArenaFaceButton>();
		face_button_cache = new List<ArenaFaceButton>();
		skill_ui_list = new List<D3DBattleSkillUI>();
		skill_ui_cache = new List<D3DBattleSkillUI>();
		LootDraw = new List<D3DEquipment>();
	}

	public new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Battle");
		CreateUIManager("Manager_FaceMask");
		CreateUIManager("Manager_Pause");
		CreateUIManager("Manager_Loot");
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
		{
			foreach (UIManager item in m_UIManagerRef)
			{
				item.SetSpriteCameraViewPort(new Rect(0f - item.ScreenOffset.x, 0f - item.ScreenOffset.y, GameScreen.width, GameScreen.height));
			}
			if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
			{
				CreateUIByCellXml("UIArenaBattleNewPadCfg", m_UIManagerRef[0]);
				CreateUIByCellXml("UIArenaPauseNewPadCfg", m_UIManagerRef[2]);
				RetreatLostGold = new D3DCurrencyText(m_UIManagerRef[2], this);
				RetreatLostGold.SetGoldColor(Color.black);
				RetreatLostGold.SetPosition(new Vector2(114f, 90f));
			}
			else
			{
				CreateUIByCellXml("UIArenaBattleIphone5Cfg", m_UIManagerRef[0]);
				CreateUIByCellXml("UIArenaPauseIphone5Cfg", m_UIManagerRef[2]);
				RetreatLostGold = new D3DCurrencyText(m_UIManagerRef[2], this);
				RetreatLostGold.SetGoldColor(Color.black);
				RetreatLostGold.SetPosition(new Vector2(114f, 90f));
			}
			BattleResultUI = new D3DBattleResultUI(m_UIManagerRef[3], this, m_UIManagerRef[0].ScreenOffset * 0.5f);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(false);
			kill_text = new UIText();
			kill_text.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			kill_text.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), scene_arena.kill_count + " / " + D3DMain.Instance.exploring_dungeon.player_battle_group_data.kill_require, Color.white);
			kill_text.Enable = false;
			kill_text.Visible = true;
			kill_text.AlignStyle = UIText.enAlignStyle.center;
			kill_text.Rect = new Rect(m_UIManagerRef[0].ScreenOffset.x + (float)(213 * D3DMain.Instance.HD_SIZE), m_UIManagerRef[0].ScreenOffset.y * 2f + (float)(265 * D3DMain.Instance.HD_SIZE), 60 * D3DMain.Instance.HD_SIZE, 30 * D3DMain.Instance.HD_SIZE);
			m_UIManagerRef[0].Add(kill_text);
			m_UIManagerRef[1].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[1].ScreenOffset.x + (float)(5 * D3DMain.Instance.HD_SIZE), m_UIManagerRef[1].ScreenOffset.y + (float)(265 * D3DMain.Instance.HD_SIZE), 210 * D3DMain.Instance.HD_SIZE, 14 * D3DMain.Instance.HD_SIZE));
		}
		else
		{
			CreateUIByCellXml("UIArenaBattleCfg", m_UIManagerRef[0]);
			CreateUIByCellXml("UIArenaPauseCfg", m_UIManagerRef[2]);
			BattleResultUI = new D3DBattleResultUI(m_UIManagerRef[3], this, Vector2.zero);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[3].gameObject.SetActiveRecursively(false);
			kill_text = new UIText();
			kill_text.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
			kill_text.Set(LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 9), scene_arena.kill_count + " / " + D3DMain.Instance.exploring_dungeon.player_battle_group_data.kill_require, Color.white);
			kill_text.Enable = false;
			kill_text.Visible = true;
			kill_text.AlignStyle = UIText.enAlignStyle.center;
			kill_text.Rect = new Rect(205 * D3DMain.Instance.HD_SIZE, 265 * D3DMain.Instance.HD_SIZE, 70 * D3DMain.Instance.HD_SIZE, 30 * D3DMain.Instance.HD_SIZE);
			m_UIManagerRef[0].Add(kill_text);
			m_UIManagerRef[1].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(5f, 265f, 210f, 14f));
			RetreatLostGold = new D3DCurrencyText(m_UIManagerRef[2], this);
			RetreatLostGold.SetGoldColor(Color.black);
			RetreatLostGold.SetPosition(new Vector2(114f, 90f));
		}
		if (!D3DGamer.Instance.TutorialState[0])
		{
			AddImageCellIndexer(new string[1] { "UImg8_cell" });
			_tutorialDamaoHints = new TutorialDamaoHints();
			_tutorialDamaoHints.Create(0, this);
			GetControl("ArenaPauseRetreatBtn").Visible = false;
			GetControl("ArenaPauseRetreatBtn").Enable = false;
			GetControl("ArenaPauseRetreatTxt").Visible = false;
			if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
			{
				((UIClickButton)GetControl("ArenaPauseResumeBtn")).Rect = new Rect(422f, 288f, 180f, 74f);
				((UIText)GetControl("ArenaPauseResumeTxt")).Rect = new Rect(428f, 264f, 180f, 74f);
			}
			else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
			{
				((UIClickButton)GetControl("ArenaPauseResumeBtn")).Rect = new Rect(478f, 224f, 180f, 74f);
				((UIText)GetControl("ArenaPauseResumeTxt")).Rect = new Rect(484f, 200f, 180f, 74f);
			}
			else
			{
				((UIClickButton)GetControl("ArenaPauseResumeBtn")).Rect = D3DMain.Instance.ConvertRectAutoHD(195f, 112f, 90f, 37f);
				((UIText)GetControl("ArenaPauseResumeTxt")).Rect = D3DMain.Instance.ConvertRectAutoHD(198f, 100f, 90f, 37f);
			}
			RetreatLostGold.Visible(false);
		}
		else
		{
			RetreatLostGold.EnableCrystal = false;
			D3DMain.Instance.exploring_dungeon.player_battle_group_data.EstimateRetreatMulct();
			RetreatLostGold.SetGold(D3DMain.Instance.exploring_dungeon.player_battle_group_data.RetreatMulct);
			Rect rect = GetControl("ArenaPauseRetreatBtn").Rect;
			RetreatLostGold.SetPosition(new Vector2((rect.x + rect.width * 0.5f - RetreatLostGold.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), rect.y * (1f / (float)D3DMain.Instance.HD_SIZE) - 20f));
			RetreatLostGold.SetGoldColor(Color.red);
		}
		PlayerCurrency = new D3DCurrencyText(m_UIManagerRef[2], this);
		UpdateCurrencyUI();
		m_UIManagerRef[1].EnableUIHandler = false;
		EnableUIFadeHold(Color.white, false);
		Time.timeScale = 0.0001f;
	}

	public new void Update()
	{
		base.Update();
		if (_tutorialDamaoHints != null && Input.GetMouseButtonDown(0) && _tutorialDamaoHints.IsShown && GameObject.Find("UITutorial") == null)
		{
			_tutorialDamaoHints.OnScreenTouched();
		}
		if (m_UIManagerRef[0].gameObject.active)
		{
			UpdateSkillCDMask();
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControl("PauseBtn") == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			GamePause();
		}
		else if (GetControl("ArenaPauseResumeBtn") == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[0].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[1].gameObject.SetActiveRecursively(true);
			foreach (ArenaFaceButton arena_face_button in arena_face_buttons)
			{
				arena_face_button.RefreshFacePuppet();
			}
			foreach (ArenaFaceButton item2 in face_button_cache)
			{
				item2.Visible(false);
			}
			battle_ui_event = true;
			Time.timeScale = 1f;
		}
		else if (GetControl("ArenaPauseRetreatBtn") == control && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			if (null != D3DAudioManager.Instance.ArenaAudio)
			{
				D3DAudioManager.Instance.ArenaAudio.Stop();
				D3DAudioManager.Instance.ArenaAudio = null;
			}
			D3DMain.Instance.LoadingScene = 3;
			D3DGamer.Instance.UpdateCurrency(-D3DMain.Instance.exploring_dungeon.player_battle_group_data.RetreatMulct);
			UpdateCurrencyUI();
			D3DGamer.Instance.SaveAllData();
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, OnRetreat, false);
		}
		else if (GetControl("ResultButton") == control && command == 0)
		{
			if (null == LootShuffle || !LootShuffle.enabled)
			{
				foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
				{
					D3DGamer.Instance.PlayerTeamData.Remove(playerBattleTeamDatum);
				}
				D3DGamer.Instance.PlayerBattleTeamData.Clear();
				foreach (PuppetArena playerTeamDatum in scene_arena.playerTeamData)
				{
					D3DGamer.D3DPuppetSaveData item = playerTeamDatum.profile_instance.ExtractPuppetSaveData();
					D3DGamer.Instance.PlayerBattleTeamData.Add(item);
					D3DGamer.Instance.PlayerTeamData.Add(item);
				}
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, OnBattleEnd, false);
			}
			else
			{
				LootShuffle.SkipShuff();
			}
		}
		else if (GetControl("ArenaPauseMusicBtn") == control)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			TAudioManager.instance.isMusicOn = !TAudioManager.instance.isMusicOn;
			((UIPushButton)GetControl("ArenaPauseMusicBtn")).Set(TAudioManager.instance.isMusicOn);
			D3DGamer.Instance.SaveGameOptions();
		}
		else if (GetControl("ArenaPauseSfxBtn") == control)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			TAudioManager.instance.isSoundOn = !TAudioManager.instance.isSoundOn;
			((UIPushButton)GetControl("ArenaPauseSfxBtn")).Set(TAudioManager.instance.isSoundOn);
			D3DGamer.Instance.SaveGameOptions();
		}
		else
		{
			if (FaceButtonEvent(control) || SkillUIEvent(control) || LootTreasures == null)
			{
				return;
			}
			foreach (D3DLootTreasure lootTreasure in LootTreasures)
			{
				if (lootTreasure.TreasureButton != control || command != 0)
				{
					continue;
				}
				int[] drawCost = D3DBattleRule.Instance.GetDrawCost(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id, D3DMain.Instance.exploring_dungeon.current_floor);
				if (D3DMain.Instance.LootEquipments.Count < 3 && (int.Parse(D3DGamer.Instance.CurrencyText) < drawCost[0] || int.Parse(D3DGamer.Instance.CrystalText) < drawCost[1]))
				{
					GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIInstantIap"));
					gameObject.GetComponent<UIInstantIap>().ui_arena = this;
					break;
				}
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.ITEM_LOOT), null, false, false);
				int index = 0;
				if (D3DMain.Instance.LootEquipments.Count > 2)
				{
					if (D3DMain.Instance.Lottery(D3DBattleRule.Instance.first_draw_odds[0]))
					{
						index = 2;
					}
					else if (D3DMain.Instance.Lottery(D3DBattleRule.Instance.first_draw_odds[1]))
					{
						index = 1;
					}
				}
				else if (D3DMain.Instance.LootEquipments.Count > 1 && D3DMain.Instance.Lottery(D3DBattleRule.Instance.second_draw_odd))
				{
					index = 1;
				}
				if (D3DMain.Instance.MutationSlot.Count > 0 && D3DMain.Instance.MutationSlot[index] != null)
				{
					lootTreasure.UpdateCrystal(BattleResultUI);
					D3DMain.Instance.MutationSlot.Clear();
				}
				else if (D3DMain.Instance.LootGoldBag[index] == null)
				{
					LootDraw.Add(D3DMain.Instance.LootEquipments[index]);
					lootTreasure.UpdateLootUIGear(D3DMain.Instance.LootEquipments[index]);
				}
				else
				{
					lootTreasure.UpdateGoldBag(D3DMain.Instance.LootEquipments[index].equipment_grade, D3DMain.Instance.LootGoldBag[index].value, BattleResultUI);
				}
				lootTreasure.TreasureButton.Enable = false;
				lootTreasure.TreasureAnimation.SetAnimation();
				lootTreasure.TreasureAnimation.Play();
				LootTreasures.Remove(lootTreasure);
				if (D3DMain.Instance.LootEquipments.Count < 3)
				{
					D3DGamer.Instance.UpdateCurrency(-drawCost[0]);
					D3DGamer.Instance.UpdateCrystal(-drawCost[1]);
					BattleResultUI.UpdateCurrency();
					D3DGamer.Instance.SaveAllData();
				}
				D3DMain.Instance.LootEquipments.RemoveAt(index);
				D3DMain.Instance.LootGoldBag.RemoveAt(index);
				if (D3DMain.Instance.MutationSlot.Count != 0)
				{
					D3DMain.Instance.MutationSlot.RemoveAt(index);
				}
				foreach (D3DLootTreasure lootTreasure2 in LootTreasures)
				{
					lootTreasure2.UpdateTreasureCost();
				}
				GetControl("ResultButton").Enable = true;
				GetControl("ResultButton").Visible = true;
				GetControl("ResultButtonText").Visible = true;
				break;
			}
		}
	}

	public void GamePause()
	{
		if (Time.timeScale == 1f && !m_UIManagerRef[2].gameObject.active)
		{
			((UIPushButton)GetControl("ArenaPauseMusicBtn")).Set(TAudioManager.instance.isMusicOn);
			((UIPushButton)GetControl("ArenaPauseSfxBtn")).Set(TAudioManager.instance.isSoundOn);
			m_UIManagerRef[2].gameObject.SetActiveRecursively(true);
			m_UIManagerRef[0].gameObject.SetActiveRecursively(false);
			m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
			battle_ui_event = false;
			Time.timeScale = 0.0001f;
			scene_arena.FreeAllTouch();
			//ChartBoostAndroid.showInterstitial(null);
		}
	}

	private void UpdateCurrencyUI()
	{
		PlayerCurrency.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		PlayerCurrency.SetPosition(new Vector2((float)GameScreen.width * (1f / (float)D3DMain.Instance.HD_SIZE) - PlayerCurrency.GetUIWidth() * (1f / (float)D3DMain.Instance.HD_SIZE), (float)GameScreen.height * (1f / (float)D3DMain.Instance.HD_SIZE) - 27f));
	}

	public void InstantIapUpdate()
	{
		BattleResultUI.UpdateCurrency();
		foreach (D3DLootTreasure lootTreasure in LootTreasures)
		{
			lootTreasure.UpdateTreasureCost();
		}
	}

	public void ShowTutorialNewHeroComin(D3DTutorialHintCfg.DamaoHintData.HintCondition condition, int nWave, float fDelay = 0f)
	{
		StartCoroutine(DoShowTutorialNewHeroComin(condition, nWave, fDelay));
	}

	private IEnumerator DoShowTutorialNewHeroComin(D3DTutorialHintCfg.DamaoHintData.HintCondition condition, int nWave, float fDelay)
	{
		yield return new WaitForSeconds(fDelay);
		_tutorialDamaoHints.Show(true, condition, nWave, OnDamaoFinished);
	}

	public void OnDamaoFinished(D3DTutorialHintCfg.DamaoHintData.HintCondition condition)
	{
		if (condition == D3DTutorialHintCfg.DamaoHintData.HintCondition.NewHeroComin)
		{
			scene_arena.CallCreatePlayerPawnsAsync();
		}
	}
}
