public class D3DPuppetVariableData
{
	public float hp_max;

	public float mp_max;

	public float[] puppet_physical_dmg;

	public float[] puppet_magical_dmg;

	public float[] main_weapon_physical_dmg;

	public float[] sub_weapon_physical_dmg;

	public float[] main_weapon_magical_dmg;

	public float[] sub_weapon_magical_dmg;

	public float attack_power;

	public float magic_power;

	public float physical_dps_dmg;

	public float magical_dps_dmg;

	public float dmg_extra = 1f;

	public float fixed_dmg_extra;

	public float critical_chance;

	public int hatred_send;

	public bool Critical
	{
		get
		{
			return D3DMain.Instance.Lottery(critical_chance);
		}
	}
}
