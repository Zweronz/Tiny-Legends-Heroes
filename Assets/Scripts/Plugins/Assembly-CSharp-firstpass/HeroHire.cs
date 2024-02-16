public class HeroHire
{
	public string puppet_id;

	public int hire_cost;

	public int hire_crystal;

	public int default_level;

	public string unlock_group;

	public D3DGamer.D3DPuppetSaveData ConvertToPuppetSaveData()
	{
		D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = new D3DGamer.D3DPuppetSaveData();
		d3DPuppetSaveData.pupet_profile_id = puppet_id;
		d3DPuppetSaveData.puppet_level = default_level;
		d3DPuppetSaveData.puppet_current_exp = 0;
		d3DPuppetSaveData.battle_puppet = false;
		D3DGamer.D3DEquipmentSaveData[] array = D3DMain.Instance.GetProfile(puppet_id).profile_arms[0];
		d3DPuppetSaveData.puppet_equipments = new D3DGamer.D3DEquipmentSaveData[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				d3DPuppetSaveData.puppet_equipments[i] = null;
			}
			else
			{
				d3DPuppetSaveData.puppet_equipments[i] = array[i].Clone();
			}
		}
		return d3DPuppetSaveData;
	}
}
