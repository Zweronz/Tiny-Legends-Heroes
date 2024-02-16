using UnityEngine;

public class D3DAudioBehaviour : MonoBehaviour
{
	private ITAudioEvent[] events;

	private bool have_host;

	private GameObject audio_host;

	private bool bind_host;

	private bool sync_host;

	private bool auto_destroy = true;

	private void Start()
	{
	}

	public void Init(GameObject host, bool bind, bool sync, bool auto_destroy = true)
	{
		Component[] components = GetComponents(typeof(ITAudioEvent));
		if (components.Length == 0)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		events = new ITAudioEvent[components.Length];
		for (int i = 0; i < components.Length; i++)
		{
			events[i] = (ITAudioEvent)components[i];
			events[i].Trigger();
		}
		audio_host = host;
		if (null != audio_host)
		{
			have_host = true;
			if (bind)
			{
				base.transform.parent = audio_host.transform;
				bind_host = true;
			}
			else if (sync)
			{
				sync_host = true;
			}
		}
		this.auto_destroy = auto_destroy;
	}

	public void Stop()
	{
		auto_destroy = true;
		for (int i = 0; i < events.Length; i++)
		{
			if (events[i].isLoop)
			{
				TAudioAuxFade component = events[i].GetComponent<TAudioAuxFade>();
				if (null == component)
				{
					events[i].Stop();
				}
				else
				{
					component.StartCoroutine(component.FadeOut(events[i].Stop));
				}
			}
		}
	}

	private void Update()
	{
		if (have_host)
		{
			if (null != audio_host)
			{
				if (!bind_host && sync_host)
				{
					base.transform.position = audio_host.transform.position;
				}
			}
			else
			{
				have_host = false;
				if (auto_destroy)
				{
					Stop();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			for (int i = 0; i < events.Length; i++)
			{
				events[i].Stop();
			}
		}
		if (!auto_destroy)
		{
			return;
		}
		for (int j = 0; j < events.Length; j++)
		{
			if (events[j].isPlaying)
			{
				return;
			}
		}
		Object.Destroy(base.gameObject);
	}
}
