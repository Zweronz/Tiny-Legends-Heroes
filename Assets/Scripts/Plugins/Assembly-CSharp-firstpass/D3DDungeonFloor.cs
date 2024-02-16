using System.Collections.Generic;
using UnityEngine;

public class D3DDungeonFloor
{
	public int floor_index;

	public Vector2 floor_map_position;

	public bool boss_level;

	public int floor_level_min;

	public int floor_level_max;

	public int floor_loot_min;

	public int floor_loot_max;

	public string floor_model_preset;

	public bool open_room1;

	public bool open_room2;

	public string spawn_points_prefab;

	public int enmey_power;

	public int floor_battle_kill_require;

	public float floor_battle_spawn_interval;

	public int floor_battle_spawn_limit;

	public List<string> floor_random_groups;

	public Dictionary<int, D3DDungeonFloorSpawner> floor_spawners;

	public List<D3DDungeonFloorTreasureChest> floor_treasures;

	public int RandomFloorLevel
	{
		get
		{
			return Random.Range(floor_level_min, floor_level_max + 1);
		}
	}

	public int LootLevelMin
	{
		get
		{
			return floor_level_min + floor_loot_min;
		}
	}

	public int LootLevelMax
	{
		get
		{
			return floor_level_max + floor_loot_max;
		}
	}

	public D3DDungeonFloor()
	{
		floor_map_position = Vector2.zero;
		boss_level = false;
		floor_random_groups = new List<string>();
		floor_spawners = new Dictionary<int, D3DDungeonFloorSpawner>();
		floor_treasures = new List<D3DDungeonFloorTreasureChest>();
	}

	public string GetSpawnGroupID(int spawner_id)
	{
		return (!(string.Empty == floor_spawners[spawner_id].group_id)) ? floor_spawners[spawner_id].group_id : floor_random_groups[Random.Range(0, floor_random_groups.Count)];
	}

	public void FloorSpawnersTimeInit()
	{
		foreach (int key in floor_spawners.Keys)
		{
			floor_spawners[key].spawn_start_time = 0f - floor_spawners[key].spawn_interval;
		}
		foreach (D3DDungeonFloorTreasureChest floor_treasure in floor_treasures)
		{
			floor_treasure.spawn_start_time = 0f - floor_treasure.spawn_interval;
		}
	}

	public void UpdateSpawnerData(int spawner_id)
	{
		if (floor_spawners.ContainsKey(spawner_id) && floor_random_groups.Count != 0)
		{
			floor_spawners[spawner_id].UpdateSpawnerData(floor_random_groups[Random.Range(0, floor_random_groups.Count)], RandomFloorLevel);
		}
	}

	public void StartSpawnerRespawn(int spawner_id, int nEnemyValue)
	{
		if (floor_spawners.ContainsKey(spawner_id) && floor_random_groups.Count != 0)
		{
			floor_spawners[spawner_id].StartRespawn(floor_random_groups[Random.Range(0, floor_random_groups.Count)], RandomFloorLevel, nEnemyValue);
		}
	}
}
