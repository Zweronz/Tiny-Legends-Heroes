public class TriggerBuff : TriggerCrowdControl
{
	public enum BuffType
	{
		RECEIVE_DAMAGE = 0,
		OUTPUT_DAMAGE = 1,
		ATTACK_INTERVAL = 2,
		MOVE_SPEED = 3,
		ARMOR = 4,
		ATTACK_POWER = 5,
		MAGICAL_POWER = 6,
		CLOAK = 7,
		IMBIBE = 8
	}

	public enum Property
	{
		PLUS_FIXED_VALUE = 0,
		PLUS_PERCENT_VALUE = 1,
		MINUS_FIXED_VALUE = 2,
		MINUS_PERCENT_VALUE = 3
	}

	public BuffType buff_type;

	public Property buff_property;

	public D3DTextFloat buff_value;

	public AureoleConfig aureole_config;

	public float GetValue(int level)
	{
		if (buff_value == null)
		{
			return 0f;
		}
		int index = ((level <= buff_value.values.Count - 1) ? level : (buff_value.values.Count - 1));
		return buff_value.values[index];
	}
}
