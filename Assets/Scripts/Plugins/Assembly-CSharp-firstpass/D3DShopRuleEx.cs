using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class D3DShopRuleEx
{
	public class DungeonLevelToEquipLevelManager
	{
		public class Range
		{
			public int rangeLow;

			public int rangeHigh;
		}

		public class DungeonLevelToEquipLevel
		{
			private Range rangeDungeon = new Range();

			private Range rangeEquip = new Range();

			public Range RangeDungeon
			{
				get
				{
					return rangeDungeon;
				}
			}

			public Range RangeEquip
			{
				get
				{
					return rangeEquip;
				}
			}

			public DungeonLevelToEquipLevel(int nDungeonLow, int nDungeonHigh, int nEquipLow, int nEquipHigh)
			{
				rangeDungeon.rangeLow = nDungeonLow;
				rangeDungeon.rangeHigh = nDungeonHigh;
				rangeEquip.rangeLow = nEquipLow;
				rangeEquip.rangeHigh = nEquipHigh;
			}

			public bool IsDungeonInRange(int nLevel)
			{
				if (nLevel >= RangeDungeon.rangeLow && nLevel <= RangeDungeon.rangeHigh)
				{
					return true;
				}
				return false;
			}
		}

		private List<DungeonLevelToEquipLevel> _dungeonEquipList = new List<DungeonLevelToEquipLevel>();

		public Range CalcEquipLevel(int nExploredLevel)
		{
			foreach (DungeonLevelToEquipLevel dungeonEquip in _dungeonEquipList)
			{
				if (dungeonEquip.IsDungeonInRange(nExploredLevel))
				{
					return dungeonEquip.RangeEquip;
				}
			}
			DebugX.LogError("There's no dungeon Range configured for this level:" + nExploredLevel);
			return null;
		}

		public Range CalcEquipLevel()
		{
			int explored_level = D3DMain.Instance.exploring_dungeon.dungeon.explored_level;
			return CalcEquipLevel(explored_level);
		}

		public void AddRange(int nDungeonLow, int nDungeonHigh, int nEquipLow, int nEquipHigh)
		{
			DungeonLevelToEquipLevel item = new DungeonLevelToEquipLevel(nDungeonLow, nDungeonHigh, nEquipLow, nEquipHigh);
			_dungeonEquipList.Add(item);
		}

		public void Test()
		{
			for (int i = 1; i < 40; i++)
			{
				Range range = CalcEquipLevel(i);
				DebugX.LogError("refresh shop explred:" + i + "Low:" + range.rangeLow + "High:" + range.rangeHigh);
			}
		}
	}

	public class SlotRule
	{
		private List<D3DEquipment> _cadidatesTypes = new List<D3DEquipment>();

		private List<float> _fEquipmentProbability = new List<float>();

		private List<float> _fGradeProbability = new List<float>();

		public List<D3DEquipment> CadidatesTypes
		{
			get
			{
				return _cadidatesTypes;
			}
			set
			{
				_cadidatesTypes = value;
			}
		}

		public List<float> EquipmentProbability
		{
			get
			{
				return _fEquipmentProbability;
			}
			set
			{
				_fEquipmentProbability = value;
			}
		}

		public List<float> GradeProbability
		{
			get
			{
				return _fGradeProbability;
			}
			set
			{
				_fGradeProbability = value;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CandidateTypes count:" + _cadidatesTypes.Count + "   ");
			for (int i = 0; i < _cadidatesTypes.Count; i++)
			{
				string text = _cadidatesTypes[i].equipment_type.ToString() + "--" + _cadidatesTypes[i].equipment_class;
				stringBuilder.Append(text + "-" + _fEquipmentProbability[i] + "  ");
			}
			stringBuilder.Append("\n CandidateGrads:");
			foreach (float item in _fGradeProbability)
			{
				stringBuilder.Append(item + "   ");
			}
			return stringBuilder.ToString();
		}
	}

	private enum EquipmentCheckState
	{
		AddtoTail = 0,
		Update = 1,
		Ignore = 2
	}

	private static D3DShopRuleEx instance;

	private int _nShopLevelMinus;

	private int _nBattleRefreshCount;

	public int RestBattleCount;

	public int LevelPhase;

	private List<float> CurrentGradeOdds = new List<float>();

	public List<float> ItemGradeOddsAdjust = new List<float>();

	public List<float> ItemGradeOddsLimit = new List<float>();

	public DungeonLevelToEquipLevelManager dungenLevelToEquipLevel = new DungeonLevelToEquipLevelManager();

	private Dictionary<string, List<D3DEquipment>> _ShopStoreAll = new Dictionary<string, List<D3DEquipment>>();

	public List<int[]> RefreshCost = new List<int[]>();

	private List<Dictionary<string, SlotRule>> _NewSlotRules = new List<Dictionary<string, SlotRule>>();

	public List<D3DEquipment> CurrentIAPShopStore = new List<D3DEquipment>();

	private static Dictionary<D3DEquipment.EquipmentClass, int> _IAPWeaponSortPriority = new Dictionary<D3DEquipment.EquipmentClass, int>();

	private static Dictionary<D3DEquipment.EquipmentType, int> _IAPArmorSortPriority = new Dictionary<D3DEquipment.EquipmentType, int>();

	public static D3DShopRuleEx Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DShopRuleEx();
			}
			return instance;
		}
	}

	public int ShopLevelMinus
	{
		set
		{
			_nShopLevelMinus = value;
		}
	}

	public int BattleRefreshCount
	{
		get
		{
			return _nBattleRefreshCount;
		}
		set
		{
			_nBattleRefreshCount = value;
		}
	}

	public D3DEquipment GetBestWeapon(D3DGamer.D3DPuppetSaveData saveData)
	{
		DungeonLevelToEquipLevelManager.Range range = dungenLevelToEquipLevel.CalcEquipLevel();
		List<D3DEquipment> allEquipmentInLevelRange = D3DMain.Instance.GetAllEquipmentInLevelRange(range.rangeLow, range.rangeHigh);
		List<D3DEquipment> list = new List<D3DEquipment>();
		for (int i = 0; i < allEquipmentInLevelRange.Count; i++)
		{
			if (IsWeaponEquipment(allEquipmentInLevelRange[i].equipment_class) && allEquipmentInLevelRange[i].IsEquipmentUseable(saveData) && allEquipmentInLevelRange[i].equipment_grade == D3DEquipment.EquipmentGrade.RARE && (!saveData.pupet_profile_id.Contains("fighter") || allEquipmentInLevelRange[i].equipment_class == D3DEquipment.EquipmentClass.SHIELD))
			{
				list.Add(allEquipmentInLevelRange[i]);
			}
		}
		if (list.Count == 0)
		{
			for (int j = 0; j < allEquipmentInLevelRange.Count; j++)
			{
				if (IsWeaponEquipment(allEquipmentInLevelRange[j].equipment_class) && allEquipmentInLevelRange[j].IsEquipmentUseable(saveData) && (allEquipmentInLevelRange[j].equipment_grade == D3DEquipment.EquipmentGrade.MAGIC || allEquipmentInLevelRange[j].equipment_grade == D3DEquipment.EquipmentGrade.SUPERIOR))
				{
					list.Add(allEquipmentInLevelRange[j]);
				}
			}
		}
		return FindHighestLevelEquipment(list).Clone();
	}

	public void RefreshShop(int nFaceIndex, int shop_level, string strProfileId, bool bClickRefresh)
	{
		D3DGamer.D3DPuppetSaveData data = D3DGamer.Instance.PlayerBattleTeamData[nFaceIndex];
		if (bClickRefresh)
		{
			RefreshStore(data);
			return;
		}
		bool flag = false;
		if (!D3DGamer.Instance.ShopRefreshStatus.ContainsKey(strProfileId))
		{
			flag = true;
			D3DGamer.Instance.ShopRefreshStatus.Add(strProfileId, true);
		}
		else if (!D3DGamer.Instance.ShopRefreshStatus[strProfileId])
		{
			flag = true;
			D3DGamer.Instance.ShopRefreshStatus[strProfileId] = true;
		}
		if (flag)
		{
			RefreshStore(data);
		}
	}

	public List<D3DEquipment> GetCurrentStore(bool bIsIAP, int nPuppetIndex)
	{
		if (bIsIAP)
		{
			return CurrentIAPShopStore;
		}
		string pupet_profile_id = D3DGamer.Instance.PlayerBattleTeamData[nPuppetIndex].pupet_profile_id;
		return _ShopStoreAll[pupet_profile_id];
	}

	public void RefreshStore(D3DGamer.D3DPuppetSaveData data)
	{
		if (!_ShopStoreAll.ContainsKey(data.pupet_profile_id))
		{
			_ShopStoreAll.Add(data.pupet_profile_id, new List<D3DEquipment>());
		}
		_ShopStoreAll[data.pupet_profile_id].Clear();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		_ShopStoreAll[data.pupet_profile_id] = DoRefreshAStore(data);
	}

	public int[] GetRefreshCost(int shop_level)
	{
		shop_level--;
		if (shop_level < 1)
		{
			shop_level = 1;
		}
		int num = shop_level / LevelPhase;
		if (num > RefreshCost.Count - 1)
		{
			num = RefreshCost.Count - 1;
		}
		return RefreshCost[num];
	}

	public void ResetOdds()
	{
		CurrentGradeOdds.Clear();
		foreach (float item in ItemGradeOddsAdjust)
		{
			float num = item;
			CurrentGradeOdds.Add(0f);
		}
	}

	public void AdjustOdds()
	{
		for (int i = 0; i < CurrentGradeOdds.Count; i++)
		{
			List<float> currentGradeOdds;
			List<float> list = (currentGradeOdds = CurrentGradeOdds);
			int index;
			int index2 = (index = i);
			float num = currentGradeOdds[index];
			list[index2] = num + ItemGradeOddsAdjust[i];
		}
	}

	private bool IsWeaponEquipment(D3DEquipment.EquipmentClass equipmentClass)
	{
		return equipmentClass < D3DEquipment.EquipmentClass.PLATE;
	}

	private bool IsAccessory(D3DEquipment equipment)
	{
		return equipment.equipment_class == D3DEquipment.EquipmentClass.NECKLACE || equipment.equipment_class == D3DEquipment.EquipmentClass.RING;
	}

	private bool IsAmorEquipment(D3DEquipment.EquipmentClass equipmentClass)
	{
		return equipmentClass == D3DEquipment.EquipmentClass.PLATE || equipmentClass == D3DEquipment.EquipmentClass.LEATHER || equipmentClass == D3DEquipment.EquipmentClass.ROBE;
	}

	private bool IsAccessory(D3DEquipment.EquipmentClass equipmentClass)
	{
		return equipmentClass == D3DEquipment.EquipmentClass.RING || equipmentClass == D3DEquipment.EquipmentClass.NECKLACE;
	}

	public int GetResultOfProbablity(List<float> probabilities)
	{
		float num = 0f;
		foreach (float probability in probabilities)
		{
			float num2 = probability;
			num += num2;
		}
		if (num == 0f)
		{
			return -1;
		}
		int result = probabilities.Count - 1;
		for (int num3 = probabilities.Count - 1; num3 >= 0; num3--)
		{
			if (probabilities[num3] > 0f)
			{
				result = num3;
				break;
			}
		}
		for (int i = 0; i < probabilities.Count - 1; i++)
		{
			float lottery_odds = probabilities[i] / num;
			if (D3DMain.Instance.Lottery(lottery_odds))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private List<float> FilterWeaponsByClass(SlotRule slotRule, D3DGamer.D3DPuppetSaveData saveData)
	{
		List<float> list = new List<float>();
		for (int i = 0; i < slotRule.CadidatesTypes.Count; i++)
		{
			float item = slotRule.EquipmentProbability[i];
			if (IsWeaponEquipment(slotRule.CadidatesTypes[i].equipment_class))
			{
				D3DPuppetProfile profile = D3DMain.Instance.GetProfile(saveData.pupet_profile_id);
				D3DClass @class = D3DMain.Instance.GetClass(profile.profile_class);
				if (!slotRule.CadidatesTypes[i].IsEquipmentUseableImplicit(@class))
				{
					item = 0f;
				}
			}
			list.Add(item);
		}
		return list;
	}

	private int GetEquipByProi(int i, D3DGamer.D3DPuppetSaveData saveData, List<D3DEquipment> allAvailableEquips, out List<D3DEquipment> equipsToChoose)
	{
		equipsToChoose = new List<D3DEquipment>();
		DungeonLevelToEquipLevelManager.Range range = dungenLevelToEquipLevel.CalcEquipLevel();
		SlotRule slotRuleByLevel = GetSlotRuleByLevel(i, range.rangeLow, range.rangeHigh);
		if (slotRuleByLevel == null)
		{
			return -1;
		}
		List<float> probabilities = FilterWeaponsByClass(slotRuleByLevel, saveData);
		int resultOfProbablity = GetResultOfProbablity(probabilities);
		if (resultOfProbablity == -1)
		{
			return 1;
		}
		List<float> probabilities2 = CalcAdjustOdds(slotRuleByLevel.GradeProbability, CurrentGradeOdds);
		int resultOfProbablity2 = GetResultOfProbablity(probabilities2);
		D3DEquipment d3DEquipment = slotRuleByLevel.CadidatesTypes[resultOfProbablity];
		d3DEquipment.equipment_grade = (D3DEquipment.EquipmentGrade)resultOfProbablity2;
		equipsToChoose = FindEquipsMeetsRequirement(d3DEquipment, saveData, allAvailableEquips);
		return 0;
	}

	private List<D3DEquipment> DoRefreshAStore(D3DGamer.D3DPuppetSaveData saveData)
	{
		List<D3DEquipment> list = new List<D3DEquipment>();
		DungeonLevelToEquipLevelManager.Range range = dungenLevelToEquipLevel.CalcEquipLevel();
		List<D3DEquipment> allEquipmentInLevelRange = D3DMain.Instance.GetAllEquipmentInLevelRange(range.rangeLow, range.rangeHigh);
		for (int i = 0; i < _NewSlotRules.Count; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				List<D3DEquipment> equipsToChoose = new List<D3DEquipment>();
				int num = 6;
				if (equipsToChoose == null || equipsToChoose.Count == 0)
				{
					while (num-- > 0 && equipsToChoose.Count == 0)
					{
						int equipByProi = GetEquipByProi(i, saveData, allEquipmentInLevelRange, out equipsToChoose);
						if (equipByProi == -1)
						{
							break;
						}
					}
				}
				if (equipsToChoose.Count <= 0)
				{
					break;
				}
				D3DEquipment d3DEquipment = FindHighestLevelEquipment(equipsToChoose);
				bool flag = false;
				foreach (D3DEquipment item in list)
				{
					if (d3DEquipment.equipment_id.Equals(item.equipment_id))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					D3DEquipment d3DEquipment2 = d3DEquipment.Clone();
					d3DEquipment2.EndowRandomMagicPower();
					list.Add(d3DEquipment2);
					break;
				}
			}
		}
		return list;
	}

	private D3DEquipment FindAEquipementRandom(List<D3DEquipment> equipsToChoose)
	{
		int index = Random.Range(0, equipsToChoose.Count);
		return equipsToChoose[index];
	}

	private D3DEquipment FindHighestLevelEquipment(List<D3DEquipment> equipsToChoose)
	{
		int num = 0;
		D3DEquipment result = new D3DEquipment();
		foreach (D3DEquipment item in equipsToChoose)
		{
			if (item.require_level > num)
			{
				num = item.require_level;
				result = item;
			}
		}
		return result;
	}

	private List<D3DEquipment> FindEquipsMeetsRequirement(D3DEquipment requirement, D3DGamer.D3DPuppetSaveData saveData, List<D3DEquipment> allAvailableEquips)
	{
		List<D3DEquipment> list = new List<D3DEquipment>();
		if (IsWeaponEquipment(requirement.equipment_class))
		{
			foreach (D3DEquipment allAvailableEquip in allAvailableEquips)
			{
				if (allAvailableEquip.IsEquipmentUseable(saveData) && allAvailableEquip.equipment_class == requirement.equipment_class && allAvailableEquip.equipment_grade == requirement.equipment_grade)
				{
					list.Add(allAvailableEquip);
				}
			}
		}
		if (IsAmorEquipment(requirement.equipment_class))
		{
			D3DPuppetProfile profile = D3DMain.Instance.GetProfile(saveData.pupet_profile_id);
			D3DClass @class = D3DMain.Instance.GetClass(profile.profile_class);
			foreach (D3DEquipment allAvailableEquip2 in allAvailableEquips)
			{
				if (allAvailableEquip2.equipment_type == requirement.equipment_type && allAvailableEquip2.equipment_grade == requirement.equipment_grade && allAvailableEquip2.equipment_class == D3DMain.Instance.D3DEquipmentManager[@class.default_armor].equipment_class)
				{
					list.Add(allAvailableEquip2);
				}
			}
		}
		else if (IsAccessory(requirement.equipment_class))
		{
			foreach (D3DEquipment allAvailableEquip3 in allAvailableEquips)
			{
				if (allAvailableEquip3.equipment_class == requirement.equipment_class && allAvailableEquip3.equipment_grade == requirement.equipment_grade)
				{
					list.Add(allAvailableEquip3);
				}
			}
		}
		return list;
	}

	private List<float> CalcAdjustOdds(List<float> orignalOdds, List<float> adjustFactor)
	{
		List<float> list = new List<float>();
		for (int i = 0; i < orignalOdds.Count; i++)
		{
			float num = 0f;
			if (i > 1 && orignalOdds[i] != 0f)
			{
				num = orignalOdds[i] + adjustFactor[i - 2];
				if (num > ItemGradeOddsLimit[i - 2])
				{
					num = ItemGradeOddsLimit[i - 2];
				}
			}
			else
			{
				num = orignalOdds[i];
			}
			list.Add(num);
		}
		return list;
	}

	private SlotRule GetSlotRuleByLevel(int nIndex, int nEquipLevelLow, int nEquipLevelHigh)
	{
		Dictionary<string, SlotRule> dictionary = _NewSlotRules[nIndex];
		foreach (string key in dictionary.Keys)
		{
			string[] array = key.Split('-');
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			int num3 = int.Parse(array[2]);
			if (nEquipLevelLow >= num2 && nEquipLevelHigh <= num3)
			{
				return dictionary[key];
			}
		}
		return null;
	}

	public void AddSlotRules(SlotRule rule, int nSlotId, int nLevelLow, int nLevelHigh)
	{
		SizeSlotRules(ref _NewSlotRules, nSlotId - 1);
		Dictionary<string, SlotRule> dictionary = _NewSlotRules[nSlotId - 1];
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, SlotRule>();
		}
		string key = MakeSlotKey(nSlotId, nLevelLow, nLevelHigh);
		dictionary.Add(key, rule);
		_NewSlotRules[nSlotId - 1] = dictionary;
	}

	public void TestOutNewRules()
	{
		foreach (Dictionary<string, SlotRule> newSlotRule in _NewSlotRules)
		{
			foreach (string key in newSlotRule.Keys)
			{
			}
		}
	}

	private string MakeSlotKey(int nSlotIndex, int nEquipLevelLow, int nEquipLevelHigh)
	{
		return nSlotIndex + "-" + nEquipLevelLow + "-" + nEquipLevelHigh;
	}

	private void SizeSlotRules(ref List<Dictionary<string, SlotRule>> rules, int nSlotIndex)
	{
		if (rules.Count < nSlotIndex + 1)
		{
			for (int i = rules.Count; i < nSlotIndex + 1; i++)
			{
				rules.Add(null);
			}
		}
	}

	public void BuildPriority(List<D3DEquipment.EquipmentType> eTypes, List<D3DEquipment.EquipmentClass> eClasses)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < eTypes.Count; i++)
		{
			D3DEquipment.EquipmentClass equipmentClass = eClasses[i];
			D3DEquipment.EquipmentType equipmentType = eTypes[i];
			if (IsWeaponEquipment(eClasses[i]))
			{
				_IAPWeaponSortPriority.Add(eClasses[i], num++);
			}
			else if (IsAmorEquipment(eClasses[i]))
			{
				_IAPArmorSortPriority.Add(eTypes[i], num2++);
			}
		}
		_IAPArmorSortPriority.Add(D3DEquipment.EquipmentType.ACCESSORY, _IAPArmorSortPriority.Count - 1);
	}

	private void DivideEquipments(List<D3DEquipment> unSortedEquips, List<D3DEquipment> weapons, List<D3DEquipment> armors)
	{
		for (int i = 0; i < unSortedEquips.Count; i++)
		{
			if (IsWeaponEquipment(unSortedEquips[i].equipment_class))
			{
				weapons.Add(unSortedEquips[i]);
			}
			else
			{
				armors.Add(unSortedEquips[i]);
			}
		}
	}

	private List<D3DEquipment> SortIAPEquipments(List<D3DEquipment> unSortedEquips)
	{
		List<D3DEquipment> list = new List<D3DEquipment>();
		List<D3DEquipment> list2 = new List<D3DEquipment>();
		List<D3DEquipment> list3 = new List<D3DEquipment>();
		DivideEquipments(unSortedEquips, list2, list3);
		SortEquipsList(list2, true);
		SortEquipsList(list3, false);
		list.AddRange(list2);
		list.AddRange(list3);
		return list;
	}

	private void SortEquipsList(List<D3DEquipment> equips, bool bIsWeapons)
	{
		for (int i = 0; i < equips.Count; i++)
		{
			for (int j = 0; j < equips.Count - i - 1; j++)
			{
				D3DEquipment d3DEquipment = equips[j];
				int num = ((!bIsWeapons) ? _IAPArmorSortPriority[equips[j].equipment_type] : _IAPWeaponSortPriority[equips[j].equipment_class]);
				int num2 = ((!bIsWeapons) ? _IAPArmorSortPriority[equips[j + 1].equipment_type] : _IAPWeaponSortPriority[equips[j + 1].equipment_class]);
				if (num > num2)
				{
					D3DEquipment value = equips[j];
					equips[j] = equips[j + 1];
					equips[j + 1] = value;
				}
			}
		}
	}

	public void RefreshIAPEquipByPuppet(D3DProfileInstance profileInstance)
	{
		CurrentIAPShopStore.Clear();
		List<D3DEquipment> equipList = new List<D3DEquipment>();
		foreach (string key in D3DMain.Instance.D3DEquipmentManager.Keys)
		{
			D3DEquipment d3DEquipment = D3DMain.Instance.D3DEquipmentManager[key];
			if (D3DMain.Instance.D3DEquipmentManager[key].equipment_grade == D3DEquipment.EquipmentGrade.IAP && d3DEquipment.IsEquipmentUseable(profileInstance) && (!IsAmorEquipment(d3DEquipment.equipment_class) || d3DEquipment.equipment_class == D3DMain.Instance.D3DEquipmentManager[profileInstance.puppet_class.default_armor].equipment_class))
			{
				UpdateEquipList(d3DEquipment, ref equipList);
			}
		}
		List<D3DEquipment> collection = SortIAPEquipments(equipList);
		CurrentIAPShopStore.AddRange(collection);
	}

	private bool IsTheSamePart(D3DEquipment checkEquip, D3DEquipment compareEquip)
	{
		if (IsWeaponEquipment(checkEquip.equipment_class) || IsAccessory(checkEquip))
		{
			if (checkEquip.equipment_class == compareEquip.equipment_class)
			{
				return true;
			}
			return false;
		}
		if (IsAmorEquipment(checkEquip.equipment_class))
		{
			if (checkEquip.equipment_type == compareEquip.equipment_type)
			{
				return true;
			}
			return false;
		}
		Debug.LogError("IsTheSamePart function failed.");
		return false;
	}

	private void UpdateEquipList(D3DEquipment checkEquip, ref List<D3DEquipment> equipList)
	{
		EquipmentCheckState equipmentCheckState = EquipmentCheckState.AddtoTail;
		for (int i = 0; i < equipList.Count; i++)
		{
			D3DEquipment d3DEquipment = equipList[i];
			if (IsTheSamePart(checkEquip, d3DEquipment))
			{
				if (checkEquip.require_level > d3DEquipment.require_level)
				{
					equipList[i] = checkEquip;
					equipmentCheckState = EquipmentCheckState.Update;
				}
				else
				{
					equipmentCheckState = EquipmentCheckState.Ignore;
				}
				break;
			}
		}
		if (equipmentCheckState == EquipmentCheckState.AddtoTail)
		{
			equipList.Add(checkEquip);
		}
	}
}
