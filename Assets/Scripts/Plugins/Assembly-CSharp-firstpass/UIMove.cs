using UnityEngine;

public class UIMove : UIControl
{
	public enum Command
	{
		Down = 0,
		Begin = 1,
		Move = 2,
		Hold = 3,
		End = 4
	}

	protected int m_FingerId;

	protected Vector2 m_TouchPosition;

	protected bool m_Move;

	protected float m_MinX;

	protected float m_MinY;

	public float MinX
	{
		get
		{
			return m_MinX;
		}
		set
		{
			m_MinX = value;
		}
	}

	public float MinY
	{
		get
		{
			return m_MinY;
		}
		set
		{
			m_MinY = value;
		}
	}

	public UIMove()
	{
		m_FingerId = -1;
		m_TouchPosition = new Vector2(0f, 0f);
		m_Move = false;
		m_MinX = 10f;
		m_MinY = 10f;
	}

	public Vector2 GetCurrentPosition()
	{
		return m_TouchPosition;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				m_TouchPosition = touch.position;
				m_Move = false;
				m_Parent.SendEvent(this, 0, 0f, 0f);
			}
			return false;
		}
		if (touch.fingerId != m_FingerId)
		{
			return false;
		}
		if (!PtInRect(touch.position))
		{
			m_FingerId = -1;
			float wparam = touch.position.x - m_TouchPosition.x;
			float lparam = touch.position.y - m_TouchPosition.y;
			m_Move = false;
			m_Parent.SendEvent(this, 4, wparam, lparam);
			m_TouchPosition = new Vector2(0f, 0f);
			return true;
		}
		if (touch.phase == TouchPhase.Moved)
		{
			float num = touch.position.x - m_TouchPosition.x;
			float num2 = touch.position.y - m_TouchPosition.y;
			if (m_Move)
			{
				m_TouchPosition = touch.position;
				m_Parent.SendEvent(this, 2, num, num2);
			}
			else
			{
				float num3 = ((!(num >= 0f)) ? (0f - num) : num);
				float num4 = ((!(num2 >= 0f)) ? (0f - num2) : num2);
				if (!(num3 > m_MinX) && !(num4 > m_MinY))
				{
					return false;
				}
				m_TouchPosition = touch.position;
				m_Move = true;
				m_Parent.SendEvent(this, 1, 0f, 0f);
				m_Parent.SendEvent(this, 2, num, num2);
			}
			return true;
		}
		if (touch.phase == TouchPhase.Stationary)
		{
			m_Parent.SendEvent(this, 3, 0f, 0f);
			return true;
		}
		if (touch.phase == TouchPhase.Ended)
		{
			m_FingerId = -1;
			float wparam2 = touch.position.x - m_TouchPosition.x;
			float lparam2 = touch.position.y - m_TouchPosition.y;
			m_Move = false;
			m_Parent.SendEvent(this, 4, wparam2, lparam2);
			m_TouchPosition = new Vector2(0f, 0f);
		}
		return false;
	}
}
