using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class D3DTutorialHintCfg
{
	public class DamaoHintData
	{
		public enum HintCondition
		{
			None = 0,
			Start = 1,
			Spawn = 2,
			End = 3,
			NewHeroComin = 4
		}

		public HintCondition _condition;

		public int _nEnemyWave;

		public int _damaoType;

		public List<string> _nHintStrings = new List<string>();
	}

	private static List<DamaoHintData> _configDatas = new List<DamaoHintData>();

	public static DamaoHintData NeedsToShowDamao(DamaoHintData.HintCondition condition, int nWave)
	{
		foreach (DamaoHintData configData in _configDatas)
		{
			if (configData._condition == DamaoHintData.HintCondition.Spawn)
			{
				if (nWave == configData._nEnemyWave)
				{
					return configData;
				}
			}
			else if (condition == configData._condition)
			{
				return configData;
			}
		}
		return null;
	}

	public static void LoadConfig(string strPath)
	{
		TextAsset textAsset = Resources.Load(strPath) as TextAsset;
		if (null == textAsset)
		{
			return;
		}
		string text = textAsset.text;
		text = XXTEAUtils.Decrypt(text, D3DGamer.Instance.Sk[2]);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode item in documentElement)
		{
			DamaoHintData damaoHintData = new DamaoHintData();
			XmlElement xmlElement = (XmlElement)item;
			if ("my:hint" == xmlElement.Name)
			{
				damaoHintData._condition = (DamaoHintData.HintCondition)int.Parse(xmlElement.GetAttribute("my:triggertype").Trim());
				damaoHintData._nEnemyWave = int.Parse(xmlElement.GetAttribute("my:enemywave").Trim());
				damaoHintData._damaoType = int.Parse(xmlElement.GetAttribute("my:damaotype").Trim());
				List<string> list = new List<string>();
				foreach (XmlNode item2 in item)
				{
					if (item2.Name == "my:hintstrings")
					{
						damaoHintData._nHintStrings.Add(item2.InnerText);
					}
				}
			}
			_configDatas.Add(damaoHintData);
		}
	}
}
