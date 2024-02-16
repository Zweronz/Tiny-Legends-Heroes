public class AureoleConfig
{
	public enum AureoleFaction
	{
		FRIEND = 0,
		FRIEND_EXCLUDE_ME = 1,
		ENEMY = 2,
		ALL = 3,
		ALL_EXCLUDE_ME = 4
	}

	public enum AureoleOrigin
	{
		CASTER = 0,
		DEFAULT_TARGET = 1,
		TRIGGER_POINT = 2
	}

	public AureoleFaction aureole_faction;

	public AureoleOrigin aureole_origin;

	public bool bind;

	public string aureole_effect;

	public string aureole_sfx;

	public string mount_point;

	public bool include_puppet_radius;

	public D3DTextFloat aureole_radius;

	public D3DTextFloat aureole_time;

	public float AureoleRadius(int skill_level)
	{
		if (aureole_radius == null || aureole_radius.values.Count == 0)
		{
			return -1f;
		}
		int index = ((skill_level <= aureole_radius.values.Count - 1) ? skill_level : (aureole_radius.values.Count - 1));
		return aureole_radius.values[index];
	}

	public float AureoleTime(int skill_level)
	{
		if (aureole_time == null || aureole_time.values.Count == 0)
		{
			return -1f;
		}
		int index = ((skill_level <= aureole_time.values.Count - 1) ? skill_level : (aureole_time.values.Count - 1));
		return aureole_time.values[index];
	}
}
