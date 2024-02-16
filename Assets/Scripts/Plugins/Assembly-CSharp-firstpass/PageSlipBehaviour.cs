using UnityEngine;

public class PageSlipBehaviour : MonoBehaviour
{
	private D3DPageSlipUI page_slip_ui;

	private float last_slip_real_time;

	private float current_slip_real_time;

	private float delta_slip_real_time;

	public D3DPageSlipUI PageSlipUI
	{
		set
		{
			page_slip_ui = value;
		}
	}

	private void Start()
	{
		last_slip_real_time = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		current_slip_real_time = Time.realtimeSinceStartup;
		delta_slip_real_time = current_slip_real_time - last_slip_real_time;
		last_slip_real_time = current_slip_real_time;
		page_slip_ui.PageAutoSlip(delta_slip_real_time);
	}

	public void StartRealTime()
	{
		last_slip_real_time = Time.realtimeSinceStartup;
	}
}
