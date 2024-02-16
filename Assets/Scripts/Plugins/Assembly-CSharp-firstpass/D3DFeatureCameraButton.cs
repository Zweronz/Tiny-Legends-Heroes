using UnityEngine;

public class D3DFeatureCameraButton : D3DFeatureCameraUI
{
	protected UIPushButton feature_button;

	public UIPushButton PushButton
	{
		get
		{
			return feature_button;
		}
	}

	public D3DFeatureCameraButton(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateControl(Vector2 position, string frame_cell, Vector2 frame_size, GameObject feature_obj, Vector2 camera_offset, Vector2 camera_size, string[] button_cells, Rect button_local_rect)
	{
		ui_position = position;
		if (button_cells != null)
		{
			feature_button = new UIPushButton();
			if (string.Empty != button_cells[0])
			{
				D3DImageCell imageCell = ui_helper.GetImageCell(button_cells[0]);
				feature_button.SetTexture(UIButtonBase.State.Normal, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(button_local_rect.width, button_local_rect.height) * D3DMain.Instance.HD_SIZE);
			}
			if (string.Empty != button_cells[1])
			{
				D3DImageCell imageCell2 = ui_helper.GetImageCell(button_cells[1]);
				feature_button.SetTexture(UIButtonBase.State.Pressed, ui_helper.LoadUIMaterialAutoHD(imageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell2.cell_rect), new Vector2(button_local_rect.width, button_local_rect.height) * D3DMain.Instance.HD_SIZE);
			}
			if (string.Empty != button_cells[2])
			{
				D3DImageCell imageCell3 = ui_helper.GetImageCell(button_cells[1]);
				feature_button.SetTexture(UIButtonBase.State.Disabled, ui_helper.LoadUIMaterialAutoHD(imageCell3.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell3.cell_rect), new Vector2(button_local_rect.width, button_local_rect.height) * D3DMain.Instance.HD_SIZE);
			}
			feature_button.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + button_local_rect.x, ui_position.y + button_local_rect.y, button_local_rect.width, button_local_rect.height);
			ui_manager.Add(feature_button);
		}
		if (string.Empty != frame_cell)
		{
			photo_frame = new UIImage();
			D3DImageCell imageCell4 = ui_helper.GetImageCell(frame_cell);
			photo_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell4.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell4.cell_rect), frame_size * D3DMain.Instance.HD_SIZE);
			photo_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, frame_size.x, frame_size.y);
			photo_frame.Enable = false;
			ui_manager.Add(photo_frame);
		}
		GameObject gameObject = new GameObject("FeatureCamera");
		feature_camera = gameObject.AddComponent<Camera>();
		feature_camera.depth = ui_manager.DEPTH;
		feature_camera.clearFlags = CameraClearFlags.Depth;
		feature_camera.cullingMask = 65536;
		feature_camera.nearClipPlane = -3f;
		feature_camera.farClipPlane = 3f;
		feature_camera.orthographic = true;
		feature_camera.aspect = 1f;
		feature_camera.orthographicSize = 1f;
		Rect rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + camera_offset.x, ui_position.y + camera_offset.y, camera_size.x, camera_size.y);
		float num = (float)Screen.height / 640f;
		feature_camera.pixelRect = new Rect(((float)Screen.width - 960f * num) * 0.5f + (rect.x + screen_offset.x) * num, (rect.y + screen_offset.y) * num, rect.width * num, rect.height * num);
		feature_camera.aspect = camera_size.x / camera_size.y;
		if (null != feature_obj)
		{
			gameObject.transform.parent = feature_obj.transform;
			gameObject.transform.localPosition = Vector3.zero;
			base.feature_obj = feature_obj;
		}
		else
		{
			gameObject.SetActiveRecursively(false);
		}
	}

	public void Set(bool push_down)
	{
		if (feature_button != null)
		{
			feature_button.Set(push_down);
		}
	}

	public new void Visible(bool visible)
	{
		feature_camera.gameObject.SetActiveRecursively(visible);
		if (photo_frame != null)
		{
			photo_frame.Visible = visible;
		}
		feature_button.Visible = visible;
		feature_button.Enable = visible;
	}

	public void SetFrame(string frame_cell, Vector2 frame_size)
	{
		if (photo_frame == null)
		{
			photo_frame = new UIImage();
			photo_frame.Enable = false;
			ui_manager.Add(photo_frame);
		}
		D3DImageCell imageCell = ui_helper.GetImageCell(frame_cell);
		photo_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), frame_size * D3DMain.Instance.HD_SIZE);
		photo_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, frame_size.x, frame_size.y);
	}
}
