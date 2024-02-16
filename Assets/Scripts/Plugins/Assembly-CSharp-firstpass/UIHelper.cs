using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UIHelper : MonoBehaviour, UIHandler
{
	protected int ui_index;

	protected string m_ui_cfgxml;

	protected string m_ui_material_path;

	protected string m_font_path;

	protected List<UIManager> m_UIManagerRef;

	protected Hashtable m_control_table;

	protected Dictionary<string, D3DImageCell> UIImageCellIndexer;

	public UIFade ui_fade;

	public bool isFadeing;

	public bool HitUI;

	protected int cur_control_id;

	public int Cur_control_id
	{
		get
		{
			return cur_control_id;
		}
		set
		{
			cur_control_id = value;
		}
	}

	public void AddControlToTable(string name, UIControl control)
	{
		m_control_table.Add(name, control);
	}

	public void RemoveControlFromTable(string name)
	{
		m_control_table.Remove(name);
	}

	public int GetControlId(string name)
	{
		if (!m_control_table.ContainsKey(name))
		{
			return -9999;
		}
		return ((UIControl)m_control_table[name]).Id;
	}

	public UIControl GetControl(string name)
	{
		if (!m_control_table.Contains(name))
		{
			return null;
		}
		return (UIControl)m_control_table[name];
	}

	protected void CreateUIManagerEmpty()
	{
		m_UIManagerRef.Add(null);
	}

	protected UIManager CreateUIManager(string manager_name)
	{
		GameObject gameObject = new GameObject(manager_name);
		UIManager uIManager = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef.Add(uIManager);
		uIManager.SetParameter(15, (float)ui_index + (float)(m_UIManagerRef.Count - 1) * 0.02f);
		uIManager.SetUIHandler(this);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward * (m_UIManagerRef.Count - 1) * 3f;
		return uIManager;
	}

	public UIManager InsertUIManager(string manager_name, int index)
	{
		if (index >= m_UIManagerRef.Count || null != m_UIManagerRef[index])
		{
			return null;
		}
		GameObject gameObject = new GameObject(manager_name);
		UIManager uIManager = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef[index] = uIManager;
		uIManager.SetParameter(15, (float)ui_index + (float)index * 0.02f);
		uIManager.SetUIHandler(this);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward * index * 3f;
		return uIManager;
	}

	public UIManager GetManager(int index)
	{
		if (index > m_UIManagerRef.Count)
		{
			return null;
		}
		return m_UIManagerRef[index];
	}

	protected void Awake()
	{
		D3DMain.Instance.D3DUIList.Add(this);
		ui_index = D3DMain.Instance.D3DUIList.Count;
		base.transform.position = new Vector3(1000 * ui_index, 0f, 1000 * ui_index);
		m_UIManagerRef = new List<UIManager>();
		UIImageCellIndexer = new Dictionary<string, D3DImageCell>();
	}

	protected void OnDestroy()
	{
		D3DMain.Instance.D3DUIList.Remove(this);
	}

	public void Start()
	{
		m_control_table = new Hashtable();
		cur_control_id = 0;
		m_ui_material_path = "Dungeons3D/Images/UIImages/";
		m_font_path = "Dungeons3D/Fonts/";
	}

	protected void AddImageCellIndexer(string[] cell_configs)
	{
		foreach (string text in cell_configs)
		{
			D3DMain.Instance.LoadD3DImageCell(ref UIImageCellIndexer, "Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UIImgCell", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt(text, D3DGamer.Instance.Sk[0])));
		}
	}

	protected void AddItemIcons()
	{
		int num = 0;
		while (true)
		{
			TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UIImgCell", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UI_Icons" + num + "_cell", D3DGamer.Instance.Sk[0]))) as TextAsset;
			if (null == textAsset)
			{
				break;
			}
			string text = XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]);
			while (text != string.Empty)
			{
				int num2 = text.IndexOf('\n');
				string text2 = text.Substring(0, num2);
				text = text.Remove(0, num2 + 1);
				num2 = text2.IndexOf('\t');
				string text3 = text2.Substring(0, num2);
				text2 = text2.Remove(0, num2 + 1);
				num2 = text2.IndexOf('\t');
				string texture = text2.Substring(0, num2);
				text2 = text2.Remove(0, num2 + 1);
				string text4 = text2;
				text4 = text4.Trim();
				string[] array = text4.Split(',');
				Rect rect = new Rect(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
				if (string.Empty != text3 && !UIImageCellIndexer.ContainsKey(text3))
				{
					UIImageCellIndexer.Add(text3, new D3DImageCell(text3, texture, rect));
				}
			}
			num++;
		}
	}

	protected void AddSkillIcons()
	{
		int num = 0;
		while (true)
		{
			TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UIImgCell", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UI_SkillIcons" + num + "_cell", D3DGamer.Instance.Sk[0]))) as TextAsset;
			if (null == textAsset)
			{
				break;
			}
			string text = XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]);
			while (text != string.Empty)
			{
				int num2 = text.IndexOf('\n');
				string text2 = text.Substring(0, num2);
				text = text.Remove(0, num2 + 1);
				num2 = text2.IndexOf('\t');
				string text3 = text2.Substring(0, num2);
				text2 = text2.Remove(0, num2 + 1);
				num2 = text2.IndexOf('\t');
				string texture = text2.Substring(0, num2);
				text2 = text2.Remove(0, num2 + 1);
				string text4 = text2;
				text4 = text4.Trim();
				string[] array = text4.Split(',');
				Rect rect = new Rect(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
				if (string.Empty != text3 && !UIImageCellIndexer.ContainsKey(text3))
				{
					UIImageCellIndexer.Add(text3, new D3DImageCell(text3, texture, rect));
				}
			}
			num++;
		}
	}

	public D3DImageCell GetIconCell(string icon_name)
	{
		if (UIImageCellIndexer.ContainsKey(icon_name))
		{
			return UIImageCellIndexer[icon_name];
		}
		return UIImageCellIndexer["item_interrogation"];
	}

	public D3DImageCell GetImageCell(string cell_name)
	{
		return UIImageCellIndexer[cell_name];
	}

	protected void CreateUIByXml(UIManager ui_manager)
	{
		XmlElement xmlElement = null;
		string empty = string.Empty;
		TextAsset textAsset = Resources.Load(m_ui_cfgxml) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]));
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!("UIElem" == childNode.Name))
			{
				continue;
			}
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				xmlElement = (XmlElement)childNode2;
				if ("UIButton" == childNode2.Name)
				{
					UIButtonBase uIButtonBase = null;
					Rect rect = new Rect(0f, 0f, 0f, 0f);
					string[] array;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array = empty.Split(',');
							rect = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array = empty.Split(',');
					empty = xmlElement.GetAttribute("type").Trim();
					if ("click" == empty)
					{
						uIButtonBase = new UIClickButton();
						((UIClickButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("push" == empty)
					{
						uIButtonBase = new UIPushButton();
						((UIPushButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("select" == empty)
					{
						uIButtonBase = new UISelectButton();
						((UISelectButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("wheel" == empty)
					{
						uIButtonBase = new UIWheelButton();
						((UIWheelButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("joystick" == empty)
					{
						uIButtonBase = new UIJoystickButton();
						((UIJoystickButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					if (uIButtonBase == null)
					{
						continue;
					}
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIButtonBase);
					uIButtonBase.Id = cur_control_id;
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 1)
					{
						uIButtonBase.Enable = "true" == empty;
					}
					empty = xmlElement.GetAttribute("visible").Trim();
					if (empty.Length > 1)
					{
						uIButtonBase.Visible = "true" == empty;
					}
					empty = xmlElement.GetAttribute("alpha").Trim();
					if (empty.Length > 1)
					{
						uIButtonBase.SetAlpha(float.Parse(empty));
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Normal");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("rect").Trim();
						array = empty.Split(',');
						empty = xmlElement.GetAttribute("material").Trim();
						uIButtonBase.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(empty), new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE), new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height));
						empty = xmlElement.GetAttribute("RGB").Trim();
						if (empty.Length > 1)
						{
							array = empty.Split(',');
							Color color = new Color(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							uIButtonBase.SetColor(UIButtonBase.State.Normal, color);
						}
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Press");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("rect").Trim();
						array = empty.Split(',');
						empty = xmlElement.GetAttribute("material").Trim();
						uIButtonBase.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(empty), new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE), new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height));
						empty = xmlElement.GetAttribute("RGB").Trim();
						if (empty.Length > 1)
						{
							array = empty.Split(',');
							Color color2 = new Color(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							uIButtonBase.SetColor(UIButtonBase.State.Pressed, color2);
						}
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Disable");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("rect").Trim();
						array = empty.Split(',');
						empty = xmlElement.GetAttribute("material").Trim();
						uIButtonBase.SetTexture(UIButtonBase.State.Disabled, LoadUIMaterialAutoHD(empty), new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE), new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height));
						empty = xmlElement.GetAttribute("RGB").Trim();
						if (empty.Length > 1)
						{
							array = empty.Split(',');
							Color color3 = new Color(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							uIButtonBase.SetColor(UIButtonBase.State.Disabled, color3);
						}
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Hover");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("rect").Trim();
						array = empty.Split(',');
						empty = xmlElement.GetAttribute("material").Trim();
						uIButtonBase.SetHoverSprite(LoadUIMaterialAutoHD(empty), new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE));
					}
					ui_manager.Add(uIButtonBase);
				}
				else if ("UIImage" == childNode2.Name)
				{
					UIImage uIImage = new UIImage();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIImage);
					uIImage.Id = cur_control_id;
					Rect rect2 = new Rect(0f, 0f, 0f, 0f);
					string[] array2;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array2 = empty.Split(',');
							rect2 = new Rect(float.Parse(array2[0].Trim()), float.Parse(array2[1].Trim()), float.Parse(array2[2].Trim()), float.Parse(array2[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array2 = empty.Split(',');
					uIImage.Rect = new Rect(float.Parse(array2[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.x, float.Parse(array2[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.y, float.Parse(array2[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.width, float.Parse(array2[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.height);
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 0)
					{
						uIImage.Enable = "true" == empty;
					}
					empty = xmlElement.GetAttribute("visible").Trim();
					if (empty.Length > 0)
					{
						uIImage.Visible = "true" == empty;
					}
					empty = xmlElement.GetAttribute("RGB").Trim();
					if (empty.Length > 0)
					{
						array2 = empty.Split(',');
						uIImage.SetColor(new Color(float.Parse(array2[0].Trim()) / 255f, float.Parse(array2[1].Trim()) / 255f, float.Parse(array2[2].Trim()) / 255f));
					}
					empty = xmlElement.GetAttribute("alpha").Trim();
					if (empty.Length > 0)
					{
						uIImage.SetAlpha(float.Parse(empty));
					}
					empty = xmlElement.GetAttribute("clip").Trim();
					if (empty.Length > 0)
					{
						array2 = empty.Split(',');
						uIImage.SetClip(new Rect(float.Parse(array2[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.x, float.Parse(array2[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.y, float.Parse(array2[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.width, float.Parse(array2[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.height));
					}
					empty = xmlElement.GetAttribute("rotation").Trim();
					if (empty.Length > 0)
					{
						uIImage.SetRotation(float.Parse(empty));
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Texture");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("rect").Trim();
						array2 = empty.Split(',');
						empty = xmlElement.GetAttribute("material").Trim();
						Rect texture_rect = new Rect(float.Parse(array2[0].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array2[1].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array2[2].Trim()) * (float)D3DMain.Instance.HD_SIZE, float.Parse(array2[3].Trim()) * (float)D3DMain.Instance.HD_SIZE);
						Vector2 size = new Vector2(uIImage.Rect.width, uIImage.Rect.height);
						uIImage.SetTexture(LoadUIMaterialAutoHD(empty), texture_rect, size);
					}
					ui_manager.Add(uIImage);
				}
				else if ("UIText" == childNode2.Name)
				{
					UIText uIText = new UIText();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIText);
					uIText.Id = cur_control_id;
					Rect rect3 = new Rect(0f, 0f, 0f, 0f);
					string[] array3;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array3 = empty.Split(',');
							rect3 = new Rect(float.Parse(array3[0].Trim()), float.Parse(array3[1].Trim()), float.Parse(array3[2].Trim()), float.Parse(array3[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array3 = empty.Split(',');
					uIText.Rect = new Rect(float.Parse(array3[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.x, float.Parse(array3[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.y, float.Parse(array3[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.width, float.Parse(array3[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.height);
					empty = xmlElement.GetAttribute("GameFont1").Trim();
					if (string.Empty != empty)
					{
						array3 = empty.Split(',');
						int num = ((D3DMain.Instance.HD_SIZE != 2) ? int.Parse(array3[0].Trim()) : int.Parse(array3[1].Trim()));
						uIText.SetFont(m_font_path + D3DMain.Instance.GameFont1.FontName + num);
						uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(num);
						uIText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(num);
					}
					else
					{
						empty = xmlElement.GetAttribute("chargap").Trim();
						if (empty.Length > 0)
						{
							uIText.CharacterSpacing = float.Parse(empty) * (float)D3DMain.Instance.HD_SIZE;
						}
						empty = xmlElement.GetAttribute("linegap").Trim();
						if (empty.Length > 0)
						{
							uIText.LineSpacing = float.Parse(empty) * (float)D3DMain.Instance.HD_SIZE;
						}
						uIText.SetFont(string.Concat(str1: (D3DMain.Instance.HD_SIZE != 2) ? xmlElement.GetAttribute("font").Trim() : xmlElement.GetAttribute("fontHD").Trim(), str0: m_font_path));
					}
					empty = xmlElement.GetAttribute("autoline").Trim();
					if (empty.Length > 0)
					{
						uIText.AutoLine = "true" == empty;
					}
					empty = xmlElement.GetAttribute("align").Trim();
					if (empty.Length > 0)
					{
						uIText.AlignStyle = (UIText.enAlignStyle)(int)Enum.Parse(typeof(UIText.enAlignStyle), empty);
					}
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 0)
					{
						uIText.Enable = "true" == empty;
					}
					empty = xmlElement.GetAttribute("visible").Trim();
					if (empty.Length > 0)
					{
						uIText.Visible = "true" == empty;
					}
					empty = xmlElement.GetAttribute("color").Trim();
					if (empty.Length > 0)
					{
						array3 = empty.Split(',');
						uIText.SetColor(new Color(float.Parse(array3[0].Trim()) / 255f, float.Parse(array3[1].Trim()) / 255f, float.Parse(array3[2].Trim()) / 255f, float.Parse(array3[3].Trim()) / 255f));
					}
					uIText.SetText(childNode2.InnerText.Trim(' ', '\t', '\r', '\n'));
					ui_manager.Add(uIText);
				}
				else if ("UIBlock" == childNode2.Name)
				{
					UIBlock uIBlock = new UIBlock();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIBlock);
					uIBlock.Id = cur_control_id;
					Rect rect4 = new Rect(0f, 0f, 0f, 0f);
					string[] array4;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array4 = empty.Split(',');
							rect4 = new Rect(float.Parse(array4[0].Trim()), float.Parse(array4[1].Trim()), float.Parse(array4[2].Trim()), float.Parse(array4[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array4 = empty.Split(',');
					uIBlock.Rect = new Rect(float.Parse(array4[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.x, float.Parse(array4[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.y, float.Parse(array4[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.width, float.Parse(array4[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.height);
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 1)
					{
						uIBlock.Enable = "true" == empty;
					}
					ui_manager.Add(uIBlock);
				}
				else if ("UIMove" == childNode2.Name)
				{
					UIMove uIMove = new UIMove();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIMove);
					uIMove.Id = cur_control_id;
					Rect rect5 = new Rect(0f, 0f, 0f, 0f);
					string[] array5;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array5 = empty.Split(',');
							rect5 = new Rect(float.Parse(array5[0].Trim()), float.Parse(array5[1].Trim()), float.Parse(array5[2].Trim()), float.Parse(array5[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array5 = empty.Split(',');
					uIMove.Rect = new Rect(float.Parse(array5[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.x, float.Parse(array5[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.y, float.Parse(array5[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.width, float.Parse(array5[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.height);
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 1)
					{
						uIMove.Enable = "true" == empty;
					}
					ui_manager.Add(uIMove);
				}
				cur_control_id++;
			}
		}
	}

	public void CreateUIByCellXml(string ui_cfg_xml, UIManager ui_manager)
	{
		XmlElement xmlElement = null;
		string empty = string.Empty;
		TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("UICfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt(ui_cfg_xml, D3DGamer.Instance.Sk[0]))) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[2]));
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!("UIElem" == childNode.Name))
			{
				continue;
			}
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				xmlElement = (XmlElement)childNode2;
				if ("UIButton" == childNode2.Name)
				{
					UIButtonBase uIButtonBase = null;
					Rect rect = new Rect(0f, 0f, 0f, 0f);
					string[] array;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array = empty.Split(',');
							rect = new Rect(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()), float.Parse(array[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array = empty.Split(',');
					empty = xmlElement.GetAttribute("type").Trim();
					if ("click" == empty)
					{
						uIButtonBase = new UIClickButton();
						((UIClickButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("push" == empty)
					{
						uIButtonBase = new UIPushButton();
						((UIPushButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("select" == empty)
					{
						uIButtonBase = new UISelectButton();
						((UISelectButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("wheel" == empty)
					{
						uIButtonBase = new UIWheelButton();
						((UIWheelButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					else if ("joystick" == empty)
					{
						uIButtonBase = new UIJoystickButton();
						((UIJoystickButton)uIButtonBase).Rect = new Rect(float.Parse(array[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.x, float.Parse(array[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.y, float.Parse(array[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.width, float.Parse(array[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect.height);
					}
					if (uIButtonBase == null)
					{
						continue;
					}
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIButtonBase);
					uIButtonBase.Id = cur_control_id;
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 1)
					{
						uIButtonBase.Enable = "true" == empty;
					}
					empty = xmlElement.GetAttribute("visible").Trim();
					if (empty.Length > 1)
					{
						uIButtonBase.Visible = "true" == empty;
					}
					empty = xmlElement.GetAttribute("alpha").Trim();
					if (empty.Length > 1)
					{
						uIButtonBase.SetAlpha(float.Parse(empty));
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Normal");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("cell").Trim();
						D3DImageCell d3DImageCell = UIImageCellIndexer[empty];
						uIButtonBase.SetTexture(UIButtonBase.State.Normal, LoadUIMaterialAutoHD(d3DImageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(d3DImageCell.cell_rect), new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height));
						empty = xmlElement.GetAttribute("RGB").Trim();
						if (empty.Length > 1)
						{
							array = empty.Split(',');
							Color color = new Color(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							uIButtonBase.SetColor(UIButtonBase.State.Normal, color);
						}
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Press");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("cell").Trim();
						D3DImageCell d3DImageCell2 = UIImageCellIndexer[empty];
						uIButtonBase.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterialAutoHD(d3DImageCell2.cell_texture), D3DMain.Instance.ConvertRectAutoHD(d3DImageCell2.cell_rect), new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height));
						empty = xmlElement.GetAttribute("RGB").Trim();
						if (empty.Length > 1)
						{
							array = empty.Split(',');
							Color color2 = new Color(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							uIButtonBase.SetColor(UIButtonBase.State.Pressed, color2);
						}
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Disable");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("cell").Trim();
						D3DImageCell d3DImageCell3 = UIImageCellIndexer[empty];
						uIButtonBase.SetTexture(UIButtonBase.State.Disabled, LoadUIMaterialAutoHD(d3DImageCell3.cell_texture), D3DMain.Instance.ConvertRectAutoHD(d3DImageCell3.cell_rect), new Vector2(uIButtonBase.Rect.width, uIButtonBase.Rect.height));
						empty = xmlElement.GetAttribute("RGB").Trim();
						if (empty.Length > 1)
						{
							array = empty.Split(',');
							Color color3 = new Color(float.Parse(array[0].Trim()), float.Parse(array[1].Trim()), float.Parse(array[2].Trim()));
							uIButtonBase.SetColor(UIButtonBase.State.Disabled, color3);
						}
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Hover");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("cell").Trim();
						D3DImageCell d3DImageCell4 = UIImageCellIndexer[empty];
						uIButtonBase.SetHoverSprite(LoadUIMaterialAutoHD(d3DImageCell4.cell_texture), D3DMain.Instance.ConvertRectAutoHD(d3DImageCell4.cell_rect));
					}
					ui_manager.Add(uIButtonBase);
				}
				else if ("UIImage" == childNode2.Name)
				{
					UIImage uIImage = new UIImage();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIImage);
					uIImage.Id = cur_control_id;
					Rect rect2 = new Rect(0f, 0f, 0f, 0f);
					string[] array2;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array2 = empty.Split(',');
							rect2 = new Rect(float.Parse(array2[0].Trim()), float.Parse(array2[1].Trim()), float.Parse(array2[2].Trim()), float.Parse(array2[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array2 = empty.Split(',');
					uIImage.Rect = new Rect(float.Parse(array2[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.x, float.Parse(array2[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.y, float.Parse(array2[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.width, float.Parse(array2[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.height);
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 0)
					{
						uIImage.Enable = "true" == empty;
					}
					empty = xmlElement.GetAttribute("visible").Trim();
					if (empty.Length > 0)
					{
						uIImage.Visible = "true" == empty;
					}
					empty = xmlElement.GetAttribute("RGB").Trim();
					if (empty.Length > 0)
					{
						array2 = empty.Split(',');
						uIImage.SetColor(new Color(float.Parse(array2[0].Trim()) / 255f, float.Parse(array2[1].Trim()) / 255f, float.Parse(array2[2].Trim()) / 255f));
					}
					empty = xmlElement.GetAttribute("alpha").Trim();
					if (empty.Length > 0)
					{
						uIImage.SetAlpha(float.Parse(empty));
					}
					empty = xmlElement.GetAttribute("clip").Trim();
					if (empty.Length > 0)
					{
						array2 = empty.Split(',');
						uIImage.SetClip(new Rect(float.Parse(array2[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.x, float.Parse(array2[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.y, float.Parse(array2[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.width, float.Parse(array2[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect2.height));
					}
					empty = xmlElement.GetAttribute("rotation").Trim();
					if (empty.Length > 0)
					{
						uIImage.SetRotation(float.Parse(empty) * ((float)Math.PI / 180f));
					}
					xmlElement = (XmlElement)childNode2.SelectSingleNode("Cell");
					if (xmlElement != null)
					{
						empty = xmlElement.GetAttribute("name").Trim();
						D3DImageCell d3DImageCell5 = UIImageCellIndexer[empty];
						Rect texture_rect = D3DMain.Instance.ConvertRectAutoHD(d3DImageCell5.cell_rect);
						Vector2 size = new Vector2(uIImage.Rect.width, uIImage.Rect.height);
						uIImage.SetTexture(LoadUIMaterialAutoHD(d3DImageCell5.cell_texture), texture_rect, size);
					}
					ui_manager.Add(uIImage);
				}
				else if ("UIBigBoard" == childNode2.Name)
				{
					D3DBigBoardUI d3DBigBoardUI = new D3DBigBoardUI(ui_manager, this);
					empty = xmlElement.GetAttribute("rect").Trim();
					string[] array3 = empty.Split(',');
					Rect board_rect = new Rect(float.Parse(array3[0].Trim()), float.Parse(array3[1].Trim()), float.Parse(array3[2].Trim()), float.Parse(array3[3].Trim()));
					d3DBigBoardUI.CreateBigBoard(board_rect);
				}
				else if ("UIText" == childNode2.Name)
				{
					UIText uIText = new UIText();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIText);
					uIText.Id = cur_control_id;
					Rect rect3 = new Rect(0f, 0f, 0f, 0f);
					string[] array4;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect_hd").Trim();
						if (empty.Length > 0)
						{
							array4 = empty.Split(',');
							rect3 = new Rect(float.Parse(array4[0].Trim()), float.Parse(array4[1].Trim()), float.Parse(array4[2].Trim()), float.Parse(array4[3].Trim()));
						}
					}
					else
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array4 = empty.Split(',');
							rect3 = new Rect(float.Parse(array4[0].Trim()), float.Parse(array4[1].Trim()), float.Parse(array4[2].Trim()), float.Parse(array4[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array4 = empty.Split(',');
					uIText.Rect = new Rect(float.Parse(array4[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.x, float.Parse(array4[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.y, float.Parse(array4[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.width, float.Parse(array4[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect3.height);
					empty = xmlElement.GetAttribute("GameFont1").Trim();
					if (string.Empty != empty)
					{
						array4 = empty.Split(',');
						int num = ((D3DMain.Instance.HD_SIZE != 2) ? int.Parse(array4[0].Trim()) : int.Parse(array4[1].Trim()));
						uIText.SetFont(m_font_path + D3DMain.Instance.GameFont1.FontName + num);
						uIText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(num);
						uIText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(num);
					}
					else
					{
						empty = xmlElement.GetAttribute("GameFont2").Trim();
						if (string.Empty != empty)
						{
							array4 = empty.Split(',');
							int num2 = ((D3DMain.Instance.HD_SIZE != 2) ? int.Parse(array4[0].Trim()) : int.Parse(array4[1].Trim()));
							uIText.SetFont(m_font_path + D3DMain.Instance.GameFont2.FontName + num2);
							uIText.CharacterSpacing = D3DMain.Instance.GameFont2.GetCharSpacing(num2);
							uIText.LineSpacing = D3DMain.Instance.GameFont2.GetLineSpacing(num2);
						}
						else
						{
							empty = xmlElement.GetAttribute("chargap").Trim();
							if (empty.Length > 0)
							{
								uIText.CharacterSpacing = float.Parse(empty) * (float)D3DMain.Instance.HD_SIZE;
							}
							empty = xmlElement.GetAttribute("linegap").Trim();
							if (empty.Length > 0)
							{
								uIText.LineSpacing = float.Parse(empty) * (float)D3DMain.Instance.HD_SIZE;
							}
							uIText.SetFont(string.Concat(str1: (D3DMain.Instance.HD_SIZE != 2) ? xmlElement.GetAttribute("font").Trim() : xmlElement.GetAttribute("fontHD").Trim(), str0: m_font_path));
						}
					}
					empty = xmlElement.GetAttribute("autoline").Trim();
					if (empty.Length > 0)
					{
						uIText.AutoLine = "true" == empty;
					}
					empty = xmlElement.GetAttribute("align").Trim();
					if (empty.Length > 0)
					{
						uIText.AlignStyle = (UIText.enAlignStyle)(int)Enum.Parse(typeof(UIText.enAlignStyle), empty);
					}
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 0)
					{
						uIText.Enable = "true" == empty;
					}
					empty = xmlElement.GetAttribute("visible").Trim();
					if (empty.Length > 0)
					{
						uIText.Visible = "true" == empty;
					}
					empty = xmlElement.GetAttribute("color").Trim();
					if (empty.Length > 0)
					{
						array4 = empty.Split(',');
						uIText.SetColor(new Color(float.Parse(array4[0].Trim()) / 255f, float.Parse(array4[1].Trim()) / 255f, float.Parse(array4[2].Trim()) / 255f, float.Parse(array4[3].Trim()) / 255f));
					}
					uIText.SetText(childNode2.InnerText.Trim(' ', '\t', '\r', '\n'));
					ui_manager.Add(uIText);
				}
				else if ("UIBlock" == childNode2.Name)
				{
					UIBlock uIBlock = new UIBlock();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIBlock);
					uIBlock.Id = cur_control_id;
					Rect rect4 = new Rect(0f, 0f, 0f, 0f);
					string[] array5;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array5 = empty.Split(',');
							rect4 = new Rect(float.Parse(array5[0].Trim()), float.Parse(array5[1].Trim()), float.Parse(array5[2].Trim()), float.Parse(array5[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array5 = empty.Split(',');
					uIBlock.Rect = new Rect(float.Parse(array5[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.x, float.Parse(array5[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.y, float.Parse(array5[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.width, float.Parse(array5[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect4.height);
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 1)
					{
						uIBlock.Enable = "true" == empty;
					}
					ui_manager.Add(uIBlock);
				}
				else if ("UIMove" == childNode2.Name)
				{
					UIMove uIMove = new UIMove();
					empty = xmlElement.GetAttribute("name").Trim();
					m_control_table.Add(empty, uIMove);
					uIMove.Id = cur_control_id;
					Rect rect5 = new Rect(0f, 0f, 0f, 0f);
					string[] array6;
					if (Utils.IsRetina())
					{
						empty = xmlElement.GetAttribute("adjustrect").Trim();
						if (empty.Length > 0)
						{
							array6 = empty.Split(',');
							rect5 = new Rect(float.Parse(array6[0].Trim()), float.Parse(array6[1].Trim()), float.Parse(array6[2].Trim()), float.Parse(array6[3].Trim()));
						}
					}
					empty = xmlElement.GetAttribute("rect").Trim();
					array6 = empty.Split(',');
					uIMove.Rect = new Rect(float.Parse(array6[0].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.x, float.Parse(array6[1].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.y, float.Parse(array6[2].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.width, float.Parse(array6[3].Trim()) * (float)D3DMain.Instance.HD_SIZE + rect5.height);
					empty = xmlElement.GetAttribute("enable").Trim();
					if (empty.Length > 1)
					{
						uIMove.Enable = "true" == empty;
					}
					ui_manager.Add(uIMove);
				}
				cur_control_id++;
			}
		}
	}

	public void SwithLevel(int nLevelIndex)
	{
		D3DMain.Instance.LoadingScene = nLevelIndex;
		EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, PushLevel, false);
	}

	public void EnableUIFade(UIFade.FadeState fade_state, Color fade_color, UIFade.OnFadeComplete onFadeCompleteDelegate, bool destroy, bool full_screen_fade = true)
	{
		D3DMain.Instance.TriggerApplicationPause = false;
		GameObject gameObject = new GameObject("Manager_Fade");
		UIManager uIManager = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef.Add(uIManager);
		uIManager.SetParameter(15, (float)ui_index + 0.99f);
		uIManager.SetUIHandler(this);
		if (full_screen_fade)
		{
			uIManager.SetSpriteCameraViewPort(new Rect(0f - uIManager.ScreenOffset.x, 0f - uIManager.ScreenOffset.y, GameScreen.width, GameScreen.height));
		}
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward * (m_UIManagerRef.Count - 1) * 3f;
		UIImage uIImage = new UIImage();
		uIImage.SetTexture(LoadUIMaterialAutoHD("monolayer"), D3DMain.Instance.ConvertRectAutoHD(new Rect(0f, 0f, 8f, 8f)));
		if (full_screen_fade)
		{
			uIImage.SetTextureSize(new Vector2(GameScreen.width, GameScreen.height));
			uIImage.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		}
		else
		{
			uIImage.SetTextureSize(new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(0f, 0f, 480f, 320f));
		}
		m_UIManagerRef[m_UIManagerRef.Count - 1].Add(uIImage);
		ui_fade = gameObject.AddComponent<UIFade>();
		ui_fade.Init(uIImage, fade_state, fade_color, onFadeCompleteDelegate, destroy);
	}

	public void EnableUIFadeHold(Color fade_color, bool alpha_zero, bool full_screen_fade = true)
	{
		D3DMain.Instance.TriggerApplicationPause = false;
		GameObject gameObject = new GameObject("Manager_Fade");
		UIManager uIManager = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef.Add(uIManager);
		uIManager.SetParameter(15, (float)ui_index + 0.99f);
		uIManager.SetUIHandler(this);
		if (full_screen_fade)
		{
			uIManager.SetSpriteCameraViewPort(new Rect(0f - uIManager.ScreenOffset.x, 0f - uIManager.ScreenOffset.y, GameScreen.width, GameScreen.height));
		}
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward * (m_UIManagerRef.Count - 1) * 3f;
		UIImage uIImage = new UIImage();
		uIImage.SetTexture(LoadUIMaterialAutoHD("monolayer"), D3DMain.Instance.ConvertRectAutoHD(new Rect(0f, 0f, 8f, 8f)));
		if (full_screen_fade)
		{
			uIImage.SetTextureSize(new Vector2(GameScreen.width, GameScreen.height));
			uIImage.Rect = new Rect(0f, 0f, GameScreen.width, GameScreen.height);
		}
		else
		{
			uIImage.SetTextureSize(new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
			uIImage.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(0f, 0f, 480f, 320f));
		}
		m_UIManagerRef[m_UIManagerRef.Count - 1].Add(uIImage);
		ui_fade = gameObject.AddComponent<UIFade>();
		ui_fade.Init(uIImage, fade_color, alpha_zero);
	}

	public void StartUIFade(UIFade.FadeState fade_state, UIFade.OnFadeComplete onFadeCompleteDelegate, bool destroy)
	{
		D3DMain.Instance.TriggerApplicationPause = false;
		ui_fade.StartFade(fade_state, onFadeCompleteDelegate, destroy);
	}

	public void HideFade()
	{
		if (null != ui_fade)
		{
			ui_fade.Disable();
		}
	}

	public UIManager PushMessageBox(List<string> mgb_content, D3DMessageBox.MgbButton button_type, List<D3DMessageBoxButtonEvent.OnButtonClick> events, bool full_screen = false, Dictionary<int, Color> content_color = null)
	{
		GameObject gameObject = new GameObject("Manager_Mgb");
		UIManager uIManager = gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManagerRef.Add(uIManager);
		uIManager.SetParameter(15, (float)ui_index + 0.88f);
		uIManager.SetUIHandler(this);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward * (m_UIManagerRef.Count - 1) * 3f;
		D3DMessageBox d3DMessageBox = new D3DMessageBox(uIManager, this, full_screen);
		d3DMessageBox.ShowMessageBox(mgb_content, button_type, events, content_color);
		return uIManager;
	}

	public void Update()
	{
		if (!HandleEventValid())
		{
			return;
		}
		for (int num = m_UIManagerRef.Count - 1; num >= 0; num--)
		{
			if (null != m_UIManagerRef[num] && m_UIManagerRef[num].EnableUIHandler && m_UIManagerRef[num].gameObject.active)
			{
				UITouchInner[] array = iPhoneInputMgr.MockTouches();
				foreach (UITouchInner touch in array)
				{
					if (m_UIManagerRef[num].HandleInput(touch))
					{
						HitUI = true;
						return;
					}
				}
			}
		}
		HitUI = false;
	}

	public bool HandleEventValid()
	{
		if (isFadeing || ui_index != D3DMain.Instance.D3DUIList.Count)
		{
			return false;
		}
		return true;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		OnHandleEvent(control, command, wparam, lparam);
	}

	public virtual void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public Material LoadUIMaterialAutoHD(string name)
	{
		string text = string.Empty;
		if (D3DMain.Instance.HD_SIZE == 2)
		{
			text = "_hd";
		}
		string path = m_ui_material_path + name + text + "_M";
		Material material = Resources.Load(path) as Material;
		if (material == null)
		{
		}
		return material;
	}

	public string LoadFontAutoHD(string font_name, int font_size)
	{
		if (D3DMain.Instance.HD_SIZE == 2)
		{
			font_size *= 2;
		}
		return m_font_path + font_name + font_size;
	}

	public string LoadFont(string font_name, int size, int hd_size)
	{
		if (D3DMain.Instance.HD_SIZE == 2)
		{
			return m_font_path + font_name + hd_size;
		}
		return m_font_path + font_name + size;
	}

	public void SwitchLevelByLoading()
	{
		Application.LoadLevel(5);
	}

	public void SwitchLevelImmediately()
	{
		D3DMain.Instance.CurrentScene = D3DMain.Instance.LoadingScene;
		Application.LoadLevel(D3DMain.Instance.LoadingScene);
	}

	protected virtual void PushLevel()
	{
		Application.LoadLevelAdditive(D3DMain.Instance.LoadingScene);
	}

	public virtual void FreezeTimeScale()
	{
		Time.timeScale = 1f;
	}

	public void RemoveUIManager(UIManager manager)
	{
		if (m_UIManagerRef.Contains(manager))
		{
			m_UIManagerRef.Remove(manager);
		}
	}
}
