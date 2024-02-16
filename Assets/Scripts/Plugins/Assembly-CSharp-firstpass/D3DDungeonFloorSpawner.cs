using UnityEngine;

public class D3DDungeonFloorSpawner
{
	public int spawner_id;

	public bool random_spawn;

	public string group_id;

	public bool use_group_interval;

	public float spawn_interval;

	public bool bShowCDTime;

	public float _fRespawnTimeFromSaved;

	public float spawn_start_time;

	public int fixed_group_level;

	public int spawned_group_level;

	private int _nEnemyValue;

	public int EnemyValue
	{
		get
		{
			return _nEnemyValue;
		}
	}

	public bool ReadySpawn
	{
		get
		{
			if (_fRespawnTimeFromSaved > 0f)
			{
				if (RespawnTimeLeft <= 0f)
				{
					_fRespawnTimeFromSaved = -1f;
					spawn_start_time = 0f - spawn_interval;
				}
				return false;
			}
			return Time.realtimeSinceStartup - spawn_start_time >= spawn_interval;
		}
	}

	public float RespawnTimeLeft
	{
		get
		{
			if (_fRespawnTimeFromSaved > 0f)
			{
				float num = _fRespawnTimeFromSaved - Time.realtimeSinceStartup + spawn_start_time + 1f;
				return _fRespawnTimeFromSaved - Time.realtimeSinceStartup + spawn_start_time + 1f;
			}
			return spawn_interval - Time.realtimeSinceStartup + spawn_start_time + 1f;
		}
	}

	public D3DDungeonFloorSpawner()
	{
		random_spawn = false;
		group_id = string.Empty;
		use_group_interval = false;
		spawn_interval = -1f;
		fixed_group_level = -1;
		spawn_start_time = 0f;
		_fRespawnTimeFromSaved = -1f;
	}

	public void StartSpawnTime(float interval, int nEnemyValue)
	{
		_fRespawnTimeFromSaved = interval;
		spawn_start_time = Time.realtimeSinceStartup;
		_nEnemyValue = nEnemyValue;
	}

	public void UpdateSpawnerData(string random_group_id, int random_group_level)
	{
		if (random_spawn)
		{
			group_id = random_group_id;
		}
		if (use_group_interval)
		{
			spawn_interval = D3DMain.Instance.GetEnemyGroup(group_id).map_spawn_interval;
		}
		spawned_group_level = ((fixed_group_level >= 0) ? fixed_group_level : random_group_level);
	}

	public void StartRespawn(string random_group_id, int random_group_level, int nEnemyValue)
	{
		UpdateSpawnerData(random_group_id, random_group_level);
		spawn_start_time = Time.realtimeSinceStartup;
		_nEnemyValue = nEnemyValue;
	}
}
