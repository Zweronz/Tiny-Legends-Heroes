using UnityEngine;

public class D3DFeatureCameraSlipUI : D3DFeatureCameraUI
{
	private Vector2 camera_offset;

	private Vector2 camera_size;

	public D3DFeatureCameraSlipUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public new void CreateControl(Vector2 position, string frame_cell, Vector2 frame_size, GameObject feature_obj, Vector2 camera_offset, Vector2 camera_size)
	{
		base.CreateControl(position, frame_cell, frame_size, feature_obj, camera_offset, camera_size);
		this.camera_offset = camera_offset;
		this.camera_size = camera_size;
	}

	public void Slip(float slip_x, float left_limit, float right_limit)
	{
		if (!(null == feature_obj))
		{
			float num = (ui_position.x + camera_offset.x) * (float)D3DMain.Instance.HD_SIZE + slip_x;
			Rect rect = D3DMain.Instance.ConvertRectAutoHD(num * 1f / (float)D3DMain.Instance.HD_SIZE, ui_position.y + camera_offset.y, camera_size.x, camera_size.y);
			float num2 = (float)Screen.height / 640f;
			left_limit *= num2;
			right_limit *= num2;
			feature_camera.pixelRect = new Rect(((float)Screen.width - 960f * num2) * 0.5f + (rect.x + screen_offset.x) * num2, (rect.y + screen_offset.y) * num2, rect.width * num2, rect.height * num2);
			feature_camera.aspect = camera_size.x / camera_size.y;
			Visible(true);
			if (feature_camera.pixelRect.xMin - left_limit < 0f)
			{
				Visible(false);
			}
			else if (right_limit - feature_camera.pixelRect.xMax < 0f)
			{
				Visible(false);
			}
		}
	}

	public void SetX(float x)
	{
		ui_position = new Vector2(x, ui_position.y);
		Rect rect = D3DMain.Instance.ConvertRectAutoHD(ui_position.x + camera_offset.x, ui_position.y + camera_offset.y, camera_size.x, camera_size.y);
		float num = (float)Screen.height / 640f;
		feature_camera.pixelRect = new Rect(((float)Screen.width - 960f * num) * 0.5f + (rect.x + screen_offset.x) * num, (rect.y + screen_offset.y) * num, rect.width * num, rect.height * num);
		feature_camera.aspect = camera_size.x / camera_size.y;
	}
}
