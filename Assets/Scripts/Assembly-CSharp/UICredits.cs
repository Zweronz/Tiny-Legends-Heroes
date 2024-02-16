using System.Collections;
using UnityEngine;

public class UICredits : UIHelper
{
	private UIImage[] name_list;

	private bool start_scroll;

	private new void Awake()
	{
		base.name = "UICredits";
		base.Awake();
		AddImageCellIndexer(new string[4] { "credits1_cell", "credits2_cell", "credits3_cell", "UI_Monolayer_cell" });
	}

	private new IEnumerator Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		CreateUIManager("Manager_NameList");
		m_UIManagerRef[1].SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 270f));
		UIImage bg = new UIImage();
		D3DImageCell image_cell3 = GetImageCell("ditu");
		bg.SetTexture(LoadUIMaterialAutoHD(image_cell3.cell_texture), D3DMain.Instance.ConvertRectAutoHD(image_cell3.cell_rect));
		bg.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		m_UIManagerRef[0].Add(bg);
		name_list = new UIImage[2];
		name_list[0] = new UIImage();
		name_list[0].Enable = false;
		image_cell3 = GetImageCell("mingdan1");
		name_list[0].SetTexture(LoadUIMaterialAutoHD(image_cell3.cell_texture), D3DMain.Instance.ConvertRectAutoHD(image_cell3.cell_rect), new Vector2(340f, 335f) * D3DMain.Instance.HD_SIZE);
		name_list[0].Rect = D3DMain.Instance.ConvertRectAutoHD(75f, -60f, 340f, 335f);
		m_UIManagerRef[1].Add(name_list[0]);
		name_list[1] = new UIImage();
		name_list[1].Enable = false;
		image_cell3 = GetImageCell("mingdan2");
		name_list[1].SetTexture(LoadUIMaterialAutoHD(image_cell3.cell_texture), D3DMain.Instance.ConvertRectAutoHD(image_cell3.cell_rect), new Vector2(340f, 335f) * D3DMain.Instance.HD_SIZE);
		name_list[1].Rect = D3DMain.Instance.ConvertRectAutoHD(75f, -394.5f, 340f, 335f);
		m_UIManagerRef[1].Add(name_list[1]);
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = m_UIManagerRef[0].DEPTH;
		}
		UIClickButton back_btn = new UIClickButton
		{
			Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f),
			Id = cur_control_id
		};
		cur_control_id++;
		m_UIManagerRef[1].Add(back_btn);
		m_control_table.Add("CreditsBackBtn", back_btn);
		EnableUIFade(UIFade.FadeState.FADE_IN, Color.black, null, true);
		yield return new WaitForSeconds(1.5f * Time.timeScale);
		start_scroll = true;
	}

	private new void Update()
	{
		base.Update();
		if (!start_scroll)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			name_list[i].SetPosition(name_list[i].GetPosition() + Vector2.up * 20f * Time.deltaTime / Time.timeScale * D3DMain.Instance.HD_SIZE);
			if (name_list[i].GetPosition().y >= 442.5f * (float)D3DMain.Instance.HD_SIZE)
			{
				name_list[i].SetPosition(new Vector2(245f, -270.5f) * D3DMain.Instance.HD_SIZE);
			}
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("CreditsBackBtn") == control.Id && command == 0)
		{
			EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, QuitCredits, false);
		}
	}

	private void QuitCredits()
	{
		UIHelper uIHelper = D3DMain.Instance.D3DUIList[ui_index - 2];
		uIHelper.ui_fade.StartFade(UIFade.FadeState.FADE_IN, null, true);
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		Object.Destroy(base.gameObject);
	}
}
