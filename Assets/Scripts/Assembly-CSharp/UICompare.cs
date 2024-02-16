using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICompare : UIHelper
{
	private D3DUICompareGearDescription SelectedGear;

	private D3DUICompareGearDescription EquipedGear;

	private bool scroll_selected;

	private bool scroll_equiped;

	private Rect[] _FeatureArea = new Rect[2]
	{
		new Rect(25f, 158f, 240f, 160f),
		new Rect(220f, 158f, 240f, 160f)
	};

	private readonly int _nMaskUIIndex = 3;

	private readonly int _nConpareCount = 2;

	private List<PuppetBasic> _comparePuppets = new List<PuppetBasic>();

	private List<D3DFeatureCameraUI> _compareCameras = new List<D3DFeatureCameraUI>();

	public IEnumerator UpdateCompareGearsInfo(D3DEquipment selected_gear, D3DEquipment equiped_gear, D3DProfileInstance profile_instance, int nCurFaceIndex)
	{
		yield return 0;
		if (selected_gear != null)
		{
			SelectedGear.UpdateDescriptionInfo(selected_gear, false, profile_instance, true);
		}
		else
		{
			SelectedGear.NoDescription();
		}
		if (equiped_gear != null)
		{
			EquipedGear.UpdateDescriptionInfo(equiped_gear, false, profile_instance, true);
		}
		else
		{
			EquipedGear.NoDescription();
		}
		CreateStashPuppet(nCurFaceIndex);
		ArmComparePuppet(selected_gear, _comparePuppets[0], profile_instance);
	}

	private new void Awake()
	{
		base.name = "UICompare";
		base.Awake();
		AddImageCellIndexer(new string[4] { "UImg1_cell", "UImg3_cell", "UImg10_cell", "UI_Monolayer_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		CreateUIManager("Manager_SelectedGear");
		CreateUIManager("Manager_EquipedGear");
		CreateUIManager("Manager_Mask");
		CreateUIByCellXml("UICompareCfg", m_UIManagerRef[0]);
		SelectedGear = new D3DUICompareGearDescription(m_UIManagerRef[1], this, new Rect(57f, 61f, 169.5f, 120f), new UIControl[2]
		{
			GetControl("SelectedScrollSubUpImg"),
			GetControl("SelectedScrollSubBottomImg")
		});
		SelectedGear.CreateScrollBar(false, true);
		SelectedGear.InitScrollBar();
		EquipedGear = new D3DUICompareGearDescription(m_UIManagerRef[2], this, new Rect(257f, 61f, 169.5f, 120f), new UIControl[2]
		{
			GetControl("EquipedScrollSubUpImg"),
			GetControl("EquipedScrollSubBottomImg")
		});
		EquipedGear.CreateScrollBar(false, true);
		EquipedGear.InitScrollBar();
	}

	private new void Update()
	{
		base.Update();
	}

	private void CreateFeatureCamera(PuppetBasic owner, int nIndex)
	{
		D3DFeatureCameraUI d3DFeatureCameraUI = new D3DFeatureCameraUI(GetManager(_nMaskUIIndex), this);
		Rect rect = _FeatureArea[nIndex];
		d3DFeatureCameraUI.CreateControl(new Vector2(rect.x, rect.y), string.Empty, Vector2.zero, null, Vector2.zero, new Vector2(rect.width, rect.height));
		d3DFeatureCameraUI.SetCameraFeatureObject(owner.gameObject);
		D3DPuppetTransformCfg transformCfg = owner.model_builder.TransformCfg;
		d3DFeatureCameraUI.SetCameraFeatureTransform(transformCfg.stash_camera_cfg.offset + new Vector3(0f, -0.4f, 0f), transformCfg.stash_camera_cfg.rotation, transformCfg.stash_camera_cfg.size);
		d3DFeatureCameraUI.Visible(true);
		_compareCameras.Add(d3DFeatureCameraUI);
	}

	private void CreateStashPuppet(int nCurFaceIndex)
	{
		D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = D3DGamer.Instance.PlayerBattleTeamData[nCurFaceIndex];
		for (int i = 0; i < _nConpareCount; i++)
		{
			GameObject gameObject = new GameObject("StashPuppet" + i);
			gameObject.transform.parent = base.transform;
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(d3DPuppetSaveData.pupet_profile_id), d3DPuppetSaveData))
			{
				Object.Destroy(gameObject);
				continue;
			}
			puppetBasic.profile_instance.InitSkillLevel(d3DPuppetSaveData);
			puppetBasic.profile_instance.InitSkillSlots(d3DPuppetSaveData);
			puppetBasic.model_builder.BuildPuppetModel();
			puppetBasic.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true, 0.1f, Random.Range(0f, 2f));
			puppetBasic.CheckPuppetWeapons();
			gameObject.transform.localPosition = new Vector3(600 * D3DMain.Instance.HD_SIZE, 0f, i * 100);
			gameObject.transform.rotation = Quaternion.identity;
			puppetBasic.model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
			D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic.gameObject, 16);
			_comparePuppets.Add(puppetBasic);
			CreateFeatureCamera(puppetBasic, i);
		}
	}

	private void ArmComparePuppet(D3DEquipment equipToArm, PuppetBasic puppetToArm, D3DProfileInstance profile)
	{
		if (equipToArm.equipment_class == D3DEquipment.EquipmentClass.PLATE || equipToArm.equipment_class == D3DEquipment.EquipmentClass.LEATHER || equipToArm.equipment_class == D3DEquipment.EquipmentClass.ROBE)
		{
			puppetToArm.ChangeArms(equipToArm.GetDefaultArm(), equipToArm);
		}
		else
		{
			if (equipToArm.equipment_class >= D3DEquipment.EquipmentClass.PLATE)
			{
				return;
			}
			if (equipToArm.IsEquipmentUseable(profile))
			{
				puppetToArm.ChangeArms(equipToArm.GetDefaultArm(), equipToArm);
				if (equipToArm.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
				{
					puppetToArm.RemoveArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, false);
				}
			}
			else
			{
				puppetToArm.RemoveArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, false);
				puppetToArm.RemoveArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, false);
			}
		}
	}

	private bool IsTouchInPuppetCamera(Vector2 pos)
	{
		for (int i = 0; i < _FeatureArea.Length; i++)
		{
			if (D3DMain.Instance.ConvertRectAutoHD(_FeatureArea[i]).Contains(pos))
			{
				return true;
			}
		}
		return false;
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (command == 2)
		{
			Vector2 currentPosition = ((UIMove)control).GetCurrentPosition();
			if (IsTouchInPuppetCamera(currentPosition))
			{
				foreach (D3DFeatureCameraUI compareCamera in _compareCameras)
				{
					compareCamera.ViewFeatureObj(Vector3.up, wparam);
				}
			}
		}
		if (GetControlId("CompareCloseBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			Object.Destroy(base.gameObject);
		}
		else
		{
			if (GetControlId("CompareMove") != control.Id)
			{
				return;
			}
			switch (command)
			{
			case 0:
			{
				Vector2 currentPosition2 = ((UIMove)control).GetCurrentPosition();
				if (GetControl("CompareGearSelectedImg").PtInRect(currentPosition2))
				{
					scroll_selected = true;
					SelectedGear.StopInertia();
				}
				else if (GetControl("CompareGearEquipedImg").PtInRect(currentPosition2))
				{
					scroll_equiped = true;
					EquipedGear.StopInertia();
				}
				break;
			}
			case 2:
				if (scroll_selected)
				{
					SelectedGear.Scroll(Vector2.up * lparam);
				}
				else if (scroll_equiped)
				{
					EquipedGear.Scroll(Vector2.up * lparam);
				}
				break;
			case 4:
				if (scroll_selected)
				{
					SelectedGear.ScrollInertia(Vector2.up * lparam);
				}
				else if (scroll_equiped)
				{
					EquipedGear.ScrollInertia(Vector2.up * lparam);
				}
				scroll_selected = false;
				scroll_equiped = false;
				break;
			}
		}
	}
}
