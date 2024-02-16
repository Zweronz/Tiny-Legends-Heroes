using UnityEngine;

public class D3DImageCell
{
	public string cell_name;

	public string cell_texture;

	public Rect cell_rect;

	public D3DImageCell()
	{
		cell_name = string.Empty;
		cell_texture = string.Empty;
		cell_rect = new Rect(0f, 0f, 0f, 0f);
	}

	public D3DImageCell(string name, string texture, Rect rect)
	{
		cell_name = name;
		cell_texture = texture;
		cell_rect = rect;
	}
}
