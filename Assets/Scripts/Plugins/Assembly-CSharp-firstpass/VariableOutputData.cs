public class VariableOutputData
{
	public TriggerVariable.VariableType variable_type;

	public float phy_value;

	public float mag_value;

	public float hpmax_percent;

	public float mpmax_percent;

	public float critical_chance;

	public int attacker_level;

	public VariableOutputData(TriggerVariable.VariableType variable_type, float phy_value, float mag_value, float hpmax_percent, float mpmax_percent, float critical_chance, int attacker_level)
	{
		this.variable_type = variable_type;
		this.phy_value = phy_value;
		this.mag_value = mag_value;
		this.hpmax_percent = hpmax_percent;
		this.mpmax_percent = mpmax_percent;
		this.critical_chance = critical_chance;
		this.attacker_level = attacker_level;
	}
}
