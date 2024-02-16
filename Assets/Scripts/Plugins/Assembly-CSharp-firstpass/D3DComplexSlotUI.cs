using UnityEngine;

public class D3DComplexSlotUI : D3DCustomUI
{
	protected UIImage select_frame;

	protected UIImage slot_icon;

	protected UIImage slot_hover_mask;

	public UIImage SlotIcon
	{
		get
		{
			return slot_icon;
		}
	}

	public D3DComplexSlotUI(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public virtual bool PtInSlot(Vector2 point)
	{
		if (select_frame.PtInRect(point))
		{
			return true;
		}
		if (D3DPlaneGeometry.CircleIntersectRect(point, 4 * D3DMain.Instance.HD_SIZE, select_frame.Rect))
		{
			return true;
		}
		return false;
	}

	public void Select(bool select)
	{
		select_frame.Visible = select;
	}

	public void SetHover(bool visible, bool enable_state)
	{
		slot_hover_mask.Visible = visible;
		if (visible)
		{
			if (enable_state)
			{
				slot_hover_mask.SetColor(new Color(0.2901961f, 1f, 0.7647059f));
			}
			else
			{
				slot_hover_mask.SetColor(Color.red);
			}
		}
	}
}
