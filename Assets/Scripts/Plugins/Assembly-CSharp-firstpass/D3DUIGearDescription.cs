using System.Collections.Generic;
using UnityEngine;

public class D3DUIGearDescription : D3DScrollManager
{
	protected enum PropertyText
	{
		EQUIPED = 0,
		NAME = 1,
		HAND_TYPE = 2,
		CLASS = 3,
		BASIC_INFO = 4,
		POPEDOM_TAG = 5,
		POPEDOM_CLASS = 6,
		REQUIRE_LEVEL = 7
	}

	protected UIText[] property_texts;

	protected List<UIText> extra_texts;

	protected List<UIText> description_texts;

	private UIClickButton compare_button;

	protected Color common_color;

	public UIClickButton CompareButton
	{
		get
		{
			return compare_button;
		}
	}

	public D3DUIGearDescription(UIManager manager, UIHelper helper, Rect camera_view_port)
		: base(manager, helper, camera_view_port)
	{
		common_color = D3DMain.Instance.CommonFontColor;
		property_texts = new UIText[8];
		extra_texts = new List<UIText>();
		description_texts = new List<UIText>();
		for (int i = 0; i <= 7; i++)
		{
			int num = 7;
			if (i == 1)
			{
				num = 8;
			}
			property_texts[i] = new UIText();
			property_texts[i].Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, num), string.Empty, common_color);
			property_texts[i].Visible = false;
			property_texts[i].Enable = false;
			property_texts[i].LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(num * D3DMain.Instance.HD_SIZE);
			property_texts[i].CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(num * D3DMain.Instance.HD_SIZE);
			ui_manager.Add(property_texts[i]);
		}
	}

	public void CreateCompareBtn()
	{
		D3DImageCell imageCell = ui_helper.GetImageCell("duibi");
		compare_button = new UIClickButton();
		compare_button.SetTexture(UIButtonBase.State.Normal, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		compare_button.SetTexture(UIButtonBase.State.Pressed, ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
		float num = 640f / (float)Screen.height;
		cameraTransformRect = new Rect(cameraTransformRect.x * num, cameraTransformRect.y * num, cameraTransformRect.width * num, cameraTransformRect.height * num);
		compare_button.Rect = new Rect(cameraTransformRect.xMax - (float)(36 * D3DMain.Instance.HD_SIZE), cameraTransformRect.yMin + (float)(1 * D3DMain.Instance.HD_SIZE), 31 * D3DMain.Instance.HD_SIZE, 21 * D3DMain.Instance.HD_SIZE);
		ui_manager.Add(compare_button);
	}

	protected UIText GetExtraTextUI(int index)
	{
		if (index > extra_texts.Count - 1)
		{
			UIText uIText = new UIText();
			uIText.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 7), string.Empty, Color.green);
			uIText.Visible = false;
			uIText.Enable = false;
			uIText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(7 * D3DMain.Instance.HD_SIZE);
			uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(7 * D3DMain.Instance.HD_SIZE);
			ui_manager.Add(uIText);
			extra_texts.Add(uIText);
			return uIText;
		}
		return extra_texts[index];
	}

	protected UIText GetDescriptionUI(int index)
	{
		if (index > description_texts.Count - 1)
		{
			UIText uIText = new UIText();
			uIText.Set(ui_helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 7), string.Empty, Color.green);
			uIText.Visible = false;
			uIText.Enable = false;
			uIText.SetColor(common_color);
			uIText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(7 * D3DMain.Instance.HD_SIZE);
			uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(7 * D3DMain.Instance.HD_SIZE);
			ui_manager.Add(uIText);
			description_texts.Add(uIText);
			return uIText;
		}
		return description_texts[index];
	}

	public void UpdateDescriptionInfo(D3DEquipment gear, bool equiped, D3DProfileInstance profile_instance, bool reset_scroll)
	{
		if (gear == null)
		{
			return;
		}
		if (reset_scroll)
		{
			ResetScroll();
		}
		UIText[] array = property_texts;
		foreach (UIText uIText in array)
		{
			uIText.Visible = false;
			uIText.SetColor(common_color);
		}
		foreach (UIText extra_text in extra_texts)
		{
			extra_text.Visible = false;
		}
		foreach (UIText description_text in description_texts)
		{
			description_text.Visible = false;
		}
		float num = camera_default_rect.x + (float)(2 * D3DMain.Instance.HD_SIZE);
		float num2 = camera_default_rect.xMax - (float)(4 * D3DMain.Instance.HD_SIZE);
		float num3 = num2 - num;
		float num4 = 5f * (float)D3DMain.Instance.HD_SIZE;
		float num5 = camera_default_rect.yMax - (float)(2 * D3DMain.Instance.HD_SIZE);
		float num6 = 0f;
		UIText uIText2;
		if (equiped)
		{
			uIText2 = property_texts[0];
			uIText2.SetText("Equipped");
			uIText2.Rect = new Rect(num, num5, num3, 999f);
			num6 = uIText2.GetLinesTotalHeight();
			num5 -= num6;
			uIText2.Rect = new Rect(num, num5, uIText2.GetLinesMaxWidth(), num6);
			uIText2.Visible = true;
			num5 -= uIText2.LineSpacing;
		}
		uIText2 = property_texts[1];
		uIText2.SetText(gear.equipment_name);
		uIText2.Rect = new Rect(num, num5, num3, 999f);
		num6 = uIText2.GetLinesTotalHeight();
		num5 -= num6;
		uIText2.Rect = new Rect(num, num5, uIText2.GetLinesMaxWidth(), num6);
		uIText2.SetColor(D3DMain.Instance.GetEquipmentGradeColor(gear.equipment_grade));
		uIText2.Visible = true;
		num5 -= uIText2.LineSpacing;
		uIText2 = property_texts[2];
		uIText2.SetText(D3DMain.Instance.GetEquipmentTypeDes(gear.equipment_type));
		uIText2.Rect = new Rect(num, num5, num3, 999f);
		num6 = uIText2.GetLinesTotalHeight();
		num5 -= num6;
		uIText2.Rect = new Rect(num, num5, uIText2.GetLinesMaxWidth(), num6);
		uIText2.Visible = true;
		uIText2 = property_texts[3];
		uIText2.AlignStyle = UIText.enAlignStyle.right;
		uIText2.SetText(D3DMain.Instance.GetEquipmentClassDes(gear.equipment_class));
		if (!gear.IsEquipmentUseableImplicit(profile_instance.puppet_class))
		{
			uIText2.SetColor(Color.red);
		}
		uIText2.Rect = new Rect(num, num5, num3, 999f);
		num6 = uIText2.GetLinesTotalHeight();
		uIText2.Rect = new Rect(num2 - uIText2.GetLinesMaxWidth(), num5, uIText2.GetLinesMaxWidth(), num6);
		uIText2.Visible = true;
		num5 -= uIText2.LineSpacing;
		if (gear.equipment_type != D3DEquipment.EquipmentType.ACCESSORY && gear.equipment_type != D3DEquipment.EquipmentType.BELT && gear.equipment_type != D3DEquipment.EquipmentType.WRIST)
		{
			bool flag = false;
			uIText2 = property_texts[4];
			string text = string.Empty;
			if ((gear.equipment_type == D3DEquipment.EquipmentType.ONE_HAND || gear.equipment_type == D3DEquipment.EquipmentType.OFF_HAND || gear.equipment_type == D3DEquipment.EquipmentType.TWO_HAND || gear.equipment_type == D3DEquipment.EquipmentType.BOW_HAND) && gear.equipment_class != D3DEquipment.EquipmentClass.SHIELD)
			{
				flag = true;
			}
			if (flag)
			{
				if (gear.GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_ACCURACY) != null)
				{
					if (string.Empty != text)
					{
						text += "\n";
					}
					text = text + "Damage: " + gear.GetBasicAttributes(D3DEquipment.BasicAttribute.PYH_DAMAGE_ACCURACY).value;
				}
				if (gear.GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_ACCURACY) != null)
				{
					if (string.Empty != text)
					{
						text += "\n";
					}
					text = text + "Magical Damage: " + gear.GetBasicAttributes(D3DEquipment.BasicAttribute.MAG_DAMAGE_ACCURACY).value;
				}
				if (gear.GetBasicAttributes(D3DEquipment.BasicAttribute.INTERVAL) != null)
				{
					if (string.Empty != text)
					{
						text += "\n";
					}
					string text2 = text;
					text = text2 + "Attack Interval: " + gear.GetBasicAttributes(D3DEquipment.BasicAttribute.INTERVAL).value + "s";
				}
			}
			else if (gear.GetBasicAttributes(D3DEquipment.BasicAttribute.ARMOR) != null)
			{
				if (string.Empty != text)
				{
					text += "\n";
				}
				text = text + "Armor: " + gear.GetBasicAttributes(D3DEquipment.BasicAttribute.ARMOR).value;
			}
			uIText2.SetText(text);
			uIText2.Rect = new Rect(num, num5, num3, 999f);
			num6 = uIText2.GetLinesTotalHeight();
			num5 -= num6;
			uIText2.Rect = new Rect(num, num5, uIText2.GetLinesMaxWidth(), num6);
			uIText2.Visible = true;
			num5 -= uIText2.LineSpacing;
		}
		PrcoessExtraAttribute(gear, num, num3, num4, ref num5, ref num6);
		if (gear.explicit_popedom_classes.Count > 0)
		{
			float num7 = 0f;
			uIText2 = property_texts[5];
			uIText2.SetText("Class: ");
			uIText2.Rect = new Rect(num, num5, num3, 999f);
			num6 = uIText2.GetLinesTotalHeight();
			num7 = uIText2.GetLinesMaxWidth();
			uIText2.Rect = new Rect(num, num5 - num6, num7, num6);
			uIText2.Visible = true;
			List<string> list = new List<string>();
			foreach (string explicit_popedom_class in gear.explicit_popedom_classes)
			{
				D3DClass @class = D3DMain.Instance.GetClass(explicit_popedom_class);
				if (@class != null && !list.Contains(@class.class_name))
				{
					list.Add(@class.class_name);
				}
			}
			string text3 = string.Empty;
			foreach (string item in list)
			{
				if (string.Empty != text3)
				{
					text3 += " ";
				}
				text3 += item;
			}
			uIText2 = property_texts[6];
			uIText2.SetText(text3);
			if (!gear.IsEquipmentUseableExplicit(profile_instance.puppet_class))
			{
				uIText2.SetColor(Color.red);
				property_texts[5].SetColor(Color.red);
			}
			uIText2.Rect = new Rect(num, num5, num3 - num7, 999f);
			num6 = uIText2.GetLinesTotalHeight();
			num5 -= num6;
			uIText2.Rect = new Rect(num7, num5, uIText2.GetLinesMaxWidth(), num6);
			uIText2.Visible = true;
			num5 -= uIText2.LineSpacing;
		}
		uIText2 = property_texts[7];
		uIText2.Rect = new Rect(num, num5, num3, 999f);
		num6 = uIText2.GetLinesTotalHeight();
		num5 -= num6;
		uIText2.Rect = new Rect(num, num5, uIText2.GetLinesMaxWidth(), num6);
		uIText2.Visible = true;
		num5 -= num4;
		for (int j = 0; j < gear.description.Count; j++)
		{
			UIText descriptionUI = GetDescriptionUI(j);
			descriptionUI.SetText(gear.description[j]);
			descriptionUI.Rect = new Rect(num, num5, num3, 999f);
			num6 = descriptionUI.GetLinesTotalHeight();
			num5 -= num6;
			descriptionUI.Rect = new Rect(num, num5, descriptionUI.GetLinesMaxWidth(), num6);
			descriptionUI.Visible = true;
			num5 -= descriptionUI.LineSpacing;
		}
		scroll_limit.yMin = num5;
		InitScrollBar();
	}

	private void PrcoessExtraAttribute(D3DEquipment gear, float left_x, float description_width, float area_space, ref float next_text_y, ref float text_rect_height)
	{
		if (gear.extra_attributes.Count <= 0)
		{
			return;
		}
		next_text_y -= area_space;
		int num = 0;
		string[] array = new string[22]
		{
			"Strength", "Agility", "Spirit", "Stamina", "Intelligence", "HP", "MP", "Attack Power", "Magic Power", "Armor",
			"Magic Deffense", "HP Recover", "MP Recover", "Move Speed", "Dodge Rate", "Critical Rate", "Damge Reduce", "Stun Reduce", "Fear Reduce", "Trady Reduce",
			"Stakme Reduce", "Extra EXP"
		};
		Dictionary<string, float> dictionary = new Dictionary<string, float>();
		for (D3DPassiveTrigger.PassiveType passiveType = D3DPassiveTrigger.PassiveType.STR; passiveType <= D3DPassiveTrigger.PassiveType.EXP_UP; passiveType++)
		{
			if (passiveType == D3DPassiveTrigger.PassiveType.MAG_DEF)
			{
				continue;
			}
			D3DFloat extraAttributes = gear.GetExtraAttributes(passiveType, true);
			if (extraAttributes != null)
			{
				List<string> strDescript;
				List<float> fValueList;
				if (gear.ConvertPassiveExtraToUnique(passiveType, out strDescript, out fValueList))
				{
					for (int i = 0; i < strDescript.Count; i++)
					{
						if (dictionary.ContainsKey(strDescript[i]))
						{
							float value = dictionary[strDescript[i]] + fValueList[i];
							dictionary[strDescript[i]] = value;
						}
						else
						{
							dictionary.Add(strDescript[i], fValueList[i]);
						}
					}
				}
				num++;
			}
			extraAttributes = gear.GetExtraAttributes(passiveType, false);
			if (extraAttributes != null)
			{
				num++;
				DebugX.LogError("Not implemented!");
			}
		}
		int num2 = num;
		foreach (string key in dictionary.Keys)
		{
			float ex_value = dictionary[key];
			num2++;
			DoAddExtraAttributeUI(left_x, description_width, ref next_text_y, ref text_rect_height, ex_value, num2, key, false);
		}
		next_text_y -= area_space;
	}

	private void DoAddExtraAttributeUI(float left_x, float description_width, ref float next_text_y, ref float text_rect_height, float ex_value, int ex_index, string strName, bool bPercent, bool bTestColor = false)
	{
		string empty = string.Empty;
		bool flag = true;
		UIText extraTextUI = GetExtraTextUI(ex_index);
		ex_value = ((!(ex_value < 1f)) ? ex_value : 1f);
		Color color;
		if (ex_value >= 0f)
		{
			empty = ((!bPercent) ? ("+" + ex_value + " " + strName) : ("+" + ex_value * 100f + "% " + strName));
			color = ((!bTestColor) ? new Color(0.11764706f, 50f / 51f, 0f) : new Color(0f, 0f, 0f, 180f));
		}
		else
		{
			empty = ((!bPercent) ? (ex_value + " " + strName) : (ex_value * 100f + "% " + strName));
			color = Color.red;
		}
		extraTextUI.SetText(empty);
		extraTextUI.SetColor(color);
		extraTextUI.Rect = new Rect(left_x, next_text_y, description_width, 999f);
		text_rect_height = extraTextUI.GetLinesTotalHeight();
		next_text_y -= text_rect_height;
		extraTextUI.Rect = new Rect(left_x, next_text_y, extraTextUI.GetLinesMaxWidth(), text_rect_height);
		extraTextUI.Visible = true;
		next_text_y -= extraTextUI.LineSpacing;
	}

	protected override void UpdateScrollBar()
	{
		base.UpdateScrollBar();
		if (compare_button != null)
		{
			Rect cameraTransformRect = ui_manager.GetCameraTransformRect();
			float num = 640f / (float)Screen.height;
			cameraTransformRect = new Rect(cameraTransformRect.x * num, cameraTransformRect.y * num, cameraTransformRect.width * num, cameraTransformRect.height * num);
			compare_button.Rect = new Rect(cameraTransformRect.xMax - (float)(36 * D3DMain.Instance.HD_SIZE), cameraTransformRect.yMin + (float)(1 * D3DMain.Instance.HD_SIZE), 31 * D3DMain.Instance.HD_SIZE, 21 * D3DMain.Instance.HD_SIZE);
		}
	}
}
