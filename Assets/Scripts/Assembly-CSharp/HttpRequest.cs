using System.Collections;
using UnityEngine;

public class HttpRequest : MonoBehaviour
{
	public delegate void OnDownloadFinished(WWW www);

	private static HttpRequest _instance;

	private OnDownloadFinished _onDownloadFinished;

	public static HttpRequest Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject("HttpRequest").AddComponent<HttpRequest>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void RequestURL(string strUrl, OnDownloadFinished onDownloadFinished)
	{
		_onDownloadFinished = onDownloadFinished;
		StartCoroutine(DoRequest(strUrl));
	}

	private IEnumerator DoRequest(string strUrl)
	{
		WWW www = new WWW(strUrl);
		yield return www;
		_onDownloadFinished(www);
		DestoryMyself();
	}

	private void DestoryMyself()
	{
		_instance = null;
		Object.Destroy(base.gameObject);
	}
}
