using System.Collections.Generic;
using UnityEngine;

public class D3DEnemyGroupSpawnPhase
{
	public class EnemySpawner
	{
		public string enemy_id;

		public float odds;

		public int level_diff;
	}

	public bool random_spawn;

	public int once_spawn_count;

	public int phase_spawn_count;

	public bool is_wait;

	public int battle_bgm;

	private int order_loop_index;

	public List<EnemySpawner> phase_enemy_spawners;

	public D3DEnemyGroupSpawnPhase()
	{
		once_spawn_count = 1;
		phase_spawn_count = -1;
		order_loop_index = 0;
		battle_bgm = 0;
		phase_enemy_spawners = new List<EnemySpawner>();
	}

	public EnemySpawner DrawAnEnemySpawner()
	{
		EnemySpawner result = null;
		if (random_spawn)
		{
			float value = Random.value;
			float num = 0f;
			float num2 = 0f;
			foreach (EnemySpawner phase_enemy_spawner in phase_enemy_spawners)
			{
				num2 = num + phase_enemy_spawner.odds;
				if (value >= 0f && value <= num2)
				{
					result = phase_enemy_spawner;
					break;
				}
				num = num2;
			}
		}
		else
		{
			result = phase_enemy_spawners[order_loop_index];
			order_loop_index++;
			if (order_loop_index >= phase_enemy_spawners.Count)
			{
				order_loop_index = 0;
			}
		}
		return result;
	}
}
