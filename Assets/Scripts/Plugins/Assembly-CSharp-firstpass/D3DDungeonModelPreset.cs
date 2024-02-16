using System.Collections.Generic;

public class D3DDungeonModelPreset
{
	public class ModelPreset
	{
		public string PresetID;

		public string ModelPostfix;

		public List<string> EnabledObstacles;
	}

	public const string MSK = "U]xcQ0I^lqH]JDyh";

	private static D3DDungeonModelPreset instance;

	private Dictionary<string, ModelPreset> ModelPresetManager;

	public static D3DDungeonModelPreset Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DDungeonModelPreset();
			}
			return instance;
		}
	}

	private D3DDungeonModelPreset()
	{
		ModelPresetManager = new Dictionary<string, ModelPreset>();
	}

	public void AddModelPreset(ModelPreset model_preset)
	{
		ModelPresetManager.Add(model_preset.PresetID, model_preset);
	}

	public ModelPreset GetModelPreset(string preset_id)
	{
		return ModelPresetManager[preset_id];
	}
}
