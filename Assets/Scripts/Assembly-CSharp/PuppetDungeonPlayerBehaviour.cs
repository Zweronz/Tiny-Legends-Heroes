using System.Collections;
using UnityEngine;

public class PuppetDungeonPlayerBehaviour : MonoBehaviour
{
	private PuppetDungeon puppet_dungeon;

	private bool invincibility;

	private float check_time;

	public bool Invincibility
	{
		get
		{
			return invincibility;
		}
	}

	private void Awake()
	{
		puppet_dungeon = GetComponent<PuppetDungeon>();
		invincibility = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (invincibility || puppet_dungeon.scene_dungeon.ui_dungeon.UIActiving)
		{
			return;
		}
		check_time += Time.deltaTime;
		if (!(check_time > 0.1f))
		{
			return;
		}
		check_time = 0f;
		if (D3DMain.Instance.exploring_dungeon.player_battle_group_data != null || D3DMain.Instance.exploring_dungeon.player_trigger_chest != null)
		{
			return;
		}
		foreach (PuppetDungeon dungeon_enemy_group in puppet_dungeon.scene_dungeon.dungeon_enemy_groups)
		{
			if (!(Vector3.Distance(base.transform.position, dungeon_enemy_group.transform.position) <= puppet_dungeon.model_builder.TransformCfg.character_controller_cfg.radius + dungeon_enemy_group.model_builder.TransformCfg.character_controller_cfg.radius))
			{
				continue;
			}
			PuppetDungeonEnmeyBehaviour component = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
			puppet_dungeon.CancelMove();
			foreach (PuppetDungeon dungeon_enemy_group2 in puppet_dungeon.scene_dungeon.dungeon_enemy_groups)
			{
				dungeon_enemy_group2.CancelMove();
			}
			D3DMain.Instance.exploring_dungeon.player_last_battle_spawner = new D3DInt(component.floor_spawner_id);
			D3DMain.Instance.exploring_dungeon.GetPlayerBattleGroupData(component.enemy_group, D3DMain.Instance.exploring_dungeon.CurrentFloor, component.floor_spawner_id, component.GroupLevel);
			D3DMain.Instance.exploring_dungeon.player_last_transform = new ExploringDungeon.LastTransform();
			D3DMain.Instance.exploring_dungeon.player_last_transform.position = base.transform.position;
			D3DMain.Instance.exploring_dungeon.player_last_transform.rotation = base.transform.localRotation;
			puppet_dungeon.scene_dungeon.EnterBattle();
			break;
		}
	}

	public IEnumerator EnableInvincibility(float time)
	{
		invincibility = true;
		yield return new WaitForSeconds(time);
		invincibility = false;
	}

	public void SetPlayerInvincibilityOnPortal()
	{
		invincibility = true;
		StopAllCoroutines();
	}
}
