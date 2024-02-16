using System.Collections.Generic;

public class D3DFontManager
{
	private string font_name;

	private Dictionary<int, float[]> font_spacing_config;

	public string FontName
	{
		get
		{
			return font_name;
		}
	}

	public D3DFontManager(string font_name, Dictionary<int, float[]> config)
	{
		this.font_name = font_name;
		font_spacing_config = config;
	}

	public float GetCharSpacing(int font_size)
	{
		if (!font_spacing_config.ContainsKey(font_size))
		{
			return 0f;
		}
		return font_spacing_config[font_size][0];
	}

	public float GetLineSpacing(int font_size)
	{
		if (!font_spacing_config.ContainsKey(font_size))
		{
			return 0f;
		}
		return font_spacing_config[font_size][1];
	}
}
