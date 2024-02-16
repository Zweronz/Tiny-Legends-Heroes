using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect
{
	public enum FilterFaction
	{
		FRIEND = 0,
		FRIEND_EXCLUDE_ME = 1,
		ENEMY = 2,
		ALL = 3,
		ALL_EXCLUDE_ME = 4
	}

	public class AreaConfig
	{
		public enum AreaShape
		{
			CIRCLE = 0,
			RECT = 1,
			ARENA = 2
		}

		public enum AreaOrigin
		{
			DEFAULT_TARGET = 0,
			TRIGGER_POINT = 1,
			CASTER = 2
		}

		public AreaShape area_shape;

		public Vector2 area_size;

		public AreaOrigin area_origin;

		public Vector2 area_offset;

		public bool include_puppet_radius;

		public Vector2 AreaSize
		{
			get
			{
				if (area_shape == AreaShape.CIRCLE)
				{
					return new Vector2(area_size.x, area_size.x);
				}
				if (area_shape == AreaShape.RECT)
				{
					return area_size;
				}
				return Vector2.one * 20f;
			}
		}
	}

	public FilterFaction filter_faction;

	public bool include_default_target;

	public D3DTextFloat range_description;

	public List<AreaConfig> trigger_areas;

	public AreaConfig GetAreaConfigs(int level)
	{
		int index = ((level <= trigger_areas.Count - 1) ? level : (trigger_areas.Count - 1));
		return trigger_areas[index];
	}
}
