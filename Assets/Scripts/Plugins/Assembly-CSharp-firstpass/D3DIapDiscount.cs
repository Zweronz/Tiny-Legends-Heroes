using System;

public class D3DIapDiscount
{
	public enum DiscountState
	{
		LOCKED = 0,
		READY = 1,
		UNLOCKED = 2,
		EXPIRED = 3
	}

	private static D3DIapDiscount instance;

	public long LastPlayDateTick;

	public int CurrentDiscount = -1;

	public DiscountState[] IapDiscountStates = new DiscountState[4];

	public float DiscountCountDownSeconds;

	public long DiscountCountDownDateTick;

	public static D3DIapDiscount Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DIapDiscount();
			}
			return instance;
		}
	}

	public void Default()
	{
		LastPlayDateTick = DateTime.Now.Ticks;
		CurrentDiscount = -1;
		IapDiscountStates = new DiscountState[4];
		DiscountCountDownSeconds = 0f;
		DiscountCountDownDateTick = DateTime.Now.Ticks;
	}

	public void CheckAvailableDiscount()
	{
		if (CurrentDiscount < 0 && Instance.IapDiscountStates[0] == DiscountState.LOCKED)
		{
			double totalHours = (DateTime.Now - new DateTime(Instance.LastPlayDateTick)).TotalHours;
			if (totalHours >= 24.0 && totalHours <= 48.0 && D3DGamer.Instance.GamePlayedTime > 0f)
			{
				CurrentDiscount = 0;
				instance.IapDiscountStates[0] = DiscountState.READY;
			}
		}
	}

	public void CheckCurrentDiscountIsExpiredOnAppStart()
	{
		if (CurrentDiscount >= 0 && instance.IapDiscountStates[CurrentDiscount] == DiscountState.UNLOCKED)
		{
			double num = (DateTime.Now - new DateTime(Instance.DiscountCountDownDateTick)).TotalSeconds;
			if (num < 0.0)
			{
				num = 0.0;
			}
			DiscountCountDownSeconds -= (float)num;
			if (DiscountCountDownSeconds < 0f)
			{
				IapDiscountStates[CurrentDiscount] = DiscountState.EXPIRED;
				CurrentDiscount = -1;
			}
			D3DGamer.Instance.SaveAllData();
		}
	}

	public bool UnlockDiscountInDungeon()
	{
		return false;
	}
}
