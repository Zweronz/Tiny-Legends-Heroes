using UnityEngine;

public class SpriteCamera : MonoBehaviour
{
	private int m_Layer;

	private Transform m_Transform;

	private Camera m_Camera;

	private Rect m_Range = new Rect(-1f, -1f, 1f, 1f);

	public void Initialize(int layer)
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		m_Layer = layer;
		m_Transform = base.gameObject.transform;
		m_Camera = (Camera)base.gameObject.AddComponent(typeof(Camera));
		m_Transform.position = Vector3.zero;
		m_Transform.LookAt(Vector3.zero, Vector3.up);
		m_Camera.clearFlags = CameraClearFlags.Nothing;
		m_Camera.backgroundColor = Color.white;
		m_Camera.nearClipPlane = -1f;
		m_Camera.farClipPlane = 1f;
		m_Camera.orthographic = true;
		m_Camera.aspect = 1f;
		m_Camera.orthographicSize = 1f;
		m_Camera.depth = 0f;
		m_Camera.cullingMask = 1 << m_Layer;
	}

	public void SetClear(bool clear)
	{
		if (clear)
		{
			m_Camera.clearFlags = CameraClearFlags.Color;
		}
		else
		{
			m_Camera.clearFlags = CameraClearFlags.Nothing;
		}
	}

	public void SetFlag(CameraClearFlags flag)
	{
		m_Camera.clearFlags = flag;
	}

	public void SetDepth(float depth)
	{
		m_Camera.depth = depth;
	}

	public void SetViewport(Rect range)
	{
		float num = (float)Screen.height / 640f;
		Rect rect = new Rect(((float)Screen.width - 960f * num) * 0.5f + range.x * num, range.y * num, range.width * num, range.height * num);
		base.transform.localPosition = new Vector3((range.xMin + range.xMax) / 2f, (range.yMin + range.yMax) / 2f, 0f);
		m_Camera.pixelRect = rect;
		m_Camera.aspect = range.width / range.height;
		m_Camera.orthographicSize = range.height / 2f;
		m_Range = rect;
	}

	public void SetCameraScreenPosition(Vector2 position)
	{
		base.transform.localPosition = new Vector3((position.x + m_Range.width) / 2f, (position.y + m_Range.height) / 2f, 0f);
	}

	public Vector2 ScreenToWorld(Vector2 point)
	{
		return new Vector2(m_Range.x + point.x / ((float)GameScreen.width * ((float)Screen.height / 640f)) * m_Range.width, m_Range.y + point.y / (float)Screen.height * m_Range.height);
	}

	public Camera GetCamera()
	{
		return m_Camera;
	}
}
