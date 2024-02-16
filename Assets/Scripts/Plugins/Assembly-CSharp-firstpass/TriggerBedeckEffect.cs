using System.Collections.Generic;
using UnityEngine;

public class TriggerBedeckEffect
{
	public enum BedeckType
	{
		ONE_SHOT = 0,
		LIFE_CYCLE = 1
	}

	public enum BedeckPlayer
	{
		CASTER = 0,
		TRIGGER_POINT = 1,
		DEFAULT_TARGET = 2
	}

	public string effect_name;

	public BedeckType bedeck_type;

	public BedeckPlayer bedeck_player;

	public float delay_time;

	public bool auto_size;

	public bool bind;

	public string mount_point;

	public Vector3 effect_offset;

	public bool include_puppet_radius;

	public List<string> sfx_list;
}
