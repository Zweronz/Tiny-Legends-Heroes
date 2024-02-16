using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHDBoard : UIHelper
{
	public enum HD_DEVICE
	{
		NEW_PAD_OR_IPAD12 = 0,
		IPHONE5 = 1,
		OTHERS = 2
	}

	public static HD_DEVICE DEVICE;

	private new void Awake()
	{
		base.name = "UIHDBoard";
		base.transform.position = new Vector3(-2000f, 0f, -2000f);
		m_UIManagerRef = new List<UIManager>();
		UIImageCellIndexer = new Dictionary<string, D3DImageCell>();
		AddImageCellIndexer(new string[1] { "HD_Boards_cell" });
	}

	private new void Start()
	{
		base.Start();
		GameObject gameObject = new GameObject("Manager_Main");
		UIManager uIManager = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef.Add(uIManager);
		uIManager.SetParameter(15, 0.5f);
		uIManager.SetUIHandler(this);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward * (m_UIManagerRef.Count - 1) * 3f;
		m_UIManagerRef[0].SetSpriteCameraViewPort(new Rect(0f - m_UIManagerRef[0].ScreenOffset.x, 0f - m_UIManagerRef[0].ScreenOffset.y, GameScreen.width, GameScreen.height));
		if (DEVICE == HD_DEVICE.NEW_PAD_OR_IPAD12)
		{
			UIImage uIImage = new UIImage();
			D3DImageCell imageCell = GetImageCell("zuobiankuang-you");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.SetRotation((float)Math.PI / 2f);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(-198f, 169.5f, 384f, 45f);
			uIImage.Enable = false;
			m_UIManagerRef[0].Add(uIImage);
			uIImage = new UIImage();
			imageCell = GetImageCell("youbiankuang-zuo");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.SetRotation((float)Math.PI / 2f);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(326f, 169.5f, 384f, 45f);
			uIImage.Enable = false;
			m_UIManagerRef[0].Add(uIImage);
			uIImage = new UIImage();
			imageCell = GetImageCell("shangbiankuang");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 352f, 512f, 32f);
			uIImage.Enable = false;
			m_UIManagerRef[0].Add(uIImage);
			uIImage = new UIImage();
			imageCell = GetImageCell("xiabiankuang");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 512f, 32f);
			uIImage.Enable = false;
			m_UIManagerRef[0].Add(uIImage);
		}
		else
		{
			UIImage uIImage = new UIImage();
			D3DImageCell imageCell = GetImageCell("zuobiankuang-you");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.SetRotation((float)Math.PI / 2f);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(-169.5f, 137.5f, 384f, 45f);
			uIImage.Enable = false;
			m_UIManagerRef[0].Add(uIImage);
			uIImage = new UIImage();
			imageCell = GetImageCell("youbiankuang-zuo");
			uIImage.SetTexture(LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			uIImage.SetRotation((float)Math.PI / 2f);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(354f, 137.5f, 384f, 45f);
			uIImage.Enable = false;
			m_UIManagerRef[0].Add(uIImage);
		}
		base.enabled = false;
	}

	private new void Update()
	{
	}

	public static HD_DEVICE GetHDDeviceType()
	{
		return HD_DEVICE.OTHERS;
	}
}
