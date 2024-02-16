using UnityEngine;

public class ArenaBattle : MonoBehaviour
{
	protected SceneArena scene_arena;

	protected void Awake()
	{
		scene_arena = GetComponent<SceneArena>();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	protected virtual void PlayBattleBGM()
	{
	}
}
