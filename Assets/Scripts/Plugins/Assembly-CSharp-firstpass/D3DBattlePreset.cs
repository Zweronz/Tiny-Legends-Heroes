using System.Collections.Generic;
using UnityEngine;

public class D3DBattlePreset
{
	public class SpawnerConfig
	{
		public List<string> ModlePostfix = new List<string>();

		public List<Vector2[]> SpawnerLine = new List<Vector2[]>();
	}

	private static D3DBattlePreset instance;

	public Vector2[] HeroPoints = new Vector2[3];

	public SpawnerConfig DefaultSpawnerConfig;

	public List<SpawnerConfig> SpawnerConfigList = new List<SpawnerConfig>();

	public Dictionary<string, string> CustomSpawnEffect = new Dictionary<string, string>();

	public static D3DBattlePreset Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DBattlePreset();
			}
			return instance;
		}
	}

	public SpawnerConfig GetSpawnerConfig(string model_postfix)
	{
		foreach (SpawnerConfig spawnerConfig in SpawnerConfigList)
		{
			if (spawnerConfig.ModlePostfix.Contains(model_postfix))
			{
				return spawnerConfig;
			}
		}
		return DefaultSpawnerConfig;
	}
}
