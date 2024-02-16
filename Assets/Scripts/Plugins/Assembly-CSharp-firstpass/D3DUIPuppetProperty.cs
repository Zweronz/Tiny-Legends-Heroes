using UnityEngine;

public class D3DUIPuppetProperty : D3DScrollManager
{
	private enum PropertyText
	{
		NAME = 0,
		CLASS = 1,
		LEVEL = 2,
		EXP = 3,
		HP = 4,
		MP = 5,
		PYH_DAMAGE = 6,
		MAGIC_DAMAGE = 7,
		DEFENSE = 8,
		ATTACK_POWER = 9,
		MAGIC_POWER = 10,
		ATK_INTERVAL = 11,
		MOVE_SPD = 12,
		STR = 13,
		AGI = 14,
		SPI = 15,
		STA = 16,
		INT = 17,
		POPEDOM_ARMOR = 18,
		POPEDOM_WEAPON = 19
	}

	private UIText[] tag_texts;

	private UIText[] value_texts;

	private Color tag_color;

	private Color value_color;

	public D3DUIPuppetProperty(UIManager manager, UIHelper helper, Rect camera_view_port)
		: base(manager, helper, camera_view_port)
	{
		tag_color = D3DMain.Instance.CommonFontColor;
		value_color = Color.white;
		tag_texts = new UIText[20];
		value_texts = new UIText[16];
		for (int i = 0; i <= 19; i++)
		{
			int num = 7;
			if (i == 0)
			{
				num = 9;
			}
			tag_texts[i] = new UIText();
			tag_texts[i].Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, num), string.Empty, tag_color);
			tag_texts[i].Visible = false;
			tag_texts[i].Enable = false;
			tag_texts[i].LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(num * D3DMain.Instance.HD_SIZE);
			tag_texts[i].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(num * D3DMain.Instance.HD_SIZE);
			ui_manager.Add(tag_texts[i]);
			int num2 = i - 4;
			if (num2 >= 0)
			{
				value_texts[num2] = new UIText();
				value_texts[num2].Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, num), string.Empty, value_color);
				value_texts[num2].Visible = false;
				value_texts[num2].Enable = false;
				value_texts[num2].LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(num * D3DMain.Instance.HD_SIZE);
				value_texts[num2].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(num * D3DMain.Instance.HD_SIZE);
				ui_manager.Add(value_texts[num2]);
			}
		}
	}

	public void UpdatePropertyInfo(D3DProfileInstance profile_instance, bool reset_scroll)
	{
		if (!(null == ui_manager))
		{
			if (reset_scroll)
			{
				ResetScroll();
			}
			UIText[] array = tag_texts;
			foreach (UIText uIText in array)
			{
				uIText.Visible = false;
				uIText.SetColor(tag_color);
			}
			UIText[] array2 = value_texts;
			foreach (UIText uIText2 in array2)
			{
				uIText2.Visible = false;
				uIText2.SetColor(value_color);
			}
			profile_instance.InitProperties();
			float num = camera_default_rect.x + (float)(4 * D3DMain.Instance.HD_SIZE);
			float num2 = camera_default_rect.xMax - (float)(7 * D3DMain.Instance.HD_SIZE);
			float width = num2 - num;
			float num3 = 10f * (float)D3DMain.Instance.HD_SIZE;
			float yMax = camera_default_rect.yMax;
			float num4 = 0f;
			float num5 = 0f;
			UIText uIText3 = tag_texts[0];
			uIText3.SetText(profile_instance.ProfileName);
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num, yMax, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[1];
			uIText3.SetText(profile_instance.puppet_class.class_name);
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num, yMax, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[2];
			uIText3.SetText("Level " + profile_instance.puppet_level);
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num, yMax, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[3];
			if (profile_instance.IsLevelMax())
			{
				uIText3.SetText("EXP --/--");
			}
			else
			{
				uIText3.SetText("EXP " + profile_instance.current_exp + " / " + D3DFormulas.ConvertLevelUpExp(profile_instance.puppet_level));
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num, yMax, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[4];
			uIText3.SetText("HP");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[0];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.hp.ToString());
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[5];
			if (profile_instance.puppet_class.sp_class)
			{
				uIText3.SetText("SP");
			}
			else
			{
				uIText3.SetText("MP");
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[1];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.mp.ToString());
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[6];
			uIText3.SetText("Damage");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[2];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			string empty = string.Empty;
			string text = empty;
			empty = text + Mathf.Round((D3DFormulas.GetPhysicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.main_weapon_physical_dmg[0] + profile_instance.puppet_property.puppet_physical_dmg[0]) * profile_instance.puppet_property.dual_adjust) + " - " + Mathf.Round((D3DFormulas.GetPhysicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.main_weapon_physical_dmg[1] + profile_instance.puppet_property.puppet_physical_dmg[1]) * profile_instance.puppet_property.dual_adjust);
			if (profile_instance.puppet_property.sub_weapon_physical_dmg != null)
			{
				text = empty;
				empty = text + "\n" + Mathf.Round((D3DFormulas.GetPhysicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.sub_weapon_physical_dmg[0] + profile_instance.puppet_property.puppet_physical_dmg[0]) * profile_instance.puppet_property.dual_adjust) + " - " + Mathf.Round((D3DFormulas.GetPhysicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.sub_weapon_physical_dmg[1] + profile_instance.puppet_property.puppet_physical_dmg[1]) * profile_instance.puppet_property.dual_adjust);
			}
			uIText3.SetText(empty);
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[7];
			uIText3.SetText("Magical Damage");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[3];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			empty = string.Empty;
			text = empty;
			empty = text + Mathf.Round((D3DFormulas.GetMagicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.main_weapon_magical_dmg[0] + profile_instance.puppet_property.puppet_magical_dmg[0]) * profile_instance.puppet_property.dual_adjust) + " - " + Mathf.Round((D3DFormulas.GetMagicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.main_weapon_magical_dmg[1] + profile_instance.puppet_property.puppet_magical_dmg[1]) * profile_instance.puppet_property.dual_adjust);
			if (profile_instance.puppet_property.sub_weapon_magical_dmg != null)
			{
				text = empty;
				empty = text + "\n" + Mathf.Round((D3DFormulas.GetMagicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.sub_weapon_magical_dmg[0] + profile_instance.puppet_property.puppet_magical_dmg[0]) * profile_instance.puppet_property.dual_adjust) + " - " + Mathf.Round((D3DFormulas.GetMagicalDps(profile_instance.puppet_property) * profile_instance.puppet_property.attack_interval + profile_instance.puppet_property.sub_weapon_magical_dmg[1] + profile_instance.puppet_property.puppet_magical_dmg[1]) * profile_instance.puppet_property.dual_adjust);
			}
			uIText3.SetText(empty);
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[8];
			uIText3.SetText("Armor");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[4];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.armor.ToString());
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[9];
			uIText3.SetText("Attack Power");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[5];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.attack_power.ToString());
			if (profile_instance.property_color_state[5] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[5] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[10];
			uIText3.SetText("Magic Power");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[6];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.magic_power.ToString());
			if (profile_instance.property_color_state[6] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[6] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[11];
			uIText3.SetText("Attack Interval");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[7];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.attack_interval.ToString("0.00").Trim('0').Trim('.') + "s");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[12];
			uIText3.SetText("Movement");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[8];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.move_speed.ToString());
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			yMax -= num3;
			uIText3 = tag_texts[13];
			uIText3.SetText("Strength");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[9];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.strength.ToString());
			if (profile_instance.property_color_state[0] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[0] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[14];
			uIText3.SetText("Agility");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[10];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.agility.ToString());
			if (profile_instance.property_color_state[1] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[1] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[15];
			uIText3.SetText("Spirit");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[11];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.spirit.ToString());
			if (profile_instance.property_color_state[2] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[2] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[16];
			uIText3.SetText("Stamina");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[12];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.stamina.ToString());
			if (profile_instance.property_color_state[3] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[3] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			uIText3 = tag_texts[17];
			uIText3.SetText("Intelligence");
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			uIText3.Rect = new Rect(num, yMax - num4, uIText3.GetLinesMaxWidth(), num4);
			uIText3.Visible = true;
			uIText3 = value_texts[13];
			uIText3.AlignStyle = UIText.enAlignStyle.right;
			uIText3.SetText(profile_instance.puppet_property.intelligence.ToString());
			if (profile_instance.property_color_state[4] > 0)
			{
				uIText3.SetColor(new Color(0.02745098f, 0.52156866f, 1f / 15f));
			}
			else if (profile_instance.property_color_state[4] < 0)
			{
				uIText3.SetColor(Color.red);
			}
			uIText3.Rect = new Rect(num, yMax, width, 999f);
			num4 = uIText3.GetLinesTotalHeight();
			yMax -= num4;
			uIText3.Rect = new Rect(num2 - uIText3.GetLinesMaxWidth() - num5, yMax, uIText3.GetLinesMaxWidth() + num5, num4);
			uIText3.Visible = true;
			yMax -= uIText3.LineSpacing;
			scroll_limit.yMin = yMax;
			InitScrollBar();
		}
	}
}
