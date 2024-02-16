using System.Collections.Generic;
using UnityEngine;

public class D3DCustomLoot
{
	public const string LSK = "E8pA*AO18Fu5.^DZ";

	public string LootID;

	public List<string> LootEquipmentIDList;

	public List<D3DFloat> CustomOdds;

	public D3DCustomLoot()
	{
		LootEquipmentIDList = new List<string>();
		CustomOdds = new List<D3DFloat>();
		for (int i = 2; i <= 5; i++)
		{
			CustomOdds.Add(null);
		}
	}

	public D3DEquipment LootCustomEquipment(int loot_level)
	{
		if (LootEquipmentIDList.Count == 0)
		{
			return null;
		}
		List<D3DEquipment> list = new List<D3DEquipment>();
		List<D3DEquipment> list2 = new List<D3DEquipment>();
		List<D3DEquipment> list3 = new List<D3DEquipment>();
		List<D3DEquipment> list4 = new List<D3DEquipment>();
		List<D3DEquipment> list5 = new List<D3DEquipment>();
		foreach (string lootEquipmentID in LootEquipmentIDList)
		{
			if (D3DMain.Instance.CheckEquipmentID(lootEquipmentID))
			{
				D3DEquipment equipment = D3DMain.Instance.GetEquipment(lootEquipmentID);
				if (equipment.equipment_grade == D3DEquipment.EquipmentGrade.NORMAL)
				{
					list.Add(equipment);
				}
				else if (equipment.equipment_grade == D3DEquipment.EquipmentGrade.SUPERIOR)
				{
					list2.Add(equipment);
				}
				else if (equipment.equipment_grade == D3DEquipment.EquipmentGrade.MAGIC)
				{
					list3.Add(equipment);
				}
				else if (equipment.equipment_grade == D3DEquipment.EquipmentGrade.RARE)
				{
					list4.Add(equipment);
				}
				else if (equipment.equipment_grade == D3DEquipment.EquipmentGrade.EX_RARE)
				{
					list5.Add(equipment);
				}
			}
		}
		D3DEquipment d3DEquipment = null;
		if (list5.Count > 0)
		{
			int index = 3;
			float lottery_odds = ((CustomOdds[index] != null) ? CustomOdds[index].value : 0f);
			if (D3DMain.Instance.Lottery(lottery_odds))
			{
				do
				{
					d3DEquipment = list5[Random.Range(0, list5.Count)];
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !D3DMain.Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				return d3DEquipment;
			}
		}
		if (list4.Count > 0)
		{
			int index = 2;
			float lottery_odds = ((CustomOdds[index] != null) ? CustomOdds[index].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.RARE, loot_level));
			if (D3DMain.Instance.Lottery(lottery_odds))
			{
				do
				{
					d3DEquipment = list4[Random.Range(0, list4.Count)];
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !D3DMain.Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				return d3DEquipment;
			}
		}
		if (list3.Count > 0)
		{
			int index = 1;
			float lottery_odds = ((CustomOdds[index] != null) ? CustomOdds[index].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.MAGIC, loot_level));
			if (D3DMain.Instance.Lottery(lottery_odds))
			{
				do
				{
					d3DEquipment = list3[Random.Range(0, list3.Count)];
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !D3DMain.Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				return d3DEquipment;
			}
		}
		if (list2.Count > 0)
		{
			int index = 0;
			float lottery_odds = ((CustomOdds[index] != null) ? CustomOdds[index].value : D3DLootFromula.Instance.GetLootOdds(D3DEquipment.EquipmentGrade.SUPERIOR, loot_level));
			if (D3DMain.Instance.Lottery(lottery_odds))
			{
				do
				{
					d3DEquipment = list2[Random.Range(0, list2.Count)];
				}
				while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !D3DMain.Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
				return d3DEquipment;
			}
		}
		if (list.Count > 0)
		{
			do
			{
				d3DEquipment = list[Random.Range(0, list.Count)];
			}
			while (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.ACCESSORY && !D3DMain.Instance.Lottery(D3DLootFromula.Instance.accessory_adjust));
			return d3DEquipment;
		}
		return null;
	}
}
