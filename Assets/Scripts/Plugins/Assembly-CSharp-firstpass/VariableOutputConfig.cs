public class VariableOutputConfig
{
	public class ImbibeConfig
	{
		public enum ImbibeType
		{
			PASSIVE = 0,
			BUFF = 1
		}

		public ImbibeType imbibe_type;

		public bool imbibe_to_hp;

		public bool imbibe_to_mp;

		public D3DTextFloat imbibe_percent;

		public float ImbibePercent(int level)
		{
			if (imbibe_percent == null)
			{
				return -1f;
			}
			int index = ((level <= imbibe_percent.values.Count - 1) ? level : (imbibe_percent.values.Count - 1));
			return imbibe_percent.values[index];
		}
	}

	public TargetFaction target_faction;

	public string effect;

	public string sfx;

	public string mount_point;

	public bool outer_play;

	public bool can_dodge;

	public ImbibeConfig imbibe_config;
}
