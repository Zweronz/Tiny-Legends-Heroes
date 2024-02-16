using System.Collections.Generic;
using UnityEngine;

public class TriggerEffect
{
	public enum EffectType
	{
		COMMON = 0,
		PERIOD = 1,
		ORNAMENT = 2
	}

	public enum EffectPlayer
	{
		MYSELF = 0,
		FRIEND = 1,
		FRIEND_EXCLUDE_ME = 2,
		ENEMY = 3,
		ALL = 4,
		ALL_EXCLUDE_ME = 5,
		TARGET_POINT = 6
	}

	public enum EffectOrigin
	{
		MY_CENTER = 0,
		MY_OUTERRING = 1
	}

	public string effect_name;

	public bool miss_play;

	public EffectPlayer effect_player;

	public bool bind;

	public bool bind_rotate;

	public bool passive_play;

	public EffectOrigin effect_origin;

	public Vector3 effect_offset;

	public float effect_delay;

	public bool auto_size;

	public List<Vector3> size_adjust;

	public List<string> effect_random_sfx;

	public List<string> mount_points;

	public TriggerEffect()
	{
		effect_name = string.Empty;
	}

	public Vector3 GetSizeAdjust(int level)
	{
		if (size_adjust == null)
		{
			return Vector3.one;
		}
		int index = ((level <= size_adjust.Count - 1) ? level : (size_adjust.Count - 1));
		return size_adjust[index];
	}

	public string GetRandomSfx()
	{
		if (effect_random_sfx == null || effect_random_sfx.Count == 0)
		{
			return string.Empty;
		}
		return effect_random_sfx[Random.Range(0, effect_random_sfx.Count)];
	}
}
