using UnityEngine;

public class D3DUICompareGearDescription : D3DUIGearDescription
{
	private UIControl[] bar_sub_control;

	public D3DUICompareGearDescription(UIManager manager, UIHelper helper, Rect camera_view_port, UIControl[] bar_sub_control)
		: base(manager, helper, camera_view_port)
	{
		this.bar_sub_control = bar_sub_control;
	}

	public void NoDescription()
	{
		ResetScroll();
		UIText[] array = property_texts;
		foreach (UIText uIText in array)
		{
			uIText.Visible = false;
			uIText.SetColor(common_color);
		}
		foreach (UIText extra_text in extra_texts)
		{
			extra_text.Visible = false;
		}
		foreach (UIText description_text in description_texts)
		{
			description_text.Visible = false;
		}
		UIControl[] array2 = bar_sub_control;
		foreach (UIControl uIControl in array2)
		{
			uIControl.Visible = false;
		}
		property_texts[0].SetText("NONE");
		property_texts[0].Rect = D3DMain.Instance.ConvertRectAutoHD(62f, 75f, 60f, 30f);
		property_texts[0].Visible = true;
	}

	protected override void UpdateScrollBar()
	{
		base.UpdateScrollBar();
		if (scrollY_bar_board != null && scrollY_bar_board.Visible)
		{
			UIControl[] array = bar_sub_control;
			foreach (UIControl uIControl in array)
			{
				uIControl.Visible = true;
			}
		}
		else
		{
			UIControl[] array2 = bar_sub_control;
			foreach (UIControl uIControl2 in array2)
			{
				uIControl2.Visible = false;
			}
		}
	}
}
