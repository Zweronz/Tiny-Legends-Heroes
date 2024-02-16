using System.Collections.Generic;
using UnityEngine;

public class D3DMessageBoxButtonEvent : UIHandler
{
	public delegate void OnButtonClick();

	private UIManager mgb_manager;

	private UIHelper mgb_helper;

	private Dictionary<UIControl, OnButtonClick> mgb_events;

	public D3DMessageBoxButtonEvent(UIHelper helper, UIManager manager, Dictionary<UIControl, OnButtonClick> events)
	{
		mgb_helper = helper;
		mgb_manager = manager;
		mgb_events = events;
		mgb_manager.SetUIHandler(this);
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (command != 0)
		{
			return;
		}
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BUTTON_SQUARE), null, false, false);
		if (mgb_events.ContainsKey(control))
		{
			if (mgb_events[control] != null)
			{
				mgb_events[control]();
			}
			mgb_helper.RemoveUIManager(mgb_manager);
			Object.Destroy(mgb_manager.gameObject);
		}
	}
}
