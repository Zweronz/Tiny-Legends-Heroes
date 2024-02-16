using System.Collections.Generic;

public class D3DEnemyGroup
{
	public const string ESK = "]]7WMBmHxiATEEZ[";

	public string group_id;

	public List<D3DGearLoot> Loots;

	public string leader_id;

	public float patrol_radius;

	public float sight_radius;

	public float pursue_radius;

	public float patrol_speed;

	public float pursue_speed;

	public float map_spawn_interval;

	public int kill_require;

	public float battle_spawn_interval;

	public int battle_spawn_limit;

	public List<D3DEnemyGroupSpawnPhase> spawn_phases;

	public D3DEnemyGroup()
	{
		Loots = new List<D3DGearLoot>();
		kill_require = -1;
		battle_spawn_interval = -1f;
		battle_spawn_limit = -1;
		spawn_phases = new List<D3DEnemyGroupSpawnPhase>();
	}
}
