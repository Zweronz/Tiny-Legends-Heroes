using System.Collections.Generic;

public class D3DLootFromula
{
	private static D3DLootFromula instance;

	public float accessory_adjust = 0.3f;

	private List<float[]> loot_coe;

	public static D3DLootFromula Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DLootFromula();
			}
			return instance;
		}
	}

	public void InitLootFormula()
	{
		loot_coe = new List<float[]>();
		for (int i = 2; i <= 4; i++)
		{
			float[] item = new float[3];
			loot_coe.Add(item);
		}
	}

	public void SetFormulaCoe(int index, float[] coe)
	{
		loot_coe[index] = coe;
	}

	public float GetLootOdds(D3DEquipment.EquipmentGrade equipment_grade, int loot_level)
	{
		int index = (int)(equipment_grade - 2);
		return loot_coe[index][0] * (float)loot_level * (float)loot_level + loot_coe[index][1] * (float)loot_level + loot_coe[index][2];
	}
}
