using System.Collections.Generic;
using UnityEngine;

public class D3DMagicPower
{
	public enum Ratio
	{
		ARMOR = 0,
		HELM = 1,
		BOOTS = 2,
		ONE_H = 3,
		TW0_H = 4,
		SHIELD = 5,
		NECKLACE = 6,
		RING = 7
	}

	public enum StaminaAdjust
	{
		PLATE = 0,
		LEATHER = 1,
		ROBE = 2,
		OTHERS = 3
	}

	public enum Redress
	{
		STA_TO_STR = 0,
		STA_TO_AGI = 1,
		STA_TO_INT = 2,
		STA_TO_SPI = 3,
		COMMON_TO_STA = 4
	}

	private static D3DMagicPower instance;

	public float[] CommonCoe = new float[2];

	public float[] NecklaceCoe = new float[2];

	public float[] RingCoe = new float[2];

	public float MPCoe;

	public float[] PowerRatioCoe = new float[8];

	public float[] StaminaAdjustCoe = new float[4];

	public float[] RedressCoe = new float[5];

	public Dictionary<string, PowerRule> RuleManager = new Dictionary<string, PowerRule>();

	public static D3DMagicPower Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DMagicPower();
			}
			return instance;
		}
	}

	public D3DMagicPowerSaveData RandomMagicPower()
	{
		int num = Random.Range(0, RuleManager.Keys.Count);
		int num2 = 0;
		foreach (string key in RuleManager.Keys)
		{
			if (num2 == num)
			{
				return RuleManager[key].RandomPower();
			}
			num2++;
		}
		return null;
	}
}
