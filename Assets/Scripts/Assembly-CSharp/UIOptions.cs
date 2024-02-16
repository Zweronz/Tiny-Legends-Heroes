using UnityEngine;

public class UIOptions : UIHelper
{
	private new void Awake()
	{
		Time.timeScale = 0.001f;
		base.name = "UIOptions";
		base.Awake();
		AddImageCellIndexer(new string[4] { "UImg0_cell", "UImg1_cell", "UI_Monolayer_cell", "UImg2_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			CreateUIByCellXml("UIOptionsNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			CreateUIByCellXml("UIOptionsIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UIOptionsCfg", m_UIManagerRef[0]);
		}
		SetMusicBtnState();
		SetSfxBtnState();
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, null, true);
		if (ui_index > 1)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			uIHelper.HideFade();
		}
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
	}

	private new void Update()
	{
		base.Update();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("CloseBtn") == control.Id && command == 0)
		{
			Time.timeScale = 1f;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, CloseOption, false);
		}
		else if (GetControlId("OptionsMenuBtn") == control.Id && command == 0)
		{
			Time.timeScale = 1f;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			D3DMain.Instance.LoadingScene = 1;
			SwitchLevelImmediately();
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, CloseToMainmenu, false);
		}
		else if (GetControlId("OptionsMusicBtn") == control.Id)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			TAudioManager.instance.isMusicOn = !TAudioManager.instance.isMusicOn;
			SetMusicBtnState();
			D3DGamer.Instance.SaveGameOptions();
		}
		else if (GetControlId("OptionsSfxBtn") == control.Id)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			TAudioManager.instance.isSoundOn = !TAudioManager.instance.isSoundOn;
			SetSfxBtnState();
			D3DGamer.Instance.SaveGameOptions();
		}
		else if (GetControlId("OptionsSupportBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			Application.OpenURL("http://www.trinitigame.com/support?game=TLHE&version=1.4.2");
		}
		else if (GetControlId("OptionsHowToBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushHowTo, false);
		}
		else if (GetControlId("OptionsReviewBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
			{
				Application.OpenURL("amzn://apps/android?p=com.trinitigame.android.tinylegendsheroes");
			}
			else if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.GOOGLE_PLAY)
			{
				Application.OpenURL("market://details?id=com.trinitigame.android.tinylegendsheroes");
			}
		}
		else if (GetControlId("OptionsCreditsBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushCredits, false);
		}
	}

	private void SetMusicBtnState()
	{
		((UIPushButton)GetControl("OptionsMusicBtn")).Set(TAudioManager.instance.isMusicOn);
	}

	private void SetSfxBtnState()
	{
		((UIPushButton)GetControl("OptionsSfxBtn")).Set(TAudioManager.instance.isSoundOn);
	}

	private void CloseOption()
	{
		if (ui_index > 1)
		{
			UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
			uIHelper.GetManager(0).gameObject.SetActiveRecursively(true);
			uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, null, true);
		}
		Object.Destroy(base.gameObject);
	}

	private void CloseToMainmenu()
	{
		Object.Destroy(base.gameObject);
	}

	private void PushCredits()
	{
		Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UICredits"));
	}

	private void PushHowTo()
	{
		Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIHowTo"));
	}
}
