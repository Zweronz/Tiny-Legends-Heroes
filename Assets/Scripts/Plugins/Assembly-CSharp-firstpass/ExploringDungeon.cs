using System.Collections.Generic;
using UnityEngine;

public class ExploringDungeon
{
	public enum FloorTransferType
	{
		WORLD_MAP = 0,
		PREVIOUS = 1,
		NEXT = 2
	}

	public class FloorSpawnerCfg
	{
		public Vector3 spawner_position;

		public GameObject spawner_gameobj;

		public float random_rotation_minY;

		public float random_rotation_maxY;

		public float chest_rotation;
	}

	public class LastTransform
	{
		public Vector3 position;

		public Quaternion rotation;
	}

	public FloorTransferType floor_transfer_type;

	public int current_floor;

	public D3DDungeon dungeon;

	public Dictionary<int, FloorSpawnerCfg> floor_spawn_points;

	public LastTransform player_last_transform;

	public D3DInt player_last_battle_spawner;

	public EnemyGroupBattleData player_battle_group_data;

	public D3DDungeonFloorTreasureChest player_trigger_chest;

	public D3DDungeonFloor CurrentFloor
	{
		get
		{
			return dungeon.dungeon_floors[current_floor - 1];
		}
	}

	public ExploringDungeon()
	{
		floor_transfer_type = FloorTransferType.PREVIOUS;
		current_floor = 0;
		dungeon = null;
		floor_spawn_points = new Dictionary<int, FloorSpawnerCfg>();
		player_last_transform = null;
		player_last_battle_spawner = null;
		player_battle_group_data = null;
		player_trigger_chest = null;
	}

	public void Reset()
	{
		floor_transfer_type = FloorTransferType.PREVIOUS;
		current_floor = 0;
		dungeon = null;
		floor_spawn_points.Clear();
		player_last_transform = null;
		player_last_battle_spawner = null;
		player_battle_group_data = null;
		player_trigger_chest = null;
	}

	public void GetPlayerBattleGroupData(D3DEnemyGroup enemy_group, D3DDungeonFloor floor_infomation, int spawner_id)
	{
		player_battle_group_data = new EnemyGroupBattleData();
		player_battle_group_data.temp_group = enemy_group;
		player_battle_group_data.spawner_id = spawner_id;
		int num = floor_infomation.floor_spawners[spawner_id].fixed_group_level;
		if (num < 0)
		{
			num = floor_infomation.RandomFloorLevel;
		}
		player_battle_group_data.group_level = num;
		player_battle_group_data.enemy_power = floor_infomation.enmey_power;
		player_battle_group_data.kill_require = ((enemy_group.kill_require >= 0) ? enemy_group.kill_require : floor_infomation.floor_battle_kill_require);
		player_battle_group_data.battle_spawn_interval = ((!(enemy_group.battle_spawn_interval < 0f)) ? enemy_group.battle_spawn_interval : floor_infomation.floor_battle_spawn_interval);
		player_battle_group_data.battle_spawn_limit = ((enemy_group.battle_spawn_limit >= 0) ? enemy_group.battle_spawn_limit : floor_infomation.floor_battle_spawn_limit);
		player_battle_group_data.spawn_phases = enemy_group.spawn_phases;
	}

	public void GetPlayerBattleGroupData(D3DEnemyGroup enemy_group, D3DDungeonFloor floor_infomation, int spawner_id, int group_level)
	{
		player_battle_group_data = new EnemyGroupBattleData();
		player_battle_group_data.temp_group = enemy_group;
		player_battle_group_data.spawner_id = spawner_id;
		player_battle_group_data.group_level = group_level;
		player_battle_group_data.enemy_power = floor_infomation.enmey_power;
		player_battle_group_data.kill_require = ((enemy_group.kill_require >= 0) ? enemy_group.kill_require : floor_infomation.floor_battle_kill_require);
		player_battle_group_data.battle_spawn_interval = ((!(enemy_group.battle_spawn_interval < 0f)) ? enemy_group.battle_spawn_interval : floor_infomation.floor_battle_spawn_interval);
		player_battle_group_data.battle_spawn_limit = ((enemy_group.battle_spawn_limit >= 0) ? enemy_group.battle_spawn_limit : floor_infomation.floor_battle_spawn_limit);
		player_battle_group_data.spawn_phases = enemy_group.spawn_phases;
	}
}
