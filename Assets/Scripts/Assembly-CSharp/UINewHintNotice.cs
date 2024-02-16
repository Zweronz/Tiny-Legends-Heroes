using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINewHintNotice : UIHelper
{
	private UIManagerOpenClose hint_open_close;

	private bool opened = true;

	private UIImage[] NewSkillBoards;

	private UIImage[] NewSkillIcons;

	private new void Awake()
	{
		base.name = "UINewHintNotice";
		base.Awake();
		AddImageCellIndexer(new string[4] { "UImg0_cell", "UImg2_cell", "UImg9_cell", "UI_Monolayer_cell" });
		AddSkillIcons();
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			CreateUIByCellXml("UINewHintNoticeNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			CreateUIByCellXml("UINewHintNoticeIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UINewHintNoticeCfg", m_UIManagerRef[0]);
		}
		D3DImageCell imageCell = GetImageCell("beibaokuang");
		NewSkillBoards = new UIImage[10];
		NewSkillIcons = new UIImage[10];
		for (int i = 0; i < 10; i++)
		{
			NewSkillBoards[i] = new UIImage();
			NewSkillBoards[i].SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			m_UIManagerRef[0].Add(NewSkillBoards[i]);
			NewSkillIcons[i] = new UIImage();
			m_UIManagerRef[0].Add(NewSkillIcons[i]);
		}
		UpdateNewSkillUI();
		hint_open_close = m_UIManagerRef[0].transform.Find("UIMesh").gameObject.AddComponent<UIManagerOpenClose>();
		hint_open_close.Init(m_UIManagerRef[0].GetCameraTransformRect(), new Rect(0f, 0f, 98f, 140f), OnBoardClose);
		hint_open_close.enabled = false;
		if (!opened)
		{
			hint_open_close.Open();
		}
		Time.timeScale = 0.0001f;
	}

	private new void Update()
	{
		base.Update();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (!hint_open_close.enabled && GetControlId("HintCloseBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			hint_open_close.Close();
		}
	}

	private void UpdateNewSkillUI()
	{
		for (int i = 0; i < 10; i++)
		{
			NewSkillBoards[i].Visible = false;
			NewSkillIcons[i].Visible = false;
		}
		using (Dictionary<string, List<string>>.KeyCollection.Enumerator enumerator = D3DGamer.Instance.CurrentUnlockedSkills.Keys.GetEnumerator())
		{
			if (!enumerator.MoveNext())
			{
				return;
			}
			string current = enumerator.Current;
			D3DPuppetProfile profile = D3DMain.Instance.GetProfile(current);
			((UIText)GetControl("NewSkillPuppetNameTxt")).SetText("HERO: " + profile.profile_name);
			((UIText)GetControl("NewSkillPuppetClassTxt")).SetText("CLASS: " + D3DMain.Instance.GetClass(profile.profile_class).class_name);
			float num = 0f;
			if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
			{
				num = 50f;
			}
			int num2 = Mathf.Min(5, D3DGamer.Instance.CurrentUnlockedSkills[current].Count);
			float num3 = 44 * D3DMain.Instance.HD_SIZE;
			float num4 = 16 * D3DMain.Instance.HD_SIZE;
			float num5 = ((float)GameScreen.width - (float)num2 * num3 - (float)(num2 - 1) * num4) * 0.5f;
			float top = (float)(140 * D3DMain.Instance.HD_SIZE) + num;
			float num6 = 36 * D3DMain.Instance.HD_SIZE;
			for (int j = 0; j < num2; j++)
			{
				NewSkillBoards[j].Rect = new Rect(num5 + (float)(60 * D3DMain.Instance.HD_SIZE * j), top, num3, num3);
				NewSkillBoards[j].Visible = true;
				D3DActiveSkill activeSkill = D3DMain.Instance.GetActiveSkill(D3DGamer.Instance.CurrentUnlockedSkills[current][j]);
				D3DImageCell iconCell;
				if (activeSkill != null)
				{
					iconCell = GetIconCell(activeSkill.skill_icon);
				}
				else
				{
					D3DPassiveSkill passiveSkill = D3DMain.Instance.GetPassiveSkill(D3DGamer.Instance.CurrentUnlockedSkills[current][j]);
					iconCell = GetIconCell(passiveSkill.skill_icon);
				}
				NewSkillIcons[j].SetTexture(LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect), new Vector2(num6, num6));
				NewSkillIcons[j].Rect = new Rect(NewSkillBoards[j].Rect.x + (float)(3 * D3DMain.Instance.HD_SIZE), NewSkillBoards[j].Rect.y + (float)(4 * D3DMain.Instance.HD_SIZE), num6, num6);
				NewSkillIcons[j].Visible = true;
			}
			int num7 = Mathf.Min(5, D3DGamer.Instance.CurrentUnlockedSkills[current].Count - num2);
			num5 = ((float)GameScreen.width - (float)num7 * num3 - (float)(num7 - 1) * num4) * 0.5f;
			float top2 = (float)(100 * D3DMain.Instance.HD_SIZE) + num;
			for (int k = 5; k < num7 + 5; k++)
			{
				NewSkillBoards[k].Rect = new Rect(num5 + (float)(60 * D3DMain.Instance.HD_SIZE * (k - 5)), top2, num3, num3);
				NewSkillBoards[k].Visible = true;
				D3DActiveSkill activeSkill2 = D3DMain.Instance.GetActiveSkill(D3DGamer.Instance.CurrentUnlockedSkills[current][k]);
				D3DImageCell iconCell;
				if (activeSkill2 != null)
				{
					iconCell = GetIconCell(activeSkill2.skill_icon);
				}
				else
				{
					D3DPassiveSkill passiveSkill2 = D3DMain.Instance.GetPassiveSkill(D3DGamer.Instance.CurrentUnlockedSkills[current][k]);
					iconCell = GetIconCell(passiveSkill2.skill_icon);
				}
				NewSkillIcons[k].SetTexture(LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect), new Vector2(num6, num6));
				NewSkillIcons[k].Rect = new Rect(NewSkillBoards[k].Rect.x + (float)(3 * D3DMain.Instance.HD_SIZE), NewSkillBoards[k].Rect.y + (float)(4 * D3DMain.Instance.HD_SIZE), num6, num6);
				NewSkillIcons[k].Visible = true;
			}
			D3DGamer.Instance.CurrentUnlockedSkills.Remove(current);
		}
	}

	public void CloseBoard()
	{
		m_UIManagerRef[1].gameObject.SetActiveRecursively(false);
	}

	private void OnBoardClose()
	{
		if (D3DGamer.Instance.CurrentUnlockedSkills.Keys.Count > 0)
		{
			StartCoroutine(PopNextHint());
			return;
		}
		Time.timeScale = 1f;
		Object.Destroy(base.gameObject);
	}

	private IEnumerator PopNextHint()
	{
		yield return new WaitForSeconds(0.25f * Time.timeScale);
		UpdateNewSkillUI();
		hint_open_close.Default();
	}

	public void Init(bool opened)
	{
		this.opened = opened;
	}
}
