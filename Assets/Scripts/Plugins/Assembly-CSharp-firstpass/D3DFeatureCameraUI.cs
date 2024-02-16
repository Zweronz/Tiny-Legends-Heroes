using UnityEngine;

public class D3DFeatureCameraUI : D3DCustomUI
{
	protected UIImage photo_frame;

	protected Camera feature_camera;

	protected GameObject feature_obj;

	protected Vector2 screen_offset;

	public Rect CameraRect
	{
		get
		{
			return feature_camera.pixelRect;
		}
	}

	public D3DFeatureCameraUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
		screen_offset = Vector2.zero;
	}

	public void CreateControl(Vector2 position, string frame_cell, Vector2 frame_size, GameObject feature_obj, Vector2 camera_offset, Vector2 camera_size)
	{
		ui_position = position;
		if (string.Empty != frame_cell)
		{
			photo_frame = new UIImage();
			D3DImageCell imageCell = ui_helper.GetImageCell(frame_cell);
			photo_frame.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), frame_size * D3DMain.Instance.HD_SIZE);
			photo_frame.Rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x, ui_position.y, frame_size.x, frame_size.y);
			photo_frame.Enable = false;
			ui_manager.Add(photo_frame);
		}
		GameObject gameObject = new GameObject("FeatureCamera");
		feature_camera = gameObject.AddComponent<Camera>();
		feature_camera.depth = ui_manager.DEPTH + 0.005f;
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
			this.feature_obj = feature_obj;
		}
		else
		{
			gameObject.SetActiveRecursively(false);
		}
	}

	public void SetCameraFeatureObject(GameObject feature_obj)
	{
		if (null == feature_obj)
		{
			this.feature_obj = null;
			feature_camera.transform.parent = null;
		}
		else
		{
			this.feature_obj = feature_obj;
			feature_camera.transform.parent = this.feature_obj.transform;
			feature_camera.transform.localPosition = Vector3.zero;
		}
	}

	public void SetCameraFeatureTransform(Vector3 offset, Vector3 rotation, float size)
	{
		feature_camera.transform.position += offset;
		feature_camera.transform.rotation = Quaternion.identity;
		feature_camera.transform.Rotate(rotation);
		feature_camera.orthographicSize = size;
	}

	public void ViewFeatureObj(Vector3 axis, float angle)
	{
		if (!(null == feature_obj))
		{
			feature_camera.transform.RotateAround(feature_obj.transform.position, axis, angle);
		}
	}

	public void Visible(bool visible)
	{
		feature_camera.gameObject.SetActiveRecursively(visible);
		if (photo_frame != null)
		{
			photo_frame.Visible = visible;
		}
	}

	public bool IsVisible()
	{
		return feature_camera.gameObject.active;
	}
}
