using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour, UIContainer
{
	public int LAYER;

	public float DEPTH;

	public bool CLEAR;

	public bool m_bCenterForiPad = true;

	private Vector2 m_ScreenOffset;

	private Vector2 m_ViewPortCenter;

	private Vector2 m_TouchOffset;

	private Rect m_ManagerRect;

	public CameraClearFlags FLAG = CameraClearFlags.Depth;

	private UIMesh m_UIMesh;

	private SpriteCamera m_SpriteCamera;

	private UIHandler m_UIHandler;

	public bool EnableUIHandler;

	private ArrayList m_Controls;

	public Vector2 ScreenOffset
	{
		get
		{
			return m_ScreenOffset;
		}
	}

	public Rect ManagerRect
	{
		get
		{
			return m_ManagerRect;
		}
	}

	public UIManager()
	{
		m_UIMesh = null;
		m_SpriteCamera = null;
		m_UIHandler = null;
		m_Controls = new ArrayList();
		m_bCenterForiPad = true;
		m_ScreenOffset = Vector2.zero;
		m_ViewPortCenter = Vector2.zero;
		m_TouchOffset = Vector2.zero;
		EnableUIHandler = true;
	}

	public void SetUIHandler(UIHandler ui_handler)
	{
		m_UIHandler = ui_handler;
	}

	public void Add(UIControl control)
	{
		m_Controls.Add(control);
		control.SetParent(this);
	}

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	public void RemoveAll()
	{
		m_Controls.Clear();
	}

	public UITouchInner ConvertTouchPositionOnScreen(UITouchInner touch)
	{
		Vector2 vector = new Vector2(m_SpriteCamera.gameObject.transform.localPosition.x, m_SpriteCamera.gameObject.transform.localPosition.y);
		touch.position -= m_ScreenOffset - vector + m_ViewPortCenter;
		return touch;
	}

	public bool HandleInput(UITouchInner touch)
	{
		Vector2 vector = new Vector2(m_SpriteCamera.gameObject.transform.localPosition.x, m_SpriteCamera.gameObject.transform.localPosition.y);
		Vector2 position = touch.position;
		touch.position -= m_ScreenOffset - vector + m_ViewPortCenter;
		if (touch.phase == TouchPhase.Began)
		{
			Rect cameraTransformRect = GetCameraTransformRect();
			float num = (float)Screen.height / 640f;
			cameraTransformRect = new Rect(cameraTransformRect.x + ((float)Screen.width - 960f * num) * 0.5f, cameraTransformRect.y, cameraTransformRect.width, cameraTransformRect.height);
			if (!D3DPlaneGeometry.PtInRect(cameraTransformRect, touch.position))
			{
				touch.position = position;
				return false;
			}
		}
		for (int num2 = m_Controls.Count - 1; num2 >= 0; num2--)
		{
			UIControl uIControl = (UIControl)m_Controls[num2];
			if (uIControl.Enable)
			{
				touch.position = TouchPointOnManager(position);
				touch.position *= 640f / (float)Screen.height;
				bool flag = uIControl.HandleInput(touch);
				touch.position = position;
				if (flag)
				{
					return true;
				}
			}
		}
		touch.position = position;
		return false;
	}

	public void Awake()
	{
	}

	public void SetParameter(int layer, float depth, bool clear)
	{
		LAYER = layer;
		DEPTH = depth;
		CLEAR = clear;
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void SetParameter(int layer, float depth, CameraClearFlags flag = CameraClearFlags.Depth)
	{
		LAYER = layer;
		DEPTH = depth;
		FLAG = flag;
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void Start()
	{
	}

	public void LateUpdate()
	{
		m_UIMesh.RemoveAll();
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = (UIControl)m_Controls[i];
			uIControl.Update();
			if (uIControl.Visible)
			{
				uIControl.Draw();
			}
		}
		m_UIMesh.DoLateUpdate();
	}

	public void DrawSprite(TUISprite sprite)
	{
		m_UIMesh.Add(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
	}

	private void Initialize()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject gameObject = new GameObject("UIMesh");
		gameObject.transform.parent = base.gameObject.transform;
		m_UIMesh = (UIMesh)gameObject.AddComponent(typeof(UIMesh));
		m_UIMesh.Initialize(LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject gameObject = new GameObject("SpriteCamera");
		gameObject.transform.parent = base.gameObject.transform;
		m_SpriteCamera = (SpriteCamera)gameObject.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetFlag(FLAG);
		m_SpriteCamera.SetDepth(DEPTH);
		m_SpriteCamera.SetViewport(new Rect(0f, 0f, GameScreen.width, GameScreen.height));
		float num = (float)Screen.height / 640f;
		m_TouchOffset = new Vector2(((float)Screen.width - 960f * num) * 0.5f, 0f);
		m_ManagerRect = new Rect(0f, 0f, 960f, 640f);
		m_ViewPortCenter = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
	}

	public void SetSpriteCameraViewPort(Rect view_port)
	{
		m_SpriteCamera.SetViewport(new Rect(view_port.xMin + m_ScreenOffset.x, view_port.yMin + m_ScreenOffset.y, view_port.width, view_port.height));
		m_SpriteCamera.gameObject.transform.localPosition -= new Vector3(m_ScreenOffset.x, m_ScreenOffset.y, 0f);
		m_ViewPortCenter = new Vector2(m_SpriteCamera.gameObject.transform.localPosition.x, m_SpriteCamera.gameObject.transform.localPosition.y);
		m_SpriteCamera.transform.localPosition -= new Vector3(view_port.x, view_port.y, 0f);
		m_ManagerRect = view_port;
		float num = (float)Screen.height / 640f;
		m_TouchOffset = new Vector2(((float)Screen.width - 960f * num) * 0.5f + view_port.xMin * num, view_port.yMin * num);
	}

	public Vector2 TouchPointOnScreen(Vector2 touch_point)
	{
		return new Vector2(m_TouchOffset.x + touch_point.x, m_TouchOffset.y + touch_point.y);
	}

	public Vector2 TouchPointOnManager(Vector2 touch_point)
	{
		return new Vector2(touch_point.x - m_TouchOffset.x, touch_point.y - m_TouchOffset.y);
	}

	public void SetSpriteCameraScreenPosition(Vector2 position)
	{
		m_SpriteCamera.GetCamera().pixelRect = new Rect(position.x, position.y, m_SpriteCamera.GetCamera().pixelWidth, m_SpriteCamera.GetCamera().pixelHeight);
	}

	public Camera GetManagerCamera()
	{
		return m_SpriteCamera.GetCamera();
	}

	public Rect GetCameraTransformRect()
	{
		Camera camera = m_SpriteCamera.GetCamera();
		Vector3 localPosition = camera.transform.localPosition;
		float num = 640f / (float)Screen.height;
		return new Rect(localPosition.x - camera.pixelWidth * num * 0.5f, localPosition.y - camera.pixelHeight * num * 0.5f, camera.pixelWidth, camera.pixelHeight);
	}
}
