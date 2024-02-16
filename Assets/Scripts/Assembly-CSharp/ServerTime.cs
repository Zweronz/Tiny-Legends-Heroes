using System;
using LitJson;
using UnityEngine;

public class ServerTime
{
	private const string _strUrl = "http://97.74.205.45";

	private const int _nPort = 7600;

	private const string _strAppend = "/gameapi/GameCommonNo.do?action=groovy&json={\"cmd\":\"GetServerTime\"}";

	private const int _nRetryCountCfg = 5;

	private static int _nRetryCountLeft;

	private static float _fTimeGetServerTime;

	private static DateTime _dtServerTime;

	private static string strUrlReqest
	{
		get
		{
			return "http://97.74.205.45:" + 7600 + "/gameapi/GameCommonNo.do?action=groovy&json={\"cmd\":\"GetServerTime\"}";
		}
	}

	public static DateTime Now
	{
		get
		{
			if (_fTimeGetServerTime > 0f)
			{
				float num = Time.realtimeSinceStartup - _fTimeGetServerTime;
				return _dtServerTime.AddSeconds(num);
			}
			Debug.LogError("Server Now failed.Not sync with server! Please call function Init() first!");
			return DateTime.MinValue;
		}
	}

	public static bool InvalidTime
	{
		get
		{
			return Now == DateTime.MinValue;
		}
	}

	public static void Init()
	{
		_nRetryCountLeft = 5;
		HttpRequest.Instance.RequestURL(strUrlReqest, onDownloadFinished);
	}

	private static void onDownloadFinished(WWW www)
	{
		int num = ((www.error != null) ? ProcessFailed() : ProcessSuccess(www));
	}

	private static int ProcessFailed()
	{
		if (--_nRetryCountLeft > 0)
		{
			Debug.LogError("retry to get server time");
			HttpRequest.Instance.RequestURL(strUrlReqest, onDownloadFinished);
		}
		else
		{
			Debug.LogError("Get server time failed!!");
		}
		return 0;
	}

	private static int ProcessSuccess(WWW www)
	{
		string text = www.text;
		_fTimeGetServerTime = Time.realtimeSinceStartup;
		_dtServerTime = DateTime.Parse((string)JsonMapper.ToObject(text)["datetime"]);
		return 0;
	}
}
