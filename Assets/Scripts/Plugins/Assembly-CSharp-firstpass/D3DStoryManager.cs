using System.Collections.Generic;
using UnityEngine;

public class D3DStoryManager
{
	public class Story
	{
		public enum StoryBgm
		{
			NORMAL = 0,
			START = 1,
			END = 2
		}

		public StoryBgm story_bgm;

		public List<StoryPhase> story_phase;
	}

	public class StoryPhase
	{
		public string Illustration;

		public List<List<string>> contents = new List<List<string>>();
	}

	private static D3DStoryManager instance;

	private Dictionary<string, Story> StoryPreset = new Dictionary<string, Story>();

	public static D3DStoryManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DStoryManager();
			}
			return instance;
		}
	}

	public void AddStoryPreset(string id, Story story)
	{
		StoryPreset.Add(id, story);
	}

	public Story GetStoryPreset(string id)
	{
		return StoryPreset[id];
	}

	public string TestRandomStory()
	{
		int num = Random.Range(0, StoryPreset.Count);
		int num2 = 0;
		foreach (string key in StoryPreset.Keys)
		{
			if (num2 == num)
			{
				return key;
			}
			num2++;
		}
		return string.Empty;
	}
}
