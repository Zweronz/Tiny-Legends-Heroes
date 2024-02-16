using System.Collections.Generic;

public class SortEquipComparer : IComparer<D3DEquipment>
{
	public int Compare(D3DEquipment equip, D3DEquipment otherEquip)
	{
		int result = 0;
		if (equip != null && otherEquip != null)
		{
			result = equip.require_level - otherEquip.require_level;
		}
		return result;
	}
}
