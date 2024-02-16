using UnityEngine;

public class AudioTest : MonoBehaviour
{
	public AudioSource audio_source;

	public GameObject audio_prefab;

	public GameObject audio_prefab2;

	private GameObject clone_prefab;

	private GameObject clone_prefab2;

	private void Start()
	{
		clone_prefab = Object.Instantiate(audio_prefab) as GameObject;
		clone_prefab2 = Object.Instantiate(audio_prefab2) as GameObject;
	}

	private void Update()
	{
		if (Input.GetKeyDown("t"))
		{
			AudioSource audioSource = Object.Instantiate(audio_source) as AudioSource;
			audioSource.PlayOneShot(audioSource.clip);
			Object.Destroy(audioSource.gameObject, audioSource.clip.length / audio_source.pitch);
		}
		if (Input.GetKeyDown("r"))
		{
			D3DAudioBehaviour d3DAudioBehaviour = clone_prefab.AddComponent<D3DAudioBehaviour>();
			d3DAudioBehaviour.Init(null, false, false);
			d3DAudioBehaviour = clone_prefab2.AddComponent<D3DAudioBehaviour>();
			d3DAudioBehaviour.Init(null, false, false);
		}
	}
}
