using System.Collections.Generic;

public class D3DProgress
{
	public class DungeonProgress
	{
		public class FloorLock
		{
			public int floor_index;

			public string unlock_enemy_group;

			public List<string> unlock_story;
		}

		public string DungeonID;

		public Dictionary<int, FloorLock> DungeonFloorLockList;
	}

	private static D3DProgress instance;

	public Dictionary<string, DungeonProgress> DungeonProgressManager;

	public static D3DProgress Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DProgress();
			}
			return instance;
		}
	}
}
