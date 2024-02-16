using System.Collections.Generic;
using UnityEngine;

public class TutorialDamaoHints : SubUIBase
{
	public delegate void OnUIFinished(D3DTutorialHintCfg.DamaoHintData.HintCondition condition);

	private readonly float _fStillTime = 0.5f;

	private OnUIFinished _onUIFinshed;

	private UIImage _damaoImg = new UIImage();

	private UIImage _bg = new UIImage();

	private List<UIText> _texts = new List<UIText>();

	private D3DTutorialHintCfg.DamaoHintData _damaoData;

	private int _nStringIndex;

	private bool _bShow;

	private float _fLastProcessTime;

	public bool IsShown
	{
		get
		{
			return _bShow;
		}
	}

	public void Show(bool bShow, D3DTutorialHintCfg.DamaoHintData.HintCondition condition, int nWave, OnUIFinished onUIFinshed = null)
	{
		_damaoData = D3DTutorialHintCfg.NeedsToShowDamao(condition, nWave);
		if (_damaoData != null)
		{
			Time.timeScale = 0.0001f;
			_nStringIndex = 0;
			CreateDamaoImg(_damaoData._damaoType);
			RefreshUIText();
			_onUIFinshed = onUIFinshed;
			DoShowContents(true);
			_bShow = true;
			_fLastProcessTime = Time.realtimeSinceStartup;
		}
	}

	private void Hide()
	{
		_bShow = false;
		if (_onUIFinshed != null)
		{
			_onUIFinshed(_damaoData._condition);
		}
		DoShowContents(false);
		Time.timeScale = 1f;
	}

	private void DoShowContents(bool bShow)
	{
		_damaoImg.Visible = bShow;
		_bg.Visible = bShow;
		foreach (UIText text in _texts)
		{
			text.Visible = bShow;
		}
	}

	private void RefreshUIText()
	{
		string text = _damaoData._nHintStrings[_nStringIndex];
		_texts[0].SetText(text);
	}

	private void CreateDamaoImg(int nDamaoType)
	{
		D3DImageCell imageCell = _ownerUI.GetImageCell("npc0" + nDamaoType);
		_damaoImg.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
	}

	public void Create(int nUIIndex, UIHelper owner)
	{
		Time.timeScale = 0.0001f;
		_ownerUI = owner;
		_uiManager = _ownerUI.GetManager(nUIIndex);
		D3DImageCell imageCell = _ownerUI.GetImageCell("npc04");
		_bg.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		_bg.Rect = D3DMain.Instance.ConvertRectAutoHD(40f, 0f, 409f, 62f);
		_bg.Enable = false;
		_uiManager.Add(_bg);
		UIText uIText = new UIText();
		uIText.Enable = false;
		uIText.Set(_ownerUI.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 8), string.Empty, D3DMain.Instance.CommonFontColor);
		uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(8 * D3DMain.Instance.HD_SIZE);
		uIText.Rect = D3DMain.Instance.ConvertRectAutoHD(167f, 16f, 260f, 30f);
		_uiManager.Add(uIText);
		_texts.Add(uIText);
		imageCell = _ownerUI.GetImageCell("npc01");
		_damaoImg.SetTexture(_ownerUI.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		_damaoImg.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 162f, 157f);
		_damaoImg.Enable = false;
		_uiManager.Add(_damaoImg);
		Hide();
	}

	public void OnScreenTouched()
	{
		if (Time.realtimeSinceStartup - _fLastProcessTime > _fStillTime)
		{
			if (_nStringIndex++ < _damaoData._nHintStrings.Count - 1)
			{
				RefreshUIText();
				_fLastProcessTime = Time.realtimeSinceStartup;
			}
			else
			{
				Hide();
				_fLastProcessTime = Time.realtimeSinceStartup;
			}
		}
	}
}
