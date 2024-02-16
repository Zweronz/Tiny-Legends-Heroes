public class D3DGearLoot
{
	public float loot_odds;

	public string custom_loot_id;

	public D3DFloat[] random_loot_odds;

	public D3DGearLoot()
	{
		loot_odds = 0f;
		custom_loot_id = string.Empty;
		random_loot_odds = new D3DFloat[3];
	}
}
