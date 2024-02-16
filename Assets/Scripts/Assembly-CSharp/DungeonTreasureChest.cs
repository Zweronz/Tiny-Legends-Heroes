using System;
using UnityEngine;

public class DungeonTreasureChest : MonoBehaviour
{
	private D3DDungeonFloorTreasureChest TreasureChest;

	private D3DTreasure TreasureLoot;

	private SceneDungeon scene_dungeon;

	private PuppetDungeon player;

	private PuppetDungeonPlayerBehaviour player_behaviour;

	private float check_time;

	public void Init(D3DDungeonFloorTreasureChest treasure_chest, SceneDungeon scene_dungeon)
	{
		TreasureChest = treasure_chest;
		ExploringDungeon.FloorSpawnerCfg floorSpawnerCfg = D3DMain.Instance.exploring_dungeon.floor_spawn_points[TreasureChest.SpawnPoint];
		base.transform.position = floorSpawnerCfg.spawner_position;
		base.transform.Rotate(Vector3.up * floorSpawnerCfg.chest_rotation);
		TreasureLoot = D3DMain.Instance.D3DTreasureManager[TreasureChest.treasure_id];
		if (TreasureLoot.big_chest)
		{
			base.transform.localScale = Vector3.one * 2f;
		}
		this.scene_dungeon = scene_dungeon;
		player = scene_dungeon.map_player;
		player_behaviour = scene_dungeon.map_player.GetComponent<PuppetDungeonPlayerBehaviour>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (player_behaviour.Invincibility || scene_dungeon.ui_dungeon.UIActiving)
		{
			return;
		}
		check_time += Time.deltaTime;
		if (check_time > 0.1f)
		{
			check_time = 0f;
			if (D3DMain.Instance.exploring_dungeon.player_battle_group_data == null && D3DMain.Instance.exploring_dungeon.player_trigger_chest == null && D3DPlaneGeometry.CircleIntersectQuads(new Vector2(base.transform.position.x, base.transform.position.z), base.transform.rotation.eulerAngles.y * ((float)Math.PI / 180f), new Vector2(base.transform.localScale.x * 0.7f, base.transform.localScale.z * 0.7f), new Vector2(player.transform.position.x, player.transform.position.z), player.model_builder.TransformCfg.character_controller_cfg.radius))
			{
				D3DMain.Instance.LootEquipments = D3DMain.Instance.LootRandomEquipmentsOnOpenTreasure(TreasureLoot, D3DMain.Instance.exploring_dungeon.CurrentFloor, 3);
				D3DMain.Instance.exploring_dungeon.player_trigger_chest = TreasureChest;
				scene_dungeon.TriggerTreasureChest();
			}
		}
	}
}
