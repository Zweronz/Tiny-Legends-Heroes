using System.Collections.Generic;

public class DungeonNpcConfig
{
	public class LevelNpcConfig
	{
		public List<D3DInteractiveNpc> level_interactive_npc;
	}

	private static DungeonNpcConfig instance;

	private Dictionary<string, Dictionary<int, LevelNpcConfig>> dungeon_npc_config;

	public static DungeonNpcConfig Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DungeonNpcConfig();
			}
			return instance;
		}
	}

	public DungeonNpcConfig()
	{
		dungeon_npc_config = new Dictionary<string, Dictionary<int, LevelNpcConfig>>();
	}

	public void AddNpcConfig(string dungeon_id, Dictionary<int, LevelNpcConfig> npc_config)
	{
		if (!dungeon_npc_config.ContainsKey(dungeon_id))
		{
			dungeon_npc_config.Add(dungeon_id, npc_config);
		}
	}

	public LevelNpcConfig GetNpcConfig(string dungeon_id, int level)
	{
		if (!dungeon_npc_config.ContainsKey(dungeon_id))
		{
			return null;
		}
		if (!dungeon_npc_config[dungeon_id].ContainsKey(level))
		{
			return null;
		}
		return dungeon_npc_config[dungeon_id][level];
	}
}
