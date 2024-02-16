using System.Collections.Generic;
using UnityEngine;

public class UIStory : UIHelper
{
	public delegate void StoryEndCallBack();

	private List<D3DStoryManager.StoryPhase> PlayingStory;

	private int story_phase_index;

	private int phase_text_index;

	private UIClickButton NextButton;

	private UIImage StoryIllustration;

	private UIText StoryText;

	private UIImage NextArrow;

	private float ArrowRad;

	private int text_fade_state;

	private float last_story_real_time;

	private float current_story_real_time;

	private float delta_story_real_time;

	private StoryEndCallBack storyEndCallBack;

	private Color fade_color;

	private D3DAudioBehaviour story_bgm;

	private new void Awake()
	{
		base.name = "UIStory";
		base.Awake();
		AddImageCellIndexer(new string[2] { "UI_Monolayer_cell", "UImg1_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		StoryIllustration = new UIImage();
		StoryIllustration.Enable = false;
		StoryIllustration.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		m_UIManagerRef[0].Add(StoryIllustration);
		StoryText = new UIText();
		StoryText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 9), string.Empty, Color.black);
		StoryText.AlignStyle = UIText.enAlignStyle.center;
		StoryText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(9 * D3DMain.Instance.HD_SIZE);
		StoryText.LineSpacing = D3DMain.Instance.GameFont2.GetLineSpacing(9 * D3DMain.Instance.HD_SIZE);
		StoryText.Enable = false;
		StoryText.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		m_UIManagerRef[0].Add(StoryText);
		NextButton = new UIClickButton();
		NextButton.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		NextButton.Enable = false;
		m_UIManagerRef[0].Add(NextButton);
		NextArrow = new UIImage();
		D3DImageCell imageCell = GetImageCell("tishijiantou");
		NextArrow.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		NextArrow.Rect = D3DMain.Instance.ConvertRectAutoHD(435f, 5f, 40f, 30f);
		NextArrow.Enable = false;
		NextArrow.Visible = false;
		m_UIManagerRef[0].Add(NextArrow);
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = m_UIManagerRef[0].DEPTH;
		}
		UpdateStoryPhaseOnStart();
		last_story_real_time = Time.realtimeSinceStartup;
	}

	private new void Update()
	{
		base.Update();
		current_story_real_time = Time.realtimeSinceStartup;
		delta_story_real_time = current_story_real_time - last_story_real_time;
		last_story_real_time = current_story_real_time;
		switch (text_fade_state)
		{
		case -1:
			StoryText.SetAlpha(StoryText.GetAlpha() - delta_story_real_time * 1f);
			if (StoryText.GetAlpha() <= 0f)
			{
				text_fade_state = 0;
				SetStoryText();
				FadeInStoryText();
			}
			break;
		case 1:
			StoryText.SetAlpha(StoryText.GetAlpha() + delta_story_real_time * 1f);
			if (StoryText.GetAlpha() >= 1f)
			{
				text_fade_state = 0;
				NextButton.Enable = true;
				NextArrow.Visible = true;
			}
			break;
		}
		ArrowRad += delta_story_real_time * 10f;
		NextArrow.SetPosition(NextArrow.GetPosition() + Vector2.up * Mathf.Cos(ArrowRad) * 0.3f);
	}

	public void Init(string story_id, StoryEndCallBack call_back, bool black_fade = true)
	{
		D3DStoryManager.Story storyPreset = D3DStoryManager.Instance.GetStoryPreset(story_id);
		string audio_name = ((storyPreset.story_bgm == D3DStoryManager.Story.StoryBgm.START) ? "Music_Story_start" : ((storyPreset.story_bgm != D3DStoryManager.Story.StoryBgm.END) ? "MusicJingle_Entry&End" : "MusicJingle_FinalEnd"));
		D3DAudioManager.Instance.PlayAudio(audio_name, ref story_bgm, base.gameObject, true, false);
		PlayingStory = storyPreset.story_phase;
		storyEndCallBack = call_back;
		fade_color = ((!black_fade) ? new Color(44f / 51f, 44f / 51f, 44f / 51f) : Color.black);
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control != NextButton || command != 0)
		{
			return;
		}
		NextArrow.Visible = false;
		ArrowRad = 0f;
		phase_text_index++;
		if (phase_text_index > PlayingStory[story_phase_index].contents.Count - 1)
		{
			phase_text_index = 0;
			story_phase_index++;
			if (story_phase_index > PlayingStory.Count - 1)
			{
				if (null != story_bgm)
				{
					story_bgm.Stop();
				}
				EnableUIFade(UIFade.FadeState.FADE_OUT, fade_color, OnStoryPlayEnd, true);
			}
			else
			{
				EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, UpdateStoryPhase, true, false);
			}
		}
		else
		{
			StoryText.SetAlpha(1f);
			text_fade_state = -1;
		}
		NextButton.Enable = false;
	}

	private void FadeInStoryText()
	{
		StoryText.SetAlpha(0f);
		text_fade_state = 1;
	}

	private void UpdateStoryPhaseOnStart()
	{
		StoryIllustration.SetTexture((Material)Resources.Load("Dungeons3D/Images/Story/" + PlayingStory[story_phase_index].Illustration + ((D3DMain.Instance.HD_SIZE != 2) ? "_M" : "_hd_M")), D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f));
		StoryText.SetAlpha(0f);
		if (phase_text_index < PlayingStory[story_phase_index].contents.Count)
		{
			SetStoryText();
		}
		EnableUIFade(UIFade.FadeState.FADE_IN, fade_color, FadeInStoryText, true);
	}

	private void UpdateStoryPhase()
	{
		StoryIllustration.SetTexture((Material)Resources.Load("Dungeons3D/Images/Story/" + PlayingStory[story_phase_index].Illustration + ((D3DMain.Instance.HD_SIZE != 2) ? "_M" : "_hd_M")), D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f));
		StoryText.SetAlpha(0f);
		if (phase_text_index < PlayingStory[story_phase_index].contents.Count)
		{
			SetStoryText();
		}
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, FadeInStoryText, true, false);
	}

	private void SetStoryText()
	{
		string text = string.Empty;
		List<string> list = PlayingStory[story_phase_index].contents[phase_text_index];
		for (int i = 0; i < list.Count; i++)
		{
			text += list[i];
			if (i != list.Count - 1)
			{
				text += "\n";
			}
		}
		StoryText.SetText(text);
		float linesTotalHeight = StoryText.GetLinesTotalHeight();
		StoryText.Rect = new Rect(0f, (float)(80 * D3DMain.Instance.HD_SIZE) - linesTotalHeight * 0.5f, 480 * D3DMain.Instance.HD_SIZE, linesTotalHeight);
	}

	private void OnStoryPlayEnd()
	{
		if (storyEndCallBack != null)
		{
			storyEndCallBack();
		}
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		Object.Destroy(base.gameObject);
	}
}
