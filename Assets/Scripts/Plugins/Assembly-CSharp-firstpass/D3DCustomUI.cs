using UnityEngine;

public class D3DCustomUI
{
	protected UIManager ui_manager;

	protected UIHelper ui_helper;

	protected Vector2 ui_position;

	public D3DCustomUI(UIManager manager, UIHelper helper)
	{
		ui_manager = manager;
		ui_helper = helper;
	}
}
