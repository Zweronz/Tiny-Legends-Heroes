using System.Collections.Generic;
using UnityEngine;

public class GotTapPointsMono : MonoBehaviour
{
	public static List<D3DCurrencyText> CurrencyTextList = new List<D3DCurrencyText>();

	private void GotTapPoints(int tapPoints)
	{
		if (tapPoints <= 0)
		{
			return;
		}
		D3DGamer.Instance.UpdateCrystal(tapPoints);
		D3DGamer.Instance.SaveAllData();
		int num = 0;
		while (num < CurrencyTextList.Count)
		{
			if (CurrencyTextList[num] == null)
			{
				CurrencyTextList.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		foreach (D3DCurrencyText currencyText in CurrencyTextList)
		{
			currencyText.SetCurrency(D3DGamer.Instance.CurrencyText, D3DGamer.Instance.CrystalText);
		}
	}
}
