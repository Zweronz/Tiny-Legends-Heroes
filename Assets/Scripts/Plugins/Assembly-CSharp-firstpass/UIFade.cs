using System;
using UnityEngine;

public class UIFade : MonoBehaviour
{
	public enum FadeState
	{
		IDLE = 0,
		FADE_IN = 1,
		FADE_OUT = 2
	}

	public delegate void OnFadeComplete();

	private UIHelper ui_helper;

	private UIImage fade_img;

	private float fade_angle;

	private float fade_speed = 150f;

	private FadeState fade_state;

	private float last_fade_real_time;

	private float current_fade_real_time;

	private float delta_fade_real_time;

	private bool fade_destroy;

	private OnFadeComplete onFadeComplete;

	public float FadeSpeed
	{
		set
		{
			fade_speed = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (fade_state == FadeState.IDLE)
		{
			return;
		}
		current_fade_real_time = Time.realtimeSinceStartup;
		delta_fade_real_time = current_fade_real_time - last_fade_real_time;
		last_fade_real_time = current_fade_real_time;
		if (fade_state == FadeState.FADE_OUT)
		{
			fade_img.SetAlpha(Mathf.Sin(fade_angle * ((float)Math.PI / 180f)));
		}
		else
		{
			fade_img.SetAlpha(Mathf.Cos(fade_angle * ((float)Math.PI / 180f)));
		}
		fade_angle += fade_speed * delta_fade_real_time;
		if (fade_angle > 90f)
		{
			fade_state = FadeState.IDLE;
			if (fade_destroy)
			{
				ui_helper.isFadeing = false;
				ui_helper.ui_fade = null;
				ui_helper.RemoveUIManager(GetComponent<UIManager>());
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (onFadeComplete != null)
			{
				onFadeComplete();
			}
		}
	}

	public void Init(UIImage image, Color fade_color, bool alpha_zero)
	{
		ui_helper = base.transform.parent.GetComponent<UIHelper>();
		if (image == null || null == ui_helper)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		image.Enable = true;
		image.Visible = true;
		image.SetColor(fade_color);
		if (alpha_zero)
		{
			image.SetAlpha(0f);
		}
		fade_img = image;
	}

	public void Init(UIImage image, FadeState fade_state, Color fade_color, OnFadeComplete onFadeCompleteDelegate, bool destroy)
	{
		ui_helper = base.transform.parent.GetComponent<UIHelper>();
		if (image == null || null == ui_helper)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ui_helper.isFadeing = true;
		image.Enable = true;
		image.Visible = true;
		image.SetColor(fade_color);
		if (fade_state == FadeState.FADE_OUT)
		{
			image.SetAlpha(0f);
		}
		fade_img = image;
		this.fade_state = fade_state;
		fade_destroy = destroy;
		onFadeComplete = onFadeCompleteDelegate;
		last_fade_real_time = Time.realtimeSinceStartup;
	}

	public void Init(UIImage image, FadeState fade_state, Color fade_color, OnFadeComplete onFadeCompleteDelegate, bool destroy, float speed)
	{
		ui_helper = base.transform.parent.GetComponent<UIHelper>();
		if (image == null || null == ui_helper)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ui_helper.isFadeing = true;
		image.Enable = true;
		image.Visible = true;
		image.SetColor(fade_color);
		if (fade_state == FadeState.FADE_OUT)
		{
			image.SetAlpha(0f);
		}
		fade_img = image;
		this.fade_state = fade_state;
		fade_destroy = destroy;
		fade_speed = speed;
		onFadeComplete = onFadeCompleteDelegate;
		last_fade_real_time = Time.realtimeSinceStartup;
	}

	public void StartFade(FadeState fade_state, OnFadeComplete onFadeCompleteDelegate, bool destroy)
	{
		if (fade_state == FadeState.FADE_OUT)
		{
			fade_img.SetAlpha(0f);
		}
		else
		{
			fade_img.SetAlpha(1f);
		}
		fade_img.Enable = true;
		fade_img.Visible = true;
		this.fade_state = fade_state;
		fade_destroy = destroy;
		onFadeComplete = onFadeCompleteDelegate;
		fade_angle = 0f;
		last_fade_real_time = Time.realtimeSinceStartup;
	}

	public void Disable()
	{
		fade_img.Visible = false;
	}
}
