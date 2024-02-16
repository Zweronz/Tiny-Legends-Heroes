using System.Collections.Generic;

public class D3DBattleRule
{
	public class DrawCost
	{
		public int[] cost;

		public int[] dungeon_level;
	}

	private static D3DBattleRule instance;

	public Dictionary<string, List<DrawCost>> dungeon_draw_cost;

	public float[] first_draw_odds;

	public float second_draw_odd;

	public float RetreatMulctCoe;

	public float[] GoldBagCoe;

	public float[] GoldBagOdds;

	public static D3DBattleRule Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DBattleRule();
			}
			return instance;
		}
	}

	public D3DBattleRule()
	{
		dungeon_draw_cost = new Dictionary<string, List<DrawCost>>();
		first_draw_odds = new float[2];
		second_draw_odd = 0f;
		RetreatMulctCoe = 0.2f;
		GoldBagCoe = new float[3] { 0.15f, 0.1f, 0.05f };
		GoldBagOdds = new float[3] { 0.2f, 0.35f, 0.7f };
	}

	public int[] GetDrawCost(string dungeon_id, int current_level)
	{
		List<DrawCost> list = dungeon_draw_cost[dungeon_id];
		foreach (DrawCost item in list)
		{
			if (current_level >= item.dungeon_level[0] && current_level <= item.dungeon_level[1])
			{
				return item.cost;
			}
		}
		return new int[2] { 99999, 99999 };
	}
}
