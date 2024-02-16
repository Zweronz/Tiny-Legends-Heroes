using UnityEngine;

public class D3DTavernTeamSlipUI : D3DPageSlipUI
{
	public delegate void UpdateCurrentPageCampPuppet();

	public delegate void UpdateNextPageCampPuppet();

	public delegate void EditCampPuppetToTeam(int index);

	public delegate void EditBattlePuppetToCamp(int index);

	public D3DFeatureCameraSlipUI[] current_page_camp_puppet;

	public D3DFeatureCameraSlipUI[] next_page_camp_puppet;

	public UIText[] current_page_camp_puppet_tag;

	public D3DFeatureCameraUI[] battle_puppet;

	private bool left_updated;

	private bool right_updated;

	private bool battle_puppet_click;

	private float[] camp_camera_limit;

	private UpdateCurrentPageCampPuppet updateCurrentPageCampPuppet;

	private UpdateNextPageCampPuppet updateNextPageCampPuppet;

	private EditCampPuppetToTeam editCampPuppetToTeam;

	private EditBattlePuppetToCamp editBattlePuppetToCamp;

	public D3DTavernTeamSlipUI(UIManager manager, UIHelper helper, int page_count, Rect page_rect, float page_dot_y)
		: base(manager, helper, page_count, page_rect, page_dot_y)
	{
		current_page_camp_puppet = new D3DFeatureCameraSlipUI[3];
		next_page_camp_puppet = new D3DFeatureCameraSlipUI[3];
		battle_puppet = new D3DFeatureCameraUI[3];
		current_page_camp_puppet_tag = new UIText[3];
		for (int i = 0; i < 3; i++)
		{
			current_page_camp_puppet[i] = new D3DFeatureCameraSlipUI(ui_manager, ui_helper);
			next_page_camp_puppet[i] = new D3DFeatureCameraSlipUI(ui_manager, ui_helper);
			battle_puppet[i] = new D3DFeatureCameraUI(ui_manager, ui_helper);
			current_page_camp_puppet_tag[i] = new UIText();
			current_page_camp_puppet_tag[i].Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), string.Empty, new Color(0.9372549f, 73f / 85f, 0.76862746f));
			current_page_camp_puppet_tag[i].AlignStyle = UIText.enAlignStyle.center;
			current_page_camp_puppet_tag[i].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
			ui_manager.Add(current_page_camp_puppet_tag[i]);
		}
		camp_camera_limit = new float[2]
		{
			ui_manager.GetManagerCamera().pixelRect.xMin + (float)(10 * D3DMain.Instance.HD_SIZE),
			ui_manager.GetManagerCamera().pixelRect.xMin + (float)(470 * D3DMain.Instance.HD_SIZE)
		};
	}

	protected override void OnBeginMove()
	{
		UIText[] array = current_page_camp_puppet_tag;
		foreach (UIText uIText in array)
		{
			uIText.Visible = false;
		}
	}

	protected override void PageUIUpdate()
	{
		if (current_page_index != next_page_index)
		{
			float slip_x = page_slip_x - page_center_x;
			for (int i = 0; i < current_page_camp_puppet.Length; i++)
			{
				current_page_camp_puppet[i].Slip(slip_x, camp_camera_limit[0], camp_camera_limit[1]);
			}
			if (next_page_index < current_page_index && !left_updated)
			{
				updateNextPageCampPuppet();
				left_updated = true;
				right_updated = false;
			}
			else if (next_page_index > current_page_index && !right_updated)
			{
				updateNextPageCampPuppet();
				right_updated = true;
				left_updated = false;
			}
			for (int j = 0; j < next_page_camp_puppet.Length; j++)
			{
				next_page_camp_puppet[j].Slip(slip_x, camp_camera_limit[0], camp_camera_limit[1]);
			}
		}
	}

	protected override void OnPageAutoSlipOver()
	{
		base.OnPageAutoSlipOver();
		updateCurrentPageCampPuppet();
		right_updated = false;
		left_updated = false;
		for (int i = 0; i < next_page_camp_puppet.Length; i++)
		{
			next_page_camp_puppet[i].Visible(false);
		}
	}

	protected override void OnClickEvent(Vector2 click_position)
	{
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.TAVERN_CHARACTER_OPERATE), null, false, false);
		Vector2 vector = new Vector2(GameScreen.width - Utils.StandardScreenSize[0], GameScreen.height - Utils.StandardScreenSize[1]) * 0.5f;
		Vector2[] array = new Vector2[3]
		{
			new Vector2(67f, 280f),
			new Vector2(332f, 280f),
			new Vector2(567f, 280f)
		};
		Vector2 vector2 = new Vector2(168f, 228f);
		for (int i = 0; i < current_page_camp_puppet.Length; i++)
		{
			if (current_page_camp_puppet[i].IsVisible())
			{
				Vector2 vector3 = (current_page_camp_puppet[i].CameraRect.center - vector) * (1f / (float)D3DMain.Instance.HD_SIZE);
				float num = (float)Screen.height / 640f;
				if (D3DPlaneGeometry.PtInRect(new Rect(array[i].x, array[i].y, vector2.x, vector2.y), click_position))
				{
					editCampPuppetToTeam(i);
					break;
				}
			}
		}
	}

	public void BattlePuppetCameraClick(UIMove.Command move_command, Vector2 touch_position)
	{
		switch (move_command)
		{
		case UIMove.Command.Down:
			if (!enable_page_auto_slip)
			{
				battle_puppet_click = true;
			}
			else
			{
				battle_puppet_click = false;
			}
			break;
		case UIMove.Command.Begin:
			battle_puppet_click = false;
			break;
		case UIMove.Command.End:
		{
			if (!battle_puppet_click)
			{
				break;
			}
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.TAVERN_CHARACTER_OPERATE), null, false, false);
			Vector2 vector = new Vector2(GameScreen.width - Utils.StandardScreenSize[0], GameScreen.height - Utils.StandardScreenSize[1]) * 0.5f;
			Vector2[] array = new Vector2[3]
			{
				new Vector2(204f, 30f),
				new Vector2(358f, 30f),
				new Vector2(502f, 30f)
			};
			Vector2 vector2 = new Vector2(124f, 136f);
			for (int i = 0; i < battle_puppet.Length; i++)
			{
				if (battle_puppet[i].IsVisible())
				{
					float num = (float)Screen.height / 640f;
					if (D3DPlaneGeometry.PtInRect(new Rect(array[i].x, array[i].y, vector2.x, vector2.y), touch_position))
					{
						editBattlePuppetToCamp(i);
						break;
					}
				}
			}
			battle_puppet_click = false;
			break;
		}
		case UIMove.Command.Move:
		case UIMove.Command.Hold:
			break;
		}
	}

	public void ClearCurrentEmptyPage()
	{
		if (page_dots.Count != 0)
		{
			float num;
			if (current_page_index == 0)
			{
				next_page_index = 0;
				current_page_index = -1;
				updateNextPageCampPuppet();
				page_slip_x = page_center_x + page_size.x * 0.5f;
				page_auto_slip_target = page_center_x - page_size.x;
				num = page_auto_slip_target - page_slip_x;
				page_auto_slip_target_index = current_page_index + 1;
			}
			else
			{
				next_page_index = current_page_index - 1;
				updateNextPageCampPuppet();
				page_slip_x = page_center_x - page_size.x * 0.5f;
				page_auto_slip_target = page_center_x + page_size.x;
				num = page_auto_slip_target - page_slip_x;
				page_auto_slip_target_index = current_page_index - 1;
			}
			page_auto_slip_delta = num / 0.3f;
			enable_page_auto_slip = true;
			page_slip_behaviour.StartRealTime();
		}
	}

	public void SetUpdateCurrentPageCampPuppetDelegate(UpdateCurrentPageCampPuppet updateCurrentPageCampPuppet)
	{
		this.updateCurrentPageCampPuppet = updateCurrentPageCampPuppet;
	}

	public void SetUpdateNextPageCampPuppetDelegate(UpdateNextPageCampPuppet updateNextPageCampPuppet)
	{
		this.updateNextPageCampPuppet = updateNextPageCampPuppet;
	}

	public void SetEditCampPuppetToTeam(EditCampPuppetToTeam editCampPuppetToTeam)
	{
		this.editCampPuppetToTeam = editCampPuppetToTeam;
	}

	public void SetEditBattlePuppetToCamp(EditBattlePuppetToCamp editBattlePuppetToCamp)
	{
		this.editBattlePuppetToCamp = editBattlePuppetToCamp;
	}
}
