using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRespawnNow : UIHelper
{
	private RespawnTimeCounter _respawn;

	private UIText _txtGoldCost;

	private UIText _txtCrystalCost;

	private UIImage _imgIconGold;

	private UIImage _imgIconCrystal;

	private int _nCurGoldCost;

	private int _nCurCrystalCost;

	public RespawnTimeCounter Respawn
	{
		get
		{
			return _respawn;
		}
		set
		{
			_respawn = value;
		}
	}

	private new void Awake()
	{
		base.name = "UIRespawnNow";
		base.Awake();
		AddImageCellIndexer(new string[5] { "UImg0_cell", "UImg1_cell", "UImg5_cell", "UI_Monolayer_cell", "UImg2_cell" });
	}

	private new void Start()
	{
		base.Start();
		CreateUIManager("Manager_Main");
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			CreateUIByCellXml("UIRespawnNowNewPadCfg", m_UIManagerRef[0]);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			CreateUIByCellXml("UIRespawnNowIphone5Cfg", m_UIManagerRef[0]);
		}
		else
		{
			CreateUIByCellXml("UIRespawnNowCfg", m_UIManagerRef[0]);
		}
		CreateDynamicUI();
		Time.timeScale = 0.0001f;
		float tolRespawnTime = _respawn.GetTolRespawnTime();
		int refreshCount = D3DRespawnRule.Instance.RefreshCount;
		float fTimeInterval = tolRespawnTime / (float)refreshCount * Time.timeScale;
		StartCoroutine(IntervalUpdateCost(fTimeInterval));
	}

	private void CreateDynamicUI()
	{
		Vector4 vector = default(Vector4);
		Vector4 vector2 = default(Vector4);
		Vector4 vector3 = default(Vector4);
		Vector4 vector4 = default(Vector4);
		Vector4 vector5 = default(Vector4);
		if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			vector = new Vector4(145f, 141f, 115f, 20f);
			vector2 = new Vector4(312f, 141f, 115f, 20f);
			vector3 = new Vector4(233f, 143f, 20f, 20f);
			vector4 = new Vector4(400f, 141f, 15f, 25f);
			vector5 = new Vector4(160f, 262f, 340f, 20f);
		}
		else if (UIHDBoard.DEVICE == UIHDBoard.HD_DEVICE.IPHONE5)
		{
			vector = new Vector4(145f, 101f, 115f, 20f);
			vector2 = new Vector4(312f, 101f, 115f, 20f);
			vector3 = new Vector4(233f, 103f, 20f, 20f);
			vector4 = new Vector4(400f, 101f, 15f, 25f);
			vector5 = new Vector4(160f, 222f, 340f, 20f);
		}
		else
		{
			vector = new Vector4(101f, 101f, 115f, 20f);
			vector2 = new Vector4(268f, 101f, 115f, 20f);
			vector3 = new Vector4(189f, 103f, 20f, 20f);
			vector4 = new Vector4(356f, 101f, 15f, 25f);
			vector5 = new Vector4(116f, 222f, 340f, 20f);
		}
		List<string> msgBoxContent = D3DTexts.Instance.GetMsgBoxContent(D3DTexts.MsgBoxState.ON_RESPAWNNOW);
		if (msgBoxContent.Count == 0)
		{
			msgBoxContent.Add("Are you sure to ???");
		}
		foreach (string item in msgBoxContent)
		{
			vector5.y -= 20f;
			UIText createText = new UIText();
			CreateText(vector5, Color.black, ref createText);
			createText.SetText(item);
		}
		CreateText(vector, D3DMain.Instance.CommonFontColor, ref _txtGoldCost);
		CreateText(vector2, D3DMain.Instance.CommonFontColor, ref _txtCrystalCost);
		_txtGoldCost.AlignStyle = UIText.enAlignStyle.center;
		_txtCrystalCost.AlignStyle = UIText.enAlignStyle.center;
		CreateImage(vector3, "jinbi", ref _imgIconGold);
		CreateImage(vector4, "shuijing", ref _imgIconCrystal);
	}

	private void CreateImage(Vector4 rect, string strTexture, ref UIImage createImg)
	{
		createImg = new UIImage();
		createImg.SetTexture(LoadUIMaterialAutoHD(GetImageCell(strTexture).cell_texture), D3DMain.Instance.ConvertRectAutoHD(GetImageCell(strTexture).cell_rect));
		createImg.Rect = D3DMain.Instance.ConvertRectAutoHD(rect.x, rect.y, rect.z, rect.w);
		createImg.Enable = false;
		m_UIManagerRef[0].Add(createImg);
	}

	private void CreateText(Vector4 rect, Color textColor, ref UIText createText)
	{
		createText = new UIText();
		createText.Enable = false;
		createText.Set(LoadFontAutoHD(D3DMain.Instance.GameFont2.FontName, 11), string.Empty, textColor);
		createText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(11 * D3DMain.Instance.HD_SIZE);
		createText.Rect = D3DMain.Instance.ConvertRectAutoHD(rect.x, rect.y, rect.z, rect.w);
		createText.SetText("Fill me!!!");
		m_UIManagerRef[0].Add(createText);
	}

	private IEnumerator IntervalUpdateCost(float fTimeInterval)
	{
		while (true)
		{
			float fTimeRatioLeft = _respawn.GetLeftTime() / _respawn.GetTolRespawnTime();
			D3DRespawnRule.Instance.GetCost(fTimeRatioLeft, _respawn.GetRespawnValue(), out _nCurGoldCost, out _nCurCrystalCost);
			_txtGoldCost.SetText(_nCurGoldCost.ToString());
			_txtCrystalCost.SetText(_nCurCrystalCost.ToString());
			float fIconGoldPosX = CalcIconPosX(_txtGoldCost);
			_imgIconGold.SetPosition(new Vector2(fIconGoldPosX, _imgIconGold.GetPosition().y));
			float fIconCrystalPosX = CalcIconPosX(_txtCrystalCost);
			_imgIconCrystal.SetPosition(new Vector2(fIconCrystalPosX, _imgIconCrystal.GetPosition().y));
			yield return new WaitForSeconds(fTimeInterval);
		}
	}

	private float CalcIconPosX(UIText txtNum)
	{
		float num = 14f;
		float num2 = (txtNum.Rect.width - txtNum.GetTextWidth()) * 0.5f + txtNum.Rect.xMin + txtNum.GetTextWidth();
		return num2 + num;
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("IIapCloseBtn") == control.Id && command == 0)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_ROUND), null, false, false);
			Object.Destroy(base.gameObject);
			Time.timeScale = 1f;
		}
		else if (GetControlId("OKBtn") == control.Id && command == 0)
		{
			_respawn.RespawnRightnow();
			Object.Destroy(base.gameObject);
			Time.timeScale = 1f;
		}
		else if (GetControlId("UseGoldBtn") == control.Id && command == 0)
		{
			DoPressRespawnNow(true);
		}
		else if (GetControlId("UseCrystalBtn") == control.Id && command == 0)
		{
			DoPressRespawnNow(false);
		}
	}

	private void DoPressRespawnNow(bool bUseGold)
	{
		bool flag = true;
		if (bUseGold)
		{
			int num = int.Parse(D3DGamer.Instance.CurrencyText);
			if (num < _nCurGoldCost)
			{
				flag = false;
			}
			else
			{
				D3DGamer.Instance.UpdateCurrency(-_nCurGoldCost);
			}
		}
		else
		{
			int num2 = int.Parse(D3DGamer.Instance.CrystalText);
			if (num2 < _nCurCrystalCost)
			{
				flag = false;
			}
			else
			{
				D3DGamer.Instance.UpdateCrystal(-_nCurCrystalCost);
			}
		}
		if (!flag)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIInstantGoldCrystalIap"));
			return;
		}
		Time.timeScale = 1f;
		_respawn.RespawnRightnow();
		Object.Destroy(base.gameObject);
	}
}
