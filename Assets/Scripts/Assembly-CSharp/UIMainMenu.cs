using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIHelper
{
	private new void Awake()
	{
		base.name = "UIMainMenu";
		base.Awake();
		AddImageCellIndexer(new string[1] { "UImg6_cell" });
		TAudioManager.instance.AudioListener.transform.position = Vector3.zero;
		TAudioManager.instance.AudioListener.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
	}

	public new void Start()
	{
		XAdManagerWrapper.SetVideoAdUrl("https://itunes.apple.com/us/app/tinylegends-monster-crasher/id605683834?ls=1&mt=8");
		XAdManagerWrapper.ShowVideoAdLocal();
		D3DGamer.Instance.UserCome(D3DGamer.EUserFisrtCome.InMainmenu);
		base.Start();
		CreateUIManager("Manager_Main");
		CreateUIManager("Manager_MainBtns");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		m_UIManagerRef[1].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[1].ScreenOffset.x, 0f - m_UIManagerRef[1].ScreenOffset.y, GameScreen.width, GameScreen.height));
		UIImage uIImage = new UIImage();
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			uIImage.SetTexture((Material)Resources.Load("Dungeons3D/Images/UIImages/mainmenu_ipad_M"), new Rect(0f, 0f, 1024f, 768f));
			uIImage.Rect = new Rect(0f, 0f, 1024f, 768f);
			CreateUIByCellXml("UIMainMenuNewPadCfg", m_UIManagerRef[1]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			uIImage.SetTexture((Material)Resources.Load("Dungeons3D/Images/UIImages/mainmenu_iphone5_M"), new Rect(0f, 0f, 1024f, 640f), new Vector2(1136f, 640f));
			uIImage.Rect = new Rect(0f, 0f, 1136f, 640f);
			CreateUIByCellXml("UIMainMenuIphone5Cfg", m_UIManagerRef[1]);
		}
		else if (D3DMain.Instance.HD_SIZE == 2)
		{
			uIImage.SetTexture((Material)Resources.Load("Dungeons3D/Images/UIImages/mainmenu_hd_M"), new Rect(0f, 0f, 960f, 640f));
			uIImage.Rect = new Rect(0f, 0f, 960f, 640f);
			CreateUIByCellXml("UIMainMenuCfg", m_UIManagerRef[1]);
		}
		else
		{
			uIImage.SetTexture((Material)Resources.Load("Dungeons3D/Images/UIImages/mainmenu_M"), new Rect(0f, 0f, 480f, 320f));
			uIImage.Rect = new Rect(0f, 0f, 480f, 320f);
			CreateUIByCellXml("UIMainMenuCfg", m_UIManagerRef[1]);
		}
		m_UIManagerRef[0].Add(uIImage);
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, CheckClaim, true);
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
		if (GetControlId("OptionBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			D3DMain.Instance.LoadingScene = 6;
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, OpenOption, false);
		}
		else
		{
			if (GetControlId("PlayBtn") != control.Id || command != 0)
			{
				return;
			}
			OpenClikPlugin.Show(true);
			D3DGamer.Instance.UserCome(D3DGamer.EUserFisrtCome.InLoading);
			if (null != D3DAudioManager.Instance.ThemeAudio)
			{
				D3DAudioManager.Instance.ThemeAudio.Stop();
				D3DAudioManager.Instance.ThemeAudio = null;
			}
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			if (D3DMain.Instance.exploring_dungeon != null)
			{
				D3DMain.Instance.exploring_dungeon.Reset();
			}
			D3DMain.Instance.exploring_dungeon.dungeon = D3DMain.Instance.GetDungeon("Dungeons001");
			if (!D3DGamer.Instance.TutorialState[0])
			{
				D3DMain.Instance.LoadingScene = -1;
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, base.SwitchLevelByLoading, false);
				return;
			}
			D3DMain.Instance.LoadingScene = 3;
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, base.SwitchLevelByLoading, false);
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
		}
	}

	private void CheckClaim()
	{
		if ("94266FCA48" == D3DGamer.Instance.Claim)
		{
			List<D3DMessageBoxButtonEvent.OnButtonClick> list = new List<D3DMessageBoxButtonEvent.OnButtonClick>();
			list.Add(PushIapLoot);
			PushMessageBox(D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_GET_IAP_ITEM_GAME_QUIT_EXCEPTION), D3DMessageBox.MgbButton.OK, list, true);
		}
	}

	private void PushIapLoot()
	{
		D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("xinshoulibaoxianglian001"));
		D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("xinshoulibaojiezhi001"));
		D3DMain.Instance.LootEquipments.Add(D3DMain.Instance.GetEquipmentClone("xinshoulibaojiezhi001"));
		D3DMain.Instance.LoadingScene = 8;
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	private void OpenOption()
	{
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
		PushLevel();
	}

	private IEnumerator ReloadUI()
	{
		yield return 0;
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			Object.DestroyImmediate(D3DMain.Instance.HD_BOARD_OBJ);
			D3DMain.Instance.HD_BOARD_OBJ = null;
		}
		UIHDBoard.DEVICE = UIHDBoard.GetHDDeviceType();
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS && null == D3DMain.Instance.HD_BOARD_OBJ)
		{
			GameObject hd_board = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIHDBoard") as GameObject;
			D3DMain.Instance.HD_BOARD_OBJ = (GameObject)Object.Instantiate(hd_board);
			Object.DontDestroyOnLoad(D3DMain.Instance.HD_BOARD_OBJ);
		}
		Application.LoadLevel(1);
	}
}
