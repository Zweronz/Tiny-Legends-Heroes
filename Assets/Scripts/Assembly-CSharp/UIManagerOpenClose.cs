using UnityEngine;

public class UIManagerOpenClose : MonoBehaviour
{
	public delegate void OnClosed();

	private bool open;

	private float passed_time;

	private float last_real_time;

	private float delta_real_time;

	private Vector3 open_scale;

	private Vector3 close_scale;

	private Vector3 open_position;

	private Vector3 close_position;

	private Vector3 scale_velocity;

	private Vector3 move_velocity;

	private OnClosed onClosed;

	private void Start()
	{
	}

	private void Update()
	{
		delta_real_time = Time.realtimeSinceStartup - last_real_time;
		last_real_time = Time.realtimeSinceStartup;
		passed_time += delta_real_time;
		if (open)
		{
			base.transform.localPosition += move_velocity * delta_real_time;
			base.transform.localScale += scale_velocity * delta_real_time;
			if (passed_time >= 0.2f)
			{
				base.transform.localPosition = open_position;
				base.transform.localScale = open_scale;
				base.enabled = false;
			}
			return;
		}
		base.transform.localPosition -= move_velocity * delta_real_time;
		base.transform.localScale -= scale_velocity * delta_real_time;
		if (passed_time >= 0.2f)
		{
			base.transform.localPosition = close_position;
			base.transform.localScale = close_scale;
			base.transform.parent.gameObject.SetActiveRecursively(false);
			if (onClosed != null)
			{
				onClosed();
			}
			base.enabled = false;
		}
	}

	public void Init(Rect open_rect, Rect close_rect, OnClosed onClosed)
	{
		open_position = new Vector3(open_rect.x, open_rect.y, 0f);
		close_position = new Vector3(close_rect.x, close_rect.y, 0f);
		open_scale = Vector3.one;
		close_scale = new Vector3(close_rect.width / open_rect.width, close_rect.height / open_rect.height, 0f);
		scale_velocity = (open_scale - close_scale) * 5f;
		move_velocity = (open_position - close_position) * 5f;
		this.onClosed = onClosed;
	}

	public void Default()
	{
		base.transform.localPosition = open_position;
		base.transform.localScale = open_scale;
		base.enabled = false;
		base.transform.parent.gameObject.SetActiveRecursively(true);
	}

	public void Open()
	{
		open = true;
		base.transform.localPosition = close_position;
		base.transform.localScale = close_scale;
		last_real_time = Time.realtimeSinceStartup;
		passed_time = 0f;
		base.transform.parent.gameObject.SetActiveRecursively(true);
		base.enabled = true;
	}

	public void Close()
	{
		open = false;
		base.transform.localPosition = open_position;
		base.transform.localScale = open_scale;
		last_real_time = Time.realtimeSinceStartup;
		passed_time = 0f;
		base.enabled = true;
	}
}
