using UnityEngine;

public class RespawnTimeCounter : MonoBehaviour
{
	private D3DDungeonFloorSpawner floor_spawner;

	private float delta_time;

	public void RespawnRightnow()
	{
		floor_spawner._fRespawnTimeFromSaved = -1f;
		floor_spawner.spawn_start_time = 0f - floor_spawner.spawn_interval;
	}

	private void Update()
	{
		delta_time += Time.deltaTime;
		if (delta_time >= 1f)
		{
			delta_time = 0f;
			GetComponent<TextMesh>().text = BuildLeftTimeStr();
		}
	}

	public void SetSpawner(D3DDungeonFloorSpawner floor_spawner)
	{
		this.floor_spawner = floor_spawner;
		GetComponent<TextMesh>().text = BuildLeftTimeStr();
	}

	public string BuildLeftTimeStr()
	{
		float leftTime = GetLeftTime();
		int num = (int)(leftTime / 3600f);
		int num2 = (int)((leftTime - (float)(num * 3600)) / 60f);
		int num3 = (int)(leftTime - (float)(num * 3600) - (float)(num2 * 60));
		return num2 + ":" + num3;
	}

	public float GetTolRespawnTime()
	{
		return floor_spawner.spawn_interval;
	}

	public int GetRespawnValue()
	{
		return floor_spawner.EnemyValue;
	}

	public float GetLeftTime()
	{
		if (floor_spawner.RespawnTimeLeft > 0f)
		{
			return floor_spawner.RespawnTimeLeft;
		}
		return 0f;
	}

	public string GetGroupId()
	{
		return floor_spawner.group_id;
	}
}
