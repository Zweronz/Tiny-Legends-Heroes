using System.Collections.Generic;
using UnityEngine;

public class UIControlSpringBehaviour : MonoBehaviour
{
	public Dictionary<UIControl, float> SpringControls = new Dictionary<UIControl, float>();

	private float last_real_time;

	private float current_real_time;

	private float delta_real_time;

	private float spring_force;

	private float spring_velocity;

	private float rest_time;

	private void Start()
	{
		spring_force = 3f;
		spring_velocity = 3f;
	}

	private void Update()
	{
		current_real_time = Time.realtimeSinceStartup;
		delta_real_time = current_real_time - last_real_time;
		last_real_time = current_real_time;
		if (rest_time < 2f)
		{
			rest_time += delta_real_time;
			return;
		}
		foreach (UIControl key in SpringControls.Keys)
		{
			key.Rect = new Rect(key.Rect.x, key.Rect.y + spring_velocity, key.Rect.width, key.Rect.height);
		}
		spring_velocity -= 20f * delta_real_time;
		if (!(spring_velocity <= 0f - spring_force))
		{
			return;
		}
		foreach (UIControl key2 in SpringControls.Keys)
		{
			key2.Rect = new Rect(key2.Rect.x, SpringControls[key2], key2.Rect.width, key2.Rect.height);
		}
		spring_force *= 0.5f;
		spring_velocity = spring_force;
		if (spring_force < 1f)
		{
			spring_force = 3f;
			spring_velocity = 3f;
			rest_time = 0f;
		}
	}

	public void RemoveControl(UIControl control)
	{
		if (SpringControls.ContainsKey(control))
		{
			SpringControls.Remove(control);
		}
	}

	public void AddControl(UIControl control)
	{
		if (!SpringControls.ContainsKey(control))
		{
			SpringControls.Add(control, control.Rect.y);
		}
	}

	public void AddControl(List<UIControl> controls)
	{
		if (controls == null)
		{
			return;
		}
		foreach (UIControl control in controls)
		{
			if (!SpringControls.ContainsKey(control))
			{
				SpringControls.Add(control, control.Rect.y);
			}
		}
	}
}
