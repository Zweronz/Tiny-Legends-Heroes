using UnityEngine;

public class test_counter : MonoBehaviour
{
	private D3DDungeonFloorSpawner floor_spawner;

	private D3DDungeonFloorTreasureChest treasure_spawner;

	private float delta_time;

	private void Awake()
	{
	}

	private void Update()
	{
		delta_time += Time.deltaTime;
		if (delta_time >= 1f)
		{
			delta_time = 0f;
			float num = 0f;
			if (floor_spawner != null)
			{
				num = floor_spawner.RespawnTimeLeft;
				UpdateTime(num);
			}
			else if (treasure_spawner != null)
			{
				num = treasure_spawner.RespawnTimeLeft;
				UpdateTime(num);
			}
		}
	}

	public void SetSpawner(D3DDungeonFloorSpawner floor_spawner)
	{
		this.floor_spawner = floor_spawner;
		float respawnTimeLeft = floor_spawner.RespawnTimeLeft;
		UpdateTime(respawnTimeLeft);
	}

	public void SetTreasureSpawner(D3DDungeonFloorTreasureChest treasure)
	{
		treasure_spawner = treasure;
		float respawnTimeLeft = treasure.RespawnTimeLeft;
		UpdateTime(respawnTimeLeft);
	}

	private void UpdateTime(float time_left)
	{
		int num = (int)(time_left / 3600f);
		int num2 = (int)((time_left - (float)(num * 3600)) / 60f);
		int num3 = (int)(time_left - (float)(num * 3600) - (float)(num2 * 60));
		GetComponent<TextMesh>().text = num + ":" + num2 + ":" + num3;
	}
}
