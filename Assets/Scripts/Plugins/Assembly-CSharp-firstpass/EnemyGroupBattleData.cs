using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupBattleData
{
	public D3DEnemyGroup temp_group;

	public int spawner_id;

	public int group_level;

	public int kill_require;

	public float battle_spawn_interval;

	public int battle_spawn_limit;

	public int enemy_power;

	public int RetreatMulct;

	public List<D3DEnemyGroupSpawnPhase> spawn_phases;

	public void EstimateRetreatMulct()
	{
		int goldBonus = D3DFormulas.GetGoldBonus(group_level);
		RetreatMulct = goldBonus * kill_require;
		foreach (D3DEnemyGroupSpawnPhase spawn_phase in spawn_phases)
		{
			for (int i = 0; i < spawn_phase.phase_spawn_count; i++)
			{
				D3DPuppetProfile profile = D3DMain.Instance.GetProfile(spawn_phase.DrawAnEnemySpawner().enemy_id);
				RetreatMulct += Mathf.RoundToInt((profile.percent_bonus[1] - 1f) * (float)goldBonus) + profile.fixed_bonus[1];
			}
		}
		RetreatMulct = Mathf.RoundToInt((float)RetreatMulct * D3DBattleRule.Instance.RetreatMulctCoe);
	}
}
