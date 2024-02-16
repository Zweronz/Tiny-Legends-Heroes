using System.Collections.Generic;
using UnityEngine;

public class D3DDungeonFloorTreasureChest
{
	public string treasure_id;

	public float spawn_odds;

	public float spawn_interval;

	public List<int> spawn_points;

	public float _fRespawnTimeFromSaved;

	public float spawn_start_time;

	public bool SpawnedChest;

	public GameObject chest_obj;

	public int SpawnPoint;

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

	public D3DDungeonFloorTreasureChest()
	{
		treasure_id = string.Empty;
		spawn_odds = 0f;
		spawn_interval = 99999f;
		spawn_points = new List<int>();
		spawn_start_time = 0f;
		chest_obj = null;
		_fRespawnTimeFromSaved = -1f;
	}

	public bool SpawnChest()
	{
		float num = Time.realtimeSinceStartup - spawn_start_time;
		int num2 = ((!(spawn_interval > 0f)) ? 1 : ((int)(num / spawn_interval)));
		for (int i = 0; i < num2; i++)
		{
			if (D3DMain.Instance.Lottery(spawn_odds))
			{
				SpawnPoint = spawn_points[Random.Range(0, spawn_points.Count)];
				SpawnedChest = true;
				return true;
			}
		}
		spawn_start_time = Time.realtimeSinceStartup + num - (float)num2 * spawn_interval;
		return false;
	}

	public void StartSpawnTime(float interval)
	{
		_fRespawnTimeFromSaved = interval;
		spawn_start_time = Time.realtimeSinceStartup;
	}

	public void Respawn()
	{
		spawn_start_time = Time.realtimeSinceStartup;
		SpawnedChest = false;
		Object.DestroyImmediate(chest_obj);
	}
}
