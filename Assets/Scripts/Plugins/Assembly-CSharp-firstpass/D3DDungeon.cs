using System.Collections.Generic;

public class D3DDungeon
{
	public const string DSK = "MjZQ*+GRJaGsk0]H";

	public string dungeon_id;

	public string dungeon_name;

	public List<string> dungeon_map_jigsaw;

	public D3DDungeonTown dungeon_town;

	public List<D3DDungeonFloor> dungeon_floors;

	public int explored_level;

	public D3DDungeon()
	{
		dungeon_map_jigsaw = new List<string>();
		dungeon_town = null;
		dungeon_floors = new List<D3DDungeonFloor>();
	}

	public void DungeonFloorsTimeInit()
	{
		foreach (D3DDungeonFloor dungeon_floor in dungeon_floors)
		{
			dungeon_floor.FloorSpawnersTimeInit();
		}
	}
}
