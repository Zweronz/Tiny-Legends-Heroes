using System;
using System.Collections.Generic;
using UnityEngine;

public class SubUIDugeonStash
{
	private class SubIconButton
	{
		private readonly int _nUIManagerIndex;

		private static readonly string _strNamePrefix = "popButton";

		private EIconType _type;

		private string _strName;

		private UIClickButton _button;

		private UIImage _imgBg;

		private MoveAttached _moveComponent;

		private UIImage _imgNewHint;

		private static readonly float fXOffset = 32f;

		private static readonly float fYOffset = -4f;

		private static readonly float fXOffsetEach = 58f;

		private static readonly float _fXHidX = -60f;

		public static string NamePrefix
		{
			get
			{
				return _strNamePrefix;
			}
		}

		public EIconType Type
		{
			get
			{
				return _type;
			}
		}

		public string Name
		{
			get
			{
				return _strName;
			}
		}

		public UIClickButton Button
		{
			get
			{
				return _button;
			}
		}

		public UIImage ImgNewHint
		{
			get
			{
				return _imgNewHint;
			}
		}

		public void Create(UIHelper uiParent, EIconType type)
		{
			_type = type;
			_strName = _strNamePrefix + type;
			createButton(uiParent, type);
			attachMoveComponent();
		}

		public void Tick()
		{
			_moveComponent.Tick();
		}

		public bool IsMoving()
		{
			return _moveComponent.IsMoving;
		}

		public void PlayAnimation(bool bShow, float fTime)
		{
			_moveComponent.StartMove(fTime, (!bShow) ? new Vector2(_fXHidX, fYOffset) : new Vector2(fXOffset + fXOffsetEach * (float)Type, fYOffset));
			if (bShow)
			{
				Show(true);
			}
		}

		public void ShowAnimBg(bool bShow)
		{
			_imgNewHint.Enable = false;
			_imgNewHint.Visible = bShow;
		}

		private void createButton(UIHelper uiParent, EIconType type)
		{
			_imgBg = new UIImage();
			D3DImageCell imageCell = uiParent.GetImageCell(_strName);
			_imgBg.SetTexture(uiParent.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			_imgBg.Rect = D3DMain.Instance.ConvertRectAutoHD(_fXHidX, fYOffset, 60f, 60f);
			uiParent.GetManager(_nUIManagerIndex).Add(_imgBg);
			_button = new UIClickButton();
			imageCell = uiParent.GetImageCell("popButtonframe");
			_button.SetTexture(UIButtonBase.State.Normal, uiParent.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			imageCell = uiParent.GetImageCell("popButtonframe2");
			_button.SetTexture(UIButtonBase.State.Pressed, uiParent.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			_button.Rect = D3DMain.Instance.ConvertRectAutoHD(_fXHidX, fYOffset, 60f, 60f);
			_button.Id = uiParent.Cur_control_id++;
			uiParent.AddControlToTable(_strName, _button);
			uiParent.GetManager(_nUIManagerIndex).Add(_button);
			_imgNewHint = new UIImage();
			imageCell = uiParent.GetImageCell("anniukuangFlash");
			_imgNewHint.SetTexture(uiParent.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			_imgNewHint.Rect = D3DMain.Instance.ConvertRectAutoHD(_fXHidX, fYOffset, 60f, 60f);
			uiParent.GetManager(_nUIManagerIndex).Add(_imgNewHint);
			_imgNewHint.Enable = false;
			_imgNewHint.Visible = false;
			Show(false);
		}

		public void Show(bool bShow)
		{
			_button.Visible = bShow;
			_button.Enable = bShow;
			_imgBg.Visible = bShow;
		}

		private void attachMoveComponent()
		{
			_moveComponent = new MoveAttached(_button, OnMoveStop, OnMove);
		}

		private void OnMoveStop()
		{
			_imgBg.Rect = _button.Rect;
			_imgNewHint.Rect = _button.Rect;
			if (_imgBg.Rect.x == _fXHidX * (float)D3DMain.Instance.HD_SIZE)
			{
				Show(false);
			}
		}

		private void OnMove(Rect rect)
		{
			_imgBg.Rect = _button.Rect;
			_imgNewHint.Rect = _button.Rect;
		}
	}

	public enum EIconType
	{
		Gear = 0,
		Team = 1,
		Shop = 2,
		Skill = 3,
		Max = 4
	}

	private List<SubIconButton> _iconButtons = new List<SubIconButton>();

	private UIClickButton _rootButtonShow = new UIClickButton();

	private UIClickButton _rootButtonHide = new UIClickButton();

	private UIImage _allButtonsBg = new UIImage();

	private UIImage _imgNewHint = new UIImage();

	private UIHelper _parentUI;

	private UIDungeon _uiDungeon;

	private NewHintBehaviour _newHintBhv;

	private UIImage _arrowPointer;

	private UIImage _arrowPointerToSkill;

	private bool IsASubIconHasNewHint()
	{
		foreach (SubIconButton iconButton in _iconButtons)
		{
			if (iconButton.ImgNewHint.Visible)
			{
				return true;
			}
		}
		return false;
	}

	public void ShowImgBgNewHint(EIconType type, bool bShow)
	{
		List<bool> tutorialState = D3DGamer.Instance.TutorialState;
		_iconButtons[(int)type].ShowAnimBg(bShow);
		if (type == EIconType.Skill && _arrowPointer != null && !D3DGamer.Instance.TutorialState[11])
		{
			_arrowPointer.Visible = bShow;
		}
		if (bShow)
		{
			_imgNewHint.Visible = true;
			_newHintBhv.AddHintImage(_iconButtons[(int)type].ImgNewHint);
			_newHintBhv.AddHintImage(_imgNewHint);
			return;
		}
		_newHintBhv.RemoveHintImage(_iconButtons[(int)type].ImgNewHint);
		if (!IsASubIconHasNewHint())
		{
			_newHintBhv.RemoveHintImage(_imgNewHint);
			_imgNewHint.Visible = false;
		}
	}

	public void ShowIcons(bool bShow, float fTime = 0.5f)
	{
		PlayAnimation(bShow, fTime);
		_rootButtonHide.Enable = bShow;
		_rootButtonHide.Visible = bShow;
		_rootButtonShow.Enable = !bShow;
		_rootButtonShow.Visible = !bShow;
		if (bShow && _arrowPointer != null && _arrowPointer.Visible && !D3DGamer.Instance.TutorialState[11])
		{
			D3DGamer.Instance.TutorialState[11] = true;
			D3DGamer.Instance.SaveTutorialState();
		}
		if (_arrowPointer != null && _newHintBhv.HintImages.Contains(_iconButtons[3].ImgNewHint))
		{
			_arrowPointer.Visible = false;
			if (!D3DGamer.Instance.TutorialState[12])
			{
				_arrowPointerToSkill.Visible = bShow;
			}
		}
	}

	public void Initialize(UIHelper uiParent)
	{
		_parentUI = uiParent;
		_uiDungeon = uiParent as UIDungeon;
		GameObject gameObject = new GameObject();
		_newHintBhv = gameObject.AddComponent<NewHintBehaviour>();
		Create();
	}

	private void Create()
	{
		D3DImageCell imageCell = _parentUI.GetImageCell("tanchukuang-di");
		_allButtonsBg.SetTexture(_parentUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		_allButtonsBg.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(0f, 8f, 246f, 38f));
		_allButtonsBg.Enable = false;
		_allButtonsBg.Visible = false;
		_parentUI.GetManager(0).Add(_allButtonsBg);
		createAllSubButtons();
		createRootButton(_rootButtonShow, "SubUIRootButtonShow", "slipout", "slipout2", true);
		createRootButton(_rootButtonHide, "SubUIRootButtonHide", "slipin", "slipin2", false);
		CreateFlashBg();
		if (!D3DGamer.Instance.TutorialState[11])
		{
			D3DImageCell imageCell2 = _parentUI.GetImageCell("12");
			_arrowPointer = new UIImage();
			_arrowPointer.SetTexture(_parentUI.LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect));
			_arrowPointer.Enable = false;
			_arrowPointer.SetRotation(3.926991f);
			_arrowPointer.Visible = false;
			_parentUI.GetManager(0).Add(_arrowPointer);
			_arrowPointerToSkill = new UIImage();
			_arrowPointerToSkill.SetTexture(_parentUI.LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect));
			_arrowPointerToSkill.Enable = false;
			_arrowPointerToSkill.SetRotation(-(float)Math.PI / 2f);
			_parentUI.GetManager(0).Add(_arrowPointerToSkill);
			_arrowPointerToSkill.Visible = false;
		}
	}

	private void CreateFlashBg()
	{
		D3DImageCell imageCell = _parentUI.GetImageCell("rootanniukuangFlash");
		_imgNewHint.SetTexture(_parentUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		_imgNewHint.Rect = D3DMain.Instance.ConvertRectAutoHD(-5.5f, -7f, 49f, 70f);
		_parentUI.GetManager(0).Add(_imgNewHint);
		_imgNewHint.Enable = false;
		_imgNewHint.Visible = false;
	}

	private void createRootButton(UIClickButton btnToCreate, string strControlName, string strTexDefault, string strTexClick, bool bShowDefault)
	{
		D3DImageCell imageCell = _parentUI.GetImageCell(strTexDefault);
		btnToCreate.SetTexture(UIButtonBase.State.Normal, _parentUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		imageCell = _parentUI.GetImageCell(strTexClick);
		btnToCreate.SetTexture(UIButtonBase.State.Pressed, _parentUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		btnToCreate.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 2f, 35f, 50f);
		btnToCreate.Id = _parentUI.Cur_control_id++;
		_parentUI.AddControlToTable(strControlName, btnToCreate);
		_parentUI.GetManager(0).Add(btnToCreate);
		btnToCreate.Enable = bShowDefault;
		btnToCreate.Visible = bShowDefault;
	}

	private void createAllSubButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			SubIconButton subIconButton = new SubIconButton();
			subIconButton.Create(_parentUI, (EIconType)i);
			_iconButtons.Add(subIconButton);
		}
	}

	public void Tick()
	{
		foreach (SubIconButton iconButton in _iconButtons)
		{
			iconButton.Tick();
		}
		if (_arrowPointer != null)
		{
			float left = 40f + Mathf.Sin(Time.realtimeSinceStartup * 6f) * 5f;
			float top = 53f + Mathf.Sin(Time.realtimeSinceStartup * 6f) * 5f;
			_arrowPointer.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(left, top, 52f, 37f));
		}
		if (_arrowPointerToSkill != null)
		{
			float left2 = 210f;
			float top2 = 70f + Mathf.Sin(Time.realtimeSinceStartup * 6f) * 5f;
			_arrowPointerToSkill.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(left2, top2, 52f, 37f));
		}
	}

	private void PlayAnimation(bool bShow, float fTime)
	{
		foreach (SubIconButton iconButton in _iconButtons)
		{
			iconButton.PlayAnimation(bShow, fTime);
		}
		_allButtonsBg.Visible = bShow;
	}

	public bool OnHandleEventFromParent(UIControl control, int command, float wparam, float lparam)
	{
		if (command == 0)
		{
			if (!_iconButtons[0].IsMoving())
			{
				if (_parentUI.GetControlId("SubUIRootButtonShow") == control.Id)
				{
					ShowIcons(true);
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
					return true;
				}
				if (_parentUI.GetControlId("SubUIRootButtonHide") == control.Id)
				{
					D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
					ShowIcons(false);
					return true;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				string name = SubIconButton.NamePrefix + (EIconType)i;
				if (_parentUI.GetControlId(name) == control.Id)
				{
					Open((EIconType)i);
				}
			}
		}
		return false;
	}

	private void Open(EIconType type)
	{
		switch (type)
		{
		case EIconType.Gear:
			_uiDungeon.OpenStash();
			break;
		case EIconType.Team:
			_uiDungeon.OpenTavern();
			break;
		case EIconType.Shop:
			_uiDungeon.OpenShop();
			break;
		case EIconType.Skill:
			if (!D3DGamer.Instance.TutorialState[12])
			{
				D3DGamer.Instance.TutorialState[12] = true;
				D3DGamer.Instance.SaveTutorialState();
				_arrowPointerToSkill.Visible = false;
			}
			_uiDungeon.OpenSkillSchool();
			break;
		}
	}
}
