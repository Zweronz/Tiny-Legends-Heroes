using UnityEngine;

public class D3DRespawnRule
{
	private static D3DRespawnRule instance;

	private int _nGoldMultiFactor;

	private int _nMinGoldMultiFactor;

	private int _nMinCrystalCost;

	private int _nExchangeRate;

	private int _nRefreshCount;

	public static D3DRespawnRule Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DRespawnRule();
			}
			return instance;
		}
	}

	public int GoldMultiFactor
	{
		set
		{
			_nGoldMultiFactor = value;
		}
	}

	public int MinGoldMultiFactor
	{
		set
		{
			_nMinGoldMultiFactor = value;
		}
	}

	public int MinCrystalCost
	{
		set
		{
			_nMinCrystalCost = value;
		}
	}

	public int ExchangeRate
	{
		set
		{
			_nExchangeRate = value;
		}
	}

	public int RefreshCount
	{
		get
		{
			if (_nRefreshCount > 0)
			{
				return _nRefreshCount;
			}
			return 1;
		}
		set
		{
			_nRefreshCount = value;
		}
	}

	public void GetCost(float fTimeLeftRatio, int nEnemyValue, out int nGoldNeed, out int nCrystalNeed)
	{
		nGoldNeed = 0;
		nCrystalNeed = 0;
		nGoldNeed = Mathf.CeilToInt((float)(nEnemyValue * _nGoldMultiFactor) * fTimeLeftRatio);
		if (_nExchangeRate <= 0)
		{
			_nExchangeRate = 1;
		}
		nCrystalNeed = Mathf.CeilToInt((float)nGoldNeed / (float)_nExchangeRate);
		if (nGoldNeed < _nMinGoldMultiFactor * nEnemyValue)
		{
			nGoldNeed = _nMinGoldMultiFactor * nEnemyValue;
		}
		if (nCrystalNeed < _nMinCrystalCost)
		{
			nCrystalNeed = _nMinCrystalCost;
		}
	}
}
