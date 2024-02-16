using UnityEngine;

public class UIWorldMap : UIHelper
{
	public class WorldDungeon
	{
		public string dungeon_id;

		public Vector2 world_position;

		public WorldDungeon(string id, Vector2 position)
		{
			dungeon_id = id;
			world_position = position;
		}
	}

	private WorldDungeon[] world_dungeons = new WorldDungeon[1]
	{
		new WorldDungeon("Dungeons001", new Vector2(30f, 250f))
	};

	private new void Awake()
	{
		base.name = "UIWorldMap";
		base.Awake();
	}

	public new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_ui_cfgxml = "Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UICfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UIWorldMapCfg", D3DGamer.Instance.Sk[0]));
		CreateUIByXml(m_UIManagerRef[0]);
		for (int i = 0; i < world_dungeons.Length; i++)
		{
			UIClickButton uIClickButton = new UIClickButton();
			uIClickButton.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD("tempbtn"), new Rect(0f, 0f, 32 * D3DMain.Instance.HD_SIZE, 16 * D3DMain.Instance.HD_SIZE), new Vector2(50 * D3DMain.Instance.HD_SIZE, 30 * D3DMain.Instance.HD_SIZE));
			uIClickButton.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD("tempbtn"), new Rect(0f, 16 * D3DMain.Instance.HD_SIZE, 32 * D3DMain.Instance.HD_SIZE, 16 * D3DMain.Instance.HD_SIZE), new Vector2(50 * D3DMain.Instance.HD_SIZE, 30 * D3DMain.Instance.HD_SIZE));
			uIClickButton.Rect = new Rect(world_dungeons[i].world_position.x * (float)D3DMain.Instance.HD_SIZE, world_dungeons[i].world_position.y * (float)D3DMain.Instance.HD_SIZE, 60 * D3DMain.Instance.HD_SIZE, 30 * D3DMain.Instance.HD_SIZE);
			m_control_table.Add("world_btn" + i, uIClickButton);
			m_UIManagerRef[0].Add(uIClickButton);
			UIText uIText = new UIText();
			uIText.AlignStyle = UIText.enAlignStyle.center;
			uIText.Set(LoadFontAutoHD("arial", 12), D3DMain.Instance.GetDungeon(world_dungeons[i].dungeon_id).dungeon_name, Color.black);
			uIText.Rect = uIClickButton.Rect;
			uIText.AutoTextRect();
			m_UIManagerRef[0].Add(uIText);
			uIClickButton.Rect = uIText.Rect;
		}
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, null, true);
		if (null == D3DAudioManager.Instance.ThemeAudio)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.THEME), ref D3DAudioManager.Instance.ThemeAudio, TAudioManager.instance.AudioListener.gameObject, true, false, false);
		}
	}

	public new void Update()
	{
		base.Update();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (!DungeonButtonEvent(control, command))
		{
			if (GetControlId("MenuBtn") == control.Id && command == 0)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				D3DMain.Instance.LoadingScene = 1;
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, base.SwitchLevelImmediately, false);
			}
			else if (GetControlId("StashBtn") == control.Id && command == 0)
			{
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				D3DMain.Instance.LoadingScene = 4;
				UIStash.EnabledOptions = new UIStash.StashOption[4]
				{
					UIStash.StashOption.GEARS,
					UIStash.StashOption.SKILLS,
					UIStash.StashOption.ITEMS,
					UIStash.StashOption.tBANK
				};
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
			}
		}
	}

	private bool DungeonButtonEvent(UIControl control, int command)
	{
		for (int i = 0; i < world_dungeons.Length; i++)
		{
			if (GetControl("world_btn" + i) == control && command == 0)
			{
				if (null != D3DAudioManager.Instance.ThemeAudio)
				{
					D3DAudioManager.Instance.ThemeAudio.Stop();
					D3DAudioManager.Instance.ThemeAudio = null;
				}
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
				D3DMain.Instance.LoadingScene = 3;
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, base.SwitchLevelByLoading, false);
				D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.WORLD_MAP;
				D3DMain.Instance.exploring_dungeon.current_floor = 0;
				D3DMain.Instance.exploring_dungeon.dungeon = D3DMain.Instance.GetDungeon(world_dungeons[i].dungeon_id);
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
				return true;
			}
		}
		return false;
	}
}
