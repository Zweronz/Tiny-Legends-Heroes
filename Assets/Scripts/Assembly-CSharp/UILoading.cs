using System;
using System.Collections;
using UnityEngine;

public class UILoading : UIHelper
{
	private UIImage dot_mask;

	private float dot_animation;

	private new void Awake()
	{
		base.name = "UILoading";
		base.Awake();
		AddImageCellIndexer(new string[2] { "UImg3_cell", "UI_Monolayer_cell" });
	}

	private new IEnumerator Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			CreateUIByCellXml("UILoadingNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			CreateUIByCellXml("UILoadingIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UILoadingCfg", m_UIManagerRef[0]);
		}
		dot_mask = (UIImage)GetControl("DotMaskImg");
		dot_mask.SetRotation((float)Math.PI);
		yield return new WaitForSeconds(1f);
		if (D3DMain.Instance.LoadingScene == -1)
		{
			GameObject story_obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIStory"));
			story_obj.GetComponent<UIStory>().Init(D3DHowTo.Instance.NewGameStory, LoadBeginnerBattle, false);
		}
		else
		{
			D3DMain.Instance.CurrentScene = D3DMain.Instance.LoadingScene;
			Application.LoadLevelAsync(D3DMain.Instance.LoadingScene);
		}
	}

	private new void Update()
	{
		dot_animation += Time.deltaTime * D3DMain.Instance.RealTimeScale * 9f;
		int num = (int)dot_animation % 4;
		Rect rect = dot_mask.Rect;
		dot_mask.SetClip(new Rect(rect.x, rect.y, rect.width - (float)(6 * num * D3DMain.Instance.HD_SIZE), rect.height));
	}

	private void LoadBeginnerBattle()
	{
		UIImage uIImage = new UIImage();
		D3DImageCell imageCell = GetImageCell("ui_monolayer");
		uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(GameScreen.width, GameScreen.height));
		uIImage.SetColor(new Color(44f / 51f, 44f / 51f, 44f / 51f));
		uIImage.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		m_UIManagerRef[0].Add(uIImage);
		D3DMain.Instance.LoadingScene = 2;
		D3DMain.Instance.exploring_dungeon.current_floor = 1;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data = new EnemyGroupBattleData();
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group = D3DMain.Instance.GetEnemyGroup("xinshouzhiyinduilie001");
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id = 0;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.group_level = 1;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.enemy_power = 0;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.kill_require = D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.kill_require;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.battle_spawn_interval = D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.battle_spawn_interval;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.battle_spawn_limit = D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.battle_spawn_limit;
		D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawn_phases = D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.spawn_phases;
		Time.timeScale = 0.0001f;
		SwitchLevelImmediately();
	}
}
