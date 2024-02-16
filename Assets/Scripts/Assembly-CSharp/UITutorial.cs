using UnityEngine;

public class UITutorial : UIHelper
{
	private enum TutorialPhase
	{
		DELAY = 0,
		PUSH = 1,
		STAND_BY = 2,
		SLIP = 3,
		POP = 4,
		STAY = 5
	}

	private D3DHowTo.TutorialType TutorialType;

	private D3DHowTo.Tutorial Tutorial;

	private int TutorialIllIndex;

	private UIImage TutorialBg;

	private UIClickButton NextButton;

	private UIImage[] TutorialImg;

	private UIImage NextArrow;

	private float ArrowRad;

	private float last_tutorial_real_time;

	private float current_tutorial_real_time;

	private float delta_tutorial_real_time;

	private float time_out;

	private TutorialPhase tutorial_phase;

	private new void Awake()
	{
		base.name = "UITutorial";
		base.Awake();
		AddImageCellIndexer(new string[2] { "UI_Monolayer_cell", "UImg1_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		TutorialBg = new UIImage();
		D3DImageCell imageCell = GetImageCell("ui_monolayer");
		TutorialBg.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
		TutorialBg.SetColor(new Color(0f, 0f, 0f, 0.8f));
		TutorialBg.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		TutorialBg.Visible = false;
		m_UIManagerRef[0].Add(TutorialBg);
		TutorialImg = new UIImage[2];
		TutorialImg[0] = new UIImage();
		TutorialImg[0].Enable = false;
		TutorialImg[0].Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		m_UIManagerRef[0].Add(TutorialImg[0]);
		TutorialImg[1] = new UIImage();
		TutorialImg[1].Enable = false;
		TutorialImg[1].Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		m_UIManagerRef[0].Add(TutorialImg[1]);
		NextButton = new UIClickButton();
		NextButton.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		NextButton.Enable = false;
		m_UIManagerRef[0].Add(NextButton);
		NextArrow = new UIImage();
		imageCell = GetImageCell("tishijiantou");
		NextArrow.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		NextArrow.Rect = D3DMain.Instance.ConvertRectAutoHD(438f, 2f, 40f, 30f);
		NextArrow.Enable = false;
		NextArrow.Visible = false;
		m_UIManagerRef[0].Add(NextArrow);
		TutorialImg[0].SetTexture(GetTutorialIll(Tutorial.TutorialIll[TutorialIllIndex]), D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f));
		TutorialImg[0].Visible = false;
		TutorialImg[0].SetScale(0f);
		TutorialImg[1].Visible = false;
		last_tutorial_real_time = Time.realtimeSinceStartup;
		tutorial_phase = TutorialPhase.DELAY;
	}

	private new void Update()
	{
		base.Update();
		current_tutorial_real_time = Time.realtimeSinceStartup;
		delta_tutorial_real_time = current_tutorial_real_time - last_tutorial_real_time;
		last_tutorial_real_time = current_tutorial_real_time;
		switch (tutorial_phase)
		{
		case TutorialPhase.DELAY:
			time_out += delta_tutorial_real_time;
			if (time_out >= 0f)
			{
				TutorialBg.Visible = true;
				TutorialImg[0].Visible = true;
				tutorial_phase = TutorialPhase.PUSH;
				if ((TutorialType == D3DHowTo.TutorialType.FIRST_IN_CAMP || TutorialType == D3DHowTo.TutorialType.FIRST_ENTER_DUNGEON || TutorialType == D3DHowTo.TutorialType.FIRST_TELEPORT || TutorialType == D3DHowTo.TutorialType.BEGINNER_BATTLE || TutorialType == D3DHowTo.TutorialType.FIRST_LOOT || TutorialType == D3DHowTo.TutorialType.FIRST_BOSS_GRAVE) && null != D3DMain.Instance.HD_BOARD_OBJ)
				{
					D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
					D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = m_UIManagerRef[0].DEPTH;
				}
			}
			break;
		case TutorialPhase.PUSH:
			TutorialImg[0].SetScale(TutorialImg[0].GetScale(0) + 3f * delta_tutorial_real_time);
			if (TutorialImg[0].GetScale(0) >= 1f)
			{
				TutorialImg[0].SetScale(1f);
				tutorial_phase = TutorialPhase.STAY;
				time_out = 0f;
			}
			break;
		case TutorialPhase.SLIP:
		{
			for (int i = 0; i < 2; i++)
			{
				TutorialImg[i].SetPosition(TutorialImg[i].GetPosition() - Vector2.right * 800f * delta_tutorial_real_time * D3DMain.Instance.HD_SIZE);
			}
			if (TutorialImg[1].GetPosition().x <= (float)(240 * D3DMain.Instance.HD_SIZE))
			{
				TutorialImg[0].SetTexture(TutorialImg[1].GetTexture());
				TutorialImg[1].Visible = false;
				TutorialImg[0].SetPosition(new Vector2(240f, 160f) * D3DMain.Instance.HD_SIZE);
				tutorial_phase = TutorialPhase.STAY;
			}
			break;
		}
		case TutorialPhase.POP:
			TutorialImg[0].SetScale(TutorialImg[0].GetScale(0) - 3f * delta_tutorial_real_time);
			if (!(TutorialImg[0].GetScale(0) <= 0f))
			{
				break;
			}
			if (TutorialType == D3DHowTo.TutorialType.FIRST_IN_CAMP || TutorialType == D3DHowTo.TutorialType.FIRST_ENTER_DUNGEON || TutorialType == D3DHowTo.TutorialType.FIRST_TELEPORT || TutorialType == D3DHowTo.TutorialType.BEGINNER_BATTLE || TutorialType == D3DHowTo.TutorialType.FIRST_LOOT || TutorialType == D3DHowTo.TutorialType.FIRST_BOSS_GRAVE)
			{
				if (null != D3DMain.Instance.HD_BOARD_OBJ)
				{
					D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
					D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
				}
				if (TutorialType != D3DHowTo.TutorialType.FIRST_TELEPORT && TutorialType != D3DHowTo.TutorialType.FIRST_LOOT)
				{
					Time.timeScale = 1f;
				}
			}
			if (TutorialType != 0 && TutorialType != D3DHowTo.TutorialType.FIRST_LOOT)
			{
				D3DGamer.Instance.TutorialState[(int)TutorialType] = true;
				D3DGamer.Instance.SaveTutorialState();
			}
			Object.Destroy(base.gameObject);
			break;
		case TutorialPhase.STAY:
			time_out += delta_tutorial_real_time;
			if (time_out >= Tutorial.ill_stay)
			{
				NextArrow.Visible = true;
				NextButton.Enable = true;
				tutorial_phase = TutorialPhase.STAND_BY;
				time_out = 0f;
			}
			break;
		}
		ArrowRad += delta_tutorial_real_time * 10f;
		NextArrow.SetPosition(NextArrow.GetPosition() + Vector2.up * Mathf.Cos(ArrowRad) * 0.3f);
	}

	public void Init(D3DHowTo.TutorialType type)
	{
		TutorialType = type;
		Tutorial = D3DHowTo.Instance.GetTutorial(TutorialType);
		TutorialIllIndex = 0;
		if (TutorialType == D3DHowTo.TutorialType.FIRST_IN_CAMP || TutorialType == D3DHowTo.TutorialType.FIRST_ENTER_DUNGEON || TutorialType == D3DHowTo.TutorialType.FIRST_TELEPORT || TutorialType == D3DHowTo.TutorialType.BEGINNER_BATTLE)
		{
			Time.timeScale = 0.0001f;
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == NextButton && command == 0)
		{
			if (TutorialIllIndex == Tutorial.TutorialIll.Count - 1)
			{
				tutorial_phase = TutorialPhase.POP;
			}
			else
			{
				TutorialIllIndex++;
				TutorialImg[1].SetTexture(GetTutorialIll(Tutorial.TutorialIll[TutorialIllIndex]), D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f));
				TutorialImg[1].Visible = true;
				TutorialImg[1].Rect = D3DMain.Instance.ConvertRectAutoHD(480f, 0f, 480f, 320f);
				tutorial_phase = TutorialPhase.SLIP;
				D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.SCROLL_SLIP), null, false, false);
			}
			NextArrow.Visible = false;
			ArrowRad = 0f;
			NextButton.Enable = false;
		}
	}

	private Material GetTutorialIll(string ill)
	{
		return (Material)Resources.Load("Dungeons3D/Images/HowTo/" + ill + ((D3DMain.Instance.HD_SIZE != 2) ? "_M" : "_hd_M"));
	}
}
