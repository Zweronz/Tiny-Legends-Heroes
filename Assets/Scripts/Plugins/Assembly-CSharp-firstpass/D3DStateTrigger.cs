public class D3DStateTrigger
{
	public enum TriggerType
	{
		EXTRA_DAMAGE_TRIGGER = 0,
		PHY_DAMAGE_TRIGGER = 100,
		MAG_DAMAGE_TRIGGER = 101,
		HEAL_TRIGGER = 102,
		REVIVAL_TRIGGER = 103,
		ABSORB_HP_TRIGGER = 104,
		SKILL_TRIGGER = 105,
		DISPEL_BUFF_TRIGGER = 106,
		DISPEL_DEBUFF_TRIGGER = 107,
		CHARGE_TRIGGER = 200,
		EXTRA_DAMAGE_BUFF = 300,
		ATTACK_SPEED_CHANGE_BUFF = 301,
		MOVE_SPEED_CHANGE_BUFF = 302,
		ARMOR_CHANGE_BUFF = 303,
		MAG_DEF_BUFF = 304,
		STR_CHANGE_BUFF = 305,
		AGI_CHANGE_BUFF = 306,
		SPI_CHANGE_BUFF = 307,
		STA_CHANGE_BUFF = 308,
		INT_CHANGE_BUFF = 309,
		SNEER_BUFF = 310,
		STUN_BUFF = 311,
		STAKME_BUFF = 312,
		FEAR_BUFF = 313,
		STEAL_BUFF = 314,
		BLEED_BUFF = 315,
		BURN_BUFF = 316,
		STUN_RESIST_BUFF = 317,
		FEAR_RESIST_BUFF = 318,
		TRADY_RESIST_BUFF = 319,
		STR_PASSIVE = 400,
		AGI_PASSIVE = 401,
		SPI_PASSIVE = 402,
		STA_PASSIVE = 403,
		INT_PASSIVE = 404,
		HP_PASSIVE = 405,
		MP_PASSIVE = 406,
		DMG_PASSIVE = 407,
		ARMOR_PASSIVE = 408,
		MAG_DEF_PASSIVE = 409,
		HP_RECOVER_PASSIVE = 410,
		MP_RECOVER_PASSIVE = 411,
		MOVE_SPD_PASSIVE = 412,
		EVASION_PASSIVE = 413,
		REDUCE_DMG_PASSIVE = 414,
		STUN_RESIST_PASSIVE = 415,
		FEAR_RESIST_PASSIVE = 416,
		TRADY_RESIST_PASSIVE = 417,
		EXP_UP_PASSIVE = 418
	}

	public enum TriggerFaction
	{
		ENEMY = 0,
		FRIEND = 1,
		NEUTRAL = 2
	}

	public enum TriggerDataType
	{
		ODDS = 0,
		RADIUS = 1,
		FIXED_VALUE = 2,
		PERCENT_VALUE = 3,
		COUNT = 4,
		TIME = 5,
		INTERVAL = 6
	}

	public TriggerType trigger_type;

	public TriggerFaction trigger_faction;

	public D3DStateTrigger()
	{
		trigger_type = TriggerType.PHY_DAMAGE_TRIGGER;
		trigger_faction = TriggerFaction.ENEMY;
	}

	~D3DStateTrigger()
	{
	}
}