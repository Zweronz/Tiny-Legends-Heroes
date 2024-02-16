public class TriggerCrowdControl
{
	public enum ControlType
	{
		STANDSTILL = 0,
		FEAR = 1,
		DURANCE = 2,
		CHICK = 3
	}

	public TargetFaction target_faction;

	public ControlType control_type;

	public D3DTextFloat odds;

	public D3DTextFloat time;

	public string awaken_effect;

	public string awaken_mount_point;

	public string awaken_sfx;

	public string effect;

	public string mount_point;

	public string sfx;

	public float GetOdds(int level)
	{
		if (odds == null)
		{
			return 1f;
		}
		int index = ((level <= odds.values.Count - 1) ? level : (odds.values.Count - 1));
		return odds.values[index];
	}

	public float GetTime(int level)
	{
		if (time == null)
		{
			return -1f;
		}
		int index = ((level <= time.values.Count - 1) ? level : (time.values.Count - 1));
		return time.values[index];
	}
}
