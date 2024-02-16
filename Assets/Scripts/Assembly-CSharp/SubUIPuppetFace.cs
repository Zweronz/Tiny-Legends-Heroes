using System.Collections.Generic;
using UnityEngine;

public class SubUIPuppetFace : SubUIBase
{
	public delegate void OnSelectAnotherPuppetFace(int nIndex);

	private OnSelectAnotherPuppetFace _onSelectAnotherPuppetFace;

	private List<PuppetBasic> PlayerTeamFacePuppets;

	private D3DFeatureCameraButton[] PuppetFaceBtn;

	private int _nCurrentFaceIndex;

	private UIImage[] FeatureMask;

	private UIImage[] EquipCompareUp;

	private UIImage[] EquipCompareDown;

	private UIManager _uiMaskManager;

	public PuppetBasic CurrentPuppet
	{
		get
		{
			return PlayerTeamFacePuppets[CurrentFaceIndex];
		}
	}

	public int CurrentFaceIndex
	{
		get
		{
			return _nCurrentFaceIndex;
		}
	}

	public void CreatePuppetFaceUI(UIHelper owner, int nMainIndex, int nMaskIndex, OnSelectAnotherPuppetFace onSelectAnotherPuppetFace)
	{
		_ownerUI = owner;
		_onSelectAnotherPuppetFace = onSelectAnotherPuppetFace;
		_uiManager = _ownerUI.GetManager(nMainIndex);
		_uiMaskManager = _ownerUI.InsertUIManager("Manager_Mask2", nMaskIndex);
		_uiMaskManager.EnableUIHandler = false;
		CreateFacePuppet();
		CreateFaceUI();
	}

	public bool PuppetFaceEvent(UIControl control)
	{
		int num = 0;
		D3DFeatureCameraButton[] puppetFaceBtn = PuppetFaceBtn;
		foreach (D3DFeatureCameraButton d3DFeatureCameraButton in puppetFaceBtn)
		{
			if (control == d3DFeatureCameraButton.PushButton)
			{
				if (num > PlayerTeamFacePuppets.Count - 1)
				{
					d3DFeatureCameraButton.Set(false);
					return true;
				}
				ShowCompareAnimImg(false, false, num);
				if (num == _nCurrentFaceIndex)
				{
					d3DFeatureCameraButton.Set(true);
				}
				else
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					PuppetFaceBtn[_nCurrentFaceIndex].Set(false);
					d3DFeatureCameraButton.Set(true);
					_nCurrentFaceIndex = num;
					_onSelectAnotherPuppetFace(num);
				}
				return true;
			}
			num++;
		}
		return false;
	}

	public void UpdateFaceFrame()
	{
		bool flag = D3DGamer.Instance.ExpBonus == 0.2f && 0.1f == D3DGamer.Instance.GoldBonus;
		for (int i = 0; i < 3; i++)
		{
			PuppetFaceBtn[i].SetFrame("touxiangkuang" + ((!flag) ? string.Empty : "2"), new Vector2(58f, 58f));
			D3DImageCell imageCell = _ownerUI.GetImageCell("touxiangkuang" + ((!flag) ? "-1" : "2-1"));
			if (i < FeatureMask.Length)
			{
				FeatureMask[i].SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			}
		}
	}

	private void CreateFaceUI()
	{
		D3DBigBoardUI d3DBigBoardUI = new D3DBigBoardUI(_uiMaskManager, _ownerUI);
		d3DBigBoardUI.CreateBigBoard(new Vector2(-5f, -5f));
		bool flag = D3DGamer.Instance.ExpBonus == 0.2f && 0.1f == D3DGamer.Instance.GoldBonus;
		PuppetFaceBtn = new D3DFeatureCameraButton[3];
		for (int i = 0; i < 3; i++)
		{
			PuppetFaceBtn[i] = new D3DFeatureCameraButton(_uiManager, _ownerUI);
			GameObject gameObject = null;
			if (i < PlayerTeamFacePuppets.Count)
			{
				gameObject = PlayerTeamFacePuppets[i].gameObject;
			}
			PuppetFaceBtn[i].CreateControl(new Vector2(422f, 230.5f - (float)(i * 58)), "touxiangkuang" + ((!flag) ? string.Empty : "2"), new Vector2(58f, 58f), gameObject, new Vector2(0f, 8f), new Vector2(50f, 50f), new string[3]
			{
				string.Empty,
				"dangqianbiaoqian",
				string.Empty
			}, new Rect(-2f, -2f, 60f, 60f));
			if (null != gameObject)
			{
				D3DPuppetTransformCfg transformCfg = PlayerTeamFacePuppets[i].model_builder.TransformCfg;
				PuppetFaceBtn[i].SetCameraFeatureTransform(transformCfg.face_camera_cfg.offset, transformCfg.face_camera_cfg.rotation, transformCfg.face_camera_cfg.size);
			}
		}
		PuppetFaceBtn[_nCurrentFaceIndex].Set(true);
		FeatureMask = new UIImage[PlayerTeamFacePuppets.Count];
		for (int j = 0; j < FeatureMask.Length; j++)
		{
			FeatureMask[j] = new UIImage();
			D3DImageCell imageCell = _ownerUI.GetImageCell("touxiangkuang" + ((!flag) ? "-1" : "2-1"));
			FeatureMask[j].SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			FeatureMask[j].Enable = false;
			FeatureMask[j].Rect = D3DMain.Instance.ConvertRectAutoHD(422f, 230.5f - (float)(j * 58), 58f, 14f);
			_uiMaskManager.Add(FeatureMask[j]);
		}
		CreateCompareAnimImg(ref EquipCompareUp, "zengjia");
		CreateCompareAnimImg(ref EquipCompareDown, "jianshao");
	}

	public void HideAll()
	{
		for (int i = 0; i < EquipCompareUp.Length; i++)
		{
			EquipCompareUp[i].Visible = false;
			EquipCompareDown[i].Visible = false;
		}
	}

	public void ShowCompareAnimImg(bool bShow, bool bUp, int nFaceIndex)
	{
		if (bShow)
		{
			if (bUp)
			{
				EquipCompareUp[nFaceIndex].Visible = true;
				EquipCompareDown[nFaceIndex].Visible = false;
			}
			else
			{
				EquipCompareDown[nFaceIndex].Visible = true;
				EquipCompareUp[nFaceIndex].Visible = false;
			}
		}
		else
		{
			EquipCompareUp[nFaceIndex].Visible = false;
			EquipCompareDown[nFaceIndex].Visible = false;
		}
	}

	private void CreateCompareAnimImg(ref UIImage[] Img, string strTexName)
	{
		Img = new UIImage[PlayerTeamFacePuppets.Count];
		for (int i = 0; i < FeatureMask.Length; i++)
		{
			Img[i] = new UIImage();
			D3DImageCell imageCell = _ownerUI.GetImageCell(strTexName);
			Img[i].SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			Img[i].Enable = false;
			Img[i].Rect = D3DMain.Instance.ConvertRectAutoHD(460f, 235.5f - (float)(i * 58), 11f, 15f);
			_uiMaskManager.Add(Img[i]);
			Img[i].Visible = false;
		}
	}

	public void Tick()
	{
		float num = Mathf.Sin(Time.realtimeSinceStartup * 5f);
		for (int i = 0; i < FeatureMask.Length; i++)
		{
			Rect rect = EquipCompareDown[i].Rect;
			rect.y = (240.5f - (float)(i * 58) + num * 2.5f) * (float)D3DMain.Instance.HD_SIZE;
			EquipCompareDown[i].Rect = rect;
			EquipCompareUp[i].Rect = rect;
		}
	}

	private void CreateFacePuppet()
	{
		PlayerTeamFacePuppets = new List<PuppetBasic>();
		int num = 1;
		foreach (D3DGamer.D3DPuppetSaveData playerBattleTeamDatum in D3DGamer.Instance.PlayerBattleTeamData)
		{
			GameObject gameObject = new GameObject("FacePuppet" + num);
			gameObject.transform.parent = _ownerUI.transform;
			PuppetBasic puppetBasic = gameObject.AddComponent<PuppetBasic>();
			if (!puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(playerBattleTeamDatum.pupet_profile_id), playerBattleTeamDatum))
			{
				Object.Destroy(gameObject);
				continue;
			}
			puppetBasic.model_builder.BuildPuppetFaceFeatureModel();
			puppetBasic.model_builder.PlayFeatureAnimation();
			gameObject.transform.localPosition = new Vector3(1000f, 0f, num * 100);
			gameObject.transform.rotation = Quaternion.identity;
			puppetBasic.model_builder.SetAllClipSpeed(D3DMain.Instance.RealTimeScale);
			D3DMain.Instance.SetGameObjectGeneralLayer(puppetBasic.gameObject, 16);
			PlayerTeamFacePuppets.Add(puppetBasic);
			num++;
		}
		_nCurrentFaceIndex = 0;
	}
}
