public class D3DPuppetTalent
{
	public enum TalentAbility
	{
		STR = 0,
		AGI = 1,
		SPI = 2,
		STA = 3,
		INT = 4,
		HP = 5,
		MP = 6,
		ARMOR = 7,
		PHY_DMG = 8,
		MAG_DMG = 9,
		AKT_SPD = 10,
		MOVE_SPD = 11
	}

	public float[] talent_ability;

	public float[,] ability_growth;

	public D3DPuppetTalent()
	{
		talent_ability = new float[12]
		{
			1f, 1f, 1f, 1f, 1f, 10f, 10f, 0f, 0f, 0f,
			1f, 10f
		};
		ability_growth = new float[10, 3];
	}

	public D3DPuppetTalent Clone()
	{
		D3DPuppetTalent d3DPuppetTalent = new D3DPuppetTalent();
		d3DPuppetTalent.talent_ability = talent_ability.Clone() as float[];
		d3DPuppetTalent.ability_growth = ability_growth.Clone() as float[,];
		return d3DPuppetTalent;
	}

	public float GetAbilityByLevel(int level, TalentAbility ability)
	{
		if (ability > TalentAbility.MAG_DMG)
		{
			return 0f;
		}
		level--;
		if (level < 0)
		{
			level = 0;
		}
		return talent_ability[(int)ability] + ability_growth[(int)ability, 0] * (float)level * (float)level * (float)level + ability_growth[(int)ability, 1] * (float)level * (float)level + ability_growth[(int)ability, 2] * (float)level;
	}
}
