using System.Collections;
using System.Collections.Generic;

public class D3DSkillBasic
{
	public string skill_id;

	public string skill_name;

	public string skill_icon;

	public int max_level;

	protected Hashtable replace_tags;

	public List<string> description;

	public D3DSkillBasic()
	{
		skill_id = string.Empty;
		skill_name = string.Empty;
		skill_icon = string.Empty;
		max_level = 1;
		replace_tags = new Hashtable();
		description = new List<string>();
	}

	~D3DSkillBasic()
	{
	}

	public List<string> GetSkillDescription(int skill_level)
	{
		List<string> list = new List<string>();
		foreach (string item in description)
		{
			string content = item;
			while (!DisposalTagsInContent(ref content, skill_level))
			{
			}
			if (string.Empty != content)
			{
				list.Add(content);
			}
		}
		return list;
	}

	private bool DisposalTagsInContent(ref string content, int skill_level)
	{
		bool result = true;
		int num = content.IndexOf("<#Replace");
		if (num != -1)
		{
			result = false;
			int num2 = content.IndexOf("#>", num);
			string text = content.Substring(num, num2 - num + 2);
			num = text.IndexOf('(');
			num2 = text.IndexOf(')');
			string key = text.Substring(num + 1, num2 - num - 1);
			if (replace_tags.ContainsKey(key))
			{
				if (replace_tags[key] is List<float>)
				{
					List<float> list = (List<float>)replace_tags[key];
					if (skill_level > list.Count - 1)
					{
						skill_level = list.Count - 1;
					}
					content = content.Replace(text, list[skill_level].ToString());
				}
				else if (replace_tags[key] is List<int>)
				{
					List<int> list2 = (List<int>)replace_tags[key];
					if (skill_level > list2.Count - 1)
					{
						skill_level = list2.Count - 1;
					}
					content = content.Replace(text, list2[skill_level].ToString());
				}
				else if (replace_tags[key] is List<string>)
				{
					List<string> list3 = (List<string>)replace_tags[key];
					if (skill_level > list3.Count - 1)
					{
						skill_level = list3.Count - 1;
					}
					content = content.Replace(text, list3[skill_level].ToString());
				}
				else
				{
					content = content.Replace(text, string.Empty);
				}
			}
			else
			{
				content = content.Replace(text, string.Empty);
			}
		}
		num = content.IndexOf("<#Percent");
		if (num != -1)
		{
			result = false;
			int num2 = content.IndexOf("#>", num);
			string text = content.Substring(num, num2 - num + 2);
			num = text.IndexOf('(');
			num2 = text.IndexOf(')');
			string key = text.Substring(num + 1, num2 - num - 1);
			if (replace_tags.ContainsKey(key))
			{
				if (replace_tags[key] is List<float>)
				{
					List<float> list4 = (List<float>)replace_tags[key];
					if (skill_level > list4.Count - 1)
					{
						skill_level = list4.Count - 1;
					}
					float num3 = list4[skill_level] * 100f;
					if (num3 == 0f)
					{
						content = content.Replace(text, num3 + "%");
					}
					else
					{
						content = content.Replace(text, num3.ToString("0.00").Trim('0').Trim('.') + "%");
					}
				}
				else if (replace_tags[key] is List<int>)
				{
					List<int> list5 = (List<int>)replace_tags[key];
					if (skill_level > list5.Count - 1)
					{
						skill_level = list5.Count - 1;
					}
					float num3 = (float)list5[skill_level] * 100f;
					if (num3 == 0f)
					{
						content = content.Replace(text, num3 + "%");
					}
					else
					{
						content = content.Replace(text, num3.ToString("0.00").Trim('0').Trim('.') + "%");
					}
				}
				else
				{
					content = content.Replace(text, string.Empty);
				}
			}
			else
			{
				content = content.Replace(text, string.Empty);
			}
		}
		num = content.IndexOf("<#Sum");
		if (num != -1)
		{
			result = false;
			int num2 = content.IndexOf("#>", num);
			string text = content.Substring(num, num2 - num + 2);
			num = text.IndexOf('(');
			num2 = text.IndexOf(')');
			string key = text.Substring(num + 1, num2 - num - 1);
			string[] array = key.Split(',');
			float num4 = 0f;
			string[] array2 = array;
			foreach (string key2 in array2)
			{
				if (!replace_tags.ContainsKey(key2))
				{
					continue;
				}
				if (replace_tags[key2] is List<float>)
				{
					List<float> list6 = (List<float>)replace_tags[key2];
					if (skill_level > list6.Count - 1)
					{
						skill_level = list6.Count - 1;
					}
					num4 += list6[skill_level];
				}
				else if (replace_tags[key2] is List<int>)
				{
					List<int> list7 = (List<int>)replace_tags[key2];
					if (skill_level > list7.Count - 1)
					{
						skill_level = list7.Count - 1;
					}
					num4 += (float)list7[skill_level];
				}
			}
			if (num4 == 0f)
			{
				content = content.Replace(text, num4 + "%");
			}
			else
			{
				content = content.Replace(text, num4.ToString("0.00").Trim('0').Trim('.'));
			}
		}
		num = content.IndexOf("<#Product");
		if (num != -1)
		{
			result = false;
			int num2 = content.IndexOf("#>", num);
			string text = content.Substring(num, num2 - num + 2);
			num = text.IndexOf('(');
			num2 = text.IndexOf(')');
			string key = text.Substring(num + 1, num2 - num - 1);
			string[] array3 = key.Split(',');
			float num5 = 1f;
			string[] array4 = array3;
			foreach (string key3 in array4)
			{
				if (!replace_tags.ContainsKey(key3))
				{
					continue;
				}
				if (replace_tags[key3] is List<float>)
				{
					List<float> list8 = (List<float>)replace_tags[key3];
					if (skill_level > list8.Count - 1)
					{
						skill_level = list8.Count - 1;
					}
					num5 *= list8[skill_level];
				}
				else if (replace_tags[key3] is List<int>)
				{
					List<int> list9 = (List<int>)replace_tags[key3];
					if (skill_level > list9.Count - 1)
					{
						skill_level = list9.Count - 1;
					}
					num5 *= (float)list9[skill_level];
				}
			}
			if (num5 == 0f)
			{
				content = content.Replace(text, num5.ToString());
			}
			else
			{
				content = content.Replace(text, num5.ToString("0.00").Trim('0').Trim('.'));
			}
		}
		num = content.IndexOf("<#IDGetName");
		if (num != -1)
		{
			result = false;
			int num2 = content.IndexOf("#>", num);
			string text = content.Substring(num, num2 - num + 2);
			num = text.IndexOf('(');
			num2 = text.IndexOf(')');
			string key = text.Substring(num + 1, num2 - num - 1);
			if (replace_tags.ContainsKey(key))
			{
				if (replace_tags[key] is List<string>)
				{
					List<string> list10 = (List<string>)replace_tags[key];
					if (skill_level > list10.Count - 1)
					{
						skill_level = list10.Count - 1;
					}
					string profile_id = list10[skill_level];
					D3DPuppetProfile profile = D3DMain.Instance.GetProfile(profile_id);
					if (profile != null)
					{
						content = content.Replace(text, profile.profile_name);
					}
					else
					{
						content = content.Replace(text, string.Empty);
					}
				}
				else
				{
					content = content.Replace(text, string.Empty);
				}
			}
			else
			{
				content = content.Replace(text, string.Empty);
			}
		}
		return result;
	}

	public virtual void CreateReplaceTags()
	{
	}
}
