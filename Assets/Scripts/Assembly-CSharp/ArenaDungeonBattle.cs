using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDungeonBattle : ArenaBattle
{
	private int battle_bgm = -1;

	private int current_phase_index;

	private int current_phase_spawned_count;

	private bool debug_info;

	private new void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		scene_arena.kill_count = 0;
		scene_arena.kill_require = D3DMain.Instance.exploring_dungeon.player_battle_group_data.kill_require;
		StartCoroutine("TheSpawner");
		battle_bgm = D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawn_phases[current_phase_index].battle_bgm;
		D3DAudioManager.Instance.ArenaAudio = null;
		PlayBattleBGM();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			debug_info = !debug_info;
		}
	}

	protected override void PlayBattleBGM()
	{
		if (battle_bgm != -1 && !(null != D3DAudioManager.Instance.ArenaAudio))
		{
			int num = 15 + battle_bgm;
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio((D3DAudioManager.CommonAudio)num), ref D3DAudioManager.Instance.ArenaAudio, null, false, false);
		}
	}

	public void RestartSpawn()
	{
	}

	public void StopSpawn()
	{
		StopCoroutine("TheSpawner");
	}

	private IEnumerator TheSpawner()
	{
		if (!D3DGamer.Instance.TutorialState[0])
		{
			yield return new WaitForSeconds(1.5f);
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.BEGINNER_BATTLE);
			yield return new WaitForSeconds(1f);
			scene_arena.ui_arena.ShowTutorialNewHeroComin(D3DTutorialHintCfg.DamaoHintData.HintCondition.Start, 4, 0f);
			yield return new WaitForSeconds(2f);
		}
		else
		{
			yield return new WaitForSeconds(2f);
		}
		while (true)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(D3DMain.Instance.exploring_dungeon.player_battle_group_data.battle_spawn_interval);
		}
	}

	public void SpawnEnemy()
	{
		if (scene_arena.kill_count >= scene_arena.kill_require || scene_arena.BattleResult != 0)
		{
			if (!debug_info)
			{
			}
			return;
		}
		D3DEnemyGroupSpawnPhase d3DEnemyGroupSpawnPhase = D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawn_phases[current_phase_index];
		if (d3DEnemyGroupSpawnPhase.phase_spawn_count >= 0 && current_phase_spawned_count >= d3DEnemyGroupSpawnPhase.phase_spawn_count)
		{
			if (debug_info)
			{
			}
			if (current_phase_index < D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawn_phases.Count - 1)
			{
				if (debug_info)
				{
				}
				if (d3DEnemyGroupSpawnPhase.is_wait && (scene_arena.enemyRecycleList.Count != 0 || scene_arena.enemyList.Count != 0))
				{
					if (!debug_info)
					{
					}
					return;
				}
				if (debug_info)
				{
				}
				current_phase_spawned_count = 0;
				current_phase_index++;
				int num = D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawn_phases[current_phase_index].battle_bgm;
				if (battle_bgm != num && null != D3DAudioManager.Instance.ArenaAudio)
				{
					D3DAudioManager.Instance.ArenaAudio.Stop();
					D3DAudioManager.Instance.ArenaAudio = null;
				}
				scene_arena.enemyBodyDestroyer.AddRange(scene_arena.enemyGrave);
				scene_arena.enemyGrave.Clear();
				int num2 = 0;
				while (num2 <= scene_arena.enemyBodyDestroyer.Count - 1)
				{
					if (null == scene_arena.enemyBodyDestroyer[num2])
					{
						scene_arena.enemyBodyDestroyer.RemoveAt(num2);
						continue;
					}
					if (scene_arena.enemyBodyDestroyer[num2].gameObject.active)
					{
						num2++;
						continue;
					}
					Object.Destroy(scene_arena.enemyBodyDestroyer[num2].gameObject);
					scene_arena.enemyBodyDestroyer.RemoveAt(num2);
				}
				return;
			}
		}
		PlayBattleBGM();
		int num3 = d3DEnemyGroupSpawnPhase.once_spawn_count;
		int num4;
		if (d3DEnemyGroupSpawnPhase.phase_spawn_count >= 0)
		{
			num4 = d3DEnemyGroupSpawnPhase.phase_spawn_count - current_phase_spawned_count;
			if (num3 > num4)
			{
				num3 = num4;
			}
		}
		num4 = D3DMain.Instance.exploring_dungeon.player_battle_group_data.battle_spawn_limit - scene_arena.enemyTallyList.Count;
		if (num3 > num4)
		{
			num3 = num4;
		}
		num4 = scene_arena.kill_require - scene_arena.kill_count;
		if (num3 > num4)
		{
			current_phase_spawned_count += num3 - num4;
			num3 = num4;
		}
		if (debug_info)
		{
		}
		if (num3 <= 0)
		{
			if (!debug_info)
			{
			}
			return;
		}
		if (debug_info)
		{
		}
		if (!D3DGamer.Instance.TutorialState[0])
		{
			scene_arena.ui_arena.ShowTutorialNewHeroComin(D3DTutorialHintCfg.DamaoHintData.HintCondition.Spawn, scene_arena.kill_count + 1, 0.5f);
		}
		if (num3 <= 0)
		{
			return;
		}
		List<PuppetArena> spawned_puppets = new List<PuppetArena>();
		for (int i = 0; i < num3; i++)
		{
			D3DEnemyGroupSpawnPhase.EnemySpawner enemySpawner = d3DEnemyGroupSpawnPhase.DrawAnEnemySpawner();
			PuppetArena puppetArena = scene_arena.CreateEnemyPuppet(enemySpawner.enemy_id, D3DMain.Instance.exploring_dungeon.player_battle_group_data.group_level + enemySpawner.level_diff, ref spawned_puppets);
			if (!(null == puppetArena))
			{
				scene_arena.SetEnemyPositionBySpawner(puppetArena);
				current_phase_spawned_count++;
			}
		}
		scene_arena.OnEnemyTallyCountChange(spawned_puppets);
	}
}
