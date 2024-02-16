using System;
using UnityEngine;

public class NeverDestroyedScript : MonoBehaviour
{
	private float last_real_time;

	public static UIClickButton DiscountOpenBtn;

	public static UIText DiscountText1;

	public static UIText DiscountText2;

	private bool AppPaused;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		last_real_time = Time.realtimeSinceStartup;
	}

	private void OnApplicationPause(bool pause)
	{
		AppPaused = pause;
		if (pause)
		{
			D3DIapDiscount.Instance.DiscountCountDownDateTick = DateTime.Now.Ticks;
			D3DGamer.Instance.SaveAllData();
		}
		else
		{
			CheckCurrentIapIsExpired();
			D3DIapDiscount.Instance.CheckAvailableDiscount();
		}
	}

	private void Update()
	{
		float num = Time.realtimeSinceStartup - last_real_time;
		last_real_time = Time.realtimeSinceStartup;
		if (!AppPaused)
		{
			D3DGamer.Instance.GamePlayedTime += num;
		}
		if (D3DIapDiscount.Instance.CurrentDiscount < 0 || D3DIapDiscount.Instance.IapDiscountStates[D3DIapDiscount.Instance.CurrentDiscount] != D3DIapDiscount.DiscountState.UNLOCKED)
		{
			return;
		}
		D3DIapDiscount.Instance.DiscountCountDownSeconds -= num;
		int num2 = (int)(D3DIapDiscount.Instance.DiscountCountDownSeconds / 60f);
		int num3 = (int)(D3DIapDiscount.Instance.DiscountCountDownSeconds - (float)(num2 * 60));
		string text = num2.ToString("d2") + ":" + num3.ToString("d2");
		if (D3DIapDiscount.Instance.DiscountCountDownSeconds < 0f)
		{
			D3DIapDiscount.Instance.DiscountCountDownSeconds = 0f;
			D3DIapDiscount.Instance.IapDiscountStates[D3DIapDiscount.Instance.CurrentDiscount] = D3DIapDiscount.DiscountState.EXPIRED;
			D3DIapDiscount.Instance.CurrentDiscount = -1;
			D3DGamer.Instance.SaveAllData();
			if (DiscountText1 != null)
			{
				DiscountText1.Visible = false;
			}
			if (DiscountOpenBtn != null)
			{
				DiscountOpenBtn.Enable = false;
				DiscountOpenBtn.Visible = false;
			}
		}
		if (DiscountText1 != null)
		{
			DiscountText1.SetText(text);
		}
		if (DiscountText2 != null)
		{
			DiscountText2.SetText(text);
		}
	}

	private void CheckCurrentIapIsExpired()
	{
		if (D3DIapDiscount.Instance.CurrentDiscount < 0 || D3DIapDiscount.Instance.IapDiscountStates[D3DIapDiscount.Instance.CurrentDiscount] != D3DIapDiscount.DiscountState.UNLOCKED)
		{
			return;
		}
		int num = (int)(D3DIapDiscount.Instance.DiscountCountDownSeconds / 60f);
		int num2 = (int)(D3DIapDiscount.Instance.DiscountCountDownSeconds - (float)(num * 60));
		string text = num.ToString("d2") + ":" + num2.ToString("d2");
		if (D3DIapDiscount.Instance.DiscountCountDownSeconds <= 0f)
		{
			D3DIapDiscount.Instance.DiscountCountDownSeconds = 0f;
			D3DIapDiscount.Instance.IapDiscountStates[D3DIapDiscount.Instance.CurrentDiscount] = D3DIapDiscount.DiscountState.EXPIRED;
			D3DIapDiscount.Instance.CurrentDiscount = -1;
			D3DGamer.Instance.SaveAllData();
			if (DiscountText1 != null)
			{
				DiscountText1.Visible = false;
			}
			if (DiscountOpenBtn != null)
			{
				DiscountOpenBtn.Enable = false;
				DiscountOpenBtn.Visible = false;
			}
		}
		if (DiscountText1 != null)
		{
			DiscountText1.SetText(text);
		}
		if (DiscountText2 != null)
		{
			DiscountText2.SetText(text);
		}
	}
}
