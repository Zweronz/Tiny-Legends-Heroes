using UnityEngine;

public class UIAnimationBehaviour : MonoBehaviour
{
	private D3DUIImageAnimation ui_image_animation;

	private float last_animation_real_time;

	private float current_animation_real_time;

	private float delta_animation_real_time;

	public D3DUIImageAnimation UIImageAnimation
	{
		set
		{
			ui_image_animation = value;
		}
	}

	private void Start()
	{
		last_animation_real_time = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		current_animation_real_time = Time.realtimeSinceStartup;
		delta_animation_real_time = current_animation_real_time - last_animation_real_time;
		last_animation_real_time = current_animation_real_time;
		ui_image_animation.AnimationPlaying(delta_animation_real_time);
	}

	public void StartRealTime()
	{
		last_animation_real_time = Time.realtimeSinceStartup;
	}
}
