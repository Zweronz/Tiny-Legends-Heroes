using UnityEngine;

public class D3DScrollManager
{
	protected UIManager ui_manager;

	protected UIHelper ui_helper;

	protected Rect scroll_limit;

	protected Vector3 camera_reset_position;

	protected Rect camera_default_rect;

	protected UIImage scrollX_bar_board;

	protected UIImage scrollX_bar;

	protected UIImage scrollY_bar_board;

	protected UIImage scrollY_bar;

	protected float bar_thickness;

	protected ScrollInertiaBehaviour scroll_inertia_behaviour;

	public D3DScrollManager(UIManager manager, UIHelper helper, Rect camera_view_port)
	{
		ui_manager = manager;
		ui_helper = helper;
		bar_thickness = 5 * D3DMain.Instance.HD_SIZE;
		GameObject gameObject = new GameObject("ScrollInertia");
		gameObject.transform.parent = ui_manager.transform;
		scroll_inertia_behaviour = gameObject.AddComponent<ScrollInertiaBehaviour>();
		scroll_inertia_behaviour.ScrollManager = this;
		scroll_inertia_behaviour.enabled = false;
		SetScrollViewPort(camera_view_port);
	}

	public void SetScrollViewPort(Rect camera_view_port)
	{
		ui_manager.SetSpriteCameraViewPort(D3DMain.Instance.ConvertRectAutoHD(camera_view_port));
		Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
		scroll_limit = new Rect(cameraTransformRect.x, cameraTransformRect.y, camera_view_port.width * 2f, camera_view_port.height * 2f);
		camera_reset_position = ui_manager.GetManagerCamera().transform.localPosition;
		camera_default_rect = scroll_limit;
	}

	public void CreateScrollBar(bool createX, bool createY)
	{
		D3DImageCell imageCell = ui_helper.GetImageCell("ui_monolayer");
		if (createX)
		{
			scrollX_bar_board = new UIImage();
			scrollX_bar_board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			scrollX_bar_board.SetColor(new Color(0f, 0f, 0f, 0.5f));
			scrollX_bar_board.Enable = false;
			ui_manager.Add(scrollX_bar_board);
		}
		if (createY)
		{
			scrollY_bar_board = new UIImage();
			scrollY_bar_board.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			scrollY_bar_board.SetColor(new Color(0f, 0f, 0f, 0.5f));
			scrollY_bar_board.Enable = false;
			ui_manager.Add(scrollY_bar_board);
		}
		imageCell = ui_helper.GetImageCell("huadongtiao1h");
		if (createX)
		{
			scrollX_bar = new UIImage();
			scrollX_bar.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			scrollX_bar.Enable = false;
			ui_manager.Add(scrollX_bar);
		}
		imageCell = ui_helper.GetImageCell("huadongtiao1");
		if (createY)
		{
			scrollY_bar = new UIImage();
			scrollY_bar.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
			scrollY_bar.Enable = false;
			ui_manager.Add(scrollY_bar);
		}
	}

	public void InitScrollBar()
	{
		Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
		float num = 640f / (float)Screen.height;
		cameraTransformRect = new Rect(cameraTransformRect.x, cameraTransformRect.y, cameraTransformRect.width * num, cameraTransformRect.height * num);
		if (scrollX_bar_board != null)
		{
			if (cameraTransformRect.width >= scroll_limit.width)
			{
				scrollX_bar_board.Visible = false;
				scrollX_bar.Visible = false;
			}
			else
			{
				scrollX_bar_board.Visible = true;
				scrollX_bar_board.SetTextureSize(new Vector2(cameraTransformRect.width, bar_thickness));
				ui_manager.Remove(scrollX_bar_board);
				ui_manager.Add(scrollX_bar_board);
				scrollX_bar.Visible = true;
				scrollX_bar.SetTextureSize(new Vector2(cameraTransformRect.width * cameraTransformRect.width / scroll_limit.width, bar_thickness));
				ui_manager.Remove(scrollX_bar);
				ui_manager.Add(scrollX_bar);
			}
		}
		if (scrollY_bar_board != null)
		{
			if (cameraTransformRect.height >= scroll_limit.height)
			{
				scrollY_bar_board.Visible = false;
				scrollY_bar.Visible = false;
			}
			else
			{
				scrollY_bar_board.Visible = true;
				scrollY_bar_board.SetTextureSize(new Vector2(bar_thickness, cameraTransformRect.height));
				ui_manager.Remove(scrollY_bar_board);
				ui_manager.Add(scrollY_bar_board);
				scrollY_bar.Visible = true;
				scrollY_bar.SetTextureSize(new Vector2(bar_thickness, cameraTransformRect.height * cameraTransformRect.height / scroll_limit.height));
				ui_manager.Remove(scrollY_bar);
				ui_manager.Add(scrollY_bar);
			}
		}
		UpdateScrollBar();
	}

	protected virtual void UpdateScrollBar()
	{
		Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
		float num = 640f / (float)Screen.height;
		cameraTransformRect = new Rect(cameraTransformRect.x, cameraTransformRect.y, cameraTransformRect.width * num, cameraTransformRect.height * num);
		if (scrollX_bar_board != null && scrollX_bar_board.Visible)
		{
			Vector2 textureSize = scrollX_bar_board.GetTextureSize();
			Vector2 textureSize2 = scrollX_bar.GetTextureSize();
			scrollX_bar_board.Rect = new Rect(cameraTransformRect.x, cameraTransformRect.y, textureSize.x, textureSize.y);
			scrollX_bar.Rect = new Rect(cameraTransformRect.x + (cameraTransformRect.x - scroll_limit.x) / scroll_limit.width * textureSize.x, cameraTransformRect.y, textureSize2.x, textureSize2.y);
		}
		if (scrollY_bar_board != null && scrollY_bar_board.Visible)
		{
			Vector2 textureSize3 = scrollY_bar_board.GetTextureSize();
			Vector2 textureSize4 = scrollY_bar.GetTextureSize();
			scrollY_bar_board.Rect = new Rect(cameraTransformRect.xMax - textureSize3.x, cameraTransformRect.y, textureSize3.x, textureSize3.y);
			scrollY_bar.Rect = new Rect(cameraTransformRect.xMax - textureSize4.x, cameraTransformRect.y + (cameraTransformRect.y - scroll_limit.y) / scroll_limit.height * textureSize3.y, textureSize4.x, textureSize4.y);
		}
	}

	public void ResetScroll()
	{
		scroll_inertia_behaviour.enabled = false;
		ui_manager.GetManagerCamera().transform.localPosition = camera_reset_position;
		UpdateScrollBar();
	}

	public void Test()
	{
		UIImage uIImage = new UIImage();
		uIImage.SetTexture(ui_helper.LoadUIMaterialAutoHD("UI_cameratest"), new Rect(0f, 0f, 480 * D3DMain.Instance.HD_SIZE, 320 * D3DMain.Instance.HD_SIZE));
		uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		ui_manager.Add(uIImage);
	}

	public void Scroll(Vector2 scroll_delta, bool ignore_deactive = false)
	{
		if (!ui_manager.gameObject.active && !ignore_deactive)
		{
			return;
		}
		Camera managerCamera = ui_manager.GetManagerCamera();
		if (Vector2.zero == scroll_delta)
		{
			return;
		}
		if (scroll_limit.width <= managerCamera.pixelWidth * (640f / (float)Screen.height))
		{
			scroll_delta.x = 0f;
		}
		if (scroll_limit.height <= managerCamera.pixelHeight * (640f / (float)Screen.height))
		{
			scroll_delta.y = 0f;
		}
		managerCamera.transform.localPosition -= new Vector3(scroll_delta.x, scroll_delta.y, 0f);
		Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
		float num = 640f / (float)Screen.height;
		cameraTransformRect = new Rect(cameraTransformRect.x, cameraTransformRect.y, cameraTransformRect.width * num, cameraTransformRect.height * num);
		if (scroll_delta.x > 0f)
		{
			if (cameraTransformRect.xMin < scroll_limit.xMin)
			{
				float x = scroll_limit.xMin - cameraTransformRect.xMin + 0.5f;
				managerCamera.transform.localPosition += new Vector3(x, 0f, 0f);
			}
		}
		else if (scroll_delta.x < 0f && cameraTransformRect.xMax > scroll_limit.xMax)
		{
			float x2 = scroll_limit.xMax - cameraTransformRect.xMax + 0.5f;
			managerCamera.transform.localPosition += new Vector3(x2, 0f, 0f);
		}
		if (scroll_delta.y > 0f)
		{
			if (cameraTransformRect.yMin < scroll_limit.yMin)
			{
				float y = scroll_limit.yMin - cameraTransformRect.yMin - 0.5f;
				managerCamera.transform.localPosition += new Vector3(0f, y, 0f);
			}
		}
		else if (scroll_delta.y < 0f && cameraTransformRect.yMax > scroll_limit.yMax)
		{
			float y2 = scroll_limit.yMax - cameraTransformRect.yMax - 0.5f;
			managerCamera.transform.localPosition += new Vector3(0f, y2, 0f);
		}
		UpdateScrollBar();
	}

	public void StopInertia()
	{
		scroll_inertia_behaviour.enabled = false;
	}

	public void ScrollInertia(Vector2 inertia)
	{
		if (!(inertia == Vector2.zero))
		{
			scroll_inertia_behaviour.enabled = true;
			scroll_inertia_behaviour.StartInertia(inertia);
		}
	}

	public void UIScrollUnlimited(Vector2 scroll_delta)
	{
		Camera managerCamera = ui_manager.GetManagerCamera();
		if (managerCamera.gameObject.active && !(Vector2.zero == scroll_delta))
		{
			managerCamera.transform.localPosition -= new Vector3(scroll_delta.x, scroll_delta.y, 0f);
			UpdateScrollBar();
		}
	}

	public void Visible(bool visible)
	{
		ui_manager.gameObject.SetActiveRecursively(visible);
	}
}
