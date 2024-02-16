using System.Collections;
using LitJson;
using UnityEngine;

public class CommunicationRegister : MonoBehaviour
{
	public OnResponse_Register callback;

	public OnRequestTimeout_Register callback_error;

	private static CommunicationRegister instance;

	private string url = "http://account.trinitigame.com:8081/gameapi/turboPlatform2.do?action=Login&json={}";

	protected string deviceid = string.Empty;

	protected string nickname = string.Empty;

	protected string gamename = "TLHE";

	public static CommunicationRegister Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("CommunicationRegister").AddComponent<CommunicationRegister>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void SentData()
	{
		deviceid = MiscPlugin.GetMacAddr();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("deviceid", deviceid);
		hashtable.Add("nickname", nickname);
		hashtable.Add("gamename", gamename);
		string request = JsonMapper.ToJson(hashtable);
		HttpManager.Instance().SendRequest(url, request, null, 15f, OnResponse, OnRequestTimeout);
	}

	private void OnResponse(int task_id, string param, int code, string response)
	{
		Debug.LogWarning("OnResponse code : " + code + " &  param : " + param);
		if (callback != null)
		{
			callback();
			callback = null;
		}
		Object.Destroy(base.gameObject);
	}

	private void OnRequestTimeout(int task_id, string param)
	{
		Debug.LogWarning("OnRequestTimeout");
		if (callback_error != null)
		{
			callback_error();
			callback_error = null;
		}
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}
}
