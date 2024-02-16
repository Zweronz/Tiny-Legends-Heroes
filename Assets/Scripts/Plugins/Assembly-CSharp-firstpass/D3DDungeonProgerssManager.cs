using System.Collections.Generic;

public class D3DDungeonProgerssManager
{
	public class LevelProgress
	{
		public class NextLevelBattleUnlock
		{
			public string on_battle_start_story;

			public string on_battle_win_story;

			public bool start_read;

			public bool win_read;

			public string target_group;

			public bool win_target;
		}

		public string on_first_enter_story;

		public bool read;

		public Dictionary<int, NextLevelBattleUnlock> UnlockBattleList = new Dictionary<int, NextLevelBattleUnlock>();
	}

	public const string PSK = "QK9iz0Ue4A1C6z3t";

	private static D3DDungeonProgerssManager instance;

	public Dictionary<string, Dictionary<int, LevelProgress>> DungeonProgressManager = new Dictionary<string, Dictionary<int, LevelProgress>>();

	public Dictionary<int, LevelProgress> CurrentDungeonProgress;

	public static D3DDungeonProgerssManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DDungeonProgerssManager();
			}
			return instance;
		}
	}
}
