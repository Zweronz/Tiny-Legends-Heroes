using System;
using System.Xml;

public class DailyGift
{
	protected class DaliyGiftSaveData
	{
		public string strGetRewardTime;

		public int nContiunousRewardCount;

		public override string ToString()
		{
			return "DaliyPresentSaveData   RewardTime=" + strGetRewardTime + " count=" + nContiunousRewardCount;
		}
	}

	private class DaliyGiftSave
	{
		private const string s_savedFileName = "DailyPresentData.tlh";

		private const string s_strLastGetRewardTime = "LastGetRewardTime";

		private const string s_strContinousRewarCount = "Count";

		private const string s_strKey = "Dyqanb&yDjtfr2d8";

		public void SaveTimeToFile(string strGettRewardTime, int nContinousRewardCount)
		{
			string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
			text += GetXmlFileString(strGettRewardTime, nContinousRewardCount);
			Utils.FileSaveString("DailyPresentData.tlh", XXTEAUtils.Encrypt(text, "Dyqanb&yDjtfr2d8"));
		}

		public string GetXmlFileString(string strGettRewardTime, int nContinousRewardCount)
		{
			string text = "<DailyPresentData LastGetRewardTime=\"" + strGettRewardTime + "\" Count=\"" + nContinousRewardCount + "\"";
			return text + " />\n";
		}

		public bool IsSavedFileExist()
		{
			return Utils.CheckFileExists("DailyPresentData.tlh");
		}

		public DaliyGiftSaveData GetSavedTime()
		{
			if (IsSavedFileExist())
			{
				string text = Utils.SavePath() + "/DailyPresentData.tlh";
				DaliyGiftSaveData daliyGiftSaveData = new DaliyGiftSaveData();
				string content = string.Empty;
				Utils.FileGetString("DailyPresentData.tlh", ref content);
				content = XXTEAUtils.Decrypt(content, "Dyqanb&yDjtfr2d8");
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(content);
				XmlElement documentElement = xmlDocument.DocumentElement;
				daliyGiftSaveData.strGetRewardTime = documentElement.GetAttribute("LastGetRewardTime");
				daliyGiftSaveData.nContiunousRewardCount = Convert.ToInt32(documentElement.GetAttribute("Count"));
				return daliyGiftSaveData;
			}
			return null;
		}
	}

	public enum EDailyGiftStat
	{
		ServerTimeUnavailable = 0,
		NotAvailable = 1,
		Available = 2
	}

	private const float _fVersionAdded = 1.2f;

	private static DailyGift _dailyPresent = null;

	private static readonly int _nDeltaRewardDay = 1;

	private static readonly int _nContinousIncreaseDay = 1;

	private static DaliyGiftSave _dailyPresentSave = new DaliyGiftSave();

	private static DaliyGiftSaveData _savedData = null;

	public static DailyGift Instance
	{
		get
		{
			if (_dailyPresent == null)
			{
				_dailyPresent = new DailyGift();
				_dailyPresent.InitSaveFile();
				_dailyPresentSave.SaveTimeToFile("2013-3-8 02:12:12", 1);
				_savedData = _dailyPresentSave.GetSavedTime();
				EDailyGiftStat eDailyGiftStat = _dailyPresent.ShouldGetTodayReward();
				int continousRewardCount = _dailyPresent.GetContinousRewardCount();
			}
			return _dailyPresent;
		}
	}

	private void InitSaveFile()
	{
		DateTime dateTime = ServerTime.Now.AddDays(-1.0);
		if (!IsVersionAddedFunc() && !ServerTime.InvalidTime)
		{
			DateTime dateTime2 = ServerTime.Now.AddDays(-1.0);
			_dailyPresentSave.SaveTimeToFile(dateTime2.ToString(), 0);
		}
		else if (_dailyPresentSave.IsSavedFileExist())
		{
			_savedData = _dailyPresentSave.GetSavedTime();
		}
		else
		{
			_dailyPresentSave.SaveTimeToFile(ServerTime.Now.ToString(), 0);
		}
	}

	private bool IsVersionAddedFunc()
	{
		return D3DGamer.Instance.SaveFileVersion >= 1.2f;
	}

	public EDailyGiftStat ShouldGetTodayReward()
	{
		if (_savedData == null)
		{
			return EDailyGiftStat.Available;
		}
		if (ServerTime.InvalidTime)
		{
			return EDailyGiftStat.ServerTimeUnavailable;
		}
		if (GetDeltaDayFromLastReward() >= _nDeltaRewardDay)
		{
			return EDailyGiftStat.Available;
		}
		return EDailyGiftStat.NotAvailable;
	}

	public int GetContinousRewardCount()
	{
		if (_savedData == null)
		{
			return 0;
		}
		int deltaDayFromLastReward = GetDeltaDayFromLastReward();
		if (GetDeltaDayFromLastReward() <= _nContinousIncreaseDay)
		{
			return _savedData.nContiunousRewardCount + 1;
		}
		return 0;
	}

	private int GetDeltaDayFromLastReward()
	{
		DateTime dtLastGetReward = Convert.ToDateTime(_savedData.strGetRewardTime);
		return GetDeltaDayFromLastReward(ServerTime.Now, dtLastGetReward);
	}

	private int GetDeltaDayFromLastReward(DateTime dtNow, DateTime dtLastGetReward)
	{
		DateTime date = dtNow.Date;
		DateTime date2 = dtLastGetReward.Date;
		return (dtNow.Date - dtLastGetReward.Date).Days;
	}
}
