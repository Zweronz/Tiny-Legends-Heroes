using System.Collections.Generic;

public class D3DTavern
{
	private static D3DTavern instance;

	public Dictionary<string, HeroSynopsis> HeroSynopsisManager = new Dictionary<string, HeroSynopsis>();

	public List<HeroHire> HeroHireManager = new List<HeroHire>();

	public string[] DefaultHeros = new string[3]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	public static D3DTavern Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DTavern();
			}
			return instance;
		}
	}
}
