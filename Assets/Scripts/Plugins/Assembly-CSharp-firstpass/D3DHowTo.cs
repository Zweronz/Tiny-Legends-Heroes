using System.Collections.Generic;

public class D3DHowTo
{
	public enum TutorialType
	{
		BEGINNER_BATTLE = 0,
		FIRST_LOOT = 1,
		FIRST_GET_LOOT = 2,
		FIRST_IN_CAMP = 3,
		FIRST_ENTER_SKILL = 4,
		FIRST_ENTER_SHOP = 5,
		FIRST_ENTER_TAVERN = 6,
		FIRST_ENTER_STASH = 7,
		FIRST_ENTER_DUNGEON = 8,
		FIRST_TELEPORT = 9,
		FIRST_BOSS_GRAVE = 10,
		FIRST_GETNEWSKILL = 11,
		FIRST_GETNEWSKILLINNTER = 12,
		TYPE_MAX = 13
	}

	public class Tutorial
	{
		public float trigger_delay;

		public float ill_stay;

		public List<string> TutorialIll = new List<string>();
	}

	private static D3DHowTo instance;

	public string NewGameStory = string.Empty;

	private Dictionary<TutorialType, Tutorial> TutorialManager;

	private Dictionary<string, List<string>> HowToCfg;

	public static D3DHowTo Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DHowTo();
			}
			return instance;
		}
	}

	private D3DHowTo()
	{
		TutorialManager = new Dictionary<TutorialType, Tutorial>();
		HowToCfg = new Dictionary<string, List<string>>();
	}

	public void AddTutorial(TutorialType type, Tutorial tutorial)
	{
		TutorialManager.Add(type, tutorial);
	}

	public Tutorial GetTutorial(TutorialType type)
	{
		return TutorialManager[type];
	}

	public void AddHowToCfg(string title, List<string> ills)
	{
		HowToCfg.Add(title, ills);
	}

	public string[] GetHowToTitles()
	{
		string[] array = new string[HowToCfg.Count];
		int num = 0;
		foreach (string key in HowToCfg.Keys)
		{
			array[num] = key;
			num++;
		}
		return array;
	}

	public List<string> GetHowToIlls(string title)
	{
		return HowToCfg[title];
	}
}
