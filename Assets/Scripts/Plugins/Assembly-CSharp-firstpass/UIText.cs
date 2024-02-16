using System.Collections;
using UnityEngine;

public class UIText : UIControlVisible
{
	public enum enAlignStyle
	{
		left = 0,
		center = 1,
		right = 2
	}

	private string m_Text;

	private Font m_Font;

	private float m_LineSpacing = 1f;

	private float m_CharacterSpacing = 1f;

	private Color m_Color = Color.black;

	private bool m_bIsAutoLine = true;

	private enAlignStyle m_AlignStyle;

	private float cell_width_offset = -2f;

	private float cell_height_offset;

	private ArrayList finalLines = new ArrayList();

	private int last_line_count;

	public Font Font
	{
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;
		}
	}

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			UpdateText();
			SetClip(value);
		}
	}

	public int TextLines
	{
		get
		{
			return finalLines.Count;
		}
	}

	public float CharacterSpacing
	{
		get
		{
			return m_CharacterSpacing;
		}
		set
		{
			m_CharacterSpacing = value;
		}
	}

	public float LineSpacing
	{
		get
		{
			return m_LineSpacing;
		}
		set
		{
			m_LineSpacing = value;
		}
	}

	public enAlignStyle AlignStyle
	{
		get
		{
			return m_AlignStyle;
		}
		set
		{
			m_AlignStyle = value;
		}
	}

	public bool AutoLine
	{
		get
		{
			return m_bIsAutoLine;
		}
		set
		{
			m_bIsAutoLine = value;
		}
	}

	~UIText()
	{
	}

	public void Set(string font, string text, Color color)
	{
		m_Font = mgrFont.Instance().getFont(font);
		m_Color = color;
		m_Text = text;
		UpdateText();
	}

	public void SetColor(Color clr)
	{
		m_Color = clr;
		UpdateText();
	}

	public void SetAlpha(float alpha)
	{
		m_Color.a = alpha;
		UpdateText();
	}

	public float GetAlpha()
	{
		return m_Color.a;
	}

	public void SetFont(string name)
	{
		m_Font = mgrFont.Instance().getFont(name);
		UpdateText();
	}

	public void SetText(string text)
	{
		if (m_Text != text)
		{
			m_Text = text;
			UpdateText();
		}
	}

	public string GetText()
	{
		return m_Text;
	}

	public float GetTextWidth()
	{
		return m_Font.GetTextWidth(m_Text, m_CharacterSpacing) - cell_width_offset;
	}

	public float GetTextWidth(string text)
	{
		return m_Font.GetTextWidth(text, m_CharacterSpacing);
	}

	public float GetLinesMaxWidth()
	{
		float num = 0f;
		for (int i = 0; i < finalLines.Count; i++)
		{
			float textWidth = GetTextWidth((string)finalLines[i]);
			if (num < textWidth)
			{
				num = textWidth;
			}
		}
		return num;
	}

	public float GetCharHeight()
	{
		return m_Font.CellHeight;
	}

	public float GetLinesTotalHeight()
	{
		int count = finalLines.Count;
		if (count < 1)
		{
			return 0f;
		}
		return GetCharHeight() * (float)count + (float)(count - 1) * m_LineSpacing;
	}

	public void AutoTextRect()
	{
		Rect rect = m_Rect;
		float linesTotalHeight = GetLinesTotalHeight();
		Rect = new Rect(rect.x, rect.y - linesTotalHeight + rect.height, GetLinesMaxWidth(), linesTotalHeight);
	}

	public override void Draw()
	{
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Parent.DrawSprite(m_Sprite[i]);
			}
		}
	}

	private void UpdateText()
	{
		m_Sprite = null;
		if (m_Font == null || m_Text == null || m_Text.Length <= 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		finalLines.Clear();
		string[] array = m_Text.Split('\n');
		if (m_bIsAutoLine)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList2 = new ArrayList();
				string[] array2 = array[i].Split(' ');
				string text = string.Empty;
				float num = 0f;
				for (int j = 0; j < array2.Length; j++)
				{
					float textWidth = m_Font.GetTextWidth(array2[j], CharacterSpacing);
					if (num + textWidth <= Rect.width)
					{
						text += array2[j];
						num += textWidth;
					}
					else
					{
						text.Trim();
						if (string.Empty != text)
						{
							arrayList2.Add(text);
						}
						text = array2[j];
						num = textWidth;
					}
					text += " ";
					num += CharacterSpacing;
					num += m_Font.GetTextWidth(" ");
				}
				text.Trim();
				if (string.Empty != text)
				{
					arrayList2.Add(text);
				}
				for (int k = 0; k < arrayList2.Count; k++)
				{
					finalLines.Add(arrayList2[k]);
				}
			}
		}
		else
		{
			for (int l = 0; l < array.Length; l++)
			{
				finalLines.Add(array[l]);
			}
		}
		float num2 = (float)m_Font.CellHeight + LineSpacing;
		int num3 = m_Font.TextureWidth / m_Font.CellWidth;
		last_line_count = 0;
		for (int m = 0; m < finalLines.Count; m++)
		{
			float num4 = 0f;
			for (int n = 0; n < ((string)finalLines[m]).Length; n++)
			{
				char c = ((string)finalLines[m])[n];
				float num5 = m_Font.getCharWidth(c);
				int num6 = c - 32;
				int num7 = num6 % num3;
				int num8 = num6 / num3;
				if (last_line_count != num8)
				{
					last_line_count = num8;
				}
				float left = num7 * m_Font.CellWidth;
				float top = num8 * m_Font.CellHeight;
				TUISprite tUISprite = new TUISprite();
				tUISprite.Position = new Vector2(m_Rect.x + num4 + (float)(m_Font.CellWidth / 2), m_Rect.y + m_Rect.height - (float)(m + 1) * num2 + (float)(m_Font.CellHeight / 2));
				tUISprite.Size = new Vector2(m_Font.CellWidth, m_Font.CellHeight);
				tUISprite.Material = m_Font.getTexture();
				tUISprite.TextureRect = new Rect(left, top, (float)m_Font.CellWidth + cell_width_offset, (float)m_Font.CellHeight + cell_height_offset);
				tUISprite.Color = m_Color;
				if (m_Clip)
				{
					tUISprite.SetClip(m_ClipRect);
				}
				arrayList.Add(tUISprite);
				num4 += num5 + CharacterSpacing;
			}
		}
		if (AlignStyle == enAlignStyle.center)
		{
			int num9 = 0;
			for (int num10 = 0; num10 < finalLines.Count; num10++)
			{
				string text2 = (string)finalLines[num10];
				float num11 = m_Font.GetTextWidth(text2, CharacterSpacing);
				float num12 = (Rect.width - num11) / 2f;
				for (int num13 = 0; num13 < text2.Length; num13++)
				{
					((TUISprite)arrayList[num13 + num9]).Position = new Vector2(((TUISprite)arrayList[num13 + num9]).Position.x + num12, ((TUISprite)arrayList[num13 + num9]).Position.y);
				}
				num9 += text2.Length;
			}
		}
		else if (AlignStyle == enAlignStyle.right)
		{
			int num14 = 0;
			for (int num15 = 0; num15 < finalLines.Count; num15++)
			{
				string text3 = (string)finalLines[num15];
				float num16 = m_Font.GetTextWidth(text3, CharacterSpacing);
				float num17 = Rect.width - num16;
				for (int num18 = 0; num18 < text3.Length; num18++)
				{
					((TUISprite)arrayList[num18 + num14]).Position = new Vector2(((TUISprite)arrayList[num18 + num14]).Position.x + num17, ((TUISprite)arrayList[num18 + num14]).Position.y);
				}
				num14 += text3.Length;
			}
		}
		m_Sprite = new TUISprite[arrayList.Count];
		for (int num19 = 0; num19 < arrayList.Count; num19++)
		{
			m_Sprite[num19] = (TUISprite)arrayList[num19];
		}
	}
}
