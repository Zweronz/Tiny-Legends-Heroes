public class D3DPassiveTrigger
{
	public enum PassiveType
	{
		STR = 0,
		AGI = 1,
		SPI = 2,
		STA = 3,
		INT = 4,
		HP = 5,
		MP = 6,
		ATTACK_POWER = 7,
		MAGIC_POWER = 8,
		DEF = 9,
		MAG_DEF = 10,
		HP_RECOVER = 11,
		MP_RECOVER = 12,
		MOVE_SPD = 13,
		DODGE = 14,
		CRITICAL_RATE = 15,
		REDUCE_DMG = 16,
		STUN_RESIST = 17,
		FEAR_RESIST = 18,
		TRADY_RESIST = 19,
		STAKME_RESIST = 20,
		EXP_UP = 21,
		DUAL_WIELD = 100,
		TITAN_POWER = 101
	}

	public enum PassiveDataType
	{
		FIXED_VALUE = 0,
		PERCENT_VALUE = 1
	}

	public PassiveType passive_type;

	public D3DPassiveTrigger()
	{
		passive_type = PassiveType.STR;
	}

	~D3DPassiveTrigger()
	{
	}
}
